using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mahjongAI
{
    public class HuTableInfo
    {
        public byte needGui;
        public bool jiang;
        public byte[] hupai = new byte[9];

        public string toString()
        {
            string tmp = "";
            int index = 1;
            if (hupai == null)
            {
                tmp = "胡清";
            }
            else
            {
                foreach (byte i in hupai)
                {
                    if (i > 0)
                    {
                        tmp += "胡" + (index);
                    }
                    index++;
                }
            }
            return tmp + " 将" + (jiang ? "1" : "0") + " 鬼" + needGui;
        }

    }
}
