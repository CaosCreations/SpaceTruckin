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

    public int CompareTo(Date otherDate)
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

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return $"{Day}/{Month}/{Year}";
    }
}
