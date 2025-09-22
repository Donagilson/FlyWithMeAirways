using System;

namespace AirlineManagement.Model
{
    public class Flight
    {
        public int FlightId { get; set; }
        public string DepAirport { get; set; }
        public DateTime DepDate { get; set; }
        public TimeSpan DepTime { get; set; }
        public string ArrAirport { get; set; }
        public DateTime ArrDate { get; set; }
        public TimeSpan ArrTime { get; set; }
    }
}
