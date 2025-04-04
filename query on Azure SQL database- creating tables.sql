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
    EventDate DATE NOT NULL,
    Description TEXT,
    VenueID INT NOT NULL,
    CONSTRAINT FK_Event_Venue FOREIGN KEY (VenueID) REFERENCES Venue(VenueID) 
       
);

CREATE TABLE Booking (
    BookingID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL,
    VenueID INT NOT NULL,
    BookingDate DATE NOT NULL,
    CONSTRAINT FK_Booking_Event FOREIGN KEY (EventID) REFERENCES Event(EventID),
    CONSTRAINT FK_Booking_Venue FOREIGN KEY (VenueID) REFERENCES Venue(VenueID) 
          
);