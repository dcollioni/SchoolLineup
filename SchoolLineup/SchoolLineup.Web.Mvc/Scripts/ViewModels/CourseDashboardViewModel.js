﻿function Student(data) {
    var self = this;

    self.id = ko.observable(data.id);
    self.name = ko.observable(data.name);
    self.email = ko.observable(data.email);

    self.display = ko.computed(function () {
        if (self.name() !== self.email()) {
            return self.name() + ' (' + self.email() + ')';
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

PartialGrade.prototype.clone = function () {
    return new PartialGrade(ko.toJS(this));
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

    self.isStudentFieldsetOpen = ko.observable(true);

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
    self.partialGradeContext = function () {
        var pg = this;

        pg.models = ko.observableArray([]);
        pg.current = ko.observable(new PartialGrade({}));
        pg.serverErrors = ko.observableArray([]);
        pg.errors = ko.observable({});
        pg.isDeleting = ko.observable(false);

        pg.select = function (model) {
            pg.current(model.clone());
            pg.deselectAll();
            model.isSelected(true);
        };

        pg.clearSelection = function () {
            pg.current(new PartialGrade({}));
            pg.errors({});
            pg.deselectAll();
        };

        pg.clearServerErrors = function () {
            pg.serverErrors([]);
            SL.unmask();
        };

        pg.deselectAll = function () {
            $.each(pg.models(), function (i, item) {
                item.isSelected(false);
            });
        };

        pg.getSelected = function () {
            return _.find(pg.models(), function (model) {
                return model.isSelected();
            });
        };

        pg.create = function () {
            if (pg.isValid()) {
                var newModel = pg.current().clone();
                newModel.id(0);
                newModel.courseId(self.courseId());

                var jsonData = ko.toJSON(newModel);
                pg.save(jsonData);
            }
        };

        pg.update = function () {
            if (pg.isValid()) {
                var jsonData = ko.toJSON(pg.current);

                pg.save(jsonData);
            }
        };

        pg.isValid = function () {
            var brokenRules = [];

            if (!pg.current().name()) {
                brokenRules['name'] = 'Esse campo deve ser preenchido.';
            }
            if (!pg.current().order()) {
                brokenRules['order'] = 'Esse campo deve ser preenchido.';
            }

            pg.errors(brokenRules);

            return !pg.errors().name && !pg.errors().order;
        };

        pg.save = function (jsonData) {
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

                        pg.select(model);

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
        };

        pg.destroy = function () {
            pg.isDeleting(true);
            SL.mask();
            SL.setModalPosition();
        };

        pg.confirmDelete = function () {
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
        };

        pg.cancelDelete = function () {
            pg.isDeleting(false);
            SL.unmask();
        };

        pg.load = function () {

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
        };

        pg.load();
    };
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