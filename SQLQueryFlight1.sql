Create Database Flight2025
use Flight2025;


--table for ADMINISTRATOR

CREATE TABLE Administrator (
    AdminId INT PRIMARY KEY IDENTITY(1,1),
    Username VARCHAR(50) UNIQUE NOT NULL,
    Password VARCHAR(255) 
);
select * from administrator

--table for AIRPORT
CREATE TABLE Airport (
    AirportCode VARCHAR(10) PRIMARY KEY,  
    City VARCHAR(50) NOT NULL,
    Country VARCHAR(50) NOT NULL
);

--table for Flight
CREATE TABLE Flight (
    FlightId INT PRIMARY KEY IDENTITY(1,1),
    DepAirport VARCHAR(10) NOT NULL FOREIGN KEY REFERENCES Airport(AirportCode),
    DepDate DATE NOT NULL,
    DepTime TIME NOT NULL,
    ArrAirport VARCHAR(10) NOT NULL FOREIGN KEY REFERENCES Airport(AirportCode),
    ArrDate DATE NOT NULL,
    ArrTime TIME NOT NULL
);

-- Insert into Administrator 
INSERT INTO Administrator (Username, Password)
VALUES 
('admin1', 'Admin@123'),
('admin2', 'Secure2025'),
('superadmin', 'FlyWithMe#99');



-- Insert into Airport 
INSERT INTO Airport (AirportCode, City, Country)
VALUES
('DEL', 'New Delhi', 'India'),
('BOM', 'Mumbai', 'India'),
('DXB', 'Dubai', 'UAE');

-- Insert into Flight  
INSERT INTO Flight (DepAirport, DepDate, DepTime, ArrAirport, ArrDate, ArrTime)
VALUES
('DEL', '2025-10-01', '08:30:00', 'BOM', '2025-10-01', '10:45:00'),
('BOM', '2025-10-02', '22:00:00', 'DXB', '2025-10-03', '00:30:00'),
('DXB', '2025-10-05', '14:15:00', 'DEL', '2025-10-05', '18:45:00');

select * from Administrator
select * from Airport
select * from  Flight



--Procedure for listallflights
CREATE PROCEDURE sp_ListAllFlights
AS
BEGIN
    SELECT 
        f.FlightId,
        f.DepAirport,
        a1.City AS DepartureCity,
        f.DepDate,
        f.DepTime,
        f.ArrAirport,
        a2.City AS ArrivalCity,
        f.ArrDate,
        f.ArrTime
    FROM Flight f
    INNER JOIN Airport a1 ON f.DepAirport = a1.AirportCode
    INNER JOIN Airport a2 ON f.ArrAirport = a2.AirportCode;
END;

--procedure for searchflight byid
CREATE PROCEDURE sp_SearchFlightById
    @FlightId INT
AS
BEGIN
    SELECT 
        f.FlightId,
        f.DepAirport,
        a1.City AS DepartureCity,
        f.DepDate,
        f.DepTime,
        f.ArrAirport,
        a2.City AS ArrivalCity,
        f.ArrDate,
        f.ArrTime
    FROM Flight f
    INNER JOIN Airport a1 ON f.DepAirport = a1.AirportCode
    INNER JOIN Airport a2 ON f.ArrAirport = a2.AirportCode
    WHERE f.FlightId = @FlightId;
END;

--procedure for add flight
CREATE PROCEDURE sp_AddFlight
    @DepAirport VARCHAR(10),
    @DepDate DATE,
    @DepTime TIME,
    @ArrAirport VARCHAR(10),
    @ArrDate DATE,
    @ArrTime TIME
AS
BEGIN
    INSERT INTO Flight (DepAirport, DepDate, DepTime, ArrAirport, ArrDate, ArrTime)
    VALUES (@DepAirport, @DepDate, @DepTime, @ArrAirport, @ArrDate, @ArrTime);
    
    SELECT SCOPE_IDENTITY() AS NewFlightId; -- Return new FlightId
END;


CREATE PROCEDURE sp_UpdateFlight
    @FlightId INT,
    @DepAirport VARCHAR(10),
    @DepDate DATE,
    @DepTime TIME,
    @ArrAirport VARCHAR(10),
    @ArrDate DATE,
    @ArrTime TIME
AS
BEGIN
    UPDATE Flight
    SET 
        DepAirport = @DepAirport,
        DepDate = @DepDate,
        DepTime = @DepTime,
        ArrAirport = @ArrAirport,
        ArrDate = @ArrDate,
        ArrTime = @ArrTime
    WHERE FlightId = @FlightId;
END;

-- Delete all flights
DELETE FROM Flight;

-- Reset identity
DBCC CHECKIDENT ('Flight', RESEED, 0);

-- Insert new records
INSERT INTO Flight (DepAirport, DepDate, DepTime, ArrAirport, ArrDate, ArrTime)
VALUES
('DEL', '2025-10-01', '08:30:00', 'BOM', '2025-10-01', '10:45:00'),
('BOM', '2025-10-02', '22:00:00', 'DXB', '2025-10-03', '00:30:00'),
('DXB', '2025-10-05', '14:15:00', 'DEL', '2025-10-05', '18:45:00');






