if (window.ticketOfficeOptions != null && window.ticketOfficeOptions != undefined) {

    function TicketListItem(ticket) {
        var self = this;
        ticket = ticket || { id: null, userId: null };
        self.id = ko.observable(ticket.id);
        self.userId = ko.observable(ticket.userId);
    };

    function SpectacleListItem(spectacle) {
        var self = this;
        spectacle = spectacle || { id: null, name: "", showDate: moment(), duration: moment.duration('02:00:00'), ticketCount: null, tickets: [] };
        self.id = ko.observable(spectacle.id);
        self.name = ko.observable(spectacle.name);
        self.startDate = ko.observable(moment(spectacle.showDate));
        self.duration = ko.observable(moment.duration(spectacle.duration));
        self.ticketCount = ko.observable(spectacle.ticketCount);
        self.endDate = ko.computed(function () {
            return self.startDate().clone().add(self.duration().hours(), 'hours').add(self.duration().minutes(), 'minutes');
        });
        self.ticketsToVm = function (tickets) {
            return ko.utils.arrayMap(tickets, function (ticket) {
                return new TicketListItem(ticket);
            });
        };
        self.tickets = ko.observableArray(self.ticketsToVm(spectacle.tickets));
        self.availableTicketCount = ko.computed(function () {
            return self.ticketCount() - self.tickets().length;
        });
        self.userHasTicket = ko.computed(function () {
            return self.tickets().filter(function (ticket) {
                return ticket.userId() === window.ticketOfficeOptions.currentUser.id;
            }).length > 0;
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
                $.post(Url.Action("CreateOrUpdateShow", "Shows"), {
                    id: self.spectacleModalModel.id() || '00000000-0000-0000-0000-000000000000',
                    name: self.spectacleModalModel.name(),
                    showDate: self.spectacleModalModel.startDate().format(),
                    duration: self.spectacleModalModel.duration().hours() + ':' + self.spectacleModalModel.duration().minutes(),
                    ticketCount: self.spectacleModalModel.ticketCount(),
                })
                    .done(function (changedSpectacle) {
                        var spectacle = self.spectacleList().filter(function (spectacle) {
                            return spectacle.id() === changedSpectacle.id;
                        })[0];

                        if (spectacle === undefined) {
                            // add
                            self.spectacleList.push(new SpectacleListItem(changedSpectacle));
                        }
                        else {
                            // update
                            spectacle.name(changedSpectacle.name);
                            spectacle.startDate(moment(changedSpectacle.showDate));
                            spectacle.duration(moment.duration(changedSpectacle.duration));
                            spectacle.ticketCount(changedSpectacle.ticketCount);
                        }
                        $("#idSpectacleDetailModal").modal('hide');
                    })
                    .fail(function () {
                        alert("Ошибка запси спектакля");
                    });
            }
        };
        self.spectacleList = ko.observableArray(self.spectaclesToVm([]));
        self.hasPrevious = ko.observable(false);
        self.hasNext = ko.observable(false);

        self.currentUserId = ko.observable(window.ticketOfficeOptions.currentUser.id);
        self.isAuthenticated = ko.observable(window.ticketOfficeOptions.currentUser.isAuthenticated);
        self.isInRoleAdmin = ko.observable(window.ticketOfficeOptions.currentUser.isInRoleAdmin);
        self.baseUrl = ko.observable(window.ticketOfficeOptions.baseUrl);
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
            if (!self.isAuthenticated())
                alert("Войдите в систему");
            $.post(Url.Action("BookTicket", "Tickets"), { showId: row.id() })
                .done(function (newTicket) {
                    alert('Вы успешно забронировали билет');
                    row.tickets.push(new TicketListItem(newTicket));
                })
                .fail(function () {
                    alert('Невозможно забронировать билет, возможно нет мест');
                });
        };
        self.unbookTicket = function (row) {
            if (!self.isAuthenticated())
                alert("Войдите в систему");

            var ticket = row.tickets().filter(function (ticket) {
                return ticket.userId() === self.currentUserId();
            })[0];

            $.post(Url.Action("UnbookTicket", "Tickets"), { ticketId: ticket.id() })
                .done(function (newTicket) {
                    alert('Вы успешно отменили бронь билета');
                    row.tickets.remove(ticket);
                })
                .fail(function () {
                    alert('Невозможно отменить бронь билета');
                });
        };

        self.paging = {
            currentPage: ko.observable(0),
            hasNext: ko.observable(false),
            hasPrevious: ko.observable(false),
            totalCount: ko.observable(0),
            totalPages: ko.observable(0)
        };
        self.filter = {
            fromDate: ko.observable(),
            toDate: ko.observable(),
            showName: ko.observable()
        }

        self.init = function () {
            $.getJSON(Url.Action("Index", "Shows"), function (data) {
                self.spectacleList(self.spectaclesToVm(data.data));
                self.paging.currentPage(data.pageIndex);
                self.paging.hasNext(data.hasNext);
                self.paging.hasPrevious(data.hasPrevious);
                self.paging.totalCount(data.totalCount);
                self.paging.totalPages(data.totalPages);
            });
        };
        self.init();
        //$('#idAlertModal').alert();
    };
    var spectacleModel = function () {

    };
    ko.applyBindings(viewModel);
}