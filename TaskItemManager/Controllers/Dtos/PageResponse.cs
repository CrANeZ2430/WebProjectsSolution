namespace TaskItemManager.Controllers.Dtos;

public record PageResponse<T>(
    int Count,
    T Data);
