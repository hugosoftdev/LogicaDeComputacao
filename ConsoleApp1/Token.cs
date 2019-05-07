using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public enum TokenType {
        INT,
        PLUS,
        MINUS,
        EOF,
        MULTIPLY,
        DIVIDE,
        PARENTHESESBEGIN,
        PARENTHESESEND,
        IDENTIFIER,
        PRINT,
        IF,
        ELSE,
        WEND,
        THEN,
        WHILE,
        END,
        OR,
        AND,
        EQUAL,
        BIGGERTHEN,
        SMALLERTHEN,
        INPUT,
        NOT,
        DIM,
        AS,
        INTEGER,
        BOOLEAN,
        BOOL,
        SUB,
        MAIN,
        BREAKLINE
    }

    public class Token
    {
        public TokenType type { get; set; }
        public Object value { get; set; }

        public Token Copy()
        {
            var json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<Token>(json);
        }
    }
}
