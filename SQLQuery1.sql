DROP DATABASE EventEaseDb;

CREATE database EventEaseDb;

use EventEaseDb;

CREATE TABLE Venue (
    VenueID INT IDENTITY(1,1) PRIMARY KEY,
    VenueName VARCHAR(100) NOT NULL,
    Location VARCHAR(255) NOT NULL,
    Capacity INT NOT NULL,
    ImageUrl NVARCHAR(255) NOT NULL
);

CREATE TABLE EventType(
    EventTypeID INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(100) NOT NULL
	);

CREATE TABLE Event (
    EventID INT IDENTITY(1,1) PRIMARY KEY,
    EventName NVARCHAR(255) NOT NULL,
    EventDate DATETIME NOT NULL,
    Description NVARCHAR(1000),
    VenueID INT NOT NULL,
	EventTypeID INT NULL,
    FOREIGN KEY (VenueID) REFERENCES Venue(VenueID) ON DELETE CASCADE,
	FOREIGN KEY (EventTypeID) REFERENCES EventType(EventTypeID) ON DELETE SET NULL
	
       
);
CREATE TABLE Booking (
    BookingID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL,
    VenueID INT NOT NULL,
    BookingDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (EventID) REFERENCES Event(EventID) ON DELETE CASCADE,
	FOREIGN KEY (VenueID) REFERENCES Venue(VenueID) ON DELETE NO ACTION,
	CONSTRAINT UQ_Venue_Event UNIQUE (VenueID,EventID)
          
);

CREATE UNIQUE INDEX UQ_Venue_Booking ON Booking(VenueID,BookingDate);

INSERT INTO Venue (VenueName, Location, Capacity, ImageUrl)
VALUES
('Convention Center', '123 Main St, Cape Town', 500, 'images/convention.jpg'),
('Sunset Hall', '45 Ocean View, Durban', 200, 'images/sunset.jpg'),
('Business Hub', '67 Sandton Drive, Johannesburg', 300, 'images/hub.jpg'),
('Grand Arena', '89 Music Lane, Pretoria', 1000, 'images/arena.jpg'),
('Botanical Gardens', '22 Green Way, Bloemfontein', 150, 'images/gardens.jpg');

INSERT INTO EventType (name)
VALUES
('Conference'),
('Wedding'),
('Naming'),
('Birthday'),
('Concert');

Insert INTO EVENT (EventName, EventDate,Description, VenueID, EventTypeID)
VALUES
('Tech Confrence 2025','2025-05-15 09:00:00','Annual conference on technology and innovations.',1,1),
('Wedding Reception - Johnson','2025-06-20 18:00:00','Celebration of the marraige between Sarah and John Johnson.',2,2),
('Buisness Seminar','2025-07-20 14:00:00', 'Seminar on buisness management and strategy.',3,3),
('Music Concert', '2025-08-25 19:00:00','Live music concert featuring popular bands.',4,4),
('Garden Party','2025-09-12 15:00:00','Outdoor garden party with refreshments and entertainment.',5,5);
