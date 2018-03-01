using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace lambda_practice.data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        { }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Department> Departments { get; set; }
    }



    public class DatabaseInitializer
    {
        public void Initialize(DatabaseContext context)
        {
            var c = InitCities(context);
            var d = InitDepartments(context, c);
            InitEmployees(context, c, d);
            context.SaveChanges();
        }

        private static void InitEmployees(DatabaseContext context, IList<City> cities, IList<Department> dpts)
        {
            var names = new string[]{
                "Jose",
                "Juan",
                "Pedro",
                "Julio",
                "Ana",
                "Monica",
                "Enrique",
                "Jesus",
                "El Maestro",
                "Batman",
            };


            var lastnames = new string[]{
                "Fernandez",
                "Hernandez",
                "Gonzalez",
                "Moreno",
                "Garcia",
                "Gallardo",
                "Soto",
                "Miranda",
                "Jimenez",
                "Marquez",
            };



            IList<Employee> emps = new List<Employee>();

            for (var i = 0; i < 1000; i++)
            {
                emps.Add(new Employee()
                {
                    FirstName = names[random()],
                    LastName = lastnames[random()],
                    HireDate = RandomDay(),
                    City = cities[rnd.Next(cities.Count)],
                    Department = dpts[rnd.Next(dpts.Count)]
                });
            }

            foreach (Employee e in emps)
                context.Employees.Add(e);
        }

        private static IList<City> InitCities(DatabaseContext context)
        {
            IList<City> cities = new List<City>();

            cities.Add(new City() { Name = "Chihuahua" });
            cities.Add(new City() { Name = "Mexico City" });
            cities.Add(new City() { Name = "New York" });
            cities.Add(new City() { Name = "Tokio" });
            cities.Add(new City() { Name = "London" });
            cities.Add(new City() { Name = "Parral" });

            foreach (City c in cities)
                context.Cities.Add(c);

            return cities;
        }

        private static IList<Department> InitDepartments(DatabaseContext context, IList<City> cities)
        {
            IList<Department> departments = new List<Department>();

            departments.Add(new Department() { Name = "Finances" });
            departments.Add(new Department() { Name = "HR" });
            departments.Add(new Department() { Name = "IT" });
            departments.Add(new Department() { Name = "Quality" });
            departments.Add(new Department() { Name = "Production" });
            departments.Add(new Department() { Name = "Support Center" });

            foreach (Department dpt in departments)
            {
                dpt.Cities = new List<City>();
                dpt.Cities.Add(cities[rnd.Next(cities.Count)]);
                dpt.Cities.Add(cities[rnd.Next(cities.Count)]);
                context.Departments.Add(dpt);
            }

            return departments;
        }

        private static Random rnd = new Random();
        private static int random()
        {
            return rnd.Next(0, 10);
        }


        private static DateTime RandomDay()
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(rnd.Next(range));
        }
    
}

}
