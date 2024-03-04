namespace PL;

internal class User
{
    public int Id { get; set; }
    public int Password { get; set; }
    public  string UserName { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastLoginDate { get; set; }    
    public BO.UserType UserType { get; set; }

}
