using System.Collections.Generic;

public class ParkingGarage
{
    public List<ParkingSpot> Spots { get; private set; }

    public ParkingGarage(int numberOfSpots)
    {
        Spots = new List<ParkingSpot>(numberOfSpots);
        for (int i = 1; i <= numberOfSpots; i++)
        {
            Spots.Add(new ParkingSpot(i));
        }
    }

    public bool ParkVehicle(Vehicle vehicle)
    {
        foreach (var spot in Spots)
        {
            if (spot.IsAvailable())
            {
                spot.ParkedVehicle = vehicle;
                return true; // Vehicle parked
            }
        }
        return false; // No available spots
    }

    public Vehicle RetrieveVehicle(int spotNumber)
    {
        var spot = Spots[spotNumber - 1];
        var vehicle = spot.ParkedVehicle;
        spot.ParkedVehicle = null; // Clear the spot
        return vehicle; // Return the retrieved vehicle
    }

    public void ShowStatus()
    {
        foreach (var spot in Spots)
        {
            string status = spot.IsAvailable() ? "Available" : $"Parked: {spot.ParkedVehicle.RegistrationNumber}";
            Console.WriteLine($"Spot {spot.SpotNumber}: {status}");
        }
    }
}