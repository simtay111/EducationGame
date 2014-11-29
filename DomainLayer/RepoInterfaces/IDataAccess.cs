using DomainLayer.Entities;

namespace DomainLayer.RepoInterfaces
{
    public interface IDataAccess
    {
        void Save<T>(T entityToSave);
    }
}