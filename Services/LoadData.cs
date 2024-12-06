using System.Collections.Concurrent;
using System.Globalization;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Lines.Services
{

    public class LoadData
    {
        private readonly ConcurrentDictionary<string, CityData> ConcurrentCityData = new();

        public void LoadFromFile(string filePath)
        {

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist!");
                throw new FileNotFoundException($"File not found at: {filePath}");
            }
            ConcurrentCityData.Clear();

            Parallel.ForEach(File.ReadLines(filePath), line =>
            {
                var parts = line.Split(';');
                if (parts.Length != 2)
                {
                    return;
                }

                string cityName = parts[0];
                if (!double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double temperature))
                {
                    return;
                }


                ConcurrentCityData.AddOrUpdate(cityName,
                    new CityData
                    {
                        Min = temperature,
                        Max = temperature,
                        Sum = temperature,
                        Count = 1,
                    },
                    (key, temps) =>
                    {
                        temps.Min = Math.Min(temperature, temps.Min);
                        temps.Max = Math.Max(temperature, temps.Max);
                        temps.Count++;
                        temps.Sum = temps.Sum + temperature;
                        return temps;
                    });
            });
            Console.WriteLine("Cleared cache and data loaded.");
        }

        public Dictionary<string, CityData> CityDataDictionary()
        {
            return new Dictionary<string, CityData>(ConcurrentCityData);
        }
    }

}

public class CityData
{
    public double Min { get; set; }
    public double Max { get; set; }
    public double Sum { get; set; }
    public int Count { get; set; }

    public double Average
    {
        get
        {
            if (Count > 0)
            {
                return Sum / Count;
            }
            else
            {
                return 0.0;
            }
        }
    }
}