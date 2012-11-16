using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoliticImpact.Models
{
    public class CaseItem
    {
        /*
            * Variables from "Dropbox/Arkitektur/tables.doc"
            * */
        [Required]
        public int ID { get; set; }

        [Required]
        public long Owner { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "You must have a description.", MinimumLength = 1)]
        public string Text { get; set; }

        [Required]
        public bool Published { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Deadline { get; set; }

        [DataType(DataType.DateTime) ]
        public DateTime LastEdited { get; set; }

        public int imageId { get; set; }

        public byte[] imageBytes { get; set; }

        public string imageName { get; set; }
        
        //added by Johannes Ullström 2012-11-08 15:11
        [Required]
        [StringLength(100, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string RecieverName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Not valid email")]
        public string RecieverEmail { get; set; }

    }
    
    public class CaseSignUp
    {
        [Key]
        public int caseID { get; set; }

        public long userID { get; set; }

        public virtual CaseItem CaseItem { get; set; }

        public DateTime created { get; set; }
    }

    //Frida Mattisson 2012-11-16
    public class CaseComment
    {
        [Key]
        public int commentID { get; set; }
        public long userID { get; set; }
        [ForeignKey ("caseID")] 
        public int caseID { get; set; }
        public virtual CaseItem CaseItem { get; set; }
        public string commentStr{ get; set;}
    }
    
}