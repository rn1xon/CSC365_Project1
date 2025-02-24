using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSC365_Project1.Models;

namespace CSC365_Project1
{
    internal class SortOperations
    {

        public SymptomTask2[] NetSort(IEnumerable<SymptomTask2> inputList)
        {
            return inputList.OrderBy(x => x.VAERS_ID).ToArray();
        }

        //-----------------------------------------------------------------------------------------------
        /// <summary>
        /// Use Quick sort approach to order the list by VAERS_ID
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public SymptomTask2[] QuickSort(IEnumerable<SymptomTask2> inputList)
        {
            SymptomTask2[] array = inputList.ToArray();

            Stack<int> stack = new Stack<int>();
            stack.Push(0);
            stack.Push(array.Length - 1);

            while (stack.Count > 0)
            {
                int right = stack.Pop();
                int left = stack.Pop();

                if (left < right)
                {
                    int pivot = Partition(array, left, right);

                    stack.Push(left);
                    stack.Push(pivot - 1);

                    stack.Push(pivot + 1);
                    stack.Push(right);
                }
            }
            return array;
        }

        private int Partition(SymptomTask2[] array, int left, int right)
        {
            // Calculate the middle index for the pivot
            int middle = left + (right - left) / 2;
            int pivot = array[middle].VAERS_ID;
            int i = left - 1;

            // Swap the middle element with the rightmost element
            Swap(array, middle, right);

            for (int j = left; j < right; j++)
            {
                if (array[j].VAERS_ID <= pivot)
                {
                    i++;
                    Swap(array, i, j);
                }
            }

            Swap(array, i + 1, right);
            return i + 1;
        }

        private void Swap(SymptomTask2[] array, int i, int j)
        {
            SymptomTask2 temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }

        //-----------------------------------------------------------------------------------------------

        /// <summary>
        /// Use Insertion sort approach to order the list by VAERS_ID
        /// </summary>
        /// <param name="theList"></param>
        /// <returns></returns>
        public SymptomTask2[] InsertionSort(IEnumerable<SymptomTask2> inputList)
        {
            //List<SymptomTask2> sortedList = new List<SymptomTask2>(inputList);

            SymptomTask2[] sortedList = inputList.ToArray();

            //int count = sortedList.Count
            int count = sortedList.Length;

            for (int i = 1; i < count; i++)
            {
                SymptomTask2 key = sortedList[i];
                int j = i - 1;

                while (j >= 0 && sortedList[j].VAERS_ID > key.VAERS_ID)
                {
                    sortedList[j + 1] = sortedList[j];
                    j--;
                }
                sortedList[j + 1] = key;
            }

            return sortedList;
        }

        //-----------------------------------------------------------------------------------------------

        /// <summary>
        /// Use Merge sort to order the list by VAERS_ID
        /// </summary>
        /// <param name="theList"></param>
        /// <returns></returns>
        public SymptomTask2[] MergeSort(IEnumerable<SymptomTask2> inputList)
        {
            SymptomTask2[] array = inputList.ToArray();
            if (array.Length <= 1)
                return array;

            SymptomTask2[] sortedArray = MergeSort(array, 0, array.Length - 1);
            return sortedArray;
        }

        private SymptomTask2[] MergeSort(SymptomTask2[] array, int left, int right)
        {
            if (left >= right)
                return new SymptomTask2[] { array[left] };

            int mid = left + (right - left) / 2;
            SymptomTask2[] leftArray = MergeSort(array, left, mid);
            SymptomTask2[] rightArray = MergeSort(array, mid + 1, right);

            return Merge(leftArray, rightArray);
        }

        private SymptomTask2[] Merge(SymptomTask2[] leftArray, SymptomTask2[] rightArray)
        {
            int leftSize = leftArray.Length;
            int rightSize = rightArray.Length;
            SymptomTask2[] mergedArray = new SymptomTask2[leftSize + rightSize];

            int i = 0, j = 0, k = 0;

            // Merge the left and right arrays
            while (i < leftSize && j < rightSize)
            {
                if (leftArray[i].VAERS_ID <= rightArray[j].VAERS_ID)
                {
                    mergedArray[k++] = leftArray[i++];
                }
                else
                {
                    mergedArray[k++] = rightArray[j++];
                }
            }

            // Copy any remaining elements from the left array
            while (i < leftSize)
            {
                mergedArray[k++] = leftArray[i++];
            }

            // Copy any remaining elements from the right array
            while (j < rightSize)
            {
                mergedArray[k++] = rightArray[j++];
            }
            return mergedArray;
        }
    }
}
