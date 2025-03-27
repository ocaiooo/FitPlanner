namespace FitPlanner.Communication.Responses;

public class ResponseErrorJson
{
    public IList<string> Errors { get; set; }

    public ResponseErrorJson(IList<string> errors) => Errors = new List<string>();
    
    public ResponseErrorJson(string error)
    {
        Errors = new List<string>
        {
            error
        };
    }
}