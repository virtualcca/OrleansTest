using System.Threading.Tasks;
using OrleanInterface;
using Orleans;

namespace OrleanGrains
{
    public class Account : Grain, IAccount
    {
        public int Money { get; set; }

        public async Task TransferAsync(int amount)
        {
            await Task.Delay(1);//转账总要点时间把,延迟个1ms,同时为了避免假异步
            Money += amount;//实际转账
            
        }

        public Task<int> AssertAsync()
        {
            return Task.FromResult(Money);
        }
    }
}
