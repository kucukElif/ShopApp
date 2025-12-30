using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Bussiness.Abstract
{
    public interface IOrderService
    {
        void Create(Order entity);
        List<Order> GetAll(string userId);
    }
}
