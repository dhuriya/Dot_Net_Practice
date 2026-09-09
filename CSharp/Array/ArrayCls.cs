using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    }
}