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

        // Lookup tables for inverse selection
        static Dictionary<string, string> LookupMarketSelection = new();
        static Dictionary<string, (string, string)> LookupCountrySelection = new();
        static Dictionary<string, (string, string, string)> LookupCompanySelection = new();


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
            var elmentsUpToMarket = geographyElementsList.Select(x => (x.Zone, x.Market)).Distinct().ToList();
            elmentsUpToMarket.ForEach(x => LookupMarketSelection[x.Market] = x.Zone);

            var elementsUpToCountry = geographyElementsList.Select(x => (x.Zone, x.Market, x.Country)).Distinct().ToList();
            elementsUpToCountry.ForEach(x => LookupCountrySelection[x.Country] = (x.Zone, x.Market));

            var elementsUpToCompany = geographyElementsList.Select(x => (x.Zone, x.Market, x.Country, x.DisplayCompany)).Distinct().ToList();
            elementsUpToCompany.ForEach(x => LookupCompanySelection[x.DisplayCompany] = (x.Zone, x.Market, x.Country));
        }

        static void LoadContentsFromDB()
        { 
            var csvFile = "masterdata_geo.csv";
            var lines = File.ReadLines(csvFile).ToList();

            lines.ForEach(line => geographyElementsList.Add(new GeographyElement(line)));
        }

        static (string, string, string, string) GetReverseFromPlant(string plantName)
        {
            var element = geographyElementsList.Where(x => x.DisplayPlant == plantName).FirstOrDefault();
            return (element.Zone, element.Market, element.Country, element.DisplayCompany);
        }

        static (string, string, string) GetReverseFromCompany(string companyName)
            => LookupCompanySelection[companyName];

        static (string, string) GetReverseFromCountry(string countryName)
            => LookupCountrySelection[countryName];

        static string GetReverseFromMarket(string marketName)
            => LookupMarketSelection[marketName];



    }
}