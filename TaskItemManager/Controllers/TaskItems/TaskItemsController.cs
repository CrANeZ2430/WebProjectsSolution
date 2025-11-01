using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskItemManager.Controllers.Dtos;
using TaskItemManager.Controllers.TaskItems.Dtos;
using TaskItemManager.Requests.TaskItems;
using TaskItemManager.Models.TaskItems.Models;
using TaskItemManager.Repositories.TaskItems;
using TaskItemManager.Repositories.UnitOfWork;
using TaskItemManager.Repositories.Users;

namespace TaskItemManager.Controllers.TaskItems
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController(
        ITaskItemsRepository taskItemsRepository,
        IUsersRepository usersRepository,
        IUnitOfWorkRepository unitOfWorkRepository)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetTaskItems(
            CancellationToken cancellationToken = default)
        {
            var taskItems = await taskItemsRepository.GetTaskItems(cancellationToken);
            var taskItemDtos = taskItems.Select(t => new TaskItemDto(
                t.Id,
                t.Title,
                t.Description,
                t.IsCompleted,
                new UserDto(
                    t.User.Id,
                    t.User.UserName,
                    t.User.Email,
                    t.User.PasswordHash,
                    t.User.CreatedAt) ?? null));

            return Ok(new PageResponse<IEnumerable<TaskItemDto>>(
                taskItemDtos.Count(), 
                taskItemDtos));
        }

        [HttpGet("{taskItemId}")]
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
                new UserDto(
                    taskItem.User.Id,
                    taskItem.User.UserName,
                    taskItem.User.Email,
                    taskItem.User.PasswordHash,
                    taskItem.User.CreatedAt));

            return Ok(taskItemDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskItem(
            [FromBody] CreateTaskItemRequest query,
            CancellationToken cancellationToken = default)
        {
            var taskItem = await TaskItem.Create(query, usersRepository, cancellationToken);
            await taskItemsRepository.AddTaskItem(taskItem, cancellationToken);
            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [HttpPut("{taskItemId}")]
        public async Task<IActionResult> UpdateTaskItem(
            [FromRoute] Guid taskItemId,
            [FromBody] UpdateTaskItemRequest query,
            CancellationToken cancellationToken = default)
        {
            var taskItem = await taskItemsRepository.GetTaskItemById(taskItemId, cancellationToken);
            await taskItem.Update(query, cancellationToken);
            taskItemsRepository.UpdateTaskItem(taskItem);
            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [HttpDelete("{taskItemId}")]
        public async Task<IActionResult> DeleteTaskItem(
            [FromRoute] Guid taskItemId,
            CancellationToken cancellationToken = default)
        {
            var taskItem = await taskItemsRepository.GetTaskItemById(taskItemId, cancellationToken);
            taskItemsRepository.DeleteTaskItem(taskItem);
            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [HttpPatch("{taskItemId}")]
        public async Task<IActionResult> CompleteTaskItem(
            [FromRoute] Guid taskItemId,
            CancellationToken cancellationToken = default)
        {
            var taskItem = await taskItemsRepository.GetTaskItemById(taskItemId);
            await taskItem.Update(new UpdateTaskItemRequest(
                taskItem.Title,
                taskItem.Description,
                true));
            taskItemsRepository.UpdateTaskItem(taskItem);
            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);

            return Ok();
        }
    }
}
