using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NorthwindDalCore.ModelPostgres
{
    [Table("orders")]
    public partial class Order
    {
        [Key]
        [Column("order_id")]
        public short OrderId { get; set; }
        [Column("customer_id", TypeName = "char")]
        public char? CustomerId { get; set; }
        [Column("employee_id")]
        public short? EmployeeId { get; set; }
        [Column("order_date")]
        public DateOnly? OrderDate { get; set; }
        [Column("required_date")]
        public DateOnly? RequiredDate { get; set; }
        [Column("shipped_date")]
        public DateOnly? ShippedDate { get; set; }
        [Column("ship_via")]
        public short? ShipVia { get; set; }
        [Column("freight")]
        public float? Freight { get; set; }
        [Column("ship_name")]
        [StringLength(40)]
        public string? ShipName { get; set; }
        [Column("ship_address")]
        [StringLength(60)]
        public string? ShipAddress { get; set; }
        [Column("ship_city")]
        [StringLength(15)]
        public string? ShipCity { get; set; }
        [Column("ship_region")]
        [StringLength(15)]
        public string? ShipRegion { get; set; }
        [Column("ship_postal_code")]
        [StringLength(10)]
        public string? ShipPostalCode { get; set; }
        [Column("ship_country")]
        [StringLength(15)]
        public string? ShipCountry { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("Orders")]
        public virtual Customer? Customer { get; set; }
        [ForeignKey("EmployeeId")]
        [InverseProperty("Orders")]
        public virtual Employee? Employee { get; set; }
    }
}
