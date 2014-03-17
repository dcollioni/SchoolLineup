function School(data) {
    data = data || {};

    var self = this;
    self.id = ko.observable();
    self.name = ko.observable();
    self.managerName = ko.observable();
    self.email = ko.observable();
    self.phone = ko.observable();
    self.selected = ko.observable();

    this.initialize(data);
}

School.prototype.clone = function () {
    return new School(ko.toJS(this));
};

ko.utils.extend(School.prototype, {
    initialize: function (data) {
        var self = this;

        self.id(data.Id);
        self.name(data.name);
        self.managerName(data.managerName);
        self.email(data.email);
        self.phone(data.phone);
        self.selected(false);
    }

    //addOption: function () {
    //    this.FormOptions.push({
    //        val: ko.observable("new option")
    //    });
    //},

    //removeOption: function (value) {
    //    this.FormOptions.remove(value);
    //}

});

function SchoolViewModel() {
    var self = this;
    self.schools = ko.observableArray([]);
    self.selected = ko.observable(new School({}));
    self.isLoading = ko.observable(false);

    self.select = function (school) {
        self.selected(school.clone());
        self.deselectAll();
        school.selected(true);
    };

    self.clearSelection = function () {
        self.selected(new School({}));
        self.deselectAll();
    };

    self.clearErrors = function () {
        self.selected().errors({});
    };

    self.deselectAll = function () {
        $.each(self.schools(), function (i, item) {
            item.selected(false);
        });
    };

    self.getSelected = function () {
        return _.find(self.schools(), function (school) {
            return school.selected();
        });
    };

    //self.create = function () {
    //    var newState = new State({
    //        id: 0,
    //        code: self.selected().code(),
    //        name: self.selected().name()
    //    });

    //    self.isLoading(true);

    //    $.post(Sat.root + 'State/Save', newState, function (response) {
    //        if (response.Success) {
    //            newState.id(response.Data.Id);
    //            newState.code(response.Data.Code);
    //            newState.name(response.Data.Name);

    //            self.states.unshift(newState);
    //            self.select(newState);
    //        }
    //        else {
    //            self.bindErrors(response);
    //        }

    //        self.isLoading(false);
    //    });
    //};

    //self.update = function () {
    //    self.isLoading(true);

    //    $.post(Sat.root + 'State/Save', self.selected(), function (response) {

    //        if (response.Success) {
    //            var selectedState = self.getSelected();

    //            selectedState.code(response.Data.Code);
    //            selectedState.name(response.Data.Name);
    //            selectedState.id(response.Data.Id);
    //            self.selected().errors({});
    //        }
    //        else {
    //            self.bindErrors(response);
    //        }

    //        self.isLoading(false);
    //    });
    //};

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

    var data = [
        { id: 1, name: 'Escola QI - FL06 - São Leopoldo', managerName: 'Nathan Batista', email: 'fl06@qi.edu.br', phone: '5130235669' },
        { id: 2, name: 'Escola QI - FL11 - Novo Hamburgo', managerName: 'Juliani Neves', email: 'fl11@qi.edu.br', phone: '5130236235' },
        { id: 3, name: 'Escola QI - FL18 - Canoas', managerName: 'Alessandra Santos', email: 'fl18@qi.edu.br', phone: '5130231020' }
    ];

    self.schools(data);
    //var viewModel = ko.mapping.fromJS(data);
}

ko.applyBindings(new SchoolViewModel(), $('section')[0]);