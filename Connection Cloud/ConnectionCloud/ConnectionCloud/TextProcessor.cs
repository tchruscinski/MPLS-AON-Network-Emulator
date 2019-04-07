using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionCloud
{
    public class TextProcessor
    {
        public string[] splitText(string message)
        {
            String[] split_str;
            split_str =  message.Split(',');

            return split_str;
        }
    }
}
