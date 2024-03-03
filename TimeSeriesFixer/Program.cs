global using OfficeOpenXml;
using TimeSeriesCommon;
using TimeSeriesFiller.TimeSeriesFiller;

namespace TimeSeriesFixer;

public static class Program
{
    public static async Task Main()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        Console.Write("Input filepath: ");
        string inputFile = Console.ReadLine();
        Console.Write("Has you file got headers? (y/n): ");
        char headersRespond = Console.ReadKey().KeyChar;
        var timeSeries = await TimeSeriesLoader.LoadTimeSeries(new FileInfo(inputFile), headersRespond == 'y');
        ITimeSeriesFiller tsf = new SimpleTimeSeriesFixer();
        TimeSeriesIterator tsi = new(tsf);
        tsi.GoThroughTimeSeries(timeSeries);
        await TimeSeriesLoader.SaveTimeSeries(timeSeries, true);
    }
    
}