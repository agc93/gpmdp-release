namespace GPMDP.Release
{
    public static class CoreExtensions
    {
        internal static bool IsEmpty(this string s) {
            return string.IsNullOrWhiteSpace(s);
        }
        internal static bool IsNotEmpty(this string s) {
            return !s.IsEmpty();
        }
    }
}