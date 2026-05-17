namespace FreeDataExportsCMD.Providers;

internal sealed class InputProvider : IInputProvider
{
    public async Task<string> ReadAllAsync(string? inputFile)
    {
        if (!string.IsNullOrWhiteSpace(inputFile))
        {
            return await File.ReadAllTextAsync(inputFile);
        }

        if (!Console.IsInputRedirected)
        {
            return string.Empty;
        }

        return await Console.In.ReadToEndAsync();
    }
}
