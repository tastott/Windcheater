using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windcheater.Models
{
    public class WeatheredStravaSegmentEffort : StravaSegmentEffort
    {
        public WeatherObservation Weather { get; private set; }

        public WeatheredStravaSegmentEffort(StravaSegmentEffort effort, WeatherObservation weather)
            : base(effort.Rank, effort.AthleteName, effort.StartTime, effort.MovingTime)
        {
            Weather = weather;
        }
    }
}
