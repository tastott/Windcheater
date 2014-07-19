using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.strava.api.Authentication;
using com.strava.api.Client;

namespace Windcheater.Web.Services
{
    public class StravaService
    {
        private StravaClient _client;

        public StravaService(string stravaApiKey)
        {
            var stravaAuth = new StaticAuthentication(stravaApiKey);
            _client = new StravaClient(stravaAuth);
        }

        public StravaSegment GetSegment(int segmentId)
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

            return new StravaSegment(segmentId, startLocation, leaderboard);
        }
    }

    public class Location
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }

    public class StravaSegmentEffort
    {
        public int Rank { get; private set; }
        public string AthleteName { get; private set; }
        public DateTime StartTime { get; private set; }
        public TimeSpan MovingTime { get; private set; }

        public StravaSegmentEffort(int rank, string athleteName, DateTime startTime, TimeSpan movingTime)
        {
            Rank = rank;
            AthleteName = athleteName;
            StartTime = startTime;
            MovingTime = movingTime;
        }
    }

    public class StravaSegment
    {
        public int Id { get; private set; }
        public Location StartLocation { get; private set; }
        public IReadOnlyCollection<StravaSegmentEffort> Leaderboard { get; private set; }

        public StravaSegment(int id, Location startLocation, IEnumerable<StravaSegmentEffort> leaderboard)
        {
            Id = id;
            StartLocation = startLocation;
            Leaderboard = new List<StravaSegmentEffort>(leaderboard);
        }
    }
}