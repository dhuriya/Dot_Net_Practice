using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManagement.API.Model
{
    public class Student
    {
        public int Id {get; set;}
        public string Admission_number {get; set;} = string.Empty;
        public string Name {get; set;} = string.Empty;
        public string? Lastname {get; set;}
        public string FatherName {get; set;} = string.Empty;
        public string MotherName {get; set;} = string.Empty;
        public string PatentPhone {get; set;} = string.Empty;
        public string DOB {get; set;} = string.Empty;
        public string Gender {get; set;} = string.Empty;
        public string Email {get; set;} = string.Empty;
        public string Mobile {get; set;} = string.Empty;
        public string Address {get; set;} = string.Empty;
        public string ClassId {get; set;} = string.Empty;
        public string SectionId {get; set;} = string.Empty;
        public string RollNo {get; set;} = string.Empty;
        public DateTime AdmissionDate {get; set;}
        public string Status {get; set;} = string.Empty;
        public DateTime CreatedDate {get; set;}
        public DateTime UpdatedDate {get; set;}
    }
}