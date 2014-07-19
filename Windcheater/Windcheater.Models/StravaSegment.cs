using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windcheater.Models
{
    public class StravaSegment<T> where T : StravaSegmentEffort
    {
        public int Id { get; private set; }
        public Location StartLocation { get; private set; }
        public IReadOnlyCollection<T> Leaderboard { get; private set; }

        public StravaSegment(int id, Location startLocation, IEnumerable<T> leaderboard)
        {
            Id = id;
            StartLocation = startLocation;
            Leaderboard = new List<T>(leaderboard);
        }

        public StravaSegment<T2> Upcast<T2>(IEnumerable<T2> leaderboard) where T2 : StravaSegmentEffort
        {
            return new StravaSegment<T2>(this.Id, this.StartLocation, leaderboard);
        }
    }
}
