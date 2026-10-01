using System;
using System.Threading.Tasks;
using FitNance.Controllers.Profile;
using FitNance.Data;
using FitNance.Models.ProfileSetup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FitNance.Tests;

public class ProfileControllerTests
{
    [Fact]
    public async Task GetProfile_ReturnsProfile_WhenProfileExists()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new AppDbContext(options);

        context.UserProfiles.Add(new UserProfileModel
        {
            UserProfileId = "user-001",
            FirstName = "Juan",
            LastName = "Dela Cruz",
            Height = 170,
            Weight = 65
        });

        await context.SaveChangesAsync();

        var controller = new ProfileController(context);

        var result = await controller.GetProfile("user-001");

        var okResult = Assert.IsType<OkObjectResult>(result);
        var profile = Assert.IsType<UserProfileModel>(okResult.Value);

        Assert.Equal("user-001", profile.UserProfileId);
        Assert.Equal("Juan", profile.FirstName);
        Assert.Equal(170, profile.Height);
    }
}
