namespace FitPlanner.Exceptions.ExceptionsBase;

public class ErrorOnValidationException(IList<string> errorMessages) : FitPlannerException(string.Empty)
{
    public IList<string> ErrorMessages { get; set; } = errorMessages;
}