﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mahjongAI
{
    class Program
    {
        static void Main(string[] args)
        {
            AIEntry.RegisterAI();  // register the AI module
            AICommonDynamic.Register();
            AIUtil.main();  // test case

            Console.ReadLine();
        }
    }
}
