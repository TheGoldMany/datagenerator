using AnimalShelterDataGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalShelterDataGenerator.Generators
{
    public static class MedicalRecordsGenerator
    {
        private static Random _random = new Random();

        public static void Generate(string outputPath, List<int> animalIds, int recordCount)
        {
            Console.WriteLine("Generating Medical Records data...");

            string[] procedureTypes = {
                "Vaccination", "Spay/Neuter", "Dental Cleaning", "Injury Treatment", "Illness Treatment",
                "Microchipping", "Annual Checkup", "Blood Test", "X-Ray", "Surgery", "Parasite Treatment"
            };

            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("-- Medical Records data generation script");
                writer.WriteLine("-- Generated: " + DateTime.Now.ToString());
                writer.WriteLine();

                for (int i = 1; i <= recordCount; i++)
                {
                    int animalId = animalIds[_random.Next(animalIds.Count)];

                    // A rekord dátuma az állat bekerülésétől számított időszak
                    DateTime recordDate = DateTime.Now.AddDays(-_random.Next(1, 365));

                    string procedureType = procedureTypes[_random.Next(procedureTypes.Length)];
                    string description = GenerateMedicalDescription(procedureType);
                    string performedBy = $"Dr. {GetRandomVetName()}";
                    double cost = GetRandomCost(procedureType);

                    writer.WriteLine($"INSERT INTO Medical_Records (record_id, animal_id, record_date, procedure_type, description, performed_by, cost) VALUES " +
                                    $"({i}, {animalId}, TO_DATE('{recordDate:yyyy-MM-dd}', 'YYYY-MM-DD'), '{procedureType}', '{Helpers.EscapeSqlString(description)}', '{performedBy}', {cost.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)});");

                    if (i % 1000 == 0)
                    {
                        Console.WriteLine($"Generated {i} medical record records...");
                        writer.WriteLine("COMMIT;");
                    }
                }

                writer.WriteLine();
                writer.WriteLine("COMMIT;");
            }

            Console.WriteLine($"Generated {recordCount} medical record records successfully!");
        }

        private static string GenerateMedicalDescription(string procedureType)
        {
            switch (procedureType)
            {
                case "Vaccination":
                    string[] vaccines = { "Rabies", "Distemper", "Parvovirus", "Bordetella", "Leptospirosis", "FVRCP", "FeLV" };
                    return $"Administered {Helpers.GetRandomFromArray(vaccines)} vaccine. Animal is in good health.";

                case "Spay/Neuter":
                    return $"Routine {procedureType.ToLower()} procedure performed. Recovery is proceeding normally.";

                case "Dental Cleaning":
                    return "Performed thorough dental cleaning. " +
                           (_random.Next(100) < 30 ? "Extracted two damaged teeth. " : "") +
                           "Gums appear healthy.";

                case "Injury Treatment":
                    string[] injuries = { "minor laceration", "paw injury", "ear infection", "eye infection", "minor burn" };
                    return $"Treated {Helpers.GetRandomFromArray(injuries)}. Applied medication and bandages. Follow-up recommended in 7 days.";

                case "Illness Treatment":
                    string[] illnesses = { "upper respiratory infection", "digestive issue", "skin condition", "mild fever", "allergic reaction" };
                    return $"Diagnosed with {Helpers.GetRandomFromArray(illnesses)}. Prescribed appropriate medication. Should recover fully within 10-14 days.";

                case "Microchipping":
                    return "Microchip inserted subcutaneously between shoulder blades. Chip functioning correctly.";

                case "Annual Checkup":
                    return "Annual wellness examination completed. " +
                           (_random.Next(100) < 70 ? "All vital signs normal. " : "Slight weight gain noted. ") +
                           "Animal appears to be in good health overall.";

                case "Blood Test":
                    return _random.Next(100) < 80 ?
                        "Blood test completed. All levels within normal range." :
                        "Blood test shows slight abnormalities. Further monitoring recommended.";

                case "X-Ray":
                    return _random.Next(100) < 85 ?
                        "X-ray completed. No abnormalities detected." :
                        "X-ray shows minor skeletal irregularities. Not cause for concern at this time.";

                case "Surgery":
                    string[] surgeries = { "tumor removal", "foreign object removal", "fracture repair", "wound repair", "mass removal" };
                    return $"Performed {Helpers.GetRandomFromArray(surgeries)} surgery. Procedure went well. Recovery expected to take 2-3 weeks.";

                case "Parasite Treatment":
                    string[] parasites = { "fleas", "ticks", "ear mites", "intestinal worms", "heartworm" };
                    return $"Treated for {Helpers.GetRandomFromArray(parasites)}. Administered appropriate medication. Follow-up in 2 weeks.";

                default:
                    return $"Routine {procedureType} procedure performed. Animal is in stable condition.";
            }
        }

        private static string GetRandomVetName()
        {
            string[] firstNames = { "James", "John", "Robert", "Michael", "Mary", "Patricia", "Jennifer", "Linda", "Elizabeth", "Susan" };
            string[] lastNames = { "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor" };

            return $"{Helpers.GetRandomFromArray(firstNames)} {Helpers.GetRandomFromArray(lastNames)}";
        }

        private static double GetRandomCost(string procedureType)
        {
            switch (procedureType)
            {
                case "Vaccination":
                    return 20 + _random.NextDouble() * 30; // $20-50
                case "Spay/Neuter":
                    return 100 + _random.NextDouble() * 200; // $100-300
                case "Dental Cleaning":
                    return 150 + _random.NextDouble() * 250; // $150-400
                case "Injury Treatment":
                    return 50 + _random.NextDouble() * 150; // $50-200
                case "Illness Treatment":
                    return 75 + _random.NextDouble() * 125; // $75-200
                case "Microchipping":
                    return 30 + _random.NextDouble() * 20; // $30-50
                case "Annual Checkup":
                    return 40 + _random.NextDouble() * 60; // $40-100
                case "Blood Test":
                    return 60 + _random.NextDouble() * 90; // $60-150
                case "X-Ray":
                    return 100 + _random.NextDouble() * 150; // $100-250
                case "Surgery":
                    return 300 + _random.NextDouble() * 700; // $300-1000
                case "Parasite Treatment":
                    return 40 + _random.NextDouble() * 60; // $40-100
                default:
                    return 50 + _random.NextDouble() * 50; // $50-100
            }
        }
    }
}
