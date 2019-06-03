using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GU2_Allswellhospital.Models
{
    //Daniel Russell 15/5/2019

    /// <summary>
    /// View model used to edit and create staff
    /// </summary>
    public class ModifyStaffViewModel
    {
        /// <summary>
        ///temp value for user id
        /// </summary>
        public string tempid { get; set; }

        /// <summary>
        ///username of new staff
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///email of new staff
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        ///staff first name
        /// </summary>
        [Required]
        public string Forename { get; set; }

        /// <summary>
        ///staff last name
        /// </summary>
        [Required]
        public string Surname { get; set; }

        /// <summary>
        ///staff phonenumber
        /// </summary>
        [Required]
        [MinLength(9)]
        public string Telnum { get; set; }

        /// <summary>
        ///staff street
        /// </summary>
        [Required]
        public string Street { get; set; }

        /// <summary>
        ///staff town
        /// </summary>
        [Required]
        public string Town { get; set; }

        /// <summary>
        ///staff city
        /// </summary>
        [Required]
        public string City { get; set; }

        /// <summary>
        ///staff date of birth
        /// </summary>
        [Required]
        public DateTime DOB { get; set; }

        /// <summary>
        ///staff password
        /// </summary>
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