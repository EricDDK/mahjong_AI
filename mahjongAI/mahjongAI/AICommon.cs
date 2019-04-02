using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mahjongAI
{
    public class AICommon
    {
        public static ConcurrentDictionary<long, List<AITableInfo>> table;
        public static int N;
        public static string NAME;
        public static string[] CARD;
        public static bool huLian;
        public static double baseP;
        public const int LEVEL = 5;

        public static void load()
        {
            try
            {
                string[] strs = System.IO.File.ReadAllLines(string.Format("{0}\\Config\\{1}", Environment.CurrentDirectory, "majiang_ai_" + NAME + ".txt"));
                List<string> lines = new List<string>();
                foreach (string str in strs)
                {
                    lines.Add(str);
                }
                load(lines);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("AI模块加载失败,进程自动关闭...");
                Environment.Exit(0);
            }
        }

        public static void load(List<string> lines)
        {
            int total = 0;
            try
            {
                foreach (string str in lines)
                {
                    string[] strs = str.Split(' ');
                    long key = long.Parse(strs[0]);
                    int jiang = int.Parse(strs[1]);
                    double p = double.Parse(strs[2]);

                    List<AITableInfo> aiTableInfos;
                    if (table.ContainsKey(key))
                    {
                        aiTableInfos = table[key];
                    }
                    else
                    {
                        aiTableInfos = new List<AITableInfo>();
                        table[key] = aiTableInfos;
                    }

                    AITableInfo aiTableInfo = new AITableInfo();
                    aiTableInfo.jiang = jiang != 0;
                    aiTableInfo.p = p;
                    aiTableInfos.Add(aiTableInfo);
                    total++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void gen()
        {
            
        }

    }
}
