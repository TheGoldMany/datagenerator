using AnimalShelterDataGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalShelterDataGenerator.Generators
{
    public static class BreedsGenerator
    {
        private static Random _random = new Random();

        public static void Generate(string outputPath, List<int> speciesIds, List<int> breedIds, Dictionary<int, int> breedSpeciesMap)
        {
            Console.WriteLine("Generating Breeds data...");

            // Dictionary a fajokhoz tartozó fajtákkal
            Dictionary<int, List<string>> breedsBySpecies = new Dictionary<int, List<string>>
            {
                // Dogs (species_id = 1)
                { 1, new List<string> {
                    "Labrador Retriever", "German Shepherd", "Golden Retriever", "Bulldog", "Beagle",
                    "Poodle", "Rottweiler", "Yorkshire Terrier", "Boxer", "Dachshund", "Siberian Husky",
                    "Doberman Pinscher", "Great Dane", "Miniature Schnauzer", "Shih Tzu", "Boston Terrier",
                    "Bernese Mountain Dog", "Pomeranian", "Havanese", "Shetland Sheepdog", "Border Collie",
                    "French Bulldog", "Cocker Spaniel", "Cavalier King Charles Spaniel", "Chihuahua",
                    "Pug", "Australian Shepherd", "Pembroke Welsh Corgi", "Bichon Frise", "Dalmatian",
                    "Maltese", "Collie", "English Springer Spaniel", "Basset Hound", "Mixed Breed"
                }},
                
                // Cats (species_id = 2)
                { 2, new List<string> {
                    "Abyssinian", "American Shorthair", "Bengal", "British Shorthair", "Devon Rex",
                    "Maine Coon", "Persian", "Ragdoll", "Russian Blue", "Siamese", "Sphynx", "Birman",
                    "Burmese", "Cornish Rex", "Egyptian Mau", "Exotic Shorthair", "Himalayan", "Manx",
                    "Norwegian Forest", "Oriental", "Scottish Fold", "Siberian", "Turkish Angora",
                    "Domestic Shorthair", "Domestic Longhair", "Mixed Breed"
                }},
                
                // Rabbits (species_id = 3)
                { 3, new List<string> {
                    "Dutch", "Mini Lop", "Holland Lop", "Netherland Dwarf", "Flemish Giant", "Rex",
                    "Lionhead", "English Spot", "Polish", "Californian", "Mini Rex", "Belgian Hare",
                    "Angora", "Jersey Wooly", "French Lop", "Mixed Breed"
                }},
                
                // Birds (species_id = 4)
                { 4, new List<string> {
                    "Canary", "Budgerigar (Budgie)", "Cockatiel", "Lovebird", "Finch", "African Grey Parrot",
                    "Cockatoo", "Macaw", "Amazon Parrot", "Conure", "Dove", "Parakeet", "Mixed Breed"
                }},
                
                // Hamsters (species_id = 5)
                { 5, new List<string> {
                    "Syrian (Golden) Hamster", "Chinese Hamster", "Roborovski Dwarf Hamster",
                    "Campbell's Dwarf Hamster", "Winter White Russian Dwarf Hamster", "Mixed Breed"
                }},
                
                // Guinea Pigs (species_id = 6) 
                { 6, new List<string> {
                    "American Guinea Pig", "Abyssinian Guinea Pig", "Peruvian Guinea Pig", "Silkie Guinea Pig",
                    "Teddy Guinea Pig", "Texel Guinea Pig", "Skinny Pig", "Mixed Breed"
                }},
                
                // Remaining species with simpler breed lists
                { 7, new List<string> { "Domestic Ferret", "Angora Ferret", "Albino Ferret", "Mixed Breed" } }, // Ferrets
                { 8, new List<string> { "Fancy Rat", "Dumbo Rat", "Rex Rat", "Hairless Rat", "Mixed Breed" } }, // Rats
                { 9, new List<string> { "Fancy Mouse", "Field Mouse", "House Mouse", "Mixed Breed" } }, // Mice
                { 10, new List<string> { "Mongolian Gerbil", "Fat-tailed Gerbil", "Mixed Breed" } }, // Gerbils
                { 11, new List<string> { "Standard Chinchilla", "Beige Chinchilla", "White Chinchilla", "Mixed Breed" } }, // Chinchillas
                { 12, new List<string> { "African Pygmy Hedgehog", "European Hedgehog", "Mixed Breed" } }, // Hedgehogs
                { 13, new List<string> { "Red-Eared Slider", "Box Turtle", "Painted Turtle", "Mixed Breed" } }, // Turtles
                { 14, new List<string> { "Bearded Dragon", "Leopard Gecko", "Crested Gecko", "Anole", "Mixed Breed" } }, // Lizards
                { 15, new List<string> { "Ball Python", "Corn Snake", "King Snake", "Garter Snake", "Mixed Breed" } }, // Snakes
                { 16, new List<string> { "Goldfish", "Betta Fish", "Guppy", "Tetra", "Angelfish", "Mixed Breed" } }  // Fish
            };

            int breedId = 1;
            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("-- Breeds data generation script");
                writer.WriteLine("-- Generated: " + DateTime.Now.ToString());
                writer.WriteLine();

                foreach (var species in breedsBySpecies)
                {
                    int speciesId = species.Key;
                    foreach (var breedName in species.Value)
                    {
                        string sizeCategory = GetRandomBreedSize(speciesId);
                        string lifespan = GetRandomLifespan(speciesId);
                        string description = $"{breedName} is a breed of {GetSpeciesName(speciesId)}.";

                        writer.WriteLine($"INSERT INTO Breeds (breed_id, species_id, name, description, size_category, lifespan) VALUES " +
                                        $"({breedId}, {speciesId}, '{breedName}', '{description}', '{sizeCategory}', '{lifespan}');");

                        breedIds.Add(breedId);
                        breedSpeciesMap[breedId] = speciesId;
                        breedId++;
                    }
                }

                writer.WriteLine();
                writer.WriteLine("COMMIT;");
            }

            Console.WriteLine($"Generated {breedId - 1} breed records successfully!");
        }

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

        private static string GetRandomBreedSize(int speciesId)
        {
            switch (speciesId)
            {
                case 1: // Dogs
                    return Helpers.GetRandomFromArray(new string[] { "Small", "Medium", "Large", "Giant" });
                case 2: // Cats
                    return Helpers.GetRandomFromArray(new string[] { "Small", "Medium", "Large" });
                case 3: // Rabbits
                    return Helpers.GetRandomFromArray(new string[] { "Dwarf", "Small", "Medium", "Large", "Giant" });
                case 4: // Birds
                    return Helpers.GetRandomFromArray(new string[] { "Tiny", "Small", "Medium", "Large" });
                default:
                    return Helpers.GetRandomFromArray(new string[] { "Small", "Medium", "Large" });
            }
        }

        private static string GetRandomLifespan(int speciesId)
        {
            switch (speciesId)
            {
                case 1: // Dogs
                    return _random.Next(8, 16) + " years";
                case 2: // Cats
                    return _random.Next(12, 21) + " years";
                case 3: // Rabbits
                    return _random.Next(6, 13) + " years";
                case 4: // Birds
                    return _random.Next(5, 26) + " years";
                case 5: // Hamsters
                    return _random.Next(2, 5) + " years";
                case 6: // Guinea Pigs
                    return _random.Next(4, 9) + " years";
                case 16: // Fish
                    return _random.Next(2, 11) + " years";
                default:
                    return _random.Next(2, 16) + " years";
            }
        }
    }
}
