function School(data) {
    data = data || {};

    var self = this;
    self.id = ko.observable(data.id);
    self.name = ko.observable(data.name);
    self.managerName = ko.observable(data.managerName);
    self.email = ko.observable(data.email);
    self.phone = ko.observable(data.phone);
    self.isSelected = ko.observable(false);
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

    self.select = function (school) {
        self.current(school.clone());
        self.deselectAll();
        school.isSelected(true);
    };

    self.clearSelection = function () {
        self.current(new School({}));
        self.deselectAll();
    };

    //self.clearErrors = function () {
    //    self.current().errors({});
    //};

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
        var newSchool = self.current().clone();
        newSchool.id(0);

        var jsonData = ko.toJSON(newSchool);
        self.save(jsonData);
    };

    self.update = function () {
        var jsonData = ko.toJSON(self.current);
        self.save(jsonData);
    };

    self.save = function (jsonData) {
        var isNew = JSON.parse(jsonData).id === 0;

        SL.mask();

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
                        self.select(school);
                    }
                }
                else {
                    //self.bindErrors(response);
                }

                SL.unmask();
            }
        });
    };

    self.delete = function () {
        SL.mask();

        $.post(SL.root + 'School/Delete', { id: self.current().id() }, function (response) {
            if (response.Success) {
                var selectedSchool = self.getSelected();
                self.schools.remove(selectedSchool);
                self.clearSelection();
            }
            else {
                //self.bindErrors(response);
            }

            SL.unmask();
        });
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
    //            self.bindErrors(response);
    //        }

    //        self.isLoading(false);
    //    });
    //};

    //self.bindErrors = function (response) {
    //    var errors = {};

    //    $.each(response.Messages, function (i, message) {
    //        errors[message.MemberNames[0]] = message.ErrorMessage;
    //    });

    //    self.selected().errors(errors);
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

        SL.mask();

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
    }

    self.load();

    //setTimeout(self.load(), 10000);

    //var viewModel = ko.mapping.fromJS(data);
}

ko.applyBindings(new SchoolViewModel(), $('section')[0]);