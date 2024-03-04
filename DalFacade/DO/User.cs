namespace DO;
 
/// <summary>
/// User entity for reprenting a user profile(engineer/admin) in the program
/// </summary>
/// <param name="Id">Id number of the user</param>
/// <param name="Password">User password</param>
/// <param name="UserName">User name. not neccesarily uniqe</param>
/// <param name="LastLoginDate">Last date when the user logged in</param>
/// <param name="CreationDate">Date when the User Profile was created</param>
/// <param name="UserType">Type of user. relevant for user permissions</param>
public record User
(
    int Id,
    int Password,
    string UserName,    
    DateTime LastLoginDate,
    DateTime CreationDate,
    DO.UserType UserType
)
{
    public User() : this(0, 0, "", DateTime.Now, DateTime.Now, DO.UserType.Engineer) { }
}