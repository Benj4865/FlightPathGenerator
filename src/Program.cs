
using System.IO.Pipes;
using System.Text.Json;
using System.Text.Json.Nodes;

var client = new PipeClient();
_= client.ConnectAsync("FlightPathGenerator");

Console.WriteLine("FlightPathGenerator is running...");

Console.WriteLine("Please enter the starting point coordinates (latitude, longitude):");
var startingPoint = Console.ReadLine();
Console.WriteLine("Please enter the destination point coordinates (latitude, longitude):");
var destinationPoint = Console.ReadLine();

Console.WriteLine("Please enter the number of your flight hardware configuration");
Console.WriteLine("1. DJI Mavic Air 2");
Console.WriteLine("2. DJI Phantom 4 Pro");
Console.WriteLine("3. DJI Inspire 2");
var hardwareChoice = Console.ReadLine();
if (!int.TryParse(hardwareChoice, out int hardwareConfig) || hardwareConfig < 1 || hardwareConfig > 3)
{
    Console.WriteLine("Invalid hardware configuration choice. Please enter a number between 1 and 3.");
    return;
}

var flightPath = GenerateFlightPath(startingPoint, destinationPoint, hardwareConfig);

await client.SendMessage("message","FlightPath", "FlightPathGenerator", "FlightPathVisualizer", flightPath);

Console.WriteLine("Flight path sent to visualizer.");

static string GenerateFlightPath(string start, string destination, int hardwareConfig)
{
    // Placeholder for flight path generation logic
    Console.WriteLine($"Generating flight path from {start} to {destination} using hardware configuration {hardwareConfig}...");
    // Here you would implement the actual flight path generation logic based on the inputs

    //load json template from FlightPathTemplate.json
    string filePath = "FlightPathTemplate.json";
    string text = File.ReadAllText(filePath);
    var jsonNode = JsonNode.Parse(text);
    var coordinatesNode = jsonNode?["geometry"]?["coordinates"] as JsonArray;
    coordinatesNode.Add(JsonNode.Parse($"[{start.Split(",")[0]}, {start.Split(",")[1]}, 0]"));
    coordinatesNode.Add(JsonNode.Parse($"[{destination.Split(",")[0]}, {destination.Split(",")[1]}, 0]"));
    //File.WriteAllText("flight_path.json", jsonNode.ToString());

    return jsonNode.ToString();
    
}


