namespace BO;



[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}


[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException)
                : base(message, innerException) { }
}


[Serializable]
public class BlInvalidValuesException : Exception
{
    public BlInvalidValuesException(string? message) : base(message) { }
    public BlInvalidValuesException(string message, Exception innerException)
                : base(message, innerException) { }
}


[Serializable]
public class BlLogicViolationException : Exception
{
    public BlLogicViolationException(string? message) : base(message) { }
    public BlLogicViolationException(string message, Exception innerException)
                : base(message, innerException) { }
}


[Serializable]
public class BlInvalidUserInputException : Exception
{
    public BlInvalidUserInputException() { }
    public BlInvalidUserInputException(string? message) : base(message) { }
    
}