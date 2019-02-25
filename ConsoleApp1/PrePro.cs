﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class PrePro
    {
        public static string filter(string input)
        {
            string buffer = string.Empty;
            bool detectedStart = false;
            foreach(char c in input)
            {
                if(!detectedStart)
                {
                    if(c != '\'')
                    {
                        buffer += c;
                    } else
                    {
                        detectedStart = true;
                    }
                } else
                {
                    if (c.Equals('\n'))
                    {
                        detectedStart = false;
                    }
                }
            }
            return buffer;
        }
    }
}
