using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mahjongAI
{
    public class AIDynamicCommon
    {
        public static Dictionary<int, HashSet<long>> _everyNormalCards = new Dictionary<int, HashSet<long>>();
        public static Dictionary<int, HashSet<long>> _everyFengCards = new Dictionary<int, HashSet<long>>();
        public static Dictionary<int, HashSet<long>> _everyJianCards = new Dictionary<int, HashSet<long>>();

        public static void Register()
        {
            for (int inputNum = 0; inputNum <= AICommon.LEVEL; inputNum++)
            {
                AICommon.N = 4;
                int[] tmpnum = new int[AICommon.N];
                HashSet<long> tmpcard = new HashSet<long>();
                AICommon.gen_card(tmpcard, tmpnum, 0, inputNum);
                _everyFengCards[inputNum] = tmpcard;
            }
            for (int inputNum = 0; inputNum <= AICommon.LEVEL; inputNum++)
            {
                AICommon.N = 3;
                int[] tmpnum = new int[AICommon.N];
                HashSet<long> tmpcard = new HashSet<long>();
                AICommon.gen_card(tmpcard, tmpnum, 0, inputNum);
                _everyJianCards[inputNum] = tmpcard;
            }
            for (int inputNum = 0; inputNum <= AICommon.LEVEL; inputNum++)
            {
                AICommon.N = 9;
                int[] tmpnum = new int[AICommon.N];
                HashSet<long> tmpcard = new HashSet<long>();
                AICommon.gen_card(tmpcard, tmpnum, 0, inputNum);
                _everyNormalCards[inputNum] = tmpcard;
            }
            int a = 1;
        }

        public static int outAI(List<int> input, List<int> guiCard, int[] remainCards, List<int> bannedCards)
        {
            int ret = 0;
            double max = double.MinValue;
            int[] cache = new int[MaJiangDef.MAX_NUM + 1];
            foreach (int c in input)
            {
                if (cache[c] == 0)
                {
                    if (!guiCard.Contains(c) && !bannedCards.Contains(c))
                    {
                        List<int> tmp = new List<int>(input);
                        tmp.Remove(c);
                        double score = calc(tmp, guiCard);
                        if (score > max)
                        {
                            max = score;
                            ret = c;
                        }
                    }
                }
                cache[c] = 1;
            }
            return ret;
        }

        public static double calc(List<int> input, List<int> guiCard)
        {
            List<int> cards = new List<int>();
            for (int i = 0; i < MaJiangDef.MAX_NUM; i++)
            {
                cards.Add(0);
            }
            foreach (int c in input)
            {
                cards[c - 1] = cards[c - 1] + 1;
            }

            int guiNum = 0;
            foreach (int gui in guiCard)
            {
                guiNum += cards[gui - 1];
                cards[gui - 1] = 0;
            }

            List<int> ting = HuUtil.isTingCard(cards, guiNum);
            if (ting.Count != 0)
            {
                return ting.Count * 10;
            }

            long wan_key = 0;
            long tong_key = 0;
            long tiao_key = 0;
            long feng_key = 0;
            long jian_key = 0;

            for (int i = MaJiangDef.WAN1; i <= MaJiangDef.WAN9; i++)
            {
                int num = cards[i - 1];
                wan_key = wan_key * 10 + num;
            }
            for (int i = MaJiangDef.TONG1; i <= MaJiangDef.TONG9; i++)
            {
                int num = cards[i - 1];
                tong_key = tong_key * 10 + num;
            }
            for (int i = MaJiangDef.TIAO1; i <= MaJiangDef.TIAO9; i++)
            {
                int num = cards[i - 1];
                tiao_key = tiao_key * 10 + num;
            }
            for (int i = MaJiangDef.FENG_DONG; i <= MaJiangDef.FENG_BEI; i++)
            {
                int num = cards[i - 1];
                feng_key = feng_key * 10 + num;
            }
            for (int i = MaJiangDef.JIAN_ZHONG; i <= MaJiangDef.JIAN_BAI; i++)
            {
                int num = cards[i - 1];
                jian_key = jian_key * 10 + num;
            }

            int ttt = 10000000;
            List<AITableInfo> t1 = AITable.table[ttt];
            List<AITableInfo> t2 = getAITable(ttt, _everyNormalCards);

            List<List<AITableInfo>> tmp = new List<List<AITableInfo>>();

            List<AITableInfo> wanAITableInfo = AITable.table[wan_key];
            tmp.Add(wanAITableInfo);
            List<AITableInfo> dynamicWanAITableInfo = getAITable(wan_key, _everyNormalCards);

            List<AITableInfo> tongAITableInfo = AITable.table[tong_key];
            tmp.Add(tongAITableInfo);

            List<AITableInfo> tiaoAITableInfo = AITable.table[tiao_key];
            tmp.Add(tiaoAITableInfo);

            List<AITableInfo> fengAITableInfo = AITableFeng.table[feng_key];
            tmp.Add(fengAITableInfo);

            List<AITableInfo> jianAITableInfo = AITableJian.table[jian_key];
            tmp.Add(jianAITableInfo);

            List<double> ret = new List<double>();
            calcAITableInfo(ret, tmp, 0, false, 0.0d);

            double result = 0.0d;
            foreach (double d in ret)
            {
                if (d > result)
                {
                    result = d;
                }
            }
            return result;
        }

        private static void calcAITableInfo(List<double> ret, List<List<AITableInfo>> tmp, int index, bool jiang, double cur)
        {
            if (index >= tmp.Count)
            {
                if (jiang)
                {
                    ret.Add(cur);
                }
                return;
            }
            List<AITableInfo> aiTableInfos = tmp[index];
            foreach (AITableInfo aiTableInfo in aiTableInfos)
            {
                if (jiang)
                {
                    if (aiTableInfo.jiang == false)
                    {
                        calcAITableInfo(ret, tmp, index + 1, jiang, cur + aiTableInfo.p);
                    }
                }
                else
                {
                    calcAITableInfo(ret, tmp, index + 1, aiTableInfo.jiang, cur + aiTableInfo.p);
                }
            }
        }

        public static List<AITableInfo> getAITable(long card, Dictionary<int, HashSet<long>> tmpcards)
        {
            int N = 9;
            double baseP = 36.0d / 136;

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

            for (int inputNum = 0; inputNum <= AICommon.LEVEL; inputNum++)
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
                        check_ai(aiInfos, num, 128, inputNum);
                        valid++;
                    }

                    for (int i = 0; i < N; i++)
                    {
                        num[i] -= tmpcnum[i];
                    }
                }

                foreach (AIInfo aiInfo in aiInfos)
                {
                    key = aiInfo.jiang != 128 ? 1 : 0;
                    if (aiInfo.inputNum == 0)
                    {
                        aiTableInfos[key].p = 1;
                    }
                    if (aiTableInfos[key].p != 1)
                    {
                        key = aiInfo.jiang != 128 ? 1 : 0;
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

        public static void check_ai(HashSet<AIInfo> aiInfos, int[] num, int jiang, int inputNum)
        {
            bool huLian = true;
            int N = 9;

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
                if (num[i] >= 2 && jiang == 128)
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
            int N = 9;
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

    }
}
