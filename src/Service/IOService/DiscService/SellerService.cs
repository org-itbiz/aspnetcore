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

        public async Task<Seller> GetSellerById(Int32 nId)
        {
            return await _unitOfWork.GetRepository<Seller>().GetFirstOrDefaultAsync(predicate: opt => opt.Id == nId);
        }

        public virtual bool ModifySeller(Seller seller)
        {
            try
            {
                var repo = _unitOfWork.GetRepository<Seller>();
               
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
