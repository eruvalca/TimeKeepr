using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeepr.UI.Common
{
    public interface ILocalSecureStorage
    {
        Task<string> GetAsync(string item);
        Task<bool> RemoveAsync(string item);

        Task<bool> SetAsync(string name, string value);
    }
}
