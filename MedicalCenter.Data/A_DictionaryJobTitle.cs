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
    
    public partial class A_DictionaryJobTitle
    {
        public A_DictionaryJobTitle()
        {
            this.IsDeleted = false;
            this.New = 0;
        }
    
        public int Id { get; private set; }
        public string JobTitle { get; set; }
        public string Code { get; set; }
        public bool IsDeleted { get; set; }
        public int New { get; set; }
    }
}
