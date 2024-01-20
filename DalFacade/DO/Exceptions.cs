namespace DO;

//Custom Exception file for the Data-Layer

/// <summary>
/// Access-Atempt for entity that does not exist
/// </summary>
[Serializable]
public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException(string? message) : base(message) { }
}


/// <summary>
/// Add-Atempt of entity that already exist
/// </summary>
[Serializable]
public class DalAlreadyExistException : Exception
{
    public DalAlreadyExistException(string? message) : base(message) { }
}


/// <summary>
/// Invalid user input. Temporary for DalTest
/// </summary>
[Serializable]
public class DalInvalidInputException : Exception
{
    public DalInvalidInputException(string? message) : base(message) { }
}


/// <summary>
/// Xml file could not be opened/created
/// </summary>
[Serializable]
public class DalXMLFileLoadCreateException : Exception
{
    public DalXMLFileLoadCreateException(string? message) : base(message) { }
}

