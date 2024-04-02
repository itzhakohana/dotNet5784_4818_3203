using DalApi;

namespace BlApi;

public interface IUser
{
    /// <summary>
    /// Adds the given user to the data base
    /// </summary>
    /// <param name="user"></param>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Add(BO.User user);
    /// <summary>
    /// Deletes the given user from the data base
    /// </summary>
    /// <param name="Id"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Delete(int Id);
    /// <summary>
    /// Reads a user from the data base by id
    /// </summary>
    /// <param name="Id"></param>
    /// <returns>The User that match the given Id, null if none matches</returns>
    public BO.User? Read(int Id);
    /// <summary>
    /// Reads the user that match the given filter condition
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>The User that match the given Conditon, null if none matches</returns>
    public BO.User? Read(Func<BO.User, bool> filter);
    /// <summary>
    /// Reads a collection of users from the data base that match the given filter condition. 
    /// if no condition is given, will return all users
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>A collection of users that match the given filter, all users if no filter is given</returns>
    public IEnumerable<BO.User>? ReadAll(Func<BO.User, bool>? filter = null);
    /// <summary>
    /// Deletes all users from the data base
    /// </summary>
    public void Reset();
    /// <summary>
    /// Updates a user in the data base. will apply the changes of the given user to the existing one.
    /// </summary>
    /// <param name="user"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Update(BO.User user);
    /// <summary>
    /// Checks if the password can be assigned to a new user.
    /// </summary>
    /// <param name="password"></param>
    /// <returns>True if the password is free to use, false if the password already in use by a different user</returns>
    public bool ValidatePassword(string password);
    /// <summary>
    /// Perfoms login action by password and user-name. will update the lastLogInDate field of the given user.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Instance of the user given by the password and user-name</returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public BO.User LogIn(string password, string userName);
}
