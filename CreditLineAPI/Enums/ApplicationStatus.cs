using CreditLineAPI.Enums.Utils;

namespace CreditLineAPI.Enums
{
    public enum ApplicationStatus
    {
        [StringValue("Accepted")]
        Accepted,
        [StringValue("Rejected")]
        Rejected
    }
}
