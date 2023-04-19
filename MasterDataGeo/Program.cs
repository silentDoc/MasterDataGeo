namespace MasterDataGeo
{
    internal class Program
    {
        // List of the database contents
        static List<GeographyElement> geographyElementsList = new();

        // Lists of all the initial elements - without filter. To provide to the frontend in the initial load
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

            // Warning - the number of DISTINCT elements in the selection is greater than the number of KEYS in the lookup, that means we have the same key applying to 
            // different previous elements - it is very possible that masterdata is not well formed.
        }

        static void LoadContentsFromDB()
        { 
            var csvFile = "masterdata_geo.csv";
            var lines = File.ReadLines(csvFile).ToList();

            lines.ForEach(line => geographyElementsList.Add(new GeographyElement(line)));
        }

        // Reverse selection endpoints (We select something and then we have to resolve the previous elements)

        static (string?, string?, string?, string?) GetReverseFromPlant(string plantName)
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

        
        
        // Cascade selection endpoints (We select something and we have to resolve the filters for the following elements)
        static List<string> FilterMarkets(string selectedZone)
            => geographyElementsList.Where(x => x.Zone == selectedZone).Select(x => x.Market).ToList();

        static List<string> FilterCountries(string selectedMarket)
           => geographyElementsList.Where(x => x.Market == selectedMarket).Select(x => x.Country).ToList();

        static List<string> FilterCompanies(string selectedCountry)
           => geographyElementsList.Where(x => x.Country == selectedCountry).Select(x => x.DisplayCompany).ToList();

        static List<string> FilterPlants(string selectedCompany)
           => geographyElementsList.Where(x => x.DisplayCompany == selectedCompany).Select(x => x.DisplayPlant).ToList();
    }
}