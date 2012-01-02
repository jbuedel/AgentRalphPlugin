// Match
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgentRalph.Tests.AstCompareTests.TestCases
{
    class LinqOuterJoin
    {
        private List<Order> Orders = new List<Order>() {
                                                    new Order {Key = 1, OrderNumber = "Order 1" },
                                                    new Order {Key = 1, OrderNumber = "Order 2" },
                                                    new Order {Key = 4, OrderNumber = "Order 3" },
                                                    new Order {Key = 4, OrderNumber = "Order 4" },
                                                    new Order {Key = 5, OrderNumber = "Order 5" },
                                                };

        private List<Customer> Customers = new List<Customer>() { 
                                                          new Customer {Key = 1, Name = "Gottshall" },
                                                          new Customer {Key = 2, Name = "Valdes" },
                                                          new Customer {Key = 3, Name = "Gauwain" },
                                                          new Customer {Key = 4, Name = "Deane" },
                                                          new Customer {Key = 5, Name = "Zeeman" } 
                                                      };

        public void Foo()
        {
            var q = from c in Customers
                    join o in Orders on c.Key equals o.Key into g
                    from o in g.DefaultIfEmpty()
                    select new { Name = c.Name, OrderNumber = o == null ? "(no orders)" : o.OrderNumber };
        }

        public void Bar()
        {
            var q = from c in Customers
                    join o in Orders on c.Key equals o.Key into g
                    from o in g.DefaultIfEmpty()
                    select new { Name = c.Name, OrderNumber = o == null ? "(no orders)" : o.OrderNumber };
        }
    }

    internal class Order
    {
        public int Key;
        public string OrderNumber;
    }

    internal class Customer
    {
        public int Key;
        public string Name;
    }
}
