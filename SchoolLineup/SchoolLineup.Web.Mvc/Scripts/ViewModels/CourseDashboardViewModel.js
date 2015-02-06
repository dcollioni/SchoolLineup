function Student(data) {
    var self = this;

    self.id = ko.observable(data.id);
    self.name = ko.observable(data.name);
    self.email = ko.observable(data.email);

    self.display = ko.computed(function () {
        if (self.name() !== self.email()) {
            return self.name();// + ' (' + self.email() + ')';
        }

        return self.name();
    });
}

function PartialGrade(data) {
    var self = this;

    self.id = ko.observable(data.id);
    self.name = ko.observable(data.name);
    self.order = ko.observable(data.order);
    self.courseId = ko.observable(data.courseId);

    self.isSelected = ko.observable(false);
}

function Exam(data) {
    var self = this;

    self.id = ko.observable(data.id);
    self.name = ko.observable(data.name);
    self.date = ko.observable(data.date);
    self.value = ko.observable(data.value);
    self.description = ko.observable(data.description);
    self.partialGradeId = ko.observable(data.partialGradeId);

    self.dateStr = ko.computed({
        read: function () {
            return SL.formatters.date(self.date());
        },
        write: function (value) {
            self.date(SL.date.getFromString(value));
        },
        owner: self
    });

    self.isSelected = ko.observable(false);
}

PartialGrade.prototype.clone = function () {
    return new PartialGrade(ko.toJS(this));
};

Exam.prototype.clone = function () {
    return new Exam(ko.toJS(this));
};

