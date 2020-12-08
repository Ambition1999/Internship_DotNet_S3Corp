
function Validator(options) {

    var selectorRules = {};

    function validate(inputElement, rule) {
        var errorMessage;
        var errorElement = inputElement.parentElement.querySelector(options.errorSelector);

        // Get all rule of selector
        var rules = selectorRules[rule.selector];
        // console.log(rules)

        // 
        for (var i = 0; i < rules.length; ++i) {
            errorMessage = rules[i](inputElement.value);
            if (errorMessage) break;
        }

        if (errorMessage) {
            errorElement.innerText = errorMessage;
            inputElement.parentElement.classList.add('invalid');
        }
        else {
            errorElement.innerText = '';
            inputElement.parentElement.classList.remove('invalid');
        }

        return !errorMessage;
    }

    function validateChecking(inputElement, rule) {
        var errorMessage;
        var errorElement = inputElement.parentElement.querySelector(options.errorSelector);

        // Get all rule of selector
        var rules = selectorRules[rule.selector];
        // console.log(rules)

        // 
        for (var i = 0; i < rules.length; ++i) {
            errorMessage = rules[i](inputElement.value);
            if (errorMessage) break;
        }

        return !errorMessage;
    }


    var formElement = document.querySelector(options.form);

    if (formElement) {

        formElement.onsubmit = function (e) {
            e.preventDefault();

            var isFormValid = true;
            var boolValue = false;

            options.rules.forEach(function (rule) {
                var inputElement = formElement.querySelector(rule.selector);
                var isVaid = validate(inputElement, rule);
                if (!isVaid) {
                    isFormValid = false;
                }
            });

            // Get all data in form
            if (isFormValid) {
                boolValue = true;
                if (typeof options.onSubmit === 'function') {
                    var formEnableInputs = formElement.querySelectorAll('[name]');
                    var formValues = Array.from(formEnableInputs).reduce(function (values, input) {
                        values[input.name] = input.value
                        return values;
                    }, {});
                    //e.currentTarget.submit();
                    //formElement.submit();
                }
            }
            else {
                console.log('Có lỗi');
            }

            //Continues to submit form
            console.log(boolValue);
            if (boolValue == true) {
                console.log(boolValue);
                e.currentTarget.submit();
            }
        }

        //Event for enable or disable submit button
        formElement.oninput = function (e) {
            console.log("onblur event form");
            var isFormValid = true;

            options.rules.forEach(function (rule) {
                var inputElement = formElement.querySelector(rule.selector);
                var isVaid = validateChecking(inputElement, rule);
                if (!isVaid) {
                    isFormValid = false;
                }
            });

            // Get all data in form
            if (isFormValid) {
                if (typeof options.onSubmit === 'function') {
                    var formEnableInputs = formElement.querySelectorAll('[name]');
                    var formValues = Array.from(formEnableInputs).reduce(function (values, input) {
                        values[input.name] = input.value
                        return values;
                    }, {});
                }
                document.getElementById("btn-submit").disabled = false;
            }
            else {
                document.getElementById("btn-submit").disabled = true;
                console.log('Có lỗi');
            }
        }

        options.rules.forEach(function (rule) {

            // Store rule to array
            if (Array.isArray(selectorRules[rule.selector])) {
                selectorRules[rule.selector].push(rule.test);
            } else {
                selectorRules[rule.selector] = [rule.test];
            }

            var inputElement = formElement.querySelector(rule.selector);

            if (inputElement) {
                inputElement.onblur = function () {
                    validate(inputElement, rule);
                }

                inputElement.oninput = function () {
                    var errorElement = inputElement.parentElement.querySelector(options.errorSelector);
                    errorElement.innerText = '';
                    inputElement.parentElement.classList.remove('invalid');
                }
            }

        })
    }
}

Validator.isRequired = function (selector, message) {
    return {
        selector: selector,
        test: function (value) {
            return value.trim() ? undefined : message

        }
    }
}

Validator.isRequiredSearch = function (selector, message) {
    return {
        selector: selector,
        test: function (value) {
            return value.trim() ? undefined : message

        }
    }
}

Validator.isEmail = function (selector) {
    return {
        selector: selector,
        test: function (value) {
            var regex = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
            return regex.test(value) ? undefined : 'Email không phù hợp';
        }
    }
}

Validator.isPhoneNumber = function (selector) {
    return {
        selector: selector,
        test: function (value) {
            var vnf_regex = /((09|03|07|08|05)+([0-9]{8})\b)/g;
            return vnf_regex.test(value) ? undefined : 'Số điện thoại không phù hợp';
        }
    }
}

Validator.minLength = function (selector, min, message) {
    return {
        selector: selector,
        test: function (value) {
            return value.length >= min ? undefined : message || 'Mật khẩu tối thiểu ' + min + ' kí tự';
        }
    }
}

Validator.isConfirmed = function (selector, getConfirmValue, message) {
    return {
        selector: selector,
        test: function (value) {
            return value == getConfirmValue() ? undefined : message || 'Mật khẩu nhập lại không trùng khớp'
        }
    }
}