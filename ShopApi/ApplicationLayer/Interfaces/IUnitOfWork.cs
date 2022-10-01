using DomainLayer.Entities;

namespace ApplicationLayer.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericReadOnlyRepository<ShopEntity> ShopsRepository { get; }
        IGenericReadOnlyRepository<GeoCoordinateEntity> LocationsRepository { get; }

        Task<ShopEntity> InsertShop(ShopEntity shopToInsert);
        Task<bool> UpdateShop(ShopEntity shopToUpdate);
        Task<bool> DeleteShop(string id);
    }
}
