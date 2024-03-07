$(document).ready(function () {
    $(function () {
        $('.two-decimals').keyup(function () {
            if ($(this).val().indexOf('.') != -1) {
                if ($(this).val().split(".")[1].length > 2) {
                    if (isNaN(parseFloat(this.value))) return;
                    this.value = parseFloat(this.value).toFixed(2);
                }
            }
            return this; //for chaining
        });
    });
});