using System;
using System.Collections.Generic;

namespace Nusstudios.Core
{
    public static class ArrayUtil
    {
        public static T[][] SplitArray<T>(T[] array, T delimiter, bool removeEmptyEntries) => SplitArray(array, new T[] { delimiter }, removeEmptyEntries);
        public static int IndexOfSubArray<T>(T[] array, T[] subarray) => IndexOfSubArray(array, subarray, 0);
        public static int LastIndexOfSubArray<T>(T[] array, T[] subarray) => LastIndexOfSubArray(array, subarray, array.Length - 1);

        public static int Partition<T>(T[] array, int start, int end, bool incrOrd) where T : IComparable
        {
            int place = start;
            T tmp;

            for (int i = start; i < end; i++)
            {
                if (incrOrd ? array[i].CompareTo(array[end]) <= 0 : array[i].CompareTo(array[end]) > 0)
                {
                    if (i != place)
                    {
                        tmp = array[i];
                        array[i] = array[place];
                        array[place] = tmp;
                    }

                    place++;
                }
            }

            tmp = array[place];
            array[place] = array[end];
            array[end] = tmp;
            return place;
        }

        public static void QuickSort<T>(T[] array, int start, int end, bool incrOrd) where T : IComparable
        {
            if (start >= end)
                return;

            int pivot = Partition(array, start, end, incrOrd);
            QuickSort(array, start, pivot - 1, incrOrd);
            QuickSort(array, pivot + 1, end, incrOrd);
        }

        public static void MinHeapify<T>(T[] array, int i, int len) where T : IComparable
        {
            int smallest = i;

            if (i * 2 + 1 < len && smallest.CompareTo(array[i * 2 + 1]) > 0)
                smallest = i * 2 + 1;

            if (i * 2 + 2 < len && smallest.CompareTo(array[i * 2 + 2]) > 0)
                smallest = i * 2 + 2;

            if (smallest != i)
            {
                T tmp = array[i];
                array[i] = array[smallest];
                array[smallest] = tmp;
                MinHeapify(array, i, len);
            }
        }

        public static void MaxHeapify<T>(T[] array, int i, int len) where T : IComparable
        {
            int largest = i;

            if (i * 2 + 1 < len && largest.CompareTo(array[i * 2 + 1]) < 0)
                largest = i * 2 + 1;

            if (i * 2 + 2 < len && largest.CompareTo(array[i * 2 + 2]) < 0)
                largest = i * 2 + 2;

            if (largest != i)
            {
                T tmp = array[i];
                array[i] = array[largest];
                array[largest] = tmp;
                MaxHeapify(array, i, len);
            }
        }

        public static void HeapSort<T>(T[] array, bool incrOrd) where T : IComparable
        {
            for (int i = array.Length / 2 - 1; i >= 0; i--) {
                if (incrOrd)
                    MaxHeapify(array, i, array.Length);
                else
                    MinHeapify(array, i, array.Length);
            }

            for (int i = array.Length - 1; i > 0; i++)
            {
                T tmp = array[i];
                array[i] = array[0];
                array[0] = tmp;

                if (incrOrd)
                    MaxHeapify(array, 0, i);
                else
                    MinHeapify(array, 0, i);
            }
        }

        public static void Merge<T>(T[] a, int b, int c, int cend, bool incrOrd) where T : IComparable
        {
            int blen = c - b;
            int clen = cend - c + 1;
            T[] d = new T[clen + blen];
            int bi = b, ci = c, di = 0;

            while (bi < c && ci < cend + 1)
            {
                if (incrOrd ? a[bi].CompareTo(a[ci]) < 0 : a[bi].CompareTo(a[ci]) > 0)
                    d[di++] = a[bi++];
                else
                    d[di++] = a[ci++];
            }

            if (c > bi)
                Array.Copy(a, bi, d, di, c - bi);
            else if (cend + 1 > ci)
                Array.Copy(a, ci, d, di, cend + 1 - ci);

            Array.Copy(d, 0, a, b, d.Length);
        }

        public static void MergeSort<T>(T[] array, int start, int end, bool incrOrd) where T : IComparable
        {
            if (start == end)
                return;

            int hsz = (end - start) / 2;
            MergeSort(array, start, start + hsz, incrOrd);
            MergeSort(array, start + hsz + 1, end, incrOrd);
            Merge(array, start, start + hsz + 1, end, incrOrd);
        }

        public static void MergeSort<T>(T[] array, bool incrOrd) where T : IComparable => MergeSort(array, 0, array.Length - 1, incrOrd);

