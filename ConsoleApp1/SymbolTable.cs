using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class SymbolTable
    {
        private Dictionary<string, Object> st;

        public SymbolTable()
        {
            st = new Dictionary<string, Object>();
        }

        public int Get(string key)
        {
            if (st.ContainsKey(key))
            {
                return (int) st[key];
            }
            else
            {
                throw new Exception("Variável não declarada");
            }
        }

        public void Set(string key, Object value)
        {
            if (st.ContainsKey(key))
            {
                st[key] = value;
            } else
            {
                st.Add(key, value);
            }
        }
    }
}
