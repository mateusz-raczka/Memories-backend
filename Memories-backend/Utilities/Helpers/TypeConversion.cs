namespace Memories_backend.Utilities.Helpers
{
    public static class TypeConversion
    {
        public static Guid ConvertStringToGuid(string stringValue)
        {
            if (!Guid.TryParse(stringValue, out Guid guidValue))
                throw new FormatException("Invalid format - failed to convert string to GUID");

            return guidValue;
        }
    }
}
