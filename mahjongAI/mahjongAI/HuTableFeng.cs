using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mahjongAI
{
    public class HuTableFeng
    {
        public static ConcurrentDictionary<long, List<HuTableInfo>> table = new ConcurrentDictionary<long, List<HuTableInfo>>();
        public static string[] ziname = new string[]
        { "东", "南", "西", "北" };

        public static void gen()
        {
            HuCommon.table = table;
            HuCommon.N = 4;
            HuCommon.NAME = "feng";
            HuCommon.CARD = ziname;
            HuCommon.huLian = false;
            HuCommon.gen();
        }

        public static void load()
        {
            table.Clear();
            HuCommon.table = table;
            HuCommon.N = 4;
            HuCommon.NAME = "feng";
            HuCommon.CARD = ziname;
            HuCommon.huLian = false;
            HuCommon.load();
        }

        public static void load(List<string> lines)
        {
            table.Clear();
            HuCommon.table = table;
            HuCommon.N = 4;
            HuCommon.NAME = "feng";
            HuCommon.CARD = ziname;
            HuCommon.huLian = false;
            HuCommon.load(lines);
        }
    }
}
