using AnimalShelterDataGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalShelterDataGenerator.Generators
{
    public static class ReviewsGenerator
    {
        private static Random _random = new Random();

        public static void Generate(string outputPath, List<int> shelterIds, List<int> userIds, int recordCount)
        {
            Console.WriteLine("Generating Reviews data...");

            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("-- Reviews data generation script");
                writer.WriteLine("-- Generated: " + DateTime.Now.ToString());
                writer.WriteLine();

                for (int i = 1; i <= recordCount; i++)
                {
                    int shelterId = shelterIds[_random.Next(shelterIds.Count)];
                    int userId = userIds[_random.Next(userIds.Count)];

                    int rating = _random.Next(3, 6); // 3-5 csillag (többnyire pozitív értékelések)
                    string title = GenerateReviewTitle(rating);
                    string content = GenerateReviewContent(rating);

                    DateTime reviewDate = DateTime.Now.AddDays(-_random.Next(1, 365));

                    writer.WriteLine($"INSERT INTO Reviews (review_id, shelter_id, user_id, rating, title, content, review_date) VALUES " +
                                    $"({i}, {shelterId}, {userId}, {rating}, '{Helpers.EscapeSqlString(title)}', '{Helpers.EscapeSqlString(content)}', TO_DATE('{reviewDate:yyyy-MM-dd}', 'YYYY-MM-DD'));");

                    if (i % 1000 == 0)
                    {
                        Console.WriteLine($"Generated {i} review records...");
                        writer.WriteLine("COMMIT;");
                    }
                }

                writer.WriteLine();
                writer.WriteLine("COMMIT;");
            }

            Console.WriteLine($"Generated {recordCount} review records successfully!");
        }

        private static string GenerateReviewTitle(int rating)
        {
            if (rating >= 4)
            {
                string[] goodTitles = {
                    "Amazing experience!", "Wonderful shelter!", "Best adoption experience!",
                    "Highly recommend!", "Great people, great animals!", "Fantastic organization!",
                    "Couldn't be happier!", "A shelter that truly cares!", "Outstanding service!"
                };

                return Helpers.GetRandomFromArray(goodTitles);
            }
            else
            {
                string[] okTitles = {
                    "Good experience overall", "Nice shelter", "Satisfied with our adoption",
                    "Decent experience", "Good people working there", "Mostly positive experience"
                };

                return Helpers.GetRandomFromArray(okTitles);
            }
        }

        private static string GenerateReviewContent(int rating)
        {
            if (rating >= 4)
            {
                string[] goodReviews = {
                    "We had a wonderful experience adopting from this shelter. The staff was knowledgeable and caring, and they helped us find the perfect pet for our family. The adoption process was straightforward and they provided great follow-up support.",
                    "I cannot say enough good things about this shelter! They truly care about the animals and make sure they go to good homes. The facility is clean and well-maintained, and all the animals seem happy and well-cared for. Our new pet has been a perfect addition to our family.",
                    "This is how all animal shelters should be run! The staff took time to understand what we were looking for and matched us with a pet that fits perfectly with our lifestyle. The adoption process was thorough but not overly complicated. Highly recommend!",
                    "Five stars all the way! The shelter staff were friendly and professional. They were honest about each animal's personality and needs. We appreciated their transparency and commitment to finding the right homes for their animals.",
                    "Outstanding shelter with a genuine passion for animal welfare. The facility is clean, the animals are well cared for, and the staff are incredibly knowledgeable. The entire adoption process was a positive experience from start to finish."
                };

                return Helpers.GetRandomFromArray(goodReviews);
            }
            else
            {
                string[] okReviews = {
                    "The shelter was clean and the animals seemed well cared for. The adoption process was a bit lengthy, but overall it was a positive experience. The staff were helpful, though sometimes busy with other tasks.",
                    "A decent shelter with good intentions. The facility could use some updates, but the animals receive proper care. Staff was friendly though sometimes difficult to reach by phone. Our adoption went smoothly despite a few minor hiccups.",
                    "Generally a good experience. The adoption process was thorough, which I appreciate. The staff seemed knowledgeable but somewhat overwhelmed. The shelter could benefit from more volunteers or staff members to provide better service.",
                    "We're happy with our new pet, but the adoption process was more complicated than expected. There was some confusion about the paperwork, but the staff eventually sorted everything out. The shelter itself is clean and well-maintained."
                };

                return Helpers.GetRandomFromArray(okReviews);
            }
        }
    }
}
