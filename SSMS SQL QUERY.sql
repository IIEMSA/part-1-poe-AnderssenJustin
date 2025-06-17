
CREATE TABLE Venue (
    VenueID INT IDENTITY(1,1) PRIMARY KEY,
    VenueName VARCHAR(100) NOT NULL,
    Location VARCHAR(255) NOT NULL,
    Capacity INT NOT NULL,
    ImageUrl VARCHAR(255) NOT NULL
);

CREATE TABLE Event (
    EventID INT IDENTITY(1,1) PRIMARY KEY,
    EventName VARCHAR(100) NOT NULL,
    EventDate DATETIME NOT NULL,
    Description TEXT,
    VenueID INT NOT NULL,
    CONSTRAINT FK_Event_Venue FOREIGN KEY (VenueID) REFERENCES Venue(VenueID) 
       
);

CREATE TABLE Booking (
    BookingID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL,
    VenueID INT NOT NULL,
    BookingDate DATETIME NOT NULL,
    CONSTRAINT FK_Booking_Event FOREIGN KEY (EventID) REFERENCES Event(EventID),
    CONSTRAINT FK_Booking_Venue FOREIGN KEY (VenueID) REFERENCES Venue(VenueID) 
          
);
INSERT INTO Venue (VenueName, Location, Capacity, ImageUrl)
VALUES ('Venue A', 'Location A', 500, 'url_to_image_A'),
       ('Venue B', 'Location B', 300, 'url_to_image_B');
INSERT INTO Event (EventName, EventDate, Description, VenueID)
VALUES ('Event 1', '2025-03-30', 'Description of Event 1', 1),
       ('Event 2', '2025-04-10', 'Description of Event 2', 2);

-- Check data in the Venue table
SELECT * FROM Venue;

-- Check data in the Event table
SELECT * FROM Event;

