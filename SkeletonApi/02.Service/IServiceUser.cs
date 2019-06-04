using SkeletonApi.Entity;

namespace SkeletonApi.Service
{
    public interface IServiceUser
    {
        int Insert(User user);
        int Update(User user);
        int Remove(User user);
        User Find(User user);
    }
}
