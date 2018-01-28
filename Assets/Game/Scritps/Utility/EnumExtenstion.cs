using System;
using System.ComponentModel;
using System.Reflection;

public static class EnumExtenstion
{
    /// <summary>
    /// 取得Enum 的 Description，搭配加上DescriptionAttribute Enum使用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string GetEnumDescription(this Enum source)
    {
        FieldInfo fi = source.GetType().GetField(source.ToString());

        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
            typeof(DescriptionAttribute), false);

        if (attributes != null && attributes.Length > 0)
            return attributes[0].Description;
        else
            return source.ToString();
    }
}

