// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using CSC365_Project1;
using CSC365_Project1.Models;

Stopwatch sw = Stopwatch.StartNew();
FileOperations fileOps = new FileOperations();

List<PatientRec> pr = fileOps.ReadAllPatientRecFiles("Data");
sw.Stop();
Console.WriteLine($"Patient Records Read: {pr.Count}.  The elapsed time was: {sw.ElapsedMilliseconds}ms");

sw.Restart();
List<Symptoms> sym = fileOps.ReadAllSymptomsFiles("Data");
sw.Stop();
Console.WriteLine($"Symptoms Read: {sym.Count}.  The elapsed time was: {sw.ElapsedMilliseconds}ms");

sw.Restart();
List<VaxInfo> vi = fileOps.ReadAllVaxInfoFiles("Data");
sw.Stop();
Console.WriteLine($"Vaccine Information Read: {vi.Count}.  The elapsed time was: {sw.ElapsedMilliseconds}ms");

sw.Restart();
VaxOperations vaxOps = new VaxOperations();
string taskOneCSV = vaxOps.CombinedDataCovidVax(vi, pr, sym);
if (File.Exists("VAERS_COVID_DataDecember2024.csv"))
{
    File.Delete("VAERS_COVID_DataDecember2024.csv");
}
File.WriteAllText("VAERS_COVID_DataDecember2024.csv", taskOneCSV);
sw.Stop();
Console.WriteLine($"VAERS_COVID_DataDecember2024.csv was created in {sw.ElapsedMilliseconds}ms");