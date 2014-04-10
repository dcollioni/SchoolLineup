function Course(data) {
    data = data || {};

    var self = this;
    self.id = ko.observable(data.id);
    self.name = ko.observable(data.name);
    self.startDate = ko.observable(data.startDate);
    self.endDate = ko.observable(data.endDate);
    self.schoolId = ko.observable(data.schoolId);
    
    self.isSelected = ko.observable(false);

    //self.phone = ko.computed({
    //    read: function () {
    //        return SL.formatters.phone(self.phoneNumber());
    //    },
    //    write: function (phone) {
    //        self.phoneNumber(phone);
    //    },
    //    owner: self
    //});
}

Course.prototype.clone = function () {
    return new Course(ko.toJS(this));
};

function CourseViewModel() {
    var self = this;
    self.courses = ko.observableArray([]);
    self.current = ko.observable(new Course({}));
    self.isLoading = ko.observable(false);
    self.isDeleting = ko.observable(false);
    self.serverErrors = ko.observableArray([]);
    self.errors = ko.observable({});
    self.schoolId = ko.observable($('#schoolId').val());

    self.select = function (course) {
        self.current(course.clone());
        self.deselectAll();
        course.isSelected(true);
    };

    self.clearSelection = function () {
        self.current(new Course({}));
        self.errors({});
        self.deselectAll();
    };

    self.clearServerErrors = function () {
        self.serverErrors([]);
        SL.unmask();
    };

    self.deselectAll = function () {
        $.each(self.courses(), function (i, item) {
            item.isSelected(false);
        });
    };

    self.getSelected = function () {
        return _.find(self.courses(), function (course) {
            return course.isSelected();
        });
    };

    self.create = function () {
        if (self.isValid()) {
            var newCourse = self.current().clone();
            newCourse.id(0);

            var jsonData = ko.toJSON(newCourse);
            self.save(jsonData);
        }
    };

    self.update = function () {
        if (self.isValid()) {
            var jsonData = ko.toJSON(self.current);

            if (self.isUnique(jsonData)) {
                self.save(jsonData);
            }
        }
    };

    self.isValid = function () {
        var brokenRules = [];

        if (!self.current().name()) {
            brokenRules['name'] = 'Esse campo deve ser preenchido.';
        }

        self.errors(brokenRules);

        return !self.errors().name;
    };

    self.save = function (jsonData) {
        var isNew = JSON.parse(jsonData).id === 0;

        SL.mask(true);

        $.ajax({
            url: SL.root + 'Course/Save',
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8;",
            data: jsonData,
            success: function (response) {
                if (response.Success) {
                    var course;

                    if (isNew) {
                        course = self.current().clone();
                    }
                    else {
                        course = self.getSelected();
                    }

                    course.id(response.Data.Id);
                    course.name(response.Data.Name);
                    course.startDate(response.Data.StartDate);
                    course.endDate(response.Data.EndDate);
                    course.schoolId(response.Data.SchoolId);

                    if (isNew) {
                        self.courses.unshift(course);
                    }

                    self.select(course);

                    SL.unmask();
                }
                else {
                    SL.hideModals();

                    $.each(response.Messages, function (i, message) {
                        self.serverErrors.push(message.MemberNames[0] + ': ' + message.ErrorMessage);
                    });

                    SL.setModalPosition();
                }
            }
        });
    };

    self.delete = function () {
        self.isDeleting(true);
        SL.mask();
        SL.setModalPosition();
    };

    self.confirmDelete = function () {
        self.isDeleting(false);
        SL.mask(true);

        $.post(SL.root + 'Course/Delete', { id: self.current().id() }, function (response) {
            if (response.Success) {
                var selectedCourse = self.getSelected();
                self.courses.remove(selectedCourse);
                self.clearSelection();
            }
            else {
                //self.bindserverErrors(response);
            }

            SL.unmask();
        });
    };

    self.cancelDelete = function () {
        self.isDeleting(false);
        SL.unmask();
    };

    self.load = function () {

        SL.mask(true);

        $.ajax({
            url: SL.root + 'Course/GetAll/' + self.schoolId(),
            dataType: 'json',
            complete: function () {
                SL.unmask();
            },
            success: function (response) {

                $.each(response, function (i, e) {
                    var course = new Course({
                        id: e.Id,
                        name: e.Name,
                        startDate: e.StartDate,
                        endDate: e.EndDate,
                        schoolId: e.SchoolId
                    });

                    self.courses.push(course);
                });
            },
            error: function () {
            }
        });
    };

    self.load();

    self.open = function (course) {
        location.href = SL.root + 'Student/' + course.id();
    }
}

ko.applyBindings(new CourseViewModel(), $('section')[0]);