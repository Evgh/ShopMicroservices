using ApplicationLayer.Interfaces;
using AutoMapper;
using InfrastuctureLayer.Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InfrastuctureLayer.Data
{
    internal class GenericRepository<DomainEntity, Model> : IGenericReadOnlyRepository<DomainEntity> where Model : BaseModel
    {
        private const int _pageSize = 50;

        private IMapper _mapper;
        private readonly IMongoCollection<Model> _itemsCollection;

        public GenericRepository(IMapper mapper,
                                 IMongoCollection<Model> itemsCollection)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _itemsCollection = itemsCollection ?? throw new ArgumentNullException(nameof(itemsCollection));
        }

        public async Task<List<DomainEntity>> GetAll(int page = 0)
        {
            List<Model> data = 
                await _itemsCollection.Find(new BsonDocument())
                .Skip(_pageSize * page)
                .Limit(_pageSize)
                .ToListAsync();

            return data.Select(element => _mapper.Map<DomainEntity>(element)).ToList();
        }

        public async Task<DomainEntity> FindById(string id)
        {
            FilterDefinition<Model> filter = Builders<Model>.Filter.Eq("Id", id);
            Model model = (await _itemsCollection.FindAsync(filter)).FirstOrDefault();

            return _mapper.Map<DomainEntity>(model);
        }

        public async Task<DomainEntity> Insert(Model item)
        {
            await _itemsCollection.InsertOneAsync(item);
            return _mapper.Map<DomainEntity>(item);
        }

        public async Task<DomainEntity> Update(Model item)
        {
            FilterDefinition<Model> filter = Builders<Model>.Filter.Eq("Id", item.Id);
            ReplaceOneResult replaceOneResult = await _itemsCollection.ReplaceOneAsync(filter, item);

            if (replaceOneResult.IsAcknowledged && replaceOneResult.IsModifiedCountAvailable && replaceOneResult.ModifiedCount > 0)
                return _mapper.Map<DomainEntity>(item);
            else
                return default(DomainEntity);
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Model> filter = Builders<Model>.Filter.Eq("Id", id);
            DeleteResult deleteResult = await _itemsCollection.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
