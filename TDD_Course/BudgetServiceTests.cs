using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace TDD_Course
{
    [TestFixture]
    public class BudgetServiceTests
    {
        private IBudgetRepo budgetRepo;

        private BudgetService budgetService;

        [SetUp]
        public void Setup()
        {
            budgetRepo = Substitute.For<IBudgetRepo>();
            budgetService = new BudgetService(budgetRepo);
        }

        [Test]
        public void test_one_day()
        {
            budgetRepo.getAll().Returns(
                new List<Budget>()
                {
                    new Budget
                    {
                        YearMonth = "202302",
                        Amount = 2800
                    }
                });


            DateTime startTime = new DateTime(2023, 2, 2);
            DateTime endTime = new DateTime(2023, 2, 2);

            Assert.AreEqual(100.00, budgetService.Query(startTime, endTime));
        }

        [Test]
        public void test_one_month()
        {
            budgetRepo.getAll().Returns(new List<Budget>
            {
                new Budget
                {
                    YearMonth = "202303",
                    Amount = 31000
                }
            });

            DateTime startTime = new DateTime(2023, 2, 2);
            DateTime endTime = new DateTime(2023, 2, 2);

            Assert.AreEqual(31000.00, budgetService.Query(startTime, endTime));
        }
    }
}