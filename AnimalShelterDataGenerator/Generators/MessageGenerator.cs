using AnimalShelterDataGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalShelterDataGenerator.Generators
{
    public static class MessagesGenerator
    {
        private static Random _random = new Random();

        public static void Generate(string outputPath, List<int> userIds, List<int> staffUserIds, int recordCount)
        {
            Console.WriteLine("Generating Messages data...");

            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("-- Messages data generation script");
                writer.WriteLine("-- Generated: " + DateTime.Now.ToString());
                writer.WriteLine();

                for (int i = 1; i <= recordCount; i++)
                {
                    int senderId = userIds[_random.Next(userIds.Count)];

                    // A címzett vagy egy felhasználó, vagy egy menhelyi dolgozó
                    int receiverId;

                    // 70% eséllyel a menhelyi dolgozónak megy az üzenet
                    if (_random.Next(100) < 70 && staffUserIds.Count > 0)
                    {
                        receiverId = staffUserIds[_random.Next(staffUserIds.Count)];
                    }
                    else
                    {
                        // Másik felhasználó, de ne önmagának
                        do
                        {
                            receiverId = userIds[_random.Next(userIds.Count)];
                        } while (receiverId == senderId);
                    }

                    string subject = GenerateMessageSubject();
                    string content = GenerateMessageContent();

                    DateTime sendDate = DateTime.Now.AddDays(-_random.Next(1, 90));

                    // Az üzenetek kb. 60%-a olvasott
                    bool isRead = _random.Next(100) < 60;
                    DateTime? readDate = null;
                    if (isRead)
                    {
                        readDate = sendDate.AddHours(_random.Next(1, 48));
                    }

                    writer.Write($"INSERT INTO Messages (message_id, sender_id, receiver_id, subject, content, send_date");

                    if (readDate.HasValue)
                        writer.Write(", read_date");

                    writer.Write($", is_read) VALUES " +
                                $"({i}, {senderId}, {receiverId}, '{Helpers.EscapeSqlString(subject)}', '{Helpers.EscapeSqlString(content)}', TO_DATE('{sendDate:yyyy-MM-dd HH:mm:ss}', 'YYYY-MM-DD HH24:MI:SS')");

                    if (readDate.HasValue)
                        writer.Write($", TO_DATE('{readDate.Value:yyyy-MM-dd HH:mm:ss}', 'YYYY-MM-DD HH24:MI:SS')");

                    writer.WriteLine($", {(isRead ? 1 : 0)});");

                    if (i % 1000 == 0)
                    {
                        Console.WriteLine($"Generated {i} message records...");
                        writer.WriteLine("COMMIT;");
                    }
                }

                writer.WriteLine();
                writer.WriteLine("COMMIT;");
            }

            Console.WriteLine($"Generated {recordCount} message records successfully!");
        }

        private static string GenerateMessageSubject()
        {
            string[] subjects = {
                "Question about adoption process", "Inquiry about available animals", "Follow-up on application",
                "Scheduled visit information", "Question about a specific animal", "Adoption fee inquiry",
                "Post-adoption support", "Volunteer opportunities", "Donation information", "General question",
                "Application status inquiry", "Animal care advice", "Event information", "Foster program inquiry"
            };

            return Helpers.GetRandomFromArray(subjects);
        }

        private static string GenerateMessageContent()
        {
            string[] contents = {
                "Hello, I'm interested in adopting a pet from your shelter. Could you please provide more information about the adoption process and any requirements? Thank you!",
                "I saw a pet on your website that I'm very interested in meeting. Is it possible to schedule a visit to meet them in person? I'm available most afternoons this week.",
                "I submitted an adoption application last week and was wondering about its status. Could you please let me know if there's any additional information you need from me?",
                "Can you tell me more about the animal with ID #12345? I'm particularly interested in their personality and if they would be a good fit for a home with children.",
                "I recently adopted a pet from your shelter and had a question about their behavior. They seem nervous in certain situations. Do you have any advice on how to help them adjust?",
                "I'm interested in volunteering at your shelter. Could you provide information about volunteer opportunities and how to get started? I have experience with animal care.",
                "Could you please provide details about your upcoming adoption event? I'd like to attend and possibly find a new family member.",
                "I have a question about the medical history of the pet I'm interested in adopting. Are there any ongoing health concerns I should be aware of?",
                "I'm considering making a donation to your shelter. Could you provide information about how donations are used and what current needs you have?",
                "I'm moving to a new home and need advice on how to help my recently adopted pet adjust to the new environment. Do you have any recommendations?"
            };

            return Helpers.GetRandomFromArray(contents);
        }
    }
}
