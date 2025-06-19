using System;
using System.Collections.Generic;
using System.Linq;

namespace Deliverr.Models;

public partial class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public List<Product> Products { get; set; }
    public List<DeliveryState> DeliveryStates { get; set; }
    public string DeliveryStatus { get; set; }

}
