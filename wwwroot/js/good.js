function ValidatorC2(options) {
    console.log(options.onSubmit);
    ArrayOfRules = {};

    Validate = (inputElement, rule) => {
        var errElement = inputElement.parentElement.querySelector(options.errSelector);
        var errMess;//rule.test(inputElement.value)
        var rules = ArrayOfRules[rule.selector]; //
        for (var i = 0; i < rules.length; i++) {

            errMess = rules[i](inputElement.value);
            console.log(errMess);
            if (errMess) break;
        }
        if (errMess) { // tra ve false -> co loi
            errElement.innerText = errMess;
        } else {
            errElement.innerText = "";
        };
        return !errMess;

    };


    var formElement = document.querySelector(options.form);

    //Buoc submit form
    if (formElement) {
        //Xu ly khi submit form
        formElement.onsubmit = (e) => {
            e.preventDefault();
            var isFormValid = true;

            //Validate
            options.rules.forEach((rule) => {
                var inputElement = formElement.querySelector(rule.selector);
                var isValid = Validate(inputElement, rule);
                if (!isValid) {
                    isFormValid = false;
                }
            });

            //buoc quan trong lay data
            if (isFormValid) {
                if (typeof options.onSubmit === 'function') {
                    var enableInputs = formElement.querySelectorAll("[name]");
                    var formValue = Array.from(enableInputs).reduce(function (values, input) {
                        return (values[input.name] = input.value) && values;
                    }, {});
                    options.onSubmit(formValue);
                } else {
                    formElement.submit();
                };
            };
        };

        //
        options.rules.forEach(function (rule) {
            var inputElement = formElement.querySelector(rule.selector);
            //var errElement = inputElement.parentElement.querySelector(options.errSelector);
            if (Array.isArray(ArrayOfRules[rule.selector])) {
                ArrayOfRules[rule.selector].push(rule.test);
            } else {
                ArrayOfRules[rule.selector] = [rule.test];
            }
            if (inputElement) {
                //Xu ly khi onblur
                inputElement.onblur = () => {
                    var check = Validate(inputElement, rule);
                    console.log(check);
                };
                //xu ly khi nhap lieu input
                inputElement.oninput = () => {
                    var errElement = inputElement.parentElement.querySelector(options.errSelector);
                    errElement.innerText = '';
                }
            };
        });




        //Lap qua cac rule -> thanh object chua all rule


    };
    console.log(ArrayOfRules);
};


//Dinh nghia ra cac rules
ValidatorC2.isRequired = (param1) => {
    return {
        selector: param1,
        test: (value) => {
            return value.trim() ? undefined : "required!";
        }
    };
};
ValidatorC2.isMinLength = (param1, param2) => {
    return {
        selector: param1,
        test: (value) => {
            return (value.length >= param2) ? undefined : `Enter at least ${param2} characters`;
        }
    };
};