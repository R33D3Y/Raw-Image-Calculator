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
        private static string image1 = @"C:\Somepath"; // Image 1 is found here
        private static string image2 = @"C:\Somepath"; // Image 2 is found here

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            BinaryReader br1 = new BinaryReader(File.Open(image1, FileMode.Open));
            BinaryReader br2 = new BinaryReader(File.Open(image2, FileMode.Open));
            int length = (int)br1.BaseStream.Length / sizeof(int);

            using (FileStream stream = new FileStream(@"C:\Somepath\Result.raw", FileMode.Create)) // Final Image created at set location
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