using AirlineManagement.Model;
using AirlineManagement.Repository;
using AirlineManagement.Service;
using AirlineManagement.Validation;
using System;
using System.Threading.Tasks;

namespace AirlineManagement
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            AdminRepository adminRepo = new AdminRepository();

            PrintHeader();

            Console.Write("Enter Admin Username: ");
            string username = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            if (!adminRepo.Login(username, password))
            {
                Console.WriteLine("Invalid credentials! Exiting...");
                return;
            }

            Console.WriteLine("Login successful! Press any key to continue...");
            Console.ReadKey();

            IRepository<Flight> flightRepo = new FlightRepositoryImpl();
            IService<Flight> flightService = new FlightServiceImpl(flightRepo);


            //for showing up the details in choice
            while (true)
            {
                Console.Clear();
                PrintHeader();
                Console.WriteLine("1. List All Flights");
                Console.WriteLine("2. Search Flight by Id");
                Console.WriteLine("3. Add Flight");
                Console.WriteLine("4. Update Flight");
                Console.WriteLine("5. Delete Flight");
                Console.WriteLine("6. Exit");
                Console.Write("Select option: ");

                string option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        await ListAllFlightsAsync(flightService);
                        break;

                    case "2":
                        await SearchFlightByIdAsync(flightService);
                        break;

                    case "3":
                        await AddFlightAsync(flightService);
                        break;

                    case "4":
                        await UpdateFlightAsync(flightService);
                        break;

                    case "5":
                        await DeleteFlightAsync(flightService);
                        break;

                    case "6":
                        return;

                    default:
                        Console.WriteLine("Invalid option!");
                        Console.ReadKey();
                        break;
                }
            }
        }
        //printing the heading
        private static void PrintHeader()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("=== Airline Management System ===".PadRight(Console.WindowWidth));
            Console.ResetColor();
            Console.WriteLine();
        }

        //for listing all flights
        private static async Task ListAllFlightsAsync(IService<Flight> service)
        {
            var flights = await service.GetAllAsync();

            if (flights.Count == 0)
            {
                Console.WriteLine("No flights found.");
                Console.ReadKey();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-------------------------------------------------------------------------------------------------");
            Console.WriteLine("| {0,-5} | {1,-15} | {2,-10} | {3,-5} | {4,-15} | {5,-10} | {6,-5} |",
                              "ID", "Departure Airport", "Dep Date", "DepT", "Arrival Airport", "Arr Date", "ArrT");
            Console.WriteLine("-------------------------------------------------------------------------------------------------");
            Console.ResetColor();

            foreach (var f in flights)
            {
                Console.WriteLine("| {0,-5} | {1,-15} | {2,-10:yyyy-MM-dd} | {3,-5:hh\\:mm} | {4,-15} | {5,-10:yyyy-MM-dd} | {6,-5:hh\\:mm} |",
                                  f.FlightId, f.DepAirport, f.DepDate, f.DepTime, f.ArrAirport, f.ArrDate, f.ArrTime);
            }

            Console.WriteLine("-------------------------------------------------------------------------------------------------");
            Console.ReadKey();
        }

        //For Search by Id

        private static async Task SearchFlightByIdAsync(IService<Flight> service)
        {
            Console.Write("Enter Flight Id: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var flight = await service.GetByIdAsync(id);
                if (flight != null)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("-------------------------------------------------------------------------------------------------");
                    Console.WriteLine("| {0,-5} | {1,-15} | {2,-10} | {3,-5} | {4,-15} | {5,-10} | {6,-5} |",
                                      "ID", "Departure Airport", "Dep Date", "DepT", "Arrival Airport", "Arr Date", "ArrT");
                    Console.WriteLine("-------------------------------------------------------------------------------------------------");
                    Console.ResetColor();

                    Console.WriteLine("| {0,-5} | {1,-15} | {2,-10:yyyy-MM-dd} | {3,-5:hh\\:mm} | {4,-15} | {5,-10:yyyy-MM-dd} | {6,-5:hh\\:mm} |",
                                      flight.FlightId, flight.DepAirport, flight.DepDate, flight.DepTime, flight.ArrAirport, flight.ArrDate, flight.ArrTime);
                    Console.WriteLine("-------------------------------------------------------------------------------------------------");
                }
                else
                {
                    Console.WriteLine("Flight not found!");
                }
            }
            Console.ReadKey();
        }



        private static async Task AddFlightAsync(IService<Flight> flightService)
        {
            Flight flight = GetFlightDetailsFromUser();

            IAirportRepository airportRepo = new AirportRepositoryImpl();

            // Ensure departure airport exists
            var depInfo = await airportRepo.GetByCodeAsync(flight.DepAirport);
            if (depInfo == null)
            {
                Console.WriteLine($"Departure airport '{flight.DepAirport}' not found. Please enter details to add it:");
                Console.Write("City: ");
                string depCity = Console.ReadLine();
                Console.Write("Country: ");
                string depCountry = Console.ReadLine();

                await airportRepo.AddAsync(new Airport
                {
                    AirportCode = flight.DepAirport,
                    City = depCity,
                    Country = depCountry
                });
                Console.WriteLine("New departure airport added.");
            }

            // Ensure arrival airport exists
            var arrInfo = await airportRepo.GetByCodeAsync(flight.ArrAirport);
            if (arrInfo == null)
            {
                Console.WriteLine($"Arrival airport '{flight.ArrAirport}' not found. Please enter details to add it:");
                Console.Write("City: ");
                string arrCity = Console.ReadLine();
                Console.Write("Country: ");
                string arrCountry = Console.ReadLine();

                await airportRepo.AddAsync(new Airport
                {
                    AirportCode = flight.ArrAirport,
                    City = arrCity,
                    Country = arrCountry
                });
                Console.WriteLine("New arrival airport added.");
            }

            if (CustomValidation.IsValidFlight(flight, out string error))
            {
                await flightService.AddAsync(flight);
                Console.WriteLine("Flight added successfully!");
            }
            else
            {
                Console.WriteLine($"Invalid flight details: {error}");
            }

            Console.ReadKey();
        }



        private static async Task UpdateFlightAsync(IService<Flight> service)
        {
            Console.Write("Enter Flight Id to update: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var flight = await service.GetByIdAsync(id);
                if (flight != null)
                {
                    flight = GetFlightDetailsFromUser(flight);
                    if (CustomValidation.IsValidFlight(flight, out string error))
                    {
                        await service.UpdateAsync(flight);
                        Console.WriteLine("Flight updated successfully!");
                    }
                    else
                    {
                        Console.WriteLine($"Invalid flight details: {error}");
                    }
                }
                else
                {
                    Console.WriteLine("Flight not found!");
                }
            }
            Console.ReadKey();
        }

        private static async Task DeleteFlightAsync(IService<Flight> service)
        {
            Console.Write("Enter Flight Id to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                await service.DeleteAsync(id);
                Console.WriteLine("Flight deleted successfully!");
            }
            Console.ReadKey();
        }

        private static Flight GetFlightDetailsFromUser(Flight flight = null)
        {
            flight ??= new Flight();

            Console.Write("Enter Departure Airport: ");
            flight.DepAirport = Console.ReadLine();

            // Departure Date
            while (true)
            {
                Console.Write("Enter Departure Date (yyyy-MM-dd): ");
                string depDateStr = Console.ReadLine();
                if (DateTime.TryParse(depDateStr, out DateTime depDate))
                {
                    flight.DepDate = depDate;
                    break;
                }
                else
                {
                    Console.WriteLine("Incorrect date format! Please enter in yyyy-MM-dd format.");
                }
            }

            // Departure Time
            while (true)
            {
                Console.Write("Enter Departure Time (HH:mm): ");
                string depTimeStr = Console.ReadLine();
                if (TimeSpan.TryParse(depTimeStr, out TimeSpan depTime))
                {
                    flight.DepTime = depTime;
                    break;
                }
                else
                {
                    Console.WriteLine("Incorrect time format! Please enter in HH:mm format.");
                }
            }

            Console.Write("Enter Arrival Airport: ");
            flight.ArrAirport = Console.ReadLine();

            // Arrival Date
            while (true)
            {
                Console.Write("Enter Arrival Date (yyyy-MM-dd): ");
                string arrDateStr = Console.ReadLine();
                if (DateTime.TryParse(arrDateStr, out DateTime arrDate))
                {
                    flight.ArrDate = arrDate;
                    break;
                }
                else
                {
                    Console.WriteLine("Incorrect date format! Please enter in yyyy-MM-dd format.");
                }
            }

            // Arrival Time
            while (true)
            {
                Console.Write("Enter Arrival Time (HH:mm): ");
                string arrTimeStr = Console.ReadLine();
                if (TimeSpan.TryParse(arrTimeStr, out TimeSpan arrTime))
                {
                    flight.ArrTime = arrTime;
                    break;
                }
                else
                {
                    Console.WriteLine("Incorrect time format! Please enter in HH:mm format.");
                }
            }

            return flight;
        }

    }
}
