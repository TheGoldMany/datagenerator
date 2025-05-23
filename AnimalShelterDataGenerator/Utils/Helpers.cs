using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalShelterDataGenerator.Utils
{
    /// <summary>
    /// Hasznos segédfüggvények gyűjteménye
    /// </summary>
    public static class Helpers
    {
        private static Random _random = new Random();

        /// <summary>
        /// Véletlenszerű szám generálása egy min és max érték között
        /// </summary>
        public static int GetRandomNumber(int min, int max)
        {
            return _random.Next(min, max + 1);
        }

        /// <summary>
        /// Véletlenszerű dátum generálása egy időintervallumban
        /// </summary>
        public static DateTime GetRandomDate(DateTime startDate, DateTime endDate)
        {
            TimeSpan timeSpan = endDate - startDate;
            TimeSpan newSpan = new TimeSpan(
                _random.Next(0, (int)timeSpan.TotalDays),
                _random.Next(0, 24),
                _random.Next(0, 60),
                _random.Next(0, 60));

            return startDate + newSpan;
        }

        /// <summary>
        /// SQL szöveg escape-elése (aposztrófok duplázása)
        /// </summary>
        public static string EscapeSqlString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return input.Replace("'", "''");
        }

        /// <summary>
        /// Véletlenszerű elem kiválasztása egy listából
        /// </summary>
        public static T GetRandomElement<T>(IList<T> list)
        {
            if (list == null || list.Count == 0)
                throw new ArgumentException("List cannot be null or empty");

            return list[_random.Next(list.Count)];
        }

        /// <summary>
        /// Véletlenszerű elem kiválasztása egy tömbből
        /// </summary>
        public static string GetRandomFromArray(string[] array)
        {
            return array[_random.Next(array.Length)];
        }

        /// <summary>
        /// Véletlenszerű logikai érték generálása adott valószínűséggel
        /// </summary>
        public static bool GetRandomBoolean(double truePercentage = 50)
        {
            return _random.NextDouble() * 100 < truePercentage;
        }

        /// <summary>
        /// Véletlenszerű decimális szám generálása
        /// </summary>
        public static double GetRandomDouble(double min, double max)
        {
            return min + (_random.NextDouble() * (max - min));
        }

        /// <summary>
        /// Súlyozott véletlenszerű elem kiválasztása
        /// </summary>
        public static string GetWeightedRandomItem(string[] items, int[] weights)
        {
            if (items == null || weights == null || items.Length != weights.Length)
                throw new ArgumentException("Items and weights arrays must have the same length");

            int totalWeight = 0;
            foreach (int weight in weights)
            {
                totalWeight += weight;
            }

            int randomValue = _random.Next(1, totalWeight + 1);
            int cumulativeWeight = 0;

            for (int i = 0; i < weights.Length; i++)
            {
                cumulativeWeight += weights[i];
                if (randomValue <= cumulativeWeight)
                {
                    return items[i];
                }
            }

            // Alapértelmezett, ha valami hiba történne
            return items[0];
        }

        /// <summary>
        /// Véletlenszerű szöveg generálása adott hosszban
        /// </summary>
        public static string GetRandomText(int minWords, int maxWords)
        {
            string[] words = {
                "animal", "shelter", "adoption", "pet", "dog", "cat", "rabbit", "care",
                "love", "family", "home", "friend", "rescue", "happy", "healthy", "playful",
                "sweet", "friendly", "gentle", "energetic", "calm", "quiet", "affectionate",
                "loyal", "smart", "training", "walk", "food", "treat", "toy", "bed", "house",
                "welcome", "forever", "companion", "behavior", "support", "help", "donate",
                "volunteer", "staff", "provide", "safety", "comfort", "secure", "heart",
                "second", "chance", "life", "soul", "beautiful", "adorable", "cute", "special"
            };

            int wordCount = _random.Next(minWords, maxWords + 1);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < wordCount; i++)
            {
                if (i > 0) sb.Append(" ");
                sb.Append(words[_random.Next(words.Length)]);

                // Néha tegyen írásjelet a szövegbe
                if (_random.Next(100) < 10 && i < wordCount - 1)
                {
                    sb.Append(_random.Next(100) < 50 ? "," : ".");
                }
            }

            // Első betűt nagybetűvé
            if (sb.Length > 0)
            {
                sb[0] = char.ToUpper(sb[0]);
            }

            // Pontot a végére
            if (!sb.ToString().EndsWith("."))
            {
                sb.Append(".");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Véletlenszerű utcanév generálása
        /// </summary>
        public static string GetRandomStreetName()
        {
            string[] streets = {
                "Maple", "Oak", "Pine", "Cedar", "Elm", "Walnut", "Cherry", "Spruce", "Birch", "Ash",
                "Washington", "Lincoln", "Jefferson", "Roosevelt", "Madison", "Adams", "Jackson", "Monroe",
                "Main", "First", "Second", "Third", "Fourth", "Fifth", "Park", "Lake", "River", "Church",
                "Highland", "Valley", "Meadow", "Forest", "Garden", "Spring", "Sunset", "Sunrise", "Hill"
            };

            string[] types = {
                "Street", "Avenue", "Road", "Boulevard", "Lane", "Drive", "Way", "Court", "Place", "Circle",
                "Trail", "Parkway", "Terrace"
            };

            return $"{GetRandomFromArray(streets)} {GetRandomFromArray(types)}";
        }

        /// <summary>
        /// Véletlenszerű város generálása
        /// </summary>
        public static string GetRandomCity()
        {
            string[] cities = {
                "New York", "Los Angeles", "Chicago", "Houston", "Phoenix", "Philadelphia",
                "San Antonio", "San Diego", "Dallas", "San Jose", "Austin", "Jacksonville",
                "Fort Worth", "Columbus", "Charlotte", "San Francisco", "Indianapolis", "Seattle",
                "Denver", "Washington DC", "Boston", "Nashville", "Portland", "Las Vegas", "Detroit",
                "Atlanta", "Miami", "St. Louis", "Tampa", "Cincinnati", "Cleveland", "Orlando",
                "Pittsburgh", "Sacramento", "Minneapolis", "New Orleans", "Kansas City", "Tucson",
                "Fresno", "Raleigh", "Omaha", "Oakland", "Tulsa", "Wichita", "Albuquerque"
            };

            return GetRandomFromArray(cities);
        }

        /// <summary>
        /// Véletlenszerű állam generálása
        /// </summary>
        public static string GetRandomState()
        {
            string[] states = {
                "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA",
                "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD",
                "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ",
                "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC",
                "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY"
            };

            return GetRandomFromArray(states);
        }

        /// <summary>
        /// Véletlenszerű teljes név generálása
        /// </summary>
        public static string GetRandomFullName()
        {
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

            return $"{GetRandomFromArray(firstNames)} {GetRandomFromArray(lastNames)}";
        }

        /// <summary>
        /// Véletlenszerű email cím generálása
        /// </summary>
        public static string GetRandomEmail(string firstName = null, string lastName = null)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                string[] names = GetRandomFullName().Split(' ');
                firstName = names[0];
                lastName = names.Length > 1 ? names[1] : "User";
            }

            string[] domains = {
                "gmail.com", "yahoo.com", "hotmail.com", "outlook.com", "aol.com",
                "icloud.com", "protonmail.com", "mail.com", "example.com"
            };

            return $"{firstName.ToLower()}.{lastName.ToLower()}{_random.Next(100, 1000)}@{GetRandomFromArray(domains)}";
        }

        /// <summary>
        /// Véletlenszerű telefonszám generálása
        /// </summary>
        public static string GetRandomPhoneNumber()
        {
            return $"({_random.Next(100, 1000)}) {_random.Next(100, 1000)}-{_random.Next(1000, 10000)}";
        }

        /// <summary>
        /// Véletlenszerű irányítószám generálása
        /// </summary>
        public static string GetRandomPostalCode()
        {
            return _random.Next(10000, 100000).ToString();
        }

        /// <summary>
        /// Véletlenszerű cím generálása
        /// </summary>
        public static string GetRandomAddress()
        {
            return $"{_random.Next(100, 10000)} {GetRandomStreetName()}";
        }

        /// <summary>
        /// Véletlenszerű mondat generálása
        /// </summary>
        public static string GetRandomSentence(int minWords = 5, int maxWords = 15)
        {
            string result = GetRandomText(minWords, maxWords);
            if (!result.EndsWith("."))
            {
                result += ".";
            }
            return result;
        }

        /// <summary>
        /// Véletlenszerű bekezdés generálása
        /// </summary>
        public static string GetRandomParagraph(int minSentences = 3, int maxSentences = 7)
        {
            int sentenceCount = _random.Next(minSentences, maxSentences + 1);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < sentenceCount; i++)
            {
                sb.Append(GetRandomSentence());
                sb.Append(" ");
            }

            return sb.ToString().Trim();
        }
    }
}
