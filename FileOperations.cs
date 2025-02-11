using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CSC365_Project1.Models;

namespace CSC365_Project1
{
    internal class FileOperations
    {
        /// <summary>
        /// Reads a Patient Record file (VAERSData) and converts each line into a patient record, and returns a list of all the patient recs in that file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public List<PatientRec> ReadPatientRecFile(string fileName)
        {
            bool firstLine = true;
            List<PatientRec> rtn = new();

            string[] fileLines = File.ReadAllLines(fileName);
            foreach (string line in fileLines)
            {
                if (!firstLine)
                {
                    string[] data = line.Split(',');
                    if (int.TryParse(data[0], out int vaers_id))
                    {
                        PatientRec rec = new PatientRec();
                        rec.VAERS_ID = vaers_id;
                        rec.RECVDATE = data[1];
                        rec.STATE = data[2];
                        rec.AGE_YRS = data[3];
                        rec.CAGE_YR = data[4];
                        rec.CAGE_MO = data[5];
                        rec.SEX = data[6];
                        rec.RPT_DATE = data[7];
                        rec.SYMPTOM_TEXT = data[8];
                        rec.DIED = data[9];
                        rec.DATEDIED = data[10];
                        rec.L_THREAT = data[11];
                        rec.ER_VISIT = data[12];
                        rec.HOSPITAL = data[13];
                        rec.HOSPDAYS = data[14];
                        rec.X_STAY = data[15];
                        rec.DISABLE = data[16];
                        rec.RECOVD = data[17];
                        rec.VAX_DATE = data[18];
                        rec.ONSET_DATE = data[19];
                        rec.NUMDAYS = data[20];
                        rec.LAB_DATA = data[21];
                        rec.V_ADMINBY = data[22];
                        rec.V_FUNDBY = data[23];
                        rec.OTHER_MEDS = data[24];
                        rec.CUR_ILL = data[25];
                        rec.HISTORY = data[26];
                        rec.PRIOR_VAX = data[27];
                        rec.SPLTTYPE = data[28];
                        rec.FORM_VERS = data[29];
                        rec.TODAYS_DATE = data[30];
                        rec.BIRTH_DEFECT = data[31];
                        rec.OFC_VISIT = data[32];
                        rec.ER_ED_VISIT = data[33];
                        rec.ALLERGIES = data[34];
                        rtn.Add(rec);
                    }
                }
                firstLine = false;

            }

            return rtn;
        }

        public List<Symptoms> ReadSymptomsFile(string fileName)
        {
            bool firstLine = true;
            List<Symptoms> rtn = new();
            string[] fileLines = File.ReadAllLines(fileName);
            foreach (string line in fileLines)
            {
                if (!firstLine)
                {
                    string[] data = line.Split(',');
                    if (int.TryParse(data[0], out int vaers_id))
                    {
                        Symptoms symp = new Symptoms();
                        symp.VAERS_ID = vaers_id;
                        symp.SYMPTOM1 = data[1];
                        symp.SYMPTOMVERSION1 = data[2];
                        symp.SYMPTOM2 = data[3];
                        symp.SYMPTOMVERSION2 = data[4];
                        symp.SYMPTOM3 = data[5];
                        symp.SYMPTOMVERSION3 = data[6];
                        symp.SYMPTOM4 = data[7];
                        symp.SYMPTOMVERSION4 = data[8];
                        symp.SYMPTOM5 = data[9];
                        symp.SYMPTOMVERSION5 = data[10];
                        rtn.Add(symp);
                    }
                }
                firstLine = false;


            }
            return rtn;
        }

        public List<VaxInfo> ReadVaxInfoFile(string fileName)
        {
            List<VaxInfo> rtn = new();
            string[] fileLines = File.ReadAllLines(fileName);
            foreach (string line in fileLines.Skip(1))
            {
                string[] data = line.Split(',');
                if (int.TryParse(data[0], out int vaers_id))
                {
                    VaxInfo vax = new VaxInfo();
                    vax.VAERS_ID = vaers_id;
                    vax.VAX_TYPE = data[1];
                    vax.VAX_MANU = data[2];
                    vax.VAX_LOT = data[3];
                    vax.VAX_DOSE_SERIES = data[4];
                    vax.VAX_ROUTE = data[5];
                    vax.VAX_SITE = data[6];
                    vax.VAX_NAME = data[7];
                    rtn.Add(vax);
                }
            }
            return rtn;
        }

        /// <summary>
        /// Given a folder name, method returns a list of file names within folder that match the naming convention of the patient rec files
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public List<string> GetPatientRecFileNames(string dirName)
        {
            return Directory.GetFiles(dirName, "*VAERSData.csv").ToList();
        }
        public List<string> GetSymptomsFileNames(string dirName)
        {
            return Directory.GetFiles(dirName, "*VAERSSYMPTOMS.csv").ToList();
        }
        public List<string> GetVaxInfoFileNames(string dirName)
        {
            return Directory.GetFiles(dirName, "*VAERSVAX.csv").ToList();
        }

        public List<PatientRec> ReadAllPatientRecFiles(string dirName)
        {
            List<PatientRec> rtn = new();
            List<string> patientRecFiles = GetPatientRecFileNames(dirName);

            foreach (string fileName in patientRecFiles)
            {
                rtn.AddRange(ReadPatientRecFile(fileName));
            }

            return rtn;
        }

        public List<Symptoms> ReadAllSymptomsFiles(string dirName)
        {
            List<Symptoms> rtn = new();
            List<string> symptomFiles = GetSymptomsFileNames(dirName);

            foreach (string fileName in symptomFiles)
            {
                rtn.AddRange(ReadSymptomsFile(fileName));
            }

            return rtn;
        }

        public List<VaxInfo> ReadAllVaxInfoFiles(string dirName)
        {
            List<VaxInfo> rtn = new();
            List<string> vaxInfoFiles = GetVaxInfoFileNames(dirName);

            foreach (string fileName in vaxInfoFiles)
            {
                rtn.AddRange(ReadVaxInfoFile(fileName));
            }

            return rtn;
        }
    }
}
