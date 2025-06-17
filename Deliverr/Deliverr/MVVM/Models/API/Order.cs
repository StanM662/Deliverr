using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deliverr.Models;
namespace Deliverr.Models;

public partial class Order
{
    public int id;
    public string orderDate;
    public int customerId;
    public Customer customer;
    public List<Product> products;
}
