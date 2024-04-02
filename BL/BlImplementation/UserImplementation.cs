namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

internal class UserImplementation : BlApi.IUser
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private readonly BlApi.IBl s_bl;
    internal UserImplementation(IBl bl) => s_bl = bl;

    /// <summary>
    /// Private helper function for converting Dal user to Bl user
    /// </summary>
    /// <param name="user"></param>
    /// <returns>BO.User type converted from the given BO.User type</returns>
    private BO.User _makeBlUser(DO.User user)
    {       
        BO.Engineer? eng = null;
        IEnumerable<BO.TaskInList>? completedTasks = null;
        if ((BO.UserType)user.UserType == BO.UserType.Engineer)
        {
            eng = s_bl.Engineer.Read(user.Id)!;
            completedTasks = (from task in s_bl.Task.ReadAll(t => t.Status == Status.Done && t.Engineer != null && t.Engineer.Id == user.Id)
                              select new BO.TaskInList() { Id = task.Id, Alias = task.Alias, Description = task.Description, Status = task.Status });
        }
        return new BO.User()
        {
            Id = user.Id,
            UserName = user.UserName,
            Password = user.Password,
            CreationDate = user.CreationDate,
            LastLoginDate = user.LastLoginDate,
            UserType = (BO.UserType)(user.UserType),
            PastTasks = completedTasks,
            CompletedTasks = completedTasks != null ? completedTasks.Count() : 0,
            Level = eng != null ? eng.Level : BO.EngineerExperience.None,
            CurrentTask = (eng != null && eng.Task != null) ? s_bl.Task.Read(eng.Task.Id) : null,
            Engineer = eng
        };
    }
    /// <summary>
    /// Adds the given user to the data base
    /// </summary>
    /// <param name="user"></param>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Add(BO.User user)
    {
        if (user.Engineer is not null)
        {
            if (_dal.User.Read(user.Engineer!.Id) is not null)
                throw new BO.BlAlreadyExistsException($"User with Id {user.Id} already exists");
            if (user.UserType == BO.UserType.Engineer && _dal.Engineer.Read(user.Engineer.Id) is null)
                throw new BO.BlDoesNotExistException($"Engineer with Id {user.Engineer.Id} does not exist");
            if (_dal.User.Read(u => u.Password == user.Password) is not null)
                throw new BO.BlAlreadyExistsException($"Password {user.Password} already in use");
            user.Id = user.Engineer.Id;
        }
        else
        {
            if (user.UserType == BO.UserType.Engineer)
                throw new BO.BlAlreadyExistsException($"No Engineer is assigned to this User");
            var rand = new Random();
            user.Id = rand.Next(200000000, 400000000);
        }
        

        _dal.User.Create(new DO.User()
        {
            Id = user.Id,
            UserName = user.UserName,
            Password = user.Password,
            CreationDate = s_bl.Clock,
            LastLoginDate = s_bl.Clock,
            UserType = (DO.UserType)(user.UserType),            
        });
    }
    /// <summary>
    /// Deletes the given user from the data base
    /// </summary>
    /// <param name="Id"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Delete(int Id)
    {
        if (Read(Id) is null)
            throw new BO.BlDoesNotExistException($"User with Id {Id} does not exist");
        _dal.User.Delete(Id);
    }
    /// <summary>
    /// Reads a user from the data base by id
    /// </summary>
    /// <param name="Id"></param>
    /// <returns>The User that match the given Id, null if none matches</returns>
    public BO.User? Read(int Id)
    {
        var user = _dal.User.Read(Id);
        if (user == null) return null;        
        return _makeBlUser(user);
    }
    /// <summary>
    /// Reads the user that match the given filter condition
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>The User that match the given Conditon, null if none matches</returns>
    public BO.User? Read(Func<BO.User, bool> filter)
    {
        var users = (from u in _dal.User.ReadAll()  select _makeBlUser(u));
        return (from u in users
                where filter(u)
                select u).FirstOrDefault();                     
    }
    /// <summary>
    /// Reads a collection of users from the data base that match the given filter condition. 
    /// if no condition is given, will return all users
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>A collection of users that match the given filter, all users if no filter is given</returns>
    public IEnumerable<BO.User>? ReadAll(Func<BO.User, bool>? filter = null)
    {
        var users = (from u in _dal.User.ReadAll() select _makeBlUser(u));
        if (filter is null) return users;
        return (from u in users
                where filter(u)
                select u).ToList();
    }
    /// <summary>
    /// Deletes all users from the data base
    /// (currently, is set to not Delete Admins)
    /// </summary>
    public void Reset()
    {
        var users = ReadAll(u => u.UserType == BO.UserType.Engineer);
        if (users is null) 
            return;
        foreach(var user in users)
        {
           Delete(user.Id);
        }
    }
    /// <summary>
    /// Updates a user in the data base. will apply the changes of the given user to the existing one.
    /// </summary>
    /// <param name="user"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Update(BO.User user)
    {
        if(Read(user.Id) is null)
            throw new BO.BlDoesNotExistException($"User with Id {user.Id} does not exist");       
        _dal.User.Update(new DO.User()
        {
            Id = user.Id,
            UserName = user.UserName,
            Password = user.Password,
            CreationDate = user.CreationDate,
            LastLoginDate = user.LastLoginDate,
            UserType = (DO.UserType)user.UserType
        });
    }
    /// <summary>
    /// Checks if the password can be assigned to a new user.
    /// </summary>
    /// <param name="password"></param>
    /// <returns>True if the password is free to use, false if the password already in use by a different user</returns>
    public bool ValidatePassword(string password)
    {
        if (_dal.User.Read(u => u.Password == password) is not null)
            return false;
        return true;
    }
    /// <summary>
    /// Perfoms login action by password and user-name. will update the lastLogInDate field of the given user.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Instance of the user given by the password and user-name</returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public BO.User LogIn(string password, string userName)
    {
        var user = Read(u => u.Password == password && u.UserName == userName);
        if (user is null)
            throw new BO.BlDoesNotExistException($"User {userName} not found");        
        user.LastLoginDate = s_bl.Clock;
        Update(user);
        return user;
    }
}
