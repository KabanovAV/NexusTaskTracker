namespace NexusTaskTracker
{
    public record Error
    {
        public string Description { get; }
        public ErrorType Type { get; }
        public Dictionary<string, string[]>? ValidationErrors { get; }

        public Error(string description, ErrorType type, Dictionary<string, string[]>? validationErrors = null)
        {
            Description = description;
            Type = type;
            ValidationErrors = validationErrors;
        }

        public static readonly Error None = new(string.Empty, ErrorType.Failure);

        public static Error Failure(string description)
            => new(description, ErrorType.Failure);

        public static Error NotFound( string description)
            => new(description, ErrorType.NotFound);
    }
}
