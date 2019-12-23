using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = File.ReadAllText(@"D:\osu! Songs\65853 Blue Stahli - Shotgun Senorita (Zardonic Remix) [no video]\Blue Stahli - Shotgun Senorita (Zardonic Remix) (Aleks719) [Insane].osu");
            text = text.Substring(text.IndexOf("[HitObjects]") + 12).Trim();
            string[] rows = text.Split('\n');
            string[] firstRow = rows[0].Split(',');
            string[] lastRow = rows[rows.Length - 1].Split(',');
            int length = Convert.ToInt32(lastRow[2]) - Convert.ToInt32(firstRow[2]);
            length = length / 1000;
            Console.WriteLine(length);
            Console.ReadKey();
        }
    }
}
