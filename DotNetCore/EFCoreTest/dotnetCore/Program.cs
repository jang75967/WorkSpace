using EFTest.Models;

namespace dotnetCore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");

            DataContext dataContext = new DataContext();

            User user = new User
            {
                ID = 1,
                Name = "Test",
                DateTime = DateTime.Now,
                Salary = 50000
            };

            dataContext.Users.Add(user);
            dataContext.SaveChanges();

            var v = dataContext.Users.FirstOrDefault();
        }
    }
}