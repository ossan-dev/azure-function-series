namespace azure_function_entities.models
{
    public class Output
    {
        public string ApiKey { get; set; }
        public string Message { get; set; }
        public Output(string message, string apiKey)
        {
            ApiKey = apiKey;
            Message = message;
        }
    }
}
