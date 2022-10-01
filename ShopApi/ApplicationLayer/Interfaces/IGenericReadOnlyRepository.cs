namespace ApplicationLayer.Interfaces
{
    public interface IGenericReadOnlyRepository<T> 
    {
        Task<List<T>> GetAll(int page);
        Task<T> FindById(string id);
    }
}
