function School(data) {
    data = data || {};

    var self = this;
    self.id = ko.observable(data.id);
    self.name = ko.observable(data.name);
    self.managerName = ko.observable(data.managerName);
    self.email = ko.observable(data.email);
    self.phoneNumber = ko.observable(data.phone);
    
    self.isSelected = ko.observable(false);

    self.phone = ko.computed({
        read: function () {
            return SL.formatters.phone(self.phoneNumber());
        },
        write: function (phone) {
            self.phoneNumber(phone);
        },
        owner: self
    });
}

School.prototype.clone = function () {
    return new School(ko.toJS(this));
};

//ko.utils.extend(School.prototype, {
//    initialize: function (data) {
//        var self = this;

//        self.id(data.Id);
//        self.name(data.name);
//        self.managerName(data.managerName);
//        self.email(data.email);
//        self.phone(data.phone);
//        self.isSelected(false);

//        console.log('initialized: ' + data.name);
//    }

//    //addOption: function () {
//    //    this.FormOptions.push({
//    //        val: ko.observable("new option")
//    //    });
//    //},

//    //removeOption: function (value) {
//    //    this.FormOptions.remove(value);
//    //}

//});

function SchoolViewModel() {
    var self = this;
    self.schools = ko.observableArray([]);
    self.current = ko.observable(new School({}));
    self.isLoading = ko.observable(false);
    self.isDeleting = ko.observable(false);
    self.serverErrors = ko.observableArray([]);
    self.errors = ko.observable({});

    self.select = function (school) {
        self.current(school.clone());
        self.deselectAll();
        school.isSelected(true);
    };

    self.clearSelection = function () {
        self.current(new School({}));
        self.errors({});
        self.deselectAll();
    };

    self.clearServerErrors = function () {
        self.serverErrors([]);
        SL.unmask();
    };

    self.deselectAll = function () {
        $.each(self.schools(), function (i, item) {
            item.isSelected(false);
        });
    };

    self.getSelected = function () {
        return _.find(self.schools(), function (school) {
            return school.isSelected();
        });
    };

    self.create = function () {
        if (self.isValid()) {
            var newSchool = self.current().clone();
            newSchool.id(0);

            var jsonData = ko.toJSON(newSchool);

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
        else if (!SL.validation.isEmailValid(self.current().email())) {
            brokenRules['email'] = 'E-mail inválido.';
        }

        self.errors(brokenRules);

        return !self.errors().name && !self.errors().email;
    };

    self.isUnique = function (jsonData) {
        var isUnique = true;

        $.ajax({
            url: SL.root + 'School/IsNameUnique',
            type: 'POST',
            dataType: 'json',
            contentType: "application/json; charset=utf-8;",
            data: jsonData,
            async: false,
            success: function (response) {
                isUnique = response;

                if (!isUnique) {
                    var brokenRules = [];
                    brokenRules['name'] = 'Esse campo deve ter um valor único entre os demais registros.';

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
            url: SL.root + 'School/Save',
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8;",
            data: jsonData,
            success: function (response) {
                if (response.Success) {
                    var school;

                    if (isNew) {
                        school = self.current().clone();
                    }
                    else {
                        school = self.getSelected();
                    }

                    school.id(response.Data.Id);
                    school.name(response.Data.Name);
                    school.managerName(response.Data.ManagerName);
                    school.email(response.Data.Email);
                    school.phone(response.Data.Phone);

                    if (isNew) {
                        self.schools.unshift(school);
                    }

                    self.select(school);

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

        $.post(SL.root + 'School/Delete', { id: self.current().id() }, function (response) {
            if (response.Success) {
                var selectedSchool = self.getSelected();
                self.schools.remove(selectedSchool);
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

    //self.destroy = function () {
    //    self.isConfirmationPending(true);
    //};

    //self.cancelDestroy = function () {
    //    self.isConfirmationPending(false);
    //};

    //self.confirmDestroy = function () {
    //    self.isConfirmationPending(false);
    //    self.isLoading(true);

    //    $.post(Sat.root + 'State/Destroy', { id: self.selected().id() }, function (response) {

    //        if (response.Success) {
    //            var selectedState = self.getSelected();
    //            self.states.remove(selectedState);
    //            self.clearSelection();
    //        }
    //        else {
    //            self.bindserverErrors(response);
    //        }

    //        self.isLoading(false);
    //    });
    //};

    //self.bindserverErrors = function (response) {
    //    var serverErrors = {};

    //    $.each(response.Messages, function (i, message) {
    //        serverErrors[message.MemberNames[0]] = message.ErrorMessage;
    //    });

    //    self.selected().serverErrors(serverErrors);
    //};

    //self.isLoading(true);

    //$.ajax({
    //    url: Sat.root + 'State/GetAll',
    //    dataType: 'json',
    //    complete: function () {
    //        self.isLoading(false);
    //    },
    //    success: function (response) {

    //        $.each(response, function (i, e) {
    //            var state = new State({
    //                id: e.Id,
    //                code: e.Code,
    //                name: e.Name
    //            });

    //            self.states.push(state);
    //        });

    //        $.fn.resizeTableHead();
    //    },
    //    error: function () {
    //    }
    //});

    //var data = [
    //    { id: 1, name: 'Escola QI - FL06 - São Leopoldo', managerName: 'Nathan Batista', email: 'fl06@qi.edu.br', phone: '5130235669' },
    //    { id: 2, name: 'Escola QI - FL11 - Novo Hamburgo', managerName: 'Juliani Neves', email: 'fl11@qi.edu.br', phone: '5130236235' },
    //    { id: 3, name: 'Escola QI - FL18 - Canoas', managerName: 'Alessandra Santos', email: 'fl18@qi.edu.br', phone: '5130231020' }
    //];

    //_.each(data, function (item) {
    //    self.schools.push(new School(item));
    //});

    self.load = function () {

        SL.mask(true);

        $.ajax({
            url: SL.root + 'School/GetAll',
            dataType: 'json',
            complete: function () {
                SL.unmask();
            },
            success: function (response) {

                $.each(response, function (i, e) {
                    var school = new School({
                        id: e.Id,
                        name: e.Name,
                        email: e.Email,
                        phone: e.Phone,
                        managerName: e.ManagerName
                    });

                    self.schools.push(school);
                });
            },
            error: function () {
            }
        });
    };

    self.load();
}

ko.applyBindings(new SchoolViewModel(), $('section')[0]);