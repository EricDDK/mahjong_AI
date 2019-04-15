using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mahjongAI
{
    public class HuInfo
    {
        public int needGui;
        public int jiang;
        public int hupai;

        public string toString()
        {
            return "胡" + (hupai + 1) + " 将" + (jiang + 1) + " 鬼" + needGui;
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (o == null || GetType() != o.GetType())
                return false;

            HuInfo huInfo = (HuInfo)o;

            if (needGui != huInfo.needGui)
                return false;
            if (jiang != huInfo.jiang)
                return false;
            if (hupai != huInfo.hupai)
                return false;
            return true;
        }

        public override int GetHashCode()
        {
            int result = needGui;
            result = 31 * result + jiang;
            result = 31 * result + hupai;
            return result;
        }
    }
}
