using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;

namespace LoggingKata
{
    class Program
    {
        static readonly ILog logger = new TacoLogger();
        const string csvPath = "TacoBell-US-AL.csv";

        static void Main(string[] args)
        {

            logger.LogInfo("Log initialized...");

            var lines = File.ReadAllLines(csvPath);

            if (lines.Length == 0)
            {
                logger.LogError("File has no data");
            }

            if (lines.Length == 1)
            {
                logger.LogWarning("File only has one line of data");
            }

            logger.LogInfo($"Lines: {lines[0]}");

            
            var parser = new TacoParser();
           
            var locations = lines.Select(parser.Parse).ToArray();

            ITrackable firstTB = null;
            ITrackable secondTB = null;
            double distance = 0;

            ITrackable closest1 = null;
            ITrackable closest2 = null;
            double proximity = 0;

            for (int i = 0; i < locations.Length; i++)
            {
                var locA = locations[i];

                var corA = new GeoCoordinate();

                corA.Latitude = locA.Location.Latitude;
                corA.Longitude = locA.Location.Longitude;

                for (int j = 0; j < locations.Length; j++)
                {
                    // Create a new corA Coordinate with your locA's lat and long
                    // Now, do another loop on the locations with the scope of your first loop, so you can grab the
                    // "destination" location (perhaps: `locB`)
                    // Create a new Coordinate with your locB's lat and long

                    var locB = locations[j];

                    var corB = new GeoCoordinate();

                    corB.Latitude = locB.Location.Latitude;
                    corB.Longitude = locB.Location.Longitude;

                    // Now, compare the two using `.GetDistanceTo()`, which returns a double
                    // If the distance is greater than the currently saved distance, update the distance and the two
                    // `ITrackable` variables you set above

                    if (corA.GetDistanceTo(corB) > distance) //to get locations farthest from each other.
                    {
                        distance = corA.GetDistanceTo(corB);
                        firstTB = locA;
                        secondTB = locB;

                        if (proximity == 0) proximity = distance;
                    }

                    if (corA.GetDistanceTo(corB) < proximity && corA.GetDistanceTo(corB) != 0) //to get locations closest to each other.
                    {
                        proximity = corA.GetDistanceTo(corB);
                        closest1 = locA;
                        closest2 = locB;
                    }
                }
            }

            logger.LogInfo($"{firstTB.Name} and {secondTB.Name} are the farthest apart");

            logger.LogInfo($"{closest1.Name} and {closest2.Name} are the closest locations");
        }
    }
}
