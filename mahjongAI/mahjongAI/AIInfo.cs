﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mahjongAI
{
    public class AIInfo
    {
        public byte inputNum;
        public byte jiang;

        public bool equals(object o)
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

        public int hashCode()
        {
            int result = inputNum;
            result = 31 * result + jiang;
            return result;
        }
    }
}
