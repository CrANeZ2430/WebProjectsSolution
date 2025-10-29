using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskItemManager.Dtos.Users;
using AppUser = TaskItemManager.Models.Users.User;
using TaskItemManager.Repositories.UnitOfWork;
using TaskItemManager.Repositories.Users;
using TaskItemManager.Controllers.Users.Dtos;
using TaskItemManager.Controllers.Dtos;

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
            var userDtos = users.Select(u => new UserDto(
                u.Id,
                u.UserName,
                u.Email,
                u.PasswordHash,
                u.CreatedAt,
                u.TaskItems.Select(t => new TaskItemDto(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.IsCompleted)),
                u.TaskItems.Count()));

            return Ok(new PageResponse<IEnumerable<UserDto>>(
                userDtos.Count(),
                userDtos));
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
                user.TaskItems.Select(t => new TaskItemDto(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.IsCompleted)),
                user.TaskItems.Count());

            return Ok(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(
            [FromBody] CreateUserRequest query,
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
            [FromBody] UpdateUserRequest query,
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
