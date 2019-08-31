using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raw_Image_Calculator
{
    class Program
    {
        private static string filePath1 = @"C:\Users\jacks\Desktop\sino801_540x1200.raw"; // Image 1 is found here
        private static string filePath2 = @"C:\Users\jacks\Desktop\Test.raw"; // Image 2 is found here
        private static string filePath3 = @"C:\Users\jacks\Desktop\Result4.raw"; // Image 3 is created here

        static async Task Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Console.WriteLine("Processing...");

            List<UInt16> image1 = await ReadAsync(filePath1);
            Console.WriteLine("Image 1 Processed " + sw.Elapsed);

            List<UInt16> image2 = await ReadAsync(filePath2);
            Console.WriteLine("Image 2 Processed " + sw.Elapsed);

            List<UInt16> image3 = new List<UInt16>();

            for (int i = 0; i < image1.Count; i++)
            {
                int val = image1[i] - image2[i];

                if (val < 0)
                {
                    val = 65535 + val;
                }

                image3.Add((UInt16)val);
            }

            Console.WriteLine("Image 3 Calculated " + sw.Elapsed);

            await WriteAsync(filePath3, image3);
            Console.WriteLine("Image 3 Created " + sw.Elapsed);

            Thread.Sleep(2000);
        }

        private static async Task<List<UInt16>> ReadAsync(string filePath)
        {
            using (FileStream sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
            {
                List<UInt16> temp = new List<UInt16>();

                using (BinaryReader reader = new BinaryReader(sourceStream))
                {
                    byte[] buffer = new byte[1];
                    int numRead;

                    while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                    {
                        temp.Add(reader.ReadUInt16());
                    }
                }

                return temp;
            }
        }

        private static async Task WriteAsync(string filePath, List<UInt16> list)
        {
            using (FileStream sourceStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                //BinaryFormatter bf = new BinaryFormatter();
                //MemoryStream ms = new MemoryStream();
                //bf.Serialize(ms, list);
                //byte[] encodedText = ms.ToArray();

                byte[] encodedText = list.SelectMany(BitConverter.GetBytes).ToArray();

                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }

        private void NonAsyncMethod()
        {
            BinaryReader br1 = new BinaryReader(File.Open(filePath1, FileMode.Open));
            BinaryReader br2 = new BinaryReader(File.Open(filePath2, FileMode.Open));
            int length = (int)br1.BaseStream.Length / sizeof(int);

            using (FileStream stream = new FileStream(filePath3, FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    for (int i = 0; i < length; i++)
                    {
                        int val1 = br1.ReadUInt16();
                        int val2 = br2.ReadUInt16();
                        int val3 = val1 - val2;

                        if (val3 < 0)
                        {
                            val3 = 65535 + val3;
                        }

                        writer.Write(Convert.ToUInt16(val3));
                    }
                }
            }
        }
    }
}