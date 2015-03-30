function ChangePasswordViewModel() {

    var self = this;

    self.password = ko.observable();
    self.passwordConfirmation = ko.observable();

    self.isLoading = ko.observable(false);
    self.errors = ko.observable({});

    self.isValid = function () {
        var brokenRules = [];
        var errorsCount = 0;

        if (!self.password()) {
            brokenRules['password'] = 'Esse campo deve ser preenchido.';
            errorsCount++;
        } else if (self.password().length < 6) {
            brokenRules['password'] = 'A senha deve conter, no mínimo, seis caracteres.';
            errorsCount++;
        }

        if (!self.passwordConfirmation()) {
            brokenRules['passwordConfirmation'] = 'Esse campo deve ser preenchido.';
            errorsCount++;
        }

        if (errorsCount == 0) {
            if (self.password() !== self.passwordConfirmation()) {
                brokenRules['passwordConfirmation'] = 'A confirmação deve ser idêntica à senha.';
                errorsCount++;
            }
        }

        self.errors(brokenRules);

        return (errorsCount == 0);
    };

    self.send = function () {
        if (self.isValid()) {
            return true;
        }
    };
}

ko.applyBindings(new ChangePasswordViewModel());