using NooshApp.Api.Models;

namespace NooshApp.Api.Data
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext context, string selfBaseUrl)
        {
            SeedMenuItems(context, selfBaseUrl);
            SeedRewardRules(context);
            SeedAppSettings(context);
        }
    }
}

