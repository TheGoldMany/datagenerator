using AnimalShelterDataGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalShelterDataGenerator.Generators
{
    public static class UsersGenerator
    {
        private static Random _random = new Random();

        public static void Generate(string outputPath, List<int> shelterIds, List<int> userIds, List<int> staffUserIds, int recordCount)
        {
            Console.WriteLine("Generating Users data...");

            // Nevek listája
            string[] firstNames = {
                "James", "John", "Robert", "Michael", "William", "David", "Richard", "Joseph", "Thomas", "Charles",
                "Mary", "Patricia", "Jennifer", "Linda", "Elizabeth", "Barbara", "Susan", "Jessica", "Sarah", "Karen",
                "Daniel", "Matthew", "Anthony", "Mark", "Donald", "Steven", "Paul", "Andrew", "Joshua", "Kenneth",
                "Nancy", "Lisa", "Betty", "Dorothy", "Sandra", "Ashley", "Kimberly", "Donna", "Emily", "Michelle"
            };

            string[] lastNames = {
                "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor",
                "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin", "Thompson", "Garcia", "Martinez", "Robinson",
                "Clark", "Rodriguez", "Lewis", "Lee", "Walker", "Hall", "Allen", "Young", "Hernandez", "King",
                "Wright", "Lopez", "Hill", "Scott", "Green", "Adams", "Baker", "Gonzalez", "Nelson", "Carter"
            };

            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("-- Users data generation script");
                writer.WriteLine("-- Generated: " + DateTime.Now.ToString());
                writer.WriteLine();

                for (int i = 1; i <= recordCount; i++)
                {
                    string firstName = firstNames[_random.Next(firstNames.Length)];
                    string lastName = lastNames[_random.Next(lastNames.Length)];
                    string email = $"{firstName.ToLower()}.{lastName.ToLower()}{_random.Next(100, 1000)}@example.com";
                    string password = "hashed_password_here"; // Éles rendszerben természetesen ez hashelt jelszó lenne
                    string phone = $"555-{_random.Next(100, 1000)}-{_random.Next(1000, 10000)}";

                    // Cím
                    string address = $"{_random.Next(100, 10000)} {Helpers.GetRandomStreetName()} St";
                    string city = Helpers.GetRandomCity();
                    string state = Helpers.GetRandomState();
                    string postalCode = _random.Next(10000, 100000).ToString();
                    string country = "USA";

                    // Regisztráció dátuma (1-3 éven belül)
                    DateTime registrationDate = DateTime.Now.AddDays(-_random.Next(30, 1095));

                    // Admin, menhelyi munkatárs státusz
                    int isAdmin = (i <= 5) ? 1 : 0; // Az első 5 felhasználó legyen admin
                    int isShelterStaff = 0;
                    int? shelterId = null;

                    // A felhasználók kb. 5%-a legyen menhelyi munkatárs
                    if (i % 20 == 0)
                    {
                        isShelterStaff = 1;
                        shelterId = shelterIds[_random.Next(shelterIds.Count)];
                        staffUserIds.Add(i); // Hozzáadjuk a staff ID-khoz
                    }

                    writer.Write($"INSERT INTO Users (user_id, email, password, first_name, last_name, phone, address, city, state, postal_code, country, registration_date, is_admin, is_shelter_staff");

                    if (shelterId.HasValue)
                        writer.Write(", shelter_id");

                    writer.Write($") VALUES ({i}, '{email}', '{password}', '{firstName}', '{lastName}', '{phone}', '{address}', '{city}', '{state}', '{postalCode}', '{country}', TO_DATE('{registrationDate:yyyy-MM-dd}', 'YYYY-MM-DD'), {isAdmin}, {isShelterStaff}");

                    if (shelterId.HasValue)
                        writer.Write($", {shelterId}");

                    writer.WriteLine(");");

                    userIds.Add(i);

                    if (i % 100 == 0)
                        Console.WriteLine($"Generated {i} user records...");
                }

                writer.WriteLine();
                writer.WriteLine("COMMIT;");
            }

            Console.WriteLine($"Generated {recordCount} user records successfully!");
        }
    }
}
