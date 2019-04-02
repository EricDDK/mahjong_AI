using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mahjongAI
{
    public class HuUtil
    {
        public static bool isHu(List<int> input, int guiCard)
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
            int guiNum = cards[guiCard - 1];
            cards[guiCard - 1] = 0;
            return isHuCard(cards, guiNum);
        }

        public static bool isHuExtra(List<int> input, List<int> guiCard, int extra)
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

            if (extra != 0)
            {
                cards[extra - 1] = cards[extra - 1] + 1;
            }

            return isHuCard(cards, guiNum);
        }

        public static bool isHuCard(List<int> cards, int guiNum)
        {
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

            List<List<HuTableInfo>> tmp = new List<List<HuTableInfo>>();
            if (wan_key != 0)
            {
                List<HuTableInfo> wanHuTableInfo = HuTable.table[wan_key];
                tmp.Add(wanHuTableInfo);
            }
            if (tong_key != 0)
            {
                List<HuTableInfo> tongHuTableInfo = HuTable.table[tong_key];
                tmp.Add(tongHuTableInfo);
            }
            if (tiao_key != 0)
            {
                List<HuTableInfo> tiaoHuTableInfo = HuTable.table[tiao_key];
                tmp.Add(tiaoHuTableInfo);
            }
            if (feng_key != 0)
            {
                List<HuTableInfo> fengHuTableInfo = HuTableFeng.table[feng_key];
                tmp.Add(fengHuTableInfo);
            }
            if (jian_key != 0)
            {
                List<HuTableInfo> jianHuTableInfo = HuTableJian.table[jian_key];
                tmp.Add(jianHuTableInfo);
            }

            List<List<HuTableInfo>> tmp1 = new List<List<HuTableInfo>>();
            foreach (List<HuTableInfo> huTableInfos in tmp)
            {
                if (huTableInfos == null)
                {
                    return false;
                }
                List<HuTableInfo> tmp2 = new List<HuTableInfo>();
                foreach (HuTableInfo huTableInfo in huTableInfos)
                {
                    if (huTableInfo.hupai == null && huTableInfo.needGui <= guiNum)
                    {
                        tmp2.Add(huTableInfo);
                    }
                }
                if (tmp2.Count == 0)
                {
                    return false;
                }
                tmp1.Add(tmp2);
            }

            return isHuTableInfo(tmp1, 0, guiNum, false);
        }

        private static bool isHuTableInfo(List<List<HuTableInfo>> tmp, int index, int guiNum, bool jiang)
        {
            if (index >= tmp.Count)
            {
                return (guiNum % 3 == 0 && jiang == true) || (guiNum % 3 == 2 && jiang == false);
            }
            List<HuTableInfo> huTableInfos = tmp[index];
            foreach (HuTableInfo huTableInfo in huTableInfos)
            {
                if (jiang)
                {
                    if (huTableInfo.hupai == null && huTableInfo.needGui <= guiNum && huTableInfo.jiang == false)
                    {
                        if (isHuTableInfo(tmp, index + 1, guiNum - huTableInfo.needGui, jiang))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (huTableInfo.hupai == null && huTableInfo.needGui <= guiNum)
                    {
                        if (isHuTableInfo(tmp, index + 1, guiNum - huTableInfo.needGui, huTableInfo.jiang))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static List<int> isTing(List<int> input, int guiCard)
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
            int guiNum = cards[guiCard - 1];
            cards[guiCard - 1] = 0;

            return isTingCard(cards, guiNum);
        }

        public static List<int> isTingExtra(List<int> input, List<int> guiCard)
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

            return isTingCard(cards, guiNum);
        }

        public static List<int> isTingCard(List<int> cards, int guiNum)
        {
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

            List<int> tmpType = new List<int>();
            List<List<HuTableInfo>> tmpTing = new List<List<HuTableInfo>>();
            List<List<HuTableInfo>> tmp = new List<List<HuTableInfo>>();

            List<HuTableInfo> wanHuTableInfo = new List<HuTableInfo>();
            if (HuTable.table.ContainsKey(wan_key))
                wanHuTableInfo = HuTable.table[wan_key];
            else
                return new List<int>();
            tmpTing.Add(wanHuTableInfo);
            if (wan_key != 0)
            {
                tmpType.Add(MaJiangDef.TYPE_WAN);
                tmp.Add(wanHuTableInfo);
            }

            List<HuTableInfo> tongHuTableInfo = new List<HuTableInfo>();
            if (HuTable.table.ContainsKey(tong_key))
                tongHuTableInfo = HuTable.table[tong_key];
            else
                return new List<int>();
            tmpTing.Add(tongHuTableInfo);
            if (tong_key != 0)
            {
                tmpType.Add(MaJiangDef.TYPE_TONG);
                tmp.Add(tongHuTableInfo);
            }

            List<HuTableInfo> tiaoHuTableInfo = new List<HuTableInfo>();
            if (HuTable.table.ContainsKey(tiao_key))
                tiaoHuTableInfo = HuTable.table[tiao_key];
            else
                return new List<int>();
            tmpTing.Add(tiaoHuTableInfo);
            if (tiao_key != 0)
            {
                tmpType.Add(MaJiangDef.TYPE_TIAO);
                tmp.Add(tiaoHuTableInfo);
            }

            List<HuTableInfo> fengHuTableInfo = new List<HuTableInfo>();
            if (HuTable.table.ContainsKey(feng_key))
                fengHuTableInfo = HuTable.table[feng_key];
            else
                return new List<int>();
            tmpTing.Add(fengHuTableInfo);
            if (feng_key != 0)
            {
                tmpType.Add(MaJiangDef.TYPE_FENG);
                tmp.Add(fengHuTableInfo);
            }

            List<HuTableInfo> jianHuTableInfo = new List<HuTableInfo>();
            if (HuTable.table.ContainsKey(jian_key))
                jianHuTableInfo = HuTable.table[jian_key];
            else
                return new List<int>();
            tmpTing.Add(jianHuTableInfo);
            if (jian_key != 0)
            {
                tmpType.Add(MaJiangDef.TYPE_JIAN);
                tmp.Add(jianHuTableInfo);
            }

            List<int> ret = new List<int>();
            for (int type = MaJiangDef.TYPE_WAN; type <= MaJiangDef.TYPE_JIAN; type++)
            {
                List<HuTableInfo> huTableInfos = tmpTing[type - 1];
                int[] cache = new int[9];
                foreach (HuTableInfo huTableInfo in huTableInfos)
                {
                    if (huTableInfo.hupai != null && huTableInfo.needGui <= guiNum)
                    {
                        bool cached = true;
                        for (int j = 0; j < huTableInfo.hupai.Length; j++)
                        {
                            if (huTableInfo.hupai[j] > 0 && cache[j] == 0)
                            {
                                cached = false;
                                break;
                            }
                        }

                        if (!cached && isTingHuTableInfo(tmpType, tmp, 0, guiNum - huTableInfo.needGui, huTableInfo.jiang,
                                type))
                        {
                            for (int j = 0; j < huTableInfo.hupai.Length; j++)
                            {
                                if (huTableInfo.hupai[j] > 0)
                                {
                                    if (cache[j] == 0)
                                    {
                                        ret.Add(MaJiangDef.toCard(type, j));
                                    }
                                    cache[j]++;
                                }
                            }
                        }
                    }
                }
            }
            return ret;
        }

        private static bool isTingHuTableInfo(List<int> tmpType, List<List<HuTableInfo>> tmp, int index, int guiNum,
                bool jiang, int tingType)
        {
            if (index >= tmp.Count)
            {
                return guiNum == 0 && jiang == true;
            }
            if (tmpType[index] == tingType)
            {
                return isTingHuTableInfo(tmpType, tmp, index + 1, guiNum, jiang, tingType);
            }
            List<HuTableInfo> huTableInfos = tmp[index];
            foreach (HuTableInfo huTableInfo in huTableInfos)
            {
                if (huTableInfo.hupai == null && huTableInfo.needGui <= guiNum)
                {
                    if (jiang)
                    {
                        if (huTableInfo.jiang == false)
                        {
                            if (isTingHuTableInfo(tmpType, tmp, index + 1, guiNum - huTableInfo.needGui, jiang, tingType))
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (isTingHuTableInfo(tmpType, tmp, index + 1, guiNum - huTableInfo.needGui, huTableInfo.jiang,
                                tingType))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static void gen()
        {
            HuTableJian.gen();
            HuTableFeng.gen();
            HuTable.gen();
        }

        public static void load()
        {
            HuTableJian.load();
            HuTableFeng.load();
            HuTable.load();
        }

        public static void testHu()
        {
            string init = "1万,1万";
            string gui = "1万";
            List<int> cards = MaJiangDef.stringToCards(init);
            Console.WriteLine(HuUtil.isHu(cards, MaJiangDef.stringToCard(gui)));
        }

        public static void testTing()
        {
            string init = "1万,1万,1筒,3筒,2筒,2条,3条,4条,东,东";
            string gui = "1筒";
            List<int> cards = MaJiangDef.stringToCards(init);
            Console.WriteLine(MaJiangDef.cardsToString(HuUtil.isTing(cards, MaJiangDef.stringToCard(gui))));
            Console.WriteLine(MaJiangDef.cardsToString(HuUtil.isTingExtra(cards, MaJiangDef.stringToCards(gui))));
        }

        public static void main()
        {
            load();
            testHu();
            testTing();
        }
    }
}
