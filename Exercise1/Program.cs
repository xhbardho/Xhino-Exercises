using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise1
{
    class Program
    {

        static void Main(string[] args)
        {


            Console.WriteLine("Write the first sequence for haystack : ");
            char[] haystack = Console.ReadLine().ToCharArray();
            Console.WriteLine("Write the second sequence for needle  : ");
            char[] needle = Console.ReadLine().ToCharArray();
            Console.WriteLine("Write the value for threshold  : ");
            int threshold = Convert.ToInt32(Console.ReadLine().ToString());
            int haystackLength = haystack.Length;
            int needleLength = needle.Length;
            int th = threshold;
            for (int i = 0; i <= haystackLength - th; i++)
            {
                int poz1 = 0;
                int poz2 = 0;
                for (int j = 0; j < needleLength - th; j++)
                {

                    if (haystack[i] == needle[j])
                    {
                        int counter = 0;
                        poz1 = i; poz2 = j;
                        int l = i;
                        for (int k = j; k < needleLength; k++)
                        {
                            if (haystack[i] == needle[k])
                            {
                                counter++;
                                if (i + 1 < haystackLength)
                                {
                                    i++;

                                }

                            }
                            else
                            {
                                if (counter >= th)
                                {

                                    Console.WriteLine("sequence of length = " + counter + " found at haystack offset " + poz1 + ", needle offset " + poz2);
                                    counter = 0;

                                }


                            }


                        }

                    }
                }




            }

            Console.ReadLine();
        }
    }
}
