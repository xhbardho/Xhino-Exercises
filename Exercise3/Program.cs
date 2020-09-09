using System;

namespace Exercise3
{
    class Program
    {
        static void maxSubArraySum(int[] array,
                           int size)
        {
            int max1 = int.MinValue,
            max2 = 0, start = 0,
            end = 0, s = 0;

            for (int i = 0; i < size; i++)
            {
                max2 += array[i];

                if (max1 < max2)
                {
                    max1 = max2;
                    start = s;
                    end = i;
                }

                if (max2 < 0)
                {
                    max2 = 0;
                    s = i + 1;
                }
            }

            Console.Write(" start index: " + start + " end index:  " + end + " and the sum " + max1 + ".");

        }
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the length of the array");
            int n = Convert.ToInt32(Console.ReadLine());
            int[] array = new int[n];
            for (int i = 0; i < n; i++)
            {
                int j = i + 1;
                Console.WriteLine("Enter the " + j + " element of the array");
                array[i] = Convert.ToInt32(Console.ReadLine());
            }
            Console.Write("For vector ");
            for (int i = 0; i < n; i++)
            {
                Console.Write(array[i] + ",");
            }
            maxSubArraySum(array, n);
            Console.Read();

        }
    }
}
