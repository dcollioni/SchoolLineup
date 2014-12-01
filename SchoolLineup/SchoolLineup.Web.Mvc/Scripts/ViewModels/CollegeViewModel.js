function College(data) {
    data = data || {};

    var self = this;
    self.id = ko.observable(data.id);
    self.name = ko.observable(data.name);
    
    self.isSelected = ko.observable(false);
}

College.prototype.clone = function () {
    return new College(ko.toJS(this));
};

function CollegeViewModel() {
    var self = this;
    self.colleges = ko.observableArray([]);
    self.current = ko.observable(new College({}));
    self.isLoading = ko.observable(false);
    self.isDeleting = ko.observable(false);
    self.serverErrors = ko.observableArray([]);
    self.errors = ko.observable({});

    self.select = function (college) {
        self.current(college.clone());
        self.deselectAll();
        college.isSelected(true);
    };

    self.clearSelection = function () {
        self.current(new College({}));
        self.errors({});
        self.deselectAll();
    };

    self.clearServerErrors = function () {
        self.serverErrors([]);
        SL.unmask();
    };

    self.deselectAll = function () {
        $.each(self.colleges(), function (i, item) {
            item.isSelected(false);
        });
    };

    self.getSelected = function () {
        return _.find(self.colleges(), function (college) {
            return college.isSelected();
        });
    };

    self.create = function () {
        if (self.isValid()) {
            var newCollege = self.current().clone();
            newCollege.id(0);

            var jsonData = ko.toJSON(newCollege);

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

        self.errors(brokenRules);

        return !self.errors().name;
    };

    self.isUnique = function (jsonData) {
        var isUnique = true;

        $.ajax({
            url: SL.root + 'College/IsNameUnique',
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
            url: SL.root + 'College/Save',
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8;",
            data: jsonData,
            success: function (response) {
                if (response.Success) {
                    var college;

                    if (isNew) {
                        college = self.current().clone();
                    }
                    else {
                        college = self.getSelected();
                    }

                    college.id(response.Data.Id);
                    college.name(response.Data.Name);

                    if (isNew) {
                        self.colleges.unshift(college);
                    }

                    self.select(college);

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

        $.post(SL.root + 'College/Delete', { id: self.current().id() }, function (response) {
            if (response.Success) {
                var selectedCollege = self.getSelected();
                self.colleges.remove(selectedCollege);
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
            url: SL.root + 'College/GetAll',
            dataType: 'json',
            complete: function () {
                SL.unmask();
            },
            success: function (response) {

                $.each(response, function (i, e) {
                    var college = new College({
                        id: e.Id,
                        name: e.Name
                    });

                    self.colleges.push(college);
                });
            },
            error: function () {
            }
        });
    };

    self.load();

    self.open = function (college) {
        location.href = SL.root + 'Course/Index/' + college.id();
    }
}

ko.applyBindings(new CollegeViewModel(), $('section')[0]);