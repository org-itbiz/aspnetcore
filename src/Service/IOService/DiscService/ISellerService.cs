using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace IOService.DiscService
{
    public interface ISellerService
    {
        Int32 GetSellerCount();
        Task<IPagedList<Seller>> GetList();
        Task<Seller> GetSellerById(Int32 nId);
    }
}
