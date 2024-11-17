using System;
using System.IO;
using System.Text.Json;
using Spectre.Console;

class MainProgram
{
    static void Main(string[] args)
    {
        // Load configuration and pricing
        Configuration config = LoadConfiguration();
        if (config == null) return;

        Pricing pricing = LoadPricing();
        if (pricing == null) return;

        ParkingGarage parkingGarage = new ParkingGarage(config.NumberOfSpaces);

        bool isRunning = true;

        while (isRunning)
        {
            Console.Clear();
            AnsiConsole.Markup("[bold green]Välkommen till parkering.Party POOPERS[/]");
            AnsiConsole.WriteLine();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Välj ett alternativ:[/]")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "1. Parkera fordon",
                        "2. Flytta fordon",
                        "3. Hämta fordon",
                        "4. Sök efter fordon",
                        "5. Visa parkeringsstatus",
                        "6. Avsluta"
                    })
                    .HighlightStyle(new Style(Color.Blue, decoration: Decoration.Bold)));

            switch (choice)
            {
                case "1. Parkera fordon":
                    ParkVehicle(parkingGarage);
                    break;
                case "2. Flytta fordon":
                    MoveVehicle(parkingGarage);
                    break;
                case "3. Hämta fordon":
                    RetrieveVehicle(parkingGarage);
                    break;
                case "4. Sök efter fordon":
                    SearchVehicle(parkingGarage);
                    break;
                case "5. Visa parkeringsstatus":
                    parkingGarage.ShowStatus();
                    break;
                case "6. Avsluta":
                    isRunning = false;
                    AnsiConsole.MarkupLine("[red]Programmet stängs ner...[/]");
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Ogiltigt val, försök igen.[/]");
                    break;
            }
            if (isRunning)
            {
                AnsiConsole.MarkupLine("[grey]Tryck på valfri tangent för att fortsätta...[/]");
                Console.ReadKey();
            }
        }
    }

    // Method to load configuration from JSON file
    private static Configuration LoadConfiguration()
    {
        try
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\jakob\\source\\repos\\Prague Parking V2 Uppgift\\Prague Parking V2 Uppgift\\Config.Json");
            AnsiConsole.MarkupLine($"[grey]Letar efter konfigurationsfil på: {path}[/]");

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<Configuration>(json);
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Konfigurationsfil saknas: Config.json[/]");
                return null;
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Fel vid inläsning av konfigurationsfilen: {ex.Message}[/]");
            return null;
        }
    }

    private static Pricing LoadPricing()
    {
        try
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\jakob\\source\\repos\\Prague Parking V2 Uppgift\\Prague Parking V2 Uppgift\\pricelist.json");
            AnsiConsole.MarkupLine($"[grey]Letar efter prislista på: {path}[/]");

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<Pricing>(json);
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Prislista saknas: pricelist.json[/]");
                return null;
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Fel vid inläsning av prislistan: {ex.Message}[/]");
            return null;
        }
    }

    private static void RetrieveVehicle(ParkingGarage parkingGarage)
    {
        Console.Write("Ange registreringsnummer på fordonet som ska hämtas: ");
        string regNr = Console.ReadLine().ToUpper();

        Vehicle vehicle = parkingGarage.RetrieveVehicle(regNr);
        if (vehicle != null)
        {
            Console.WriteLine($"Fordon med registreringsnummer {regNr} har hämtats.");
            Console.WriteLine($"Parkeringsavgift: {vehicle.CalculateParkingFee()} SEK");
        }
        else
        {
            Console.WriteLine("Fordonet hittades inte.");
        }
    }

    private static void SearchVehicle(ParkingGarage parkingGarage)
    {
        Console.Write("Ange registreringsnummer för att söka: ");
        string regNr = Console.ReadLine().ToUpper();

        Vehicle vehicle = parkingGarage.SearchVehicle(regNr);
        if (vehicle != null)
        {
            Console.WriteLine($"Fordon med registreringsnummer {regNr} finns i garaget.");
        }
        else
        {
            Console.WriteLine("Fordonet hittades inte.");
        }
    }

    private static void MoveVehicle(ParkingGarage parkingGarage)
    {
        Console.Write("Ange registreringsnummer på fordonet som ska flyttas: ");
        string regNr = Console.ReadLine().ToUpper();

        if (parkingGarage.MoveVehicle(regNr))
        {
            Console.WriteLine($"Fordon med registreringsnummer {regNr} har flyttats.");
        }
        else
        {
            Console.WriteLine("Fordonet hittades inte eller kan inte flyttas.");
        }
    }

    // Method for parking a vehicle
    static void ParkVehicle(ParkingGarage parkingGarage)
    {
        Console.Write("Ange fordonstyp (CAR/MC): ");
        string vehicleType = Console.ReadLine().ToUpper();

        Vehicle vehicle = null;

        Console.Write("Ange registreringsnummer: ");
        string regNr = Console.ReadLine().ToUpper();

        if (vehicleType == "CAR")
        {
            vehicle = new Car(regNr);
        }
        else if (vehicleType == "MC")
        {
            vehicle = new MC(regNr);
        }
        else
        {
            Console.WriteLine("Ogiltig fordonstyp.");
            return;
        }

        if (parkingGarage.ParkVehicle(vehicle))
        {
            Console.WriteLine("Fordon parkerat på plats.");
        }
        else
        {
            Console.WriteLine("Inga lediga platser tillgängliga.");
        }
    }
}