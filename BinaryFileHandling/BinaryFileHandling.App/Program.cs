using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace BinaryFileHandling.App
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] logo = File.ReadAllBytes("white.bmp"); //Currently loading white.bmp for steganography
            byte first = logo[0];
            byte second = logo[1];

            char b = 'B';
            char m = 'M';

            int res1 = first ^ (byte)b;
            int res2 = second ^ (byte)m;

            if (res1 == 0 && res2 == 0)
                Console.WriteLine("This is a .bmp image");
            else 
                Console.WriteLine("This is not a .bmp imgae");


            byte sizeUnreliable = logo[2];

            Console.WriteLine("\nThe approximate size of the bmp is {0} bytes", sizeUnreliable.ToString());

            byte offset = logo[10];
            Console.WriteLine("\nThe offset of the image data is {0} bytes", offset.ToString());

            byte width = logo[18];
            byte height = logo[22];

            
            Console.WriteLine("\n{0} pixels is the width of the image",width.ToString());
            Console.WriteLine("{0} pixels is the height of the image",height.ToString());

            byte planeNumber = logo[26];

            Console.WriteLine("\nThe number of planes in the image is " + planeNumber.ToString());


            byte bitDepth = logo[28];

            Console.WriteLine("\nThe Bit Depth of the image is {0} bits", bitDepth.ToString());

            byte compressionType = logo[30];

            Console.WriteLine("\nThe Compression Type of the image is {0}", compressionType.ToString());
            //Console.WriteLine("which means the image was compressed using Huffman 1D");

            //byte imageDataSize1 = logo[34];
            //byte imageDataSize2 = logo[35];
            //byte imageDataSize3 = logo[36];
            //byte imageDataSize4 = logo[37];

            //Console.WriteLine("\nThe imagedata1 of the image is {0}", imageDataSize1.ToString());
            //Console.WriteLine("The imagedata2 of the image is {0}", imageDataSize2.ToString());
            //Console.WriteLine("The imagedata3 of the image is {0}", imageDataSize3.ToString());
            //Console.WriteLine("The imagedata4 of the image is {0}", imageDataSize4.ToString());

            //I cant figure out what these values mean under these context

            byte numberColours = logo[46];

            Console.WriteLine("\nThe number of colours of the image is {0} - ends up this is normal sometimes??", numberColours.ToString());

            byte numberImportantColours = logo[50];


            Console.WriteLine("\nThe number of important colours of the image is {0} - also normal if all colours are used?\n", numberImportantColours.ToString());

            EncodeText("hello",logo,offset);
            File.WriteAllBytes("WOW.bmp", logo);
            Console.WriteLine(DecodeText(logo, offset));
            
            Console.ReadKey();

        }

        static void EncodeText(string text, byte[] imageData, int offset) //This was so painful with the other image, so i used a white one
        {
            foreach (char c in text)
            {
                for (int i = 0; i < 16; ++i) //char utf16
                {
                    int bit = c & (int)Math.Pow(2, i);
                    byte mask = (byte)(bit == 0 ? 0xFE : 0xFF);
                    imageData[offset] &= mask;
                    ++offset;
                }
            }

        }
        static string DecodeText(byte[] imageData, int offset) //should output garbage data as well, but at the start the text should be there
        {
            int bitIndex = 0;
            char c = '\0';
            string result ="";
            for (; offset < imageData.Length; ++offset)
            {
                int bit = imageData[offset] & 1;
                if (bit == 1)
                { 
                    char mask = (char)Math.Pow(2, bitIndex);
                    c |= mask;
                }
                ++bitIndex;
                if (bitIndex == 16)
                {
                    bitIndex = 0;
                    result += c;
                    c = '\0';
                }

                
            }
            return result;
        }
    }
}
