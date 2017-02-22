using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;


namespace RevRegFinal.Models
{
    interface ICourse
    {
        bool AddStudent(StudentModel student);
        bool AddStudents(List<StudentModel> roster);
        bool RemoveStudent(StudentModel student);
       
        bool RemoveStudentById(int id);
       
        bool isFull { get; }
        int RosterCount { get; }
        string Title { get; }
        //List<Student> GetStudentRoster();
    }
}