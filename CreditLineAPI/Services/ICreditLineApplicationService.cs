using CreditLineAPI.Enums;
using CreditLineAPI.Models;

namespace CreditLineAPI.Services
{
    public interface ICreditLineApplicationService
    {
        string GetNewApplicationId();
        CreditLineApplication Validate(CreditLineApplication application);
        CreditLineApplication FindPrevious(string applicationId);
    }
}