function CourseDashboardViewModel() {
    var self = this;

    self.courseId = ko.observable($('#courseId').val());
    self.studentQuery = ko.observable('');
    self.studentQueryHasFocus = ko.observable(false);
    self.studentResultsHover = ko.observable(false);
    self.studentResults = ko.observableArray([]);
    self.selectedStudentId = ko.observable(0);
    self.preventSearch = false;
    self.students = ko.observableArray([]);
    self.studentsCount = ko.computed(function () {
        return '(' + self.students().length + ')';
    });

    self.hasStudentQueryResults = ko.computed(function () {
        return (self.studentQueryHasFocus() || self.studentResultsHover()) && self.studentResults().length > 0;
    });

    self.studentQuery.subscribe(function (newValue) {
        self.preventSearch = false;
        self.selectedStudentId(0);
        delay(function () {
            self.searchStudents();
        }, 300);
    });

    self.searchStudentRequest;

    self.searchStudents = function () {
        if (!self.preventSearch) {
            self.studentResults.removeAll();

            if (!!self.searchStudentRequest) {
                self.searchStudentRequest.abort();
            }

            if (!!self.studentQuery()) {
                self.searchStudentRequest =
                    $.ajax({
                        url: SL.root + 'Student/Search/?query=' + self.studentQuery(),
                        dataType: 'json',
                        success: function (response) {

                            $.each(response, function (i, e) {
                                var student = new Student({
                                    id: e.Id,
                                    name: e.Name,
                                    email: e.Email
                                });

                                self.studentResults.push(student);
                            });

                            self.studentResultsHover(false);
                        }
                    });
            }
        }
    };

    self.hoverStudentResults = function (hover) {
        self.studentResultsHover(hover);
    };

    self.selectStudent = function (student) {
        self.studentResults.removeAll();
        self.studentQuery(student.display());
        self.selectedStudentId(student.id());
        self.preventSearch = true;
    };

    self.loadStudents = function () {
        SL.mask(true);

        $.ajax({
            url: SL.root + 'Course/GetAllStudents/?courseId=' + self.courseId(),
            dataType: 'json',
            success: function (response) {

                self.students.removeAll();

                $.each(response, function (i, e) {
                    var student = new Student({
                        id: e.Id,
                        name: e.Name,
                        email: e.Email
                    });

                    self.students.push(student);
                });

                SL.unmask();
            }
        });
    };

    self.addStudent = function () {
        if (!!self.selectedStudentId()) {
            SL.mask(true);
            $.ajax({
                url: SL.root + 'Course/AddStudent',
                data: {
                    courseId: self.courseId(),
                    studentId: self.selectedStudentId()
                },
                dataType: 'json',
                success: function (response) {
                    if (response.Success) {
                        self.studentQuery('');
                        self.loadStudents();
                    }
                }
            });
        }
    };

    self.removeStudent = function (student) {
        SL.mask(true);
        $.ajax({
            url: SL.root + 'Course/RemoveStudent',
            data: {
                courseId: self.courseId(),
                studentId: student.id()
            },
            dataType: 'json',
            success: function (response) {
                if (response.Success) {
                    self.loadStudents();
                }
            }
        });
    };

    self.isStudentFieldsetOpen = ko.observable(false);

    self.toggleStudentFieldset = function () {
        var current = self.isStudentFieldsetOpen();
        self.isStudentFieldsetOpen(!current);
    };

    self.isPartialGradeFieldsetOpen = ko.observable(true);

    self.togglePartialGradeFieldset = function () {
        var current = self.isPartialGradeFieldsetOpen();
        self.isPartialGradeFieldsetOpen(!current);
    };

    self.loadStudents();

    // -- Partial Grade context
    self.partialGradeContext = {

        models: ko.observableArray([]),
        current: ko.observable(new PartialGrade({})),
        serverErrors: ko.observableArray([]),
        errors: ko.observable({}),
        isDeleting: ko.observable(false),

        select: function (pg, model) {
            if (!!pg.current() && pg.current().id() === model.id()) {
                return;
            }

            pg.current(model.clone());
            pg.deselectAll();
            model.isSelected(true);

            pg.examContext.clearAll();
            pg.examContext.load();
        },

        clearSelection: function () {
            var pg = this;
            pg.current(new PartialGrade({}));
            pg.errors({});
            pg.deselectAll();

            pg.examContext.clearAll();
        },

        clearServerErrors: function () {
            var pg = this;
            pg.serverErrors([]);
            SL.unmask();
        },

        deselectAll: function () {
            var pg = this;
            $.each(pg.models(), function (i, item) {
                item.isSelected(false);
            });
        },

        getSelected: function () {
            var pg = this;
            return _.find(pg.models(), function (model) {
                return model.isSelected();
            });
        },

        create: function () {
            var pg = this;
            if (pg.isValid()) {
                var newModel = pg.current().clone();
                newModel.id(0);
                newModel.courseId(self.courseId());

                var jsonData = ko.toJSON(newModel);
                pg.save(jsonData);
            }
        },

        update: function () {
            var pg = this;
            if (pg.isValid()) {
                var jsonData = ko.toJSON(pg.current);

                pg.save(jsonData);
            }
        },

        isValid: function () {
            var pg = this;
            var brokenRules = [];

            if (!pg.current().name()) {
                brokenRules['name'] = 'Esse campo deve ser preenchido.';
            }
            if (!pg.current().order()) {
                brokenRules['order'] = 'Esse campo deve ser preenchido.';
            }

            pg.errors(brokenRules);

            return !pg.errors().name && !pg.errors().order;
        },

        save: function (jsonData) {
            var pg = this;
            var isNew = JSON.parse(jsonData).id === 0;

            SL.mask(true);

            $.ajax({
                url: SL.root + 'PartialGrade/Save',
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8;",
                data: jsonData,
                success: function (response) {
                    if (response.Success) {
                        var model;

                        if (isNew) {
                            model = pg.current().clone();
                        }
                        else {
                            model = pg.getSelected();
                        }

                        model.id(response.Data.Id);
                        model.name(response.Data.Name);
                        model.order(response.Data.Order);
                        model.courseId(response.Data.CourseId);

                        if (isNew) {
                            pg.models.unshift(model);
                        }

                        pg.select(pg, model);

                        SL.unmask();
                    }
                    else {
                        SL.hideModals();

                        $.each(response.Messages, function (i, message) {
                            pg.serverErrors.push(message.MemberNames[0] + ': ' + message.ErrorMessage);
                        });

                        SL.setModalPosition();
                    }
                }
            });
        },

        destroy: function () {
            var pg = this;
            pg.isDeleting(true);
            SL.mask();
            SL.setModalPosition();
        },

        confirmDelete: function () {
            var pg = this;
            pg.isDeleting(false);
            SL.mask(true);

            $.post(SL.root + 'PartialGrade/Delete', { id: pg.current().id() }, function (response) {
                if (response.Success) {
                    var selectedModel = pg.getSelected();
                    pg.models.remove(selectedModel);
                    pg.clearSelection();

                    SL.unmask();
                }
                else {
                    SL.hideModals();

                    $.each(response.Messages, function (i, message) {
                        var memberName = !!message.MemberNames[0] ? message.MemberNames[0] + ': ' : '';
                        pg.serverErrors.push(memberName + message.ErrorMessage);
                    });

                    SL.setModalPosition();
                }
            });
        },

        cancelDelete: function () {
            var pg = this;
            pg.isDeleting(false);
            SL.unmask();
        },

        load: function () {
            var pg = this;

            SL.mask(true);

            $.ajax({
                url: SL.root + 'PartialGrade/GetAll/?courseId=' + self.courseId(),
                dataType: 'json',
                complete: function () {
                    SL.unmask();
                },
                success: function (response) {

                    $.each(response, function (i, e) {
                        var model = new PartialGrade({
                            id: e.Id,
                            name: e.Name,
                            order: e.Order,
                            courseId: e.CourseId
                        });

                        pg.models.push(model);
                    });
                },
                error: function () {
                }
            });
        },

        isExamFieldsetOpen: ko.observable(true),

        toggleExamFieldset: function () {
            var pg = this;

            var current = pg.isExamFieldsetOpen();
            pg.isExamFieldsetOpen(!current);
        },

        // -- Exam context
        examContext: {

            models: ko.observableArray([]),
            current: ko.observable(new Exam({})),
            serverErrors: ko.observableArray([]),
            errors: ko.observable({}),
            isDeleting: ko.observable(false),

            select: function (ex, model) {
                ex.current(model.clone());
                ex.deselectAll();
                model.isSelected(true);
            },

            clearSelection: function () {
                var ex = this;
                ex.current(new Exam({}));
                ex.errors({});
                ex.deselectAll();
            },

            clearServerErrors: function () {
                var ex = this;
                ex.serverErrors([]);
                SL.unmask();
            },

            deselectAll: function () {
                var ex = this;
                $.each(ex.models(), function (i, item) {
                    item.isSelected(false);
                });
            },

            getSelected: function () {
                var ex = this;
                return _.find(ex.models(), function (model) {
                    return model.isSelected();
                });
            },

            clearAll: function () {
                var ex = this;
                ex.clearSelection();
                ex.models.removeAll();
            },

            create: function () {
                var ex = this;
                if (ex.isValid()) {
                    var newModel = ex.current().clone();
                    newModel.id(0);
                    newModel.partialGradeId(self.partialGradeContext.current().id());

                    var jsonData = ko.toJSON(newModel);
                    ex.save(jsonData);
                }
            },

            update: function () {
                var ex = this;
                if (ex.isValid()) {
                    var jsonData = ko.toJSON(ex.current);

                    ex.save(jsonData);
                }
            },

            isValid: function () {
                var ex = this;
                var brokenRules = [];

                if (!ex.current().name()) {
                    brokenRules['name'] = 'Esse campo deve ser preenchido.';
                }
                if (!ex.current().date()) {
                    brokenRules['date'] = 'Esse campo deve ser preenchido.';
                }
                if (!ex.current().value()) {
                    brokenRules['value'] = 'Esse campo deve ser preenchido.';
                }
                else if (isNaN(ex.current().value())) {
                    brokenRules['value'] = 'Esse campo deve ser um valor numérico.';
                }

                ex.errors(brokenRules);

                return !ex.errors().name && !ex.errors().date && !ex.errors().value;
            },

            save: function (jsonData) {
                var ex = this;
                var isNew = JSON.parse(jsonData).id === 0;

                SL.mask(true);

                $.ajax({
                    url: SL.root + 'Exam/Save',
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8;",
                    data: jsonData,
                    success: function (response) {
                        if (response.Success) {
                            var model;

                            if (isNew) {
                                model = ex.current().clone();
                            }
                            else {
                                model = ex.getSelected();
                            }

                            model.id(response.Data.Id);
                            model.name(response.Data.Name);
                            model.date(new Date(parseInt(response.Data.Date.substr(6))));
                            model.value(SL.formatters.double(response.Data.Value));
                            model.description(response.Data.Description);
                            model.partialGradeId(response.Data.PartialGradeId);

                            if (isNew) {
                                ex.models.unshift(model);
                            }

                            ex.select(ex, model);

                            SL.unmask();
                        }
                        else {
                            SL.hideModals();

                            $.each(response.Messages, function (i, message) {
                                ex.serverErrors.push(message.MemberNames[0] + ': ' + message.ErrorMessage);
                            });

                            SL.setModalPosition();
                        }
                    }
                });
            },

            destroy: function () {
                var ex = this;
                ex.isDeleting(true);
                SL.mask();
                SL.setModalPosition();
            },

            confirmDelete: function () {
                var ex = this;
                ex.isDeleting(false);
                SL.mask(true);

                $.post(SL.root + 'Exam/Delete', { id: ex.current().id() }, function (response) {
                    if (response.Success) {
                        var selectedModel = ex.getSelected();
                        ex.models.remove(selectedModel);
                        ex.clearSelection();

                        SL.unmask();
                    }
                    else {
                        SL.hideModals();

                        $.each(response.Messages, function (i, message) {
                            var memberName = !!message.MemberNames[0] ? message.MemberNames[0] + ': ' : '';
                            ex.serverErrors.push(memberName + message.ErrorMessage);
                        });

                        SL.setModalPosition();
                    }
                });
            },

            cancelDelete: function () {
                var ex = this;
                ex.isDeleting(false);
                SL.unmask();
            },

            load: function () {
                var ex = this;

                SL.mask(true);

                $.ajax({
                    url: SL.root + 'Exam/GetAll/?partialGradeId=' + self.partialGradeContext.current().id(),
                    dataType: 'json',
                    complete: function () {
                        SL.unmask();
                    },
                    success: function (response) {

                        $.each(response, function (i, e) {
                            var model = new Exam({
                                id: e.Id,
                                name: e.Name,
                                date: new Date(parseInt(e.Date.substr(6))),
                                value: SL.formatters.double(e.Value),
                                description: e.Description,
                                partialGradeId: e.PartialGradeId
                            });

                            ex.models.push(model);
                        });
                    },
                    error: function () {
                    }
                });
            },

            open: function (exam) {
                location.href = SL.root + 'ExamResult/?examId=' + exam.id();
            }
        }
    };

    self.partialGradeContext.load();
}

var timer;
var delay = (function () {
    timer = 0;
    return function (callback, ms) {
        clearTimeout(timer);
        timer = setTimeout(callback, ms);
    };
})();

ko.applyBindings(new CourseDashboardViewModel(), $('section')[0]);