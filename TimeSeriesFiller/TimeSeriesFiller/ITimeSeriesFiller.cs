global using TimeSeriesCommon;

namespace TimeSeriesFiller.TimeSeriesFiller;

public interface ITimeSeriesFiller
{
    public void FillShortTerm(List<TimeSeriesEntry> entries, int index);
    public void FillLongTerm(List<TimeSeriesEntry> entries, int index);
}