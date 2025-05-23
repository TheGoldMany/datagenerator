using AnimalShelterDataGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalShelterDataGenerator.Generators
{
    public static class AdoptionApplicationsGenerator
    {
        private static Random _random = new Random();

        public static void Generate(string outputPath, List<int> animalIds, List<int> userIds,
            List<int> staffUserIds, Dictionary<int, string> animalStatusDict, int recordCount)
        {
            Console.WriteLine("Generating Adoption Applications data...");

            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("-- Adoption Applications data generation script");
                writer.WriteLine("-- Generated: " + DateTime.Now.ToString());
                writer.WriteLine();

                // Státuszok és súlyaik
                string[] statuses = { "pending", "approved", "rejected" };
                int[] statusWeights = { 60, 20, 20 }; // 60% pending, 20% approved, 20% rejected

                // Lakhatási típusok
                string[] housingTypes = { "House", "Apartment", "Condo", "Mobile Home", "Farm", "Other" };

                for (int i = 1; i <= recordCount; i++)
                {
                    // Véletlenszerűen választunk egy állatot, amihez a kérelmet társítjuk
                    int animalId = animalIds[_random.Next(animalIds.Count)];

                    // Ellenőrizzük, hogy az állat státusza "available" vagy "pending"
                    // Ha nem elérhető, választunk egy másikat
                    string animalStatus = "";
                    if (animalStatusDict.TryGetValue(animalId, out animalStatus))
                    {
                        if (animalStatus != "available" && animalStatus != "pending")
                        {
                            // Próbáljunk meg 10-szer találni egy megfelelő állatot
                            bool foundAvailable = false;
                            for (int j = 0; j < 10; j++)
                            {
                                animalId = animalIds[_random.Next(animalIds.Count)];
                                if (animalStatusDict.TryGetValue(animalId, out animalStatus) &&
                                    (animalStatus == "available" || animalStatus == "pending"))
                                {
                                    foundAvailable = true;
                                    break;
                                }
                            }

                            // Ha nem találtunk, akkor generáljunk újra egy azonosítót és folytassuk
                            if (!foundAvailable)
                            {
                                continue;
                            }
                        }
                    }

                    // Véletlenszerűen választunk egy felhasználót, aki a kérelmet beadja
                    int userId = userIds[_random.Next(userIds.Count)];

                    // Kérelem dátumának generálása (6 hónapon belül)
                    DateTime applicationDate = DateTime.Now.AddDays(-_random.Next(1, 180));

                    // Státusz kiválasztása a megadott súlyok alapján
                    string status = Helpers.GetWeightedRandomItem(statuses, statusWeights);

                    // Review dátum csak akkor van, ha a státusz nem "pending"
                    DateTime? reviewDate = null;
                    int? reviewedBy = null;
                    if (status != "pending")
                    {
                        reviewDate = applicationDate.AddDays(_random.Next(1, 14)); // 1-14 napon belül

                        // Bírálót csak staff felhasználók közül választunk
                        if (staffUserIds.Count > 0)
                        {
                            reviewedBy = staffUserIds[_random.Next(staffUserIds.Count)];
                        }
                    }

                    // Lakhatási típus és udvar
                    string housingType = housingTypes[_random.Next(housingTypes.Length)];
                    bool hasYard = (housingType == "House" || housingType == "Farm") ?
                        _random.Next(100) < 80 : _random.Next(100) < 20;

                    // Egyéb mezők generálása
                    string otherPetsDescription = GenerateOtherPetsDescription();
                    string reasonForAdoption = GenerateReasonForAdoption();
                    string lifestyleDescription = GenerateLifestyleDescription();

                    // Elutasítás indoka, ha a státusz "rejected"
                    string decisionReason = status == "rejected" ?
                        GenerateRejectionReason() : (status == "approved" ? "Application approved" : "");

                    // SQL utasítás formálása
                    writer.Write($"INSERT INTO Adoption_Applications (application_id, animal_id, user_id, application_date, status");

                    if (reviewDate.HasValue)
                        writer.Write(", review_date, reviewed_by");

                    writer.Write($", housing_type, has_yard, other_pets_description, reason_for_adoption, lifestyle_description");

                    if (!string.IsNullOrEmpty(decisionReason))
                        writer.Write(", decision_reason");

                    writer.Write($") VALUES ({i}, {animalId}, {userId}, TO_DATE('{applicationDate:yyyy-MM-dd}', 'YYYY-MM-DD'), '{status}'");

                    if (reviewDate.HasValue)
                        writer.Write($", TO_DATE('{reviewDate.Value:yyyy-MM-dd}', 'YYYY-MM-DD'), {(reviewedBy.HasValue ? reviewedBy.ToString() : "NULL")}");

                    writer.Write($", '{Helpers.EscapeSqlString(housingType)}', {(hasYard ? 1 : 0)}, " +
                        $"'{Helpers.EscapeSqlString(otherPetsDescription)}', '{Helpers.EscapeSqlString(reasonForAdoption)}', '{Helpers.EscapeSqlString(lifestyleDescription)}'");

                    if (!string.IsNullOrEmpty(decisionReason))
                        writer.Write($", '{Helpers.EscapeSqlString(decisionReason)}'");

                    writer.WriteLine(");");

                    if (i % 1000 == 0)
                    {
                        Console.WriteLine($"Generated {i} adoption application records...");
                        writer.WriteLine("COMMIT;"); // Időnként commitolunk
                    }
                }

                writer.WriteLine();
                writer.WriteLine("COMMIT;");
            }

            Console.WriteLine($"Generated {recordCount} adoption application records successfully!");
        }

        // Segédfüggvények
        private static string GenerateOtherPetsDescription()
        {
            bool hasPets = _random.Next(100) < 40; // 40% eséllyel van más háziállata

            if (!hasPets)
                return "No other pets";

            string[] petTypes = { "dog", "cat", "fish", "bird", "hamster", "rabbit", "reptile" };
            int petCount = _random.Next(1, 5);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < petCount; i++)
            {
                string petType = Helpers.GetRandomFromArray(petTypes);
                int count = _random.Next(1, 4);

                if (i > 0)
                    sb.Append(", ");

                sb.Append($"{count} {petType}{(count > 1 ? "s" : "")}");
            }

            return sb.ToString();
        }

        private static string GenerateReasonForAdoption()
        {
            string[] reasons = {
                "Looking for a companion",
                "Want to provide a forever home for a shelter animal",
                "My children want a pet",
                "Recently lost a pet and ready for a new one",
                "Want to add to our family",
                "Looking for an emotional support animal",
                "Want a pet for protection/security",
                "Always wanted this type of animal",
                "Found this animal and fell in love",
                "Want to teach my children responsibility"
            };

            return Helpers.GetRandomFromArray(reasons);
        }

        private static string GenerateLifestyleDescription()
        {
            string[] activityLevels = { "very active", "moderately active", "somewhat active", "mostly relaxed" };
            string[] workSchedules = { "work from home", "work part-time outside the home", "work full-time outside the home" };
            string[] homeTypes = { "spacious house", "suburban home", "city apartment", "rural property", "condo" };
            string[] familyStructures = { "live alone", "live with spouse/partner", "small family with children", "large family" };

            string activityLevel = Helpers.GetRandomFromArray(activityLevels);
            string workSchedule = Helpers.GetRandomFromArray(workSchedules);
            string homeType = Helpers.GetRandomFromArray(homeTypes);
            string familyStructure = Helpers.GetRandomFromArray(familyStructures);

            return $"I am {activityLevel} and {workSchedule}. I {familyStructure} in a {homeType}.";
        }

        private static string GenerateRejectionReason()
        {
            string[] reasons = {
                "Housing situation not suitable for this animal",
                "Insufficient experience with this type of animal",
                "Work schedule does not allow enough time for care",
                "Not a good match with existing pets",
                "Another applicant was selected",
                "References did not provide positive feedback",
                "Living conditions not meeting requirements",
                "Applicant's expectations do not match animal's needs",
                "Landlord does not allow pets",
                "Animal requires special care applicant cannot provide"
            };

            return Helpers.GetRandomFromArray(reasons);
        }
    }
}

