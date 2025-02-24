// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Drawing;
using CSC365_Project1;
using CSC365_Project1.Models;

Stopwatch sw = Stopwatch.StartNew();
FileOperations fileOps = new();
CollectionsOperations collOps = new();
SortOperations sortOps = new();


// --------------------------------------------------------------------------------------------
//  Read all the VAERSVAX files and collect only COVID19 records.
//  Also fills the HashSet CovidVaersIds with each (COVID19) VAERS_ID from the files
//  The Hashset is used in subsequent operations to only read in records from the other files
//  that have VAERS_IDs from COVID19 vaccines
// --------------------------------------------------------------------------------------------
sw.Restart();
HashSet<int> CovidVaersIds = [];
List<VaxInfo> vaersVaxRecords = fileOps.ReadAllVaxInfoFiles("Data", CovidVaersIds, ["COVID19", "COVID19-2"]);
sw.Stop();
Console.WriteLine($"Vaccine Information Read: {vaersVaxRecords.Count}.  The elapsed time was: {sw.ElapsedMilliseconds}ms");
Console.WriteLine($"There are {CovidVaersIds.Count} unique VAERS_IDs collected from the various VAERSVAX files");

// --------------------------------------------------------------------------------------------
//  Read all the Patient Data files and collect only records where the VAERS_ID on the incoming
//  record is in the Hashset CovidVaersIds
// --------------------------------------------------------------------------------------------
sw.Restart();
List<PatientRec> patientDataRecs = fileOps.ReadAllPatientRecFiles("Data", CovidVaersIds);
sw.Stop();
Console.WriteLine($"Patient Records Read: {patientDataRecs.Count}.  The elapsed time was: {sw.ElapsedMilliseconds}ms");

// --------------------------------------------------------------------------------------------
//  Read all the Symptom Data files and collect only records where the VAERS_ID on the incoming
//  record is in the Hashset CovidVaersIds
// --------------------------------------------------------------------------------------------
sw.Restart();
List<Symptoms> symptomRecs = fileOps.ReadAllSymptomsFiles("Data", CovidVaersIds);
sw.Stop();
Console.WriteLine($"Symptoms Records Read: {symptomRecs.Count}.  The elapsed time was: {sw.ElapsedMilliseconds}ms");

// --------------------------------------------------------------------------------------------
//  Create VAERS_COVID_DataDecember2024 csv output file containing the combination of info
//  from all 3 sources (this is the deliverable for Task 1)
// --------------------------------------------------------------------------------------------
sw.Restart();
IEnumerable<dynamic> joinedList = collOps.GetCombinedDataSet(vaersVaxRecords, patientDataRecs, symptomRecs);
fileOps.WriteCSVFile(joinedList, "VAERS_COVID_DataDecember2024.csv");
sw.Stop();
Console.WriteLine($"VAERS_COVID_DataDecember2024.csv was created in {sw.ElapsedMilliseconds}ms and contains {joinedList.Count()} records");

// --------------------------------------------------------------------------------------------
//  Create a dataset from the set created in TASK 1, containing the following information:
//  VAERS_ID, AGE_YRS, SEX, VAX_NAME, RPT_Date, SYMPTOM, DIED, DATEDIED, SYMPTOM_TEXT.
//  Save the dataset as SYMPTOMDATA.csv.
// --------------------------------------------------------------------------------------------
sw.Restart();
IEnumerable<SymptomTask2> symptomData = collOps.CreateSymptomDatasetFromCombinedDataSet(joinedList);
fileOps.WriteCSVFile(symptomData, "SYMPTOMDATA.csv");
sw.Stop();
Console.WriteLine($"SYMPTOMDATA.csv was created in {sw.ElapsedMilliseconds}ms and contains {symptomData.Count()} records");


// --------------------------------------------------------------------------------------------
//  Implement Quicksort, InsertionSort and another Sorting algorithm of your choice (MergeSort)
//  Utilize the sorts and sort your data by VAERS_ID. Use subsets of different sizes to analyze
//  the time efficiency of the running time of the three algorithms.
//  Include the results of your empirical analyses in the final report.
// --------------------------------------------------------------------------------------------

//The below chunk of code can genertae symptomData so that the sorting can be done without the above code
//sw.Restart();
//IEnumerable<SymptomTask2> symptomData = fileOps.ReadSymptomsTask2Sort("SYMPTOMDATA.csv");
//sw.Stop();
//Console.WriteLine($"Reading SYMPTOMDATA took {sw.ElapsedMilliseconds}ms");


List<int> sizes = [500000, 1000000, 3000000, 6000000, 8000000];

foreach (int size in sizes)
{
    long qs;
    long ins;
    long ms;
    long ns;

    sw.Restart();
    SymptomTask2[] quickSortResult = sortOps.QuickSort(symptomData.Take(size)); // symptomData.Take(number) to take different sized subsets
    sw.Stop();
    qs = sw.ElapsedMilliseconds;

    if (size <= 3000000)
    {
        sw.Restart();
        SymptomTask2[] insertionSortResult = sortOps.InsertionSort(symptomData.Take(size));
        sw.Stop();
        ins = sw.ElapsedMilliseconds;
    }
    else
    {
        ins = 0;
    }

    sw.Restart();
    SymptomTask2[] mergeSortResult = sortOps.MergeSort(symptomData.Take(size));
    sw.Stop();
    ms = sw.ElapsedMilliseconds;

    sw.Restart();
    SymptomTask2[] netSortResult = sortOps.NetSort(symptomData.Take(size));
    sw.Stop();
    ns = sw.ElapsedMilliseconds;

    Console.WriteLine($"Collection Size is {size} records");
    Console.WriteLine($"QuickSort:      {qs}ms");
    Console.WriteLine($"InsertionSort:  {(ins > 0 ? ins.ToString() + "ms" : "N/A")}");
    Console.WriteLine($"MergeSort:      {ms}ms");
    Console.WriteLine($"NetSort:        {ns}ms");
    Console.WriteLine();

}

// The below methods can be put in the above code if one wants to ensure the validity of the sort
//fileOps.WriteCSVFile(quickSortResult, "QuickSort.csv");
//fileOps.WriteCSVFile(insertionSortResult, "InsertionSort.csv");
//fileOps.WriteCSVFile(mergeSortResult, "MergeSort.csv");


// --------------------------------------------------------------------------------------------
//  Utilizing the dataset you created for TASK2, sort the data, grouped by age group,
//  then by gender, then by Vaccine Name, then by symptom. For each age group,
//  compute the number of cases that have been reported as resulting in death.
//  While there might be multiple symptoms reported, each VAERSID should be counted only once.
// --------------------------------------------------------------------------------------------

IEnumerable<Task3Grouping> task3DataSet = collOps.GetTask3DataSet(symptomData);
Console.WriteLine($"Task 3 dataset {task3DataSet.Count()}");

fileOps.WriteTask3Report(task3DataSet, "Task3Report.txt");
