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
            AICommonStatic.table = table;
            AICommonStatic.N = 9;
            AICommonStatic.NAME = "normal";
            AICommonStatic.CARD = names;
            AICommonStatic.huLian = true;
            AICommonStatic.baseP = 36.0d / 136;
            AICommonStatic.gen();
        }

        public static void load()
        {
            table.Clear();
            AICommonStatic.table = table;
            AICommonStatic.N = 9;
            AICommonStatic.NAME = "normal";
            AICommonStatic.CARD = names;
            AICommonStatic.huLian = true;
            AICommonStatic.baseP = 36.0d / 136;
            AICommonStatic.load();
        }

        public static void load(List<string> lines)
        {
            table.Clear();
            AICommonStatic.table = table;
            AICommonStatic.N = 9;
            AICommonStatic.NAME = "normal";
            AICommonStatic.CARD = names;
            AICommonStatic.huLian = true;
            AICommonStatic.baseP = 36.0d / 136;
            AICommonStatic.load(lines);
        }
    }
}
