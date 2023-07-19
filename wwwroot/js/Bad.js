function Validator(options) {
    
    var ArrayRuleSelector = {}

    //
    ValidateC2 = (inputElement, rule) => {


        var ErrElement = inputElement.parentElement.querySelector(options.errSelector);
        var ErrMess;//rule.test(inputElement.value)
        var rulesInSelector = ArrayRuleSelector[rule.selector];
        for (var i = 0; i < rulesInSelector.length; i++) {
            ErrMess = rulesInSelector[i](inputElement.value)
            if (ErrMess) break;
        }
        if (ErrMess) {
            ErrElement.innerText = ErrMess;
        } else {
            ErrElement.innerText = "";
        };
        return !ErrMess
    }
    //
    var formElement = document.querySelector(options.form);

    if (formElement) {
        //submit form
        formElement.onsubmit = (e) => {
            e.preventDefault();
            var isValidForm = true;

            //validate
            options.rules.forEach((rule) => {
                var inputElement = formElement.querySelector(rule.selector);
                var isValid = ValidateC2(inputElement, rule);
                if (!isValid) { //co loi -> true
                    isValidForm = false;
                }
            });
            //buoc quan trong
            if (isValidForm) {
                
                if (typeof options.onSubmit === 'function') {
                    var ennableInputs = formElement.querySelectorAll("[name]");
                    var formValue = Array.from(ennableInputs).reduce((values, input) => {
                        values[input.name] = input.value
                        return values;
                    }, {});
                    options.onSubmit(formValue);
                } else {
                    formElement.submit();
                };
            };
        };


        //
        options.rules.forEach((rule) => {
            console.log(rule); //callback function rule from defined
            var inputElement = document.querySelector(rule.selector);

            if (Array.isArray(ArrayRuleSelector[rule.selector])) {
                ArrayRuleSelector[rule.selector].push(rule.test);
            } else {
                ArrayRuleSelector[rule.selector] = [rule.test];
            }
            if (inputElement) {
                //xu ly khi on blur 
                inputElement.onblur = () => {
                    ValidateC2(inputElement, rule);
                };
                //xu ly khi nhap
                inputElement.oninput = () => {
                    var ErrElement = inputElement.parentElement.querySelector(options.errSelector);

                    ErrElement.innerText = ""
                }
            };
        });
    };
    console.log(ArrayRuleSelector);
};

//V
Validator.isRequires = (param1) => {
    return {
        selector: param1,
        test: (value) => {
            return value ? undefined : "required!";
        }
    };
};
Validator.isMinLength = (param1, param2) => {
    return {
        selector: param1,
        test: (value) => {
            return value.length >= param2 ? undefined : `Enter at least ${param2} characters`;
        }
    }
};
Validator.isHello = (param1, param2) => {
    return {
        selector: param1,
        test: (value) => {
            return value === param2() ? undefined : "Password not confirm!";
        }
    };
};