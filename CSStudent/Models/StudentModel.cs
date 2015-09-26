using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CSStudent.Models
{
    public class StudentModel
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ExpGraduation { get; set; }
        public string Name { get; set; }
        public bool CanCode { get; set; }
        public int CreditsLeft { get; set; }
        public string Advisor { get; set; }

        public List<string> Advisors = new List<string>(){"Fred Orchard", "Bill", "Bob", "Joe"};

        public StudentModel()
        {
            Advisor = "Fred Orchard";
        }
    }

    
}