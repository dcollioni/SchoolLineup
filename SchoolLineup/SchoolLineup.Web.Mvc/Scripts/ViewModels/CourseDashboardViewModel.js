function Student(data) {
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
        return ' (total: ' + self.students().length + ')';
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

    self.loadStudents();
}

var timer;
var delay = (function () {
    timer = 0;
    return function (callback, ms) {
        clearTimeout(timer);
        timer = setTimeout(callback, ms);
    };
})();

ko.applyBindings(new CourseDashboardViewModel());