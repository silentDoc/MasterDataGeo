namespace MasterDataGeo
{
    public class GeographyElement
    {
        public string? Zone { get; set; }
        public string? Market { get; set; }
        public string? Country { get; set; }
        public string? CompanyCode { get; set; }
        public string? CompanyName { get; set; }
        public string? PlantCode { get; set; }
        public string? PlantName { get; set; }

        public string DisplayCompany
            => (CompanyCode ?? "") + " " + (CompanyName ?? "");

        public string DisplayPlant
            => (PlantCode ?? "") + " " + (PlantName ?? "");

        public GeographyElement(string? zone, string? market, string? country, string? companyCode, string? companyName, string? plantCode, string? plantName)
        {
            Zone = zone;
            Market = market;
            Country = country;
            CompanyCode = companyCode;
            CompanyName = companyName;
            PlantCode = plantCode;
            PlantName = plantName;
        }

        public GeographyElement(string csvLine)
        {
            var elements = csvLine.Split(";", StringSplitOptions.TrimEntries);
            Zone = elements[0];
            Market = elements[1];
            Country = elements[2];
            CompanyCode = elements[3];
            CompanyName = elements[4];
            PlantCode = elements[5];
            PlantName = elements[6];
        }
    }
}
