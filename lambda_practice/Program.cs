using lambda_practice.data;
using Microsoft.EntityFrameworkCore;
using System;

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
                context.Cities.ForEachAsync(c => Console.WriteLine(c.Name));
                context.Departments.ForEachAsync(d => Console.WriteLine(d.Name));
                context.Employees.ForEachAsync(e => Console.WriteLine($"{e.FirstName} {e.LastName}"));

                //1. List all employees whose departament has an office in Chihuahua


                //2. List all departaments and the number of employees that belong to each department.


                //3. List all remote employees. That is all employees whose living city is not the same one as their department's.


                //4. List all employees whose hire aniversary is next month.


                //5. List all 12 months of the year and the number of employees hired on each month.
            }

            Console.ReadLine();
        }
    }
}
