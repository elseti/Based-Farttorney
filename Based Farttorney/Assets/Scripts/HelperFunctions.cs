using System;

namespace DefaultNamespace
{
    public static class HelperFunctions
    {
        // Extension method to enable Array.Slice in Unity
        public static T[] Slice<T>(this T[] source, int start, int length)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (start < 0 || start >= source.Length)
                throw new ArgumentOutOfRangeException(nameof(start));

            if (length < 0 || start + length > source.Length)
                throw new ArgumentOutOfRangeException(nameof(length));

            T[] slice = new T[length];
            Array.Copy(source, start, slice, 0, length);

            return slice;
        }
        
        public static string GetSubstring(string str, int startIndex, int length)
        {
            // Perform bounds checking to ensure valid indices
            if (startIndex < 0)
                startIndex = 0;
            
            if (startIndex >= str.Length)
                return string.Empty;

            // Adjust length if it goes beyond the end of the string
            if (startIndex + length > str.Length)
                length = str.Length - startIndex;

            return str.Substring(startIndex, length);
        }
        
        public static float ParseToFloat(string str)
        {
            float floatValue;
            if (float.TryParse(str, out floatValue)) return floatValue;
            throw new Exception("Parsing to float failed");
        }
        
        public static int ParseToInt(string str)
        {
            int intValue;
            if (int.TryParse(str, out intValue)) return intValue;
            throw new Exception("Parsing to int failed");
        }
    }
}