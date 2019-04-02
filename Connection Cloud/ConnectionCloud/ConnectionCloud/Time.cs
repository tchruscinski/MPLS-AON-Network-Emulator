using System;


namespace ConnectionCloud
{
    class Time
    {

        public string GetTimestamp(DateTime dt)
        {
            return dt.ToString("[yyyy-MM-dd HH:mm:ss]");
        }

    }
}
