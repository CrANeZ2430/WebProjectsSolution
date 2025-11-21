namespace TaskItemManager.ExceptionHandling.Exceptions;

public class UnauthorizedException : DomainException
{
    public UnauthorizedException(string message) : base(message)
    {

    }
}
