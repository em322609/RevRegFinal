﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using RevRegFinal.Models;

namespace RevRegFinal.Models
{
   
    public static class DataAccess
    {

        private static string connection = "Data Source=revdb.camzcailekkc.us-west-2.rds.amazonaws.com,1433;Initial Catalog = RevRegMVC; Integrated Security = False; Persist Security Info=True;User ID = master; Password=12345678;Encrypt=False;";

        //Accessor function to get connection string
        public static string GetConnection()
        {
            return connection;
        }

        //helper function to set the connection string to string parameter s
        public static void SetConnection(string s)
        {
            connection = s;
        }


        //verifyLogin will take incoming StudentModel object as well as email and password
        //specified from Index input form to check whether or not that student exists in the DB
        public static bool verifyLogin(StudentModel student, string email, string password)
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
                    if ((string)item["password"] == password && (string)item["Email"] == email)
                    {
                        student.FullName = (string)item["FullName"];
                        student.Email = email;
                        student.Password = password;
                        student.Major = (string)item["Major"];
                        student.StudentModelId = (int)item[0];
                        
                        return true;
                    }
                }
                return false;


            }
        }

        //method that returns a given student from the DB specified by the primary key idnum
        //will populate StudentModel object with Sql query results
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

        //will return a CourseModel object specified by the courseModelId in the DB
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
                            string name = tempid;
                            string startTime = (string)reader["timeOfDay"];
                            string Location = (string)reader["Location"];
                            int creditHours = (int)reader["creditHour"];
                            int numOfStudents = (int)reader["numStudents"];
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


        //Will return a list of courses that the student specified by idnum
        //is registered for.
        public static List<CourseModel> getStudentSchedule(int idnum)
        {
           List <CourseModel> schedule = new List<CourseModel>();

            string query = "Select * From [REGISTRATION] WHERE [StudentModelId] ="+idnum+";";
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(connection))
                {
                    SqlCommand command = new SqlCommand(query, sqlcon);

                    sqlcon.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {


                        string course = (string)reader["CourseModelId"];
                        CourseModel tempCourse = getCourse(course);


                        schedule.Add(tempCourse);



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


        //method to return a list of all courses in [COURSEMODELS]
        public static List<CourseModel> getCourses()
        {

            List<CourseModel> courseList = new List<CourseModel>();
            string query = "Select * From COURSEMODELS";

            using (SqlConnection sqlcon = new SqlConnection(connection))
            {
                SqlCommand command = new SqlCommand(query, sqlcon);

                sqlcon.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string tempid = (string)reader["CourseModelId"];

                    string name = tempid;
                    string startTime = (string)reader["timeOfDay"];
                    string Location = (string)reader["Location"];
                    int creditHours = (int)reader["creditHour"];
                    int numOfStudents = (int)reader["numStudents"];
                    int isClosed = (int)reader["isClosed"];


                    courseList.Add(new CourseModel(name, startTime, Location, creditHours, numOfStudents, isClosed));

                }
                reader.Close();
                return courseList;
            }
        }
 
        //will add studentmodelid value & coursemodelid value into the registration table in the DB
        public static bool RegisterStudent(string courseID, int studentID)
        {
            CourseModel c = getCourse(courseID);
           StudentModel s = getStudent(studentID);

            
            try
            {
                string sqlCommand = "INSERT INTO [REGISTRATION] values (" + studentID + "," + "'"+ courseID + "');";
                using (SqlConnection sqlcon = new SqlConnection(connection))
                {
                    DataSet ds = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(sqlCommand, sqlcon);
                    try
                    {
                        
                        adapter.Fill(ds);
                        UpdateCourseCount(c);
                        if (c.RosterCount.Equals(20))
                        {
                            UpdateCourseStatus(c);
                        }
                        return true;
                    }
                    catch (Exception e)
                    {

                        Console.WriteLine(e.Message);
                    }

                }
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException(Errors.coundNotConnectToDatabase, e);
            }
            return false;


        }
        
        public static bool UpdateCourseCount(CourseModel course)
        {
            try
            {
                string sqlCommand = "UPDATE [COURSEMODELS] SET [numStudents] = [numStudents]+1 WHERE [courseModelId] ="+"'"+course.CourseModelId+"'"+";";
                using (SqlConnection sqlcon = new SqlConnection(connection))
                {
                    DataSet ds = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(sqlCommand, sqlcon);
                    try
                    {

                        adapter.Fill(ds);
                        return true;
                    }
                    catch (Exception e)
                    {

                        Console.WriteLine(e.Message);
                    }

                }
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException(Errors.coundNotConnectToDatabase, e);
            }
            return false;
        }

        public static bool UpdateCourseStatus(CourseModel course)
        {
            try
            {
                string sqlCommand = "UPDATE [COURSEMODELS] SET [isClosed] = 1 WHERE [courseModelId] =" + "'" + course.CourseModelId + "'" + ";";
                using (SqlConnection sqlcon = new SqlConnection(connection))
                {
                    DataSet ds = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(sqlCommand, sqlcon);
                    try
                    {
                        adapter.Fill(ds);
                        return true;
                    }
                    catch (Exception e)
                    {

                        Console.WriteLine(e.Message);
                    }

                }
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException(Errors.coundNotConnectToDatabase, e);
            }
            return false;
        }
    }
}