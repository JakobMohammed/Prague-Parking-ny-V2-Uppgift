using System.Collections.Generic;

public class Configuration
{
    public int NumberOfSpaces { get; set; } = 100;
    public Dictionary<string, VehicleTypeConfig> VehicleTypes { get; set; } = new Dictionary<string, VehicleTypeConfig>
    {
        { "CAR", new VehicleTypeConfig { MaxPerSpace = 1 } },
        { "MC", new VehicleTypeConfig { MaxPerSpace = 2 } }
    };
}

public class VehicleTypeConfig
{
    public int MaxPerSpace { get; set; }
}