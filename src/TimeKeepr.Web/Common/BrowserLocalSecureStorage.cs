using Blazored.LocalStorage;
using TimeKeepr.UI.Common;

namespace TimeKeepr.Web.Common
{
    public class BrowserLocalSecureStorage : ILocalSecureStorage
    {
        private readonly ILocalStorageService _localStorage;

        public BrowserLocalSecureStorage(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<string> GetAsync(string item)
        {
            return await _localStorage.GetItemAsync<string>(item);
        }

        public async Task<bool> RemoveAsync(string item)
        {
            try
            {
                await _localStorage.RemoveItemAsync(item);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SetAsync(string name, string value)
        {
            try
            {
                await _localStorage.SetItemAsStringAsync(name, value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
