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
    
    public partial class M_DictionaryDisease
    {
        public M_DictionaryDisease()
        {
            this.New = 0;
            this.M_L4Diseases = new HashSet<M_L4Disease>();
        }
    
        public int Id { get; private set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int New { get; set; }
    
        public virtual ICollection<M_L4Disease> M_L4Diseases { get; set; }
    }
}
