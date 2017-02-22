using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RevRegFinal.Models;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RevRegFinal.Models
{
    public class CourseModel : ICourse
    {


        public int isClosed { get; set; }
        public string CourseModelId { get; set; }
        public int numStudents { get; set; }
        public string timeOfDay { get; set; }
        public int creditHour { get; set; }
        public List<StudentModel> studentRoster { get; set; }
        public string Location { get; set; }

        public CourseModel(string title, string timeOfDay, string location, int creditHour = 1, int numStudents = 0, int isclosed = 0)
        {
            
            this.CourseModelId = title;
            this.timeOfDay = timeOfDay;
            this.creditHour = creditHour;
            this.numStudents = numStudents;
            this.Location = location;
            this.isClosed = isclosed;
        }

        public CourseModel()
        {

        }
        public delegate bool CloseRegistration(CourseModel thisCourseToClose);
        public CloseRegistration closeReg = null;


        public bool isFull
        {
            get
            {

                return studentRoster.Count == Global.maxStudents;
            }
            set { }
        }

        public int RosterCount
        {
            get
            {
                return studentRoster.Count;
            }
        }

        public string Title
        {
            get { return CourseModelId; }
        }

       
        public bool AddStudent(StudentModel student)
        {
            SpaceCheck(studentRoster.Count + 1);
            studentRoster.Add(student);
            if (closeReg != null && isFull)
            {
                closeReg(this);
            }
            return true;
        }

        public bool AddStudents(List<StudentModel> roster)
        {
            SpaceCheck(roster.Count + studentRoster.Count);
            foreach (StudentModel item in roster)
            {
                AddStudent(item);
            }
            return true;
        }

        public IEnumerable<StudentModel> GetIenumerableStudentList()
        {
            return studentRoster;
        }

        public async Task<List<StudentModel>> GetStudentRoster()
        {
            Console.WriteLine("init asynch task to get student roster");
            var results = await FetchRoster();
            Console.WriteLine($"number of students is {results.Count}");
            Console.WriteLine("Done asynch task to get roster");
            return results;
        }

        public Task<List<StudentModel>> FetchRoster()
        {
            //lambda inline function () => { return studentRoster; }
            return Task.Run(() => { return studentRoster; });
        }

        public void PrintRosterCount()
        {
            Console.WriteLine(studentRoster.Count);
        }

        public StudentModel GetStudentById(int id)
        {

            var student = studentRoster.Where(s => s.StudentModelId == id).FirstOrDefault();

            return student;
        }



        public bool RemoveStudent(StudentModel student)
        {
            return studentRoster.Remove(student);
        }





        private bool SpaceCheck(int countDracula)
        {
            if (countDracula > Global.maxStudents)
            {
                throw new Exception(Errors.notEnoughSpace);
            }
            return true;
        }

        public bool RemoveStudentById(int id)
        {
            return studentRoster.Remove(GetStudentById(id));
        }
    }
}