using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Text;

namespace AnimalShelterDataGenerator.Generators
{
    public static class TriggersGenerator
    {
        public static void Generate(string outputPath)
        {
            Console.WriteLine("Generating Triggers script...");

            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("-- Triggers generation script");
                writer.WriteLine("-- Generated: " + DateTime.Now.ToString());
                writer.WriteLine();

                // Örökbefogadási kérelem jóváhagyásakor állapot frissítése
                writer.WriteLine(@"
CREATE OR REPLACE TRIGGER trg_application_approved
AFTER UPDATE OF status ON Adoption_Applications
FOR EACH ROW
WHEN (NEW.status = 'approved' AND OLD.status = 'pending')
BEGIN
    -- Az állat státuszának frissítése ""adopted""-re
    UPDATE Animals
    SET status = 'adopted',
        adoption_date = SYSDATE
    WHERE animal_id = :NEW.animal_id;
    
    -- Többi függőben lévő kérelem elutasítása ugyanarra az állatra
    UPDATE Adoption_Applications
    SET status = 'rejected',
        review_date = SYSDATE,
        decision_reason = 'Another application was approved'
    WHERE animal_id = :NEW.animal_id
    AND application_id != :NEW.application_id
    AND status = 'pending';
END;
/");

                // Új egészségügyi rekord hozzáadásakor naplózás (csak demonstrációs célra)
                writer.WriteLine(@"
CREATE OR REPLACE TRIGGER trg_log_medical_record
AFTER INSERT ON Medical_Records
FOR EACH ROW
DECLARE
    v_animal_name VARCHAR2(50);
BEGIN
    -- Állat nevének lekérdezése
    SELECT name INTO v_animal_name
    FROM Animals
    WHERE animal_id = :NEW.animal_id;
    
    -- Esemény naplózása (szimulált művelet)
    DBMS_OUTPUT.PUT_LINE('New medical record added for animal: ' || v_animal_name ||
                         ' on date: ' || TO_CHAR(:NEW.record_date, 'YYYY-MM-DD'));
END;
/");

                // Új fotó hozzáadásakor, ha primary, akkor a többi ne legyen az
                writer.WriteLine(@"
CREATE OR REPLACE TRIGGER trg_primary_photo
BEFORE INSERT OR UPDATE ON Photos
FOR EACH ROW
WHEN (NEW.is_primary = 1)
BEGIN
    -- Minden más fotó legyen nem elsődleges ugyanannál az állatnál
    UPDATE Photos
    SET is_primary = 0
    WHERE animal_id = :NEW.animal_id
    AND photo_id != :NEW.photo_id;
END;
/");

                // Felhasználó törlésének megakadályozása, ha van aktív örökbefogadási kérelme
                writer.WriteLine(@"
CREATE OR REPLACE TRIGGER trg_prevent_user_deletion
BEFORE DELETE ON Users
FOR EACH ROW
DECLARE
    v_applications_count NUMBER;
BEGIN
    -- Ellenőrizzük, hogy a felhasználónak vannak-e aktív kérelmei
    SELECT COUNT(*) INTO v_applications_count
    FROM Adoption_Applications
    WHERE user_id = :OLD.user_id
    AND status = 'pending';
    
    -- Ha van aktív kérelem, akkor meggátoljuk a törlést
    IF v_applications_count > 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'Cannot delete user who has pending adoption applications');
    END IF;
END;
/");

                writer.WriteLine();
                writer.WriteLine("COMMIT;");
            }

            Console.WriteLine("Generated triggers script successfully!");
        }
    }
}
