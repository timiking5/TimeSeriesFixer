namespace TimeSeriesFiller.TimeSeriesFiller;

public class SimpleTimeSeriesFixer : ITimeSeriesFiller
{
    public void FillLongTerm(List<TimeSeriesEntry> entries, int index)
    {
        var leftEntry = entries[index];
        var rightEntry = entries[index + 1];
        int dayDifference = (int)(rightEntry.Date - leftEntry.Date).TotalDays - 1;

        var coef1 = (rightEntry.Value - leftEntry.Value) / dayDifference;
        var coef0 = leftEntry.Value;

        var variance = CountVariance(entries, index);
        var sign = 1;
        for (int i = 1; i < dayDifference + 1; i++)
        {
            var value = coef0 + coef1 * i + (decimal)variance * sign;
            sign *= -1;
            var date = leftEntry.Date.AddDays(i);
            entries.Insert(index + i, new(date, value));
        }
    }

    public void FillShortTerm(List<TimeSeriesEntry> entries, int index)
    {
        var leftEntry = entries[index];
        var rightEntry = entries[index + 1];

        int daysToFill= (int)(rightEntry.Date - leftEntry.Date).TotalDays - 1;
        int leftHalf = (daysToFill / 2) + daysToFill % 2;
        int rightHalf = daysToFill / 2;

        for (int j = 0; j < leftHalf; j++)
        {
            var value = leftEntry.Value;
            var date = leftEntry.Date.AddDays(j + 1);
            entries.Insert(index + j + 1, new(date, value));
        }
        for (int j = 0; j < rightHalf; j++)
        {
            var value = rightEntry.Value;
            var date = rightEntry.Date.AddDays(-j - 1);
            entries.Insert(index + 1 + j + leftHalf, new(date, value));
        }
    }
    private double CountVariance(List<TimeSeriesEntry> ts, int index)
    {
        int cnt = 0;
        double value = 0;
        var mean = CountMean(ts, index);
        for (int i = 0; i < 30 && index - i >= 0; i++)
        {
            value += Math.Pow(mean - (double)ts[index - i].Value, 2);
            cnt++;
        }
        int n = ts.Count;
        for (int i = 0; i < 30 && index + i <= n; i++)
        {
            value += Math.Pow(mean - (double)ts[index - i].Value, 2);
            cnt++;
        }
        return Math.Sqrt(value / (cnt - 1));
    }

    private double CountMean(List<TimeSeriesEntry> ts, int index)
    {
        int cnt = 0;
        decimal sum = 0;
        for (int i = 0; i < 30 && index - i >= 0; i++)
        {
            sum += ts[index - i].Value;
            cnt++;
        }
        int n = ts.Count;
        for (int i = 0; i < 30 && index + i < n; i++)
        {
            sum += ts[index + i].Value;
            cnt++;
        }
        return (double)sum / cnt;
    }
}
