using AnimalShelterDataGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalShelterDataGenerator.Generators
{
    public static class SheltersGenerator
    {
        private static Random _random = new Random();

        public static void Generate(string outputPath, List<int> shelterIds)
        {
            Console.WriteLine("Generating Shelters data...");

            // Menhely nevek és városok
            string[] shelterNames = {
                "Happy Tails Rescue", "Second Chance Animal Shelter", "Forever Home Society",
                "Paws & Claws Rescue", "Animal Haven", "Furry Friends Adoption Center",
                "Hope for Homeless Pets", "New Life Animal Sanctuary", "Loving Hearts Pet Shelter",
                "Rainbow Bridge Rescue", "Guardian Angels Animal Rescue", "The Pet Project",
                "Safe Harbor Animal Shelter", "Precious Paws Rescue", "Whiskers & Wags Shelter",
                "Sunny Days Pet Rescue", "Harmony House Animal Sanctuary", "Grateful Hearts Pet Adoption",
                "Green Valley Animal Shelter", "Blue Sky Pet Rescue"
            };

            string[] cities = {
                "New York", "Los Angeles", "Chicago", "Houston", "Phoenix", "Philadelphia",
                "San Antonio", "San Diego", "Dallas", "San Jose", "Austin", "Jacksonville",
                "Fort Worth", "Columbus", "Charlotte", "San Francisco", "Indianapolis", "Seattle",
                "Denver", "Washington DC"
            };

            string[] states = {
                "NY", "CA", "IL", "TX", "AZ", "PA", "TX", "CA", "TX", "CA", "TX", "FL",
                "TX", "OH", "NC", "CA", "IN", "WA", "CO", "DC"
            };

            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("-- Shelters data generation script");
                writer.WriteLine("-- Generated: " + DateTime.Now.ToString());
                writer.WriteLine();

                for (int i = 0; i < shelterNames.Length; i++)
                {
                    int id = i + 1;
                    string name = shelterNames[i];
                    string email = name.Replace(" ", "").ToLower() + "@example.com";
                    string phone = $"555-{_random.Next(100, 1000)}-{_random.Next(1000, 10000)}";
                    string address = $"{_random.Next(100, 10000)} {Helpers.GetRandomStreetName()} St";
                    string city = cities[i];
                    string state = states[i];
                    string postalCode = _random.Next(10000, 100000).ToString();
                    string country = "USA";
                    string website = "www." + name.Replace(" ", "").ToLower() + ".org";
                    string description = $"{name} is a non-profit animal shelter dedicated to finding loving homes for abandoned and rescued animals.";
                    string operationHours = "Monday-Friday: 10am-6pm, Saturday: 10am-5pm, Sunday: 12pm-4pm";
                    int isActive = 1;

                    writer.WriteLine($"INSERT INTO Shelters (shelter_id, name, email, phone, address, city, state, postal_code, country, website, description, operation_hours, is_active) VALUES " +
                                    $"({id}, '{name}', '{email}', '{phone}', '{address}', '{city}', '{state}', '{postalCode}', '{country}', '{website}', '{Helpers.EscapeSqlString(description)}', '{operationHours}', {isActive});");

                    shelterIds.Add(id);
                }

                writer.WriteLine();
                writer.WriteLine("COMMIT;");
            }

            Console.WriteLine($"Generated {shelterNames.Length} shelter records successfully!");
        }
    }
}
