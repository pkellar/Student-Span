using StudentSpan.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Identity;

namespace StudentSpan.Models
{
    public class SeedDatabase
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            // Method used to encapsulate the logic to read
            // from an embedded resource file.

            using (var context = new StudentSpanContext(serviceProvider.GetRequiredService<DbContextOptions<StudentSpanContext>>()))
            {
                // Look for any Students.
                if (context.StudentModel.Any())
                {
                    return;   // DB has been seeded
                }
                // Reference:
                // https://stackoverflow.com/questions/3314140/how-to-read-embedded-resource-text-file


                string recruiter1 = "recruiter1@example.com";

                var user = new ApplicationUser
                {
                    UserName = recruiter1,
                    NormalizedUserName = recruiter1.ToUpper(),
                    Email = recruiter1,
                    NormalizedEmail = recruiter1.ToUpper(),
                    EmailConfirmed = true,
                };

                PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
                user.PasswordHash = passwordHasher.HashPassword(user, "Test_1234");

                context.Users.Add(user);



                var assembly = Assembly.GetExecutingAssembly();

                // NOTE:
                // Use the following to get the exact resource name
                // to be assigned to the resourceName variable below.
                string[] resourceNames = assembly.GetManifestResourceNames();

                string resourceName = "StudentSpan.Models.Students.csv";
                string line;

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Writes to the Output Window.
                        Debug.WriteLine(line);

                        // Logic to parse the line, separate by comma(s), and assign fields
                        // to the Student model.
                        var values = line.Split(',');
                        context.StudentModel.AddRange(
                        new Student
                        {
                            FirstName = values[0],
                            LastName = values[1],
                            GradMonth = values[2],
                            GradYear = Convert.ToInt32(values[3]),
                            Degree = values[4],
                            School = values[5],
                        }
                        );

                        foreach (var student in context.StudentModel.ToArray())
                        {
                            addComment(context, student);
                        }
                        }
                    }
                    context.SaveChanges();
            }

        }
        private static void addComment(StudentSpanContext context, Student student)
        {
            context.Comment.Add(
                new Comment
                {
                    EnteredOn = DateTime.Now,
                    Text = "Example Comment",
                    Student = student,
                    ApplicationUser = context.Users.FirstOrDefault()
                });
        }

        private static void addWatch(StudentSpanContext context, Student student)
        {
            context.Watch.Add(
                new Watch
                {
                    WatchedOn = DateTime.Now,
                    Text = "Example Watch",
                    Student = student,
                    ApplicationUser = context.Users.FirstOrDefault()
                });
        }
    }
}

