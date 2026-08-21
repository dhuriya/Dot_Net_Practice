using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp.Array
{
    public static class SubArray
    {
        public static void PrintAllSubArray()
        {
            int[] arr = [1,2,3,4,5];
            for(int i = 0; i < arr.Length; i++)
            {
                for(int j = i; j <arr.Length; j++)
                {
                    for(int k = i; k <= j; k++)
                    {
                        Console.Write(arr[k] + " ");    
                    }
                    Console.WriteLine();
                }
            }
        }
        public static void maxSumSubArray()
        {
            int[] arr = [3,-4,5,4,-1,7,-8];
            int maxSum = int.MinValue;
            for(int i = 0; i < arr.Length; i++)
            {
                int currSum = 0;
                for(int j = i; j<arr.Length;j++){
                    currSum += arr[j];
                    maxSum = Math.Max(currSum,maxSum);
                }
            }
            Console.WriteLine(maxSum);
        }
        public static void KadansAlog()
        {
            int[] arr = [3,-4,5,4,-1,7,-8];
            int currSum = 0,maxSum = int.MinValue;
            for(int i = 0; i < arr.Length; i++)
            {
                currSum +=arr[i];
                maxSum = Math.Max(currSum,maxSum);
                if(currSum < 0)
                {
                    currSum = 0;
                }
            }
            Console.WriteLine($"max sum of sumarray : {maxSum}");
        }
        public static void SplitArrayTwoSubArray()
        {
            int[] arr = [4, 3, 2, 1];
            int totalSum = int.MinValue;
            for(int i = 0; i < arr.Length; i++)
            {
                totalSum +=arr[i];
            }
            int leftSum = int.MinValue;
            for(int i = 0; i < arr.Length; i++)
            {
                leftSum +=arr[i];
                int rightSum = totalSum - leftSum;
                if(leftSum == rightSum)
                {
                    Console.WriteLine("true");
                    return;
                }

            }
            Console.WriteLine("False");
        }
        public static  void checkProductSubArray()
        {
            
            int[] arr = {2, 0, 4, 5};int K = 20;
            for(int i = 0; i < arr.Length; i++)
            {
                int product = 1;
                for(int j = i; j<arr.Length;j++){
                     product *= arr[j];  
                    if(product == K)
                    {
                        Console.WriteLine("ture");
                        return;
                    }
                }
            }
            Console.Write("False");

        }
        public static void Subarray_of_size_k_with_given_sum()
        {
            int[] arr = {1, 4, 2, 10, 2, 3, 1, 0, 20};
            int k = 4, sum = 18;
            for(int i = 0; i < arr.Length-k; i++)
            {
                int sumSubArray = 0;
                for(int j = 0; j < k; j++)
                {
                    sumSubArray += arr[i+j];
                }
                if(sum == sumSubArray)
                {
                    Console.WriteLine("True");
                    return;
                }
            }
            Console.WriteLine("False");
        }
        public static void SortArray()
        {
            int[] arr = { 1, 2, 6, 5, 4, 3, 7, 8 };

            int start = 0;
            int end = arr.Length - 1;

            // Find where increasing order breaks
            while (start < arr.Length - 1 && arr[start] <= arr[start + 1])
            {
                start++;
            }

            // Find where increasing order starts again
            end = start;

            while (end < arr.Length - 1 && arr[end] >= arr[end + 1])
            {
                end++;
            }

            // Reverse the subarray
            while (start < end)
            {
                int temp = arr[start];
                arr[start] = arr[end];
                arr[end] = temp;

                start++;
                end--;
            }

            Console.WriteLine(string.Join(" ", arr));
        }
    }
}