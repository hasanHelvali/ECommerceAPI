﻿using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Repositories
{
    public interface ICustomerReadRepository:IReadRepository<Customer>
    {
        //ICustomerReadRepository artık customer a ozel IReadRepository icindeki yapılari barındırır.
    }
}
