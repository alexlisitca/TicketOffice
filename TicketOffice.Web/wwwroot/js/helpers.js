var Url = {
    Action: function (action, controller, attrs) {
        window.ticketOfficeOptions.baseUrl = window.ticketOfficeOptions.baseUrl.replace(/\/$/, "");
        var arr = [];
        arr.push(window.ticketOfficeOptions.baseUrl);
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
