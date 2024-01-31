namespace BlApi;

/// <summary>
/// Factory class for the logic-layer implementation
/// </summary>
public static class Factory
{
    public static IBl Get() => new BlImplementation.Bl();
}
