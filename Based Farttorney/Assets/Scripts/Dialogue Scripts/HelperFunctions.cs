using System;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

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
        
        // used in "choice" action; converts "choiceText:choiceAction" to two different arrays
        public static (string[], string[]) ParseChoices(string[] parameterList)
        {
            // convert it all back to a single string
            string parameterString = String.Join(" ", parameterList);
            Debug.Log(parameterString);
            string[] splitList = parameterString.Split("|");
            
            // split to choiceText and choiceScript
            string[] choiceTextList = new string[splitList.Length];
            string[] choiceScriptList = new string[splitList.Length];
            
            for(int x = 0; x < splitList.Length; x++)
            {
                string choiceText = splitList[x].Split(":")[0];
                string choiceScript = splitList[x].Split(":")[1];
                Debug.Log("choicetext " + choiceText);
                Debug.Log("choicescript " + choiceScript);
                choiceTextList[x] = choiceText.Trim();
                choiceScriptList[x] = choiceScript.Trim();
            }

            return (choiceTextList, choiceScriptList);
        }
    }
}