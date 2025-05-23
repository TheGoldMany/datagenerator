using AnimalShelterDataGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalShelterDataGenerator.Generators
{
    public static class PhotosGenerator
    {
        private static Random _random = new Random();

        public static void Generate(string outputPath, List<int> animalIds, int recordCount)
        {
            Console.WriteLine("Generating Photos data...");

            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("-- Photos data generation script");
                writer.WriteLine("-- Generated: " + DateTime.Now.ToString());
                writer.WriteLine();

                for (int i = 1; i <= recordCount; i++)
                {
                    int animalId = animalIds[_random.Next(animalIds.Count)];

                    // Szimulált URL-ek
                    string photoUrl = $"https://example.com/animal_photos/{animalId}/{Guid.NewGuid().ToString().Substring(0, 8)}.jpg";

                    DateTime uploadDate = DateTime.Now.AddDays(-_random.Next(1, 365));

                    // Az első fotó legyen az elsődleges (vagy véletlenszerűen)
                    bool isPrimary = (_random.Next(5) == 0); // 20% eséllyel legyen primary

                    string title = GetRandomPhotoTitle();
                    string description = GetRandomPhotoDescription();

                    writer.WriteLine($"INSERT INTO Photos (photo_id, animal_id, photo_url, upload_date, is_primary, title, description) VALUES " +
                                    $"({i}, {animalId}, '{photoUrl}', TO_DATE('{uploadDate:yyyy-MM-dd}', 'YYYY-MM-DD'), {(isPrimary ? 1 : 0)}, '{Helpers.EscapeSqlString(title)}', '{Helpers.EscapeSqlString(description)}');");

                    if (i % 1000 == 0)
                    {
                        Console.WriteLine($"Generated {i} photo records...");
                        writer.WriteLine("COMMIT;");
                    }
                }

                writer.WriteLine();
                writer.WriteLine("COMMIT;");
            }

            Console.WriteLine($"Generated {recordCount} photo records successfully!");
        }

        private static string GetRandomPhotoTitle()
        {
            string[] titles = {
                "Beautiful portrait", "Happy pet", "Playing outside", "Relaxing", "Nap time",
                "Looking for a home", "Curious explorer", "Sweet smile", "New arrival", "Adoption ready",
                "Best friend material", "Playtime", "Loving eyes", "Perfect pose", "Cuddle time"
            };

            return Helpers.GetRandomFromArray(titles);
        }

        private static string GetRandomPhotoDescription()
        {
            string[] descriptions = {
                "A beautiful photo showing the pet's personality.",
                "Taken during playtime in our shelter's yard.",
                "This photo really captures the sweet personality of this animal.",
                "Looking happy and healthy, ready for a forever home.",
                "Showing off those adorable eyes that will melt your heart.",
                "A moment of pure joy captured on camera.",
                "This cutie is waiting for the right family to come along.",
                "One of our staff's favorite photos of this sweetie.",
                "Just relaxing and enjoying some sunshine.",
                "A perfect representation of this pet's gentle nature."
            };

            return Helpers.GetRandomFromArray(descriptions);
        }
    }
}
