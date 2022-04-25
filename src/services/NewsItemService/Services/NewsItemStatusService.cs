using NewsItemService.Data;
using NewsItemService.Interfaces;

namespace NewsItemService.Services
{
    public class NewsItemStatusService
    {
        
        public NewsItemStatusService()
        {
            
        }

        public Dictionary<bool, string> CheckNewsItemValue(int newsItemID)
        {
            if (newsItemID < 0)
            {
                return new Dictionary<bool, string>() { { false, "Fout" } };
            }
            if (newsItemID.GetType() == typeof(int))
            {

            }
            return new Dictionary<bool, string>() { { true, "goed" } };
        }
    }
}
