using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mahjongAI
{
    public class AIInfo
    {
        public int inputNum;
        public int jiang;

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (o == null || GetType() != o.GetType())
                return false;

            AIInfo huInfo = (AIInfo)o;

            if (inputNum != huInfo.inputNum)
                return false;
            if (jiang != huInfo.jiang)
                return false;
            return true;
        }

        // HashSet deduplication
        public override int GetHashCode()
        {
            int result = inputNum;
            result = 31 * result + jiang;
            return result;
        }
    }
}
