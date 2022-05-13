

namespace TimeZones
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            TimeZonesUtil.GetTimeZones()?.ForEach(Console.WriteLine);
        }
    }
}
