function ExamResult(data) {
    data = data || {};

    var self = this;
    self.id = ko.observable(data.id);
    self.examId = ko.observable(data.examId);

    self.studentId = ko.observable(data.studentId);
    self.studentName = ko.observable(data.studentName);

    self.value = ko.observable(data.value);
    self.description = ko.observable(data.description);

    self.valueError = ko.observable(null);

    self.isSelected = ko.observable(false);
}

ExamResult.prototype.clone = function () {
    return new ExamResult(ko.toJS(this));
};

function ExamResultViewModel() {
    var self = this;

    self.models = ko.observableArray([]);
    
    self.isLoading = ko.observable(false);

    self.serverErrors = ko.observableArray([]);
    self.errors = ko.observable({});

    self.resultsCount = ko.computed(function () {
        return '(' + self.models().length + ')';
    });

    self.isResultsFieldsetOpen = ko.observable(false);

    self.toggleResultsFieldset = function () {
        var current = self.isResultsFieldsetOpen();
        self.isResultsFieldsetOpen(!current);
    };

    self.examId = ko.observable($('#examId').val());
    self.examValue = ko.observable($('#examValue').val());

    self.clearServerErrors = function () {
        self.serverErrors([]);
        SL.unmask();
    };

    self.isAllSelected = ko.observable(false);

    self.isAllSelected.subscribe(function (selected) {
        _.each(self.models(), function (item) {
            item.isSelected(selected);
        });
    });

    self.selectedStudentsIds = ko.computed(function () {
        return _.map(_.filter(self.models(), function (item) { return item.isSelected(); }), function (item) { return item.studentId(); });
    });

    self.anySelected = ko.computed(function () {
        return _.some(self.models(), function (item) { return item.isSelected(); });
    });

    self.studentsSelectedMessage = ko.computed(function () {
        var count = self.selectedStudentsIds().length;

        var text = count > 1 ? " alunos selecionados" : " aluno selecionado";

        return count + text;
    });

    self.save = function () {
        if (!self.isValid()) {
            SL.unmask();
            return;
        }

        SL.mask(true);

        var data = [];

        _.each(self.models(), function (model) {
            data.push(model);
        });
        
        data = ko.toJSON(data);

        $.ajax({
            url: SL.root + 'ExamResult/Save',
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8;",
            data: data,
            success: function (response) {
                if (response.Success) {

                    self.load();
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

    self.isValid = function () {
        
        var invalid = 0;

        _.each(self.models(), function(model) {

            if (isNaN(model.value())) {
                model.valueError('Valor inválido');
                invalid++;
            }
            else if (parseFloat(model.value()) > parseFloat(self.examValue())) {
                model.valueError('O valor deve ser menor ou igual a ' + self.examValue());
                invalid++;
            }
        });

        return invalid == 0;
    };

    self.load = function () {

        SL.mask(true);

        $.ajax({
            url: SL.root + 'ExamResult/GetAll/?examId=' + self.examId(),
            dataType: 'json',
            complete: function () {
                SL.unmask();
                self.isAllSelected(false);
            },
            success: function (response) {

                self.models.removeAll();

                $.each(response, function (i, e) {
                    var model = new ExamResult({
                        id: e.Id,
                        examId: e.ExamId,
                        studentId: e.StudentId,
                        studentName: e.StudentName,
                        value: SL.formatters.double(e.Value),
                        description: e.Description
                    });

                    self.models.push(model);
                });
            },
            error: function () {
            }
        });
    };

    self.isSending = ko.observable(false);

    self.sendResultsByEmail = function () {
        self.isSending(true);
        SL.mask();
        SL.setModalPosition();
    };

    self.confirmSending = function () {
        self.isSending(false);
        SL.mask(true);

        var data = { examId: self.examId(), studentsIds: self.selectedStudentsIds() };
        data = ko.toJSON(data);

        $.ajax({
            url: SL.root + 'ExamResult/SendResultsByEmail',
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8;",
            data: data,
            complete: function () {
                SL.unmask();
                self.isAllSelected(true);
                self.isAllSelected(false);
            }
        });
    };

    self.cancelSending = function () {
        self.isSending(false);
        SL.unmask();
    };

    self.load();
}

ko.applyBindings(new ExamResultViewModel(), $('section')[0]);