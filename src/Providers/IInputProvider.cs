namespace FreeDataExportsCMD.Providers;

internal interface IInputProvider
{
    Task<string> ReadAllAsync(string? inputFile);
}
