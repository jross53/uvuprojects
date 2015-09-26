using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;

namespace CSStudent.Models
{
    public class DatabaseQueries
    {
        private static SqlCeConnection CreateConnection()
        {
            SqlCeConnection conn = new SqlCeConnection();
            conn.ConnectionString =
                ConfigurationManager.ConnectionStrings["CSStudent"].ConnectionString;
            conn.Open();

            return conn;
        }

        public static void CreateStudent(StudentModel student)
        {
            string cmdText =
                @"INSERT INTO Student (ExpGraduation, Name, CanCode, CreditsLeft, Advisor)
                  VALUES (@expGraduation, @name, @canCode, @creditsLeft, @advisor)";

            using (SqlCeConnection conn = CreateConnection())
            {
                using (SqlCeCommand cmd = new SqlCeCommand(cmdText, conn))
                {
                    cmd.Parameters.AddWithValue("@expGraduation", student.ExpGraduation);
                    cmd.Parameters.AddWithValue("@name", student.Name);
                    cmd.Parameters.AddWithValue("@canCode", student.CanCode);
                    cmd.Parameters.AddWithValue("@creditsLeft", student.CreditsLeft);
                    cmd.Parameters.AddWithValue("@advisor", student.Advisor);

                    int rowsAffected = cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<StudentModel> ReadAllStudents()
        {
            string cmdText =
                @"SELECT Id, ExpGraduation, Name, CanCode, CreditsLeft, Advisor FROM Student";

            List<StudentModel> list = new List<StudentModel>();

            using (SqlCeConnection conn = CreateConnection())
            {
                using (SqlCeCommand cmd = new SqlCeCommand(cmdText, conn))
                {
                    using (SqlCeDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StudentModel item = new StudentModel()
                            {
                                Id = (int) reader["Id"],
                                ExpGraduation = (DateTime) reader["ExpGraduation"],
                                Name = (string) reader["Name"],
                                CanCode = (bool) reader["CanCode"],
                                CreditsLeft = (int) reader["CreditsLeft"],
                                Advisor = (string) reader["Advisor"]
                            };
                            list.Add(item);
                        }
                    }
                }
            }

            return list; 
        }

        public static StudentModel ReadStudent(int id)
        {
            string cmdText =
                @"SELECT Id, ExpGraduation, Name, CanCode, CreditsLeft, Advisor FROM Student
                  WHERE Id = @id";

            StudentModel item = null;

            using (SqlCeConnection conn = CreateConnection())
            {
                using (SqlCeCommand cmd = new SqlCeCommand(cmdText, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlCeDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item = new StudentModel()
                            {
                                Id = (int)reader["Id"],
                                ExpGraduation = (DateTime)reader["ExpGraduation"],
                                Name = (string)reader["Name"],
                                CanCode = (bool)reader["CanCode"],
                                CreditsLeft = (int)reader["CreditsLeft"],
                                Advisor = (string)reader["Advisor"]
                            };
                        }
                    }
                }
            }

            return item; 
        }

        public static void UpdateStudent(StudentModel student)
        {
            string cmdText =
                @"UPDATE Student
                    SET
                        ExpGraduation = @expGraduation,
                        Name = @name,
                        CanCode = @canCode,
                        CreditsLeft = @creditsLeft,
                        Advisor = @advisor
                    WHERE Id = @id";

            using (SqlCeConnection conn = CreateConnection())
            {
                using (SqlCeCommand cmd = new SqlCeCommand(cmdText, conn))
                {
                    cmd.Parameters.AddWithValue("@id", student.Id);
                    cmd.Parameters.AddWithValue("@expGraduation", student.ExpGraduation);
                    cmd.Parameters.AddWithValue("@name", student.Name);
                    cmd.Parameters.AddWithValue("@canCode", student.CanCode);
                    cmd.Parameters.AddWithValue("@creditsLeft", student.CreditsLeft);
                    cmd.Parameters.AddWithValue("@advisor", student.Advisor);

                    cmd.ExecuteNonQuery();   
                }
            }
        }

        public static void DeleteStudent(int id)
        {
            string cmdText =
                @"DELETE FROM Student WHERE Id = @id";

            using (SqlCeConnection conn = CreateConnection())
            {
                using (SqlCeCommand cmd = new SqlCeCommand(cmdText, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}