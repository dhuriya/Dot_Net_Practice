using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Array
{
    public static class ArrayCls
    {
        public static void AlternatElement()
        {
            int [] arr = {10, 20, 30, 40, 50};
            for(int i = 0; i < arr.Length;i +=2)
            {
                Console.Write(arr[i] + " ");
            }
        }
        public static void alternateByRecusion(int i)
        {
            int [] arr = {10, 20, 30, 40, 50};
            if(i> arr.Length)
            {
                return;
            }
            Console.Write(arr[i] + " ");
            i +=2;
            alternateByRecusion(i);
        }
        public static void LeadersinArray()
        {
            int[] arr = {16, 17, 4, 3, 5, 2};
            for(int i = 0; i < arr.Length; i++)
            {
                int j;
                for(j = i+1; j < arr.Length; j++)
                {
                    if (arr[i] < arr[j])
                    {
                        break;
                    }
                }
                if(j == arr.Length)
                {
                    Console.Write(arr[i]+ " ");   
                }
            }
        }
        public static void Leader()
        {
            List<int> result = new List<int>();
            int[] arr = {16, 17, 4, 3, 5, 2};
            int n = arr.Length;
            int maxLeader = arr[n-1];
            result.Add(maxLeader);
            for(int i = n-2; i >= 0; i--)
            {
                if (arr[i] >= maxLeader)
                {
                    maxLeader = arr[i];
                    result.Add(maxLeader);
                }
            }
            result.Reverse();
            for(int i = 0; i < result.Count; i++)
            {
                Console.Write(result[i] + " ");
            }
        }
        public static void RemoveDuplicate()
        {
            int[] arr = {1, 2, 2, 3, 4, 4, 4, 5, 5};
            for(int i = 0; i < arr.Length; i++)
            {
                int j;
                //int Count=0;
                for(j = i + 1; j < arr.Length; j++)
                {
                    if(arr[i] == arr[j])
                    {
                        //Count++;
                        break;
                    }
                }
                if(j == arr.Length)
                {
                    Console.Write(arr[i] + " ");
                }
            }
        }
        public static void RemoveDuplicate2()
        {
            List<int> result = new List<int>();
            HashSet<int> du = new HashSet<int>();
            int[] arr = {2, 2, 2, 2};
            for(int i = 0; i < arr.Length; i++)
            {
                du.Add(arr[i]);
            }
            // int n = arr.Length;
            // int duplicate = arr[0];
            // result.Add(duplicate);
            // for(int i = 1; i <arr.Length; i++)
            // {
            //     if (arr[i] != duplicate)
            //     {
            //         duplicate = arr[i];
            //         result.Add(duplicate);
            //     }
            // }
            // //result.Reverse();
            // for(int i = 0; i < result.Count; i++)
            // {
            //     Console.Write(result[i] + " ");
            // }
            for(int i = 0; i < du.Count; i++)
            {
                Console.Write(du.ElementAt(i) + " ");
            }
        }
        public static void allSubArray()
        {
            int[] arr = {1, 2, 3};
            for(int i = 0; i < arr.Length; i++)
            {
                for(int j = i; j < arr.Length; j++)
                {
                    for(int k = i;k<=j;k++){
                        Console.Write(arr[k]+" ");
                    }
                    Console.WriteLine();
                }
            }
        }
        public static void ReverseArray()
        {
            int[] arr = {1, 4, 3, 2, 6, 5};
            int n = arr.Length;
            PrintArray(arr);
            for(int i = 0; i < n/2; i++)
            {
                int temp = arr[i];
                arr[i] = arr[n-i-1];
                arr[n-i-1]=temp;
            }
            PrintArray(arr);
        }
        private static void PrintArray(int[] arr)
        {
            for(int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i]+" ");
            }
            Console.WriteLine();
        }
        public static void findMinMax()
        {
            int[] numbers = {10,30,5,2,50};
            int min= numbers[0];
            int max=numbers[0];
            for(int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] < min)
                {
                    min=numbers[i];
                }
                if (numbers[i] > max)
                {
                    max = numbers[i];
                }
            }
            Console.WriteLine("Min = " + min);
            Console.WriteLine("Max = " + max);
        }
        public static void SumOfArray()
        {
            int[] numbers = {10,20,30,40};
            int sum =0;
            for(int i = 0; i < numbers.Length; i++)
            {
                sum +=numbers[i];
            }
            Console.WriteLine("Sum of Array = "+ sum);
            // Sum Through LINQ
            int sumLinq = numbers.Sum();
        }
        public static void InsertInArray()
        {
            int[] numbers = {10,20,30,40};
            int positon = 2; 
            int value =25;
            // 1 create a new array and size will depend on existig array size
            // + howmany values we want to insert
            PrintArray(numbers);
            int[] result = new int[numbers.Length + 1];
            //2 first loop itreat position
            for(int i =0; i < positon; i++)
            {
                result[i] = numbers[i];
            }
            result[positon] = value;
            for(int i = positon; i< numbers.Length; i++)
            {
                result[i + 1]= numbers[i];
            }
            PrintArray(result);
        }
        public static void DeleteInArray()
        {
            int[] numbers = {10,20,30,40,50};
            int position = 2;
            PrintArray(numbers);
            int[] result = new int[numbers.Length - 1];

            for(int i = 0;i<position ; i++)
            {
                
                result[i] = numbers[i];
            }
            for(int i = position; i< result.Length; i++)
            {
                result[i] = numbers[i+1];
            }
            PrintArray(result);
        }
        public static void elementFreqency()
        {
            int[] numbers = {10,20,10,30,10,20};
            Dictionary<int,int> frequency = new();

            foreach(int number in numbers)
            {
                if (frequency.ContainsKey(number))
                {
                    frequency[number]++;
                }
                else
                {
                    frequency[number]=1;
                }
            }
            foreach(var item in frequency)
            {
                Console.WriteLine(item.Key + " -> "+ item.Value);
            }
        }
        public static void FizzBuzz()
        {
            for(int i = 1;i<=50;i++)
            {
                if(i % 3==0 && i % 5 == 0)
                {
                    Console.WriteLine(i + " FizzBuzz");
                }else if(i % 3 == 0)
                {
                    Console.WriteLine(i + " Fizz");
                }else if(i % 5 == 0)
                {
                    Console.WriteLine(i + " Buzz");
                }
            }
        }
        public static void reverseInteger()
        {
            int number =  12345;
            int reverse = 0;
            while(number != 0)
            {
                int digit = number % 10;
                reverse = reverse * 10 + digit;
                number = number / 10;
            }
            Console.WriteLine(reverse);
        }
        public static void palindrome()
        {
            int number = -121;
            int original = number;
            int reverse = 0;
            while(number != 0)
            {
                int digit = number % 10;
                reverse = reverse * 10 + digit;
                number = number / 10;
            }
            if(original == reverse)
            {
                Console.WriteLine("Number is Palindrome");
            }
            else
            {
                Console.WriteLine("Number is not Palindrome");
            }
        }
        public static void countDigit()
        {
            int number = 12345;
            int count = 0;
            if(number == 0)
            {
                count = 1;
            }else if(number <= 0)
            {
                number = Math.Abs(number);
                while(number != 0)
                {
                    number = number / 10;
                    count++;
                }
            }
            else
            {
                while(number != 0)
                {
                    number = number / 10;
                    count++;
                }
            }
            Console.WriteLine("Digit Count " + count);
        }
        public static void Sumofdigit()
        {
            int number = 345;
            int sum = 0;
            while(number != 0)
            {
                int digit = number % 10;
                sum = sum + digit;
                number = number / 10;
            }
            Console.WriteLine("Sum of Digit " +sum);
        }
        public static void factorial()
        {
            int number = 6;
            int fac = 1;
            // while(number != 0)
            // {
            //     int digit = number % 10;
            //     fac = fac * digit;
            //     number =number / 10;
            // }
            for(int i = 1; i<=number; i++)
            {
                fac = fac * i;
            }
            Console.WriteLine("Factorial " + fac);
        }
        public static void fibonacci()
        {
            int n = 10;
            int first = 0;
            int second = 1;
            for(int i = 0; i< n; i++)
            {
                Console.Write(first + " ");
                int next = first + second;
                first = second;
                second = next;
            }
        }
        public static void RomanToInt() 
        {
            string s = "MCMXCIV";
            Dictionary<string, int> roman = new Dictionary<string, int>
            {
                { "I", 1 },{ "V", 5 },{ "X", 10 },{"L", 50},{"C", 100},{"D", 500},{"M", 1000}
            };
            int size = s.Length;
            int number=0;
            for(int i = 0; i< size; i++)
            {
                int value = roman[s[i].ToString()];
                if(i + 1 < size && roman[s[i+1].ToString()] > value)
                {
                    Console.WriteLine("Value inside if: " + value);
                    number = number - value;
                }
                else
                {
                    Console.WriteLine("Value else: " + value);
                    number += value;   
                }
                
            }
            Console.WriteLine("Roman to Integer: " + number);
        }
        public static void reverseString()
        {
            string str="madam";
            StringBuilder reverse = new StringBuilder();
            for(int i = str.Length-1; i>= 0; i--)
            {
                reverse.Append(str[i]);
            }
            if(str == reverse.ToString())
            {
                Console.WriteLine("Palindrome");    
            }
            Console.WriteLine(reverse);
        }
    }
}