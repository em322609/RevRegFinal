using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using RevRegFinal.Models;

namespace RevRegFinal.Models
{
   
    public static class DataConnection
    {

        private static string connection = "Data Source=revdb.camzcailekkc.us-west-2.rds.amazonaws.com,1433;Initial Catalog = RevRegMVC; Integrated Security = False; Persist Security Info=True;User ID = master; Password=12345678;Encrypt=False;";
        public static string GetConnection()
        {
            return connection;
        }
        public static void SetConnection(string s)
        {
            connection = s;
        }

        public static bool CheckLogInInfo(StudentModel s)
        {
            string query = "SELECT * FROM STUDENTMODELS";

            using (SqlConnection sqlcon = new SqlConnection(connection))
            {
                SqlCommand command = new SqlCommand(query, sqlcon);

                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter();

                adapter.SelectCommand = new SqlCommand(query, sqlcon);

                adapter.Fill(ds);
                foreach (var item in ds.Tables[0].AsEnumerable())
                {
                    if ((string)item["password"] == s.Password && (string)item["Email"] == s.Email)
                    {
                        s.FullName = (string)item["FullName"];
                        
                        s.Major = (string)item["Major"];
                        s.StudentModelId = (int)item[0];
                        s.AddCourses(getStudentSchedule(s.StudentModelId));
                        return true;
                    }
                }
                return false;


            }
        }
        public static StudentModel getStudent(int idnum)
        {

            StudentModel tempStudent = new StudentModel();

            string query = "SELECT * FROM STUDENTMODELS";
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(connection))
                {
                    SqlCommand command = new SqlCommand(query, sqlcon);

                    sqlcon.Open(); //InvalidOperationException
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int tempid = (int)reader["StudentModelId"];
                        if (tempid == idnum)

                        {
                            string fullname = (string)reader["fullName"];
                            string email = (string)reader["Email"];
                            string password = (string)reader["Password"];
                            string major = (string)reader["Major"];

                            tempStudent = new StudentModel(fullname, password, email, idnum, major);

                        }


                    }
                    reader.Close();
                    if (tempStudent.StudentModelId != idnum) //the student with that ID was not found
                    {
                        throw new IndexOutOfRangeException(Errors.studentNotFound);
                    }
                    else
                    {
                        return tempStudent;
                    }
                }
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException(Errors.coundNotConnectToDatabase, e);
            }



        }
        
        public static CourseModel getCourse(string idnum)
        {
            CourseModel tempCourse = new CourseModel();

            string query = "Select * From COURSEMODELS";
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(connection))
                {
                    SqlCommand command = new SqlCommand(query, sqlcon);

                    sqlcon.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string tempid = (string)reader["CourseModelId"];
                        if (tempid == idnum)

                        {
                            string name = (string)reader["CourseName"];
                            string startTime = (string)reader["timeOfDay"];
                            string Location = (string)reader["Location"];
                            int creditHours = (int)reader["CreditHours"];
                            int numOfStudents = (int)reader["numOfStudents"];
                            int isClosed = (int)reader["isClosed"];


                            tempCourse = new CourseModel(name, startTime, Location, creditHours, numOfStudents,isClosed);

                        }

                    }
                    reader.Close();
                    if (tempCourse.CourseModelId != idnum)//Course not found 
                    {
                        throw new IndexOutOfRangeException(Errors.courseNotFound);
                    }
                    else
                    {
                        return tempCourse;
                    }
                }
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException(Errors.coundNotConnectToDatabase, e);
            }

        }
        
        public static Dictionary<string, CourseModel> getStudentSchedule(int idnum)
        {
            Dictionary<string, CourseModel> schedule = new Dictionary<string, CourseModel>();

            string query = $"Select * From COURSEMODELS({idnum})";
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(connection))
                {
                    SqlCommand command = new SqlCommand(query, sqlcon);

                    sqlcon.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {


                        string name = (string)reader["CourseModelId"];
                        string startTime = (string)reader["timeOfDay"];
                        string location = (string)reader["Location"];
                        int creditHours = (int)reader["CreditHours"];
                        int numOfStudents = (int)reader["numOfStudents"];
                        int isClosed = (int)reader["isClosed"];


                        schedule.Add(name, new CourseModel(name, startTime, location, creditHours,numOfStudents,isClosed));



                    }
                    reader.Close();

                }
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException(Errors.coundNotConnectToDatabase, e);
            }
            return schedule;

        }
       
        
       
 
     
        public static bool RegisterStudentForCourse(string courseID, int studentID)
        {
            getCourse(courseID); //Will Throw IndexOutOfRange exception if the course is not found
            getStudent(studentID); //Will Throw IndexOUtOfRange exception if the student is not found
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {

                    SqlCommand cmd = new SqlCommand("RegisterStudentForCourse", con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add("@StudentModelId", SqlDbType.Int).Value = studentID;
                    cmd.Parameters.Add("@CourseModelId", SqlDbType.NVarChar).Value = courseID;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return true;

                }
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException(Errors.coundNotConnectToDatabase, e);
            }


        }
        
    }
}