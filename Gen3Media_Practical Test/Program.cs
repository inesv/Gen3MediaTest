using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Gen3Media_Practical_Test
{
    public class Program
    {
        public List<SaleItem> ItemList= new List<SaleItem>();
        public List<Product> ProductList= new List<Product>();
        public decimal newSaleTotal,definedSaleTotal;
        public int example;
        static void Main(string[] args)
        {
            Program prg = new Program();
            prg.DoWork();
          
            Console.ReadLine();
            
        }

        public  void DoWork()
        {
            for (int i = 1; i <= 4; i++)
            {
                example = i;
                ItemList = CreateSaleItems(example);
                ProductList = CreateProducts(example);
                definedSaleTotal = newSaleTotal;
                ItemList = BulkDiscount(ItemList, ProductList);
                PresentSale();
            }

        }

        public List<SaleItem> BulkDiscount(List<SaleItem> ListofItems, List<Product> ListofProducts)
        {
           
            decimal SaleTotal = ListofItems.Sum(item => item.TotalPrice);
            decimal disc = 0, newdisc = 0, restdisc = 0, newrestdisc=0;
            decimal minprice = 0;
            decimal remainingSaleTotal = 0;
           /// Calculating and setting discount, or setting max discount for min price and storing remaining discount
           /// (Example1)
            foreach (SaleItem item in ListofItems)
            {
                disc = Math.Round(item.Quantity * item.UnitPrice * (SaleTotal - newSaleTotal) / SaleTotal, 2);
                minprice = ListofProducts.Single(s => s.ProductId == item.ProductId).MinimumPrice;

                if (minprice > 0 && (item.UnitPrice * item.Quantity - disc) / item.Quantity < minprice)
                {
                    newdisc = item.Quantity * (item.UnitPrice - minprice);
                    restdisc += disc - newdisc;
                    item.Discount = newdisc;
                }
                else
                {
                    item.Discount = disc;
                }

            }

            IEnumerable<SaleItem> availableitems = ListofItems.Where(
                s => s.UnitPrice != ListofProducts.Single(p => p.ProductId == s.ProductId).MinimumPrice &&
                ListofProducts.Single(p => p.ProductId == s.ProductId).MinimumPrice * s.Quantity != s.TotalPrice);

            IEnumerable<SaleItem> notavailableitems = ListofItems.Where(
                s => s.UnitPrice == ListofProducts.Single(p => p.ProductId == s.ProductId).MinimumPrice ||
                ListofProducts.Single(p => p.ProductId == s.ProductId).MinimumPrice * s.Quantity == s.TotalPrice);


            newrestdisc = restdisc;

            ///apportioning remaining discount while there are still items available for change
            ///(Example 2)
            while (restdisc > 0 && availableitems.Count()>0)  
            {

                remainingSaleTotal = Math.Round( availableitems.Sum(item => (item.Quantity * item.UnitPrice)), 2);

                foreach (SaleItem item in availableitems)
                {
                    minprice = ListofProducts.Single(s => s.ProductId == item.ProductId).MinimumPrice;

                    disc = Math.Round(item.Quantity * item.UnitPrice * restdisc / remainingSaleTotal, 2);
                    if (minprice > 0 && (item.UnitPrice * item.Quantity - disc) / item.Quantity < minprice)
                    {
                        newdisc = Math.Round(item.Quantity * (item.UnitPrice - minprice),2);
                        newrestdisc = Math.Round(newrestdisc - (disc - newdisc),2);
                        item.Discount += newdisc;
                    }
                    else
                    {
                        item.Discount += disc;
                        newrestdisc = Math.Round(newrestdisc - disc,2);
                    }
                }

               availableitems = ListofItems.Where(
               s => s.UnitPrice != ListofProducts.Single(p => p.ProductId == s.ProductId).MinimumPrice &&
               ListofProducts.Single(p => p.ProductId == s.ProductId).MinimumPrice * s.Quantity != s.TotalPrice);

               notavailableitems = ListofItems.Where(
                    s => s.UnitPrice == ListofProducts.Single(p => p.ProductId == s.ProductId).MinimumPrice ||
                    ListofProducts.Single(p => p.ProductId == s.ProductId).MinimumPrice * s.Quantity == s.TotalPrice);

                restdisc = newrestdisc;

            }
            ///if there is no more avilable items (all min price or discounted to 0) but  
            ///there is still remaining discount, NewSaleTotal is changed (Example 4)
            if (restdisc > 0)
            {
                newSaleTotal += restdisc;
            }
            ///ajusting rounding error if there is no remaining discount and there are still items available to change
            ///(Example 3)
            else if (ListofItems.Sum(item => item.TotalPrice)!= newSaleTotal)
            {
                if (availableitems.Count()>0)
                {
                  SaleItem maxSaleItem=  availableitems.OrderByDescending(p => (p.Quantity*p.UnitPrice)).First();
                    decimal runderr = ListofItems.Sum(item => item.TotalPrice) - newSaleTotal;
                    ListofItems.Single(p => p == maxSaleItem).Discount += runderr;
                    
                }
            }
            return ListofItems;
        }

        public void PresentSale()
        {
            Console.WriteLine(  "EXAMPLE " + example.ToString() + "\r\n \r\n");
            Console.WriteLine("Product Name \t Quantity \t Unit Price \t Discount \t Total ");
            Console.WriteLine("---------------------------------------------------------------------- ");
            foreach (SaleItem item in ItemList)
            {
                Console.WriteLine(item.Description + " \t \t " + item.Quantity.ToString() +
                    " \t \t " + item.UnitPrice.ToString() + " \t   \t" + item.Discount.ToString() +
                    " \t \t " + item.TotalPrice.ToString() + " \t ");
            }
            Console.WriteLine("---------------------------------------------------------------------- ") ;
            Console.WriteLine("New Sale Total \t " + Math.Round(definedSaleTotal,2).ToString() +
                "       /      Ajusted New Sale Total \t " + Math.Round(newSaleTotal,2).ToString() + "\r\n \r\n");
        }

        /// <summary>
        /// Creates Sale Items
        /// </summary>
        public List<SaleItem> CreateSaleItems(int example)
        {
            List<SaleItem> list1 = new List<SaleItem>();
            SaleItem item1 = new SaleItem();
            SaleItem item2 = new SaleItem();
            SaleItem item3 = new SaleItem();
           
            item1.Description = "Widget";
            item1.ProductId = 1;
            item1.Quantity = 1;
            item1.UnitPrice=1.75m;

            list1.Add(item1);

            item2.Description = "Gadget";
            item2.ProductId = 2;
            item2.Quantity = 2;
            item2.UnitPrice = 2.95m;

            list1.Add(item2);

            item3.Description = "Gizmo";
            item3.ProductId = 3;
            item3.Quantity = 3;
            item3.UnitPrice = 2.35m;

            list1.Add(item3);


           

            return list1;

        }
        /// <summary>
        /// Creates Products and Defines New Sale Price
        /// </summary> 
        public List<Product> CreateProducts(int example)
        {

            List<Product> list1 = new List<Product>();
            Product item1 = new Product();
            Product item2 = new Product();
            Product item3 = new Product();

          
            item1.ProductId = 1;
            item2.ProductId = 2;
            item3.ProductId = 3;
           
            switch (example)
            {


                case 1:
                    item1.MinimumPrice = 0;
                    item2.MinimumPrice = 0;
                    item3.MinimumPrice = 0;
                    newSaleTotal = 10;
                    break;
                case 2:
                    item1.MinimumPrice = 0;
                    item2.MinimumPrice = 2.50m;
                    item3.MinimumPrice = 0;
                    newSaleTotal = 10;
                    break;
                case 3:
                    item1.MinimumPrice = 0;
                    item2.MinimumPrice = 0;
                    item3.MinimumPrice = 0;
                    newSaleTotal = 10.23m;
                    break;
                case 4:
                    item1.MinimumPrice = 1.50m;
                    item2.MinimumPrice = 2.50m;
                    item3.MinimumPrice = 2.00m;
                    newSaleTotal = 10;
                    break;
            }

            list1.Add(item1);
            list1.Add(item2);
            list1.Add(item3);

            return list1;
        }
    }
}
