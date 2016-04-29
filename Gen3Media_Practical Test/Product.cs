using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gen3Media_Practical_Test
{
    /// <summary> 
    /// Defines a Product sold in a Retail POS system. 
    /// NOTE: In a real world scenario this class would have more properties, 
    /// however, for the purposes of this test, it has only two 
    /// </summary> 
    public class Product
    {
        /// <summary> 
        /// Gets or sets the product identifier. 
        /// </summary> 
        /// <value> 
        /// The product identifier.  			/// </value> 
        public int ProductId { get; set; }
        /// <summary> 
        /// Gets or sets the minimum price for a product. A product must never  	
        /// /// be sold for a price lower than the minimum price. 
        /// </summary>  			/// <value> 
        /// The minimum price. 
        /// </value> 
        public decimal MinimumPrice { get; set; }


    }
}
