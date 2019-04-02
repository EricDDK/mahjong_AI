using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mahjongAI
{
    public class AITableFeng
    {
        public static ConcurrentDictionary<long, List<AITableInfo>> table = new ConcurrentDictionary<long, List<AITableInfo>>();
        public static string[] ziname = new string[]
        { "东", "南", "西", "北" };

        public static void gen()
        {
            AICommon.table = table;
            AICommon.N = 4;
            AICommon.NAME = "feng";
            AICommon.CARD = ziname;
            AICommon.huLian = false;
            AICommon.baseP = 16.0d / 136;
            AICommon.gen();
        }

        public static void load()
        {
            table.Clear();
            AICommon.table = table;
            AICommon.N = 4;
            AICommon.NAME = "feng";
            AICommon.CARD = ziname;
            AICommon.huLian = false;
            AICommon.baseP = 16.0d / 136;
            AICommon.load();
        }

        public static void load(List<string> lines)
        {
            table.Clear();
            AICommon.table = table;
            AICommon.N = 4;
            AICommon.NAME = "feng";
            AICommon.CARD = ziname;
            AICommon.huLian = false;
            AICommon.baseP = 16.0d / 136;
            AICommon.load(lines);
        }
    }
}
