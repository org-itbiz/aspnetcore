using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace IOService.DiscService
{
    public class SellerService : ISellerService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SellerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Int32 GetSellerCount()
        {
            return _unitOfWork.GetRepository<Seller>().Count();
        }

        public async Task<IPagedList<Seller>> GetList()
        {
            return await _unitOfWork.GetRepository<Seller>().GetPagedListAsync();
        }
    }
}
