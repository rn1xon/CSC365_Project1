using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSC365_Project1.Models;
using CsvHelper.Configuration;
using CsvHelper;
using System.Xml.XPath;

namespace CSC365_Project1
{
    internal class CollectionsOperations
    {
        /// <summary>
        /// Combines the data from the 3 Lists by VAERS_ID and returns an IEnumerable of the Anonymous type created from the 3 lists
        /// </summary>
        /// <param name="vaxInfos"></param>
        /// <param name="patientRecs"></param>
        /// <param name="symptoms"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetCombinedDataSet(List<VaxInfo> vaxInfos, List<PatientRec> patientRecs, List<Symptoms> symptoms)
        {
            var joinedList = from vax in vaxInfos
                             join pat in patientRecs on vax.VAERS_ID equals pat.VAERS_ID into vaxPat
                             from pat in vaxPat.DefaultIfEmpty()
                             join sym in symptoms on vax.VAERS_ID equals sym.VAERS_ID into vaxPatSym
                             from sym in vaxPatSym.DefaultIfEmpty()
                             select new
                             {
                                 vax.VAERS_ID,
                                 RECVDATE = pat?.RECVDATE ?? string.Empty,
                                 STATE = pat?.STATE ?? string.Empty,
                                 AGE_YRS = pat?.AGE_YRS,
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
                                 vax.VAX_TYPE,
                                 vax.VAX_MANU,
                                 vax.VAX_LOT,
                                 vax.VAX_DOSE_SERIES,
                                 vax.VAX_ROUTE,
                                 vax.VAX_SITE,
                                 vax.VAX_NAME,
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

            return joinedList;
        }

        public IEnumerable<SymptomTask2> CreateSymptomDatasetFromCombinedDataSet(IEnumerable<dynamic> originalList)
        {
            var selectedPropertiesList = originalList.SelectMany(item => new[]
            {
                new SymptomTask2() { VAERS_ID = item.VAERS_ID, AGE_YRS = item.AGE_YRS, SEX = item.SEX, VAX_NAME = item.VAX_NAME, RPT_DATE = item.RPT_DATE, SYMPTOM = item.SYMPTOM1, DIED = item.DIED, DATEDIED = item.DATEDIED, SYMPTOM_TEXT = item.SYMPTOM_TEXT },
                new SymptomTask2() { VAERS_ID = item.VAERS_ID, AGE_YRS = item.AGE_YRS, SEX = item.SEX, VAX_NAME = item.VAX_NAME, RPT_DATE = item.RPT_DATE, SYMPTOM = item.SYMPTOM2, DIED = item.DIED, DATEDIED = item.DATEDIED, SYMPTOM_TEXT = item.SYMPTOM_TEXT },
                new SymptomTask2() { VAERS_ID = item.VAERS_ID, AGE_YRS = item.AGE_YRS, SEX = item.SEX, VAX_NAME = item.VAX_NAME, RPT_DATE = item.RPT_DATE, SYMPTOM = item.SYMPTOM3, DIED = item.DIED, DATEDIED = item.DATEDIED, SYMPTOM_TEXT = item.SYMPTOM_TEXT },
                new SymptomTask2() { VAERS_ID = item.VAERS_ID, AGE_YRS = item.AGE_YRS, SEX = item.SEX, VAX_NAME = item.VAX_NAME, RPT_DATE = item.RPT_DATE, SYMPTOM = item.SYMPTOM4, DIED = item.DIED, DATEDIED = item.DATEDIED, SYMPTOM_TEXT = item.SYMPTOM_TEXT },
                new SymptomTask2() { VAERS_ID = item.VAERS_ID, AGE_YRS = item.AGE_YRS, SEX = item.SEX, VAX_NAME = item.VAX_NAME, RPT_DATE = item.RPT_DATE, SYMPTOM = item.SYMPTOM5, DIED = item.DIED, DATEDIED = item.DATEDIED, SYMPTOM_TEXT = item.SYMPTOM_TEXT }
            })
            .Where(x => !string.IsNullOrEmpty(x.SYMPTOM));

            return selectedPropertiesList;
        }

        /// <summary>
        /// Utilizing the dataset you created for TASK2, sort the data, grouped by age group 
        /// (<1 year, 1-3 years, 4-11, 12-18, 19-30, 31-40, 41-50, 51-60, 61-70, 71-80, > 80), 
        /// then by gender, then by Vaccine Name, then by symptom. For each age group, 
        /// compute the number of cases that have been reported as resulting in death. 
        /// While there might be multiple symptoms reported, each VAERSID should be counted only once.
        /// </summary>
        public IEnumerable<Task3Grouping> GetTask3DataSet(IEnumerable<SymptomTask2> symptomData)
        {
            List<AgeBucket> ageBuckets = new List<AgeBucket>
            {
                new() { Label = "< 1", MinAge = 0, MaxAge = 1 },
                new() { Label = "1-3", MinAge = 1, MaxAge = 3 },
                new() { Label = "4-11", MinAge = 4, MaxAge = 11 },
                new() { Label = "12-18", MinAge = 12, MaxAge = 18 },
                new() { Label = "19-30", MinAge = 19, MaxAge = 30 },
                new() { Label = "31-40", MinAge = 31, MaxAge = 40 },
                new() { Label = "41-50", MinAge = 41, MaxAge = 50 },
                new() { Label = "51-60", MinAge = 51, MaxAge = 60 },
                new() { Label = "61-70", MinAge = 61, MaxAge = 70 },
                new() { Label = "71-80", MinAge = 71, MaxAge = 80 },
                new() { Label = "> 80", MinAge = 80, MaxAge = 130 }
            };

            var grouped = symptomData.GroupBy(x => new
            {
                SortOrder = ageBuckets.First(y => x.AGE_YRS >= y.MinAge && Math.Floor(x.AGE_YRS) <= y.MaxAge).MinAge,
                Bucket = ageBuckets.First(y => x.AGE_YRS >= y.MinAge && Math.Floor(x.AGE_YRS) <= y.MaxAge).Label,
                x.SEX,
                x.VAX_NAME,
                x.SYMPTOM,
                VAERSID = x.VAERS_ID // Ensure each VAERSID is counted only once
            })
            .Select(g => new
            {
                g.Key.SortOrder,
                g.Key.Bucket,
                g.Key.SEX,
                g.Key.VAX_NAME,
                g.Key.SYMPTOM,
                DeathCount = g.Any(x => !string.IsNullOrEmpty(x.DIED)) ? 1 : 0 // Compute death cases
            })
            .GroupBy(g => new { g.SortOrder, g.Bucket, g.SEX, g.VAX_NAME, g.SYMPTOM })
            .Select(g => new Task3Grouping
            {
                SortOrder = g.Key.SortOrder,
                Bucket = g.Key.Bucket,
                SEX = g.Key.SEX,
                VAX_NAME = g.Key.VAX_NAME,
                SYMPTOM = g.Key.SYMPTOM,
                DeathCount = g.Sum(x => x.DeathCount) // Sum death counts per group
            })
            .OrderBy(g => g.SortOrder) // Sort by age group
            .ThenBy(g => g.SEX) // Sort by gender within age group
            .ThenBy(g => g.VAX_NAME) // Sort by vaccine name
            .ThenBy(g => g.SYMPTOM); // Sort by symptom

            return grouped;
        }

    }
}

