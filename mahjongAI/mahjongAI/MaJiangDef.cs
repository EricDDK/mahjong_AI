using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mahjongAI
{
    public class MaJiangDef
    {
        public const int WAN1 = 1;
        public const int WAN2 = 2;
        public const int WAN3 = 3;
        public const int WAN4 = 4;
        public const int WAN5 = 5;
        public const int WAN6 = 6;
        public const int WAN7 = 7;
        public const int WAN8 = 8;
        public const int WAN9 = 9;

        public const int TONG1 = 10;
        public const int TONG2 = 11;
        public const int TONG3 = 12;
        public const int TONG4 = 13;
        public const int TONG5 = 14;
        public const int TONG6 = 15;
        public const int TONG7 = 16;
        public const int TONG8 = 17;
        public const int TONG9 = 18;

        public const int TIAO1 = 19;
        public const int TIAO2 = 20;
        public const int TIAO3 = 21;
        public const int TIAO4 = 22;
        public const int TIAO5 = 23;
        public const int TIAO6 = 24;
        public const int TIAO7 = 25;
        public const int TIAO8 = 26;
        public const int TIAO9 = 27;

        public const int FENG_DONG = 28;
        public const int FENG_NAN = 29;
        public const int FENG_XI = 30;
        public const int FENG_BEI = 31;

        public const int JIAN_ZHONG = 32;
        public const int JIAN_FA = 33;
        public const int JIAN_BAI = 34;

        public const int HUA_CHUN = 35;
        public const int HUA_XIA = 36;
        public const int HUA_QIU = 37;
        public const int HUA_DONG = 38;
        public const int HUA_MEI = 39;
        public const int HUA_LAN = 40;
        public const int HUA_ZHU = 41;
        public const int HUA_JU = 42;

        public const int MAX_NUM = 42;

        public const int TYPE_WAN = 1;
        public const int TYPE_TONG = 2;
        public const int TYPE_TIAO = 3;
        public const int TYPE_FENG = 4;
        public const int TYPE_JIAN = 5;
        public const int TYPE_HUA = 6;

        public static int toCard(int type, int index)
        {
            switch (type)
            {
                case TYPE_WAN:
                    return WAN1 + index;
                case TYPE_TONG:
                    return TONG1 + index;
                case TYPE_TIAO:
                    return TIAO1 + index;
                case TYPE_FENG:
                    return FENG_DONG + index;
                case TYPE_JIAN:
                    return JIAN_ZHONG + index;
                case TYPE_HUA:
                    return HUA_CHUN + index;
            }
            return 0;
        }

        public static string cardsToString(List<int> card)
        {
            string ret = "";
            foreach (int c in card)
            {
                ret += cardToString(c) + ",";
            }
            return ret;
        }

        public static string cardsToString(HashSet<int> card)
        {
            string ret = "";
            foreach (int c in card)
            {
                ret += cardToString(c) + ",";
            }
            return ret;
        }

        public static string cardToString(int card)
        {
            if (card >= WAN1 && card <= WAN9)
            {
                return (card - WAN1 + 1) + "万";
            }
            if (card >= TONG1 && card <= TONG9)
            {
                return (card - TONG1 + 1) + "筒";
            }
            if (card >= TIAO1 && card <= TIAO9)
            {
                return (card - TIAO1 + 1) + "条";
            }

            string[] strs = new string[]
            { "东", "南", "西", "北", "中", "发", "白", "春", "夏", "秋", "冬", "梅", "兰", "竹", "菊" };
            if (card >= FENG_DONG && card <= MAX_NUM)
            {
                return strs[card - FENG_DONG];
            }
            return "错误" + card;
        }

        public static List<int> stringToCards(string str)
        {
            List<int> ret = new List<int>();
            string[] strs = str.Split(',');
            foreach (string s in strs)
            {
                if (s != null && s.Length > 0)
                {
                    ret.Add(stringToCard(s));
                }
            }
            return ret;
        }

        public static int stringToCard(string str)
        {
            if (str.Contains("万"))
            {
                return WAN1 - 1 + int.Parse(str.Substring(0, 1));
            }
            if (str.Contains("筒"))
            {
                return TONG1 - 1 + int.Parse(str.Substring(0, 1));
            }
            if (str.Contains("条"))
            {
                return TIAO1 - 1 + int.Parse(str.Substring(0, 1));
            }

            string[] strs = new string[]
            { "东", "南", "西", "北", "中", "发", "白", "春", "夏", "秋", "冬", "梅", "兰", "竹", "菊" };
            int c = FENG_DONG;
            foreach (string s in strs)
            {
                if (str.Contains(s))
                {
                    return c;
                }
                c++;
            }

            return 0;
        }

        public static int type(int card)
        {
            if (card >= WAN1 && card <= WAN9)
            {
                return TYPE_WAN;
            }
            if (card >= TONG1 && card <= TONG9)
            {
                return TYPE_TONG;
            }
            if (card >= TIAO1 && card <= TIAO9)
            {
                return TYPE_TIAO;
            }
            if (card >= FENG_DONG && card <= FENG_BEI)
            {
                return TYPE_FENG;
            }
            if (card >= JIAN_ZHONG && card <= JIAN_BAI)
            {
                return TYPE_JIAN;
            }
            if (card >= HUA_CHUN && card <= HUA_JU)
            {
                return TYPE_HUA;
            }
            return 0;
        }

        //万通条 1-9，10-18，19-27
        public static int convertToAI(int tileID)
        {
            if (tileID <= 35)
            {
                return (tileID) / 4 + 1;
            }
            else if (tileID <= 71)
            {
                return (tileID) / 4 + 10;
            }
            else if (tileID <= 107)
            {
                return (tileID) / 4 - 8;
            }
            else if (tileID <= 135)
            {
                return (tileID) / 4 + 1;
            }
            else
            {
                return tileID - 101;
            }
        }
        
        public static string convertAIToStr(int card)
        {
            if (card >= WAN1 && card <= WAN9)
            {
                return (card - WAN1 + 1) + "wan";
            }
            if (card >= TONG1 && card <= TONG9)
            {
                return (card - TONG1 + 1) + "tong";
            }
            if (card >= TIAO1 && card <= TIAO9)
            {
                return (card - TIAO1 + 1) + "tiao";
            }

            string[] strs = new string[]
            { "dfeng", "nfeng", "xfeng", "bfeng", "hzong", "fcai", "bban", "hua", "hua", "hua", "hua", "hua", "hua", "hua", "hua" };
            if (card >= FENG_DONG && card <= MAX_NUM)
            {
                return strs[card - FENG_DONG];
            }
            return "";
        }

    }
}
