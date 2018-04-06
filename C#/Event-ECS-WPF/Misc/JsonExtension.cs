using Newtonsoft.Json;
using System;

namespace Event_ECS_Client_WPF
{
    public static class JsonExtension
    {
        public static bool ValueEquals<T>(this JsonReader reader, T value)
        {
            return reader.ValueType == typeof(T) && reader.Value.Equals(value);
        }

        public static bool ValueEquals(this JsonReader reader, string str)
        {
            return reader.ValueType == typeof(string) && string.Equals((string)reader.Value, str, StringComparison.OrdinalIgnoreCase);
        }

        public static void ReadArray<T>(this JsonReader reader, Action<T> callback)
        {
            while (reader.Read() && reader.TokenType != JsonToken.EndObject && reader.TokenType != JsonToken.EndArray)
            {
                callback((T)reader.Value);
            }
        }
    }
}
