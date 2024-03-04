using DalApi;
using DO;

namespace Dal;

internal class UserImplementation : IUser
{
    public int Create(User item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public User? Read(int id)
    {
        throw new NotImplementedException();
    }

    public User? Read(Func<User, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<User?> ReadAll(Func<User, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void Update(User item)
    {
        throw new NotImplementedException();
    }
}
