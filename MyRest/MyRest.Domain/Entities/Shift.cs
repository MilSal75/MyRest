using System;
using MyRest.Domain.Base;
using MyRest.Domain.ValueObjects;

namespace MyRest.Domain.Entities;

public class Shift : Entity<Guid>
{
    
    public Manager Manager { get; } = default!;

    
    public DateTime ShiftDate { get; private set; }
    public ShiftDuration Duration { get; private set; } = default!;

    
    protected Shift() { }

    
    public Shift(Manager manager, DateTime date, ShiftDuration duration)
        : this(Guid.NewGuid(), manager, date, duration) { }

    
    protected Shift(Guid id, Manager manager, DateTime date, ShiftDuration duration)
        : base(id)
    {
        Manager = manager ?? throw new ArgumentNullException(nameof(manager));
        ShiftDate = date;
        Duration = duration ?? throw new ArgumentNullException(nameof(duration));
    }

    
    public void Reschedule(DateTime newDate, ShiftDuration newDuration)
    {
        ShiftDate = newDate;
        Duration = newDuration ?? throw new ArgumentNullException(nameof(newDuration));
    }
}