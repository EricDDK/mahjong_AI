using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mahjongAI
{
    public class AITableInfo
    {
        public bool jiang;
        public double p;

        public string toString()
        {
            return " 将" + (jiang ? "1" : "0") + " 几率" + p;
        }

    }
}
