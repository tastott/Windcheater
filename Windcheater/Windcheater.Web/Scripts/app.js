require(['bootstrap'])

require(['jquery', 'knockout', 'moment', 'wind-widget'], function ($, ko, moment) {

    ko.bindingHandlers.timeText = {
        update: function (element, valueAccessor, allBindingsAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());

            var formattedValue = moment(new Date(value*1000)).format('mm:ss');

            ko.bindingHandlers.text.update(element, function () { return formattedValue; });
        }
    };

    var appVm = new AppViewModel();
    ko.applyBindings(appVm);

    function AppViewModel() {
        var _this = this;

        this.SegmentId = ko.observable(3752866);
        this.Segment = ko.observable();

        this.GetStravaSegment = function () {

            //require(['json!../Content/example.json'], function (segment) {
            //    _this.Segment(segment);
            //});

            $.getJSON('/Home/GetStravaSegmentWithWeather?id=' + _this.SegmentId(), function (response) {
                if (response.success) {

                    _this.Segment(response.data);


                } else {
                    alert("Failed to get strava segment");
                }
            });

        }
    }
});

