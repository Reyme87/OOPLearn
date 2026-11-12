namespace ApiGateway.Dtos
{
    public class SubmitTaskRequest
    {
        public Guid UserId { get; set; }
        public List<string> SelectedOptionIds { get; set; } = new();
        public string SubmittedCode { get; set; } = string.Empty;
    }
}
