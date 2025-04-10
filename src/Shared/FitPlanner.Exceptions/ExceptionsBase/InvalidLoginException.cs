namespace FitPlanner.Exceptions.ExceptionsBase;

public class InvalidLoginException : FitPlannerException
{
    public InvalidLoginException() : base(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID)
    { }
};