        public static int[] TBL<T>(T[] array)
        {
            int[] tbl = new int[array.Length];
            int lps, i;
            i = (tbl[0] = lps = 0) + 1;

            while (i < array.Length)
            {
                if (Equals(array[i], array[lps]))
                    tbl[i++] = ++lps;
                else if (lps == 0)
                    tbl[i++] = lps;
                else
                    lps = tbl[lps - 1];
            }

            return tbl;
        }

        public static int KMPFI<T>(T[] array, int from, T[] delimiter, int[] tbl)
        {
            int ptrni = 0, i = from;

            while (i < array.Length)
            {
                if (Equals(array[i], delimiter[ptrni]))
                {
                    if (ptrni == delimiter.Length - 1)
                        return i - ptrni;
                    else
                    {
                        ptrni++;
                        i++;
                    }
                }
                else if (ptrni != 0)
                    ptrni = tbl[ptrni - 1];
                else
                    i++;
            }

            return -1;
        }

        public static T[][] KMPSplit<T>(T[] array, T[] delimiter, bool removeEmptyEntries)
        {
            int[] tbl = TBL(delimiter);
            List<T[]> result = new List<T[]>();
            int from = 0;
            int match;

            while ((match = KMPFI(array, from, delimiter, tbl)) != -1)
            {
                if (match == from && !removeEmptyEntries)
                    result.Add(new T[0]);
                else
                    result.Add(SubArray(array, from, match - from));

                from = match + delimiter.Length;
            }

            if (match < array.Length - 1)
                result.Add(SubArray(array, from));

            return result.ToArray();
        }

        public static int[] KMP<T>(T[] array, T[] delimiter, bool removeEMptyEntries)
        {
            int ptrni = 0, i = 0;
            List<int> occurrences = new List<int>();
            int[] tbl = TBL(array);

            while (i < array.Length)
            {
                if (Equals(array[i], delimiter[ptrni]))
                {
                    if (ptrni == delimiter.Length - 1)
                    {
                        occurrences.Add(i - ptrni);
                        ptrni = tbl[ptrni];
                    }

                    i++;
                    ptrni++;
                }
                else if (ptrni == 0)
                    i++;
                else
                    ptrni = tbl[ptrni - 1];
            }

            return occurrences.ToArray();
        }

        public static T[][] SplitArray<T>(T[] array, T[] delimiter, bool removeEmptyEntries)
        {
            int toPlus1 = -1;
            int from = 0;
            List<T[]> split = new List<T[]>();

            while ((toPlus1 = IndexOfSubArray(array, delimiter, toPlus1 + 1)) != -1)
            {
                if (!(((toPlus1 == from + delimiter.Length) ^ (toPlus1 == 0 && from == 0)) && removeEmptyEntries))
                {
                    T[] splitpart = new T[toPlus1 - from];
                    Array.Copy(array, from, splitpart, 0, splitpart.Length);
                    split.Add(splitpart);
                }

                from = toPlus1 + delimiter.Length;

                if (from >= array.Length) break;
            }

            if (split.Count == 0) return new T[1][] { array };
            else if (array.Length > from)
            {
                T[] splitpart = new T[array.Length - from];
                Array.Copy(array, from, splitpart, 0, splitpart.Length);
                split.Add(splitpart);

            }

            return split.ToArray();
        }

        public static T[] SubArray<T>(T[] array, int index)
        {
            T[] sub = new T[array.Length - index];
            Array.Copy(array, index, sub, 0, sub.Length);
            return sub;
        }

        public static T[] SubArray<T>(T[] array, int index, int length)
        {
            T[] sub = new T[length];
            Array.Copy(array, index, sub, 0, length);
            return sub;
        }

        public static int LastIndexOfSubArray<T>(T[] array, T[] subarray, int index)
        {
            int x;
            int match = 0;

            for (int i = index; (i + 1) >= subarray.Length; i--)
            {
                for (x = subarray.Length; 1 <= x; x--)
                {
                    if (Equals(array[i - (subarray.Length - x)], subarray[x - 1])) match++;
                    else break;
                }

                if (match == subarray.Length) return i;
                else match = 0;
            }

            return -1;
        }

        public static int IndexOfSubArray<T>(T[] array, T[] subarray, int index)
        {
            int x;
            int match = 0;

            for (int i = index; (array.Length - (i + 1)) >= subarray.Length; i++)
            {
                for (x = 0; x < subarray.Length; x++)
                {
                    if (Equals(array[i + x], subarray[x])) match++;
                    else break;
                }

                if (match == subarray.Length) return i;
                else match = 0;
            }

            return -1;
        }
    }
}
