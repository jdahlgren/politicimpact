
using System;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel;


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
        //added by Michaela
        public int numberOfLikes { get; set; }
        

        
        //Frida Mattisson 2012-11-19 (och document)
        public List<CaseComment> caseComment { get; set; }
        public int caseMode { get; set; }
                

        //added by Johannes Ullström 2012-11-08 15:11
        [Required]
        [StringLength(100, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string RecieverName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Not valid email")]
        public string RecieverEmail { get; set; }


        //added by Daniel Jonsson, David Falk 2012-11-13 13:59
        [DisplayName("Likes")]
        public bool enableLikes { get; set; }

        [DisplayName("Comments")]
        public bool enableComments { get; set; }

        [DisplayName("Signatures")]
        public bool enableSigns { get; set; }

        [Required(ErrorMessage = "Value must be set")]
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public virtual CaseCategory caseCategory { get; set; }

        //added by Joel Brüde, 2012-11-18 22:04
        [Required]
        public int ResponseID { get; set; }
        [ForeignKey("ResponseID")]
        public virtual RecieverResponse recieverResponse { get; set; }
    }
   
    public class CaseCategory
    {
        [Key]
        public int CategoryID { get; set; }

        public string Title { get; set; }
    }

    public class CaseVoting
    {
        [Key]
        public int VotingID { get; set; }
        public int CaseID { get; set; }
        [StringLength(100, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime Created { get; set; }
    }

    public class CaseVote
    {
        [Key]
        public int VoteID { get; set; }
        public int VotingID { get; set; }
        public long UserID { get; set; }
        [ForeignKey("VotingID")]
        public virtual CaseVoting casevoting { get; set; }
        public Boolean Vote { get; set; }
    }

    public class CaseLike
    {
        [Key]
        public long likeID { get; set; }
        public int caseID {get; set;}
        public long userID {get; set;}
        public DateTime created {get; set;}
    }
    

    public class CaseSignUp
    {
        [Key]
        public int caseID { get; set; }

        [Required]
        public long userID { get; set; }


        [ForeignKey("CaseItem")]
        public int CaseItemID { get; set; }
        public virtual CaseItem CaseItem { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime created { get; set; }
    }

    public class RecieverResponse
    {
        [Key]
        public int ResponseID { get; set; }

        public string ResponseText { get; set; }

        public string ResponseCode { get; set; }
    }

    //Frida Mattisson 2012-11-16
    public class CaseComment
    {
        [Key]
        public int commentID { get; set; }
        public long userID { get; set; }
        [ForeignKey ("CaseItem")] 
        public int caseID { get; set; }
        public virtual CaseItem CaseItem { get; set; }
        public string commentStr{ get; set;}
    }
    
    public class User
    {
        public string uid { get; set; }
        public string accessToken { get; set; }
        public string name { get; set; }
    }
    
}