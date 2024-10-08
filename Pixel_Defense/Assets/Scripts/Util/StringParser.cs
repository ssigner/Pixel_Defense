using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class StringParser
{
    public static string SetText(string str)
    {
        str = str.Replace("q", "\n");
        return str;
    }
}
