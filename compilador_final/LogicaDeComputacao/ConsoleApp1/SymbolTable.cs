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

        public SymbolTable Daddy { get; set; }

        public SymbolTable()
        {
            st = new Dictionary<string, Object>();
        }

        public Object GetFuncDeclaration(string key)
        {
            SymbolTable temp = Daddy;
            while(temp.Daddy != null)
            {
                temp = temp.Daddy;
            }
            return temp.GetValue(key);
        }

        public Object GetValue(string key, bool isFunc=false)
        {
            if (isFunc && (Daddy != null))
            {
                return GetFuncDeclaration(key);
            }
            if (st.ContainsKey(key))
            {

                return ((List<Object>) st[key])[0];
            }
            else
            {
                if(Daddy != null)
                {
                    return Daddy.GetValue(key);
                }
                throw new Exception("Variável não declarada");
            }
        }

        public Object GetType(string key)
        {
            if (st.ContainsKey(key))
            {

                return ((List<Object>)st[key])[1];
            }
            else
            {
                if (Daddy != null)
                {
                    return Daddy.GetType(key);
                }
                throw new Exception("Variável não declarada");
            }
        }

        public void SetValue(string key, Object value)
        {
            if (st.ContainsKey(key))
            {
                List<Object> actualValue = (List<Object>) st[key];
                actualValue[0] = value;
                st[key] = actualValue;
            } else
            {
                if (Daddy != null)
                {
                    Daddy.SetValue(key, value);
                }
                throw new Exception("Varíavel não declarada");
            }
        }

        public void SetType(string key, Object type)
        {
            if (st.ContainsKey(key))
            {
                throw new Exception("Essa varíavel ja foi declarada");
            }
            else
            {
                st.Add(key, new List<Object>() { null, type });
            }
        }
    }
}
