using TaskItemManager.Dtos.TaskItems;
using TaskItemManager.Models.Users;

namespace TaskItemManager.Models.TaskItems
{
    public class TaskItem
    {
        private TaskItem() { }

        private TaskItem(
            Guid id, 
            string title, 
            string description, 
            bool isCompleted,
            Guid userId)
        {
            Id = id;
            Title = title;
            Description = description;
            IsCompleted = isCompleted;
            UserId = userId;
        }

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool IsCompleted { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public static TaskItem Create(CreateTaskItemDto dto)
        {
            return new TaskItem(
                Guid.NewGuid(),
                dto.Title,
                dto.Description,
                dto.IsCompleted,
                dto.UserId);
        }

        public void Update(UpdateTaskItemDto dto)
        {
            Title = dto.Title;
            Description = dto.Description;
            IsCompleted = dto.IsCompleted;
        }
    }
}
