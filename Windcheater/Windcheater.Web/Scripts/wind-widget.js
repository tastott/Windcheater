define(['jquery', 'knockout', 'jcanvas'], function ($, ko) {


    function draw(canvas, windSpeed, windBearing, diameter, colourIntervals) {
        

        var colour = '#000'; //default to black
        if (colourIntervals) {
            for (var i = 0; i < colourIntervals.length; i++) {
                if((colourIntervals[i].LowerBound == undefined || colourIntervals[i].LowerBound < windSpeed) 
                    && (colourIntervals[i].UpperBound == undefined || colourIntervals[i].UpperBound >= windSpeed)) {
                    colour = colourIntervals[i].Colour;
                    break;
                }
            }
        }

        var width = diameter * 2.5, height = diameter * 2.5;
        var x = width / 2, y = height / 2;
        var arrowlength = diameter;

        canvas.attr('width', width);
        canvas.attr('height', height);

        canvas.clearCanvas();

        canvas.attr('title', 'Speed: ' + windSpeed + ', Bearing: ' + windBearing);

        var strokeWidth = Math.floor(diameter / 5);

        //Arrow points to wind destination
        canvas.drawLine({
            strokeStyle: colour,
            strokeWidth: strokeWidth,
            //rounded: true,
            endArrow: true,
            arrowRadius: 12,
            arrowAngle: 90,
            x1: x, y1: y,
            x2: x - Math.sin(windBearing) * arrowlength, y2: y + Math.cos(windBearing) * arrowlength
        });

        var radial = canvas.createGradient({
            x1: x - 7, y1: y - 7,
            x2: x - 7, y2: y - 7,
            r1: diameter / 8, r2: diameter / 2,
            c1: '#aaa',
            c2: colour
        });

        canvas.drawArc({
            fillStyle: colour, //radial,
            //strokeStyle: '#000',
            //strokeWidth:3,
            x: x, y: y,
            radius: diameter / 2
        });
        
        var fontSize = Math.floor(diameter * 0.5);

        canvas.drawText({
            fillStyle: '#fff',
            //strokeStyle: '#fff',
            //strokeWidth: 1,
            x: x, y: y,
            fontSize: fontSize,
            fontFamily: 'Verdana, sans-serif',
            fontStyle: 'bold',
            text: windSpeed.toFixed(0)
        });
    }

    ko.bindingHandlers.windWidget = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var canvas = $('<canvas class="wind-widget"></canvas>').appendTo($(element));
        },
        update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var config = ko.unwrap(valueAccessor());

            var diameter = config.diameter || 36;

            var canvas = $(element).children('canvas.wind-widget');
            var colourIntervals = [
                { UpperBound: 5, Colour: '#0c0' },
                { LowerBound: 5, UpperBound: 20, Colour: '#c60' },
                { LowerBound: 20, Colour: '#f00' }
            ];

            if (config.windSpeed != undefined && config.windBearing != undefined) {
                draw(canvas, config.windSpeed, config.windBearing, diameter, colourIntervals);
            }
        }
    };
});