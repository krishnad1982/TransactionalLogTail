using System.Threading.Tasks;

namespace Poc.LogTail.Core.Contracts
{
    public interface IStreamFeed
    {
        Task StartFeed();
    }
}
