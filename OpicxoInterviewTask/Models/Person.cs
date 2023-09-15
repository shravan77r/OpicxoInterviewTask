using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OpicxoInterviewTask.Models
{
    public class Person
    {
        public int SrNo { get; set; }
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Name")]  
        public string PersonName { get; set; }

        [Required(ErrorMessage = "Please Enter Weight")]
        public decimal PersonWeight { get; set; }

        [Required(ErrorMessage = "Please Enter Weight")]
        public decimal PersonHeight { get; set; }

        public decimal BMI { get; set; }

        public int Gender { get; set; }
        public string GenderName { get; set; }
        public string ACTION { get; set; }

    }
    public class Activities
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public decimal PersonWeight { get; set; }
        public decimal PersonHeight { get; set; }

        public int Id { get; set; }
        public string ActivityDate { get; set; }
        public string WakeUpTime { get; set; }
        public int IsGym { get; set; }
        public int IsMeditation { get; set; }
        public string MeditationMinutes { get; set; }
        public int IsRead { get; set; }
        public string ReadPages { get; set; }
        public string ACTION { get; set; }
    }
}