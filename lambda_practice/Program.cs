using lambda_practice.data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace lambda_practice
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                 .UseInMemoryDatabase(databaseName: "Sample_Data")
                 .Options;

            // Insert test data in the DB
            using (var context = new DatabaseContext(options))
            {
                var intiliazer = new DatabaseInitializer();
                intiliazer.Initialize(context);
            }

            using (var context = new DatabaseContext(options))
            {
                //You can erase the following 3 lines if needed
                //context.Cities.ForEachAsync(c => Console.WriteLine(c.Name));
                //context.Departments.ForEachAsync(d => Console.WriteLine(d.Name));
                //context.Employees.ForEachAsync(e => Console.WriteLine($"{e.FirstName} {e.LastName}"));

                //1. List all employees whose departament has an office in Chihuahua
                var query = context.Employees.Where(e => e.Department.Cities.Where(c => c.Name == "Chihuahua").Count() > 0);

                Console.WriteLine("List all employees whose departament has an office in Chihuahua\n\n");

                query.ForEachAsync(e => Console.WriteLine($"{e.FirstName} {e.LastName}"));


                //2. List all departaments and the number of employees that belong to each department.
                var secondQuery = context.Departments.Select(d => new {
                    Name = d.Name,
                    Count = context.Employees.Where(e => e.Department == d).Count()
                });

                Console.WriteLine("\n\n2. List all departaments and the number of employees that belong to each department.\n\n");

                secondQuery.ForEachAsync(d => Console.WriteLine($"{d.Name} {d.Count}"));


                //3. List all remote employees. That is all employees whose living city is not the same one as their department's.
                var thirdQuery = context.Employees.Where(e => e.Department.Cities.Where(c => c.Name == e.City.Name).Count() == 0);
                
                Console.WriteLine("\n\n3. List all remote employees. That is all employees whose living city is not the same one as their department's.\n\n");

                thirdQuery.ForEachAsync(e => Console.WriteLine($"{e.FirstName} {e.LastName}"));


                //4. List all employees whose hire aniversary is next month.
                var fourthQuery = context.Employees.Where(e => e.HireDate.Month == DateTime.Now.AddMonths(1).Month);
                Console.WriteLine("\n\n4. List all employees whose hire aniversary is next month.\n\n");

                fourthQuery.ForEachAsync(e => Console.WriteLine($"{e.FirstName} {e.LastName}"));


                //5. List all 12 months of the year and the number of employees hired on each month.
                var fifthQuery = context.Employees.GroupBy(e => e.HireDate.Month);

                Console.WriteLine("\n\n5. List all 12 months of the year and the number of employees hired on each month.\n\n");
                
                fifthQuery.ForEachAsync(m => Console.WriteLine($"{System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m.Key)}: {m.Count()}"));
            }

            Console.WriteLine("\nDone.");
            Console.ReadLine();
        }
    }
}
