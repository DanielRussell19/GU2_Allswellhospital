using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GU2_Allswellhospital.Models
{
    public class ModifyStaffViewModel
    {

        public string tempid { get; set; }

        //username of new staff
        public string Username { get; set; }

        //email of new staff
        public string Email { get; set; }

        //staff first name
        public string Forename { get; set; }

        //staff last name
        public string Surname { get; set; }
      
        public string Telnum { get; set; }

        public string Street { get; set; }

       
        public string Town { get; set; }

        
        public string City { get; set; }

        
        public DateTime DOB { get; set; }

        //staff password
        public string Password { get; set; }

        ///<summary>
        ///user's previous role
        ///</summary>
        public string OldRole { get; set; }

        ///<summary>
        ///user's new role selected from view
        ///</summary>
        [Required, Display(Name = "Role")]
        public string Role { get; set; }

        ///<summary>
        ///temp to hold roles loaded
        ///</summary>
        public ICollection<SelectListItem> Roles { get; set; }

        //constructor
        public ModifyStaffViewModel()
        {
            Username = Email;
        }

    }
}