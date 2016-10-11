type = ['', 'info', 'success', 'warning', 'danger'];

notif = {
    showNotification: function (from, align) {
        color = Math.floor((Math.random() * 4) + 1);

        $.notify({
            icon: "pe-7s-gift",
            message: "A <b>notification</b> example."

        }, {
            type: type[color],
            timer: 4000,
            placement: {
                from: from,
                align: align
            }
        });
    }
}

notifSuccess = {
    showNotification: function (from, align, message) {
        $.notify({
            message: message
        }, {
            type: type[2],
            timer: 4000,
            placement: {
                from: from,
                align: align
            }
        });
    }
}