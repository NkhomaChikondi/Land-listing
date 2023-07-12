using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Land_listing.Services
{
    public interface IdataUserland<T>
    {
        Task<bool> AddUserlandAsync(T item);
        Task<bool> UpdateUserlandAsync(T item);
        Task<bool> DeleteUserlandAsync(int id);
        Task<T> GetUserlandAsync(int id);
        Task<IEnumerable<T>> GetUserlandAsync(bool forceRefresh = false);
    }
}
