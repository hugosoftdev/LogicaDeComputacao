﻿using System;
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

        public Object GetValue(string key)
        {
            if (st.ContainsKey(key))
            {

                return ((List<Object>) st[key])[0];
            }
            else
            {
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
