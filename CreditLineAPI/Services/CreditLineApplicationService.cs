using CreditLineAPI.Enums;
using CreditLineAPI.Enums.Utils;
using CreditLineAPI.Models;

namespace CreditLineAPI.Services
{
    public class CreditLineApplicationService : ICreditLineApplicationService
    {
        private Dictionary<string, CreditLineApplication> _applications;

        public CreditLineApplicationService()
        {
            _applications = new Dictionary<string, CreditLineApplication>();
        }

        public string GetNewApplicationId()
        {
            return Guid.NewGuid().ToString();
        }

        public CreditLineApplication Validate(CreditLineApplication application)
        {
            CreditLineApplication previousApplication = FindPrevious(application.Id);

            if (previousApplication == null)
            {
                if (IsApplicationValid(application))
                    AcceptApplication(application);
                else
                    RejectApplication(application);
                _applications.Add(application.Id, application);
                return application;
            }

            if (previousApplication.IsAccepted || !previousApplication.HasRetriesLeft)
                return previousApplication;

            previousApplication.UpdateValues(application);

            if (IsApplicationValid(previousApplication))
            {
                AcceptApplication(previousApplication);
                return previousApplication;
            }

            RejectApplication(previousApplication);
            return previousApplication;
        }

        public CreditLineApplication FindPrevious(string applicationId)
        {
            if (_applications.ContainsKey(applicationId))
                return _applications[applicationId];
            return null;
        }

        private void AcceptApplication(CreditLineApplication application)
        {
            application.Status = Enums.ApplicationStatus.Accepted.GetStringValue();
        }

        private bool IsApplicationValid(CreditLineApplication application)
        {
            if (application.FoundingType == FoundingType.SME.GetStringValue())
            {
                float recommended = application.MonthlyRevenue / 5;
                return recommended > application.RequestedCreditLine;
            }
            else if (application.FoundingType == FoundingType.Startup.GetStringValue())
            {
                float cashBalanceRate = application.CashBalance / 3;
                float monthlyRevenueRate = application.MonthlyRevenue / 5;
                float recommended = Math.Max(cashBalanceRate, monthlyRevenueRate);
                return recommended > application.RequestedCreditLine;
            }
            return false;
        }

        private void RejectApplication(CreditLineApplication application)
        {
            application.Status = Enums.ApplicationStatus.Rejected.GetStringValue();
            application.Retries++;
        }
    }
}
