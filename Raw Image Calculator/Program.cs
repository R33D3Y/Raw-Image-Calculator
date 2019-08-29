using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raw_Image_Calculator
{
    class Program
    {
        private static string image1 = @"C:\Users\jacks\Desktop\sino800_540x1200.raw";
        private static string image2 = @"C:\Users\jacks\Desktop\sino801_540x1200.raw";

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            BinaryReader br1 = new BinaryReader(File.Open(image1, FileMode.Open));
            BinaryReader br2 = new BinaryReader(File.Open(image2, FileMode.Open));
            int length = (int)br1.BaseStream.Length / sizeof(int);

            using (FileStream stream = new FileStream(@"C:\Users\jacks\Desktop\Result.raw", FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    for (int i = 0; i < length; i++)
                    {
                        int val1 = br1.ReadInt16();
                        int val2 = br2.ReadInt16();
                        int val3 = val1 - val2;

                        if (val3 < 0)
                        {
                            val3 = 65535 + val3;
                        }

                        writer.Write(Convert.ToUInt16(val3));
                    }
                }
            }

            Console.WriteLine("Time Taken: " + sw.Elapsed);
            Thread.Sleep(2000);
        }
    }
}