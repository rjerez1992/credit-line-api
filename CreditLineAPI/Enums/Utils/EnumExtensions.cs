﻿using System.Reflection;

namespace CreditLineAPI.Enums.Utils
{
    static class EnumExtensions
    {
        #pragma warning disable CS8600, CS8602, CS8603
        public static string GetStringValue(this Enum value)
        {
            Type type = value.GetType();

            FieldInfo fieldInfo = type.GetField(value.ToString());

            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }
    }
}
