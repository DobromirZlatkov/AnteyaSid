namespace AnteyaSidOnContainers.WebApps.WebMVC.Infrastructure
{
    public static class API
    {
        public static class Catalog
        {
            public static string GetAllCatalogItems(string baseUri, string queryStringParams)
            {
                var filterQs = queryStringParams;
                return $"{baseUri}items{filterQs}";
            }

            public static string CreateCatalogItem(string baseUri)
            {
                return $"{baseUri}CreateItem/";
            }

            public static string UpdateCatalogItem(string baseUri)
            {
                return $"{baseUri}UpdateItem/";
            }
        }
    }
}
