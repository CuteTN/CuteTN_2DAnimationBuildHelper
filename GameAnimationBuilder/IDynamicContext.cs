using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAnimationBuilder
{
    /// <summary>
    /// Some object (e.g Class, Object has no static context), in fact, it depends on current code to realize what should be the context for an element
    /// </summary>
    interface IDynamicContext: IScriptable
    {
        ContextType GetDynamicContext(int order, List<string> code);
        string GetDynamicHint(int order, List<string> code);
    }
}
