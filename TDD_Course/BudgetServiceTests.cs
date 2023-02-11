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

            DateTime startTime = new DateTime(2023, 3, 1);
            DateTime endTime = new DateTime(2023, 3, 31);

            Assert.AreEqual(31000.00, budgetService.Query(startTime, endTime));
        }

        [Test]
        public void test_one_month_no_budget_but_other_month_have_budget()
        {
            budgetRepo.getAll().Returns(new List<Budget>
            {
                new Budget
                {
                    YearMonth = "202304",
                    Amount = 100
                }
            });

            DateTime startTime = new DateTime(2023, 3, 1);
            DateTime endTime = new DateTime(2023, 3, 31);

            Assert.AreEqual(0.00, budgetService.Query(startTime, endTime));
        }
        
        [Test]
        public void test_one_month_no_budget()
        {
            budgetRepo.getAll().Returns(new List<Budget>
            {
                new Budget
                {
                    YearMonth = "202304",
                    Amount = 0
                }
            });

            DateTime startTime = new DateTime(2023, 3, 1);
            DateTime endTime = new DateTime(2023, 3, 31);

            Assert.AreEqual(0.00, budgetService.Query(startTime, endTime));
        }
        
        [Test]
        public void test_cross_month()
        {
            budgetRepo.getAll().Returns(new List<Budget>
            {
                new Budget
                {
                    YearMonth = "202303",
                    Amount = 31000
                },
                new Budget
                {
                    YearMonth = "202304",
                    Amount = 30
                }
            });

            DateTime startTime = new DateTime(2023, 3, 30);
            DateTime endTime = new DateTime(2023, 4, 1);

            Assert.AreEqual(2001.00, budgetService.Query(startTime, endTime));
        }

        [Test]
        public void test_cross_three_month()
        {
            budgetRepo.getAll().Returns(new List<Budget>
            {
                new Budget
                {
                    YearMonth = "202303",
                    Amount = 31000
                },
                new Budget
                {
                    YearMonth = "202304",
                    Amount = 30000
                },
                new Budget
                {
                    YearMonth = "202305",
                    Amount = 0
                },
                new Budget
                {
                    YearMonth = "202306",
                    Amount = 30000
                }
            });

            DateTime startTime = new DateTime(2023, 3, 30);
            DateTime endTime = new DateTime(2023, 6, 5);

            Assert.AreEqual(37000.00, budgetService.Query(startTime, endTime));
        }
        
        [Test]
        public void test_cross_three_month_reverse()
        {
            budgetRepo.getAll().Returns(new List<Budget>
            {
                new Budget
                {
                    YearMonth = "202306",
                    Amount = 30
                },
                new Budget
                {
                    YearMonth = "202305",
                    Amount = 31000
                },
                new Budget
                {
                    YearMonth = "202304",
                    Amount = 0
                },
                new Budget
                {
                    YearMonth = "202303",
                    Amount = 31000
                }
            });

            DateTime startTime = new DateTime(2023, 3, 30);
            DateTime endTime = new DateTime(2023, 6, 5);

            Assert.AreEqual(2000 + 31000 + 5.00, budgetService.Query(startTime, endTime));
        }
    }
}