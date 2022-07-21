/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
namespace Sportradar.MTS.SDK.Test.Helpers
{
    public static class TestHelper
    {
        public const string ChannelParamName = "limitId";


        public static int ContainsCount(string text, string value, System.StringComparison comparison)
        {
            if (text == null) throw new System.ArgumentNullException("text");
            if (string.IsNullOrEmpty(value))
                return -1;

            int index = 0;
            int count = 0;
            while ((index = text.IndexOf(value, index, text.Length - index, comparison)) != -1)
            {
                count++;
                index++;
            }
            return count;
        }
    }
}
