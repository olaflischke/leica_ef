using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NorthwindDalCore.ModelPostgres
{
    [Table("employees")]
    public partial class Employee
    {
        public Employee()
        {
            InverseReportsToNavigation = new HashSet<Employee>();
            Orders = new HashSet<Order>();
        }

        [Key]
        [Column("employee_id")]
        public short EmployeeId { get; set; }
        [Column("last_name")]
        [StringLength(20)]
        public string LastName { get; set; } = null!;
        [Column("first_name")]
        [StringLength(10)]
        public string FirstName { get; set; } = null!;
        [Column("title")]
        [StringLength(30)]
        public string? Title { get; set; }
        [Column("title_of_courtesy")]
        [StringLength(25)]
        public string? TitleOfCourtesy { get; set; }
        [Column("birth_date")]
        public DateOnly? BirthDate { get; set; }
        [Column("hire_date")]
        public DateOnly? HireDate { get; set; }
        [Column("address")]
        [StringLength(60)]
        public string? Address { get; set; }
        [Column("city")]
        [StringLength(15)]
        public string? City { get; set; }
        [Column("region")]
        [StringLength(15)]
        public string? Region { get; set; }
        [Column("postal_code")]
        [StringLength(10)]
        public string? PostalCode { get; set; }
        [Column("country")]
        [StringLength(15)]
        public string? Country { get; set; }
        [Column("home_phone")]
        [StringLength(24)]
        public string? HomePhone { get; set; }
        [Column("extension")]
        [StringLength(4)]
        public string? Extension { get; set; }
        [Column("photo")]
        public byte[]? Photo { get; set; }
        [Column("notes")]
        public string? Notes { get; set; }
        [Column("reports_to")]
        public short? ReportsTo { get; set; }
        [Column("photo_path")]
        [StringLength(255)]
        public string? PhotoPath { get; set; }

        [ForeignKey("ReportsTo")]
        [InverseProperty("InverseReportsToNavigation")]
        public virtual Employee? ReportsToNavigation { get; set; }
        [InverseProperty("ReportsToNavigation")]
        public virtual ICollection<Employee> InverseReportsToNavigation { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
