using AnimalShelterDataGenerator.Generators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalShelterDataGenerator
{
    public class DataGenerator
    {
        // Listák az ID-k nyilvántartásához
        private readonly List<int> _speciesIds = new List<int>();
        private readonly List<int> _breedIds = new List<int>();
        private readonly List<int> _shelterIds = new List<int>();
        private readonly List<int> _userIds = new List<int>();
        private readonly List<int> _animalIds = new List<int>();
        private readonly List<int> _staffUserIds = new List<int>();
        private readonly Dictionary<int, int> _breedSpeciesMap = new Dictionary<int, int>();
        private readonly Dictionary<int, string> _animalStatusDict = new Dictionary<int, string>();

        // Rekordszámok
        private const int ANIMALS_COUNT = 100000;
        private const int ADOPTION_APPLICATIONS_COUNT = 150000;
        private const int MEDICAL_RECORDS_COUNT = 50000;
        private const int PHOTOS_COUNT = 200000;
        private const int REVIEWS_COUNT = 5000;
        private const int MESSAGES_COUNT = 20000;
        private const int EVENTS_COUNT = 200;
        private const int DONATIONS_COUNT = 10000;
        private const int USERS_COUNT = 1000;

        public void GenerateData()
        {
            Directory.CreateDirectory("sql_output");

            // 1. Törzsadatok generálása
            SpeciesGenerator.Generate("sql_output/01_species_data.sql", _speciesIds);
            BreedsGenerator.Generate("sql_output/02_breeds_data.sql", _speciesIds, _breedIds, _breedSpeciesMap);
            SheltersGenerator.Generate("sql_output/03_shelters_data.sql", _shelterIds);
            UsersGenerator.Generate("sql_output/04_users_data.sql", _shelterIds, _userIds, _staffUserIds, USERS_COUNT);

            // 2. Nagy táblák
            AnimalsGenerator.Generate("sql_output/05_animals_data.sql", _speciesIds, _breedIds, _shelterIds,
                _animalIds, _breedSpeciesMap, _animalStatusDict, ANIMALS_COUNT);
            AdoptionApplicationsGenerator.Generate("sql_output/06_adoption_applications_data.sql",
                _animalIds, _userIds, _staffUserIds, _animalStatusDict, ADOPTION_APPLICATIONS_COUNT);

            // 3. Többi tábla
            MedicalRecordsGenerator.Generate("sql_output/07_medical_records_data.sql", _animalIds, MEDICAL_RECORDS_COUNT);
            PhotosGenerator.Generate("sql_output/08_photos_data.sql", _animalIds, PHOTOS_COUNT);
            ReviewsGenerator.Generate("sql_output/09_reviews_data.sql", _shelterIds, _userIds, REVIEWS_COUNT);
            MessagesGenerator.Generate("sql_output/10_messages_data.sql", _userIds, _staffUserIds, MESSAGES_COUNT);
            EventsGenerator.Generate("sql_output/11_events_data.sql", _shelterIds, EVENTS_COUNT);
            DonationsGenerator.Generate("sql_output/12_donations_data.sql", _userIds, _shelterIds, DONATIONS_COUNT);

            // 4. Oracle szekvenciák és triggerek
            SequencesGenerator.Generate("sql_output/13_sequences.sql", _speciesIds.Count, _breedIds.Count,
                _shelterIds.Count, _userIds.Count, _animalIds.Count);
            TriggersGenerator.Generate("sql_output/14_triggers.sql");

            // 5. Master script
            GenerateMasterScript();
        }

        private void GenerateMasterScript()
        {
            using var writer = new StreamWriter("sql_output/00_run_all.sql");
            writer.WriteLine("-- Master script to run all SQL files in correct order");
            writer.WriteLine("-- Generated: " + DateTime.Now.ToString());
            writer.WriteLine();

            string[] files = {
            "01_species_data.sql", "02_breeds_data.sql", "03_shelters_data.sql",
            "04_users_data.sql", "05_animals_data.sql", "06_adoption_applications_data.sql",
            "07_medical_records_data.sql", "08_photos_data.sql", "09_reviews_data.sql",
            "10_messages_data.sql", "11_events_data.sql", "12_donations_data.sql",
            "13_sequences.sql", "14_triggers.sql"
        };

            foreach (var file in files)
            {
                writer.WriteLine($"@{file}");
                writer.WriteLine("COMMIT;");
                writer.WriteLine();
            }

            writer.WriteLine("SELECT 'Database successfully populated!' AS Message FROM DUAL;");
        }
    }
}