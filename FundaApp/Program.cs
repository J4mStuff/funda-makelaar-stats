using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FundaApp;

public static class Program
{
    public static void Main()
    {
        var sharedClient = new HttpClient
        {
            BaseAddress = new Uri("http://partnerapi.funda.nl/feeds/Aanbod.svc/"),
        };
        GetAsync(sharedClient).Wait();
    }

    private static async Task GetAsync(HttpClient httpClient)
    {
        var key = await File.ReadAllTextAsync("data/secret.txt");
        const string purchase = "koop";
        const string garden = "tuin";
        //const string agent = "makelaar";
        
        var uri = httpClient.BaseAddress + $"json/{key}/?type={purchase}&zo=/amsterdam/{garden}/&page=1&pagesize=25";
        //var uri = httpClient.BaseAddress + $"json/{key}/?type={purchase}&zo=/amsterdam/&page=1&pagesize=25"; 
        
        using var response = await httpClient.GetAsync(uri);
        
        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"{jsonResponse}\n");
    }
}