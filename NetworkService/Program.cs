// See https://aka.ms/new-console-template for more information
using NetworkUtility.Ping;
using System.Net.NetworkInformation;

Console.WriteLine("Hello, World!");

NetworkService _pingService = new NetworkService();

var result = _pingService.GetLastPings();
Console.WriteLine(result.GetType());

IEnumerable<PingOptions> lastPings = new List<PingOptions>();
Console.WriteLine(lastPings.GetType());
