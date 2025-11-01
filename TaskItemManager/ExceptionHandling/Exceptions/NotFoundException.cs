namespace TaskItemManager.ExceptionHandling.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string message) : base(message)
    {

    }
}
