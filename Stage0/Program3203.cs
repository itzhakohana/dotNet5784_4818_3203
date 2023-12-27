partial class Program
{
    private static void Main(string[] args)
    {
        Welcome3203();
        Welcome4818();
        Console.ReadKey();
    }

    private static void Welcome3203()
    {
        Console.WriteLine("Enter your name:");
        string userName = Console.ReadLine();
        Console.WriteLine("{0}, Welcome to my first console application!", userName);
    }


     static void Welcome4818()
    {
        Console.WriteLine("Enter your name UPDATED!!!:");
        string userName = Console.ReadLine();
        Console.WriteLine("{0}, Welcome to my first console application!", userName);
    }

}