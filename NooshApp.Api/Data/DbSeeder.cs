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

        private static void SeedMenuItems(ApplicationDbContext context, string selfBaseUrl)
        {
            if (context.MenuItems.Any())
            {
                foreach (var item in context.MenuItems)
                {
                    if (!string.IsNullOrWhiteSpace(item.ImageUrl))
                    {
                        var fileName = Path.GetFileName(
                            new Uri(item.ImageUrl).AbsolutePath
                        );

                        item.ImageUrl =
                            $"{selfBaseUrl.TrimEnd('/')}/images/menu/{fileName}";
                    }
                }

                context.SaveChanges();
                return;
            }

            var items = new List<MenuItem>
            {
                new MenuItem { Name = "Chicken Wrap", Description = "Grilled chicken, onions, tomatoes, pickles, fried brinjal, cabbage, lettuce, cheese and fries.", Price = 80.00m, Category = "Shawarma Wraps", ImageUrl = $"{selfBaseUrl}/images/menu/chicken-wrap.webp", IsPopular = true, SpiceLevel = SpiceLevel.Mild, ContainsWheat = true, ContainsDairy = true },
                new MenuItem { Name = "Beef Wrap", Description = "Slow-cooked beef, onions, tomatoes, pickles, fried brinjal, cabbage, lettuce, cheese and fries.", Price = 90.00m, Category = "Shawarma Wraps", ImageUrl = $"{selfBaseUrl}/images/menu/beef-wrap.webp", IsPopular = true, SpiceLevel = SpiceLevel.Mild, ContainsWheat = true, ContainsDairy = true },
                new MenuItem { Name = "Falafel Wrap", Description = "Crispy falafel, onions, tomatoes, pickles, fried brinjal, cabbage, lettuce, cheese and fries.", Price = 80.00m, Category = "Shawarma Wraps", ImageUrl = $"{selfBaseUrl}/images/menu/falafel-wrap.webp", IsPopular = true, IsVegetarian = true, SpiceLevel = SpiceLevel.Medium, ContainsWheat = true, ContainsDairy = true, ContainsSesame = true },
                new MenuItem { Name = "Chicken Bowl", Description = "Grilled chicken on your choice of rice, fries, or half & half, loaded with all the fixings.", Price = 110.00m, Category = "Shawarma Bowls", ImageUrl = $"{selfBaseUrl}/images/menu/chicken-bowl.webp", IsPopular = true, SpiceLevel = SpiceLevel.Mild, ContainsDairy = true },
                new MenuItem { Name = "Beef Bowl", Description = "Slow-cooked beef on your choice of rice, fries, or half & half, loaded with all the fixings.", Price = 120.00m, Category = "Shawarma Bowls", ImageUrl = $"{selfBaseUrl}/images/menu/beef-bowl.webp", SpiceLevel = SpiceLevel.Mild, ContainsDairy = true },
                new MenuItem { Name = "Falafel Bowl", Description = "Crispy falafel on your choice of rice, fries, or half & half, loaded with all the fixings.", Price = 110.00m, Category = "Shawarma Bowls", ImageUrl = $"{selfBaseUrl}/images/menu/falafel-bowl.webp", IsVegetarian = true, SpiceLevel = SpiceLevel.Medium, ContainsDairy = true, ContainsSesame = true },
                new MenuItem { Name = "Chilli Cheese Fries", Description = "Loaded fries topped with melted cheese, guacamole, sour cream and crushed nachos.", Price = 60.00m, Category = "Loaded Cheesy Fries", ImageUrl = $"{selfBaseUrl}/images/menu/chilli-cheese-fries.webp", IsPopular = true, IsVegetarian = true, SpiceLevel = SpiceLevel.Medium, ContainsDairy = true },
                new MenuItem { Name = "Chilli Cheese Fries — Chicken", Description = "Loaded fries topped with grilled chicken, melted cheese, guacamole, sour cream and crushed nachos.", Price = 80.00m, Category = "Loaded Cheesy Fries", ImageUrl = $"{selfBaseUrl}/images/menu/chilli-cheese-fries-chicken.webp", SpiceLevel = SpiceLevel.Medium, ContainsDairy = true },
                new MenuItem { Name = "Chilli Cheese Fries — Beef", Description = "Loaded fries topped with slow-cooked beef, melted cheese, guacamole, sour cream and crushed nachos.", Price = 100.00m, Category = "Loaded Cheesy Fries", ImageUrl = $"{selfBaseUrl}/images/menu/chilli-cheese-fries-beef.webp", SpiceLevel = SpiceLevel.Medium, ContainsDairy = true },
                new MenuItem { Name = "Medium Fries", Description = "Crispy golden fries.", Price = 25.00m, Category = "Fries", ImageUrl = $"{selfBaseUrl}/images/menu/fries-medium.webp", IsVegetarian = true },
                new MenuItem { Name = "Medium Saucy Fries", Description = "Crispy fries drizzled with garlic sauce and peri peri.", Price = 30.00m, Category = "Fries", ImageUrl = $"{selfBaseUrl}/images/menu/fries-medium-saucy.webp", IsVegetarian = true, SpiceLevel = SpiceLevel.Mild },
                new MenuItem { Name = "Large Fries", Description = "Crispy golden fries.", Price = 45.00m, Category = "Fries", ImageUrl = $"{selfBaseUrl}/images/menu/fries-large.webp", IsVegetarian = true },
                new MenuItem { Name = "Large Saucy Fries", Description = "Crispy fries drizzled with garlic sauce and peri peri.", Price = 55.00m, Category = "Fries", ImageUrl = $"{selfBaseUrl}/images/menu/fries-large-saucy.webp", IsVegetarian = true, SpiceLevel = SpiceLevel.Mild },
                new MenuItem { Name = "Noosh Chilli Paste", Description = "Our signature house-made chilli paste.", Price = 10.00m, Category = "Sauce Tubs", ImageUrl = $"{selfBaseUrl}/images/menu/sauce-chilli-paste.webp", IsVegetarian = true, SpiceLevel = SpiceLevel.ExtraHot, ContainsSesame = true },
                new MenuItem { Name = "Noosh Garlic Sauce", Description = "Creamy house garlic sauce.", Price = 10.00m, Category = "Sauce Tubs", ImageUrl = $"{selfBaseUrl}/images/menu/sauce-garlic.webp", IsVegetarian = true, ContainsDairy = true },
                new MenuItem { Name = "Noosh Peri Peri", Description = "Fiery house peri peri sauce.", Price = 10.00m, Category = "Sauce Tubs", ImageUrl = $"{selfBaseUrl}/images/menu/sauce-peri-peri.webp", IsVegetarian = true, SpiceLevel = SpiceLevel.Hot },
                new MenuItem { Name = "Noosh Kids Meal", Description = "2 x Chicken Nooshie Wraps, crispy fries, a sauce tub, juice and a fun activity.", Price = 75.00m, Category = "Kids Meal", ImageUrl = $"{selfBaseUrl}/images/menu/kids-meal.webp", IsPopular = true, ContainsWheat = true, ContainsDairy = true }
            };

            context.MenuItems.AddRange(items);
            context.SaveChanges();
        }

        private static void SeedRewardRules(ApplicationDbContext context)
        {
            if (context.RewardRules.Any()) return;

            context.RewardRules.AddRange(
                new RewardRule { Name = "Free Fries", PointsRequired = 150, RewardDescription = "1 Free Fries", DisplayOrder = 1 },
                new RewardRule { Name = "Free Shawarma", PointsRequired = 400, RewardDescription = "1 Free Shawarma", DisplayOrder = 2 }
            );
            context.SaveChanges();
        }

        private static void SeedAppSettings(ApplicationDbContext context)
        {
            if (context.AppSettings.Any()) return;

            context.AppSettings.Add(new AppSettings { Id = 1, PointsPerRand = 0.1m });
            context.SaveChanges();
        }
    }
}