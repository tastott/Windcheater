using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windcheater.Models
{
    public class WeatherObservation
    {
        public string Source { get; private set; }
        public string SiteName { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude {get; private set;}
        public double WindSpeed { get; private set; }
        public double WindBearing { get; private set; }

        public WeatherObservation(string source, string siteName, double latitude, double longitude, double windSpeed, double windBearing)
        {
            Source = source;
            SiteName = siteName;
            Latitude = latitude;
            Longitude = longitude;
            WindSpeed = windSpeed;
            WindBearing = windBearing;
        }
    }

    /*type WeatherObservation = 
    {   Source : string;
        SiteName : string;
        Latitude : float<degree>;
        Longitude : float<degree>;
        WindSpeed : float<meter/second>;
        WindBearing : float<radian>; }*/
}
