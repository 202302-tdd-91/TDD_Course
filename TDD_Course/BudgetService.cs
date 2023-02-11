using System;
using System.Collections.Generic;
using System.Threading;

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
            var startBudget = GetMonthBudget(startTime, result);
            var endBudget = GetMonthBudget(endTime, result);

            if (SameMonth(startTime, endTime))
            {
                var range = endTime - startTime;
                var start = startBudget.Amount / DateTime.DaysInMonth(startTime.Year, startTime.Month) * (range.Days + 1);
                return start;
            }
            else
            {
                var range = DateTime.DaysInMonth(startTime.Year, startTime.Month) - startTime.Day;
                var start = startBudget.Amount / DateTime.DaysInMonth(startTime.Year, startTime.Month) * (range + 1);
                var end = endBudget.Amount / DateTime.DaysInMonth(endTime.Year, endTime.Month) * endTime.Day;

                var currentTemp = startTime.AddMonths(1);
                var current = new DateTime(currentTemp.Year, currentTemp.Month, 1);
                var middleAmount = 0;

                while (current < new DateTime(endTime.Year, endTime.Month, 1))
                {
                    var budget = result.Find(x => x.YearMonth == current.ToString("yyyyMM"));
                    
                    middleAmount += budget?.Amount ?? 0;

                    current = current.AddMonths(1);
                }

                return end + start + middleAmount;
            }


            return 0;
        }

        private static bool SameMonth(DateTime startTime, DateTime endTime)
        {
            var s = startTime.ToString("yyyyMM");
            var e = endTime.ToString("yyyyMM");
            return s == e;
        }

        private static Budget GetMonthBudget(DateTime startTime, List<Budget> result)
        {
            return result.Find(x => x.YearMonth == $"{startTime:yyyyMM}");
        }
    }
}