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
    
    public partial class getCollaborators_Result
    {
        public int Id { get; set; }
        public int IdeaId { get; set; }
        public string collaborator_globalid { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public bool Freezed { get; set; }
        public string collaboratorname { get; set; }
    }
}
