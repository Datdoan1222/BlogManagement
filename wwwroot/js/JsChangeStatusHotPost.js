
var user = {
    init: function () {
        user.registerEvents();
    },
    registerEvents: function () {
        $('.btnhotpost').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/home/manager/changestatushotpost",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('True');
                    }
                    else {
                        btn.text('False');
                    }
                }
            });
        });
    }
}
user.init();