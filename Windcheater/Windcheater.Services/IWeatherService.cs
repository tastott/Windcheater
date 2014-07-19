using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Windcheater.Services
{
    using Models;

    public interface IWeatherService
    {
        WeatherObservation GetWeather(Location location, DateTime dateTime);
    }
}