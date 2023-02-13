// See https://aka.ms/new-console-template for more information
using Kadmium_sACN;
using Kadmium_sACN.MulticastAddressProvider;
using Kadmium_sACN.Layers.Framing;
using Kadmium_sACN.SacnSender;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using DMX_StartupSetter;

Console.WriteLine("Hello, World!");

var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: false);

IConfiguration config = builder.Build();

var settings = config.Get<Settings>();


var sacnSender = new SacnSender();
byte[] cid = new byte[16]; 
Random.Shared.NextBytes(cid);

string sourceName = settings.SourceName;
var factory = new SacnPacketFactory(cid, sourceName);





//using System.Timers.Timer timer = new System.Timers.Timer(10000);
//timer.Elapsed += async (sender, e) =>
//{
//    var packets = factory.CreateUniverseDiscoveryPackets(new UInt16[] { universe });
//    foreach (var packet in packets)
//    {
//        await sacnSender.SendMulticast(packet);
//    }
//    Console.WriteLine("sending req");
//};





using System.Timers.Timer timer2 = new System.Timers.Timer(10000);
timer2.Elapsed += async (sender, e) =>
{
    var settings = config.Get<Settings>();
    var text = await File.ReadAllTextAsync(settings.ConfigFile);
    var dic = JsonSerializer.Deserialize<Dictionary<string, int>>(text);


    UInt16 universe = (UInt16) settings.Universe;
    byte[] data = new byte[512];

    for (int i = 0; i < data.Length; i++)
    {
        if(dic != null && dic.TryGetValue((i + 1).ToString(), out int dVal))
            data[i] = (byte)dVal;
    }


    var packets = factory.CreateUniverseDiscoveryPackets(new UInt16[] { universe });

    var packet = factory.CreateDataPacket(universe, data);
    await sacnSender.SendUnicast(packet, System.Net.IPAddress.Parse(settings.Destination));

    Console.WriteLine("sending packages");
};




//timer.Start();
timer2.Start();



Console.ReadLine();