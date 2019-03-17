using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace OrleanInterface
{
    public interface IAccount : IGrainWithIntegerKey
    {
        Task TransferAsync(int amount);

        Task<int> AssertAsync();
    }
}
