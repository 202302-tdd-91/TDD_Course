using System;

namespace TDD_Course
{
    public class BudgetService
    {
        private IBudgetRepo budgetRepo;

        public BudgetService(IBudgetRepo budgetRepo)
        {
            this.budgetRepo = budgetRepo;
        }

        public double Query(DateTime startTime, DateTime endTime)
        {
            return 100.00;
        }
    }
}