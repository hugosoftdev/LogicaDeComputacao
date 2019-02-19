﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{

    public enum TokenType { INT, PLUS, MINUS, EOF }

    public class Token
    {
        public TokenType type { get; set; }
        public int value { get; set; }
    }
}
