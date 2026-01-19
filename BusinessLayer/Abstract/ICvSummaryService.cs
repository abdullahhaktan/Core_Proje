using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ICvSummaryService
    {
        Task<string> GetSummaryAsync(string cvText);
    }
}
