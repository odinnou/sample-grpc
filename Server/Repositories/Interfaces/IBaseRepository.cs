using System.Threading.Tasks;

namespace Server.Repositories.Interfaces
{
    public interface IBaseRepository
    {
        Task SaveChangesIgnoringNumberOfChanges();

        Task SaveChanges();
    }
}
