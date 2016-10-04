type = ['', 'info', 'success', 'warning', 'danger'];

notifCreateAccount = {
    showNotification: function (from, align) {
        color = Math.floor((Math.random() * 4) + 1);

        $.notify({
            icon: "pe-7s-bell",
            message: "Account has <b>Succesfully</b> created."

        }, {
            type: 'info',
            timer: 1000,
            placement: {
                from: from,
                align: align
            }
        });
    }
}

notifCreateAccountFail = {
    showNotification: function (from, align) {
        color = Math.floor((Math.random() * 4) + 1);

        $.notify({
            icon: "pe-7s-bell",
            message: "Account creation is <b>Unsuccessful</b>."

        }, {
            type: 'alert',
            timer: 1000,
            placement: {
                from: from,
                align: align
            }
        });
    }
}