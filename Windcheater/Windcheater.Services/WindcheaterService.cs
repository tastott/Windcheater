using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windcheater.Services
{
    using Models;

    public class WindcheaterService
    {
        private StravaService _stravaService;
        private IWeatherService _weatherService;

        public WindcheaterService(StravaService stravaService, IWeatherService weatherService)
        {
            _stravaService = stravaService;
            _weatherService = weatherService;
        }

        public StravaSegment<WeatheredStravaSegmentEffort> GetStravaSegmentWithWeather(int segmentId)
        {
            var segment = _stravaService.GetSegment(segmentId);

            var weatheredLeaderboard = segment.Leaderboard.Select(effort =>
            {
                var weather = _weatherService.GetWeather(segment.StartLocation, effort.StartTime);

                return new WeatheredStravaSegmentEffort(effort, weather);
            });

            return segment.Upcast<WeatheredStravaSegmentEffort>(weatheredLeaderboard);
        }
    }
}
