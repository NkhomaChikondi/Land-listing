using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Land_listing.Services
{
    public interface INotification<T>
    {
        Task<bool> AddNotificationAsync(T item);
        Task<bool> UpdateNotificationAsync(T item);
        Task<bool> DeleteNotificationAsync(int id);
        Task<T> GetNotificationAsync(int id);
        Task<IEnumerable<T>> GetNotificationsAsync(bool forceRefresh = false);
    }
}
