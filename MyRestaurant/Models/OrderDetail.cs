using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRestaurant.Models
{
    public class OrderDetail
    {

        [Display(Name = "Order Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailId;


        [Display(Name = "Order Quantity")]
        [Required]
        public int Quantity { get; set; }


        [Display(Name = "Customer Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        [Display(Name = "Customer Name")]
        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use Characters only")]
        public string CustomerName { get; set; }


        [Display(Name = "Customer Address")]
        [Required]
        [StringLength(100)]
        public string Address { get; set; }


        [Display(Name = "Date And Time")]
        [Required]      
        public DateTime OrderDateTime { get; set; } = DateTime.Now;


        #region Navigation Properties to the MenuItemModel

        public int MenuItemId { get; set; }
        [ForeignKey(nameof(OrderDetail.MenuItemId))]
        public MenuItem MenuItem { get; set; }

        #endregion



    }
}
