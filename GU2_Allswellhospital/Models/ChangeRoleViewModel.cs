using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GU2_Allswellhospital.Models
{
    //Daniel Russell 13/05/2019

    /// <summary>
    /// Model used to handle the changing of user roles 
    /// </summary>
    public class ChangeRoleViewModel
    {

        ///<summary>
        ///user's username
        ///</summary>
        public string UserName { get; set; }

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
    }
}