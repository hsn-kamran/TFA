namespace TFA.Storage;

internal interface IMomentProvider
{
    DateTimeOffset Now { get; }
}
