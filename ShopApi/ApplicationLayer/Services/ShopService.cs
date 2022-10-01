using ApplicationLayer.Interfaces;
using DomainLayer.Entities;
using DomainLayer.Interfaces;

namespace ApplicationLayer.Services
{
    public class ShopService : IShopService
    {
        IUnitOfWork _unitOfWork;

        public ShopService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<List<ShopEntity>> GetShops(int page)
            => await _unitOfWork.ShopsRepository.GetAll(page);

        public async Task<ShopEntity> GetShopById(string id)
            => await _unitOfWork.ShopsRepository.FindById(id);

        public async Task<ShopEntity> Create(ShopEntity shop)
            => await _unitOfWork.InsertShop(shop);

        public async Task<bool> Update(ShopEntity shop)
            => await _unitOfWork.UpdateShop(shop);

        public async Task<bool> Delete(string id)
            => await _unitOfWork.DeleteShop(id);
    }
}
