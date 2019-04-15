using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mahjongAI
{
    public class AITableJian
    {
        public static ConcurrentDictionary<long, List<AITableInfo>> table = new ConcurrentDictionary<long, List<AITableInfo>>();
        public static string[] ziname = new string[]
        { "中", "发", "白" };

        public static void gen()
        {
            AICommonStatic.table = table;
            AICommonStatic.N = 3;
            AICommonStatic.NAME = "jian";
            AICommonStatic.CARD = ziname;
            AICommonStatic.huLian = false;
            AICommonStatic.baseP = 12.0d / 136;
            AICommonStatic.gen();
        }

        public static void load()
        {
            table.Clear();
            AICommonStatic.table = table;
            AICommonStatic.N = 3;
            AICommonStatic.NAME = "jian";
            AICommonStatic.CARD = ziname;
            AICommonStatic.huLian = false;
            AICommonStatic.baseP = 12.0d / 136;
            AICommonStatic.load();
        }

        public static void load(List<string> lines)
        {
            table.Clear();
            AICommonStatic.table = table;
            AICommonStatic.N = 3;
            AICommonStatic.NAME = "jian";
            AICommonStatic.CARD = ziname;
            AICommonStatic.huLian = false;
            AICommonStatic.baseP = 12.0d / 136;
            AICommonStatic.load(lines);
        }
    }
}
