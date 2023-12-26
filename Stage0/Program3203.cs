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
        Console.WriteLine("Hello, World!");
        Console.WriteLine("Enter your name:");
    }

    static partial void Welcome4818();
}