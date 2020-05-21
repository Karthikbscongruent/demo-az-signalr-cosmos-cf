var ViewModel = function () {

    var self = this;
    self.telemetry = ko.observableArray([]);

    // Start the connection.
    var connection = new signalR.HubConnectionBuilder()
        .withUrl('/iot')
        .build();

    // Create a function that the hub can call to broadcast messages.
    connection.on('telemetry', function (message) {
        console.log('recived');

        var telemetry = self.telemetry();
        telemetry.push(message);

        self.telemetry(telemetry.sort(telemetry_sort_desc));
    });

    // Transport fallback functionality is now built into start.
    connection.start()
        .then(function () {
            console.log('connection started');
        })
        .catch(error => {
            console.error(error.message);
        });

    var telemetry_sort_desc = function (i, ii) {
        if (i.Timestamp < ii.Timestamp) return 1;
        if (i.Timestamp > ii.Timestamp) return -1;
        return 0;
    };
};




