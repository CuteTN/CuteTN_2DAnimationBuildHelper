using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAnimationBuilder
{
    public class PropertyInfo
    {
        public ContextType Type;
        public string Name;
        public string Hint;
        public string EncodedValue;

        public PropertyInfo() 
        { 
            Type = ContextType.Unknown;
            Name = "";
            Hint = "";
            EncodedValue = "";
        }

        public PropertyInfo(ContextType type, string name, string encodedValue, string hint)
        {
            Type = type;
            Name = name;
            Hint = hint;
            EncodedValue = encodedValue;
        }

        public PropertyInfo Clone()
        {
            return new PropertyInfo(Type, Name, EncodedValue, Hint);
        }
    }
}
