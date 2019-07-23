// This is included with the Parsley library itself,
// thus there is no use in adding it to your project.


Parsley.addMessages('vi', {
    defaultMessage: "This value seems to be invalid.",
    type: {
        email: "Email chưa hợp lệ.",
        url: "This value should be a valid url.",
        number: "This value should be a valid number.",
        integer: "This value should be a valid integer.",
        //digits:       "This value should be digits.",
        digits: "Giá trị phải là số.",
        alphanum: "This value should be alphanumeric."
    },
    notblank: "This value should not be blank.",
    required: "Vui lòng nhập vào ô này.",
    pattern: "This value seems to be invalid.",
    //min: "This value should be greater than or equal to %s.",
    min: "Giá trị này phải lớn hơn hoặc bằng %s.",
    //max: "This value should be lower than or equal to %s.",
    max: "Giá trị này phải nhỏ hơn hoặc bằng %s.",
    //range:          "This value should be between %s and %s.",
    range: "Giá trị này chỉ được có độ dài từ %s đến %s ký tự.",
    minlength: "This value is too short. It should have %s characters or more.",
    maxlength: "This value is too long. It should have %s characters or fewer.",
    //length:         "This value length is invalid. It should be between %s and %s characters long.",
    length: "Độ dài chưa hợp lệ. Độ dài chỉ được từ %s đến %s ký tự.",
    mincheck: "You must select at least %s choices.",
    maxcheck: "You must select %s choices or fewer.",
    check: "You must select between %s and %s choices.",
    equalto: "This value should be the same.",
    euvatin: "It's not a valid VAT Identification Number.",
});

Parsley.setLocale('vi');
