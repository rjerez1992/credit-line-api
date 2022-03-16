using CreditLineAPI.Enums;
using CreditLineAPI.Models;
using CreditLineAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CreditLineAPI.Tests
{
    public class CreditLineApplicationServiceTest
    {
        CreditLineApplicationService _creditLineApplicationService;
        public CreditLineApplicationServiceTest() { 
            _creditLineApplicationService = new CreditLineApplicationService();
        }

        [Fact]
        public void GivenNothingShouldReturnUUID() {
            string uuid = _creditLineApplicationService.GetNewApplicationId();
            Assert.NotNull(uuid);
            Assert.NotEmpty(uuid);
        }

        [Fact]
        public void GivenMultipleUUIDShouldBeDifferent()
        {
            string uuidA = _creditLineApplicationService.GetNewApplicationId();
            string uuidB = _creditLineApplicationService.GetNewApplicationId();
            Assert.NotEqual(uuidA, uuidB);
        }

        [Fact]
        public void GivenCorrectValuesApplicationShouldBeAccepted() {
            CreditLineApplication application = CreateApplication(FoundingType.SME.ToString(), 435.0f, 4235.0f);
            application.RequestedCreditLine = (application.MonthlyRevenue / 5) - 1;
            application = _creditLineApplicationService.Validate(application);
            Assert.True(application.IsAccepted);

            application = CreateApplication(FoundingType.Startup.ToString(), 310.0f, 5050.0f);
            application.RequestedCreditLine = Math.Max((application.CashBalance / 3) - 1, (application.MonthlyRevenue / 5) - 1);
            application = _creditLineApplicationService.Validate(application);
            Assert.True(application.IsAccepted);

            application = CreateApplication(FoundingType.Startup.ToString(), 7250.0f, 350.0f);
            application.RequestedCreditLine = Math.Max((application.CashBalance / 3) - 1, (application.MonthlyRevenue / 5) - 1);
            application = _creditLineApplicationService.Validate(application);
            Assert.True(application.IsAccepted);
        }

        [Fact]
        public void GivenInvalidValuesApplicationShouldBeRejected()
        {
            CreditLineApplication application = CreateApplication(FoundingType.SME.ToString(), 856.0f, 7423.0f);
            application.RequestedCreditLine = (application.MonthlyRevenue / 5) + 1;
            application = _creditLineApplicationService.Validate(application);
            Assert.False(application.IsAccepted);

            application = CreateApplication(FoundingType.Startup.ToString(), 551.0f, 3756.0f);
            application.RequestedCreditLine = Math.Max((application.CashBalance / 3) + 1, (application.MonthlyRevenue / 5) + 1);
            application = _creditLineApplicationService.Validate(application);
            Assert.False(application.IsAccepted);

            application = CreateApplication(FoundingType.Startup.ToString(), 6650.0f, 1220.0f);
            application.RequestedCreditLine = Math.Max((application.CashBalance / 3) + 1, (application.MonthlyRevenue / 5) + 1);
            application = _creditLineApplicationService.Validate(application);
            Assert.False(application.IsAccepted);
        }

        [Fact]
        public void GivenRetriesOverThresholdShouldBeRejected()
        {
            CreditLineApplication application = CreateApplication(FoundingType.SME.ToString(), 435.0f, 4235.0f);
            application.RequestedCreditLine = application.MonthlyRevenue;
            application = _creditLineApplicationService.Validate(application);
            Assert.False(application.IsAccepted);

            application = _creditLineApplicationService.Validate(application);
            Assert.False(application.IsAccepted);

            application = _creditLineApplicationService.Validate(application);
            Assert.False(application.IsAccepted);

            application.RequestedCreditLine = 1;
            Assert.False(application.IsAccepted);
        }

        [Fact]
        public void GivenInvalidApplicationTypeShouldBeRejected()
        {
            CreditLineApplication application = CreateApplication("InvalidType", 435.0f, 4235.0f);
            application.RequestedCreditLine = (application.MonthlyRevenue / 5) - 1;
            application = _creditLineApplicationService.Validate(application);
            Assert.False(application.IsAccepted);
        }

        [Fact]
        public void GivenApplicationAlreadyAcceptedReturnsSameValues()
        {
            CreditLineApplication application = CreateApplication(FoundingType.SME.ToString(), 435.0f, 4235.0f);
            application.RequestedCreditLine = (application.MonthlyRevenue / 5) - 1;
            application = _creditLineApplicationService.Validate(application);
            Assert.True(application.IsAccepted);

            CreditLineApplication clonedApplication = CreateApplication(FoundingType.SME.ToString(), 535.0f, 8240.0f);
            clonedApplication.Id = application.Id;
            clonedApplication = _creditLineApplicationService.Validate(clonedApplication);
            Assert.Equal(application, clonedApplication);
        }

        private CreditLineApplication CreateApplication(string foundingType, float cashBalance, float monthlyRevenue) {
            CreditLineApplication application = new CreditLineApplication();
            application.Id = _creditLineApplicationService.GetNewApplicationId();
            application.FoundingType = foundingType;
            application.CashBalance = cashBalance;
            application.MonthlyRevenue = monthlyRevenue;
            application.RequestedDate = DateTime.Now;
            return application;
        }
    }
}
