global using OfficeOpenXml;
using TimeSeriesCommon;
using TimeSeriesFiller.TimeSeriesFiller;

namespace TimeSeriesFixer;

public static class Program
{
    public static async Task Main()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var x = 2;
        Console.Write("Input filepath: ");
        string inputFilePath = Console.ReadLine().Replace("\"", "");
        FileInfo inputFile = new FileInfo(inputFilePath);
        switch (x)
        {
            case 1:
                await FillMissingRows(inputFile);
                break;
            case 2:
                await CountCountries(inputFile);
                break;
            default:
                DefaultCase();
                break;
        }
    }

    private static void DefaultCase()
    {
        Console.WriteLine("You chose wrong option");
    }
    public static async Task FillMissingRows(FileInfo inputFile)
    {
        Console.Write("Has you file got headers? (y/n): ");
        char headersRespond = Console.ReadKey().KeyChar;
        var timeSeries = await TimeSeriesLoader.LoadTimeSeries(inputFile, headersRespond == 'y');
        ITimeSeriesFiller tsf = new SimpleTimeSeriesFixer();
        TimeSeriesIterator tsi = new(tsf);
        tsi.GoThroughTimeSeries(timeSeries);
        await TimeSeriesLoader.SaveTimeSeries(timeSeries, true);
    }
    public static async Task CountCountries(FileInfo inputFile)
    {
        CountryCounter countryCounter = new(inputFile);
        foreach (var country in await countryCounter.CountCountries())
        {
            Console.WriteLine(country);
        }
    }
}