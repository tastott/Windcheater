using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windcheater.Services
{
    using System.Device.Location;
    using System.Net;
    using System.Text.RegularExpressions;
    using Models;
    using Windcheater.Services.DataGovUKMetOfficeWeatherOpenData;

    public class MetOfficeWeatherService : IWeatherService
    {
        private static IDictionary<string, double> compassBearings;
        private static decimal proximityThreshold = 0.5M;

        static MetOfficeWeatherService()
        {
            compassBearings = new Dictionary<string, double>();
            var directions = new string[] { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW" };

            for (int i = 0; i < 16; i++)
            {
                compassBearings[directions[0]] = Math.PI * i / 8;
            }
        }
  
        private int _maxSites = 10;
        private int _timeoutSeconds = 180;

        private DataGovUKMetOfficeWeatherOpenDataContainer webService;

        public MetOfficeWeatherService(string azureDataMarketUsername, string azureDataMarketApiKey)
        {
            webService = new DataGovUKMetOfficeWeatherOpenDataContainer(new Uri("https://api.datamarket.azure.com/DataGovUK/MetOfficeWeatherOpenData/v1/"));
            webService.IgnoreMissingProperties = true;
            webService.Timeout = _timeoutSeconds;
            webService.Credentials = new NetworkCredential(azureDataMarketUsername, azureDataMarketApiKey);
        }


        public WeatherObservation GetWeather(Location location, DateTime dateTime)
        {
            //Get up to [_maxSites] near-ish sites with server-side filter
            var nearSitesx = webService.Site.Where
                (
                    s =>
                    (s.Latitude.Value - (decimal)location.Latitude) < proximityThreshold
                    && (s.Latitude.Value - (decimal)location.Latitude) > -proximityThreshold
                    && (s.Longitude.Value - (decimal)location.Longitude) < proximityThreshold
                    && (s.Longitude.Value - (decimal)location.Longitude) > -proximityThreshold
                    && s.ID < 99999 //Dubious filter for observation sites
                );


            var nearSites = nearSitesx.ToList()
                .OrderBy(s => SiteDistance(location, s))
                .Take(_maxSites)
                .ToDictionary(s => s.Name);

            if (!nearSites.Any()) return null;

            //Get observations for these sites and pick closest
            var siteNameFilter = String.Join(" or ", nearSites.Keys.Select(s => String.Format("(SiteName eq '{0}')", s)));
            var dateFilter = String.Format("ObservationDate eq datetime'{0}'", dateTime.Date.ToString("yyyy-MM-dd"));
            var timeFilter = String.Format("ObservationTime eq {0}", dateTime.Hour);
            var filter = String.Format("({0}) and ({1}) and ({2})", siteNameFilter, dateFilter, timeFilter);

            var nearestObservation = webService.Observation.AddQueryOption("$filter", filter)
                .ToList()
                .OrderBy(o => SiteDistance(location, nearSites[o.SiteName]))
                .FirstOrDefault();


            if (nearestObservation == null) return null;
            else
            {
                if (!nearestObservation.WindDirection.HasValue || nearestObservation.WindDirection.Value < 0 || nearestObservation.WindDirection.Value > 15)
                    throw new Exception("Unexpected wind direction value");
                double windBearing = nearestObservation.WindDirection.Value * Math.PI / 8;

                return new WeatherObservation(source: "Met Office",
                    siteName: CleanSiteName(nearestObservation.SiteName),
                    latitude: (double)nearestObservation.Latitude.Value,
                    longitude: (double)nearestObservation.Longitude.Value,
                    windSpeed: (double)nearestObservation.WindSpeed.Value,
                    windBearing: windBearing
                );
            }
        }

        private double SiteDistance(Location location, Site s)
        {
            var from = new GeoCoordinate(location.Latitude, location.Longitude);
            var to = new GeoCoordinate((double)s.Latitude.Value, (double)s.Longitude.Value);

            return from.GetDistanceTo(to);
        }

        private static string CleanSiteName(string originalName)
        {
            var siteIdRegex = new Regex(@"\([0-9]+\)");
            var cleanName = siteIdRegex.Replace(originalName, "");

            var wordRegex = new Regex(@"\S+");
            MatchEvaluator properCaseEvaluator = match => match.Value.Substring(0, 1).ToUpper() + match.Value.Substring(1).ToLower();
            cleanName = wordRegex.Replace(cleanName, properCaseEvaluator);

            return cleanName.Trim();
        }
    }
}
