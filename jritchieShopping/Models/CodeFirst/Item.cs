using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jritchieShopping.Models.CodeFirst {

    public class Item {

        public int Id { get; set; } 
        public DateTime  CreateDate { get; set; }   
        public DateTime? UpdatedDate { get; set; }          // ? means nullable
        public string Name { get; set; }    
        public decimal Price { get; set; }   
        public string MediaURL { get; set; }   
        [AllowHtml]                                         // Allows Description field to contain HTML tags
        public string Description { get; set; } 
    }
}