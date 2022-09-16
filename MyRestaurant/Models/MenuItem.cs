using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRestaurant.Models
{
    public class MenuItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Menu Item ID")]
        public int MenuItemId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} cannot be empty")]
        [MinLength(2, ErrorMessage = "{0} cannot have lesser than {1} characters")]
        [Display(Name = "Menu Item Name")]
        public string MenuItemName { get; set; }


        [Display(Name = "Price")]
        [Required(ErrorMessage = "Price is required.")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Price must be a Numbers only.")]
        public int MenuPrice { get; set; }

        [Required]
        [DefaultValue(true)]
        [Display(Name = "Is Available")]
        public bool IsEnabled { get; set; } = true;


        #region Navigation Properties for Category Model

        [Required]
        [Display(Name = "Category Name")]
        public int CategoryId { get; set; }


        [ForeignKey(nameof(MenuItem.CategoryId))]
        public Category Category { get; set; }


        #endregion

        #region Navigation Properties for OrderDetail Model
        public ICollection<OrderDetail> OrderDetails { get; set; }
        #endregion

    }
}
