using CreditLineAPI.Enums;

namespace CreditLineAPI.Models
{
    public class CreditLineApplication
    {
        public string Id { get; set; }
        public string FoundingType { get; set; }
        public float CashBalance { get; set; }
        public float MonthlyRevenue { get; set; }
        public float RequestedCreditLine { get; set; }
        public DateTime RequestedDate { get; set; }
        public string Status { get; set; }
        public int Retries { get; set; }
        public bool IsAccepted
        {
            get
            {
                return this.Status == ApplicationStatus.Accepted.ToString();
            }
        }
        public bool HasRetriesLeft
        {
            get
            {
                return this.Retries < 3;
            }
        }

        public CreditLineApplication()
        {
            Id = String.Empty;
            Retries = 0;
            FoundingType = String.Empty;
            Status = String.Empty;
        }

        public object CreditLineResume()
        {
            return new
            {
                CreditLine = this.RequestedCreditLine,
                Status = this.Status
            };
        }

        public void UpdateValues(CreditLineApplication updatedApplication)
        {
            this.FoundingType = updatedApplication.FoundingType;
            this.CashBalance = updatedApplication.CashBalance;
            this.MonthlyRevenue = updatedApplication.MonthlyRevenue;
            this.RequestedCreditLine = updatedApplication.RequestedCreditLine;
            this.RequestedDate = updatedApplication.RequestedDate;
        }
    }
}
