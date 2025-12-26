using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public class EfCoreOrderDal: EfCoreGenericRepository<Order,ShopContext>,IOrderDal
    {
        
    }
}
