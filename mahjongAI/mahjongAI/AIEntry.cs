using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace mahjongAI
{
    public class AIEntry
    {
        public static void RegisterAI()
        {
            Console.WriteLine(" ====== AI start ======");

            HuUtil.load();
            AIUtil.load();

            Console.WriteLine(" ====== AI end ======");
        }

    }
}
