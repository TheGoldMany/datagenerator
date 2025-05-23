using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalShelterDataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Animal Shelter Database Generator ===");
            Console.WriteLine("Starting data generation process...");

            var dataGenerator = new DataGenerator();

            try
            {
                dataGenerator.GenerateData();
                Console.WriteLine("All data generated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred during data generation: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}