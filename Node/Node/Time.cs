using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node
{
    public class Time
    {

        public string GetTimestamp(DateTime dt)
        {

            return dt.ToString("[yyyy-MM-dd HH:mm:ss]");
        }

    }

}