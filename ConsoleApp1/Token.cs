using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{

    public enum TokenType { INT, PLUS, MINUS, EOF, MULTIPLY, DIVIDE, PARENTHESESBEGIN, PARENTHESESEND }

    public class Token
    {
        public TokenType type { get; set; }
        public int value { get; set; }

        public Token Copy()
        {
            var json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<Token>(json);
        }
    }
}
