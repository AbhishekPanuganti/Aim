//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IMSAPI.Models
{
    using System;
    
    public partial class IMS_GetCCListById_Result
    {
        public int Id { get; set; }
        public string IdeaId { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> Updatedon { get; set; }
        public Nullable<int> updatedby { get; set; }
        public string Updatedbyname { get; set; }
        public Nullable<int> EOI_Score { get; set; }
        public Nullable<int> CSP_Score { get; set; }
        public string selectionremarks { get; set; }
        public string scoringremarks { get; set; }
        public string CCID { get; set; }
        public string CCIDname { get; set; }
        public string onbehalfselectionremarks { get; set; }
        public string onbehalfscoringremarks { get; set; }
    }
}
