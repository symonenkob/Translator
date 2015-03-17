using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator_1.RPN
{
    class RpnRow
    {
        public int Step { get; set; }
        public string Stack { get; set; }
        public string InputChain { get; set; }
        public string Rpn { get; set; }
    }
}
