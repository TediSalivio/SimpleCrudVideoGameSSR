namespace VideoGameSSR.Utility
{
    public class AppSettingsHelper
    {

        static IConfiguration _config = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, false).Build();

        public static string VideoGamesDbConnection
        {
            get
            {
                return _config["ConnectionString:DefaultConnection"];
            }
        }
    }
}
