using AnimalShelterDataGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalShelterDataGenerator.Generators
{
    /// <summary>
    /// Állatok adatainak generálását végző osztály
    /// </summary>
    public static class AnimalsGenerator
    {
        private static Random _random = new Random();

        /// <summary>
        /// Generálja az állatok adatait és SQL fájlba írja
        /// </summary>
        public static void Generate(string outputPath, List<int> speciesIds, List<int> breedIds,
            List<int> shelterIds, List<int> animalIds, Dictionary<int, int> breedSpeciesMap,
            Dictionary<int, string> animalStatusDict, int recordCount)
        {
            Console.WriteLine("Generating Animals data...");

            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("-- Animals data generation script");
                writer.WriteLine("-- Generated: " + DateTime.Now.ToString());
                writer.WriteLine();

                for (int i = 1; i <= recordCount; i++)
                {
                    // Véletlenszerűen választunk egy fajt
                    int speciesId = speciesIds[_random.Next(speciesIds.Count)];

                    // Az adott fajhoz tartozó fajtát választunk
                    int breedId = GetCompatibleBreedId(speciesId, breedSpeciesMap, breedIds);

                    // Véletlenszerű menhely
                    int shelterId = shelterIds[_random.Next(shelterIds.Count)];

                    // Név generálása
                    string name = GetRandomAnimalName(speciesId);

                    // Kor generálása (fajtól függően)
                    int age = GetRandomAge(speciesId);

                    // Nem generálása
                    string gender = (_random.Next(2) == 0) ? "Male" : "Female";

                    // Szín generálása
                    string color = GetRandomColor(speciesId);

                    // Méret generálása
                    string size = GetRandomSize(speciesId);

                    // Súly generálása
                    double weight = GetRandomWeight(speciesId, size);

                    // Státusz generálása
                    string status = GetRandomStatus();
                    animalStatusDict[i] = status; // Elmentjük a státuszt később használatra

                    // Leírás generálása
                    string description = GenerateAnimalDescription(speciesId, breedId, age, gender, color, size);

                    // Bekerülés dátuma (1 év belül)
                    DateTime admissionDate = DateTime.Now.AddDays(-_random.Next(1, 365));

                    // Örökbefogadás dátuma (csak "adopted" státusz esetén)
                    DateTime? adoptionDate = null;
                    if (status == "adopted")
                    {
                        adoptionDate = admissionDate.AddDays(_random.Next(1, (DateTime.Now - admissionDate).Days));
                    }

                    // Egyéb tulajdonságok generálása
                    bool isNeutered = CanBeNeutered(speciesId) && _random.Next(100) < 70;
                    bool isHouseTrained = CanBeHouseTrained(speciesId) && _random.Next(100) < 60;
                    bool isGoodWithKids = CanBeGoodWithKids(speciesId) && _random.Next(100) < 70;
                    bool isGoodWithCats = CanBeGoodWithCats(speciesId) && _random.Next(100) < 50;
                    bool isGoodWithDogs = CanBeGoodWithDogs(speciesId) && _random.Next(100) < 60;

                    // SQL utasítás formálása
                    writer.Write($"INSERT INTO Animals (animal_id, name, species_id, breed_id, age, gender, color, " +
                        $"size1, weight, shelter_id, status, description, admission_date");

                    if (adoptionDate.HasValue)
                        writer.Write(", adoption_date");

                    writer.Write($", is_neutered, is_house_trained, is_good_with_kids, is_good_with_cats, is_good_with_dogs) VALUES " +
                        $"({i}, '{Helpers.EscapeSqlString(name)}', {speciesId}, {breedId}, {age}, '{gender}', '{Helpers.EscapeSqlString(color)}', " +
                        $"'{size}', {weight.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}, {shelterId}, '{status}', '{Helpers.EscapeSqlString(description)}', " +
                        $"TO_DATE('{admissionDate:yyyy-MM-dd}', 'YYYY-MM-DD')");

                    if (adoptionDate.HasValue)
                        writer.Write($", TO_DATE('{adoptionDate.Value:yyyy-MM-dd}', 'YYYY-MM-DD')");

                    writer.WriteLine($", {(isNeutered ? 1 : 0)}, {(isHouseTrained ? 1 : 0)}, " +
                        $"{(isGoodWithKids ? 1 : 0)}, {(isGoodWithCats ? 1 : 0)}, {(isGoodWithDogs ? 1 : 0)});");

                    animalIds.Add(i);

                    if (i % 1000 == 0)
                    {
                        Console.WriteLine($"Generated {i} animal records...");
                        writer.WriteLine("COMMIT;"); // Időnként commitolunk
                    }
                }

                writer.WriteLine();
                writer.WriteLine("COMMIT;");
            }

            Console.WriteLine($"Generated {recordCount} animal records successfully!");
        }

        /// <summary>
        /// Fajhoz tartozó fajtát ad vissza
        /// </summary>
        private static int GetCompatibleBreedId(int speciesId, Dictionary<int, int> breedSpeciesMap, List<int> breedIds)
        {
            List<int> compatibleBreeds = new List<int>();

            foreach (var entry in breedSpeciesMap)
            {
                if (entry.Value == speciesId)
                {
                    compatibleBreeds.Add(entry.Key);
                }
            }

            if (compatibleBreeds.Count > 0)
            {
                return compatibleBreeds[_random.Next(compatibleBreeds.Count)];
            }
            else
            {
                // Ha nincs találat, akkor alapértelmezett érték (első fajta)
                return breedIds[0];
            }
        }

        /// <summary>
        /// Véletlenszerű állatnév generálása
        /// </summary>
        private static string GetRandomAnimalName(int speciesId)
        {
            // Faj-specifikus nevek
            switch (speciesId)
            {
                case 1: // Dogs
                    return Helpers.GetRandomFromArray(new string[] {
                        "Max", "Bella", "Charlie", "Luna", "Cooper", "Lucy", "Buddy", "Daisy", "Rocky", "Lola",
                        "Tucker", "Sadie", "Duke", "Stella", "Oliver", "Ruby", "Bailey", "Finn", "Coco", "Bear",
                        "Rosie", "Scout", "Diesel", "Lulu", "Jake", "Sophie", "Rex", "Gracie", "Zeus", "Penny",
                        "Bruno", "Molly", "Jack", "Maggie", "Murphy", "Emma", "Toby", "Zoe", "Winston", "Lily",
                        "Leo", "Dixie", "Sam", "Princess", "Dexter", "Roxy", "Baxter", "Stella", "Gus", "Ellie"
                    });

                case 2: // Cats
                    return Helpers.GetRandomFromArray(new string[] {
                        "Oliver", "Luna", "Leo", "Lily", "Milo", "Bella", "Simba", "Chloe", "Max", "Lucy",
                        "Felix", "Nala", "Charlie", "Sophie", "Jack", "Zoe", "Tiger", "Cleo", "Oscar", "Molly",
                        "Jasper", "Kitty", "Simon", "Willow", "Salem", "Lola", "Mittens", "Daisy", "Oreo", "Pepper",
                        "Shadow", "Lily", "Smokey", "Lulu", "Tigger", "Missy", "Gizmo", "Coco", "Lucky", "Angel",
                        "Boots", "Cali", "Pumpkin", "Ginger", "Sasha", "Romeo", "Sammy", "Princess", "Misty", "Whiskers"
                    });

                case 3: // Rabbits
                    return Helpers.GetRandomFromArray(new string[] {
                        "Thumper", "Coco", "Oreo", "Daisy", "Bunny", "Cotton", "Hoppy", "Snowball", "Pepper", "Biscuit",
                        "Peter", "Fluffy", "Bugs", "Clover", "Midnight", "Nibbles", "Toffee", "Honey", "Flopsy", "Mopsy",
                        "Cottontail", "Benjamin", "Hazel", "Thistle", "Holly", "Sage", "Basil", "Dill", "Nutmeg", "Ginger",
                        "Peanut", "Whiskers", "Shadow", "Dusty", "Cocoa", "Caramel", "Brownie", "Spot", "Speckle", "Dot"
                    });

                case 4: // Birds
                    return Helpers.GetRandomFromArray(new string[] {
                        "Tweety", "Polly", "Rio", "Blue", "Sky", "Sunny", "Echo", "Piper", "Kiwi", "Robin",
                        "Phoenix", "Pepper", "Angel", "Charlie", "Skye", "Zazu", "Coco", "Joey", "Kiki", "Olive",
                        "Peaches", "Mango", "Peanut", "Jasper", "Bella", "Oscar", "Riley", "Ziggy", "Zoe", "Ruby"
                    });

                case 5: // Hamsters
                    return Helpers.GetRandomFromArray(new string[] {
                        "Hammy", "Peanut", "Gizmo", "Nibbles", "Squeaky", "Cookie", "Biscuit", "Whiskers", "Scooter", "Oreo",
                        "Nugget", "Pumpkin", "Pepper", "Teddy", "Button", "Tiny", "Cotton", "Marshmallow", "Butterscotch", "Caramel"
                    });

                case 6: // Guinea Pigs
                    return Helpers.GetRandomFromArray(new string[] {
                        "Piggy", "Guinness", "Squeaky", "Oreo", "Cookie", "Brownie", "Patches", "Peanut", "Daisy", "Ginger",
                        "Pepper", "Cinnamon", "Nutmeg", "Marshmallow", "Butter", "Popcorn", "Noodle", "Muffin", "Cupcake", "Biscuit"
                    });

                default:
                    // Általános nevek, ha nincs faj-specifikus
                    return Helpers.GetRandomFromArray(new string[] {
                        "Buddy", "Max", "Charlie", "Bella", "Lucy", "Daisy", "Rocky", "Lily", "Luna", "Oliver",
                        "Ruby", "Sophie", "Coco", "Pepper", "Ginger", "Cookie", "Lucky", "Peanut", "Nemo", "Angel",
                        "Shadow", "Jasper", "Milo", "Oreo", "Rusty", "Misty", "Leo", "Toby", "Roxy", "Zoey",
                        "Simba", "Tiger", "Princess", "Teddy", "Midnight", "Sammy", "Sasha", "Felix", "Sunny", "Cocoa"
                    });
            }
        }

        /// <summary>
        /// Véletlenszerű életkor generálása
        /// </summary>
        private static int GetRandomAge(int speciesId)
        {
            switch (speciesId)
            {
                case 1: // Dogs
                    return _random.Next(1, 13);
                case 2: // Cats
                    return _random.Next(1, 16);
                case 3: // Rabbits
                    return _random.Next(1, 9);
                case 4: // Birds
                    return _random.Next(1, 21);
                case 5: // Hamsters
                    return _random.Next(1, 3);
                case 6: // Guinea Pigs
                    return _random.Next(1, 6);
                case 15: // Snakes
                    return _random.Next(1, 21);
                case 16: // Fish
                    return _random.Next(1, 6);
                default:
                    return _random.Next(1, 11);
            }
        }

        /// <summary>
        /// Véletlenszerű szín generálása
        /// </summary>
        private static string GetRandomColor(int speciesId)
        {
            switch (speciesId)
            {
                case 1: // Dogs
                    return Helpers.GetRandomFromArray(new string[] {
                        "Black", "White", "Brown", "Golden", "Gray", "Cream", "Black and White",
                        "Brown and White", "Tricolor", "Brindle", "Red", "Blue", "Fawn", "Merle",
                        "Tan", "Spotted", "Sable", "Yellow", "Chocolate"
                    });

                case 2: // Cats
                    return Helpers.GetRandomFromArray(new string[] {
                        "Black", "White", "Gray", "Orange", "Cream", "Brown", "Tabby", "Calico",
                        "Tortoiseshell", "Black and White", "Gray and White", "Orange and White",
                        "Siamese", "Spotted", "Tuxedo", "Silver", "Blue"
                    });

                case 3: // Rabbits
                    return Helpers.GetRandomFromArray(new string[] {
                        "White", "Black", "Brown", "Gray", "Cream", "Spotted", "Agouti",
                        "Sable", "Black and White", "Brown and White", "Tan", "Blue", "Fawn"
                    });

                case 4: // Birds
                    return Helpers.GetRandomFromArray(new string[] {
                        "Green", "Blue", "Yellow", "Red", "Orange", "White", "Gray", "Black",
                        "Multi-colored", "Red and Green", "Blue and Yellow", "Purple", "Pink"
                    });

                case 5: // Hamsters
                    return Helpers.GetRandomFromArray(new string[] {
                        "Golden", "White", "Cream", "Gray", "Black", "Brown", "Spotted",
                        "Black and White", "Cinnamon", "Sable"
                    });

                case 6: // Guinea Pigs
                    return Helpers.GetRandomFromArray(new string[] {
                        "White", "Black", "Brown", "Cream", "Orange", "Tricolor", "Agouti",
                        "Himalayan", "Tortoiseshell", "Spotted", "Roan"
                    });

                default:
                    return Helpers.GetRandomFromArray(new string[] {
                        "Black", "White", "Brown", "Gray", "Golden", "Cream", "Multi-colored",
                        "Spotted", "Striped", "Mixed", "Red", "Blue", "Yellow", "Green", "Orange"
                    });
            }
        }

        /// <summary>
        /// Véletlenszerű méret generálása
        /// </summary>
        private static string GetRandomSize(int speciesId)
        {
            switch (speciesId)
            {
                case 1: // Dogs
                    return Helpers.GetRandomFromArray(new string[] { "Small", "Medium", "Large", "X-Large" });

                case 2: // Cats
                    return Helpers.GetRandomFromArray(new string[] { "Small", "Medium", "Large" });

                case 3: // Rabbits
                    return Helpers.GetRandomFromArray(new string[] { "Small", "Medium", "Large" });

                case 4: // Birds
                    return Helpers.GetRandomFromArray(new string[] { "Tiny", "Small", "Medium", "Large" });

                case 5: // Hamsters
                    return Helpers.GetRandomFromArray(new string[] { "Tiny", "Small" });

                case 6: // Guinea Pigs
                    return Helpers.GetRandomFromArray(new string[] { "Small", "Medium" });

                default:
                    return Helpers.GetRandomFromArray(new string[] { "Small", "Medium", "Large" });
            }
        }

        /// <summary>
        /// Véletlenszerű súly generálása
        /// </summary>
        private static double GetRandomWeight(int speciesId, string size)
        {
            switch (speciesId)
            {
                case 1: // Dogs
                    switch (size.ToLower())
                    {
                        case "small": return 2 + _random.NextDouble() * 7; // 2-9 kg
                        case "medium": return 10 + _random.NextDouble() * 15; // 10-25 kg
                        case "large": return 26 + _random.NextDouble() * 19; // 26-45 kg
                        case "x-large": return 46 + _random.NextDouble() * 44; // 46-90 kg
                        default: return 5 + _random.NextDouble() * 25; // 5-30 kg
                    }

                case 2: // Cats
                    return 2 + _random.NextDouble() * 8; // 2-10 kg

                case 3: // Rabbits
                    switch (size.ToLower())
                    {
                        case "small": return 1 + _random.NextDouble() * 2; // 1-3 kg
                        case "medium": return 3 + _random.NextDouble() * 2; // 3-5 kg
                        case "large": return 5 + _random.NextDouble() * 3; // 5-8 kg
                        default: return 2 + _random.NextDouble() * 4; // 2-6 kg
                    }

                case 4: // Birds
                    switch (size.ToLower())
                    {
                        case "tiny": return 0.01 + _random.NextDouble() * 0.09; // 0.01-0.1 kg
                        case "small": return 0.1 + _random.NextDouble() * 0.4; // 0.1-0.5 kg
                        case "medium": return 0.5 + _random.NextDouble() * 1.5; // 0.5-2 kg
                        case "large": return 2 + _random.NextDouble() * 8; // 2-10 kg
                        default: return 0.1 + _random.NextDouble() * 2; // 0.1-2.1 kg
                    }

                case 5: // Hamsters
                    return 0.05 + _random.NextDouble() * 0.15; // 0.05-0.2 kg

                case 6: // Guinea Pigs
                    return 0.5 + _random.NextDouble() * 1.5; // 0.5-2 kg

                default:
                    return 0.1 + _random.NextDouble() * 5; // 0.1-5.1 kg
            }
        }

        /// <summary>
        /// Véletlenszerű státusz generálása
        /// </summary>
        private static string GetRandomStatus()
        {
            string[] statuses = { "available", "pending", "adopted" };
            int[] weights = { 70, 10, 20 }; // 70% elérhető, 10% függőben, 20% örökbefogadott

            return Helpers.GetWeightedRandomItem(statuses, weights);
        }

        /// <summary>
        /// Véletlenszerű leírás generálása
        /// </summary>
        private static string GenerateAnimalDescription(int speciesId, int breedId, int age, string gender, string color, string size)
        {
            // Faj és fajta neve (a BreedsGenerator-ban hasonló a feldolgozása)
            string speciesName = GetSpeciesName(speciesId);
            string breedName = GetBreedNameById(breedId); // Ez egy helper függvény, amely visszaadja a fajta nevét id alapján

            // Személyiségjegyek
            string[] personalityTraits = {
                "playful", "friendly", "energetic", "calm", "gentle", "shy", "curious", "affectionate",
                "independent", "loyal", "intelligent", "social", "active", "relaxed", "timid", "brave",
                "loving", "cuddly", "outgoing", "adventurous", "docile", "spirited", "well-behaved", "cautious"
            };

            string personality = Helpers.GetRandomFromArray(personalityTraits);

            // Másodlagos személyiségjegy, ami nem ugyanaz, mint az első
            List<string> secondaryTraits = new List<string>(personalityTraits);
            secondaryTraits.Remove(personality);
            string personality2 = Helpers.GetRandomFromArray(secondaryTraits.ToArray());

            StringBuilder description = new StringBuilder();

            // Alap információk
            description.Append($"This {color}, {personality} {breedName} is a wonderful {speciesName.ToLower()}. ");

            // Kor alapján különböző leírás
            if (age < 1)
                description.Append($"This {(gender == "Male" ? "boy" : "girl")} is just a baby, only {age} months old. ");
            else if (age < 3)
                description.Append($"This young {(gender == "Male" ? "boy" : "girl")} is {age} years old. ");
            else if (age < 8)
                description.Append($"This {(gender == "Male" ? "boy" : "girl")} is {age} years old and in the prime of life. ");
            else
                description.Append($"This {(gender == "Male" ? "boy" : "girl")} is a {age}-year-old senior citizen. ");

            // Méret alapján különböző leírás
            switch (size.ToLower())
            {
                case "small":
                case "tiny":
                    description.Append($"Being small in size, {(gender == "Male" ? "he" : "she")} doesn't require much space. ");
                    break;
                case "medium":
                    description.Append($"Being medium-sized, {(gender == "Male" ? "he" : "she")} would fit well in most homes. ");
                    break;
                case "large":
                case "x-large":
                    description.Append($"Being a large {speciesName.ToLower()}, {(gender == "Male" ? "he" : "she")} needs plenty of space. ");
                    break;
            }

            // További tulajdonságok
            description.Append($"{(gender == "Male" ? "He" : "She")} is {personality} and {personality2}, ");

            // Fajspecifikus tulajdonságok
            switch (speciesId)
            {
                case 1: // Dogs
                    description.Append($"and would make a {(_random.Next(2) == 0 ? "great family pet" : "wonderful companion")}. ");
                    break;
                case 2: // Cats
                    description.Append($"and {(_random.Next(2) == 0 ? "loves to play with toys" : "enjoys lounging in the sun")}. ");
                    break;
                case 3: // Rabbits
                    description.Append($"and {(_random.Next(2) == 0 ? "enjoys munching on fresh vegetables" : "loves to hop around and explore")}. ");
                    break;
                case 4: // Birds
                    description.Append($"and {(_random.Next(2) == 0 ? "sings beautiful melodies" : "has a colorful personality")}. ");
                    break;
                case 5: // Hamsters
                    description.Append($"and {(_random.Next(2) == 0 ? "loves running on the wheel" : "enjoys burrowing in bedding")}. ");
                    break;
                case 6: // Guinea Pigs
                    description.Append($"and {(_random.Next(2) == 0 ? "makes adorable squeaking sounds" : "loves munching on fresh veggies")}. ");
                    break;
                default:
                    description.Append($"and is looking for a loving forever home. ");
                    break;
            }

            // Záró gondolat
            description.Append($"Come meet {(gender == "Male" ? "him" : "her")} today!");

            return description.ToString();
        }

        /// <summary>
        /// Faj nevének lekérdezése ID alapján
        /// </summary>
        private static string GetSpeciesName(int speciesId)
        {
            string[] speciesNames = new string[] {
                "Dog", "Cat", "Rabbit", "Bird", "Hamster", "Guinea Pig",
                "Ferret", "Rat", "Mouse", "Gerbil", "Chinchilla", "Hedgehog",
                "Turtle", "Lizard", "Snake", "Fish"
            };

            if (speciesId >= 1 && speciesId <= speciesNames.Length)
                return speciesNames[speciesId - 1];
            else
                return "Unknown Species";
        }

        /// <summary>
        /// Fajta nevének lekérdezése ID alapján (egyszerűsített verzió)
        /// </summary>
        private static string GetBreedNameById(int breedId)
        {
            // Ideális esetben ez az információ a breedSpeciesMap-ből származna, de egyszerűség kedvéért:
            if (breedId % 10 == 0)
                return "Mixed Breed"; // Minden 10. ID keverék fajta

            // Véletlenszerű fajtanevek
            string[] breedNames = {
                "Retriever", "Shepherd", "Terrier", "Bulldog", "Spaniel", "Collie", "Husky", "Poodle",
                "Siamese", "Persian", "Maine Coon", "Ragdoll", "Bengal", "Sphynx", "Manx",
                "Holland Lop", "Rex", "Dutch", "Flemish Giant", "Netherland Dwarf",
                "Parakeet", "Canary", "Finch", "Cockatiel", "Lovebird"
            };

            return breedNames[breedId % breedNames.Length];
        }

        /// <summary>
        /// Ellenőrzés, hogy ivartalanítható-e az adott faj
        /// </summary>
        private static bool CanBeNeutered(int speciesId)
        {
            // Csak emlősöket lehet ivartalanítani
            int[] neuterableSpecies = { 1, 2, 3, 5, 6, 7, 8, 9, 10, 11, 12 };
            return neuterableSpecies.Contains(speciesId);
        }

        /// <summary>
        /// Ellenőrzés, hogy szobatisztaságra nevelhető-e az adott faj
        /// </summary>
        private static bool CanBeHouseTrained(int speciesId)
        {
            // Csak bizonyos állatokat lehet szobatisztaságra nevelni
            int[] houseTrainableSpecies = { 1, 2, 3, 7 };
            return houseTrainableSpecies.Contains(speciesId);
        }

        /// <summary>
        /// Ellenőrzés, hogy lehet-e jó gyerekekkel az adott faj
        /// </summary>
        private static bool CanBeGoodWithKids(int speciesId)
        {
            // A legtöbb állat lehet jó gyerekekkel (vagy nem)
            int[] notApplicableWithKids = { 15, 16 }; // kígyók, halak
            return !notApplicableWithKids.Contains(speciesId);
        }

        /// <summary>
        /// Ellenőrzés, hogy lehet-e jó macskákkal az adott faj
        /// </summary>
        private static bool CanBeGoodWithCats(int speciesId)
        {
            // Bizonyos állatok esetén nem értelmezhető a macskákkal való viszony
            int[] notApplicableWithCats = { 2, 16, 13, 15, 14 }; // macskák, halak, teknősök, kígyók, gyíkok
            return !notApplicableWithCats.Contains(speciesId);
        }

        /// <summary>
        /// Ellenőrzés, hogy lehet-e jó kutyákkal az adott faj
        /// </summary>
        private static bool CanBeGoodWithDogs(int speciesId)
        {
            // Bizonyos állatok esetén nem értelmezhető a kutyákkal való viszony
            int[] notApplicableWithDogs = { 1, 16, 13, 15, 14 }; // kutyák, halak, teknősök, kígyók, gyíkok
            return !notApplicableWithDogs.Contains(speciesId);
        }
    }
}
