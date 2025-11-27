using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskItemManager.Requests.Users;
using AppUser = TaskItemManager.Models.Users.Models.User;
using TaskItemManager.Repositories.UnitOfWork;
using TaskItemManager.Repositories.Users;
using TaskItemManager.Controllers.Users.Dtos;
using TaskItemManager.Controllers.Dtos;
using Microsoft.AspNetCore.Authorization;
using TaskItemManager.Models.Users.Checkers;

namespace TaskItemManager.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(
        IEmailUniqueChecker emailUniqueChecker,
        IUsersRepository usersRepository,
        IUnitOfWorkRepository unitOfWorkRepository) 
        : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(PageResponse<IEnumerable<UserDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUsers(
            CancellationToken cancellationToken = default)
        {
            var users = await usersRepository.GetUsers(cancellationToken);
            var userDtos = users.Select(u => new UserDto(
                u.Id,
                u.UserName,
                u.Email,
                u.CreatedAt,
                u.TaskItems.Select(t => new TaskItemSummaryDto(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.IsCompleted,
                    t.StartedAt,
                    t.DoneAt)),
                u.TaskItems.Count()));

            return Ok(new PageResponse<IEnumerable<UserDto>>(
                userDtos.Count(),
                userDtos));
        }

        [Authorize]
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(
            [FromRoute] Guid userId,
            CancellationToken cancellationToken = default)
        {
            var user = await usersRepository.GetUserById(userId, cancellationToken);
            var userDto = new UserDto(
                user.Id,
                user.UserName,
                user.Email,
                user.CreatedAt,
                user.TaskItems.Select(t => new TaskItemSummaryDto(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.IsCompleted,
                    t.StartedAt,
                    t.DoneAt)),
                user.TaskItems.Count());

            return Ok(userDto);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(UserCreatedResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateUser(
            [FromBody] CreateUserRequest query,
            CancellationToken cancellationToken = default)
        {
            var user = await AppUser.Create(query, emailUniqueChecker, cancellationToken);
            await usersRepository.AddUser(user, cancellationToken);
            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(
                nameof(CreateUser), 
                new { id = user.Id }, 
                new UserCreatedResponse(
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.CreatedAt));
        }

        [Authorize]
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(
            [FromRoute] Guid userId,
            [FromBody] UpdateUserRequest query,
            CancellationToken cancellationToken = default)
        {
            var user = await usersRepository.GetUserById(userId, cancellationToken);
            await user.Update(query, emailUniqueChecker, cancellationToken);
            usersRepository.UpdateUser(user);
            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(
            [FromRoute] Guid userId,
            CancellationToken cancellationToken = default)
        {
            var user = await usersRepository.GetUserById(userId);
            usersRepository.DeleteUser(user);
            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
