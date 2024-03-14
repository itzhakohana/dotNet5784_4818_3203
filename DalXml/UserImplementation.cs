using DalApi;
using DO;
using System.Xml.Linq;

namespace Dal;

internal class UserImplementation : IUser
{
    readonly string s_users_xml = "users";
    private XElement? _user_root;

    /// <summary>
    /// Add a user to the data base
    /// </summary>
    /// <param name="item"></param>
    /// <returns>The Id of the newly created user</returns>
    /// <exception cref="DalAlreadyExistException"></exception>
    public int Create(User item)
    {
        if (Read(item.Id) is not null)
            throw new DalAlreadyExistException($"User with Id {item.Id} already exists");

        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);        
        users.Add(item);
        XMLTools.SaveListToXMLSerializer(users, s_users_xml);
        return item.Id;
    }
    /// <summary>
    /// Deletes a user by Id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalAlreadyExistException"></exception>
    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new DalAlreadyExistException($"User with Id {id} not found");

        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        users.Remove(users.FirstOrDefault(u => u.Id == id)!);
        XMLTools.SaveListToXMLSerializer(users, s_users_xml);        
    }

    public User? Read(int id)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        return users.FirstOrDefault(u => u.Id == id) ?? null;        
    }

    public User? Read(Func<User, bool> filter)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        return users.FirstOrDefault(u => filter(u)) ?? null;
    }

    public IEnumerable<User?> ReadAll(Func<User, bool>? filter = null)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        if (filter is null)
            return users;        
        return users.FindAll(u => filter(u));
    }

    public void Reset()
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        users.RemoveAll(u => u.UserType == DO.UserType.Engineer);
        XMLTools.SaveListToXMLSerializer(users, s_users_xml);
    }

    public void Update(User item)
    {
        User originalUser = Read(item.Id)
            ?? throw new DalDoesNotExistException($"User with Id {item.Id} does not exist");
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        users.Remove(originalUser);
        users.Add(item);
        XMLTools.SaveListToXMLSerializer(users, s_users_xml);
    }
}
