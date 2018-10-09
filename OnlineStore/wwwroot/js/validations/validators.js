$.validator.addMethod("mustbegreaterthan", function (value, element, params) {
    var result = checkIsValidDate(value);
    if (!result.isValid) {
        return false;
    }
    var checkResult = checkIsValidDate(params);
    return result.value > checkResult.value;
});

$.validator.unobtrusive.adapters.add("mustbegreaterthan", ["minimumdate"], function (options) {
    options.rules["mustbegreaterthan"] = options.params.minimumdate;
    options.message["mustbegreaterthan"] = options.message;

})

function checkIsValidDate(valueToCheck){
    var d = new Date(valueToCheck);
    if (Object.prototype.toString.call(d) === "[object Date]") {
        if (!isNaN(d.getTime())) {
            return { isValid: true, value: d };
        }
    }
    return { isValid: false };
}