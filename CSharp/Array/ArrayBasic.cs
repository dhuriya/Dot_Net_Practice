using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp.Array
{
    public static class ArrayBasic
    {
        public static void CountUnique()
        {
            int[] arr = [1,1,2,2,3,3];
            int count = 1;
            if(arr.Length == 0)
            {
                Console.WriteLine("length is 0");
                return;
            }

            for(int i = 1; i < arr.Length; i++)
            {
                if(arr[i] != arr[i - 1])
                {
                    count++;
                }
            }
            Console.WriteLine("length is " + count);
        }
        public static void RemoveDulicates()
        {
            int [] arr = [1,1,2,2,3,3,3];
            int i =0;
            for(int j= 1; j < arr.Length; j++)
            {
                if(arr[j] != arr[i])
                {
                    i++;
                    arr[i] = arr[j];
                }
            }
            Console.WriteLine(i+1);
        }
        public static void leftRotatebyOne()
        {
            int[] arr= [1,2,3,4,5];
            int temp = arr[0];
            for(int i = 1; i<arr.Length; i++)
            {
                arr[i - 1]= arr[i];
            }
            arr[arr.Length-1] = temp;

            for(int j = 0 ;j< arr.Length; j++)
            {
                Console.Write(j +" ");
            }
        }
    }
}