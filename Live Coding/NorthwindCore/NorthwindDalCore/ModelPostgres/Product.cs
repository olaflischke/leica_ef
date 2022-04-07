using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NorthwindDalCore.ModelPostgres
{
    [Table("products")]
    public partial class Product
    {
        [Key]
        [Column("product_id")]
        public short ProductId { get; set; }
        [Column("product_name")]
        [StringLength(40)]
        public string ProductName { get; set; } = null!;
        [Column("supplier_id")]
        public short? SupplierId { get; set; }
        [Column("category_id")]
        public short? CategoryId { get; set; }
        [Column("quantity_per_unit")]
        [StringLength(20)]
        public string? QuantityPerUnit { get; set; }
        [Column("unit_price")]
        public float? UnitPrice { get; set; }
        [Column("units_in_stock")]
        public short? UnitsInStock { get; set; }
        [Column("units_on_order")]
        public short? UnitsOnOrder { get; set; }
        [Column("reorder_level")]
        public short? ReorderLevel { get; set; }
        [Column("discontinued")]
        public int Discontinued { get; set; }
    }
}
