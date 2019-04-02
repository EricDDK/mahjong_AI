using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mahjongAI
{
    public class HuCommon
    {
        public static ConcurrentDictionary<long, List<HuTableInfo>> table;
        public static int N;
        public static string NAME;
        public static string[] CARD;
        public static bool huLian;

        public static void load()
        {
            try
            {
                string[] strs = System.IO.File.ReadAllLines(string.Format("{0}\\Config\\{1}", Environment.CurrentDirectory, "majiang_clien_" + NAME + ".txt"));

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
                    int gui = int.Parse(strs[1]);
                    int jiang = int.Parse(strs[2]);
                    int hu = int.Parse(strs[3]);

                    List<HuTableInfo> huTableInfos;
                    if (table.ContainsKey(key))
                    {
                        huTableInfos = table[key];
                    }
                    else
                    {
                        huTableInfos = new List<HuTableInfo>();
                        table[key] = huTableInfos;
                    }

                    byte[] num = new byte[N];
                    long tmp = hu;
                    for (int i = 0; i < N; i++)
                    {
                        num[N - 1 - i] = (byte)(tmp % 10);
                        tmp = tmp / 10;
                    }
                    HuTableInfo huTableInfo = new HuTableInfo();
                    huTableInfo.needGui = (byte)gui;
                    huTableInfo.jiang = jiang != 0;
                    huTableInfo.hupai = hu == -1 ? null : num;
                    huTableInfos.Add(huTableInfo);
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
