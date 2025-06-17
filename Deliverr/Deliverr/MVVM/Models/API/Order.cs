using System;
using System.Collections.Generic;

namespace Deliverr.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public List<Product> Products { get; set; }
    public List<DeliveryState> DeliveryStates { get; set; }
}
