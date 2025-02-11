using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSC365_Project1.Models;
using CsvHelper.Configuration;
using CsvHelper;

namespace CSC365_Project1
{
    internal class VaxOperations
    {

        //each string is a line in the csv file
        public string CombinedDataCovidVax(List<VaxInfo> vaxInfos, List<PatientRec> patientRecs, List<Symptoms> symptoms)
        {
            string headerLine = "VAERS_ID,RECVDATE,STATE,AGE_YRS,CAGE_YR,CAGE_MO,SEX,RPT_DATE,SYMPTOM_TEXT,DIED,DATEDIED,L_THREAT,ER_VISIT,HOSPITAL,HOSPDAYS," +
                "X_STAY,DISABLE,RECOVD,VAX_DATE,ONSET_DATE,NUMDAYS,LAB_DATA,V_ADMINBY,V_FUNDBY,OTHER_MEDS,CUR_ILL,HISTORY,PRIOR_VAX,SPLTTYPE,FORM_VERS,TODAYS_DATE," +
                "BIRTH_DEFECT,OFC_VISIT,ER_ED_VISIT,ALLERGIES,VAX_TYPE,VAX_MANU,VAX_LOT,VAX_DOSE_SERIES,VAX_ROUTE,VAX_SITE,VAX_NAME,SYMPTOM1,SYMPTOMVERSION1,SYMPTOM2," +
                "SYMPTOMVERSION2,SYMPTOM3,SYMPTOMVERSION3,SYMPTOM4,SYMPTOMVERSION4,SYMPTOM5,SYMPTOMVERSION5";
            List<string> rtn = new List<string>();
            rtn.Add(headerLine);

            var joinedList = from vax in vaxInfos.Where(x => x.VAX_TYPE == "COVID19") //vax is a name for each thing in covidVaxInfos
                             join pat in patientRecs on vax.VAERS_ID equals pat.VAERS_ID into vaxPat
                             from pat in vaxPat.DefaultIfEmpty()
                             join sym in symptoms on vax.VAERS_ID equals sym.VAERS_ID into vaxPatSym
                             from sym in vaxPatSym.DefaultIfEmpty()
                             select new
                             {
                                 VAERS_ID = vax.VAERS_ID,
                                 RECVDATE = pat?.RECVDATE ?? string.Empty,
                                 STATE = pat?.STATE ?? string.Empty,
                                 AGE_YRS = pat?.AGE_YRS ?? string.Empty,
                                 CAGE_YR = pat?.CAGE_YR ?? string.Empty,
                                 CAGE_MO = pat?.CAGE_MO ?? string.Empty,
                                 SEX = pat?.SEX ?? string.Empty,
                                 RPT_DATE = pat?.RPT_DATE ?? string.Empty,
                                 SYMPTOM_TEXT = pat?.SYMPTOM_TEXT ?? string.Empty,
                                 DIED = pat?.DIED ?? string.Empty,
                                 DATEDIED = pat?.DATEDIED ?? string.Empty,
                                 L_THREAT = pat?.L_THREAT ?? string.Empty,
                                 ER_VISIT = pat?.ER_VISIT ?? string.Empty,
                                 HOSPITAL = pat?.HOSPITAL ?? string.Empty,
                                 HOSPDAYS = pat?.HOSPDAYS ?? string.Empty,
                                 X_STAY = pat?.X_STAY ?? string.Empty,
                                 DISABLE = pat?.DISABLE ?? string.Empty,
                                 RECOVD = pat?.RECOVD ?? string.Empty,
                                 VAX_DATE = pat?.VAX_DATE ?? string.Empty,
                                 ONSET_DATE = pat?.ONSET_DATE ?? string.Empty,
                                 NUMDAYS = pat?.NUMDAYS ?? string.Empty,
                                 LAB_DATA = pat?.LAB_DATA ?? string.Empty,
                                 V_ADMINBY = pat?.V_ADMINBY ?? string.Empty,
                                 V_FUNDBY = pat?.V_FUNDBY ?? string.Empty,
                                 OTHER_MEDS = pat?.OTHER_MEDS ?? string.Empty,
                                 CUR_ILL = pat?.CUR_ILL ?? string.Empty,
                                 HISTORY = pat?.HISTORY ?? string.Empty,
                                 PRIOR_VAX = pat?.PRIOR_VAX ?? string.Empty,
                                 SPLTTYPE = pat?.SPLTTYPE ?? string.Empty,
                                 FORM_VERS = pat?.FORM_VERS ?? string.Empty,
                                 TODAYS_DATE = pat?.TODAYS_DATE ?? string.Empty,
                                 BIRTH_DEFECT = pat?.BIRTH_DEFECT ?? string.Empty,
                                 OFC_VISIT = pat?.OFC_VISIT ?? string.Empty,
                                 ER_ED_VISIT = pat?.ER_ED_VISIT ?? string.Empty,
                                 ALLERGIES = pat?.ALLERGIES ?? string.Empty,
                                 VAX_TYPE = vax.VAX_TYPE,
                                 VAX_MANU = vax.VAX_MANU,
                                 VAX_LOT = vax.VAX_LOT,
                                 VAX_DOSE_SERIES = vax.VAX_DOSE_SERIES,
                                 VAX_ROUTE = vax.VAX_ROUTE,
                                 VAX_SITE = vax.VAX_SITE,
                                 VAX_NAME = vax.VAX_NAME,
                                 SYMPTOM1 = sym?.SYMPTOM1 ?? string.Empty,
                                 SYMPTOMVERSION1 = sym?.SYMPTOMVERSION1 ?? string.Empty,
                                 SYMPTOM2 = sym?.SYMPTOM2 ?? string.Empty,
                                 SYMPTOMVERSION2 = sym?.SYMPTOMVERSION2 ?? string.Empty,
                                 SYMPTOM3 = sym?.SYMPTOM3 ?? string.Empty,
                                 SYMPTOMVERSION3 = sym?.SYMPTOMVERSION3 ?? string.Empty,
                                 SYMPTOM4 = sym?.SYMPTOM4 ?? string.Empty,
                                 SYMPTOMVERSION4 = sym?.SYMPTOMVERSION4 ?? string.Empty,
                                 SYMPTOM5 = sym?.SYMPTOM5 ?? string.Empty,
                                 SYMPTOMVERSION5 = sym?.SYMPTOMVERSION5 ?? string.Empty
                             };

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using (var writer = new StringWriter())
            using (var csv = new CsvWriter(writer, csvConfig))
            {
                // Write the records to the CSV
                csv.WriteRecords(joinedList.Take(1000));

                // Get the CSV content as a string
                return writer.ToString();
            }
        }
    }
}
