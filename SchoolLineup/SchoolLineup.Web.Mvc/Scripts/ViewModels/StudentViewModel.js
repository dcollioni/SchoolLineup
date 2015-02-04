function Student(data) {
    data = data || {};

    var self = this;
    self.id = ko.observable(data.id);
    self.name = ko.observable(data.name);
    self.email = ko.observable(data.email);
    self.registrationCode = ko.observable(data.registrationCode);
    
    self.isSelected = ko.observable(false);
}

Student.prototype.clone = function () {
    return new Student(ko.toJS(this));
};

function StudentViewModel() {
    var self = this;
    self.students = ko.observableArray([]);
    self.current = ko.observable(new Student({}));
    self.isLoading = ko.observable(false);
    self.isDeleting = ko.observable(false);
    self.isImporting = ko.observable(false);
    self.serverErrors = ko.observableArray([]);
    self.errors = ko.observable({});

    self.select = function (student) {
        self.current(student.clone());
        self.deselectAll();
        student.isSelected(true);
    };

    self.clearSelection = function () {
        self.current(new Student({}));
        self.errors({});
        self.deselectAll();
    };

    self.clearServerErrors = function () {
        self.serverErrors([]);
        SL.unmask();
    };

    self.deselectAll = function () {
        $.each(self.students(), function (i, item) {
            item.isSelected(false);
        });
    };

    self.getSelected = function () {
        return _.find(self.students(), function (student) {
            return student.isSelected();
        });
    };

    self.create = function () {
        if (self.isValid()) {
            var newStudent = self.current().clone();
            newStudent.id(0);

            var jsonData = ko.toJSON(newStudent);

            if (self.isUnique(jsonData)) {
                self.save(jsonData);
            }
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
        if (!self.current().email()) {
            brokenRules['email'] = 'Esse campo deve ser preenchido.';
        }

        self.errors(brokenRules);

        return !self.errors().name && !self.errors().email;
    };

    self.isUnique = function (jsonData) {
        var isUnique = true;

        $.ajax({
            url: SL.root + 'Student/IsEmailUnique',
            type: 'POST',
            dataType: 'json',
            contentType: "application/json; charset=utf-8;",
            data: jsonData,
            async: false,
            success: function (response) {
                isUnique = response;

                if (!isUnique) {
                    var brokenRules = [];
                    brokenRules['email'] = 'Esse campo deve ter um valor único entre os demais registros.';

                    self.errors(brokenRules);
                }
            }
        });

        return isUnique;
    };

    self.save = function (jsonData) {
        var isNew = JSON.parse(jsonData).id === 0;

        SL.mask(true);

        $.ajax({
            url: SL.root + 'Student/Save',
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8;",
            data: jsonData,
            success: function (response) {
                if (response.Success) {
                    var student;

                    if (isNew) {
                        student = self.current().clone();
                    }
                    else {
                        student = self.getSelected();
                    }

                    student.id(response.Data.Id);
                    student.name(response.Data.Name);
                    student.email(response.Data.Email);
                    student.registrationCode(response.Data.RegistrationCode);

                    if (isNew) {
                        self.students.unshift(student);
                    }

                    self.select(student);

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

        $.post(SL.root + 'Student/Delete', { id: self.current().id() }, function (response) {
            if (response.Success) {
                var selectedStudent = self.getSelected();
                self.students.remove(selectedStudent);
                self.clearSelection();

                SL.unmask();
            }
            else {
                SL.hideModals();

                $.each(response.Messages, function (i, message) {
                    var memberName = !!message.MemberNames[0] ? message.MemberNames[0] + ': ' : '';
                    self.serverErrors.push(memberName + message.ErrorMessage);
                });

                SL.setModalPosition();
            }
        });
    };

    self.cancelDelete = function () {
        self.isDeleting(false);
        SL.unmask();
    };

    self.load = function () {

        SL.mask(true);

        $.ajax({
            url: SL.root + 'Student/GetAll',
            dataType: 'json',
            complete: function () {
                SL.unmask();
            },
            success: function (response) {

                $.each(response, function (i, e) {
                    var student = new Student({
                        id: e.Id,
                        name: e.Name,
                        email: e.Email,
                        registrationCode: e.RegistrationCode
                    });

                    self.students.push(student);
                });
            },
            error: function () {
            }
        });
    };

    self.importData = function () {
        self.isImporting(true);
        SL.mask();
        SL.setModalPosition();
    };

    self.cancelImport = function () {
        self.isImporting(false);
        SL.unmask();
    };

    self.load();

    self.selectByIndex = function (index) {
        var lastIndex = self.students().length - 1;

        if (0 <= index && index <= lastIndex) {
            var item = self.students()[index];
            self.select(item);
            return true;
        }
        return false;
    };

    $(document).keydown(function (e) {
        if (!!self.getSelected()) {
            var itemIndex = self.students.indexOf(self.getSelected());

            if (e.keyCode === SL.consts.KEY_UP || e.keyCode === SL.consts.KEY_DOWN) {
                itemIndex = e.keyCode === SL.consts.KEY_UP ? itemIndex - 1 : itemIndex + 1;

                var changed = self.selectByIndex(itemIndex);

                if (changed) {
                    e.preventDefault();
                }
            }
        }
    });
}

ko.applyBindings(new StudentViewModel(), $('section')[0]);