namespace TFA.Domain.Exceptions;

internal static class ValidationErrorCode
{
    public static string NotEmpty => "The item must be not empty";
    public static string TooLong => "The string is too long";
    public static string TooShort => "The string is too short"; 
}
