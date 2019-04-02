using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mahjongAI
{
    public class AITable
    {
        public static ConcurrentDictionary<long, List<AITableInfo>> table = new ConcurrentDictionary<long, List<AITableInfo>>();
        public static string[] names = new string[]
        { "1万", "2万", "3万", "4万", "5万", "6万", "7万", "8万", "9万" };

        public static void gen()
        {
            AICommon.table = table;
            AICommon.N = 9;
            AICommon.NAME = "normal";
            AICommon.CARD = names;
            AICommon.huLian = true;
            AICommon.baseP = 36.0d / 136;
            AICommon.gen();
        }

        public static void load()
        {
            table.Clear();
            AICommon.table = table;
            AICommon.N = 9;
            AICommon.NAME = "normal";
            AICommon.CARD = names;
            AICommon.huLian = true;
            AICommon.baseP = 36.0d / 136;
            AICommon.load();
        }

        public static void load(List<string> lines)
        {
            table.Clear();
            AICommon.table = table;
            AICommon.N = 9;
            AICommon.NAME = "normal";
            AICommon.CARD = names;
            AICommon.huLian = true;
            AICommon.baseP = 36.0d / 136;
            AICommon.load(lines);
        }
    }
}
