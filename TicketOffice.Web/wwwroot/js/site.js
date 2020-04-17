if (window.__pageModel != null && window.__pageModel != undefined) {

    var Url = {
        Action: function (action, controller, attrs) {
            window.__baseUrl = window.__baseUrl.replace(/\/$/, "");
            var arr = [];
            arr.push(window.__baseUrl);
            if (attrs && attrs.area)
                arr.push(attrs.area);
            if (controller)
                arr.push(controller);
            if (action)
                arr.push(action);
            if (attrs && attrs.id)
                arr.push(attrs.id);
            return arr.join("/");
        }
    };

    function SpectacleListItem(spectacle) {
        var self = this;
        spectacle = spectacle || { Id: null, Name: "", ShowDate: moment(), Duration: moment.duration('02:00:00'), TicketCount: null };
        self.id = ko.observable(spectacle.Id);
        self.name = ko.observable(spectacle.Name);
        self.startDate = ko.observable(moment(spectacle.ShowDate));
        self.duration = ko.observable(moment.duration(spectacle.Duration));
        self.ticketCount = ko.observable(spectacle.TicketCount);
        self.endDate = ko.computed(function () {
            return self.startDate().clone().add(self.duration().hours(), 'hours').add(self.duration().minutes(), 'minutes');
        });
    };

    var viewModel = function () {
        var self = this;

        $('input[name="spectacleStartDate"]').daterangepicker({
            timePicker: true,
            timePicker24Hour: true,
            minYear: parseInt(moment().format('YYYY'), 10),
            maxYear: parseInt(moment().format('YYYY'), 10) + 3,
            locale: {
                format: 'DD.MM HH:mm'
            }
        },
            function (start, end) {
                self.spectacleModalModel.startDate(start);
                self.spectacleModalModel.duration(moment.duration(end.diff(start)));
            }
        );

        self.spectaclesToVm = function (spectacles) {
            return ko.utils.arrayMap(spectacles, function (spectacle) {
                return new SpectacleListItem(spectacle);
            });
        };

        /* Переменные */
        self.spectacleModalModel = {
            id: ko.observable(),
            name: ko.observable(),
            startDate: ko.observable(),
            duration: ko.observable(),
            ticketCount: ko.observable(),
            saveChanges: function () {
                $.ajax({
                    type: "POST",
                    url: Url.Action("CreateOrUpdateShow", "Home"),
                    data: {
                        id: self.spectacleModalModel.id() || '00000000-0000-0000-0000-000000000000',
                        name: self.spectacleModalModel.name(),
                        showDate: self.spectacleModalModel.startDate().format(),
                        duration: self.spectacleModalModel.duration().hours() + ':' + self.spectacleModalModel.duration().minutes(),
                        ticketCount: self.spectacleModalModel.ticketCount(),
                    },
                    success: function (data) {
                        location.reload();
                    },
                    error: function (err) {
                        console.log(err.status + " - " + err.statusText);
                    }
                });
            }
        };
        self.spectacleList = ko.observableArray(self.spectaclesToVm(window.__pageModel.Data || []));
        self.hasPrevious = ko.observable(window.__pageModel.HasPrevious);
        self.hasNext = ko.observable(window.__pageModel.HasNext);
        /* Переменные */

        self.openSpectacleModal = function (spectacle) {
            self.spectacleModalModel.id(spectacle.id());
            self.spectacleModalModel.name(spectacle.name());
            self.spectacleModalModel.startDate(spectacle.startDate());
            self.spectacleModalModel.duration(spectacle.duration());
            self.spectacleModalModel.ticketCount(spectacle.ticketCount());
            $('input[name="spectacleStartDate"]').data('daterangepicker').setStartDate(spectacle.startDate());
            $('input[name="spectacleStartDate"]').data('daterangepicker').setEndDate(spectacle.endDate());
            $("#idSpectacleDetailModal").modal('show');
        };

        self.addSpectacle = function () {
            self.openSpectacleModal(new SpectacleListItem());
        };

        self.changeSpectacle = function (row) {
            self.openSpectacleModal(row);
        };

        self.bookTicket = function (row) {
            $.ajax({
                type: "POST",
                url: Url.Action("BookTicket", "Home"),
                data: {
                    showId: row.id(),
                },
                success: function (data) {
                    alert('Вы успешно забронировали билет');
                },
                error: function (err) {
                    alert('Невозможно забронировать билет, возможно нет мест');
                }
            });
        };
    }
    ko.applyBindings(viewModel);
}