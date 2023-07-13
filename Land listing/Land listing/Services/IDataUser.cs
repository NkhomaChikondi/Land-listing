using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Land_listing.Services
{
    public interface IDataUser<T>
    {
        Task<bool> AddUserAsync(T item);
        Task<bool> UpdateUserAsync(T item);
        Task<bool> DeleteUserAsync(int id);
        Task<T> GetUserAsync(int id);
        Task<IEnumerable<T>> GetUsersAsync(bool forceRefresh = false);
    }
}
