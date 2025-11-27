using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskItemManager.Controllers.Dtos;
using TaskItemManager.Controllers.TaskItems.Dtos;
using TaskItemManager.Models.TaskItems.Checkers;
using TaskItemManager.Models.TaskItems.Models;
using TaskItemManager.Repositories.TaskItems;
using TaskItemManager.Repositories.UnitOfWork;
using TaskItemManager.Repositories.Users;
using TaskItemManager.Requests.TaskItems;

namespace TaskItemManager.Controllers.TaskItems
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController(
        IUserExistsChecker userExistsChecker,
        ITaskItemsRepository taskItemsRepository,
        IUnitOfWorkRepository unitOfWorkRepository)
        : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(PageResponse<IEnumerable<TaskItemDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetTaskItems(
            CancellationToken cancellationToken = default)
        {
            var taskItems = await taskItemsRepository.GetTaskItems(cancellationToken);
            var taskItemDtos = taskItems.Select(t => new TaskItemDto(
                t.Id,
                t.Title,
                t.Description,
                t.IsCompleted,
                t.StartedAt,
                t.DoneAt,
                new UserSummaryDto(
                    t.User.Id,
                    t.User.UserName,
                    t.User.Email,
                    t.User.CreatedAt) ?? null));

            return Ok(new PageResponse<IEnumerable<TaskItemDto>>(
                taskItemDtos.Count(), 
                taskItemDtos));
        }

        [Authorize]
        [HttpGet("{taskItemId}")]
        [ProducesResponseType(typeof(TaskItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTaskItem(
            [FromRoute] Guid taskItemId,
            CancellationToken cancellationToken = default)
        {
            var taskItem = await taskItemsRepository.GetTaskItemById(taskItemId, cancellationToken);
            var taskItemDto = new TaskItemDto(
                taskItem.Id,
                taskItem.Title,
                taskItem.Description,
                taskItem.IsCompleted,
                taskItem.StartedAt,
                taskItem.DoneAt,
                new UserSummaryDto(
                    taskItem.User.Id,
                    taskItem.User.UserName,
                    taskItem.User.Email,
                    taskItem.User.CreatedAt));

            return Ok(taskItemDto);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(TaskItemCreatedResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateTaskItem(
            [FromBody] CreateTaskItemRequest query,
            CancellationToken cancellationToken = default)
        {
            var taskItem = await TaskItem.Create(query, userExistsChecker, cancellationToken);
            await taskItemsRepository.AddTaskItem(taskItem, cancellationToken);
            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(
                nameof(CreateTaskItem),
                new { id = taskItem.Id },
                new TaskItemCreatedResponse(
                    taskItem.Id,
                    taskItem.Title,
                    taskItem.Description,
                    taskItem.IsCompleted,
                    taskItem.UserId));
        }

        [Authorize]
        [HttpPut("{taskItemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTaskItem(
            [FromRoute] Guid taskItemId,
            [FromBody] UpdateTaskItemRequest query,
            CancellationToken cancellationToken = default)
        {
            var taskItem = await taskItemsRepository.GetTaskItemById(taskItemId, cancellationToken);
            await taskItem.Update(query, cancellationToken);
            taskItemsRepository.UpdateTaskItem(taskItem);
            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        [Authorize]
        [HttpPatch("{taskItemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CompleteTaskItem(
            [FromRoute] Guid taskItemId,
            CancellationToken cancellationToken = default)
        {
            var taskItem = await taskItemsRepository.GetTaskItemById(taskItemId);
            await taskItem.Update(new UpdateTaskItemRequest(
                taskItem.Title,
                taskItem.Description,
                true,
                taskItem.StartedAt,
                taskItem.DoneAt));
            taskItemsRepository.UpdateTaskItem(taskItem);
            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{taskItemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTaskItem(
            [FromRoute] Guid taskItemId,
            CancellationToken cancellationToken = default)
        {
            var taskItem = await taskItemsRepository.GetTaskItemById(taskItemId, cancellationToken);
            taskItemsRepository.DeleteTaskItem(taskItem);
            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
