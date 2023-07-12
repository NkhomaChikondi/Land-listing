using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Land_listing.Services
{
    public interface IdataLand<T>
    {
        Task<bool> AddlandAsync(T item);
        Task<bool> UpdatelandAsync(T item);
        Task<bool> DeletelandAsync(int id);
        Task<T> GetlandAsync(int id);
        Task<IEnumerable<T>> GetlandAsync(bool forceRefresh = false);
    }
}
