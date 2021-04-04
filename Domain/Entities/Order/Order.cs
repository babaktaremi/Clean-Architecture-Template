using System;
using System.Collections.Generic;
using System.Text;
using Domain.Common;

namespace Domain.Entities.Order
{
   public class Order:BaseEntity
    {
        public string OrderName { get; set; }
    }
}
