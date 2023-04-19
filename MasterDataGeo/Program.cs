namespace MasterDataGeo
{
    internal class Program
    {

        // List of the database contents
        static List<GeographyElement> geographyElementsList = new();

        // Lists of all the initial elements - without filter
        static List<string?> allTheZones = new();
        static List<string?> allTheMarkets = new();
        static List<string?> allTheCountries = new();
        static List<string> allTheCompanies = new();
        static List<string> allThePlants = new();


        static void Main(string[] args)
        {
            Console.WriteLine("Step 1 - Read contents from DB");
            LoadContentsFromDB();

            Console.WriteLine("Step 2 - Generate the initial contents of each Dropdown");

            allTheZones = geographyElementsList.Select(x => x.Zone).Distinct().ToList();
            allTheMarkets = geographyElementsList.Select(x => x.Market).Distinct().ToList();
            allTheCountries = geographyElementsList.Select(x => x.Country).Distinct().ToList();
            allTheCompanies = geographyElementsList.Select(x => x.DisplayCompany).Distinct().ToList();
            allThePlants = geographyElementsList.Select(x => x.DisplayPlant).Distinct().ToList();

            Console.WriteLine("Step 3 - Generate the Lookup tables for reverse selection");
            




        }

        static void LoadContentsFromDB()
        { 
            var csvFile = "masterdata_geo.csv";
            var lines = File.ReadLines(csvFile).ToList();

            lines.ForEach(line => geographyElementsList.Add(new GeographyElement(line)));
        }
    }
}