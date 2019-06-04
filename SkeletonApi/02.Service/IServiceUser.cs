using SkeletonApi.Entity;

namespace SkeletonApi.Service
{
    public interface IServiceUser
    {
        int Insert(EntityUser user);
        int Update(EntityUser user);
        int Remove(EntityUser user);
        EntityUser Find(EntityUser user);
        EntityUser Authenticate(string email, string password);
    }
}
