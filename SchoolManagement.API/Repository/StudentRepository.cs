using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SchoolManagement.API.IRepository;
using SchoolManagement.API.Model;

namespace SchoolManagement.API.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly string _connectionString;

        public StudentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("School")!;
        }
        public async Task<Student> CreateAsync(Student student)
        {
            ArgumentNullException.ThrowIfNull(student);

            if (!DateTime.TryParse(student.DOB, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateOfBirth))
                throw new ArgumentException("DOB must be a valid date.", nameof(student));

            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand("dbo.ManageStudent", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@Action", SqlDbType.VarChar, 10).Value = "INSERT";
            command.Parameters.Add("@admission_number", SqlDbType.VarChar, 12).Value = student.Admission_number;
            command.Parameters.Add("@first_name", SqlDbType.VarChar, 50).Value = student.Name;
            command.Parameters.Add("@last_name", SqlDbType.VarChar, 40).Value = (object?)student.Lastname ?? DBNull.Value;
            command.Parameters.Add("@father_name", SqlDbType.VarChar, 50).Value = student.FatherName;
            command.Parameters.Add("@mother_name", SqlDbType.VarChar, 50).Value = student.MotherName;
            command.Parameters.Add("@parent_phone", SqlDbType.VarChar, 10).Value = student.PatentPhone;
            command.Parameters.Add("@date_of_birth", SqlDbType.Date).Value = dateOfBirth;
            command.Parameters.Add("@gender", SqlDbType.Char, 1).Value = student.Gender;
            command.Parameters.Add("@email", SqlDbType.VarChar, 50).Value = student.Email;
            command.Parameters.Add("@phone", SqlDbType.VarChar, 10).Value = student.Mobile;
            command.Parameters.Add("@address", SqlDbType.VarChar, -1).Value = student.Address;
            command.Parameters.Add("@class_id", SqlDbType.VarChar, 8).Value = student.ClassId;
            command.Parameters.Add("@section_id", SqlDbType.VarChar, 10).Value = student.SectionId;
            command.Parameters.Add("@roll_number", SqlDbType.VarChar, 11).Value = student.RollNo;
            command.Parameters.Add("@admission_date", SqlDbType.Date).Value = student.AdmissionDate;

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            if (result is null || result == DBNull.Value)
                throw new InvalidOperationException("The database did not return the new student ID.");

            student.Id = Convert.ToInt32(result);
            return student;

        }
        public async Task<IEnumerable<Student>> getAllStudentsAsync()
        {
            var students = new List<Student>();
            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand("dbo.ManageStudent", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@Action", SqlDbType.VarChar, 10).Value = "SELECT";
            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var student = new Student
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Admission_number = reader.GetString(reader.GetOrdinal("admission_number")),
                    Name = reader.GetString(reader.GetOrdinal("first_name")),
                    Lastname = reader.IsDBNull(reader.GetOrdinal("last_name"))? null: reader.GetString(reader.GetOrdinal("last_name")),
                    FatherName = reader.GetString(reader.GetOrdinal("father_name")),
                    MotherName = reader.GetString(reader.GetOrdinal("mother_name")),
                    PatentPhone = reader.GetString(reader.GetOrdinal("parent_phone")),
                    DOB = reader.GetDateTime(reader.GetOrdinal("date_of_birth")).ToString(),
                    Gender = reader.GetString(reader.GetOrdinal("gender")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Mobile = reader.GetString(reader.GetOrdinal("phone")),
                    Address = reader.GetString(reader.GetOrdinal("address")),
                    ClassId = reader.GetString(reader.GetOrdinal("class_id")),
                    SectionId = reader.GetString(reader.GetOrdinal("section_id")),
                    RollNo = reader.GetString(reader.GetOrdinal("roll_number")),
                    AdmissionDate = reader.GetDateTime("admission_date"),
                    Status = reader.IsDBNull(reader.GetOrdinal("status")).ToString(),
                    CreatedDate = reader.GetDateTime("created_at"),
                    //UpdatedDate = reader.GetDateTime("updated_at")
                };
                students.Add(student);
            }
            return students;
        }
    }
}