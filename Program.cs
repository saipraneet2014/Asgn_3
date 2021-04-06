using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace EntityFramework_Assignment
{
    class Order
    {
        public int OrderId { get; set; }
        public string CustName { get; set; }
        public List<OrderDetail> ProductList { get; set; }
    }



    class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ListPrice { get; set; }
        public int SupplierId { get; set; }
        public List<OrderDetail> OrderList { get; set; }
    }



    class OrderDetail
    {
        public int id { get; set; }




        public Order SaleOrderDetail { get; set; }
        public Product SaleProductDetail { get; set; }
        public DateTime OrderDate { get; set; }
    }



    class OrderDetailsContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=InClass10;Trusted_Connection=True;MultipleActiveResultSets=true";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }




    class Program
    {
        static void Main(string[] args)
        {
            using (OrderDetailsContext context = new OrderDetailsContext())
            {
                context.Database.EnsureCreated();



                Order order1 = new Order { CustName = "Clara" };
                Order order2 = new Order { CustName = "Jane" };
                Order order3 = new Order { CustName = "Kate" };
                Product product1 = new Product
                {
                    ProductName = "Syrup",
                    ListPrice = 25,
                    SupplierId = 2001
                };



                Product product2 = new Product
                {
                    ProductName = "Capsule",
                    ListPrice = 33,
                    SupplierId = 2011
                };



                Product product3 = new Product
                {
                    ProductName = "Syringe",
                    ListPrice = 11,
                    SupplierId = 2021
                };



                OrderDetail orderDetail1 = new OrderDetail
                {
                    SaleOrderDetail = order1,
                    SaleProductDetail = product1,
                    OrderDate = DateTime.Now
                };
                OrderDetail orderDetail2 = new OrderDetail
                {
                    SaleOrderDetail = order2,
                    SaleProductDetail = product2,
                    OrderDate = DateTime.Now
                };
                OrderDetail orderDetail3 = new OrderDetail
                {
                    SaleOrderDetail = order3,
                    SaleProductDetail = product3,
                    OrderDate = DateTime.Now
                };
                context.Orders.Add(order1);
                context.Products.Add(product1);
                context.OrderDetails.Add(orderDetail1);
                context.Orders.Add(order2);
                context.Products.Add(product2);
                context.OrderDetails.Add(orderDetail2);
                context.Orders.Add(order2);
                context.Products.Add(product3);
                context.OrderDetails.Add(orderDetail3);
                context.SaveChanges();

                //Order when a product is sold
                IQueryable<OrderDetail> soldproductorders = context.OrderDetails
                .Include(c => c.SaleProductDetail).Where(c => c.SaleProductDetail == product1);



                //List of orders when a product is sold
                List<OrderDetail> SelectedSoldProductOrders = soldproductorders.ToList();


                //Orders list when a product is sold maximun
                OrderDetail maxsoldproductorders = context.OrderDetails
                .Include(c => c.SaleProductDetail).Where(c => c.SaleProductDetail == product1).OrderByDescending(x => x.Quantity).FirstOrDefault();


            }
        }
    }
}