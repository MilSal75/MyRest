#nullable disable
using System;

namespace MyRest.Domain.Entities
{
    public class Shift
    {
        public DateTime ShiftDate { get; set; }
        public decimal Hours { get; set; }
        public bool IsDone { get; set; }

        public Shift()
        {
        }

        public Shift(DateTime shiftDate, decimal hours)
        {
            ShiftDate = shiftDate;
            Hours = hours;
            IsDone = false;
        }

        public void MarkDone() => IsDone = true;

        public override string ToString() => $"{ShiftDate.ToShortDateString()} {Hours}ч {(IsDone ? "V" : "O")}";
    }
}