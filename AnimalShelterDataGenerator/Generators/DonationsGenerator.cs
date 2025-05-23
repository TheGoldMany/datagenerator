using AnimalShelterDataGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalShelterDataGenerator.Generators
{
    public static class DonationsGenerator
    {
        private static Random _random = new Random();

        public static void Generate(string outputPath, List<int> userIds, List<int> shelterIds, int recordCount)
        {
            Console.WriteLine("Generating Donations data...");

            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("-- Donations data generation script");
                writer.WriteLine("-- Generated: " + DateTime.Now.ToString());
                writer.WriteLine();

                for (int i = 1; i <= recordCount; i++)
                {
                    int? userId = null;

                    // 85% eséllyel ismert felhasználótól jön az adomány
                    bool isAnonymous = _random.Next(100) < 15;
                    if (!isAnonymous)
                    {
                        userId = userIds[_random.Next(userIds.Count)];
                    }

                    int shelterId = shelterIds[_random.Next(shelterIds.Count)];

                    // Adomány összege (5-500 dollár)
                    double amount = Math.Round(5 + _random.NextDouble() * 495, 2);

                    DateTime donationDate = DateTime.Now.AddDays(-_random.Next(1, 365));

                    string message = isAnonymous ? "Anonymous donation" : GenerateDonationMessage();

                    writer.Write($"INSERT INTO Donations (donation_id, shelter_id, amount, donation_date, is_anonymous, message");

                    if (userId.HasValue)
                        writer.Write(", user_id");

                    writer.Write($") VALUES ({i}, {shelterId}, {amount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}, TO_DATE('{donationDate:yyyy-MM-dd}', 'YYYY-MM-DD'), {(isAnonymous ? 1 : 0)}, '{Helpers.EscapeSqlString(message)}'");

                    if (userId.HasValue)
                        writer.Write($", {userId}");

                    writer.WriteLine(");");

                    if (i % 1000 == 0)
                    {
                        Console.WriteLine($"Generated {i} donation records...");
                        writer.WriteLine("COMMIT;");
                    }
                }

                writer.WriteLine();
                writer.WriteLine("COMMIT;");
            }

            Console.WriteLine($"Generated {recordCount} donation records successfully!");
        }

        private static string GenerateDonationMessage()
        {
            string[] messages = {
                "Thank you for the wonderful work you do for these animals!",
                "Happy to support your mission of finding homes for pets in need.",
                "In memory of my beloved pet who brought so much joy to my life.",
                "Please use this donation where it's needed most.",
                "Keep up the great work! The animals deserve the best care possible.",
                "Grateful for the important work you do in our community.",
                "For the animals still waiting for their forever homes.",
                "To help with medical care for the animals that need it most.",
                "Supporting your mission of saving lives and finding homes.",
                "Thank you for giving these animals a second chance."
            };

            return Helpers.GetRandomFromArray(messages);
        }
    }
}
