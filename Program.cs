using System.Diagnostics;
using System.Linq;
using static Program;

//LINQ is a great framework working with C# to make queries from collection of data. These source data types could be Databases of SQL, Collection of
//Objects, Entities of Entity Framework DbSet, DataSet of ADO.Net and XML of XML files. The syntax is the same whatever the chosen source database.
//Also supported by IntelliSense.
//LINQ queries can be "connected" after each other if there is any logic like -> Where(...).OrderBy(...).ElementAt(.);

//THERE ARE MUCH MORE METHODS THAN IN THIS PROGRAM, BUT CONNECTED TO DATABASES SO I'LL CHECK THEM LATER
internal class Program
{
    private static void Main(string[] args)
    {
        #region Initializing collection
        List<Person> people = new List<Person>()
        {
            new Person() { Id = 1, Name = "George", Email = "george@gmail.com", City = "Debrecen", Job = JobTitle.Developer },
            new Person() { Id = 2, Name = "Emily", Email = "emily@gmail.com", City = "London", Job = JobTitle.Accountant }, 
            new Person() { Id = 3, Name = "Jack", Email = "jack@gmail.com", City = "London",Job = JobTitle.Analyst },
            new Person() { Id = 4, Name = "Dorothy", Email = "dorothy@gmail.com", City = "Madrid", Job = JobTitle.Accountant },
            new Person() { Id = 5, Name = "Julie", Email = "julie@gmail.com", City = "Debrecen" , Job = JobTitle.Developer }
        };

        Console.WriteLine("All person of people list: ");
        people.ForEach(person => Console.WriteLine(person.Id + ", " + person.Name + ", " + person.Email + ", " + person.City + ", " + person.Job));
        #endregion

        #region Where
        //Where -> method makes a IEnumerable of the selected type with the matches of the lambda expression condition.
        //If there's no match the referenceVariable gets a 'null' value. Anyway it stores the 'people' reference, so if i change
        //any value of any Prop of any Person it changes in 'matches' too. *1
        IEnumerable<Person> matches = people.Where(person => person.Job == JobTitle.Developer);
        //*1 people[0].Name = "Scott";
        Console.WriteLine("\nPersons who are developers:");

        foreach (Person person in matches)
        {
            Console.WriteLine(person.Name + "'s job is " + person.Job);
        }
        #endregion

        #region OrderBy
        //OrderBy -> orders the members by the selected property (as you can see if it's an enum it depends on the order of the enum's members)
        IOrderedEnumerable<Person> matchesOrderBy = people.OrderBy(person => person.Job);
        Console.WriteLine("\nOrderBy jobs: ");

        foreach (Person person in matchesOrderBy)
        {
            Console.WriteLine(person.Id + ", " + person.Name + ", " + person.Email + ", " + person.City + ", " + person.Job);
        }
        #endregion

        #region OrderByDescending
        //OrderByDescending
        IOrderedEnumerable<Person> matchesOrderByDescending = people.OrderByDescending(person => person.Name);
        Console.WriteLine("\nOrderByDescending names: ");

        foreach (Person person in matchesOrderByDescending)
        {
            Console.WriteLine(person.Id + ", " + person.Name + ", " + person.Email + ", " + person.City + ", " + person.Job);
        }
        #endregion

        #region ThenBy & ThenByDescending
        //ThenBy -> is an added suborder after the first order. Multiple ThenBy works in the coded order. ThenByDescending is the same.
        matchesOrderBy = people.OrderBy(person => person.Job).
                                ThenBy(person => person.Name);
        Console.WriteLine("\nOrderByDescending jobs and ThenBy name: ");

        foreach (Person person in matchesOrderBy)
        {
            Console.WriteLine(person.Id + ", " + person.Name + ", " + person.Email + ", " + person.City + ", " + person.Job);
        }
        #endregion

        #region First & FirstOrDefault
        //First -> Gves back the first matched element of the condition. This runs on error if there's no match.
        Person first = people.First(person => person.Id > 2);
        Console.WriteLine("\nFirst match of condition: Id > 2: ");

        Console.WriteLine(first.Id + ", " + first.Name + ", " + first.Email + ", " + first.City + ", " + first.Job);

        //FirstOrDefault -> Doest the same as First except that if condition does no matches it returns null. 
        //However still must be check the called refVariable if not null because if that gets null the properties can't be accessible.
        Person firstOrDefault = people.FirstOrDefault(person => person.Id > 10);
        Console.WriteLine("\nFirst match of condition: Id > 10: ");

        Console.WriteLine(firstOrDefault == null ? "There's no match" : $"\nMatch: {firstOrDefault.Id}, {firstOrDefault.Name}");
        #endregion

        #region Last & LastOrDefault
        //They work same as First/FirstOrDefault except the match will be the last possible.

        Person last = people.Last(person => person.Job == JobTitle.Developer);
        Console.WriteLine("\nLast match of condition: Job is Developer: ");

        Console.WriteLine(last.Id + ", " + last.Name + ", " + last.Email + ", " + last.City + ", " + last.Job);

        Person lastOrDefault = people.LastOrDefault(person => person.City == "Debrecen");
        Console.WriteLine("\nLast match of condition: City is Debrecen: ");

        Console.WriteLine(lastOrDefault == null ? "There's no match" : $"Match: {lastOrDefault.City}, {lastOrDefault.Name}");
        #endregion

        #region ElementAt & ElementAtOrDefault
        //Gives back the element of the indexed member.
        Person elementAt = people.Where(person => person.City == "Debrecen").ElementAt(1);
        Console.WriteLine("\nElement at 2nd position where ppl are from Debrecen: " +
                          $"\n{elementAt.Name}, {elementAt.City}");

        Person elementAtOrDefault = people.Where(person => person.City == "Debrecen").ElementAtOrDefault(5);
        Console.WriteLine("\nElement at 5th position where ppl are from Debrecen: (it gives null reference)");
        Console.WriteLine(elementAtOrDefault == null ? "There's no match!" : $"{elementAtOrDefault.Name}, {elementAtOrDefault.City}");
        #endregion

        #region Single & SingleOrDefault
        //They work almost the same as First and FirstOrDefault except it throws extra exception when there are 2 or more elements in the object
        //match the condition. Unfortunately if there are multiple elements matching with the condition SingleOrDefault still throw exception.
        //However it could still work with unique Properties such as Id.
        #endregion

        #region Select & ToList
        //Select -> it doesn't copy reference of the original objects but makes new objects.
        //ToList -> convert the result from IEnumerable to List, so it's possible to call List class methods and properties.
        int id = 11;
        List<AnotherPersonClass> similarButNotSamePeopleObjects = people.Select(person => 
                                                                         new AnotherPersonClass() { Name = person.Name, Id = id++ }).ToList();

        Console.WriteLine("\nObjects of made class from Person class -> AnotherPersonClass by Select LINQ method: ");
        foreach (AnotherPersonClass person in similarButNotSamePeopleObjects)
        {
            Console.WriteLine(person.Id + ", " + person.Name);
        }

        Console.WriteLine("\nHere's the evidence Person list stored in people hasn't changed: ");
        foreach (Person person in people)
        {
            Console.WriteLine(person.Id + ", " + person.Name);
        }

        ////Kinda weird stuff, but it works also
        //IEnumerable<int> nums = people.Select(person => 10);
        //foreach (Person person in people)
        //{
        //    Console.WriteLine(person.Name);
        //}

        //Also List methods now works on similarButNotSamePeopleObjects.
        //    similarButNotSamePeopleObjects.Add(new AnotherPersonClass() { Id = id++, Name = "John" });
        #endregion

        #region Min & Max & Sum & Avarage & Count
        //Pretty obvious: makes the actual math process on numeric data types.
        int max = people.Max(person => person.Id);
        Console.WriteLine("\nHighest Id of people \n" + max);
        int sum = people.Sum(person => person.Id);
        Console.WriteLine("\nSum the Id of people \n" + sum);
        #endregion
    }
    #region Predifined class + enum for practice
    public class AnotherPersonClass
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public JobTitle Job { get; set; }
    }

    //A little ply with the values of jobs in connection with OrderBy
    public enum JobTitle
    {
        Developer = 3,
        Accountant = 1,
        Analyst = 2
    }
    #endregion
}