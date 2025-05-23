using AnimalShelterDataGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalShelterDataGenerator.Generators
{
    public static class EventsGenerator
    {
        private static Random _random = new Random();

        public static void Generate(string outputPath, List<int> shelterIds, int recordCount)
        {
            Console.WriteLine("Generating Events data...");

            string[] eventTitles = {
                "Adoption Day", "Pet Vaccination Clinic", "Dog Training Workshop", "Cat Grooming Seminar",
                "Animal Welfare Awareness", "Fundraising Gala", "Volunteer Orientation", "Pet Health Check",
                "Kids Reading to Pets", "Senior Pets Adoption", "Pet First Aid Workshop", "Spay/Neuter Awareness",
                "Pet Photography Day", "Meet Our New Arrivals", "Kitten Season Preparation", "Puppy Socialization",
                "Foster Family Meetup", "Holiday Pet Drive", "Summer Pet Safety", "Winter Pet Care Workshop"
            };

            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("-- Events data generation script");
                writer.WriteLine("-- Generated: " + DateTime.Now.ToString());
                writer.WriteLine();

                for (int i = 1; i <= recordCount; i++)
                {
                    int shelterId = shelterIds[_random.Next(shelterIds.Count)];

                    string title = eventTitles[_random.Next(eventTitles.Length)];
                    string description = GenerateEventDescription(title);

                    // Az események 60%-a a jövőben, 40%-a a múltban
                    DateTime eventDate;
                    if (_random.Next(100) < 60)
                    {
                        eventDate = DateTime.Now.AddDays(_random.Next(1, 90));
                    }
                    else
                    {
                        eventDate = DateTime.Now.AddDays(-_random.Next(1, 90));
                    }

                    int duration = _random.Next(60, 240); // 1-4 óra

                    string location = $"Shelter Location, {Helpers.GetRandomCity()}, {Helpers.GetRandomState()}";

                    // 20% eséllyel virtuális esemény
                    bool isVirtual = _random.Next(100) < 20;
                    string registrationLink = isVirtual ? $"https://virtual-events.example.com/{shelterId}/{i}" : "";

                    writer.WriteLine($"INSERT INTO Events (event_id, shelter_id, title, description, event_date, duration, location, is_virtual, registration_link) VALUES " +
                                    $"({i}, {shelterId}, '{Helpers.EscapeSqlString(title)}', '{Helpers.EscapeSqlString(description)}', TO_DATE('{eventDate:yyyy-MM-dd HH:mm:ss}', 'YYYY-MM-DD HH24:MI:SS'), {duration}, '{Helpers.EscapeSqlString(location)}', {(isVirtual ? 1 : 0)}, '{registrationLink}');");
             
                
                
                }
                writer.WriteLine();
                writer.WriteLine("COMMIT;");
            }

            Console.WriteLine($"Generated {recordCount} event records successfully!");
        }

        private static string GenerateEventDescription(string eventTitle)
        {
            switch (eventTitle)
            {
                case "Adoption Day":
                    return "Join us for a special adoption day event! We'll have a variety of animals available for adoption, and our staff will be on hand to help match you with your perfect pet companion. Adoption fees will be reduced for this event.";

                case "Pet Vaccination Clinic":
                    return "Our low-cost vaccination clinic provides essential vaccines for dogs and cats. Protect your pet's health and the health of others in your community. No appointment necessary, but please bring your pet's previous vaccination records if available.";

                case "Dog Training Workshop":
                    return "Learn effective training techniques from our professional dog trainers. This workshop covers basic commands, leash training, and behavior management. Great for new dog owners or those looking to refresh their training skills.";

                case "Cat Grooming Seminar":
                    return "Learn how to properly groom your feline friend at home. Our experts will demonstrate nail trimming, brushing techniques for different coat types, and tips for making grooming a stress-free experience for both you and your cat.";

                case "Animal Welfare Awareness":
                    return "An informative session on animal welfare issues in our community and how you can help. Learn about responsible pet ownership, reporting animal cruelty, and supporting local animal welfare initiatives.";

                case "Fundraising Gala":
                    return "Join us for an elegant evening in support of our shelter animals. The gala includes dinner, live entertainment, a silent auction, and testimonials from successful adopters. All proceeds directly benefit the animals in our care.";

                case "Volunteer Orientation":
                    return "Interested in volunteering at our shelter? Attend this orientation session to learn about volunteer opportunities, responsibilities, and how you can make a difference in the lives of homeless animals. No experience necessary.";

                case "Pet Health Check":
                    return "Our veterinary team offers free basic health checks for community pets. Services include weight check, dental examination, ear cleaning, and general wellness assessment. Please bring your pet on a leash or in a carrier.";

                case "Kids Reading to Pets":
                    return "A heartwarming program where children can practice their reading skills by reading to shelter animals. This activity helps socialize the animals and builds children's confidence in reading. Books are provided, but children may bring their own.";

                default:
                    return "Join us for this special event at our shelter. It's a great opportunity to meet our animals, learn more about pet care, and connect with other animal lovers in the community. We look forward to seeing you there!";
            }
        }
    }

}

