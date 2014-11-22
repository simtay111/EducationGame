using DomainLayer.Entities;

namespace DomainLayer.RepoInterfaces
{
    public interface ISystemStateRepository
    {
        SystemState Get();
        void Save(SystemState state);
    }
}