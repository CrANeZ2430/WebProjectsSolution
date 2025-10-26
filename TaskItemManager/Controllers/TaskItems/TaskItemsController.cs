using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskItemManager.Controllers.TaskItems.Dtos;
using TaskItemManager.Dtos.TaskItems;
using TaskItemManager.Models.TaskItems;
using TaskItemManager.Repositories.TaskItems;
using TaskItemManager.Repositories.UnitOfWork;

namespace TaskItemManager.Controllers.TaskItems
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController(
        ITaskItemsRepository taskItemsRepository,
        IUnitOfWorkRepository unitOfWorkRepository)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetTaskItems(
            CancellationToken cancellationToken = default)
        {
            var taskItems = await taskItemsRepository.GetTaskItems(cancellationToken);
            var taskItemDtos = taskItems.Select(x => new TaskItemDto(
                x.Id,
                x.Title,
                x.Description,
                x.IsCompleted,
                x.User == null ? null :
                new UserDto(
                    x.User.Id,
                    x.User.UserName,
                    x.User.Email,
                    x.User.PasswordHash,
                    x.User.CreatedAt)));

            return Ok(taskItemDtos);
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
            [FromBody] CreateTaskItemDto query,
            CancellationToken cancellationToken = default)
        {
            var taskItem = TaskItem.Create(query);
            await taskItemsRepository.AddTaskItem(taskItem, cancellationToken);
            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [HttpPut("{taskItemId}")]
        public async Task<IActionResult> UpdateTaskItem(
            [FromRoute] Guid taskItemId,
            [FromBody] UpdateTaskItemDto query,
            CancellationToken cancellationToken = default)
        {
            var taskItem = await taskItemsRepository.GetTaskItemById(taskItemId, cancellationToken);
            taskItem.Update(query);
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
            taskItem.Update(new UpdateTaskItemDto(
                taskItem.Title,
                taskItem.Description,
                true));
            taskItemsRepository.UpdateTaskItem(taskItem);
            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);

            return Ok();
        }
    }
}
