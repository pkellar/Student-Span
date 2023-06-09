using StudentSpan.Data;
using StudentSpan.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;
using System.Linq;


namespace StudentSpan.Tests.Fixture
{
    public class DatabaseFixture : IDisposable
    {
        private static readonly object _lock = new object();
        private static bool _databaseInitialized;

        public DatabaseFixture()
        {
            Connection = new SqlConnection(@"Server=(localdb)\mssqllocaldb;Database=StudentSpan;Trusted_Connection=True;MultipleActiveResultSets=true");

            Seed();

            Connection.Open();
        }

        public void Dispose() => Connection.Dispose();

        public DbConnection Connection { get; }

        public StudentSpanContext CreateContext(DbTransaction transaction = null)
        {
            var context = new StudentSpanContext(new DbContextOptionsBuilder<StudentSpanContext>().UseSqlServer(Connection).Options);

            if(transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }

            return context;
        }

        private void Seed()
        {
            lock (_lock)
            {
                if(!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                        context.StudentModel.AddRange(
                            new Student
                            {
                                FirstName = "James",
                                LastName = "Hill",
                                GradMonth = "May",
                                GradYear = 2023,
                                Degree = "MET",
                                School = "MIT"

                            },
                            new Student
                            {
                                FirstName = "Paul",
                                LastName = "Timmy",
                                GradMonth = "June",
                                GradYear = 2023,
                                Degree = "CSC",
                                School = "Harbard"

                            },
                            new Student
                            {
                                FirstName = "Jane",
                                LastName = "Blaze",
                                GradMonth = "July",
                                GradYear = 2025,
                                Degree = "CSC",
                                School = "MIT"

                            },
                            new Student
                            {
                                FirstName = "Allen",
                                LastName = "Hill",
                                GradMonth = "July",
                                GradYear = 2024,
                                Degree = "CENG",
                                School = "USD"

                            }
                            );

                        foreach(var student in context.StudentModel.ToArray())
                        {
                            addComment(context, student);
                        }
                        context.SaveChanges();
                    }
                    _databaseInitialized = true;
                }
            }
        }

        private void addComment(StudentSpanContext context, Student student)
        {
            context.Comment
                .Add(
                     new Comment
                     {
                         EnteredOn = DateTime.Now,
                         Text = "Example Comment",
                         Student = student,
                         ApplicationUser = context.Users.FirstOrDefault()
                     });
        }

    }
}
