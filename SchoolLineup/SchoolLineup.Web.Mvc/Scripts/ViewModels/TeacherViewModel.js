function Teacher(data) {
    data = data || {};

    var self = this;
    self.id = ko.observable(data.id);
    self.name = ko.observable(data.name);
    self.email = ko.observable(data.email);

    self.isSelected = ko.observable(false);
}

Teacher.prototype.clone = function () {
    return new Teacher(ko.toJS(this));
};

function TeacherViewModel() {
    
    var self = this;

    self.models = ko.observableArray([]);
    self.current = ko.observable(new Teacher({}));
    self.serverErrors = ko.observableArray([]);
    self.errors = ko.observable({});
    self.isDeleting = ko.observable(false);

    self.select = function (model) {
        if (!!self.current() && self.current().id() === model.id()) {
            return;
        }

        self.current(model.clone());
        self.deselectAll();
        model.isSelected(true);
    };

    self.clearSelection = function () {
        self.current(new Teacher({}));
        self.errors({});
        self.deselectAll();
    };

    self.clearServerErrors = function () {
        self.serverErrors([]);
        SL.unmask();
    };

    self.deselectAll = function () {
        $.each(self.models(), function (i, item) {
            item.isSelected(false);
        });
    };

    self.getSelected = function () {
        return _.find(self.models(), function (model) {
            return model.isSelected();
        });
    };

    self.create = function () {
        if (self.isValid()) {
            var newModel = self.current().clone();
            newModel.id(0);

            var jsonData = ko.toJSON(newModel);
            self.save(jsonData);
        }
    };

    self.update = function () {
        if (self.isValid()) {
            var jsonData = ko.toJSON(self.current);

            self.save(jsonData);
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

    self.save = function (jsonData) {
        var isNew = JSON.parse(jsonData).id === 0;

        SL.mask(true);

        $.ajax({
            url: SL.root + 'Teacher/Save',
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8;",
            data: jsonData,
            success: function (response) {
                if (response.Success) {
                    var model;

                    if (isNew) {
                        model = self.current().clone();
                    }
                    else {
                        model = self.getSelected();
                    }

                    model.id(response.Data.Id);
                    model.name(response.Data.Name);
                    model.email(response.Data.Email);

                    if (isNew) {
                        self.models.unshift(model);
                    }

                    self.select(model);

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

    self.destroy = function () {
        self.isDeleting(true);
        SL.mask();
        SL.setModalPosition();
    };

    self.confirmDelete = function () {
        self.isDeleting(false);
        SL.mask(true);

        $.post(SL.root + 'Teacher/Delete', { id: self.current().id() }, function (response) {
            if (response.Success) {
                var selectedModel = self.getSelected();
                self.models.remove(selectedModel);
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
            url: SL.root + 'Teacher/GetAll/',
            dataType: 'json',
            complete: function () {
                SL.unmask();
            },
            success: function (response) {

                $.each(response, function (i, e) {
                    var model = new Teacher({
                        id: e.Id,
                        name: e.Name,
                        email: e.Email
                    });

                    self.models.push(model);
                });
            },
            error: function () {
            }
        });
    };

    self.load();

    self.selectByIndex = function (index) {
        var lastIndex = self.models().length - 1;

        if (0 <= index && index <= lastIndex) {
            var item = self.models()[index];
            self.select(item);
            return true;
        }
        return false;
    };

    $(document).keydown(function (e) {
        if (!!self.getSelected()) {
            var itemIndex = self.models.indexOf(self.getSelected());

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

ko.applyBindings(new TeacherViewModel(), $('section')[0]);