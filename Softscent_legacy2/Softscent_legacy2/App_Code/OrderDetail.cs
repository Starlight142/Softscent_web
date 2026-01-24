using System;
using System.ComponentModel.DataAnnotations;

namespace Softscent.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public Product ProductInfo { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public string CustomConfiguration { get; set; }
    }
}
