using System;

/// <summary>
/// Represent a calendar date in a format that can be set in the inspector.
/// </summary>
[Serializable]
public struct Date : IComparable<Date>
{
    public int Day;
    public int Month;
    public int Year;

    public Date(int day, int month, int year)
    {
        Day = day;
        Month = month;
        Year = year;
    }

    public Date(double day, double month, double year)
    {
        Day = (int)day;
        Month = (int)month;
        Year = (int)year;
    }

    public readonly int CompareTo(Date otherDate)
    {
        int thisDateInDays = this.ToDays();
        int otherDateInDays = otherDate.ToDays();

        return thisDateInDays.CompareTo(otherDateInDays);
    }

    public static bool operator <(Date dateA, Date dateB)
    {
        return dateA.CompareTo(dateB) < 0;
    }

    public static bool operator >(Date dateA, Date dateB)
    {
        return dateA.CompareTo(dateB) > 0;
    }

    public static bool operator ==(Date dateA, Date dateB)
    {
        return dateA.Equals(dateB);
    }

    public static bool operator !=(Date dateA, Date dateB)
    {
        return !dateA.Equals(dateB);
    }

    public static bool operator >=(Date dateA, Date dateB)
    {
        return dateA.CompareTo(dateB) > 0 || dateA.Equals(dateB);
    }

    public static bool operator <=(Date dateA, Date dateB)
    {
        return dateA.CompareTo(dateB) < 0 || dateA.Equals(dateB);
    }

    public override readonly bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override readonly int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override readonly string ToString()
    {
        return $"{Day}-{Month}-{Year}";
    }
}
