//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MaintenanceWebUtilityWebForm2
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employee
    {
        public Employee()
        {
            this.ChangeLogs = new HashSet<ChangeLog>();
        }
    
        public int Id { get; set; }
        public int EmployeeTypeId { get; set; }
        public string Username { get; set; }
        public string PasswordTxt { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
    
        public virtual ICollection<ChangeLog> ChangeLogs { get; set; }
        public virtual EmployeeType EmployeeType { get; set; }
    }
}