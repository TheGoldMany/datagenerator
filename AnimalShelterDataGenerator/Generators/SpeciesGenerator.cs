using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalShelterDataGenerator.Generators
{
    public static class SpeciesGenerator
    {
        public static void Generate(string outputPath, List<int> speciesIds)
        {
            Console.WriteLine("Generating Species data...");

            // Fajok definíciója
            string[] speciesNames = new string[] {
                "Dog", "Cat", "Rabbit", "Bird", "Hamster", "Guinea Pig",
                "Ferret", "Rat", "Mouse", "Gerbil", "Chinchilla", "Hedgehog",
                "Turtle", "Lizard", "Snake", "Fish"
            };

            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("-- Species data generation script");
                writer.WriteLine("-- Generated: " + DateTime.Now.ToString());
                writer.WriteLine();

                for (int i = 0; i < speciesNames.Length; i++)
                {
                    int id = i + 1;
                    string name = speciesNames[i];
                    string description = $"Common household pet species: {name}";

                    writer.WriteLine($"INSERT INTO Species (species_id, name, description) VALUES " +
                                    $"({id}, '{name}', '{description}');");

                    speciesIds.Add(id);
                }

                writer.WriteLine();
                writer.WriteLine("COMMIT;");
            }

            Console.WriteLine($"Generated {speciesNames.Length} species records successfully!");
        }
    }
}
