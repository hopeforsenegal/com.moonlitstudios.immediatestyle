namespace MoonlitSystem
{
    public static class RandomUtil
    {
        public static string RandomString(char[] chars, int size)
        {
            if (chars == null) return string.Empty;
            
            var result = string.Empty;
            for (var i = 0; i < size; i++) {
                result += chars[UnityEngine.Random.Range(0, chars.Length)];
            }
            return result;
        }
    }
}