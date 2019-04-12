using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mahjongAI
{
    public class AIUtil
    {
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

            List<List<AITableInfo>> tmp = new List<List<AITableInfo>>();

            List<AITableInfo> wanAITableInfo = AITable.table[wan_key];
            tmp.Add(wanAITableInfo);

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

        private static void calcAITableInfo(List<double> ret, List<List<AITableInfo>> tmp, int index, bool jiang,
                double cur)
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

        public static bool chiAI(List<int> input, List<int> guiCard, int card, int card1, int card2)
        {
            if (guiCard.Contains(card) || guiCard.Contains(card1) || guiCard.Contains(card2))
            {
                return false;
            }

            if (!input.Contains(card1) || !input.Contains(card2))
            {
                return false;
            }

            double score = calc(input, guiCard);

            List<int> tmp = new List<int>(input);
            tmp.Remove(card1);
            tmp.Remove(card2);
            double scoreNew = calc(tmp, guiCard);

            return scoreNew >= score;
        }

        public static List<int> chiAI(List<int> input, List<int> guiCard, int card)
        {
            List<int> ret = new List<int>();
            if (guiCard.Contains(card))
            {
                return ret;
            }

            double score = calc(input, guiCard);
            double scoreNewMax = 0;

            int card1 = 0;
            int card2 = 0;

            if (getHasCount(input, card - 2) > 0 && getHasCount(input, card - 1) > 0
                    && MaJiangDef.type(card) == MaJiangDef.type(card - 2)
                    && MaJiangDef.type(card) == MaJiangDef.type(card - 1))
            {
                List<int> tmp = new List<int>(input);
                tmp.Remove((int)(card - 2));
                tmp.Remove((int)(card - 1));
                double scoreNew = calc(tmp, guiCard);
                if (scoreNew > scoreNewMax)
                {
                    scoreNewMax = scoreNew;
                    card1 = card - 2;
                    card2 = card - 1;
                }
            }

            if (getHasCount(input, card - 1) > 0 && getHasCount(input, card + 1) > 0
                    && MaJiangDef.type(card) == MaJiangDef.type(card - 1)
                    && MaJiangDef.type(card) == MaJiangDef.type(card + 1))
            {
                List<int> tmp = new List<int>(input);
                tmp.Remove((int)(card - 1));
                tmp.Remove((int)(card + 1));
                double scoreNew = calc(tmp, guiCard);
                if (scoreNew > scoreNewMax)
                {
                    scoreNewMax = scoreNew;
                    card1 = card - 1;
                    card2 = card + 1;
                }
            }

            if (getHasCount(input, card + 1) > 0 && getHasCount(input, card + 2) > 0
                    && MaJiangDef.type(card) == MaJiangDef.type(card + 1)
                    && MaJiangDef.type(card) == MaJiangDef.type(card + 2))
            {
                List<int> tmp = new List<int>(input);
                tmp.Remove((int)(card + 1));
                tmp.Remove((int)(card + 2));
                double scoreNew = calc(tmp, guiCard);
                if (scoreNew > scoreNewMax)
                {
                    scoreNewMax = scoreNew;
                    card1 = card + 1;
                    card2 = card + 2;
                }
            }

            if (scoreNewMax > score)
            {
                ret.Add(card1);
                ret.Add(card2);
            }

            return ret;
        }

        public static bool pengAI(List<int> input, List<int> guiCard, int card, double award)
        {
            if (guiCard.Contains(card))
            {
                return false;
            }

            if (getHasCount(input, card) < 2)
            {
                return false;
            }

            double score = calc(input, guiCard);

            List<int> tmp = new List<int>(input);
            tmp.Remove((int)card);
            tmp.Remove((int)card);
            double scoreNew = calc(tmp, guiCard);

            return scoreNew + award >= score;
        }

        public static bool gangAI(List<int> input, List<int> guiCard, int card, double award)
        {
            if (guiCard.Contains(card))
            {
                return false;
            }

            if (getHasCount(input, card) < 3)
            {
                return false;
            }

            double score = calc(input, guiCard);

            List<int> tmp = new List<int>(input);
            tmp.Remove((int)card);
            tmp.Remove((int)card);
            tmp.Remove((int)card);
            tmp.Remove((int)card);
            double scoreNew = calc(tmp, guiCard);

            return scoreNew + award >= score;
        }

        public static void testOut()
        {
            string init = "1万,2万,2万,1条,1条,东";
            string guiStr = "1万";
            List<int> cards = MaJiangDef.stringToCards(init);
            List<int> gui = MaJiangDef.stringToCards(guiStr);
            int[] remain = new int[43] { 0,
                4, 4, 4, 4, 4, 4, 4, 4, 4,
                4, 4, 4, 4, 4, 4, 4, 4, 4,
                4, 4, 4, 4, 4, 4, 4, 4, 4,
                4, 4, 4, 4,
                4, 4, 4,
                1, 1, 1, 1, 1, 1, 1, 1};
            List<int> bannedCards = new List<int>();
            int outRet = outAI(cards, gui, remain, bannedCards);
            int outDRet = AIDynamicCommon.outAI(cards, gui, remain, bannedCards);
            Console.WriteLine(MaJiangDef.cardToString(outRet));
        }

        public static void testChi()
        {
            string init = "1万,2万,2万,1条,1条,1筒,2筒,4筒,4筒,5筒";
            string guiStr = "1万";
            List<int> cards = MaJiangDef.stringToCards(init);
            List<int> gui = MaJiangDef.stringToCards(guiStr);

            Console.WriteLine(chiAI(cards, gui, MaJiangDef.stringToCard("3筒"), MaJiangDef.stringToCard("2筒"),
                    MaJiangDef.stringToCard("4筒")));
            Console.WriteLine(MaJiangDef.cardsToString(chiAI(cards, gui, MaJiangDef.stringToCard("3筒"))));
        }

        public static void testPeng()
        {
            string init = "1万,2万,2万,1条,1条,2筒,4筒,4筒";
            string guiStr = "1万";
            List<int> cards = MaJiangDef.stringToCards(init);
            List<int> gui = MaJiangDef.stringToCards(guiStr);

            Console.WriteLine(pengAI(cards, gui, MaJiangDef.stringToCard("2万"), 0.0d));
        }

        public static void testGang()
        {
            string init = "1万,2万,2万,2万,3万,4万,4筒,4筒";
            string guiStr = "1万";
            List<int> cards = MaJiangDef.stringToCards(init);
            List<int> gui = MaJiangDef.stringToCards(guiStr);

            Console.WriteLine(gangAI(cards, gui, MaJiangDef.stringToCard("2万"), 1.0d));
        }

        public static void gen()
        {
            AITableJian.gen();
            AITableFeng.gen();
            AITable.gen();
        }

        public static void load()
        {
            AITableJian.load();
            AITableFeng.load();
            AITable.load();
        }

        private static void testHu()
        {
            List<int> total = new List<int>();
            for (int i = MaJiangDef.WAN1; i <= MaJiangDef.JIAN_BAI; i++)
            {
                total.Add(i);
                total.Add(i);
                total.Add(i);
                total.Add(i);
            }
            //Collections.shuffle(total);

            List<int> cards = new List<int>();
            for (int i = 0; i < 14; i++)
            {
                int tmp = total[0];
                total.RemoveAt(0);
                cards.Add(tmp);
            }

            cards.Sort();
            Console.WriteLine("before " + MaJiangDef.cardsToString(cards));

            List<int> gui = new List<int>();

            int step = 0;
            int[] remain = new int[43] { 0,
                4, 4, 4, 4, 4, 4, 4, 4, 4,
                4, 4, 4, 4, 4, 4, 4, 4, 4,
                4, 4, 4, 4, 4, 4, 4, 4, 4,
                4, 4, 4, 4,
                4, 4, 4,
                1, 1, 1, 1, 1, 1, 1, 1};
            List<int> bannedCards = new List<int>();
            while (total.Count != 0)
            {
                if (HuUtil.isHuExtra(cards, gui, 0))
                {
                    cards.Sort();
                    Console.WriteLine("after " + MaJiangDef.cardsToString(cards));
                    Console.WriteLine("step " + step);
                    break;
                }
                step++;
                int outRet = outAI(cards, gui, remain, bannedCards);
                cards.Remove((int)outRet);
                int tmp = total[0];
                total.RemoveAt(0);
                cards.Add(tmp);
            }
        }

        private static int getHasCount(List<int> input, int obj)
        {
            int result = 0;
            foreach (int i in input)
            {
                if (i == obj)
                    ++result;
            }
            return result;
        }

        public static void main()
        {
            if (true)
            {
                Console.WriteLine("11111111");
                AITableJian.gen();
                AITableFeng.gen();
                Console.WriteLine("22222222");
                AITable.gen();
                Console.WriteLine("33333333");
            }
            testOut();
            testChi();
            testPeng();
            testGang();
            testHu();
        }

    }
}
