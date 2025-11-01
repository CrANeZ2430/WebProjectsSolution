namespace TaskItemManager.ExceptionHandling.Exceptions;

public class InternalServerErrorException : DomainException
{
    public InternalServerErrorException(string message) : base(message)
    {

    }
}
