using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mahjongAI
{
    public class HuTable
    {
        public static ConcurrentDictionary<long, List<HuTableInfo>> table = new ConcurrentDictionary<long, List<HuTableInfo>>();
        public static string[] names = new string[]
        { "1万", "2万", "3万", "4万", "5万", "6万", "7万", "8万", "9万" };

        public static void gen()
        {
            HuCommon.table = table;
            HuCommon.N = 9;
            HuCommon.NAME = "normal";
            HuCommon.CARD = names;
            HuCommon.huLian = true;
            HuCommon.gen();
        }

        public static void load()
        {
            table.Clear();
            HuCommon.table = table;
            HuCommon.N = 9;
            HuCommon.NAME = "normal";
            HuCommon.CARD = names;
            HuCommon.huLian = true;
            HuCommon.load();
        }

        public static void load(List<string> lines)
        {
            table.Clear();
            HuCommon.table = table;
            HuCommon.N = 9;
            HuCommon.NAME = "normal";
            HuCommon.CARD = names;
            HuCommon.huLian = true;
            HuCommon.load(lines);
        }
    }
}
