using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeepr.UI.Common;

namespace TimeKeepr.NativeClients.Common
{
    public class NativeLocalSecureStorage : ILocalSecureStorage
    {
        public async Task<string> GetAsync(string item)
        {
            return await SecureStorage.Default.GetAsync(item);
        }

        public async Task<bool> RemoveAsync(string item)
        {
            return await Task.FromResult(SecureStorage.Default.Remove(item));
        }

        public async Task<bool> SetAsync(string name, string value)
        {
            try
            {
                await Task.FromResult(SecureStorage.Default.SetAsync(name, value));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
