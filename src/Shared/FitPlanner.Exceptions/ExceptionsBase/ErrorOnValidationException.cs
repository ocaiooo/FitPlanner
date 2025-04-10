namespace FitPlanner.Exceptions.ExceptionsBase;

public class ErrorOnValidationException : FitPlannerException
{
    public IList<string> ErrorMessages { get; set; }

    public ErrorOnValidationException(IList<string> errorMessages) : base(string.Empty)
    {
        ErrorMessages = errorMessages;
    }
}