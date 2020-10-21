using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAnimationBuilder
{
    public interface IScriptable
    {
        string GetSnippet();
        
        /// <summary>
        /// base: 0 => Tag; 1 => Id
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        ContextType GetContext(int order);

        string GetHint(int order);

        /// <summary>
        /// Returns a string to specify error
        /// </summary>
        /// <param name="codeWords"></param>
        /// <returns></returns>
        string ParseData(List<string> codeWords);
    }
}
