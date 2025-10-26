using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskItemManager.Dtos.Users;
using AppUser = TaskItemManager.Models.Users.User;
using TaskItemManager.Repositories.UnitOfWork;
using TaskItemManager.Repositories.Users;
using TaskItemManager.Controllers.Users.Dtos;

namespace TaskItemManager.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(
        IUsersRepository usersRepository,
        IUnitOfWorkRepository unitOfWorkRepository) 
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetUsers(
            CancellationToken cancellationToken = default)
        {
            var users = await usersRepository.GetUsers(cancellationToken);
            var userDtos = users.Select(x => new UserDto(
                x.Id,
                x.UserName,
                x.Email,
                x.PasswordHash,
                x.CreatedAt,
                x.TaskItems.Select(x => new TaskItemDto(
                    x.Id,
                    x.Title,
                    x.Description,
                    x.IsCompleted))));

            return Ok(userDtos);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(
            [FromRoute] Guid userId,
            CancellationToken cancellationToken = default)
        {
            var user = await usersRepository.GetUserById(userId, cancellationToken);
            var userDto = new UserDto(
                user.Id,
                user.UserName,
                user.Email,
                user.PasswordHash,
                user.CreatedAt,
                user.TaskItems.Select(x => new TaskItemDto(
                    x.Id,
                    x.Title,
                    x.Description,
                    x.IsCompleted)));

            return Ok(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(
            [FromBody] CreateUserDto query,
            CancellationToken cancellationToken = default)
        {
            var user = AppUser.Create(query);
            await usersRepository.AddUser(user, cancellationToken);
            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(
            [FromRoute] Guid userId,
            [FromBody] UpdateUserDto query,
            CancellationToken cancellationToken = default)
        {
            var user = await usersRepository.GetUserById(userId, cancellationToken);
            user.Update(query);
            usersRepository.UpdateUser(user);
            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(
            [FromRoute] Guid userId,
            CancellationToken cancellationToken = default)
        {
            var user = await usersRepository.GetUserById(userId);
            usersRepository.DeleteUser(user);
            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);

            return Ok();
        }
    }
}
