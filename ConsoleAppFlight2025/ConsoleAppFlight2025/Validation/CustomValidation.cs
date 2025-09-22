using AirlineManagement.Model;
using System;

namespace AirlineManagement.Validation
{
    public static class CustomValidation
    {
        // Airport code should be 3-10 characters and not empty
        public static bool IsValidAirportCode(string code)
        {
            return !string.IsNullOrWhiteSpace(code) && code.Length >= 3 && code.Length <= 10;
        }

        public static bool AreAirportsDifferent(string depAirport, string arrAirport)
        {
            return !string.Equals(depAirport, arrAirport, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsValidDate(DateTime date)
        {
            return date.Date >= DateTime.Today;
        }

        public static bool IsValidTime(TimeSpan time)
        {
            return time >= TimeSpan.Zero && time <= TimeSpan.FromHours(23).Add(TimeSpan.FromMinutes(59));
        }

        public static bool IsArrivalAfterDeparture(Flight flight)
        {
            DateTime dep = flight.DepDate.Date + flight.DepTime;
            DateTime arr = flight.ArrDate.Date + flight.ArrTime;
            return arr > dep;
        }

        public static bool IsValidFlight(Flight flight, out string error)
        {
            if (!IsValidAirportCode(flight.DepAirport))
            {
                error = "Invalid departure airport code.";
                return false;
            }
            if (!IsValidAirportCode(flight.ArrAirport))
            {
                error = "Invalid arrival airport code.";
                return false;
            }
            if (!AreAirportsDifferent(flight.DepAirport, flight.ArrAirport))
            {
                error = "Departure and arrival airports cannot be the same.";
                return false;
            }
            if (!IsValidDate(flight.DepDate))
            {
                error = "Departure date must be today or later.";
                return false;
            }
            if (!IsValidDate(flight.ArrDate))
            {
                error = "Arrival date must be today or later.";
                return false;
            }
            if (!IsValidTime(flight.DepTime))
            {
                error = "Invalid departure time.";
                return false;
            }
            if (!IsValidTime(flight.ArrTime))
            {
                error = "Invalid arrival time.";
                return false;
            }
            if (!IsArrivalAfterDeparture(flight))
            {
                error = "Arrival must be after departure.";
                return false;
            }

            error = null;
            return true;
        }
    }
}
