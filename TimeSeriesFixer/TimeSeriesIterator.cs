using TimeSeriesCommon;
using TimeSeriesFiller.TimeSeriesFiller;

namespace TimeSeriesFixer;

public class TimeSeriesIterator
{
    private readonly ITimeSeriesFiller _timeSeriesFixer;

    public TimeSeriesIterator(ITimeSeriesFiller timeSeriesFixer)
    {
        _timeSeriesFixer = timeSeriesFixer;
    }
    public void GoThroughTimeSeries(List<TimeSeriesEntry> ts)
    {
        int i = 0;
        while (i < ts.Count - 1)
        {
            int dayDifference = (int)(ts[i + 1].Date - ts[i].Date).TotalDays;
            if (1 < dayDifference && dayDifference <= 5)
            {
                _timeSeriesFixer.FillShortTerm(ts, i);
            }
            if (dayDifference > 5)
            {
                _timeSeriesFixer.FillLongTerm(ts, i);
            }
            i += dayDifference;
        }
    }
}
