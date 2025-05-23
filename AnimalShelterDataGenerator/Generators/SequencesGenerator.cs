using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalShelterDataGenerator.Generators
{
    public static class SequencesGenerator
    {
        public static void Generate(string outputPath, int speciesCount, int breedsCount,
            int sheltersCount, int usersCount, int animalsCount)
        {
            Console.WriteLine("Generating Sequences script...");

            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("-- Sequences generation script");
                writer.WriteLine("-- Generated: " + DateTime.Now.ToString());
                writer.WriteLine();

                writer.WriteLine("CREATE SEQUENCE species_seq START WITH " + (speciesCount + 1) + " INCREMENT BY 1 NOCACHE NOCYCLE;");
                writer.WriteLine("CREATE SEQUENCE breeds_seq START WITH " + (breedsCount + 1) + " INCREMENT BY 1 NOCACHE NOCYCLE;");
                writer.WriteLine("CREATE SEQUENCE shelters_seq START WITH " + (sheltersCount + 1) + " INCREMENT BY 1 NOCACHE NOCYCLE;");
                writer.WriteLine("CREATE SEQUENCE users_seq START WITH " + (usersCount + 1) + " INCREMENT BY 1 NOCACHE NOCYCLE;");
                writer.WriteLine("CREATE SEQUENCE animals_seq START WITH " + (animalsCount + 1) + " INCREMENT BY 1 NOCACHE NOCYCLE;");
                writer.WriteLine("CREATE SEQUENCE applications_seq START WITH 150001 INCREMENT BY 1 NOCACHE NOCYCLE;");
                writer.WriteLine("CREATE SEQUENCE medical_records_seq START WITH 50001 INCREMENT BY 1 NOCACHE NOCYCLE;");
                writer.WriteLine("CREATE SEQUENCE photos_seq START WITH 200001 INCREMENT BY 1 NOCACHE NOCYCLE;");
                writer.WriteLine("CREATE SEQUENCE reviews_seq START WITH 5001 INCREMENT BY 1 NOCACHE NOCYCLE;");
                writer.WriteLine("CREATE SEQUENCE messages_seq START WITH 20001 INCREMENT BY 1 NOCACHE NOCYCLE;");
                writer.WriteLine("CREATE SEQUENCE events_seq START WITH 201 INCREMENT BY 1 NOCACHE NOCYCLE;");
                writer.WriteLine("CREATE SEQUENCE donations_seq START WITH 10001 INCREMENT BY 1 NOCACHE NOCYCLE;");

                writer.WriteLine();
                writer.WriteLine("COMMIT;");
            }

            Console.WriteLine("Generated sequences script successfully!");
        }
    }
}
