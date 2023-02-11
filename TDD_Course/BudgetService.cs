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
            var result = budgetRepo.getAll();
            var range = endTime - startTime;
            var startBudget = result.Find(x => x.YearMonth == $"{startTime:yyyyMM}");
            var endBudget = result.Find(x => x.YearMonth == $"{startTime=endTime:yyyyMM}");

            if (startBudget != null)
            {
                var start = startBudget.Amount / DateTime.DaysInMonth(startTime.Year, startTime.Month) *
                            (range.Days + 1);
                if (endBudget != null && startTime.Month != endTime.Month)
                {
                    var end = endBudget.Amount/DateTime.DaysInMonth(endTime.Year, endTime.Month)* (range.Days + 1);
                    return end + start;

                }
                return start;
            }

            return 0;
            
        }
    }
}