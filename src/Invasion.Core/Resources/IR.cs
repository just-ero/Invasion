namespace Invasion.Core.Resources;

internal static class IR
{
    public const string ProcessMustNotBeNull = "Process must not be null.";
    public const string ProcessMustNotHaveExited = "Process must not have exited.";
    public const string ProcessExitedArchitecture = "Cannot infer the architecture of an exited process.";
    public const string ProcessExitedModules = "Cannot enumerate the modules of an exited process.";
    public const string ProcessExitedPages = "Cannot enumerate the memory pages of an exited process.";
    public const string ProcessExitedRead = "Cannot read the memory of an exited process.";
    public const string ProcessExitedWrite = "Cannot write the memory of an exited process.";

    public const string SupportedOperatingSystems = "Supported operating systems include Windows, Linux, and MacOS.";
}
