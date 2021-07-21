using EfCoreSimpleConsoleApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EfCoreSimpleConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start EF Core App");

            var students = ParseTxtToStudents("c:\\files\\students.txt").ToList();
            
            using var context = new StudentsContext();            
            context.Students.AddRange(students);
            context.SaveChanges();
        }

        static IEnumerable<Student> ParseTxtToStudents(string path)
        {
            var lines = File.ReadAllLines(path);            

            foreach (var line in lines)
            {
                var lineFragments = line.Split('\t').Select(s => s.Trim()).ToArray();

                var nameFragments = lineFragments[1].Split(' ');

                var student = new Student
                {
                    LastName = nameFragments[0],
                    FirstName = nameFragments[1],
                    MiddleName = nameFragments.Length > 2 ? nameFragments[2] : string.Empty,
                    GitAccount = new GitAccount
                    {
                        Url = $"https://github.com/{lineFragments[2]}/",
                        UserName = lineFragments[2]
                    }
                };

                yield return student;
            }
        }
    }
}
