using Microsoft.AspNetCore.Hosting;
using Moq;
using NooshApp.Api.Dtos;
using NooshApp.Api.Models;
using NooshApp.Api.Repositories.Interfaces;
using NooshApp.Api.Services;
using Xunit;

namespace NooshApp.Api.Tests
{
    public class AdminServiceMenuItemTests
    {
        private readonly Mock<IRewardRuleRepository> _rewardRuleRepo = new();
        private readonly Mock<IAppSettingsRepository> _settingsRepo = new();
        private readonly Mock<IMenuItemRepository> _menuItemRepo = new();
        private readonly Mock<IWebHostEnvironment> _env = new();

        private AdminService BuildService() =>
            new AdminService(_rewardRuleRepo.Object, _settingsRepo.Object, _menuItemRepo.Object, _env.Object);

        [Fact]
        public async Task DeleteMenuItemAsync_SoftDeletesViaRepository()
        {
            // Explanation: confirms deleting a menu item goes through the
            // repository's soft-delete path (IsDeleted flag), not a hard
            // database delete — protects historical references to the item.
            await BuildService().DeleteMenuItemAsync(42);
            _menuItemRepo.Verify(r => r.DeleteAsync(42), Times.Once);
        }

        [Fact]
        public async Task UpdateMenuItemAsync_ReturnsNull_WhenItemDoesNotExist()
        {
            // Explanation: confirms trying to update a non-existent (or already
            // soft-deleted) item fails gracefully with null, which the Controller
            // turns into a 404 — rather than throwing a NullReferenceException.
            _menuItemRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((MenuItem?)null);

            var result = await BuildService().UpdateMenuItemAsync(999, new UpdateMenuItemRequestDto
            {
                Name = "Ghost Item", Price = 10, Category = "Fries", IsAvailable = true
            });

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateMenuItemAsync_DefaultsToAvailable()
        {
            // Explanation: confirms every newly-created menu item is immediately
            // visible to customers by default (IsAvailable = true), matching
            // the expected Admin workflow of "add, then optionally hide."
            _menuItemRepo.Setup(r => r.GetAllForAdminAsync()).ReturnsAsync(new List<MenuItem>());
            _menuItemRepo.Setup(r => r.CreateAsync(It.IsAny<MenuItem>()))
                .ReturnsAsync((MenuItem item) => item);

            var result = await BuildService().CreateMenuItemAsync(new CreateMenuItemRequestDto
            {
                Name = "Test Wrap", Price = 80, Category = "Shawarma Wraps"
            });

            Assert.True(result.IsAvailable);
        }
    }
}