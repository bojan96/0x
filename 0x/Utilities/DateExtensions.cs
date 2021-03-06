﻿using System;

namespace ZeroX.Utilities
{
    public static class DateExtensions
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long GetUnixTime(this DateTime dateTime)
            => (long)(dateTime.ToUniversalTime() - _epoch).TotalSeconds;

        public static DateTime GetUtcDate(this long unixTime)
            => _epoch + TimeSpan.FromSeconds(unixTime);
        
    }
}
