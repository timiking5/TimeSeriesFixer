
namespace TimeSeriesFixer;

public class CountryCounter
{
    private readonly HashSet<int> allowedListsIndexes = new()
    {
        0, 1, 2, 4, 6, 7, 10, 11, 12, 13, 14
    };
    private readonly FileInfo _inputFile;
    public CountryCounter(FileInfo inputFile, HashSet<int> allowedIndexes)
    {
        _inputFile = inputFile;
        allowedListsIndexes = allowedIndexes;
    }
    public CountryCounter(FileInfo inputFile)
    {
        _inputFile = inputFile;
    }
    public async Task<HashSet<string>> CountCountries()
    {
        using var package = new ExcelPackage(_inputFile);
        await package.LoadAsync(_inputFile);
        var wb = package.Workbook;
        HashSet<string> result = ScanWorkSheet(wb.Worksheets[0]);
        foreach (var ws in wb.Worksheets)
        {
            if (allowedListsIndexes.Contains(ws.Index))
            {
                var buff = result.Intersect(ScanWorkSheet(ws)).ToHashSet();
                await Console.Out.WriteLineAsync($"Countries lost on {ws.Name}: {result.Count - buff.Count} and {buff.Contains("Russian Federation")}");
                result = buff;
            }
        }
        return result;
    }

    private HashSet<string> ScanWorkSheet(ExcelWorksheet ws)
    {
        HashSet<string> result = new();
        var col = 1;
        var row = 1;
        while (string.IsNullOrEmpty(ws.Cells[row, col].Value?.ToString()) == false || row < 5)
        {
            result.Add(ws.Cells[row, col].Value?.ToString());
            row++;
        }
        return result;
    }
}
