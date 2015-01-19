using EmployeeDirectory.API.Models;
using EmployeeDirectory.API.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(EmployeeDirectory.API.Startup))]
namespace EmployeeDirectory.API
{
    /// <summary>
    /// Class that fires on start-up as stated by the assembly attribute, it wires ASP.NET Web API to Owin server pipeline.
    /// </summary>
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //CreateTestDatabase();
            //SeedDatabase();

            ConfigureOAuth(app);
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(15),
                Provider = new AuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            //Create Roles
            using (AuthRepository _repo = new AuthRepository())
            {
                _repo.InitializeUsersAndRoles();
            }
        }

        private void CreateTestDatabase()
        {
            SqlConnection connection = new SqlConnection(@"Server=(LocalDB)\v11.0; Integrated Security=true;AttachDbFileName=|DataDirectory|\EmployeeDirectory_data.mdf");
            using (connection)
            {
                connection.Open();

                //sql = "DROP DATABASE [EmployeeDirectory]";
                //command = new SqlCommand(sql, connection);
                //command.ExecuteNonQuery();

                string sql = string.Format(@"
                CREATE DATABASE
                    [EmployeeDirectory01]
                ON PRIMARY (
                   NAME=EmployeeDirectory_data,
                   FILENAME = '{0}\EmployeeDirectory_data.mdf'
                )
                LOG ON (
                    NAME=EmployeeDirectory_log,
                    FILENAME = '{0}\EmployeeDirectory_log.ldf'
                )",
                    @"C:\Users\Alberto\Devel\Visual Studio Projects\employee-directory\Data"
                );

                SqlCommand command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }

        private void SeedDatabase()
        {
            using (AuthContext _db = new AuthContext())
            {
                Random rand = new Random();

                List<string> boyNames = new List<string>() { "James", "John", "Robert", "Michael", "William", "David", "Richard", "Joseph", "Charles", "Thomas", "Christopher", "Daniel", "Matthew", "Donald", "Anthony", "Paul", "Mark", "George", "Steven", "Kenneth", "Andrew", "Edward", "Joshua", "Brian", "Kevin", "Ronald", "Timothy", "Jason", "Jeffrey", "Gary", "Ryan", "Nicholas", "Eric", "Jacob", "Stephen", "Jonathan", "Larry", "Frank", "Scott", "Justin", "Brandon", "Raymond", "Gregory", "Samuel", "Benjamin", "Patrick", "Jack", "Dennis", "Jerry", "Alexander", "Tyler", "Henry", "Douglas", "Peter", "Aaron", "Walter", "Jose", "Adam", "Zachary", "Harold", "Nathan", "Kyle", "Carl", "Arthur", "Gerald", "Roger", "Lawrence", "Keith", "Albert", "Jeremy", "Terry", "Joe", "Sean", "Willie", "Jesse", "Austin", "Christian", "Ralph", "Billy", "Bruce", "Bryan", "Roy", "Eugene", "Ethan", "Louis", "Wayne", "Jordan", "Harry", "Russell", "Alan", "Juan", "Philip", "Randy", "Dylan", "Howard", "Vincent", "Bobby", "Johnny", "Phillip", "Shawn" };
                List<string> girlNames = new List<string>() { "Mary", "Patricia", "Jennifer", "Elizabeth", "Linda", "Barbara", "Susan", "Margaret", "Jessica", "Dorothy", "Sarah", "Karen", "Nancy", "Betty", "Lisa", "Sandra", "Helen", "Ashley", "Donna", "Kimberly", "Carol", "Michelle", "Emily", "Amanda", "Melissa", "Deborah", "Laura", "Stephanie", "Rebecca", "Sharon", "Cynthia", "Kathleen", "Ruth", "Anna", "Shirley", "Amy", "Angela", "Virginia", "Brenda", "Pamela", "Catherine", "Katherine", "Nicole", "Christine", "Janet", "Debra", "Samantha", "Carolyn", "Rachel", "Heather", "Maria", "Diane", "Frances", "Joyce", "Julie", "Emma", "Evelyn", "Martha", "Joan", "Kelly", "Christina", "Lauren", "Judith", "Alice", "Victoria", "Doris", "Ann", "Jean", "Cheryl", "Marie", "Megan", "Kathryn", "Andrea", "Jacqueline", "Gloria", "Teresa", "Janice", "Sara", "Rose", "Hannah", "Julia", "Theresa", "Judy", "Grace", "Beverly", "Denise", "Marilyn", "Mildred", "Amber", "Danielle", "Brittany", "Olivia", "Diana", "Jane", "Lori", "Madison", "Tiffany", "Kathy", "Tammy", "Crystal" };

                List<string> surnames = new List<string>() { "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor", "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin", "Thompson", "Garcia", "Martinez", "Robinson", "Clark", "Rodriguez", "Lewis", "Lee", "Walker", "Hall", "Allen", "Young", "Hernandez", "King", "Wright", "Lopez", "Hill", "Scott", "Green", "Adams", "Baker", "Gonzalez", "Nelson", "Carter", "Mitchell", "Perez", "Roberts", "Turner", "Phillips", "Campbell", "Parker", "Evans", "Edwards", "Collins", "Stewart", "Sanchez", "Morris", "Rogers", "Reed", "Cook", "Morgan", "Bell", "Murphy", "Bailey", "Rivera", "Cooper", "Richardson", "Cox", "Howard", "Ward", "Torres", "Peterson", "Gray", "Ramirez", "James", "Watson", "Brooks", "Kelly", "Sanders", "Price", "Bennett", "Wood", "Barnes", "Ross", "Henderson", "Coleman", "Jenkins", "Perry", "Powell", "Long", "Patterson", "Hughes", "Flores", "Washington", "Butler", "Simmons", "Foster", "Gonzales", "Bryant", "Alexander", "Russell",
                        "Griffin", "Diaz", "Hayes", "Myers", "Ford", "Hamilton", "Graham", "Sullivan", "Wallace", "Woods", "Cole", "West", "Jordan", "Owens", "Reynolds", "Fisher", "Ellis", "Harrison", "Gibson", "Mcdonald", "Cruz", "Marshall", "Ortiz", "Gomez", "Murray", "Freeman", "Wells", "Webb", "Simpson", "Stevens", "Tucker", "Porter", "Hunter", "Hicks", "Crawford", "Henry", "Boyd", "Mason", "Morales", "Kennedy", "Warren", "Dixon", "Ramos", "Reyes", "Burns", "Gordon", "Shaw", "Holmes", "Rice", "Robertson", "Hunt", "Black", "Daniels", "Palmer", "Mills", "Nichols", "Grant", "Knight", "Ferguson", "Rose", "Stone", "Hawkins", "Dunn", "Perkins", "Hudson", "Spencer", "Gardner", "Stephens", "Payne", "Pierce", "Berry", "Matthews", "Arnold", "Wagner", "Willis", "Ray", "Watkins", "Olson", "Carroll", "Duncan", "Snyder", "Hart", "Cunningham", "Bradley", "Lane", "Andrews", "Ruiz", "Harper", "Fox", "Riley", "Armstrong", "Carpenter", "Weaver", "Greene", "Lawrence", "Elliott", "Chavez", "Sims", "Austin", "Peters", "Kelley", "Franklin", "Lawson", 
                        "Fields", "Gutierrez", "Ryan", "Schmidt", "Carr", "Vasquez", "Castillo", "Wheeler", "Chapman", "Oliver", "Montgomery", "Richards", "Williamson", "Johnston", "Banks", "Meyer", "Bishop", "Mccoy", "Howell", "Alvarez", "Morrison", "Hansen", "Fernandez", "Garza", "Harvey", "Little", "Burton", "Stanley", "Nguyen", "George", "Jacobs", "Reid", "Kim", "Fuller", "Lynch", "Dean", "Gilbert", "Garrett" };

                List<string> offices = new List<string>() { "Houston", "Austin" };

                for (int i = 0; i < 15000; i++)
                {
                    DirectoryEntryModel Entry = new DirectoryEntryModel()
                    {
                        UserName = string.Format("user{0}", i.ToString("D5")),
                        FullName = string.Format("{0} {1}", girlNames[rand.Next(0, 99)], surnames[rand.Next(0, 237)]),
                        OfficeLocation = offices[rand.Next(0, 1)],
                        OfficePhoneNumber = string.Format("713{0}", rand.Next(0000000, 9990000)),
                        PersonalPhoneNumber = "",
                        EmailAddress = string.Format("user{0}@headspring.com", i.ToString("D5")),
                        Gender = "female"
                    };

                    _db.DirectoryEntryModels.Add(Entry);
                }

                for (int i = 15000; i < 30000; i++)
                {
                    DirectoryEntryModel Entry = new DirectoryEntryModel()
                    {
                        UserName = string.Format("user{0}", i.ToString("D5")),
                        FullName = string.Format("{0} {1}", boyNames[rand.Next(0, 99)], surnames[rand.Next(0, 237)]),
                        OfficeLocation = offices[rand.Next(0,1)],
                        OfficePhoneNumber = string.Format("713{0}", rand.Next(0000000, 9990000)),
                        PersonalPhoneNumber = "",
                        EmailAddress = string.Format("user{0}@headspring.com", i.ToString("D5")),
                        Gender = "male"
                    };

                    _db.DirectoryEntryModels.Add(Entry);
                }

                _db.SaveChanges();
            }
        }

    }
}