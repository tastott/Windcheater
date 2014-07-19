using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windcheater.Models
{
    public class StravaSegmentEffort
    {
        public int Rank { get; private set; }
        public string AthleteName { get; private set; }
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// Moving time in seconds.
        /// </summary>
        public int MovingTime { get; private set; }

        public StravaSegmentEffort(int rank, string athleteName, DateTime startTime, int movingTime)
        {
            Rank = rank;
            AthleteName = athleteName;
            StartTime = startTime;
            MovingTime = movingTime;
        }
    }
}
