using System.Collections.Concurrent;
using ValetAPI.Filters;
using ValetAPI.Models;

namespace ValetAPI.Services;

public class RecurringSitting
{
    // Title, Areas, Type, Capacity, Start, End/Duration?, Rrule, GroupId
    // Rrule = { freq, interval, byWeekDay, dtStart, until, count, 
    //  freq= day,week,month
    //  interval = int
    //  byWeekDay = [SU,MO,TU,WE,TH,FR,SA]
    //  until = dateTime
    //  count = int
    
    public int Id { get; set; }
    public Guid? GroupId { get; set; } // GUID
    
    public List<Area> Areas { get; set; } = new(); // Areas
    
    public string Title { get; set; } // 
    public int Capacity { get; set; } // 
    public string Type { get; set; } // Breakfast/Lunch/Dinner/Special

    public DateTime StartTime { get; set; } // 
    public int Duration { get; set; } // 

    public List<Reservation> Reservations { get; set; } = new();
    
    
}

public abstract class Rrule
{
    public Frequency Freq { get; set; }

    public int Interval { get; set; }
    public string[] WeekDay { get; set; }
    public DateTime Until { get; set; }
    public int Count { get; set; }

    public Rrule(string freq, int interval, string[] weekDays, DateTime until)
    {
        if (!Enum.TryParse(freq, out Frequency frequency))
            throw new HttpResponseException(400, "Invalid frequency");
        Freq = frequency;
        Interval = interval;
        Until = until;
        WeekDay = weekDays;
    }

    // freq, interval, byWeekDay, dtStart, until, count, 
    public Rrule SetCount(DateTime start, DateTime end)
    {
        var span = end - start;
        Count = Freq switch
        {
            Frequency.day => span.Days / Interval,
            Frequency.week => span.Days / Interval,
            Frequency.month => span.Days / Interval,
            _ => throw new HttpResponseException(400, "Invalid frequency")
        };
        return this;
    }

    public RecurringSitting[] Create()
    {
        var groupId = new Guid();
        var sittings = new ConcurrentBag<RecurringSitting>();
        // var sittings = new List<SittingEntity>();
        Parallel.For(0,Count, count => sittings.Add(new RecurringSitting
        {
            Title = "",
            Type = "",
            GroupId = groupId,
            Capacity = 1,
            StartTime = new DateTime(),
            Duration = 90,
            Areas = new List<Area>()

        }));
        return sittings.ToArray();
    }
}

public enum Frequency
{
    day, week, month
}

public enum WeekDay
{
    SU,MO,TU,WE,TH,FR,SA
}