namespace TimeSeriesCommon;

public class TimeSeriesEntry
{
    public DateTime Date { get; set; }
    public decimal Value { get; set; }
    public TimeSeriesEntry(DateTime date, decimal value)
    {
        Date = date;
        Value = value;
    }
    public TimeSeriesEntry(string date, decimal value)
    {
        Date = DateTime.Parse(date);
        Value = value;
    }
}
