using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;

namespace IMSAPI.Models
{
    public class IMSClass
    {
        public int Id { get; set; }
        public int IdeaId { get; set; }
        public string IdeaCode { get; set; }
        public string IdeaTitle { get; set; }
        public string Description { get; set; }
        public string EvaluationStatus { get; set; }
        public int ProcessingStatusId { get; set; }
        public string GlobalId { get; set; }
        public int CategoryCode { get; set; }
        public List<IdeaFile> IdeaFiles { get; set; }
        public List<IdeaValue> IdeaValues { get; set; }
        public string Status { get; set; }
        public int EOI_Score { get; set; }
        public int CSP_Score { get; set; }
        public string Remarks { get; set; }
        public decimal TwelveMonthsSavings { get; set; }
        public int PaybackPeriod { get; set; }
        public decimal M1 { get; set; }
        public decimal M2 { get; set; }
        public decimal M3 { get; set; }
        public decimal M4 { get; set; }
        public decimal M5 { get; set; }
        public decimal M6 { get; set; }
        public decimal M7 { get; set; }
        public decimal M8 { get; set; }
        public decimal M9 { get; set; }
        public decimal M10 { get; set; }
        public decimal M11 { get; set; }
        public decimal M12 { get; set; }
        public string ImplementHOD { get; set; }
        public string ImplementLead { get; set; }
        public string Member { get; set; }
        public string membertype { get; set; }
        public string actionremarks { get; set; }
        public List<IdeaFile> EvaluationFiles { get; set; }
        public List<IdeaFile> TrackingFiles { get; set; }
        public string actualimplementationstartstr { get; set; }
        public Nullable<DateTime> actualimplementationstarts { get; set; }
        public string actualimplementationendstr { get; set; }
        public Nullable<DateTime> actualimplementationends { get; set; }
        public string actualsavingstartstr { get; set; }
        public Nullable<DateTime> actualsavingstarts { get; set; }
        public decimal ImplementationPeriod { get; set; }

        public string awardingremarks { get; set; }
        public string shortlistremarks { get; set; }
        public Nullable<int> EvaluationStatusCode { get; set; }
        public Nullable<DateTime> Createdon { get; set; }
        public string Createdby { get; set; }
        public string Createdbyname { get; set; }
        public string CategoryName { get; set; }
        public string IdeaValueText { get; set; }
        public string locationname { get; set; }
        public string departmentname { get; set; }
        public Nullable<double> AVG_EOI { get; set; }
        public Nullable<double> AVG_CSP { get; set; }
        public Nullable<double> Idea_score { get; set; }
        public Boolean isboosted { get; set; }
        public List<IMS_getTeamDetailsIdeaid_Result> teammembers { get; set; }
        public IMS_GetIdeaEvaluationByIdeaid_Result ideaevaluation { get; set; }
        public IMS_GetIdeaTrackingByIdeaid_Result ideatracking { get; set; }
        public List<IMS_GetIdeaValues_Result> ideavalues { get; set; }
        public List<CollaboratorModel> collaborators { get; set; }
        public List<getCollaborators_Result> collaboratorsview { get; set; }
        public string onbehalfselectionid { get; set; }
        public string onbehalfselectionidname { get; set; }
        public string onbehalfselectionremarks { get; set; }
        public string onbehalfscoringid { get; set; }
        public string onbehalfscoringidname { get; set; }
        public string onbehalfscoringremarks { get; set; }
        public string CCID { get; set; }
        public string CCIDname { get; set; }
        public decimal incentiveearned { get; set; }


    }


    public class IdeaFile
    {
        public string FileName { get; set; }
        public int FileLength { get; set; }
        public string FileType { get; set; }
        // public byte[] FileAttachment { get; set; }
        public string Base64string { get; set; }
        public string attachmenttype { get; set; }
    }

    public class IdeaValue
    {
        public int IdeaValueId { get; set; }
        public string IdeaValueText { get; set; }
    }

    public class MUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AuthenticationMode { get; set; }
        public string samAccountName { get; set; }
        public string adid { get; set; }
    }

    public class Customresponse
    {
        public HttpStatusCode status { get; set; }
        public getEmployeeByGlobalID_Result emp1 { get; set; }
        public spChkUserAD_Result emp2 { get; set; }
        public string stringresult { get; set; }
        public List<string> stringresultarray { get; set; }
    }

    public class UserRoleMatrix
    {
        public int Id { get; set; }
        public int location { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string adid { get; set; }
        public int userid { get; set; }
        public int roleid { get; set; }
        public int createdby { get; set; }
        public string globalid { get; set; }
        public int locationid { get; set; }
        public int departmentid { get; set; }
        public int subdepartmentid { get; set; }
        public string managerid { get; set; }
        public string hodid { get; set; }
        public string verifier { get; set; }
        public string approver { get; set; }
        public string departmentname { get; set; }
        public string subdepartmentname { get; set; }
        public string ipaddress { get; set; }
        public string remarks { get; set; }
        public string savetype { get; set; }

    }

    public class CollaboratorModel
    {
        public int Id { get; set; }
        public int IdeaId { get; set; }
        public string collaborator_globalid { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    public class IMSQuery
    {
        public int Id { get; set; }
        public string QueryText { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string Filetype { get; set; }
        public string Base64String { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string ResponseText { get; set; }
        public string ResponseBy { get; set; }
        public string Status { get; set; }
    }
}