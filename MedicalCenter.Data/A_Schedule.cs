//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MedicalCenter.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class A_Schedule
    {
        public A_Schedule()
        {
            this.WorkerId = 0;
        }
    
        public int Id { get; private set; }
        public int WorkerId { get; set; }
        public System.DateTime ValidFrom { get; set; }
        public Nullable<System.DateTime> ValidTo { get; set; }
        public Nullable<System.DateTime> D1From { get; set; }
        public Nullable<System.DateTime> D1To { get; set; }
        public Nullable<System.DateTime> D2From { get; set; }
        public Nullable<System.DateTime> D2To { get; set; }
        public Nullable<System.DateTime> D3From { get; set; }
        public Nullable<System.DateTime> D3To { get; set; }
        public Nullable<System.DateTime> D4From { get; set; }
        public Nullable<System.DateTime> D4To { get; set; }
        public Nullable<System.DateTime> D5From { get; set; }
        public Nullable<System.DateTime> D5To { get; set; }
        public Nullable<System.DateTime> D6From { get; set; }
        public Nullable<System.DateTime> D6To { get; set; }
        public Nullable<System.DateTime> D7From { get; set; }
        public Nullable<System.DateTime> D7To { get; set; }
    
        public virtual A_Worker A_Worker { get; set; }
    }
}