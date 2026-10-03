using Moq;
using NooshApp.Api.Models;
using NooshApp.Api.Repositories.Interfaces;
using NooshApp.Api.Services;
using Xunit;

namespace NooshApp.Api.Tests
{
    public class FavouriteServiceTests
    {
        private readonly Mock<IFavouriteRepository> _favouriteRepo = new();
        private readonly Mock<ICustomerRepository> _customerRepo = new();
        private readonly Mock<IMenuItemRepository> _menuItemRepo = new();

        private FavouriteService BuildService() =>
            new FavouriteService(_favouriteRepo.Object, _customerRepo.Object, _menuItemRepo.Object);

        [Fact]
        public async Task ToggleFavouriteAsync_AddsFavourite_WhenNotAlreadyFavourited()
        {
            // Explanation: confirms a brand-new favourite is created when none exists yet,
            // and the method correctly reports "now favourited" (true) back to the caller.
            var customer = new Customer { Id = 1, Email = "test@example.com" };
            _customerRepo.Setup(r => r.GetByEmailAsync("test@example.com")).ReturnsAsync(customer);
            _favouriteRepo.Setup(r => r.FindAsync(1, 5)).ReturnsAsync((Favourite?)null);

            var service = BuildService();
            var result = await service.ToggleFavouriteAsync("test@example.com", 5);

            Assert.True(result);
            _favouriteRepo.Verify(r => r.AddAsync(It.IsAny<Favourite>()), Times.Once);
        }

        [Fact]
        public async Task ToggleFavouriteAsync_RemovesFavourite_WhenAlreadyFavourited()
        {
            // Explanation: confirms clicking the heart a second time un-favourites the
            // item rather than creating a duplicate row (the real-world "toggle" behavior).
            var customer = new Customer { Id = 1, Email = "test@example.com" };
            var existing = new Favourite { Id = 10, CustomerId = 1, MenuItemId = 5 };
            _customerRepo.Setup(r => r.GetByEmailAsync("test@example.com")).ReturnsAsync(customer);
            _favouriteRepo.Setup(r => r.FindAsync(1, 5)).ReturnsAsync(existing);

            var service = BuildService();
            var result = await service.ToggleFavouriteAsync("test@example.com", 5);

            Assert.False(result);
            _favouriteRepo.Verify(r => r.RemoveAsync(existing), Times.Once);
        }

        [Fact]
        public async Task ToggleFavouriteAsync_CreatesCustomer_WhenEmailNotSeenBefore()
        {
            // Explanation: confirms a customer favouriting something for the first time
            // (no existing Customer row yet) gets one created automatically, matching
            // the same "get-or-create" pattern used throughout Rewards.
            _customerRepo.Setup(r => r.GetByEmailAsync("new@example.com")).ReturnsAsync((Customer?)null);
            _customerRepo.Setup(r => r.CreateAsync("new@example.com", null))
                .ReturnsAsync(new Customer { Id = 7, Email = "new@example.com" });
            _favouriteRepo.Setup(r => r.FindAsync(7, 3)).ReturnsAsync((Favourite?)null);

            var service = BuildService();
            await service.ToggleFavouriteAsync("new@example.com", 3);

            _customerRepo.Verify(r => r.CreateAsync("new@example.com", null), Times.Once);
        }

        [Fact]
        public async Task GetFavouritesAsync_ReturnsEmptyList_WhenCustomerDoesNotExist()
        {
            // Explanation: confirms a lookup for someone with no account at all
            // returns an empty list rather than throwing — avoids a null-reference
            // crash on the Favourites page for an edge case that's easy to miss.
            _customerRepo.Setup(r => r.GetByEmailAsync("ghost@example.com")).ReturnsAsync((Customer?)null);

            var service = BuildService();
            var result = await service.GetFavouritesAsync("ghost@example.com");

            Assert.Empty(result);
        }
    }
}