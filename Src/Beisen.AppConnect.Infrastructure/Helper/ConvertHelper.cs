namespace Beisen.AppConnect.Infrastructure.Helper
{
    public static class ConvertHelper
    {
        public static int ToInt(string value)
        {
            var result = 0;
            if (!string.IsNullOrWhiteSpace(value))
            {
                int.TryParse(value, out result);
            }
            return result;
        }
    }
}
