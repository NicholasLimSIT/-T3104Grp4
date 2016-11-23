type = ['', 'info', 'success', 'warning', 'danger'];

notifCreateAccount = {
    showNotification: function (from, align) {
        $.notify({
            icon: "pe-7s-bell",
            message: "Account is <b>succesfully</b> created."

        }, {
            type: 'success',
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
        $.notify({
            icon: "pe-7s-bell",
            message: "Account creation is <b>unsuccessful</b>."

        }, {
            type: 'danger',
            timer: 1000,
            placement: {
                from: from,
                align: align
            }
        });
    }
}

notifArchive = {
    showNotification: function (from, align) {
        color = Math.floor((Math.random() * 4) + 1);

        $.notify({
            icon: "pe-7s-bell",
            message: "Archived Records <b>Succesful</b>."

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
notifFailArchive = {
    showNotification: function (from, align) {
        color = Math.floor((Math.random() * 4) + 1);

        $.notify({
            icon: "pe-7s-bell",
            message: "Archived Records <b>Unsuccesful</b>."

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

notifSuccess = {
    showNotification: function (message) {
        $.notify({
            message: message
        }, {
            type: type[2],
            timer: 4000,
            placement: {
                from: "top",
                align: "center"
            }
        });
    }
}

notifInfo = {
    showNotification: function (message) {
        $.notify({
            message: message
        }, {
            type: type[1],
            timer: 4000,
            placement: {
                from: "top",
                align: "center"
            }
        });
    }
}