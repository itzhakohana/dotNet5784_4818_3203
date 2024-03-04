namespace PL;
using System.Collections;

internal class EngExperienceCollection : IEnumerable
{
    static readonly IEnumerable<BO.EngineerExperience> s_enums =
        (Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}



internal class UserTypeCollection : IEnumerable
{
    static readonly IEnumerable<BO.UserType> s_enums =
        (Enum.GetValues(typeof(BO.UserType)) as IEnumerable<BO.UserType>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
