//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Diversity
{
    using System;
    using System.Collections.Generic;
    
    public partial class DiscountCode
    {
        public int DiscountCodeID { get; set; }
        public string Code { get; set; }
        public decimal DiscountAmount { get; set; }
        public System.DateTime ValidFrom { get; set; }
        public System.DateTime ValidUntil { get; set; }
    }
}
