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
    }
}
