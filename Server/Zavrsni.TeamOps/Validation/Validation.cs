namespace Zavrsni.TeamOps.Validation
{
    public class ValidationHandler
    {
        public ValidationResult Result;
        public ValidationHandler()
        {
            Result = new ValidationResult();
        }
        public ValidationHandler Validate(bool condition, string errorMessage)
        {
            if (!condition)
            {
                Result.IsValid = false;
                Result.Messages.Add(errorMessage);
            }

            return this;
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Messages { get; set; }
        public ValidationResult()
        {
            IsValid = true;
            Messages = new List<string>();
        }
    }
}
