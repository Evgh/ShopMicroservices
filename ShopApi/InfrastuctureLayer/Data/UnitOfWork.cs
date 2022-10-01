using ApplicationLayer.Interfaces;
using AutoMapper;
using DomainLayer.Entities;
using InfrastuctureLayer.Data.Models;
using InfrastuctureLayer.Data.Parameters;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InfrastuctureLayer.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMapper _mapper;
        private readonly GenericRepository<ShopEntity, ShopModel> _shopsRepository;
        private readonly GenericRepository<GeoCoordinateEntity, GeoCoordinateModel> _locationsRepository;

        public IGenericReadOnlyRepository<ShopEntity> ShopsRepository => _shopsRepository;
        public IGenericReadOnlyRepository<GeoCoordinateEntity> LocationsRepository => _locationsRepository;         

        public UnitOfWork(IOptions<DatabaseSettings> databaseSettings, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            var shopsCollection = mongoDatabase.GetCollection<ShopModel>(databaseSettings.Value.ShopsCollectionName);
            var locationsCollection = mongoDatabase.GetCollection<GeoCoordinateModel>(databaseSettings.Value.LocationsCollectionName);

            _shopsRepository = new GenericRepository<ShopEntity, ShopModel>(mapper, shopsCollection);
            _locationsRepository = new GenericRepository<GeoCoordinateEntity, GeoCoordinateModel>(mapper, locationsCollection);
        }

        public async Task<ShopEntity> InsertShop(ShopEntity shop)
        {
            ShopModel shopToInsert = _mapper.Map<ShopModel>(shop);
            var insertedShop = await _shopsRepository.Insert(shopToInsert);

            if(insertedShop != null)
                await RegisterShopLocation(shopToInsert);

            return insertedShop;
        }

        public async Task<bool> UpdateShop(ShopEntity shopEntity)
        {
            ShopModel shopToUpdate = _mapper.Map<ShopModel>(shopEntity);
            ShopModel oldShop = _mapper.Map<ShopModel>(await _shopsRepository.FindById(shopToUpdate.Id));

            bool isShopExist = oldShop != null;
            
            if (isShopExist)
            {
                var updatedShop = await _shopsRepository.Update(shopToUpdate);

                if (updatedShop != null)
                {
                    await UnregisterShopLocation(oldShop);
                    await RegisterShopLocation(shopToUpdate);
                }

                return true;
            }

            return false;
        }

        public async Task<bool> DeleteShop(string id)
        {
            ShopModel shopToDelete = _mapper.Map<ShopModel>(await _shopsRepository.FindById(id));
            bool isShopExist = shopToDelete != null;

            if (isShopExist)
            {
                bool isSuccessfull = true;

                isSuccessfull &= await UnregisterShopLocation(shopToDelete);
                
                if(isSuccessfull)
                    isSuccessfull &= await _shopsRepository.Delete(id);

                return isSuccessfull;
            }

            return isShopExist;
        }

        #region Private Methods
        private async Task<bool> RegisterShopLocation(ShopModel associatedShop)
        {
            GeoCoordinateModel existingCoordinate = _mapper.Map<GeoCoordinateModel>(await _locationsRepository.FindById(associatedShop.LocationId));
            bool isLocationExist = existingCoordinate != null;

            if (isLocationExist)
            {
                existingCoordinate.Shops.Add(associatedShop);
                return await _locationsRepository.Update(existingCoordinate) != null;
            }
            else
            {
                GeoCoordinateModel newCoordinate = new()
                {
                    Location = associatedShop.Location,
                    Shops = new() { associatedShop }
                };

                return await _locationsRepository.Insert(newCoordinate) != null;
            }
        }

        private async Task<bool> UnregisterShopLocation(ShopModel oldShop)
        {
            GeoCoordinateModel existingCoordinate = _mapper.Map<GeoCoordinateModel>(await _locationsRepository.FindById(oldShop.LocationId));
            bool isLocationExist = existingCoordinate != null;

            if (isLocationExist)
            {
                ShopModel existingShop = existingCoordinate.Shops.Find(sh => sh.Id.Equals(oldShop.Id));

                if(existingShop != null)
                {
                    existingCoordinate.Shops.Remove(existingShop);

                    if (existingCoordinate.Shops.Count > 0)
                    {
                        return await _locationsRepository.Update(existingCoordinate) != null;
                    }
                    else
                    {
                        return await _locationsRepository.Delete(existingCoordinate.Id);
                    }
                }    
            }
            return isLocationExist;
        }
        #endregion
    }
}
