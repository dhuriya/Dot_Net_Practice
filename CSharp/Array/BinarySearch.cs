using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp.Array
{
    public class BinarySearch
    {
        //----------------------
        // Binary Search -> Binary Search is a searching algorithm that operates on sorted space, repeatedly dividing into halves to
        //                  find target value or optimal answer in logrithmic time O(logN)
        //------------------------
        //---------------------------------
        // Iterative Algorithm: O(log n) Time and O(1) Space
        //------------------------------------
        public static int binarySearch(int[] arr,int x)
        {
            int low = 0;
            int high = arr.Length - 1;
            while(low <= high)
            {
                int mid = low + (high - low)/2;
                if (arr[mid] == x)
                {
                    return mid;
                }else if (arr[mid] > x)
                {
                    low = mid + 1;
                }
                else
                {
                    high = mid - 1;
                }
            }
            return -1;
        }
        //-----------------------------
        // Recursive Algorithm: O(log n) Time and O(Log n) Space
        //------------------------------
        public static int binarySearch2(int[] arr,int x, int low, int high)
        {
            if(high >= low)
            {
                int mid = low + (high - low)/2;
                if(arr[mid] == x)
                    return mid;
                if(arr[mid] > x)
                   return binarySearch2(arr,x,low,mid-1);
                return binarySearch2(arr,x,mid+1,high);
            }
            return -1;
        }
    }
}