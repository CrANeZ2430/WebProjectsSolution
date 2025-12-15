using Moq;
using TaskItemManager.Models.Users.Checkers;
using TaskItemManager.Models.Users.Validation;
using TaskItemManager.Requests.Users;

namespace TaskItemManager.Validators.Tests;

public class CreateUserRequestValidator_UnitTest : IClassFixture<TaskItemManagerFixture>
{
    private IEmailUniqueChecker _checker;

    public CreateUserRequestValidator_UnitTest(TaskItemManagerFixture fixture)
    {
        _checker = fixture.EmailUniqueChecker;
    }

    [Fact]
    public async Task Validate_InputLongName_Fails()
    {
        //Arrange
        var request = new CreateUserRequest(
            "Namedddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd",
            "str@dddd");
        var emailUniqueCheckerMock = new Mock<IEmailUniqueChecker>();
        emailUniqueCheckerMock.Setup(x => x.IsUnique(request.Email, default)).ReturnsAsync(true);
        var validator = new CreateUserRequestValidator(emailUniqueCheckerMock.Object);

        //Act
        var result = await validator.ValidateAsync(request);
        var errors = result.Errors;

        //Assert
        Assert.True(result.Errors.Count() == 1);
    }

    [Fact]
    public async Task Validate_InputProperEmail_True()
    {
        //Arrange
        var request = new CreateUserRequest(
            "Name",
            "string@gmail.com");
        var validator = new CreateUserRequestValidator(_checker);

        //Act
        var result = await validator.ValidateAsync(request);
        var errors = result.Errors;

        //Assert
        Assert.True(result.Errors.Count() == 1);
    }

    //[Theory]
    //[InlineData("namerree")]
    //[InlineData("dhdhdhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh")]
    //public async Task Validate_InputProperLengthName(string name)
    //{
    //    //Arrange
    //    var request = new CreateUserRequest(
    //        name,
    //        "str@dddd");
    //    var emailUniqueCheckerMock = new Mock<IEmailUniqueChecker>();
    //    emailUniqueCheckerMock.Setup(x => x.IsUnique(request.Email, default)).ReturnsAsync(true);
    //    var validator = new CreateUserRequestValidator(emailUniqueCheckerMock.Object);

    //    //Act
    //    var result = await validator.ValidateAsync(request);
    //    var errors = result.Errors;

    //    //Assert
    //    Assert.True(result.Errors.Count() == 1);
    //}
}