using DomainLayer.Entities;

namespace DomainLayer.Interfaces
{
    public interface IShopService
    {
        public Task<List<ShopEntity>> GetShops(int page);
        public Task<ShopEntity> GetShopById(string id);
        public Task<ShopEntity> Create(ShopEntity shop);
        public Task<bool> Update(ShopEntity shop);
        public Task<bool> Delete(string id);
    }
}
