using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.strava.api.Authentication;
using com.strava.api.Client;

namespace Windcheater.Services
{
    using Models;

    public class StravaService
    {
        private StravaClient _client;

        public StravaService(string stravaApiKey)
        {
            var stravaAuth = new StaticAuthentication(stravaApiKey);
            _client = new StravaClient(stravaAuth);
        }

        public StravaSegment<StravaSegmentEffort> GetSegment(int segmentId)
        {
            string segmentIdString = segmentId.ToString();

            var apiSegment = _client.Segments.GetSegment(segmentIdString);
            var apiLeaderboard = _client.Segments.GetSegmentLeaderboard(segmentIdString, 1, 10);

            var startLocation = new Location(apiSegment.StartCoordinates[0], apiSegment.StartCoordinates[1]);

            var leaderboard = apiLeaderboard.Entries.Select(apiEntry =>
            {
                var startDate = DateTime.Parse(apiEntry.StartDate);
                var movingTime = TimeSpan.FromSeconds(apiEntry.MovingTime);

                return new StravaSegmentEffort(apiEntry.Rank, apiEntry.AthleteName, startDate, movingTime);
            });

            return new StravaSegment<StravaSegmentEffort>(segmentId, startLocation, leaderboard);
        }
    }
}