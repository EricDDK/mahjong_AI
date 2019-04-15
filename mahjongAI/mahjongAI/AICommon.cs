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
            HashSet<long> card = new HashSet<long>();

            for (int i = 0; i <= 14; i++)
            {
                int[] num = new int[N];
                gen_card(card, num, 0, i);
            }

            Dictionary<int, HashSet<long>> tmpcards = new Dictionary<int, HashSet<long>>();
            for (int inputNum = 0; inputNum <= LEVEL; inputNum++)
            {
                int[] tmpnum = new int[N];
                HashSet<long> tmpcard = new HashSet<long>();
                gen_card(tmpcard, tmpnum, 0, inputNum);
                tmpcards[inputNum] = tmpcard;
            }

            Console.WriteLine(card.Count);

            int index = 1;
            foreach (long l in card)
            {
                index++;
                check_ai(l, tmpcards);
                output(l, null);
                Console.WriteLine(index);
            }
        }

        private static void output(long card, object o)
        {
            long key = card;
            List<string> result = new List<string>();
            List<AITableInfo> aiTableInfos = table[card];
            if (aiTableInfos.Count != 0)
            {
                foreach (AITableInfo aiTableInfo in aiTableInfos)
                {
                    string str = key + " ";
                    str += aiTableInfo.jiang ? "1 " : "0 ";
                    str += aiTableInfo.p;
                    str += " ";
                    str += show_card(key) + " ";
                    str += aiTableInfo.jiang ? "有将 " : "无将 ";
                    str += aiTableInfo.p;
                    str += "\n";
                    result.Add(str);
                }
            }
            int a = 1;
        }

        public static List<AITableInfo> getAITable(long card, Dictionary<int, HashSet<long>> tmpcards)
        {
            int[] num = new int[N];
            long tmp = card;
            for (int i = 0; i < N; i++)
            {
                num[N - 1 - i] = (int)(tmp % 10);
                tmp = tmp / 10;
            }

            int total = 0;
            for (int i = 0; i < N; i++)
            {
                total += num[i];
            }

            Dictionary<int, AITableInfo> aiTableInfos = new Dictionary<int, AITableInfo>();

            AITableInfo aiTableInfo = new AITableInfo();
            aiTableInfo.p = 0;
            aiTableInfo.jiang = true;
            int key = aiTableInfo.jiang ? 1 : 0;
            aiTableInfos[key] = aiTableInfo;
            aiTableInfo = new AITableInfo();
            aiTableInfo.p = 0;
            aiTableInfo.jiang = false;
            key = aiTableInfo.jiang ? 1 : 0;
            aiTableInfos[key] = aiTableInfo;

            for (int inputNum = 0; inputNum <= LEVEL; inputNum++)
            {
                HashSet<long> tmpcard = tmpcards[inputNum];

                HashSet<AIInfo> aiInfos = new HashSet<AIInfo>();

                int valid = 0;

                foreach (long tmpc in tmpcard)
                {
                    int[] tmpcnum = new int[N];
                    long tt = tmpc;
                    for (int i = 0; i < N; i++)
                    {
                        tmpcnum[N - 1 - i] = (int)(tt % 10);
                        tt = tt / 10;
                    }

                    bool max = false;
                    for (int i = 0; i < N; i++)
                    {
                        num[i] += tmpcnum[i];
                        if (num[i] > 4)
                        {
                            max = true;
                        }
                    }

                    if (!max)
                    {
                        check_ai(aiInfos, num, -1, inputNum);
                        valid++;
                    }

                    for (int i = 0; i < N; i++)
                    {
                        num[i] -= tmpcnum[i];
                    }
                }

                foreach (AIInfo aiInfo in aiInfos)
                {
                    key = aiInfo.jiang != -1 ? 1 : 0;
                    if (aiInfo.inputNum == 0)
                    {
                        aiTableInfos[key].p = 1;
                    }
                    if (aiTableInfos[key].p != 1)
                    {
                        key = aiInfo.jiang != -1 ? 1 : 0;
                        aiTableInfos[key].p += baseP * 1.0d / valid;
                    }
                }
            }

            List<AITableInfo> tmpAI = new List<AITableInfo>();
            foreach (var o in aiTableInfos.Values)
            {
                tmpAI.Add(o);
            }
            return tmpAI;
        }

        public static void check_ai(long card, Dictionary<int, HashSet<long>> tmpcards)
        {
            int[] num = new int[N];
            long tmp = card;
            for (int i = 0; i < N; i++)
            {
                num[N - 1 - i] = (int)(tmp % 10);
                tmp = tmp / 10;
            }

            int total = 0;
            for (int i = 0; i < N; i++)
            {
                total += num[i];
            }

            Dictionary<int, AITableInfo> aiTableInfos = new Dictionary<int, AITableInfo>();

            AITableInfo aiTableInfo = new AITableInfo();
            aiTableInfo.p = 0;
            aiTableInfo.jiang = true;
            int key = aiTableInfo.jiang ? 1 : 0;
            aiTableInfos[key] = aiTableInfo;
            aiTableInfo = new AITableInfo();
            aiTableInfo.p = 0;
            aiTableInfo.jiang = false;
            key = aiTableInfo.jiang ? 1 : 0;
            aiTableInfos[key] = aiTableInfo;

            for (int inputNum = 0; inputNum <= LEVEL; inputNum++)
            {
                HashSet<long> tmpcard = tmpcards[inputNum];

                HashSet<AIInfo> aiInfos = new HashSet<AIInfo>();

                int valid = 0;

                foreach (long tmpc in tmpcard)
                {
                    int[] tmpcnum = new int[N];
                    long tt = tmpc;
                    for (int i = 0; i < N; i++)
                    {
                        tmpcnum[N - 1 - i] = (int)(tt % 10);
                        tt = tt / 10;
                    }

                    bool max = false;
                    for (int i = 0; i < N; i++)
                    {
                        num[i] += tmpcnum[i];
                        if (num[i] > 4)
                        {
                            max = true;
                        }
                    }

                    if (!max)
                    {
                        check_ai(aiInfos, num, -1, inputNum);
                        valid++;
                    }

                    for (int i = 0; i < N; i++)
                    {
                        num[i] -= tmpcnum[i];
                    }
                }

                foreach (AIInfo aiInfo in aiInfos)
                {
                    key = aiInfo.jiang != -1 ? 1 : 0;
                    if (aiInfo.inputNum == 0)
                    {
                        aiTableInfos[key].p = 1;
                    }
                    if (aiTableInfos[key].p != 1)
                    {
                        key = aiInfo.jiang != -1 ? 1 : 0;
                        aiTableInfos[key].p += baseP * 1.0d / valid;
                    }
                }
            }

            List<AITableInfo> tmpAI = new List<AITableInfo>();
            foreach (var o in aiTableInfos.Values)
            {
                tmpAI.Add(o);
            }
            table[card] = tmpAI;
        }

        public static void check_ai(HashSet<AIInfo> aiInfos, int[] num, int jiang, int inputNum)
        {
            if (huLian)
            {
                for (int i = 0; i < N; i++)
                {
                    if (num[i] > 0 && i + 1 < N && num[i + 1] > 0 && i + 2 < N && num[i + 2] > 0)
                    {
                        num[i]--;
                        num[i + 1]--;
                        num[i + 2]--;
                        check_ai(aiInfos, num, jiang, inputNum);
                        num[i]++;
                        num[i + 1]++;
                        num[i + 2]++;
                    }
                }
            }

            for (int i = 0; i < N; i++)
            {
                if (num[i] >= 2 && jiang == -1)
                {
                    num[i] -= 2;
                    check_ai(aiInfos, num, i, inputNum);
                    num[i] += 2;
                }
            }

            for (int i = 0; i < N; i++)
            {
                if (num[i] >= 3)
                {
                    num[i] -= 3;
                    check_ai(aiInfos, num, jiang, inputNum);
                    num[i] += 3;
                }
            }

            for (int i = 0; i < N; i++)
            {
                if (num[i] != 0)
                {
                    return;
                }
            }

            AIInfo aiInfo = new AIInfo();
            aiInfo.inputNum = (int)inputNum;
            aiInfo.jiang = (int)jiang;
            aiInfos.Add(aiInfo);
        }

        public static void gen_card(HashSet<long> card, int[] num, int index, int total)
        {
            if (index == N - 1)
            {
                if (total > 4)
                {
                    return;
                }
                num[index] = total;

                long ret = 0;
                foreach (int c in num)
                {
                    ret = ret * 10 + c;
                }
                card.Add(ret);
                return;
            }
            for (int i = 0; i <= 4; i++)
            {
                if (i <= total)
                {
                    num[index] = i;
                }
                else
                {
                    num[index] = 0;
                }
                gen_card(card, num, index + 1, total - num[index]);
            }
        }

        public static string show_card(long card)
        {
            int[] num = new int[N];
            long tmp = card;
            for (int i = 0; i < N; i++)
            {
                num[N - 1 - i] = (int)(tmp % 10);
                tmp = tmp / 10;
            }
            string ret = "";
            int index = 1;
            foreach (int i in num)
            {
                string str1 = CARD[index - 1];
                for (int j = 0; j < i; j++)
                {
                    ret += str1 + "";
                }
                index++;
            }
            return ret;
        }

    }
}
