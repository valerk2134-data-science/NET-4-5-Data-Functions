using MathFunctionsNamespace;
using ReportFunctionsNamespace;
using System;
using System.Linq;
using System.Text;
using StringFunctionsNamespace;
using System.Collections.Generic;
using System.Diagnostics;
using AudioFileFunctions;
using NetworkFunctionsNamespace;
using FileFunctionsNamespace;
using static ArrayFunctionsNamespace.ArrayFunctions;
using System.IO;




namespace ArrayFunctionsNamespace
{



    public static class Int32ArrayFunctions
    {
        /// <summary>
        /// For generating random array.
        /// </summary>
        static Random _internal_random = new Random();



        // template for code execution. Warsaw. Workplace. 2024-07-16 13-48. 
        /*
        float execution_time_ms_start = 0;
        if (TimeExecutionShow == true)
            {
                execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
            }
        if (TimeExecutionShow == true)
            {
                float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                TimeExecutionMessage(nameof(function_name_here), execution_time_ms_stop - execution_time_ms_start);
            }
        */

        /// <summary>       
        /// Execution speed measurement. Set true to see the execution time in console.
        /// </summary>
        public static bool TimeExecutionShow
        {
            get
            {
                return _time_execution_bool;
            }

            set
            {
                if (value == true)
                {
                    _time_execution_bool = true;
                    _time_execution.Start();
                    return;
                }
                if (value == false)
                {
                    _time_execution_bool = false;
                    _time_execution.Stop();
                    _time_execution.Reset();
                    return;

                }
            }

            // <info>
            // Warsaw. Workplace. 2024-07-16 13-48.
            // </info>
        }
        static bool _time_execution_bool = false;
        static Stopwatch _time_execution = new Stopwatch();
        static Int32 _time_execution_count = 0;

        static void TimeExecutionMessage(string function_name, float total_ms_passed)
        {
            _time_execution_count += 1;
            Console.WriteLine(_time_execution_count.ToString() + ". " + DateTime.Now.ToString("HH:mm:ss") + " " + function_name +
                " exectuion time: " + total_ms_passed.ToString("0.000") + " ms");
        }


        public static Int32[][] Shift(Int32[][] array_in, Int32 column_shift, Int32 row_shift, Int32 missing_value = 0)
        {
            Int32[][] array_out = new Int32[array_in.Length][];

            for (int c = 0; c < array_in.Length; c++)
            {
                array_out[c] = new Int32[array_in[c].Length];
                for (int r = 0; r < array_in.Length; r++)
                {
                    array_out[c][r] = missing_value;
                }
            }

            int c_index = 0;
            int c_length = 0;

            if (column_shift >= 0)
            {
                c_index = column_shift;
                c_length = array_in.Length;
            }
            else
            {
                c_index = 0;
            }
            // Warsaw. Workplace. 2024-07-17 14-08. 
            // need i, j for second array.
            for (int c = c_index; c < c_length; c++)
            {
                array_out[c] = new Int32[array_in[c].Length];
                for (int r = (int)row_shift; r < array_in.Length; r++)
                {
                    array_out[c][r] = array_in[c][r];
                }
            }
            return array_out;

            // Written. Warsaw. Workplace. 2024-07-17 13-55. 
            // not finished.

        }



        public static class Insert
        {
            // Written. Warsaw. Workplace. 2024-07-17 12-55. 
            public static Int32[][] AInBWithBLevel(Int32[][] array_a, Int32[][] array_b)
            {
                Int32[][] array_return = new Int32[array_b.Length][];
                for (int c = 0; c < array_return.Length; c++)
                {
                    array_return[c] = new Int32[array_b[c].Length]; 
                    for (int r = 0; r < array_return[0].Length; r++)
                    {
                        array_return[c][r] = array_a[c][r];
                        if (array_return[c][r] < array_b[c][r])
                        {
                            array_return[c][r] = array_b[c][r];
                        }
                    }
                }
                return array_return;
                // Written. Warsaw. Workplace. 2024-07-17 12-59. 
            }


        }


            /// <summary>
            /// Written. 2023.11.08 15:39. Warsaw. Workplace. 
            /// </summary>
            /// Note. 2023.11.08 16:12. Warsaw. Workplace. 
            /// Columns and Rows words to be used and all combinations can be 
            /// descibed using it.
            /// Letter description did not work good when
            /// Int32[][] into 1 array by columns and by rows.
            public static class Merge
        {

            /// <summary>
            /// Merge two arrays Int32[] into one array Int32[]. <br></br>
            /// Written. 2024.01.21 16:40. Warsaw. Hostel. 
            /// </summary>
            /// <param name="arr_1"></param>
            /// <param name="arr_2"></param>
            /// <returns></returns>
            public static Int32[] A_B_To_C(Int32[] arr_1, Int32[] arr_2)
            {
                Int32[] arr_out = new Int32[arr_1.Length + arr_2.Length];
                Array.Copy(arr_1, arr_out, arr_1.Length);
                Array.Copy(arr_2, 0, arr_out, arr_1.Length, arr_2.Length);
                return arr_out;
            }

            /// <summary>
            /// Takes each Int32[m] from Int32[n][m] and makes one array Int32[nxm] <br></br>
            /// <br></br>
            /// Input: Int32[a][b] <br></br>
            /// Output: Int32[c], c = axb <br></br>
            /// </summary>
            /// <param name="array_in"></param>
            /// <returns></returns>
            public static Int32[] AxB_To_C(Int32[][] array_in)
            {
                return ArrayFunctions.Merge.NxM_To_A(array_in);
            }
        }
        /// <summary>
        /// Written. 2023.11.05 16:41. Gdansk. Home. 
        /// not tested. 2023.11.05 16:42. Gdansk. Home. 
        /// </summary>
        public static class Normilize
        {
            /// <summary>
            /// Written. 2023.11.05 16:57. Gdansk. Home. 
            /// Tested. Works. 2023.11.05 17:44. Gdansk. Home. 
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="number_for_max_in_arr"></param>
            /// <returns></returns>
            public static Int32[] ScaleMaximumToNumber(Int32[] arr_in, UInt32 number_for_max_in_arr)
            {
                // check if input is correct
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return new Int32[0];
                }
                if (number_for_max_in_arr == 0)
                {
                    ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Length_is_0);
                }
                // calculating normilize number.                   
                float normlize_coeficient = (float)arr_in.Max() / (float)number_for_max_in_arr;
                // making array for return
                Int32[] arr_out = new Int32[arr_in.Length];
                // devide each number in array
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    arr_out[i] = (int)((float)arr_in[i] / (float)normlize_coeficient);
                }
                // return array
                return arr_out;
            }
        }
        /// <summary>
        /// Checks if Int32 array 1 contains Int32 array 2.
        /// 2023.08.23 09:57. written.
        /// 2023.08.23 09:57. tested. works.
        /// /// </summary>
        /// <param name="arr_search_in"></param>
        /// <param name="arr_search"></param>
        /// <returns></returns>
        public static bool Contains(Int32[] arr_search_in, Int32[] arr_search)
        {
            return ArrayFunctions.Contains(arr_search_in, arr_search);
        }
        public static class Split
        {
            /// <summary>
            /// Split Int32[] to Int32[n][], where n - required number of columns
            /// 2023.08.23 10:00. written.
            /// 2023.08.23 10:00. tested. works.
            /// </summary>
            /// <param name="array_in"></param>
            /// <param name="columns_number"></param>
            /// <param name="columns_length_equal"></param>
            /// <returns></returns>
            public static Int32[][] A_To_MxN(Int32[] array_in, Int32 columns_number, bool columns_length_equal = true)
            {
                return ArrayFunctions.Split.A_To_MxN(array_in, columns_number, columns_length_equal);
            }
        }

        /// <summary>
        /// Written. 2024.02.09 15:45. Warsaw. Workplace. 
        /// </summary>
        public static class Generate
        {
            public static Int32[][] NormalDistribution(UInt32 array_size, UInt32 max_value, UInt32 points_of_1_deviation, UInt32 cone_center_x, UInt32 cone_center_y)
            {
                Int32[][] array_out = new Int32[array_size][];
                for (int c = 0; c < array_size; c++)
                {
                    array_out[c] = new Int32[array_size];
                    for (int r = 0; r < array_size; r++)
                    {
                        int r_distance = r - (int)cone_center_x;
                        int c_distance = c - (int)cone_center_y;
                        array_out[c][r] = MathFunctions.Distribution.NormalDistribution(
                            (int)max_value,
                            (int)System.Math.Sqrt(System.Math.Pow((double)r_distance, 2) + System.Math.Pow((double)c_distance, 2)),
                            0, points_of_1_deviation);                    
                    }
                }
                return array_out;

                // Written. Warsaw. Workplace. 2024-07-17 12-05.
                // Tested. Works. Warsaw. Workplace. 2024-07-17 12-14. 

            }

            public static Int32[][] ConeXYEdgeIsNormalDistribution(UInt32 array_size, UInt32 max_value, UInt32 points_of_1_deviation, UInt32 cone_center_x, UInt32 cone_center_y)
            {
                Int32[][] array_out = new Int32[array_size][];
               
                // level by Y axis
                Int32[] y_values = new Int32[array_size];
                for (int i = 0; i < array_size; i++)
                {
                    y_values[i] = MathFunctions.Distribution.NormalDistribution((int)max_value, i, (int)cone_center_y, points_of_1_deviation);
                }              
                // level by X axis
                Int32[] x_values = new Int32[array_size];
                for (int i = 0; i < array_size; i++)
                {
                    x_values[i] = MathFunctions.Distribution.NormalDistribution((int)max_value, i, (int)cone_center_x, points_of_1_deviation);
                }
                // filling array
                for (int c = 0; c < array_size; c++)
                {
                    array_out[c] = new Int32[array_size];
                    for (int r = 0; r < array_size; r++)
                    {
                        array_out[c][r] = x_values[r];
                        if (x_values[r] > y_values[c])
                        {
                            array_out[c][r] = y_values[c];
                        }                        
                    }
                }
                return array_out;

                // Written. Warsaw. Workplace. 2024-07-17 12-05.
                // Tested. Works. Warsaw. Workplace. 2024-07-17 12-14. 

            }

                /// <summary>
                /// Generates Int32[][] with values that form Cone with Square base.
                /// </summary>
                /// <param name="array_size"></param>
                /// <param name="cone_square_size"></param>
                /// <param name="cone_max_value"></param>
                /// <param name="points_for_half_value"></param>
                /// <param name="cone_center_x"></param>
                /// <param name="cone_center_y"></param>
                /// <returns></returns>
                public static Int32[][] ConeWithSquareBase(UInt32 array_size, UInt32 cone_max_value, UInt32 points_for_half_value, UInt32 cone_center_x, UInt32 cone_center_y, bool zero_is_min = true)
            {
                Int32[][] array_out = new Int32[array_size][];
                Int32 value_decrease_per_point = (Int32)(cone_max_value / (points_for_half_value * 2));
                // level by Y axis
                Int32[] y_values = new Int32[array_size];
                for (int i = (int)cone_center_y; i < array_size; i++)
                {
                    y_values[i] = (Int32)cone_max_value - value_decrease_per_point * (i - (int)cone_center_y);
                }
                for (int i = (int)cone_center_y; i >= 0; i--)
                {
                    y_values[i] = (Int32)cone_max_value - value_decrease_per_point * ((int)cone_center_y - i);
                }
                // level by X axis
                Int32[] x_values = new Int32[array_size];
                for (int i = (int)cone_center_x; i < array_size; i++)
                {
                    x_values[i] = (Int32)cone_max_value - value_decrease_per_point * (i - (int)cone_center_x);
                }
                for (int i = (int)cone_center_x; i >= 0; i--)
                {
                    x_values[i] = (Int32)cone_max_value - value_decrease_per_point * ((int)cone_center_x - i);
                }
                // filling array
                for (int c = 0; c < array_size; c++)
                {
                    array_out[c] = new Int32[array_size];
                    for (int r = 0; r < array_size; r++)
                    {
                        array_out[c][r] = x_values[r];
                        if (x_values[r] > y_values[c])
                        {
                            array_out[c][r] = y_values[c];
                        }
                        if (zero_is_min == true)
                        {
                            if (array_out[c][r] < 0)
                            {
                                array_out[c][r] = 0;
                            }
                        }
                    }
                }
                return array_out;

                // Written. Warsaw. Workplace. 2024-07-16 14-12. 
                // Tested. Works. Warsaw. Workplace. 2024-07-16 14-31. 
                // Note. No trouble in generating at the border of the array.
            }

            /// <summary>
            /// Written. 2024.02.09 15:51. Warsaw. Workplace. 
            /// Tested. Works. 2024.02.09 15:54. Warsaw. Workplace. 
            /// </summary>
            /// <param name="numbers_num"></param>
            /// <param name="element_value"></param>
            /// <returns></returns>
            public static Int32[] WithTheSameValue(Int32 numbers_num, Int32 element_value)
            {
                // note. word - identical. 2024.02.09 15:51. Warsaw. Workplace. 
                Int32[] arr_out = new Int32[numbers_num];
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    arr_out[i] = element_value;
                }
                return arr_out;
            }


            /// <summary>
            /// Generate Int32[] filled with random numbers
            /// 2023.07.10 - 2023.07.20. 10 - 15 o'clock. written.
            /// 2023.08.22 16:10. tested. works.
            /// </summary>
            /// <param name="numbers_num"></param>
            /// <param name="min"></param>
            /// <param name="max"></param>
            /// <returns></returns>
            public static Int32[] RandomMinMaxValue(Int32 numbers_num, Int32 min, Int32 max)
            {
                Int32[] arr_out = new Int32[numbers_num];
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    arr_out[i] = _internal_random.Next(min, max + 1);
                }
                return arr_out;
            }
            /// <summary>
            /// Generate Int32[] filled with numbers from 0 to array length - 1 <br></br>
            /// 2023.08.22 16:17. written. <br></br>
            /// 2023.08.22 16:18. tested. works. <br></br>
            /// </summary>
            /// <param name="numbers_num"></param>
            /// <returns>Array with element from 0 to length - 1. <br></br>
            /// Can return array  with length is 0</returns>
            public static Int32[] WithValueIncreasesBy1(Int32 numbers_num)
            {
                Int32[] arr_out = new Int32[numbers_num];
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    arr_out[i] = i;
                }
                return arr_out;
            }
            /// <summary>
            /// Generate Int32[][] filled with random numbers
            /// 2023.07.10 - 2023.07.20. 10 - 15 o'clock. written.
            /// 2023.22.08 15:10. tested. works.
            /// </summary>
            /// <param name="numbers_num_x"></param>
            /// <param name="numbers_num_y"></param>
            /// <param name="min"></param>
            /// <param name="max"></param>
            /// <returns></returns>
            public static Int32[][] RandomMinMaxValue(Int32 numbers_num_x, Int32 numbers_num_y, Int32 min, Int32 max)
            {
                Int32[][] arr_out = new Int32[numbers_num_x][];
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    arr_out[i] = new Int32[numbers_num_y];
                    for (Int32 j = 0; j < arr_out[i].Length; j++)
                    {
                        arr_out[i][j] = _internal_random.Next(min, max + 1);
                    }
                }
                return arr_out;
            }
            /// <summary>
            ///  Generate Int32[][] filled with numbers starting from 0 to (Rows x Cols - 1)<br></br>
            ///  2023.10.30 13:49. Written.
            ///  2023.10.30 13:50.
            /// </summary>
            /// <param name="numbers_num_x"></param>
            /// <param name="numbers_num_y"></param>
            /// <returns></returns>
            public static Int32[][] WithValueIncreasesBy1(Int32 numbers_num_x, Int32 numbers_num_y)
            {
                Int32[][] arr_out = new Int32[numbers_num_x][];
                Int32 num_for_arr = 0;
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    arr_out[i] = new Int32[numbers_num_y];
                    for (Int32 j = 0; j < arr_out[i].Length; j++)
                    {
                        arr_out[i][j] = num_for_arr;
                        num_for_arr++;
                    }
                }
                return arr_out;
            }
        }
        /// <summary>
        /// Accepts: char[]
        /// 2023-07-26 13:15
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr_in"></param>
        /// <returns></returns>
        public static Int32[] ConvertToInt32(char[] arr_in)
        {
            Int32[] arr_out = new Int32[arr_in.Length];
            for (Int32 i = 0; i < arr_out.Length; i++)
            {
                arr_out[i] = (int)arr_in[i];
            }
            return arr_out;
        }
        /// <summary>
        /// string is char[] and each char is 2 bytes
        /// 2023-08-01 15:27
        /// </summary>
        /// <param name="str_in"></param>
        /// <returns></returns>
        public static Int16[] ConvertToInt16Array(string str_in)
        {
            Int16[] arr_out = Array.ConvertAll(str_in.ToArray(), CharToInt16);
            Int16 CharToInt16(char char_in)
            {
                return (Int16)char_in;
            }
            return arr_out;
        }
        public static void ToConsole(Int32[] arr_1, Int32[] arr_2)
        {
            Type type = typeof(int);
            Console.WriteLine("Array 1" + type.Name.ToString() + ". Length is " + arr_1.Length.ToString());
            Console.WriteLine("Array 2" + type.Name.ToString() + ". Length is " + arr_2.Length.ToString());
            Console.WriteLine();
            string str_size_min = "";
            str_size_min = arr_2.Min().ToString();
            if (arr_1.Min() < arr_2.Min())
            {
                str_size_min = arr_1.Min().ToString();
            }
            string str_size_max = "";
            str_size_max = arr_2.Max().ToString();
            if (arr_1.Max() > arr_2.Max())
            {
                str_size_min = arr_1.Max().ToString();
            }
            string pad_str = "".PadRight(str_size_max.Length, ' '); ;
            if (str_size_min.Length > str_size_max.Length)
            {
                pad_str = "".PadRight(str_size_min.Length, ' ');
            }
            for (Int32 i = 0; i < arr_1.Length; i++)
            {
                Console.WriteLine(arr_1[i].ToString().PadRight(pad_str.Length, ' ') +
                    pad_str + arr_2[i].ToString().PadRight(pad_str.Length, ' '));
            }
            Console.WriteLine();
        }
        /// <summary>
        /// Prints Int32[][] to console <br></br>
        /// 2023.08.12 09:31. written. <br></br>
        /// 2023.08.22 15:16. tested. works. <br></br>
        /// Modified. Added parameter - spaces. Tested. Works.  2024.02.09 10:33. Warsaw. Workplace. 
        /// </summary>
        /// <param name="arr_in"></param>
        public static void ToConsole(Int32[][] arr_in, UInt32 spaces_between_numbers = 3)
        {
            if (arr_in.Length == 0)
            {
                Console.WriteLine("Attention! Array is empty");
                return;
            }
            Console.WriteLine("Array" + typeof(int).Name.ToString() + ". Length is " + arr_in.Length.ToString() + "x" + arr_in[0].Length.ToString());
            Int32[] array_all_values = ArrayFunctions.Merge.NxM_To_A(arr_in);
            string min_num = array_all_values.Min().ToString();
            string max_num = array_all_values.Max().ToString();
            Int32 pad_size = max_num.Length;
            if (max_num.Length < min_num.Length)
            {
                pad_size = min_num.Length;
            }
            string[][] str_arr = TwoDimensions.Convert.ToStringArray.ToDecimal(arr_in);
            str_arr = StringArray.Pad.Right(str_arr, pad_size);
            string str_for_console = StringArray.Convert.ToFileString(str_arr, "".PadRight((int)spaces_between_numbers, ' '));
            // 2024.01.21 16:52. Warsaw. Hostel. 
            // There were Writeline. FileString adds "\r\n" at the last line therefore
            // Console.Writeline is not needed.
            Console.Write(str_for_console);
        }
        /// <summary>
        /// Accepts: object[]
        /// 2023-08-01 15:21
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr_in"></param>
        /// <returns></returns>
        public static Int32[] ConvertToInt32(object[] arr_in)
        {
            Int32[] arr_out = new Int32[arr_in.Length];
            for (Int32 i = 0; i < arr_out.Length; i++)
            {
                try
                {
                    arr_out[i] = (int)arr_in[i];
                }
                catch
                {
                    ReportFunctions.ReportError("Cast failed at " + i.ToString());
                }
            }
            return arr_out;
        }
        /// <summary>
        /// Tested. Works. 2024.02.14 13:22. Warsaw. Workplace. 
        /// </summary>
        /// <param name="arr_in"></param>
        /// <returns></returns>
        public static Int32 Average(Int32[] arr_in)
        {
            // 2024.02.14 13:22. Warsaw. Workplace. 
            // need long[] because there were overflow of Int32 because of a lot of large numbers.
            long[] arr_long = new long[arr_in.Length];
            Array.Copy(arr_in, arr_long, arr_long.Length);
            return (int)(arr_long.Sum() / (long)arr_in.Length);
        }
        /// <summary>
        /// Sort array according to provided indexes
        /// 2023.08.12 12:12
        /// </summary>
        /// <param name="arr_in"></param>
        /// <param name="indexes_in"></param>
        /// <returns></returns>
        public static Int32[] Sort(Int32[] arr_in, Int32[] indexes_in)
        {
            return ArrayFunctions.Sort(arr_in, indexes_in);
        }
        /// <summary>
        /// Sort AxB array according to indexes provided
        /// 2023.08.12 12:13
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr_in"></param>
        /// <param name="indexes_in"></param>
        /// <returns></returns>
        public static Int32[][] Sort(Int32[][] arr_in, Int32[] indexes_in)
        {
            return ArrayFunctions.Sort(arr_in, indexes_in);
        }
        public static Int32[] Sort(Int32[] arr_in, ArraySortingEnum order_to_sort)
        {
            Int32[] arr_out = arr_in;
            Int32 sort_condition(Int32 num1, Int32 num2)
            {
                Int32 res_out = 0;
                if (num1 > num2)
                {
                    return 1;
                }
                if (num1 == num2)
                {
                    return 0;
                }
                if (num1 < num2)
                {
                    return -1;
                }
                return res_out;
            }
            Array.Sort(arr_out, sort_condition);
            if (order_to_sort == ArraySortingEnum.Descending)
            {
                Array.Reverse(arr_out);
            }
            return arr_out;
        }
        /// <summary>
        /// Provides change in indexes after sorting
        /// 2023.08.12 11:07
        /// </summary>
        /// <param name="arr_in"></param>
        /// <param name="order_to_sort"></param>
        /// <returns></returns>
        public static Int32[] SortingIndexes(Int32[] arr_in, ArraySortingEnum order_to_sort)
        {
            Int32[] arr_to_sort = new Int32[arr_in.Length];
            Int32[] indexes = new Int32[arr_in.Length];
            Array.Copy(arr_in, arr_to_sort, arr_in.Length);
            for (Int32 i = 0; i < indexes.Length; i++)
            {
                indexes[i] = i;
            }
            for (Int32 i = 0; i < arr_to_sort.Length; i++)
            {
                Int32 max_number = arr_to_sort[i];
                Int32 max_number_index = i;
                for (Int32 j = i; j < arr_to_sort.Length; j++)
                {
                    if (arr_to_sort[j] >= max_number)
                    {
                        max_number = arr_to_sort[j];
                        max_number_index = j;
                    }
                }
                arr_to_sort[max_number_index] = arr_to_sort[i];
                arr_to_sort[i] = max_number;
                Int32 index_tmp = indexes[i];
                indexes[i] = indexes[max_number_index];
                indexes[max_number_index] = index_tmp;
            }
            if (order_to_sort == ArraySortingEnum.Ascenidng)
            {
                Array.Reverse(indexes);
            }
            return indexes;
        }
        /// <summary>
        /// Sort Int32 array and provides change in indexes
        /// 2023.08.12 11:04
        /// </summary>
        /// <param name="arr_in"></param>
        /// <param name="sorting_indexes"></param>
        /// <param name="order_to_sort"></param>
        /// <returns></returns>
        public static Int32[] Sort(Int32[] arr_in, out Int32[] sorting_indexes, ArraySortingEnum order_to_sort)
        {
            Int32[] arr_out = arr_in;
            Int32[] indexes = new Int32[arr_in.Length];
            for (Int32 i = 0; i < arr_out.Length; i++)
            {
                indexes[i] = i;
            }
            for (Int32 i = 0; i < arr_out.Length; i++)
            {
                Int32 max_number = arr_in[i];
                Int32 max_number_index = i;
                for (Int32 j = i; j < arr_out.Length; j++)
                {
                    if (arr_in[j] >= max_number)
                    {
                        max_number = arr_in[j];
                        max_number_index = j;
                    }
                }
                arr_out[max_number_index] = arr_out[i];
                arr_out[i] = max_number;
                Int32 index_tmp = indexes[i];
                indexes[i] = indexes[max_number_index];
                indexes[max_number_index] = index_tmp;
            }
            if (order_to_sort == ArraySortingEnum.Ascenidng)
            {
                Array.Reverse(arr_out);
                Array.Reverse(indexes);
            }
            sorting_indexes = indexes;
            return arr_out;
        }
        /// <summary>
        /// Prints Int32 Array to console
        /// 2023.08.12 08:18
        /// Modified. Added with index. 2024.03.08 15:15. Warsaw. Hostel.
        /// </summary>
        /// <param name="arr_in"></param>
        public static void ToConsole(Int32[] arr_in, bool with_index = false)
        {
            if (with_index == false)
            {
                Type type = typeof(int);
                Console.WriteLine("Array " + type.Name.ToString() + ". Length is " + arr_in.Length.ToString());
                string[] strings_arr = Convert.ToStringArray(arr_in);
                Console.Write(StringArray.Convert.ToFileString(strings_arr));
                return;
            }

            if (with_index == true)
            {
                Type type = typeof(int);
                Console.WriteLine("Array " + type.Name.ToString() + ". Length is " + arr_in.Length.ToString());
                string[] strings_arr = Convert.ToStringArray(arr_in);
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    Console.Write(i.ToString() + ".\t");
                    Console.WriteLine(strings_arr[i]);
                }
                return;
            }





        }


        /// <summary>
        /// Written. 2024.03.08 15:03. Warsaw. Hostel.
        /// </summary>
        public static class Find
        {
            /// <summary>
            /// Returns max value in Int32[][].
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static Int32 Max(Int32[][] arr_in)
            {
                Int32[] max_of_each_column = new Int32[arr_in.Length];
                for (int i = 0; i < max_of_each_column.Length; i++) 
                {
                    max_of_each_column[i] = arr_in[i].Max();
                }
                return max_of_each_column.Max();
                // Written. Warsaw. Workplace. 2024-07-16 15-03. 

            }

            /// <summary>
            /// Written. 2024.03.08 15:30. Warsaw. Hostel.
            /// </summary>
            public static class ClosestElement
            {

                /// <summary>
                /// Find the closest element to provided value in Int32[] and return the value of this element<br></br>
                /// Written. 2024.03.08 15:33. Warsaw. Hostel. <br></br>
                /// Tested. Works. 2024.03.08 15:35. Warsaw. Hostel.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <param name="value_in"></param>
                /// <returns></returns>
                public static Int32 Value(Int32[] arr_in, Int32 value_in)
                {
                    Int32 index_found = ClosestElement.Index(arr_in, value_in);
                    Int32 int_return = arr_in[index_found];
                    return int_return;
                }


                /// <summary>
                /// Find the closest element to provided value in Int32[] and return the index of this element<br></br> 
                /// Written. 2024.03.08 15:11. Warsaw. Hostel. <br></br>
                /// Tested. Works. 2024.03.08 15:27. Warsaw. Hostel.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <param name="value_in"></param>
                /// <returns></returns>
                public static Int32 Index(Int32[] arr_in, Int32 value_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return 0;
                    }


                    Int32 substract_value = arr_in[0] - value_in;
                    Int32 index_found = 0;
                    // postive decreases. that value
                    // negative becomes bigger

                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        if (System.Math.Abs(substract_value) > System.Math.Abs(arr_in[i] - value_in))
                        {
                            substract_value = arr_in[i] - value_in;
                            index_found = i;
                        }
                    }
                    return index_found;



                }


            }

        }
        public static class Math
        {

            /// <summary>
            /// Written. 2024.02.09 15:43. Warsaw. Workplace. 
            /// Tested. Works. 2024.02.09 15:57. Warsaw. Workplace. 
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static bool ElementsTheSame(Int32[] arr_in)
            {
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return false;
                }
                bool result_out = true;
                Int32 arr_element_1st = arr_in[0];
                foreach (Int32 arr_element in arr_in)
                {
                    if (arr_element != arr_element_1st)
                    {
                        result_out = false;
                        break;
                    }
                }

                return result_out;

            }

            /// <summary>
            /// Counts how many times each number is used in array. Returns Int32[][]. <br></br>
            /// 1 index is numbers, 2 index is count <br></br>
            /// Written. 2024.01.20 22:55. Warsaw. Hostel.
            /// Tested. Works. 2024.01.21 16:52. Warsaw. Hostel. 
            /// </summary>
            public static Int32[][] Distribution(Int32[] arr_in)
            {
                Int32[] numbers_of_array = NumbersUsedInArray(arr_in);
                Int32[] numbers_count = new Int32[numbers_of_array.Length];

                for (Int32 i = 0; i < numbers_of_array.Length; i++)
                {

                    for (Int32 j = 0; j < arr_in.Length; j++)
                    {
                        if (arr_in[j] == numbers_of_array[i])
                        {
                            numbers_count[i] += 1;
                        }
                    }

                }

                Int32[][] arr_out = new Int32[2][];
                arr_out[0] = numbers_of_array;
                arr_out[1] = numbers_count;
                return arr_out;

            }







            /// <summary>
            /// Return numbers that are used in the array without repetetion of the same number <br></br>
            /// Written. 2024.01.20 22:21. Warsaw. Hostel. <br></br>
            /// Tested. Works. 2024.01.21 16:48. Warsaw. Hostel. 
            /// </summary>
            public static Int32[] NumbersUsedInArray(Int32[] arr_in)
            {
                List<int> arr_out_list = new List<int>();
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    if (arr_out_list.Contains(arr_in[i]) == false)
                    {
                        arr_out_list.Add(arr_in[i]);
                    }
                }

                return arr_out_list.ToArray();

            }








            /// <summary>
            /// 2023.10.30 14:19. Written. Warsaw. Workplace. <br></br>
            /// 2023.10.30 14:22. Tested. Works
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static Int32[][] InvertSign(Int32[][] arr_in)
            {
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return arr_in;
                }
                Int32[][] arr_out = new Int32[arr_in.Length][];
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    arr_out[i] = InvertSign(arr_in[i]);
                }
                return arr_out;
            }
            /// <summary>
            /// 2023.10.30 13:01. Written. Warsaw. Workplace. <br></br>
            /// 2023.10.30 13:01. Tested. Works.
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static Int32[] InvertSign(Int32[] arr_in)
            {
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return arr_in;
                }
                Int32[] arr_out = new Int32[arr_in.Length];
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    arr_out[i] = 0;
                    arr_out[i] = -arr_in[i];
                }
                return arr_out;
            }
            /// <summary>
            /// Adds Int32 Array to Int32 Array and returns Int32 Array.<br></br>
            /// 2023.08.01 - 2023.09.01. Written. Warsaw. <br></br>
            /// 2023.10.30 12:56. Tested. Works.
            /// </summary>
            /// <param name="arr_1"></param>
            /// <param name="arr_2"></param>
            /// <returns></returns>
            public static Int32[] Add(Int32[] arr_1, Int32[] arr_2)
            {
                if (arr_1.Length != arr_2.Length)
                {
                    ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                    return arr_1;
                }
                Int32[] arr_out = new Int32[arr_1.Length];
                for (Int32 i = 0; i < arr_1.Length; i++)
                {
                    arr_out[i] = arr_1[i] + arr_2[i];
                }
                return arr_out;
            }
            /// <summary>
            /// Adds number to Int32 Array and returns Int32 Array.<br></br>
            /// 2023.08.01 - 2023.09.01. Written. Warsaw. <br></br>
            /// 2023.10.30 12:47. Tested. Works.
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="number_in"></param>
            /// <returns></returns>
            public static Int32[] Add(Int32[] arr_in, Int32 number_in)
            {
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return arr_in;
                }
                Int32[] arr_out = new Int32[arr_in.Length];
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    arr_out[i] = arr_in[i] + number_in;
                }
                return arr_out;
            }
            /// <summary>
            /// Adds Int32[][] Array to Int32[][] Array and returns Int32[][] array<br></br>
            /// 2023.08.01 - 2023.09.01. Written. Warsaw. <br></br>
            /// 2023.10.30 14:44. Tested. Works.
            /// </summary>
            /// <param name="arr_1"></param>
            /// <param name="arr_2"></param>
            /// <returns></returns>
            public static Int32[][] Add(Int32[][] arr_1, Int32[][] arr_2)
            {
                if (arr_1.Length != arr_2.Length)
                {
                    ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                    return arr_1;
                }
                for (Int32 i = 0; i < arr_1.Length; i++)
                {
                    if (arr_1[i].Length != arr_2[i].Length)
                    {
                        ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                        return arr_1;
                    }
                }
                Int32[][] arr_out = new Int32[arr_1.Length][];
                for (Int32 i = 0; i < arr_1.Length; i++)
                {
                    arr_out[i] = Add(arr_1[i], arr_2[i]);
                }
                return arr_out;
            }
            /// <summary>
            /// Adds number to Int32[][] and returns Int32[][] array<br></br>
            /// 2023.08.01 - 2023.09.01. Written. Warsaw. <br></br>
            /// 2023.10.30 14:42. Tested. Works.
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="number_in"></param>
            /// <returns></returns>
            public static Int32[][] Add(Int32[][] arr_in, Int32 number_in)
            {
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return arr_in;
                }
                Int32[][] arr_out = new Int32[arr_in.Length][];
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    arr_out[i] = Add(arr_in[i], number_in);
                }
                return arr_out;
            }
            public static Int32[] Multiply(Int32[] arr_1, Int32[] arr_2)
            {
                if (arr_1.Length != arr_2.Length)
                {
                    ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                    return arr_1;
                }
                Int32[] arr_out = new Int32[arr_1.Length];
                for (Int32 i = 0; i < arr_1.Length; i++)
                {
                    arr_out[i] = arr_1[i] * arr_2[i];
                }
                return arr_out;
            }
            public static Int32[] Multiply(Int32[] arr_in, Int32 number_in)
            {
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return arr_in;
                }
                Int32[] arr_out = new Int32[arr_in.Length];
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    arr_out[i] = arr_in[i] * number_in;
                }
                return arr_out;
            }

            /// <summary>
            /// Written. 2024.04.19 15:57. Warsaw. Workplace.
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="number_in"></param>
            /// <returns></returns>
            public static Int32[] Multiply(Int32[] arr_in, float number_in)
            {
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return arr_in;
                }
                Int32[] arr_out = new Int32[arr_in.Length];
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    arr_out[i] = (int)((float)arr_in[i] * number_in);
                }
                return arr_out;
            }



            public static Int32[][] Multiply(Int32[][] arr_1, Int32[][] arr_2)
            {
                if (arr_1.Length != arr_2.Length)
                {
                    ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                    return arr_1;
                }
                for (Int32 i = 0; i < arr_1.Length; i++)
                {
                    if (arr_1[i].Length != arr_2[i].Length)
                    {
                        ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                        return arr_1;
                    }
                }
                Int32[][] arr_out = new Int32[arr_1.Length][];
                for (Int32 i = 0; i < arr_1.Length; i++)
                {
                    arr_out[i] = Multiply(arr_1[i], arr_2[i]);
                }
                return arr_out;
            }
            public static Int32[][] Multiply(Int32[][] arr_in, Int32 number_in)
            {
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return arr_in;
                }
                Int32[][] arr_out = new Int32[arr_in.Length][];
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    arr_out[i] = Multiply(arr_in[i], number_in);
                }
                return arr_out;
            }
            public static Int32[] Devide(Int32[] arr_1, Int32[] arr_2)
            {
                if (arr_1.Length != arr_2.Length)
                {
                    ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                    return arr_1;
                }
                Int32[] arr_out = new Int32[arr_1.Length];
                for (Int32 i = 0; i < arr_1.Length; i++)
                {
                    try
                    {
                        arr_out[i] = arr_1[i] / arr_2[i];
                    }
                    catch
                    {
                        ReportFunctions.ReportError(i);
                        return arr_1;
                    }
                }
                return arr_out;
            }
            public static Int32[] Devide(Int32[] arr_in, Int32 number_in)
            {
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return arr_in;
                }
                if (number_in == 0)
                {
                    ReportFunctions.ReportError("Devide by 0");
                    return arr_in;
                }
                Int32[] arr_out = new Int32[arr_in.Length];
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    arr_out[i] = arr_in[i] / number_in;
                }
                return arr_out;
            }
            public static Int32[][] Devide(Int32[][] arr_1, Int32[][] arr_2)
            {
                if (arr_1.Length != arr_2.Length)
                {
                    ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                    return arr_1;
                }
                for (Int32 i = 0; i < arr_1.Length; i++)
                {
                    if (arr_1[i].Length != arr_2[i].Length)
                    {
                        ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                        return arr_1;
                    }
                }
                Int32[][] arr_out = new Int32[arr_1.Length][];
                for (Int32 i = 0; i < arr_1.Length; i++)
                {
                    try
                    {
                        arr_out[i] = Devide(arr_1[i], arr_2[i]);
                    }
                    catch
                    {
                        ReportFunctions.ReportError(i);
                        return arr_1;
                    }
                }
                return arr_out;
            }
            public static Int32[][] Devide(Int32[][] arr_in, Int32 number_in)
            {
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return arr_in;
                }
                if (number_in == 0)
                {
                    ReportFunctions.ReportError("Devide by 0");
                    return arr_in;
                }
                Int32[][] arr_out = new Int32[arr_in.Length][];
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    arr_out[i] = Devide(arr_in[i], number_in);
                }
                return arr_out;
            }
            /// <summary>
            /// Substracts Int32 Array to Int32 Array and returns Int32 Array.<br></br>
            /// 2023.08.01 - 2023.09.01. Written. Warsaw. <br></br>
            /// 2023.10.30 16:00. Tested. Works.
            /// </summary>
            /// <param name="arr_1"></param>
            /// <param name="arr_2"></param>
            /// <returns></returns>
            public static Int32[] Substract(Int32[] arr_1, Int32[] arr_2)
            {
                if (arr_1.Length != arr_2.Length)
                {
                    ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                    return arr_1;
                }
                Int32[] arr_out = new Int32[arr_1.Length];
                for (Int32 i = 0; i < arr_1.Length; i++)
                {
                    arr_out[i] = arr_1[i] - arr_2[i];
                }
                return arr_out;
            }
            /// <summary>
            /// Substracts number to Int32 Array and returns Int32 Array.<br></br>
            /// 2023.08.01 - 2023.09.01. Written. Warsaw. <br></br>
            /// 2023.10.30 16:01. Tested. Works.
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="number_in"></param>
            /// <returns></returns>
            public static Int32[] Substract(Int32[] arr_in, Int32 number_in)
            {
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return arr_in;
                }
                Int32[] arr_out = new Int32[arr_in.Length];
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    arr_out[i] = arr_in[i] - number_in;
                }
                return arr_out;
            }
            /// <summary>
            /// Substracts Int32[][] Array to Int32[][] Array and returns Int32[][] array<br></br>
            /// 2023.08.01 - 2023.09.01. Written. Warsaw. <br></br>
            /// 2023.10.30 16:04. Tested. Works.
            /// </summary>
            /// <param name="arr_1"></param>
            /// <param name="arr_2"></param>
            /// <returns></returns>
            public static Int32[][] Substract(Int32[][] arr_1, Int32[][] arr_2)
            {
                if (arr_1.Length != arr_2.Length)
                {
                    ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                    return arr_1;
                }
                for (Int32 i = 0; i < arr_1.Length; i++)
                {
                    if (arr_1[i].Length != arr_2[i].Length)
                    {
                        ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                        return arr_1;
                    }
                }
                Int32[][] arr_out = new Int32[arr_1.Length][];
                for (Int32 i = 0; i < arr_1.Length; i++)
                {
                    arr_out[i] = Substract(arr_1[i], arr_2[i]);
                }
                return arr_out;
            }
            /// <summary>
            /// Substracts number to Int32[][] and returns Int32[][] array<br></br>
            /// 2023.08.01 - 2023.09.01. Written. Warsaw. <br></br>
            /// 2023.10.30 16:02. Tested. Worked.
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="number_in"></param>
            /// <returns></returns>
            public static Int32[][] Substract(Int32[][] arr_in, Int32 number_in)
            {
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return arr_in;
                }
                Int32[][] arr_out = new Int32[arr_in.Length][];
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    arr_out[i] = Substract(arr_in[i], number_in);
                }
                return arr_out;
            }
            static object[] SubstractObjectArrays(object[] arr_1, object[] arr_2, Type type_in)
            {
                object[] arr_out = new object[arr_1.Length];
                for (Int32 i = 0; i < arr_1.Length; i++)
                {
                    if (type_in == typeof(int))
                    {
                        arr_out[i] = (int)arr_1[i] - (int)arr_2[i];
                    }
                    if (type_in == typeof(short))
                    {
                        arr_out[i] = (short)arr_1[i] - (short)arr_2[i];
                    }
                    if (type_in == typeof(byte))
                    {
                        arr_out[i] = (byte)arr_1[i] - (byte)arr_2[i];
                    }
                    if (type_in == typeof(float))
                    {
                        arr_out[i] = (float)arr_1[i] - (float)arr_2[i];
                    }
                }
                return arr_out;
            }
        }
        public static class Convert
        {

            /// <summary>
            /// Converts array to filestring according to required number format, delimer and missing number string (defined by char). <br></br> 
            /// Written. 2024.03.21 11:15. Warsaw. Workplace. <br></br>
            /// Tested. Works. 2024.03.21 11:27. Warsaw. Workplace. <br></br> 
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="num_per_row"></param>
            /// <param name="delimer"></param>
            /// <param name="missing_number_char">Defines missing number string. The string will be of the length of the longest number with this char</param>
            /// <param name="base_in"></param>
            /// <param name="pad_number"></param>
            /// <returns></returns>
            public static string ToFileString(Int32[] arr_in, int num_per_row, int base_in = 10, string delimer = "\t", char missing_number_char = '.', int pad_number = -1)
            {
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return "";
                }

                int rows_num = arr_in.Length / num_per_row;
                bool IsNotRectangle = false;
                if ((arr_in.Length % num_per_row) != 0)
                {
                    rows_num += 1;
                    IsNotRectangle = true;
                }

                string[] strings_filestring = new string[rows_num];

                StringBuilder write_string = new StringBuilder();

                int col_index = 0;

                int pad_length = pad_number;

                if (base_in == 10)
                {
                    if (pad_number == -1)
                    {
                        int max_int = arr_in.Max();
                        int min_int = arr_in.Min();
                        pad_length = min_int.ToString().Length;
                        if (max_int.ToString().Length > min_int.ToString().Length)
                        {
                            pad_length = max_int.ToString().Length;
                        }
                    }
                }

                if (base_in == 16)
                {
                    pad_length = 8;
                }

                for (int i = 0; i < arr_in.Length; i++)
                {
                    if (base_in == 10)
                    {
                        write_string.Append(System.Convert.ToString(arr_in[i], base_in).ToUpper().PadRight(pad_length, ' '));
                    }
                    if (base_in == 16)
                    {
                        write_string.Append(System.Convert.ToString(arr_in[i], base_in).ToUpper().PadLeft(pad_length, '0'));
                    }
                    write_string.Append(delimer);
                    col_index += 1;
                    if (col_index > (num_per_row - 1))
                    {
                        write_string.Append("\r\n");
                        col_index = 0;
                    }
                }

                if (IsNotRectangle == true)
                {
                    int row_filled = arr_in.Length % num_per_row;
                    for (int i = 0; i < (num_per_row - row_filled); i++)
                    {
                        write_string.Append("".PadRight(pad_length, missing_number_char));
                        write_string.Append(delimer);
                        write_string.Append("\r\n");
                    }
                }

                string return_string = write_string.ToString();

                return return_string;

            }


            /// <summary>
            /// Written. 2023.11.05 13:26. Gdansk. Home. <br></br>
            /// Tested. Works. 2023.11.05 13:33. Gdansk. Home. 
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static char[] ToCharArray(Int32[] arr_in)
            {
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return new char[0];
                }
                char[] arr_out = Array.ConvertAll(arr_in, Int32ToChar);
                char Int32ToChar(Int32 number)
                {
                    return System.Convert.ToChar(number);
                }
                return arr_out;
            }
            /// <summary>
            /// 2023.08.29 10:39. written. <br></br>
            /// 2023.08.29 10:39. tested. works. <br></br>
            /// what if the number > 255 ? 2023.12.14 12:36. Workplace
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static byte[] ToByteArray(Int32[] arr_in)
            {
                byte[] arr_out = Array.ConvertAll(arr_in, Int32ToByte);
                byte Int32ToByte(Int32 number)
                {
                    return System.Convert.ToByte(number);
                }
                return arr_out;
            }
            /// <summary>
            /// 2023.08.29 10:39. written.
            /// 2023.08.29 10:39. not checked.
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static byte[][] ConvertToByteArray(Int32[][] arr_in)
            {
                byte[][] arr_out = new byte[arr_in.Length][];
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    arr_out[i] = ToByteArray(arr_in[i]);
                }
                return arr_out;
            }
            /// <summary>
            /// not tested
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static string[] ToStringArray(Int32[] arr_in)
            {
                // space for code. start.
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return new string[0];
                }
                string[] arr_out = new string[arr_in.Length];
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    try
                    {
                        arr_out[i] = System.Convert.ToString(arr_in[i]);
                    }
                    catch
                    {
                        ReportFunctions.ReportError(i);
                    }
                }
                // space for code. end.
                return arr_out;
            }

        }


        /// <summary>
        /// Written. 2024.05.20 17:21. Warsaw. Workplace 
        /// </summary>

        public static class TwoDimensions
        {

            /// <summary>
            /// Written. 2024.05.20 17:21. Warsaw. Workplace.
            /// </summary>
            public static class Generate
            {
                /// <summary>
                /// Generate Int32[][] filled with random numbers from min to max number. <br></br>
                /// Written. 2023.07.10 - 2023.07.20. 10 - 15 o'clock. <br></br>
                /// Tested. Works. 2023.22.08 15:10. <br></br>
                /// </summary>
                /// <param name="numbers_num_x"></param>
                /// <param name="numbers_num_y"></param>
                /// <param name="min"></param>
                /// <param name="max"></param>
                /// <returns></returns>
                public static Int32[][] RandomMinMaxValue(Int32 numbers_num_x, Int32 numbers_num_y, Int32 min, Int32 max)
                {
                    // Moved to TwoDimensions. 2024.05.20 17:24.Warsaw.Workplace.						
                    Int32[][] arr_out = new Int32[numbers_num_x][];
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        arr_out[i] = new Int32[numbers_num_y];
                        for (Int32 j = 0; j < arr_out[i].Length; j++)
                        {
                            arr_out[i][j] = _internal_random.Next(min, max + 1);
                        }
                    }
                    return arr_out;
                }
                /// <summary>
                ///  Generate Int32[][] filled with numbers starting from 0 to (Rows x Cols - 1)<br></br>
                ///  2023.10.30 13:49. Written.
                ///  2023.10.30 13:50.
                /// </summary>
                /// <param name="numbers_num_x"></param>
                /// <param name="numbers_num_y"></param>
                /// <returns></returns>
                public static Int32[][] WithValueIncreasesBy1(Int32 numbers_num_x, Int32 numbers_num_y)
                {
                    Int32[][] arr_out = new Int32[numbers_num_x][];
                    Int32 num_for_arr = 0;
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        arr_out[i] = new Int32[numbers_num_y];
                        for (Int32 j = 0; j < arr_out[i].Length; j++)
                        {
                            arr_out[i][j] = num_for_arr;
                            num_for_arr++;
                        }
                    }
                    return arr_out;
                }
            }



            /// <summary>
            /// Written. 2024.05.20 17:21. Warsaw. Workplace 
            /// </summary>
            public static class Convert
            {




                /// <summary>
                /// 2023.08.29 10:39. written.
                /// 2023.08.29 10:39. not checked.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static byte[][] ConvertToByteArray(Int32[][] arr_in)
                {
                    byte[][] arr_out = new byte[arr_in.Length][];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = Int32ArrayFunctions.Convert.ToByteArray(arr_in[i]);
                    }
                    return arr_out;
                }
                /// <summary>
                /// not tested
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>

                public static class ToStringArray
                {

                    /// <summary>
                    /// Written. 2024.05.20 16:49. Warsaw. Workplace 
                    /// </summary>
                    /// <param name="arr_in"></param>
                    /// <param name="add_to_start">0x or can be left empty</param>
                    /// <param name="padding_with_0"></param>
                    /// <returns></returns>
                    public static string[][] ToHEX(Int32[][] arr_in, bool padding_by_zero = true, bool upper_case = true, string add_to_start = "")
                    {
                        float execution_time_ms_start = 0;
                        if (TimeExecutionShow == true)
                        {
                            execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                        }




                        if (arr_in.Length == 0)
                        {
                            ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                            return new string[0][];
                        }

                        string[][] arr_out = new string[arr_in.Length][];
                        for (Int32 i = 0; i < arr_in.Length; i++)
                        {
                            arr_out[i] = new string[arr_in[i].Length];
                            for (Int32 j = 0; j < arr_in[i].Length; j++)
                            {
                                try
                                {
                                    arr_out[i][j] = System.Convert.ToString(arr_in[i][j], 16);
                                    if (padding_by_zero == true)
                                    {
                                        if (upper_case == true)
                                        {
                                            arr_out[i][j] = arr_out[i][j].PadLeft(8, '0').ToUpper();
                                            if (add_to_start != "")
                                            {
                                                arr_out[i][j] += add_to_start;
                                            }
                                        }
                                        else
                                        {
                                            arr_out[i][j] = arr_out[i][j].PadLeft(8, '0').ToLower();
                                            if (add_to_start != "")
                                            {
                                                arr_out[i][j] += add_to_start;
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    ReportFunctions.ReportError(i, j);
                                }
                            }
                        }
                        if (TimeExecutionShow == true)
                        {
                            float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                            TimeExecutionMessage(nameof(TwoDimensions.Convert.ToStringArray.ToHEX), execution_time_ms_stop - execution_time_ms_start);
                        }
                        return arr_out;
                    }



                    public static string[][] ToDecimal(Int32[][] arr_in, bool padding = true, bool right_padding = true)
                    {
                        // space for code. start.
                        if (arr_in.Length == 0)
                        {
                            ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                            return new string[0][];
                        }

                        // 2024.05.20 16:44. Warsaw. Workplace. Added.
                        int padding_length = 0;
                        if (padding == true)
                        {
                            Int32[] max_of_column = new Int32[arr_in.Length];
                            Int32[] min_of_column = new Int32[arr_in.Length];
                            for (int i = 0; i < arr_in.Length; i++)
                            {
                                max_of_column[i] = arr_in[i].Max();
                                min_of_column[i] = arr_in[i].Min();
                            }

                            string max_str = max_of_column.Max().ToString();
                            string min_str = min_of_column.Min().ToString();

                            padding_length = max_str.Length;
                            if (min_str.Length > max_str.Length)
                            {
                                padding_length = min_str.Length;
                            }
                        }



                        string[][] arr_out = new string[arr_in.Length][];
                        for (Int32 i = 0; i < arr_in.Length; i++)
                        {
                            arr_out[i] = new string[arr_in[i].Length];
                            for (Int32 j = 0; j < arr_in[i].Length; j++)
                            {
                                try
                                {
                                    arr_out[i][j] = System.Convert.ToString(arr_in[i][j], 10);
                                    if (padding == true)
                                    {
                                        if (right_padding == true)
                                        {
                                            arr_out[i][j] = arr_out[i][j].PadRight(padding_length, ' ');
                                        }
                                        else
                                        {
                                            arr_out[i][j] = arr_out[i][j].PadLeft(padding_length, ' ');
                                        }
                                    }
                                }
                                catch
                                {
                                    ReportFunctions.ReportError(i, j);
                                }
                            }
                        }
                        // space for code. end.
                        return arr_out;
                    }
                }
            }


        }



    }










    /* 
        2023.09.02 19:50. About array.
        1. 2023.09.02 19:50. assigning array 1 to array 2 means array 2 will have the same
        changes array 1 has.
        2. 2023.09.02 19:52. return array 1 from function via return without making local variable 
        does the same.
        3. 2023.09.02 19:53. to 2. note. making local array and assign input array does the same,
        making new array and then assign input array does the same.
        4. 2023.09.02 19:58. there is copy function written in array my functions.
        tested. it creates copy of array.
        */
    public static class ArrayFunctions
    {

        static Random _internal_random = new Random();



        // template for code execution. 2024.03.08 20:57. Warsaw. Hostel.
        /*
        float execution_time_ms_start = 0;
        if (TimeExecutionShow == true)
            {
                execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
            }
        if (TimeExecutionShow == true)
            {
                float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                TimeExecutionMessage(nameof(function_name_here), execution_time_ms_stop - execution_time_ms_start);
            }
        */



        /// <summary>
        /// Written. 2024.03.08 20:57. Warsaw. Hostel.
        /// </summary>
        public static bool TimeExecutionShow
        {
            get
            {
                return _time_execution_bool;
            }

            set
            {
                if (value == true)
                {
                    _time_execution_bool = true;
                    _time_execution.Start();
                    return;
                }
                if (value == false)
                {
                    _time_execution_bool = false;
                    _time_execution.Stop();
                    _time_execution.Reset();
                    return;

                }
            }
        }

        static bool _time_execution_bool = false;

        static Stopwatch _time_execution = new Stopwatch();
        static Int32 _time_execution_count = 0;
        /// <summary>
        /// Added. 2024.03.08 20:59. Warsaw. Hostel.
        /// </summary>
        /// <param name="function_name"></param>
        /// <param name="total_ms_passed"></param>
        static void TimeExecutionMessage(string function_name, float total_ms_passed)
        {
            _time_execution_count += 1;
            Console.WriteLine(_time_execution_count.ToString() + ". " + DateTime.Now.ToString("HH:mm:ss") + " " + function_name +
                " exectuion time: " + total_ms_passed.ToString("0.000") + " ms");
        }

        /// <summary>
        /// 2023.09.02 19:41. written.
        /// Tested. Works. 2023.12.26 12:19. Warsaw. Workplace. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr_in"></param>
        /// <returns></returns>
        public static T[] Copy<T>(T[] arr_in)
        {
            
            if (arr_in.Length == 0)
            {
                // 2023.09.02 19:40. no error. new array is made and no reference to the old array.
            }
            T[] arr_out = new T[arr_in.Length];
            Array.Copy(arr_in, arr_out, arr_out.Length);
            return arr_out;
        }
        /// <summary>
        /// Written. 2023.12.21 15:38. Workplace.
        /// </summary>
        public static class Column
        {


            /// <summary>
            /// Takes from array[][] column and returns array[] <br></br>
            /// Written. 2023.12.26 13:05. Warsaw. Workplace. <br></br>
            /// Note. 1st dimension is column count <br></br>
            /// Tested. Works. 2023.12.26 13:12. Warsaw. Workplace.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="arr_in"></param>
            /// <param name="column_num"></param>
            /// <returns></returns>
            public static T[] Take<T>(T[][] arr_in, Int32 column_num)
            {
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return new T[0];
                }
                if (column_num < 0)
                {
                    ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Index_is_wrong);
                    return new T[0];
                }
                // 2023.12.26 13:08. Warsaw. Workplace. Check if it is reference or copied array.
                // 2023.12.26 13:12. Warsaw. Workplace. Checked it is reference. Code was updated.  

                // T[] arr_out = arr_in[column_num];                
                T[] arr_out = ArrayFunctions.Copy(arr_in[column_num]);


                return arr_out;
            }




            /// <summary>
            /// Written. 2024.02.09 14:08. Warsaw. Workplace. 
            /// </summary>
            public static class Remove
            {

                /// <summary>
                /// Written. 2024.02.09 14:10. Warsaw. Workplace. 
                /// Tested. Works. 2024.02.09 13:33. Warsaw. Workplace. 
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="arr_in"></param>
                /// <param name="start_index"></param>
                /// <param name="remove_count"></param>
                /// <returns></returns>
                public static T[] FromStartByLength<T>(T[] arr_in, UInt32 remove_count)
                {
                    return Extract.FromIndexByLength(arr_in, remove_count, (uint)arr_in.Length - remove_count);
                }






                /// <summary>
                /// Written. 2024.02.09 13:28. Warsaw. Workplace. 
                /// Tested. Works. 2024.02.09 15:15. Warsaw. Workplace.  
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="arr_in"></param>
                /// <param name="start_index"></param>
                /// <param name="remove_count"></param>
                /// <returns></returns>
                public static T[] FromIndexByLength<T>(T[] arr_in, UInt32 start_index, UInt32 remove_count)
                {
                    T[] arr_part_1 = Extract.FromIndexByLength(arr_in, 0, start_index - 1 + 1);
                    T[] arr_part_2 = Extract.FromIndexByLength(arr_in, start_index + remove_count, (uint)arr_in.Length - (start_index + remove_count));
                    T[] arr_out = Merge.A_B_To_C(arr_part_1, arr_part_2);
                    return arr_out;
                }


                /// <summary>
                /// Remove from Array element and returns Array with size lower by 1. <br></br> 
                /// Written. 2023.12.21 15:41. Workplace. <br></br>
                /// Tested. Works. 2023.12.26 12:30. Warsaw. Workplace.
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="arr_in"></param>
                /// <param name="row_num"></param>
                /// <returns></returns>
                public static T[] OneElement<T>(T[] arr_in, Int32 row_num)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new T[0];
                    }
                    if (row_num < 0)
                    {
                        ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Index_is_wrong);
                        return new T[0];
                    }
                    T[] arr_out = new T[arr_in.Length - 1];
                    Int32 ifill = 0;
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        if (i == row_num)
                        {
                            continue;
                        }
                        arr_out[ifill] = arr_in[i];
                        ifill++;
                    }
                    return arr_out;
                }
            }
        }
        public static class Row
        {

            /// <summary>
            /// Written. 2024.04.19 15:40. Warsaw. Workplace.
            /// </summary>
            public static class AddWithShift
            {
                
                /// <summary>
                /// Written. 2024.04.19 15:40. Warsaw. Workplace.
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="arr_in"></param>
                /// <param name="add_data"></param>
                /// <returns></returns>
                public static T[] ToStart<T>(T[] arr_in, T add_data)
                {
                    T[] arr_out = new T[arr_in.Length];
                    Array.Copy(arr_in, 0, arr_out, 1, arr_in.Length - 1);
                    arr_out[0] = add_data;
                    return arr_out;
                }

                /// <summary>
                /// Written. 2024.04.19 15:41. Warsaw. Workplace.
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="arr_in"></param>
                /// <param name="add_data"></param>
                /// <returns></returns>
                public static T[] ToEnd<T>(T[] arr_in, T add_data)
                {
                    T[] arr_out = new T[arr_in.Length];
                    Array.Copy(arr_in, 1, arr_out, 0, arr_in.Length - 1);
                    arr_out[arr_out.Length - 1] = add_data;
                    return arr_out;
                }

            }



                /// <summary>
                /// Takes from array[][] row and returns array[] <br></br>
                /// Written. 2023.12.21 15:37. Warsaw. Workplace. <br></br>
                /// Note. 1st dimension is column count <br></br>
                /// Tested. Works. 2023.12.26 13:02. Warsaw. Workplace. 
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="arr_in"></param>
                /// <param name="row_num"></param>
                /// <returns></returns>
                public static T[] Take<T>(T[][] arr_in, Int32 row_num)
            {
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return new T[0];
                }
                if (row_num < 0)
                {
                    ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Index_is_wrong);
                    return new T[0];
                }
                T[] arr_out = new T[arr_in.Length];
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    arr_out[i] = arr_in[i][row_num];
                }
                return arr_out;
            }



            /// <summary>
            /// Written. 2024.02.09 14:13. Warsaw. Workplace. 
            /// </summary>
            public static class Remove
            {


                /// <summary>
                /// Removes 1 row from Array[][] and returns Array[][] with size of 1st array lower by 1 <br></br>
                /// Note. 1st dimension is column count <br></br>
                /// Written. 2023.12.21 15:34. Workplace. <br></br>
                /// Tested. Works. 2023.12.26 12:59. Warsaw. Workplace. 
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="arr_in"></param>
                /// <param name="row_num"></param>
                /// <returns></returns>
                public static T[][] OneRow<T>(T[][] arr_in, Int32 row_num)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new T[0][];
                    }
                    if (row_num < 0)
                    {
                        ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Index_is_wrong);
                        return new T[0][];
                    }
                    T[][] arr_out = new T[arr_in.Length][];
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        arr_out[i] = Column.Remove.OneElement(arr_in[i], row_num);
                    }
                    return arr_out;
                }

                /// <summary>
                /// Removes range of rows from start of array[][]<br></br>
                /// Written. 2024.02.09 14:16. Warsaw. Workplace. <br></br>
                /// Tested. Works. 2024.02.09 15:16. Warsaw. Workplace. 
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="arr_in"></param>
                /// <param name="remove_amount"></param>
                /// <returns></returns>
                public static T[][] FromStartByAmount<T>(T[][] arr_in, UInt32 remove_amount)
                {
                    return FromIndexByAmount(arr_in, 0, remove_amount);
                }


                /// <summary>
                /// Removes range of rows from end of array[][]<br></br>
                /// Written. 2024.02.09 14:58. Warsaw. Workplace. <br></br>
                /// Tested. Works. 2024.02.09 15:15. Warsaw. Workplace. 
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="arr_in"></param>
                /// <param name="remove_amount"></param>
                /// <returns></returns>
                public static T[][] FromEndByAmount<T>(T[][] arr_in, UInt32 remove_amount)
                {
                    // arr_in.Length - 1 - remove_amount + 1. 2024.02.09 14:59. Warsaw. Workplace. 
                    return FromIndexByAmount(arr_in, (uint)arr_in[0].Length - remove_amount, remove_amount);
                }

                /// <summary>
                /// Removes range of rows in array[][] starting from provided index. <br></br>
                /// Written. 2024.02.09 13:29. Warsaw. Workplace. <br></br>  
                /// Tested. Works. 2024.02.09 15:15. Warsaw. Workplace. <br></br>
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="arr_in"></param>
                /// <param name="start_row"></param>
                /// <param name="remove_amount"></param>
                /// <returns></returns>
                public static T[][] FromIndexByAmount<T>(T[][] arr_in, UInt32 start_row, UInt32 remove_amount)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new T[0][];
                    }
                    if (start_row < 0)
                    {
                        ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Index_is_wrong);
                        return new T[0][];
                    }
                    T[][] arr_out = new T[arr_in.Length][];
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        arr_out[i] = Column.Remove.FromIndexByLength(arr_in[i], start_row, remove_amount);
                    }
                    return arr_out;
                }
            }






        }
        /// <summary>
        /// Checks if Array 1 contains Array 2.
        /// 2023.08.23 09:30. written.
        /// 2023.08.23 09:35. tested. works.
        /// /// </summary>
        /// <param name="arr_search_in"></param>
        /// <param name="arr_search"></param>
        /// <returns></returns>
        public static bool Contains<T>(T[] arr_search_in, T[] arr_search)
        {
            if (arr_search.Length > arr_search_in.Length)
                return false;
            for (Int32 a = 0; a <= arr_search_in.Length - arr_search.Length; a++)
            {
                if (arr_search_in[a].Equals(arr_search[0]))
                {
                    for (Int32 i = 0; i < arr_search.Length; i++)
                    {
                        if (arr_search_in[a + i].Equals(arr_search[i]) == false)
                        {
                            break;
                        }
                        if (i == (arr_search.Length - 1))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if Array 1 contains Array 2.
        /// 2023.08.23 10:32 written.
        /// 2023.08.23 09:35. not tested. 
        /// Int32[4][10] 4 are columns
        /// Int32[20][5] 20 are rows.
        /// /// </summary>
        /// <param name="arr_search_in"></param>
        /// <param name="arr_search"></param>
        /// <returns></returns>
        public static bool Contains<T>(T[][] arr_search_in, T[] arr_search)
        {
            if (arr_search.Length > arr_search_in.Length)
                return false;
            for (Int32 a = 0; a < arr_search_in.Length; a++)
            {
                if (Contains(arr_search_in[a], arr_search) == true)
                {
                    return true;
                }
            }
            return false;
        }

        

            public static class Split
        {
            
            
            
            /// <summary>
            /// Splits array on two arrays starting from certain index. <br></br>
            /// Written. 2024.02.08 16:48. Warsaw. Workplace. <br></br>
            /// Tested. Works. 2024.02.08 17:03. Warsaw. Workplace. 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="arr_in"></param>
            /// <param name="index_split"></param>
            /// <returns></returns>
            public static T[][] FromIndex<T>(T[] arr_in, Int32 index_split)
            {
                T[][] arr_out = new T[2][];
                // -1 for before index, +1 - from index to length. 2024.02.08 16:45. Warsaw. Workplace. 
                arr_out[0] = new T[index_split - 1 + 1];
                Array.Copy(arr_in, arr_out[0], arr_out[0].Length);
                arr_out[1] = new T[arr_in.Length - arr_out[0].Length];
                Array.Copy(arr_in, index_split, arr_out[1], 0, arr_out[1].Length);
                return arr_out;
            }
            
            
            
            
            

            public class SplitOptions
            {
                bool FromIndexToIndex = false;
                bool RemoveSplitValue = false;
                bool SaveBefore1stIndex = false;
                bool SaveAfterLastIndex = false;
                bool SplitValue1st = false;
                bool SplitValueLast = false;
            }
            /// <summary>
            /// Not fully completed. 2024.02.08 16:43. Warsaw. Workplace. 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="array_in"></param>
            /// <param name="indexes_in"></param>
            /// <param name="option"></param>
            /// <returns></returns>
            public static T[][] ByIndex<T>(T[] array_in, Int32[] indexes_in, SplitOptions option)
            {
                return new T[0][];
            }
            /// <summary>
            /// Splits array A on Array NxM
            /// 2023.08.22 15:00. written.
            /// 2023.08.23 10:00. tested. works.
            /// </summary>
            /// <param name="array_in"></param>
            /// <param name="columns_number">required number of columns</param>
            /// <param name="columns_length_equal"></param>
            /// <returns></returns>
            public static T[][] A_To_MxN<T>(T[] array_in, Int32 columns_number, bool columns_length_equal = true)
            {
                if (columns_length_equal == true)
                {
                    if ((array_in.Length % columns_number) != 0)
                    {
                        ReportFunctions.ReportError("Columns have different length\r\n" +
                            "array length: " + array_in.Length.ToString() + "\r\n" +
                            "to split on: " + columns_number.ToString() + "\r\n" +
                            "division reminder: " + (array_in.Length % columns_number).ToString());
                        return new T[0][];
                    }
                    T[][] array_out = new T[columns_number][];
                    Int32 arr_copy_index = 0;
                    for (Int32 i = 0; i < array_out.Length; i++)
                    {
                        array_out[i] = new T[array_in.Length / columns_number];
                        Array.Copy(array_in, arr_copy_index, array_out[i], 0, array_out[i].Length);
                        arr_copy_index += array_out[i].Length - 1 + 1;
                    }
                    return array_out;
                }
                else
                {
                    T[][] array_out = new T[columns_number + 1][];
                    Int32 arr_copy_index = 0;
                    for (Int32 i = 0; i < array_out.Length - 1; i++)
                    {
                        array_out[i] = new T[array_in.Length / columns_number];
                        Array.Copy(array_in, arr_copy_index, array_out[i], 0, array_out[i].Length);
                        arr_copy_index += array_out[i].Length - 1 + 1;
                    }
                    // last column
                    Int32 last_column_index = array_out.Length - 1;
                    array_out[last_column_index] = new T[array_in.Length / columns_number];
                    Array.Copy(array_in, arr_copy_index, array_out[last_column_index], 0, array_in.Length % columns_number);
                    // 23.08.2023 12:59
                    // can not be done
                    // uncomment to see
                    //if (typeof(T) == typeof(string))
                    // {
                    //     for (Int32 i = (array_in.Length % columns_number) - 1 + 1; i < array_out[last_column_index].Length; i++)
                    //     {
                    //         array_out[last_column_index] = "reminder is " + (array_in.Length % columns_number).ToString();
                    //     }
                    // }
                    // if (typeof(T) == typeof(int))
                    // {
                    //     for (Int32 i = (array_in.Length % columns_number) - 1 + 1; i < array_out[last_column_index].Length; i++)
                    //     {
                    //         array_out[last_column_index] = -255;
                    //     }
                    // }
                    return array_out;
                }
            }
            // by index
            // arr, indexes, SplitOptions
        }
        public static class Extract
        {


            /// <summary>
            /// Extact array of certain length from the start of array <br></br>
            /// Written. 2024.01.15 17:15. Warsaw. Workplace. <br></br>
            /// Tested. Works. 2024.01.15 17:28. Warsaw. Workplace.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="arr_in"></param>
            /// <param name="length_arr"></param>
            /// <returns></returns>
            public static T[] FromStartByLength<T>(T[] arr_in, UInt32 length_arr)
            {
                T[] arr_out = new T[length_arr];
                Array.Copy(arr_in, arr_out, arr_out.Length);
                return arr_out;
            }

          


            /// <summary>
            /// Returns Array[A][B] using provided A, B from Array[C][D].<br></br>
            /// Written. 2023.11.08 19:40. Warsaw. Workplace. <br></br>
            /// Tested. Works. 2024.02.10 17:39. Warsaw. Hostel.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="arr_in"></param>
            /// <param name="row_index"></param>
            /// <param name="rows_count"></param>
            /// <param name="column_index"></param>
            /// <param name="columns_count"></param>
            /// <returns></returns>
            public static T[][] PartAxBFromCxD<T>(T[][] arr_in, UInt32 row_index, UInt32 rows_count,
                UInt32 column_index, UInt32 columns_count)
            {
                UInt32 rows_size = row_index + rows_count;
                UInt32 columns_size = column_index + columns_count;
                UInt32 ifill = 0;
                UInt32 jfill = 0;
                T[][] arr_out = new T[columns_count][];
                for (UInt32 j = column_index; j < columns_size; j++)
                {
                    arr_out[jfill] = new T[rows_count];
                    ifill = 0;
                    for (UInt32 i = row_index; i < rows_size; i++)
                    {
                        arr_out[jfill][ifill] = arr_in[j][i];
                        ifill++;
                    }
                    jfill++;
                }
                return arr_out;
            }
            /// <summary>
            /// Return array of element below index of source array
            /// 2023.08.12 08:20
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="arr_in"></param>
            /// <param name="index"></param>
            /// <returns></returns>
            public static T[] BelowIndex<T>(T[] arr_in, Int32 index)
            {
                // 2023-07-03 16:17 length to index, then - start_index, then index to length
                T[] arr_out = new T[arr_in.Length - (index + 1)];
                Array.Copy(arr_in, index + 1, arr_out, 0, arr_out.Length);
                return arr_out;
            }

            /// <summary>
            /// Extracts array starting from defined index and with defined length. <br></br>
            /// Written. 2024.01.16 08:31. Warsaw. Workplace. <br></br>
            /// Tested. Works. 2024.01.16 08:38. Warsaw. Workplace.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="arr_in"></param>
            /// <param name="index"></param>
            /// <param name="length"></param>
            /// <returns></returns>
            public static T[] FromIndexByLength<T>(T[] arr_in, UInt32 index, UInt32 length)
            {
                if (index + 1 + length > arr_in.Length)
                {
                    ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Length_is_exceeded);
                    T[] error_arr_out = new T[length];
                    Array.Copy(arr_in, arr_in.Length - length - 1 + 1, error_arr_out, 0, error_arr_out.Length);
                    return error_arr_out;
                    // Tested. Works. Warsaw. Workplace. 2024-07-17 17-07.                  
                }
                
                
                T[] arr_out = new T[length];
                Array.Copy(arr_in, index, arr_out, 0, arr_out.Length);
                return arr_out;
            }


            public static T[] FromIndexToIndex<T>(T[] arr_in, Int32 index_start, Int32 index_end)
            {
                Int32 size_arr = index_end - index_start + 1;
                T[] arr_out = new T[size_arr];
                Array.Copy(arr_in, index_start, arr_out, 0, arr_out.Length);
                return arr_out;
            }
            public static T[] AboveIndex<T>(T[] arr_in, Int32 index)
            {
                T[] arr_out = new T[index + 1 - 1];
                // comment for the [index + 1 - 1]
                // 2023-07-03 16:33 index to length and then before index
                Array.Copy(arr_in, arr_out, arr_out.Length);
                return arr_out;
            }
        }
        /// <summary>
        /// Sort array according to provided indexes
        /// 2023.08.12 12:14
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr_in"></param>
        /// <param name="indexes_in"></param>
        /// <returns></returns>
        public static T[] Sort<T>(T[] arr_in, Int32[] indexes_in)
        {
            T[] arr_out = new T[arr_in.Length];
            for (Int32 i = 0; i < arr_out.Length; i++)
            {
                try
                {
                    arr_out[i] = arr_in[indexes_in[i]];
                }
                catch (Exception e)
                {
                    ReportFunctions.ReportError("Error! check input arrays\r\nError message: " + e.Message);
                }
            }
            return arr_out;
        }
        /// <summary>
        /// Sort AxB array according to indexes provided
        /// 2023.08.12 12:14
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr_in"></param>
        /// <param name="indexes_in"></param>
        /// <returns></returns>
        public static T[][] Sort<T>(T[][] arr_in, Int32[] indexes_in)
        {
            if (indexes_in.Length == 0)
            {
                ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Length_is_0);
                return arr_in;
            }
            T[][] arr_out = new T[arr_in.Length][];
            for (Int32 i = 0; i < arr_out.Length; i++)
            {
                try
                {
                    arr_out[i] = Sort(arr_in[i], indexes_in);
                }
                catch (Exception e)
                {
                    ReportFunctions.ReportError(i);
                }
            }
            return arr_out;
        }



        /// <summary>
        /// Written. 2024.02.08 16:39. Warsaw. Workplace. 
        /// </summary>
        public static class Insert
        {

            /// <summary>
            /// Inserts array into another array starting from certain index. <br></br>
            /// Written. 2024.02.08 16:52. Warsaw. Workplace. <br></br>
            /// Tested. Works. 2024.02.08 17:01. Warsaw. Workplace. <br></br>
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="arr_1"></param>
            /// <param name="arr_2"></param>
            /// <param name="from_index"></param>
            /// <returns></returns>
            public static T[] FromIndex<T>(T[] arr_1, T[] arr_2, Int32 from_index)
            {
                T[][] arr_split = ArrayFunctions.Split.FromIndex(arr_1, from_index);
                T[] arr_out = new T[arr_1.Length + arr_2.Length];
                Int32 index_for_insert = 0;
                Array.Copy(arr_split[0], arr_out, arr_split[0].Length);
                index_for_insert = arr_split[0].Length - 1 + 1;
                Array.Copy(arr_2, 0, arr_out, index_for_insert, arr_2.Length);
                index_for_insert += arr_2.Length;
                Array.Copy(arr_split[1], 0, arr_out, index_for_insert, arr_split[1].Length);
                return arr_out;
            }


        }


        public static class Merge
        {
            public static T[][] A_BxA_To_CxA<T>(T[] arr_1, T[][] arr_2)
            {
                T[][] arr_out = new T[arr_2.Length + 1][];
                for (Int32 i = 0; i < arr_2.Length; i++)
                {
                    arr_out[i] = new T[arr_1.Length];
                    Array.Copy(arr_2[i], arr_out[i], arr_2[i].Length);
                }
                arr_out[arr_out.Length - 1] = new T[arr_1.Length];
                Array.Copy(arr_1, arr_out[arr_out.Length - 1], arr_1.Length);
                return arr_out;
            }



            /// <summary>
            /// Takes Array[n] and Array[n] and returns Array[2][n]. <br></br>
            /// Tested. Works. 2024.01.16 10:56. Warsaw. Workplace.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="arr_1"></param>
            /// <param name="arr_2"></param>
            /// <returns></returns>
            public static T[][] A_B_To_NxM<T>(T[] arr_1, T[] arr_2)
            {
                T[][] arr_out = new T[2][];
                arr_out[0] = new T[arr_1.Length];
                Array.Copy(arr_1, arr_out[0], arr_1.Length);
                arr_out[1] = new T[arr_2.Length];
                Array.Copy(arr_2, arr_out[1], arr_2.Length);
                return arr_out;
            }
            public static T[] A_B_To_C<T>(T[] arr_1, T[] arr_2)
            {
                T[] arr_out = new T[arr_1.Length + arr_2.Length];
                Array.Copy(arr_1, arr_out, arr_1.Length);
                Array.Copy(arr_2, 0, arr_out, arr_1.Length, arr_2.Length);
                return arr_out;
            }
            /// <summary>
            /// Takes each Int32[m] from Int32[n][m] and makes one array Int32[nxm] <br></br>
            /// Tested. Works. 2023.11.08 15:48. Warsaw. Workplace. <br></br>
            /// 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="array_in">Array[n][m]</param>
            /// <returns>Array[size = nxm]</returns>
            /// Note. 2023.11.08 15:49. Warsaw. Workplace. 
            /// Tested 6-9 month ago. 
            public static T[] NxM_To_A<T>(T[][] array_in)
            {
                Int32 arr_size = 0;
                for (Int32 i = 0; i < array_in.Length; i++)
                {
                    arr_size += array_in[i].Length;
                }
                T[] arr_out = new T[arr_size];
                Int32 arr_out_fill_index = 0;
                for (Int32 i = 0; i < array_in.Length; i++)
                {
                    if (i == 0)
                    {
                        Array.Copy(array_in[i], arr_out, array_in[i].Length);
                        continue;
                    }
                    arr_out_fill_index += array_in[i - 1].Length;
                    Array.Copy(array_in[i], 0, arr_out, arr_out_fill_index, array_in[i].Length);
                }
                return arr_out;
            }
        }
        public enum ArraySortingEnum
        {
            NoSorting,
            Descending,
            Ascenidng
        }
        /// <summary>
        /// Written. 2023.12.21 12:35. Workplace
        /// </summary>
        public static class UInt16Array
        {



            /// <summary>
            /// Written. 2024.03.22 13:06. Warsaw. Workplace.
            /// </summary>
            public static class Generate
            {
                /// <summary>
                /// Generate UInt16[] filled with random numbers <br></br>
                /// Written. 2024.03.22 13:06. Warsaw. Workplace. <br></br>
                /// Tested. Works.2024.03.22 13:27. Warsaw. Workplace.
                /// </summary>
                /// <param name="numbers_num"></param>
                /// <param name="min"></param>
                /// <param name="max"></param>
                /// <returns></returns>
                public static ushort[] RandomMinMaxValue(ushort numbers_num, ushort min, ushort max)
                {
                    ushort[] arr_out = new ushort[numbers_num];
                    for (int i = 0; i < arr_out.Length; i++)
                    {
                        arr_out[i] = (ushort)_internal_random .Next(min, max + 1);                                                
                    }
                    return arr_out;
                }
            }


            /// <summary>
            /// Written. 2023.12.21 12:39. Workplace.
            /// </summary>
            public static class Convert
            {






                /// <summary>
                /// Converts UInt16[] to filestring according to required number format, delimer and missing number string (defined by char). <br></br> 
                /// Written. 2024.03.22 13:31. Warsaw. Workplace. <br></br>
                /// Tested. Works. 2024.03.22 13:34. Warsaw. Workplace. <br></br>
                /// </summary>
                /// <param name="arr_in"></param>
                /// <param name="num_per_row"></param>
                /// <param name="delimer"></param>
                /// <param name="missing_number_char">Defines missing number string. The string will be of the length of the longest number with this char</param>
                /// <param name="base_in"></param>
                /// <param name="pad_number"></param>
                /// <returns></returns>
                public static string ToFileString(ushort[] arr_in, uint num_per_row, int base_in = 10, string delimer = "\t", char missing_number_char = '.', int pad_number = -1)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return "";
                    }

                    int rows_num = arr_in.Length / (int)num_per_row;
                    bool IsNotRectangle = false;
                    if ((arr_in.Length % num_per_row) != 0)
                    {
                        rows_num += 1;
                        IsNotRectangle = true;
                    }

                    string[] strings_filestring = new string[rows_num];

                    StringBuilder write_string = new StringBuilder();

                    int col_index = 0;

                    int pad_length = pad_number;

                    if (base_in == 10)
                    {
                        if (pad_number == -1)
                        {
                            uint max_int = arr_in.Max();
                            pad_length = max_int.ToString().Length;
                        }
                    }

                    if (base_in == 16)
                    {
                        pad_length = 4;
                    }

                    for (int i = 0; i < arr_in.Length; i++)
                    {
                        if (base_in == 10)
                        {
                            write_string.Append(System.Convert.ToString(arr_in[i], base_in).ToUpper().PadRight(pad_length, ' '));
                        }
                        if (base_in == 16)
                        {
                            write_string.Append(System.Convert.ToString(arr_in[i], base_in).ToUpper().PadLeft(pad_length, '0'));
                        }
                        write_string.Append(delimer);
                        col_index += 1;
                        if (col_index > (num_per_row - 1))
                        {
                            write_string.Append("\r\n");
                            col_index = 0;
                        }
                    }

                    if (IsNotRectangle == true)
                    {
                        int row_filled = arr_in.Length % (int)num_per_row;
                        for (int i = 0; i < (num_per_row - row_filled); i++)
                        {
                            write_string.Append("".PadRight(pad_length, missing_number_char));
                            write_string.Append(delimer);
                        }
                        write_string.Append("\r\n");
                    }

                    string return_string = write_string.ToString();

                    return return_string;

                }







                /// <summary>
                /// Converts UInt16[] to string[]. <br></br>
                /// Written. 2024.01.16 10:11. Warsaw. Workplace. <br></br>
                /// Tested. Works. 2024.01.16 10:52. Warsaw. Workplace.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static string[] ToStringArray(UInt16[] arr_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new string[0];
                    }
                    string[] arr_out = new string[arr_in.Length];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        try
                        {
                            arr_out[i] = System.Convert.ToString(arr_in[i]);
                        }
                        catch
                        {
                            ReportFunctions.ReportError(i);
                            return new string[0];
                        }
                    }
                    return arr_out;
                }



                /// <summary>
                /// Converts UInt16[][] to string[][] <br></br>
                /// Written. 2023.12.21 12:39. Workplace. <br></br>
                /// Tested. Works. 2023.12.21 12:52. Workplace.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static string[][] ToStringArray(UInt16[][] arr_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new string[0][];
                    }
                    string[][] arr_out = new string[arr_in.Length][];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = new string[arr_in[i].Length];
                        for (Int32 j = 0; j < arr_in[i].Length; j++)
                        {
                            try
                            {
                                arr_out[i][j] = System.Convert.ToString(arr_in[i][j]);
                            }
                            catch
                            {
                                ReportFunctions.ReportError(i, j);
                            }
                        }
                    }
                    return arr_out;
                }
            }


            /// <summary>
            /// Print UInt16[] in console. There is numbering elements that can be turned on/off<br></br>
            /// Written. 2024.03.22 13:21. Warsaw. Workplace. <br></br>
            /// Tested. Works. 2024.03.22 13:27. Warsaw. Workplace.
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="numbering_elements"></param>
            public static void ToConsole(UInt16[] arr_in, bool numbering_elements = true)
            {
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return;
                }

                Console.WriteLine("Array" + typeof(UInt16).Name.ToString() + ". Length is " + arr_in.Length.ToString());
                for (int i = 0; i < arr_in.Length; i++)
                {
                    if (numbering_elements == true)
                    {
                        Console.Write(i.ToString() + ".\t");
                    }
                    Console.WriteLine(arr_in[i].ToString());
                }

            }
            


                /// <summary>
                /// Prints UInt16[][] to Console <br></br>
                /// Written. 2023.12.21 12:36. Workplace. <br></br>
                /// Tested. Works. 2023.12.21 12:52. Workplace.
                /// </summary>
                /// <param name="arr_in"></param>
                public static void ToConsole(UInt16[][] arr_in)
            {
                if (arr_in.Length == 0)
                {
                    Console.WriteLine("Attention! Array is empty");
                    return;
                }
                Console.WriteLine("Array" + typeof(UInt16).Name.ToString() + ". Length is " + arr_in.Length.ToString() + "x" + arr_in[0].Length.ToString());
                UInt16[] array_all_values = Merge.NxM_To_A(arr_in);
                string min_num = array_all_values.Min().ToString();
                string max_num = array_all_values.Max().ToString();
                Int32 pad_size = max_num.Length;
                if (max_num.Length < min_num.Length)
                {
                    pad_size = min_num.Length;
                }
                string[][] str_arr = UInt16Array.Convert.ToStringArray(arr_in);
                str_arr = StringArray.Pad.Right(str_arr, pad_size);
                string str_for_console = StringArray.Convert.ToFileString(str_arr, "".PadRight(3, ' '));
                // 2023.12.21 12:49. Workplace. FileString is in use with "\r\n" at the last line and therefore there is no need WriteLine. Write instead.
                //Console.WriteLine(str_for_console);
                Console.Write(str_for_console);
            }
        }



        /// <summary>
        /// Written. 2024.01.16 09:38. Warsaw. Workplace.
        /// not tested
        /// </summary>
        public static class FloatArray
        {


            /// <summary>
            /// Written. 2024.01.16 10:17. Warsaw. Workplace.
            /// </summary>
            public static class Merge
            {

                /// <summary>
                /// Written. 2024.01.16 10:20. Warsaw. Workplace.
                /// not tested
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="arr_1"></param>
                /// <param name="arr_2"></param>
                /// <returns></returns>
                public static float[][] A_B_To_NxM(float[] arr_1, float[] arr_2)
                {
                    float[][] arr_out = new float[2][];
                    arr_out[0] = new float[arr_1.Length];
                    Array.Copy(arr_1, arr_out[0], arr_1.Length);
                    arr_out[1] = new float[arr_2.Length];
                    Array.Copy(arr_2, arr_out[1], arr_2.Length);
                    return arr_out;
                }

            }


            /// <summary>
            /// Generates Float Array with numbers from 0 to (n - 1) and with 1 decimal number from 0 to (n - 1) devided by 10 <br></br>
            /// Written. 2024.01.11 16:00. Warsaw. Workplace. <br></br>
            /// Tested. Works. 2024.01.11 16:04. Warsaw. Workplace.
            /// </summary>
            public static float[] Generate(UInt32 num_count, bool negative_numbers = false)
            {
                float[] arr_out = new float[num_count];
                for (Int32 i = 0; i < num_count; i++)
                {
                    arr_out[i] = (float)i + (float)i / (float)10;
                    if (negative_numbers == true)
                    {
                        arr_out[i] = -arr_out[i];
                    }
                }
                return arr_out;
            }


            /// <summary>
            /// Written. 2024.01.11 15:24. Warsaw. Workplace.
            /// 
            /// </summary>
            public static class Convert
            {


                /// <summary>
                /// Converts float[] to string[] with defined decimal placees and defined decimal point. <br></br>
                /// Written. 2024.01.16 10:14. Warsaw. Workplace. <br></br>
                /// Tested. Works. 2024.01.16 10:54. Warsaw. Workplace.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <param name="decimal_digits"></param>
                /// <param name="decimal_point"></param>
                /// <returns></returns>
                public static string[] ToStringArray(float[] arr_in, Int32 decimal_digits, char decimal_Point = '.')
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new string[0];
                    }
                    string[] arr_out = new string[arr_in.Length];
                    string float_format = "0" + "." + 0.ToString().PadRight(decimal_digits, '0');
                    if (decimal_digits == 0)
                    {
                        float_format = "0";
                    }
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        try
                        {
                            arr_out[i] = arr_in[i].ToString(float_format);
                            if (decimal_Point == '.')
                            {
                                arr_out[i] = arr_out[i].Replace(',', '.');
                            }
                        }
                        catch
                        {
                            ReportFunctions.ReportError(i);
                            return new string[0];
                        }
                    }
                    return arr_out;
                }



                /// <summary>
                /// Converts Float Array to UInt32 Array by bit conversion. It makes bits of float to be bits of UInt32 <br></br>
                /// Written. 2024.01.11 15:24. Warsaw. Workplace. <br></br>
                /// Tested. Works. 2024.01.11 16:37. Warsaw. Workplace.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static UInt32[] ToUInt32ArrayByBitConversion(float[] arr_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new UInt32[0];
                    }

                    UInt32[] arr_out = new UInt32[arr_in.Length];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = MathFunctions.FloatNumber.Convert.ToUInt32ByBitConversion(arr_in[i]);

                    }

                    return arr_out;


                }
            }
        }


        /// <summary>
        /// Written. 2023.12.20 12:57. Workplace
        /// </summary>
        public static class Int16Array
        {
            /// <summary>
            /// Prints Int16[][] to Console <br></br>
            /// Written. 2023.12.20 12:57. Workplace. <br></br>
            /// Tested. Works. 2023.12.20 15:45. Workplace.
            /// </summary>
            /// <param name="arr_in"></param>
            public static void ToConsole(Int16[][] arr_in)
            {
                if (arr_in.Length == 0)
                {
                    Console.WriteLine("Attention! Array is empty");
                    return;
                }
                Console.WriteLine("Array" + typeof(Int16).Name.ToString() + ". Length is " + arr_in.Length.ToString() + "x" + arr_in[0].Length.ToString());
                Int16[] array_all_values = Merge.NxM_To_A(arr_in);
                string min_num = array_all_values.Min().ToString();
                string max_num = array_all_values.Max().ToString();
                Int32 pad_size = max_num.Length;
                if (max_num.Length < min_num.Length)
                {
                    pad_size = min_num.Length;
                }
                string[][] str_arr = Int16Array.Convert.ToStringArray(arr_in);
                str_arr = StringArray.Pad.Right(str_arr, pad_size);
                string str_for_console = StringArray.Convert.ToFileString(str_arr, "".PadRight(3, ' '));
                // 2023.12.21 12:50. Workplace. FileString is in use with "\r\n" at the last line and therefore there is no need WriteLine. Write instead.
                //Console.WriteLine(str_for_console);
                Console.Write(str_for_console);
            }
            /// <summary>
            /// Written. 2023.12.20 12:54. Workplace.
            /// </summary>
            public static class Split
            {
                /// <summary>
                /// Split Int16[] to Int16[n][], where n - required number of columns
                /// Written. 2023.12.20 12:54. Workplace.
                /// 
                /// </summary>
                /// <param name="array_in"></param>
                /// <param name="columns_number"></param>
                /// <param name="columns_length_equal"></param>
                /// <returns></returns>
                public static Int16[][] A_To_MxN(Int16[] array_in, Int32 columns_number, bool columns_length_equal = true)
                {
                    return ArrayFunctions.Split.A_To_MxN(array_in, columns_number, columns_length_equal);
                }
            }
            /// <summary>
            /// Generate Int16[] filled with numbers from 0 to array length - 1 <br></br>
            /// Written. 2023.12.14 14:49. Workplace. <br></br>
            /// Tested. Works. 2023.12.14 14:51. Workplace. <br></br>
            /// </summary>
            /// <param name="arr_size"></param>
            /// <returns>Array with element from 0 to length - 1. <br></br>
            /// Can return array  with length is 0</returns>
            public static Int16[] Generate(Int32 arr_size)
            {
                Int16[] arr_out = new Int16[arr_size];
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    arr_out[i] = (Int16)i;
                }
                return arr_out;
            }
            /// <summary>
            /// Written. 2023.12.14 14:46. Workplace.
            /// </summary>
            public static class Convert
            {

                

                /// <summary>
                /// Converts Int16[][] to UInt16[][] according to bits. Note. negative numbers is converted to positive according to bits. <br></br>
                /// Written. 2023.12.21 12:32. Workplace. <br></br>
                /// Tested. Works. 2023.12.21 12:43. Workplace.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static UInt16[][] ToUInt16AxBArrayByBitConversion(Int16[][] arr_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new UInt16[0][];
                    }
                    UInt16[][] arr_out = new UInt16[arr_in.Length][];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = ToUInt16ArrayByBitConversion(arr_in[i]);
                    }
                    return arr_out;
                }
                /// <summary>
                /// Converts Int16[] to UInt16[] according to bits. Note. negative numbers is converted to positive according to bits. <br></br>
                /// Written. 2023.12.21 12:17. Workplace.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static UInt16[] ToUInt16ArrayByBitConversion(Int16[] arr_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new UInt16[0];
                    }
                    UInt16[] arr_out = new UInt16[arr_in.Length];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = (UInt16)arr_in[i];
                    }
                    return arr_out;
                }
                /// <summary>
                /// Converts Int16[][] to string[][] <br></br>
                /// Written. 2023.12.20 13:06. Workplace. <br></br>
                /// 
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static string[][] ToStringArray(Int16[][] arr_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new string[0][];
                    }
                    string[][] arr_out = new string[arr_in.Length][];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = new string[arr_in[i].Length];
                        for (Int32 j = 0; j < arr_in[i].Length; j++)
                        {
                            try
                            {
                                arr_out[i][j] = System.Convert.ToString(arr_in[i][j]);
                            }
                            catch
                            {
                                ReportFunctions.ReportError(i, j);
                            }
                        }
                    }
                    return arr_out;
                }
                /// <summary>
                /// Converts Int16[] to Int32[] <br></br>
                /// Written. 2023.12.20 12:58. Workplace.<br></br>
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static Int32[] ToInt32Array(Int16[] arr_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new Int32[0];
                    }
                    Int32[] arr_out = new Int32[arr_in.Length];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        try
                        {
                            arr_out[i] = System.Convert.ToInt32(arr_in[i]);
                        }
                        catch
                        {
                            ReportFunctions.ReportError("There should be such error - Int16 to Int32 Conversion. Check the code");
                            return new Int32[0];
                        }
                    }
                    return arr_out;
                }

                /* 
                2024.01.20 22:18. Warsaw. Hostel 
                Moved from Int32Array.

                /// <summary>
                /// Accepts: Int16[]
                /// 2023-07-26 13:14
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="arr_in"></param>
                /// <returns>Int32 Array</returns>
                public static Int32[] ConvertToInt32(Int16[] arr_in)
                {
                    // 2023-07-26 13:16 it is not cast by bits, it is cast by number
                    Int32[] arr_out = new Int32[arr_in.Length];
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        arr_out[i] = System.Convert.ToInt32(arr_in[i]);
                    }
                    return arr_out;
                }

                */




                /// <summary>
                /// Written. 2023.12.14 14:47. Workplace.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static string[] ToStringArray(Int16[] arr_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new string[0];
                    }
                    string[] arr_out = new string[arr_in.Length];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        try
                        {
                            arr_out[i] = System.Convert.ToString(arr_in[i]);
                        }
                        catch
                        {
                            ReportFunctions.ReportError(i);
                            return new string[0];
                        }
                    }
                    return arr_out;
                }
            }
        }


        /// <summary>
        /// Written. 2024.01.11 15:19. Warsaw. Workplace.
        /// </summary>
        public static class UInt32Array
        {

            /// <summary>
            /// Written. 2024.03.21 11:37. Warsaw. Workplace.
            /// </summary>
            public static class Generate
            {

                /// <summary>
                /// Generate UInt32[] filled with random numbers
                /// Written. 2024.03.21 11:40. Warsaw. Workplace.
                /// Tested. Works. 2024.03.21 12:00. Warsaw. Workplace.
                /// </summary>
                /// <param name="numbers_num"></param>
                /// <param name="min"></param>
                /// <param name="max"></param>
                /// <returns></returns>
                public static UInt32[] RandomMinMaxValue(UInt32 numbers_num, UInt32 min, UInt32 max)
                {
                    UInt32[] arr_out = new UInt32[numbers_num];
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        if (max <= int.MaxValue)
                        {
                            arr_out[i] = (uint)_internal_random.Next((int)min, (int)max);
                        }
                          
                        if (max > int.MaxValue)
                        {
                            byte[] bytes_of_number = new byte[4];                           
                            _internal_random.NextBytes(bytes_of_number);


                            arr_out[i] = MathFunctions.UInt32Number.BytesToUInt32(bytes_of_number);
                            // 2024.03.21 11:51. Warsaw. Workplace.
                            // There is trouble with casting int to uint. negative casting gives error or casts to 0
                            /*
                            arr_out[i] = (uint)_internal_random.Next((int)min, int.MaxValue);
                            uint bit_set = (uint)_internal_random.Next(0, 2);                      
                            arr_out[i] |= (bit_set << 32);                            
                             */

                        }
                    }
                    return arr_out;
                }



            }

            /// <summary>
            /// Written. 2024.01.11 15:19. Warsaw. Workplace.
            /// </summary>
            public static class Convert
            {







                /// <summary>
                /// Converts array to filestring according to required number format, delimer and missing number string (defined by char). <br></br> 
                /// Written. 2024.03.21 11:33. Warsaw. Workplace. <br></br>
                /// Tested. Works. 2024.03.21 12:02. Warsaw. Workplace. <br></br> 
                /// </summary>
                /// <param name="arr_in"></param>
                /// <param name="num_per_row"></param>
                /// <param name="delimer"></param>
                /// <param name="missing_number_char">Defines missing number string. The string will be of the length of the longest number with this char</param>
                /// <param name="base_in"></param>
                /// <param name="pad_number"></param>
                /// <returns></returns>
                public static string ToFileString(UInt32[] arr_in, uint num_per_row, int base_in = 10, string delimer = "\t", char missing_number_char = '.', int pad_number = -1)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return "";
                    }

                    int rows_num = arr_in.Length / (int)num_per_row;
                    bool IsNotRectangle = false;
                    if ((arr_in.Length % num_per_row) != 0)
                    {
                        rows_num += 1;
                        IsNotRectangle = true;
                    }

                    string[] strings_filestring = new string[rows_num];

                    StringBuilder write_string = new StringBuilder();

                    int col_index = 0;

                    int pad_length = pad_number;

                    if (base_in == 10)
                    {
                        if (pad_number == -1)
                        {
                            uint max_int = arr_in.Max();
                            pad_length = max_int.ToString().Length;
                        }
                    }

                    if (base_in == 16)
                    {
                        pad_length = 8;
                    }

                    for (int i = 0; i < arr_in.Length; i++)
                    {
                        if (base_in == 10)
                        {
                            write_string.Append(System.Convert.ToString(arr_in[i], base_in).ToUpper().PadRight(pad_length, ' '));
                        }
                        if (base_in == 16)
                        {
                            write_string.Append(System.Convert.ToString(arr_in[i], base_in).ToUpper().PadLeft(pad_length, '0'));
                        }
                        write_string.Append(delimer);
                        col_index += 1;
                        if (col_index > (num_per_row - 1))
                        {
                            write_string.Append("\r\n");
                            col_index = 0;
                        }
                    }

                    if (IsNotRectangle == true)
                    {
                        int row_filled = arr_in.Length % (int)num_per_row;
                        for (int i = 0; i < (num_per_row - row_filled); i++)
                        {
                            write_string.Append("".PadRight(pad_length, missing_number_char));
                            write_string.Append(delimer);
                        }
                        write_string.Append("\r\n");
                    }
                   
                    string return_string = write_string.ToString();

                    return return_string;

                }










                /// <summary>
                /// Converts UInt32Array to FloatArray using bii conversion. Bits of UInt32 become bits of float. <br></br>
                /// Written. 2024.01.11 15:19. Warsaw. Workplace. <br></br>
                /// Tested. Works. 2024.01.11 16:35. Warsaw. Workplace.
                /// </summary>
                public static float[] ToFloatArrayByBitConversion(UInt32[] arr_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new float[0];
                    }

                    float[] arr_out = new float[arr_in.Length];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = MathFunctions.UInt32Number.Convert.ToFloatByBitConversion(arr_in[i]);

                    }

                    return arr_out;


                }
            }

        }

        public static class Int32Array
        {










            
            
            
            /// <summary>
            /// Written. 2023.11.08 15:39. Warsaw. Workplace. 
            /// </summary>
            /// Note. 2023.11.08 16:12. Warsaw. Workplace. 
            /// Columns and Rows words to be used and all combinations can be 
            /// descibed using it.
            /// Letter description did not work good when
            /// Int32[][] into 1 array by columns and by rows.
            public static class Merge
            {

                /// <summary>
                /// Merge two arrays Int32[] into one array Int32[]. <br></br>
                /// Written. 2024.01.21 16:40. Warsaw. Hostel. 
                /// </summary>
                /// <param name="arr_1"></param>
                /// <param name="arr_2"></param>
                /// <returns></returns>
                public static Int32[] A_B_To_C(Int32[] arr_1, Int32[] arr_2)
                {
                    Int32[] arr_out = new Int32[arr_1.Length + arr_2.Length];
                    Array.Copy(arr_1, arr_out, arr_1.Length);
                    Array.Copy(arr_2, 0, arr_out, arr_1.Length, arr_2.Length);
                    return arr_out;
                }

                /// <summary>
                /// Takes each Int32[m] from Int32[n][m] and makes one array Int32[nxm] <br></br>
                /// <br></br>
                /// Input: Int32[a][b] <br></br>
                /// Output: Int32[c], c = axb <br></br>
                /// </summary>
                /// <param name="array_in"></param>
                /// <returns></returns>
                public static Int32[] AxB_To_C(Int32[][] array_in)
                {
                    return ArrayFunctions.Merge.NxM_To_A(array_in);
                }
            }
            /// <summary>
            /// Written. 2023.11.05 16:41. Gdansk. Home. 
            /// not tested. 2023.11.05 16:42. Gdansk. Home. 
            /// </summary>
            public static class Normilize
            {
                /// <summary>
                /// Written. 2023.11.05 16:57. Gdansk. Home. 
                /// Tested. Works. 2023.11.05 17:44. Gdansk. Home. 
                /// </summary>
                /// <param name="arr_in"></param>
                /// <param name="number_for_max_in_arr"></param>
                /// <returns></returns>
                public static Int32[] ScaleMaximumToNumber(Int32[] arr_in, UInt32 number_for_max_in_arr)
                {
                    // check if input is correct
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new Int32[0];
                    }
                    if (number_for_max_in_arr == 0)
                    {
                        ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Length_is_0);
                    }
                    // calculating normilize number.                   
                    float normlize_coeficient = (float)arr_in.Max() / (float)number_for_max_in_arr;
                    // making array for return
                    Int32[] arr_out = new Int32[arr_in.Length];
                    // devide each number in array
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        arr_out[i] = (int)((float)arr_in[i] / (float)normlize_coeficient);
                    }
                    // return array
                    return arr_out;
                }
            }
            /// <summary>
            /// Checks if Int32 array 1 contains Int32 array 2.
            /// 2023.08.23 09:57. written.
            /// 2023.08.23 09:57. tested. works.
            /// /// </summary>
            /// <param name="arr_search_in"></param>
            /// <param name="arr_search"></param>
            /// <returns></returns>
            public static bool Contains(Int32[] arr_search_in, Int32[] arr_search)
            {
                return ArrayFunctions.Contains(arr_search_in, arr_search);
            }
            public static class Split
            {
                /// <summary>
                /// Split Int32[] to Int32[n][], where n - required number of columns
                /// 2023.08.23 10:00. written.
                /// 2023.08.23 10:00. tested. works.
                /// </summary>
                /// <param name="array_in"></param>
                /// <param name="columns_number"></param>
                /// <param name="columns_length_equal"></param>
                /// <returns></returns>
                public static Int32[][] A_To_MxN(Int32[] array_in, Int32 columns_number, bool columns_length_equal = true)
                {
                    return ArrayFunctions.Split.A_To_MxN(array_in, columns_number, columns_length_equal);
                }
            }

            /// <summary>
            /// Written. 2024.02.09 15:45. Warsaw. Workplace. 
            /// </summary>
            public static class Generate
            {

                /// <summary>
                /// Written. 2024.02.09 15:51. Warsaw. Workplace. 
                /// Tested. Works. 2024.02.09 15:54. Warsaw. Workplace. 
                /// </summary>
                /// <param name="numbers_num"></param>
                /// <param name="element_value"></param>
                /// <returns></returns>
                public static Int32[] WithTheSameValue(Int32 numbers_num, Int32 element_value)
                {
                   // note. word - identical. 2024.02.09 15:51. Warsaw. Workplace. 
                    Int32[] arr_out = new Int32[numbers_num];
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        arr_out[i] = element_value;
                    }
                    return arr_out;
                }


                /// <summary>
                /// Generate Int32[] filled with random numbers
                /// 2023.07.10 - 2023.07.20. 10 - 15 o'clock. written.
                /// 2023.08.22 16:10. tested. works.
                /// </summary>
                /// <param name="numbers_num"></param>
                /// <param name="min"></param>
                /// <param name="max"></param>
                /// <returns></returns>
                public static Int32[] RandomMinMaxValue(Int32 numbers_num, Int32 min, Int32 max)
                {
                    Int32[] arr_out = new Int32[numbers_num];
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        arr_out[i] = _internal_random.Next(min, max + 1);
                    }
                    return arr_out;
                }
                /// <summary>
                /// Generate Int32[] filled with numbers from 0 to array length - 1 <br></br>
                /// 2023.08.22 16:17. written. <br></br>
                /// 2023.08.22 16:18. tested. works. <br></br>
                /// </summary>
                /// <param name="numbers_num"></param>
                /// <returns>Array with element from 0 to length - 1. <br></br>
                /// Can return array  with length is 0</returns>
                public static Int32[] WithValueIncreasesBy1(Int32 numbers_num)
                {
                    Int32[] arr_out = new Int32[numbers_num];
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        arr_out[i] = i;
                    }
                    return arr_out;
                }
                /// <summary>
                /// Generate Int32[][] filled with random numbers
                /// 2023.07.10 - 2023.07.20. 10 - 15 o'clock. written.
                /// 2023.22.08 15:10. tested. works.
                /// </summary>
                /// <param name="numbers_num_x"></param>
                /// <param name="numbers_num_y"></param>
                /// <param name="min"></param>
                /// <param name="max"></param>
                /// <returns></returns>
                public static Int32[][] RandomMinMaxValue(Int32 numbers_num_x, Int32 numbers_num_y, Int32 min, Int32 max)
                {
                    Int32[][] arr_out = new Int32[numbers_num_x][];
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        arr_out[i] = new Int32[numbers_num_y];
                        for (Int32 j = 0; j < arr_out[i].Length; j++)
                        {
                            arr_out[i][j] = _internal_random.Next(min, max + 1);
                        }
                    }
                    return arr_out;
                }
                /// <summary>
                ///  Generate Int32[][] filled with numbers starting from 0 to (Rows x Cols - 1)<br></br>
                ///  2023.10.30 13:49. Written.
                ///  2023.10.30 13:50.
                /// </summary>
                /// <param name="numbers_num_x"></param>
                /// <param name="numbers_num_y"></param>
                /// <returns></returns>
                public static Int32[][] WithValueIncreasesBy1(Int32 numbers_num_x, Int32 numbers_num_y)
                {
                    Int32[][] arr_out = new Int32[numbers_num_x][];
                    Int32 num_for_arr = 0;
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        arr_out[i] = new Int32[numbers_num_y];
                        for (Int32 j = 0; j < arr_out[i].Length; j++)
                        {
                            arr_out[i][j] = num_for_arr;
                            num_for_arr++;
                        }
                    }
                    return arr_out;
                }
            }
            /// <summary>
            /// Accepts: char[]
            /// 2023-07-26 13:15
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static Int32[] ConvertToInt32(char[] arr_in)
            {
                Int32[] arr_out = new Int32[arr_in.Length];
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    arr_out[i] = (int)arr_in[i];
                }
                return arr_out;
            }
            /// <summary>
            /// string is char[] and each char is 2 bytes
            /// 2023-08-01 15:27
            /// </summary>
            /// <param name="str_in"></param>
            /// <returns></returns>
            public static Int16[] ConvertToInt16Array(string str_in)
            {
                Int16[] arr_out = Array.ConvertAll(str_in.ToArray(), CharToInt16);
                Int16 CharToInt16(char char_in)
                {
                    return (Int16)char_in;
                }
                return arr_out;
            }
            public static void ToConsole(Int32[] arr_1, Int32[] arr_2)
            {
                Type type = typeof(int);
                Console.WriteLine("Array 1" + type.Name.ToString() + ". Length is " + arr_1.Length.ToString());
                Console.WriteLine("Array 2" + type.Name.ToString() + ". Length is " + arr_2.Length.ToString());
                Console.WriteLine();
                string str_size_min = "";
                str_size_min = arr_2.Min().ToString();
                if (arr_1.Min() < arr_2.Min())
                {
                    str_size_min = arr_1.Min().ToString();
                }
                string str_size_max = "";
                str_size_max = arr_2.Max().ToString();
                if (arr_1.Max() > arr_2.Max())
                {
                    str_size_min = arr_1.Max().ToString();
                }
                string pad_str = "".PadRight(str_size_max.Length, ' '); ;
                if (str_size_min.Length > str_size_max.Length)
                {
                    pad_str = "".PadRight(str_size_min.Length, ' ');
                }
                for (Int32 i = 0; i < arr_1.Length; i++)
                {
                    Console.WriteLine(arr_1[i].ToString().PadRight(pad_str.Length, ' ') +
                        pad_str + arr_2[i].ToString().PadRight(pad_str.Length, ' '));
                }
                Console.WriteLine();
            }
            /// <summary>
            /// Prints Int32[][] to console <br></br>
            /// 2023.08.12 09:31. written. <br></br>
            /// 2023.08.22 15:16. tested. works. <br></br>
            /// Modified. Added parameter - spaces. Tested. Works.  2024.02.09 10:33. Warsaw. Workplace. 
            /// </summary>
            /// <param name="arr_in"></param>
            public static void ToConsole(Int32[][] arr_in, UInt32 spaces_between_numbers = 3)
            {
                if (arr_in.Length == 0)
                {
                    Console.WriteLine("Attention! Array is empty");
                    return;
                }
                Console.WriteLine("Array" + typeof(int).Name.ToString() + ". Length is " + arr_in.Length.ToString() + "x" + arr_in[0].Length.ToString());
                Int32[] array_all_values = ArrayFunctions.Merge.NxM_To_A(arr_in);
                string min_num = array_all_values.Min().ToString();
                string max_num = array_all_values.Max().ToString();
                Int32 pad_size = max_num.Length;
                if (max_num.Length < min_num.Length)
                {
                    pad_size = min_num.Length;
                }
                string[][] str_arr = TwoDimensions.Convert.ToStringArray.ToDecimal(arr_in);
                str_arr = StringArray.Pad.Right(str_arr, pad_size);
                string str_for_console = StringArray.Convert.ToFileString(str_arr, "".PadRight((int)spaces_between_numbers, ' '));
                // 2024.01.21 16:52. Warsaw. Hostel. 
                // There were Writeline. FileString adds "\r\n" at the last line therefore
                // Console.Writeline is not needed.
                Console.Write(str_for_console);
            }
            /// <summary>
            /// Accepts: object[]
            /// 2023-08-01 15:21
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static Int32[] ConvertToInt32(object[] arr_in)
            {
                Int32[] arr_out = new Int32[arr_in.Length];
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    try
                    {
                        arr_out[i] = (int)arr_in[i];
                    }
                    catch
                    {
                        ReportFunctions.ReportError("Cast failed at " + i.ToString());
                    }
                }
                return arr_out;
            }
            /// <summary>
            /// Tested. Works. 2024.02.14 13:22. Warsaw. Workplace. 
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static Int32 Average(Int32[] arr_in)
            {
                // 2024.02.14 13:22. Warsaw. Workplace. 
                // need long[] because there were overflow of Int32 because of a lot of large numbers.
                long[] arr_long = new long[arr_in.Length];
                Array.Copy(arr_in,arr_long, arr_long.Length);
                return (int)(arr_long.Sum() / (long)arr_in.Length);
            }
            /// <summary>
            /// Sort array according to provided indexes
            /// 2023.08.12 12:12
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="indexes_in"></param>
            /// <returns></returns>
            public static Int32[] Sort(Int32[] arr_in, Int32[] indexes_in)
            {
                return ArrayFunctions.Sort(arr_in, indexes_in);
            }
            /// <summary>
            /// Sort AxB array according to indexes provided
            /// 2023.08.12 12:13
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="arr_in"></param>
            /// <param name="indexes_in"></param>
            /// <returns></returns>
            public static Int32[][] Sort(Int32[][] arr_in, Int32[] indexes_in)
            {
                return ArrayFunctions.Sort(arr_in, indexes_in);
            }
            public static Int32[] Sort(Int32[] arr_in, ArraySortingEnum order_to_sort)
            {
                Int32[] arr_out = arr_in;
                Int32 sort_condition(Int32 num1, Int32 num2)
                {
                    Int32 res_out = 0;
                    if (num1 > num2)
                    {
                        return 1;
                    }
                    if (num1 == num2)
                    {
                        return 0;
                    }
                    if (num1 < num2)
                    {
                        return -1;
                    }
                    return res_out;
                }
                Array.Sort(arr_out, sort_condition);
                if (order_to_sort == ArraySortingEnum.Descending)
                {
                    Array.Reverse(arr_out);
                }
                return arr_out;
            }
            /// <summary>
            /// Provides change in indexes after sorting
            /// 2023.08.12 11:07
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="order_to_sort"></param>
            /// <returns></returns>
            public static Int32[] SortingIndexes(Int32[] arr_in, ArraySortingEnum order_to_sort)
            {
                Int32[] arr_to_sort = new Int32[arr_in.Length];
                Int32[] indexes = new Int32[arr_in.Length];
                Array.Copy(arr_in, arr_to_sort, arr_in.Length);
                for (Int32 i = 0; i < indexes.Length; i++)
                {
                    indexes[i] = i;
                }
                for (Int32 i = 0; i < arr_to_sort.Length; i++)
                {
                    Int32 max_number = arr_to_sort[i];
                    Int32 max_number_index = i;
                    for (Int32 j = i; j < arr_to_sort.Length; j++)
                    {
                        if (arr_to_sort[j] >= max_number)
                        {
                            max_number = arr_to_sort[j];
                            max_number_index = j;
                        }
                    }
                    arr_to_sort[max_number_index] = arr_to_sort[i];
                    arr_to_sort[i] = max_number;
                    Int32 index_tmp = indexes[i];
                    indexes[i] = indexes[max_number_index];
                    indexes[max_number_index] = index_tmp;
                }
                if (order_to_sort == ArraySortingEnum.Ascenidng)
                {
                    Array.Reverse(indexes);
                }
                return indexes;
            }
            /// <summary>
            /// Sort Int32 array and provides change in indexes
            /// 2023.08.12 11:04
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="sorting_indexes"></param>
            /// <param name="order_to_sort"></param>
            /// <returns></returns>
            public static Int32[] Sort(Int32[] arr_in, out Int32[] sorting_indexes, ArraySortingEnum order_to_sort)
            {
                Int32[] arr_out = arr_in;
                Int32[] indexes = new Int32[arr_in.Length];
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    indexes[i] = i;
                }
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    Int32 max_number = arr_in[i];
                    Int32 max_number_index = i;
                    for (Int32 j = i; j < arr_out.Length; j++)
                    {
                        if (arr_in[j] >= max_number)
                        {
                            max_number = arr_in[j];
                            max_number_index = j;
                        }
                    }
                    arr_out[max_number_index] = arr_out[i];
                    arr_out[i] = max_number;
                    Int32 index_tmp = indexes[i];
                    indexes[i] = indexes[max_number_index];
                    indexes[max_number_index] = index_tmp;
                }
                if (order_to_sort == ArraySortingEnum.Ascenidng)
                {
                    Array.Reverse(arr_out);
                    Array.Reverse(indexes);
                }
                sorting_indexes = indexes;
                return arr_out;
            }
            /// <summary>
            /// Prints Int32 Array to console
            /// 2023.08.12 08:18
            /// Modified. Added with index. 2024.03.08 15:15. Warsaw. Hostel.
            /// </summary>
            /// <param name="arr_in"></param>
            public static void ToConsole(Int32[] arr_in, bool with_index = false)
            {
                if (with_index == false)
                {
                    Type type = typeof(int);
                    Console.WriteLine("Array " + type.Name.ToString() + ". Length is " + arr_in.Length.ToString());
                    string[] strings_arr = Convert.ToStringArray(arr_in);
                    Console.Write(StringArray.Convert.ToFileString(strings_arr));
                    return;
                }

                if (with_index == true)
                {
                    Type type = typeof(int);
                    Console.WriteLine("Array " + type.Name.ToString() + ". Length is " + arr_in.Length.ToString());
                    string[] strings_arr = Convert.ToStringArray(arr_in);
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        Console.Write(i.ToString() + ".\t");
                        Console.WriteLine(strings_arr[i]);
                    }
                    return;
                }





            }


            /// <summary>
            /// Written. 2024.03.08 15:03. Warsaw. Hostel.
            /// </summary>
            public static class Find
            {

                /// <summary>
                /// Written. 2024.03.08 15:30. Warsaw. Hostel.
                /// </summary>
                public static class ClosestElement
                {

                    /// <summary>
                    /// Find the closest element to provided value in Int32[] and return the value of this element<br></br>
                    /// Written. 2024.03.08 15:33. Warsaw. Hostel. <br></br>
                    /// Tested. Works. 2024.03.08 15:35. Warsaw. Hostel.
                    /// </summary>
                    /// <param name="arr_in"></param>
                    /// <param name="value_in"></param>
                    /// <returns></returns>
                    public static Int32 Value(Int32[] arr_in, Int32 value_in)
                    {
                        Int32 index_found = ClosestElement.Index(arr_in, value_in);
                        Int32 int_return = arr_in[index_found];
                        return int_return;
                    }


                    /// <summary>
                    /// Find the closest element to provided value in Int32[] and return the index of this element<br></br> 
                    /// Written. 2024.03.08 15:11. Warsaw. Hostel. <br></br>
                    /// Tested. Works. 2024.03.08 15:27. Warsaw. Hostel.
                    /// </summary>
                    /// <param name="arr_in"></param>
                    /// <param name="value_in"></param>
                    /// <returns></returns>
                    public static Int32 Index(Int32[] arr_in, Int32 value_in)
                    {
                        if (arr_in.Length == 0)
                        {
                            ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                            return 0;
                        }


                        Int32 substract_value = arr_in[0] - value_in;
                        Int32 index_found = 0;
                        // postive decreases. that value
                        // negative becomes bigger

                        for (Int32 i = 0; i < arr_in.Length; i++)
                        {
                            if (System.Math.Abs(substract_value) > System.Math.Abs(arr_in[i] - value_in))
                            {
                                substract_value = arr_in[i] - value_in;
                                index_found = i;
                            }
                        }
                        return index_found;



                    }


                }

            }
                public static class Math
            {

                /// <summary>
                /// Written. 2024.02.09 15:43. Warsaw. Workplace. 
                /// Tested. Works. 2024.02.09 15:57. Warsaw. Workplace. 
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static bool ElementsTheSame(Int32[] arr_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return false;
                    }
                    bool result_out = true;
                    Int32 arr_element_1st = arr_in[0];
                    foreach (Int32 arr_element in arr_in)
                    {
                        if (arr_element != arr_element_1st)
                        {
                            result_out = false;
                            break;
                        }
                    }

                    return result_out;

                }

                    /// <summary>
                    /// Counts how many times each number is used in array. Returns Int32[][]. <br></br>
                    /// 1 index is numbers, 2 index is count <br></br>
                    /// Written. 2024.01.20 22:55. Warsaw. Hostel.
                    /// Tested. Works. 2024.01.21 16:52. Warsaw. Hostel. 
                    /// </summary>
                    public static Int32[][] Distribution(Int32[] arr_in)
                {
                    Int32[] numbers_of_array = NumbersUsedInArray(arr_in);
                    Int32[] numbers_count = new Int32[numbers_of_array.Length];

                    for (Int32 i = 0; i < numbers_of_array.Length; i++)
                    {

                        for (Int32 j = 0; j < arr_in.Length; j++)
                        {
                            if (arr_in[j] == numbers_of_array[i])
                            {
                                numbers_count[i] += 1;
                            }
                        }

                    }

                    Int32[][] arr_out = new Int32[2][];
                    arr_out[0] = numbers_of_array;
                    arr_out[1] = numbers_count;
                    return arr_out;

                }







                /// <summary>
                /// Return numbers that are used in the array without repetetion of the same number <br></br>
                /// Written. 2024.01.20 22:21. Warsaw. Hostel. <br></br>
                /// Tested. Works. 2024.01.21 16:48. Warsaw. Hostel. 
                /// </summary>
                public static Int32[] NumbersUsedInArray(Int32[] arr_in)
                {
                    List<int> arr_out_list = new List<int>();
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        if (arr_out_list.Contains(arr_in[i]) == false)
                        {
                            arr_out_list.Add(arr_in[i]);
                        }
                    }

                    return arr_out_list.ToArray();

                }








                /// <summary>
                /// 2023.10.30 14:19. Written. Warsaw. Workplace. <br></br>
                /// 2023.10.30 14:22. Tested. Works
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static Int32[][] InvertSign(Int32[][] arr_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return arr_in;
                    }
                    Int32[][] arr_out = new Int32[arr_in.Length][];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = InvertSign(arr_in[i]);
                    }
                    return arr_out;
                }
                /// <summary>
                /// 2023.10.30 13:01. Written. Warsaw. Workplace. <br></br>
                /// 2023.10.30 13:01. Tested. Works.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static Int32[] InvertSign(Int32[] arr_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return arr_in;
                    }
                    Int32[] arr_out = new Int32[arr_in.Length];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = 0;
                        arr_out[i] = -arr_in[i];
                    }
                    return arr_out;
                }
                /// <summary>
                /// Adds Int32 Array to Int32 Array and returns Int32 Array.<br></br>
                /// 2023.08.01 - 2023.09.01. Written. Warsaw. <br></br>
                /// 2023.10.30 12:56. Tested. Works.
                /// </summary>
                /// <param name="arr_1"></param>
                /// <param name="arr_2"></param>
                /// <returns></returns>
                public static Int32[] Add(Int32[] arr_1, Int32[] arr_2)
                {
                    if (arr_1.Length != arr_2.Length)
                    {
                        ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                        return arr_1;
                    }
                    Int32[] arr_out = new Int32[arr_1.Length];
                    for (Int32 i = 0; i < arr_1.Length; i++)
                    {
                        arr_out[i] = arr_1[i] + arr_2[i];
                    }
                    return arr_out;
                }
                /// <summary>
                /// Adds number to Int32 Array and returns Int32 Array.<br></br>
                /// 2023.08.01 - 2023.09.01. Written. Warsaw. <br></br>
                /// 2023.10.30 12:47. Tested. Works.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <param name="number_in"></param>
                /// <returns></returns>
                public static Int32[] Add(Int32[] arr_in, Int32 number_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return arr_in;
                    }
                    Int32[] arr_out = new Int32[arr_in.Length];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = arr_in[i] + number_in;
                    }
                    return arr_out;
                }
                /// <summary>
                /// Adds Int32[][] Array to Int32[][] Array and returns Int32[][] array<br></br>
                /// 2023.08.01 - 2023.09.01. Written. Warsaw. <br></br>
                /// 2023.10.30 14:44. Tested. Works.
                /// </summary>
                /// <param name="arr_1"></param>
                /// <param name="arr_2"></param>
                /// <returns></returns>
                public static Int32[][] Add(Int32[][] arr_1, Int32[][] arr_2)
                {
                    if (arr_1.Length != arr_2.Length)
                    {
                        ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                        return arr_1;
                    }
                    for (Int32 i = 0; i < arr_1.Length; i++)
                    {
                        if (arr_1[i].Length != arr_2[i].Length)
                        {
                            ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                            return arr_1;
                        }
                    }
                    Int32[][] arr_out = new Int32[arr_1.Length][];
                    for (Int32 i = 0; i < arr_1.Length; i++)
                    {
                        arr_out[i] = Add(arr_1[i], arr_2[i]);
                    }
                    return arr_out;
                }
                /// <summary>
                /// Adds number to Int32[][] and returns Int32[][] array<br></br>
                /// 2023.08.01 - 2023.09.01. Written. Warsaw. <br></br>
                /// 2023.10.30 14:42. Tested. Works.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <param name="number_in"></param>
                /// <returns></returns>
                public static Int32[][] Add(Int32[][] arr_in, Int32 number_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return arr_in;
                    }
                    Int32[][] arr_out = new Int32[arr_in.Length][];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = Add(arr_in[i], number_in);
                    }
                    return arr_out;
                }
                public static Int32[] Multiply(Int32[] arr_1, Int32[] arr_2)
                {
                    if (arr_1.Length != arr_2.Length)
                    {
                        ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                        return arr_1;
                    }
                    Int32[] arr_out = new Int32[arr_1.Length];
                    for (Int32 i = 0; i < arr_1.Length; i++)
                    {
                        arr_out[i] = arr_1[i] * arr_2[i];
                    }
                    return arr_out;
                }
                public static Int32[] Multiply(Int32[] arr_in, Int32 number_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return arr_in;
                    }
                    Int32[] arr_out = new Int32[arr_in.Length];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = arr_in[i] * number_in;
                    }
                    return arr_out;
                }

                /// <summary>
                /// Written. 2024.04.19 15:57. Warsaw. Workplace.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <param name="number_in"></param>
                /// <returns></returns>
                public static Int32[] Multiply(Int32[] arr_in, float number_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return arr_in;
                    }
                    Int32[] arr_out = new Int32[arr_in.Length];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = (int)((float)arr_in[i] * number_in);
                    }
                    return arr_out;
                }



                public static Int32[][] Multiply(Int32[][] arr_1, Int32[][] arr_2)
                {
                    if (arr_1.Length != arr_2.Length)
                    {
                        ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                        return arr_1;
                    }
                    for (Int32 i = 0; i < arr_1.Length; i++)
                    {
                        if (arr_1[i].Length != arr_2[i].Length)
                        {
                            ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                            return arr_1;
                        }
                    }
                    Int32[][] arr_out = new Int32[arr_1.Length][];
                    for (Int32 i = 0; i < arr_1.Length; i++)
                    {
                        arr_out[i] = Multiply(arr_1[i], arr_2[i]);
                    }
                    return arr_out;
                }
                public static Int32[][] Multiply(Int32[][] arr_in, Int32 number_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return arr_in;
                    }
                    Int32[][] arr_out = new Int32[arr_in.Length][];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = Multiply(arr_in[i], number_in);
                    }
                    return arr_out;
                }
                public static Int32[] Devide(Int32[] arr_1, Int32[] arr_2)
                {
                    if (arr_1.Length != arr_2.Length)
                    {
                        ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                        return arr_1;
                    }
                    Int32[] arr_out = new Int32[arr_1.Length];
                    for (Int32 i = 0; i < arr_1.Length; i++)
                    {
                        try
                        {
                            arr_out[i] = arr_1[i] / arr_2[i];
                        }
                        catch
                        {
                            ReportFunctions.ReportError(i);
                            return arr_1;
                        }
                    }
                    return arr_out;
                }
                public static Int32[] Devide(Int32[] arr_in, Int32 number_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return arr_in;
                    }
                    if (number_in == 0)
                    {
                        ReportFunctions.ReportError("Devide by 0");
                        return arr_in;
                    }
                    Int32[] arr_out = new Int32[arr_in.Length];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = arr_in[i] / number_in;
                    }
                    return arr_out;
                }
                public static Int32[][] Devide(Int32[][] arr_1, Int32[][] arr_2)
                {
                    if (arr_1.Length != arr_2.Length)
                    {
                        ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                        return arr_1;
                    }
                    for (Int32 i = 0; i < arr_1.Length; i++)
                    {
                        if (arr_1[i].Length != arr_2[i].Length)
                        {
                            ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                            return arr_1;
                        }
                    }
                    Int32[][] arr_out = new Int32[arr_1.Length][];
                    for (Int32 i = 0; i < arr_1.Length; i++)
                    {
                        try
                        {
                            arr_out[i] = Devide(arr_1[i], arr_2[i]);
                        }
                        catch
                        {
                            ReportFunctions.ReportError(i);
                            return arr_1;
                        }
                    }
                    return arr_out;
                }
                public static Int32[][] Devide(Int32[][] arr_in, Int32 number_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return arr_in;
                    }
                    if (number_in == 0)
                    {
                        ReportFunctions.ReportError("Devide by 0");
                        return arr_in;
                    }
                    Int32[][] arr_out = new Int32[arr_in.Length][];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = Devide(arr_in[i], number_in);
                    }
                    return arr_out;
                }
                /// <summary>
                /// Substracts Int32 Array to Int32 Array and returns Int32 Array.<br></br>
                /// 2023.08.01 - 2023.09.01. Written. Warsaw. <br></br>
                /// 2023.10.30 16:00. Tested. Works.
                /// </summary>
                /// <param name="arr_1"></param>
                /// <param name="arr_2"></param>
                /// <returns></returns>
                public static Int32[] Substract(Int32[] arr_1, Int32[] arr_2)
                {
                    if (arr_1.Length != arr_2.Length)
                    {
                        ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                        return arr_1;
                    }
                    Int32[] arr_out = new Int32[arr_1.Length];
                    for (Int32 i = 0; i < arr_1.Length; i++)
                    {
                        arr_out[i] = arr_1[i] - arr_2[i];
                    }
                    return arr_out;
                }
                /// <summary>
                /// Substracts number to Int32 Array and returns Int32 Array.<br></br>
                /// 2023.08.01 - 2023.09.01. Written. Warsaw. <br></br>
                /// 2023.10.30 16:01. Tested. Works.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <param name="number_in"></param>
                /// <returns></returns>
                public static Int32[] Substract(Int32[] arr_in, Int32 number_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return arr_in;
                    }
                    Int32[] arr_out = new Int32[arr_in.Length];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = arr_in[i] - number_in;
                    }
                    return arr_out;
                }
                /// <summary>
                /// Substracts Int32[][] Array to Int32[][] Array and returns Int32[][] array<br></br>
                /// 2023.08.01 - 2023.09.01. Written. Warsaw. <br></br>
                /// 2023.10.30 16:04. Tested. Works.
                /// </summary>
                /// <param name="arr_1"></param>
                /// <param name="arr_2"></param>
                /// <returns></returns>
                public static Int32[][] Substract(Int32[][] arr_1, Int32[][] arr_2)
                {
                    if (arr_1.Length != arr_2.Length)
                    {
                        ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                        return arr_1;
                    }
                    for (Int32 i = 0; i < arr_1.Length; i++)
                    {
                        if (arr_1[i].Length != arr_2[i].Length)
                        {
                            ReportFunctions.ReportError(arr_1.Length, arr_2.Length, ReportFunctions.ErrorMessage.LengthDifferent);
                            return arr_1;
                        }
                    }
                    Int32[][] arr_out = new Int32[arr_1.Length][];
                    for (Int32 i = 0; i < arr_1.Length; i++)
                    {
                        arr_out[i] = Substract(arr_1[i], arr_2[i]);
                    }
                    return arr_out;
                }
                /// <summary>
                /// Substracts number to Int32[][] and returns Int32[][] array<br></br>
                /// 2023.08.01 - 2023.09.01. Written. Warsaw. <br></br>
                /// 2023.10.30 16:02. Tested. Worked.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <param name="number_in"></param>
                /// <returns></returns>
                public static Int32[][] Substract(Int32[][] arr_in, Int32 number_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return arr_in;
                    }
                    Int32[][] arr_out = new Int32[arr_in.Length][];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = Substract(arr_in[i], number_in);
                    }
                    return arr_out;
                }
                static object[] SubstractObjectArrays(object[] arr_1, object[] arr_2, Type type_in)
                {
                    object[] arr_out = new object[arr_1.Length];
                    for (Int32 i = 0; i < arr_1.Length; i++)
                    {
                        if (type_in == typeof(int))
                        {
                            arr_out[i] = (int)arr_1[i] - (int)arr_2[i];
                        }
                        if (type_in == typeof(short))
                        {
                            arr_out[i] = (short)arr_1[i] - (short)arr_2[i];
                        }
                        if (type_in == typeof(byte))
                        {
                            arr_out[i] = (byte)arr_1[i] - (byte)arr_2[i];
                        }
                        if (type_in == typeof(float))
                        {
                            arr_out[i] = (float)arr_1[i] - (float)arr_2[i];
                        }
                    }
                    return arr_out;
                }
            }
            public static class Convert
            {

                /// <summary>
                /// Converts array to filestring according to required number format, delimer and missing number string (defined by char). <br></br> 
                /// Written. 2024.03.21 11:15. Warsaw. Workplace. <br></br>
                /// Tested. Works. 2024.03.21 11:27. Warsaw. Workplace. <br></br> 
                /// </summary>
                /// <param name="arr_in"></param>
                /// <param name="num_per_row"></param>
                /// <param name="delimer"></param>
                /// <param name="missing_number_char">Defines missing number string. The string will be of the length of the longest number with this char</param>
                /// <param name="base_in"></param>
                /// <param name="pad_number"></param>
                /// <returns></returns>
                public static string ToFileString(Int32[] arr_in, int num_per_row, int base_in = 10, string delimer = "\t", char missing_number_char = '.', int pad_number = -1)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return "";
                    }

                    int rows_num = arr_in.Length/num_per_row;
                    bool IsNotRectangle = false;
                    if ((arr_in.Length % num_per_row) != 0)
                    {
                        rows_num += 1;
                        IsNotRectangle = true;
                    }

                    string[] strings_filestring = new string[rows_num];

                    StringBuilder write_string = new StringBuilder();
                   
                    int col_index = 0;

                    int pad_length = pad_number;

                    if (base_in == 10)
                    {
                        if (pad_number == -1)
                        {
                            int max_int = arr_in.Max();
                            int min_int = arr_in.Min();
                            pad_length = min_int.ToString().Length;
                            if (max_int.ToString().Length > min_int.ToString().Length)
                            {
                                pad_length = max_int.ToString().Length;
                            }
                        }
                    }

                    if (base_in == 16)
                    {
                        pad_length = 8;
                    }

                    for (int i = 0; i < arr_in.Length; i++)
                    {
                        if (base_in == 10)
                        {
                            write_string.Append(System.Convert.ToString(arr_in[i], base_in).ToUpper().PadRight(pad_length, ' '));
                        }
                        if (base_in == 16)
                        {
                            write_string.Append(System.Convert.ToString(arr_in[i], base_in).ToUpper().PadLeft(pad_length, '0'));
                        }
                        write_string.Append(delimer);
                        col_index += 1;
                        if (col_index > (num_per_row - 1))
                        {
                            write_string.Append("\r\n");
                            col_index = 0;                            
                        }
                    }

                    if (IsNotRectangle == true)
                    {
                        int row_filled = arr_in.Length % num_per_row;
                        for (int i = 0; i < (num_per_row - row_filled); i++)
                        {
                            write_string.Append("".PadRight(pad_length, missing_number_char));
                            write_string.Append(delimer);
                            write_string.Append("\r\n");
                        }
                    }
                    
                    string return_string = write_string.ToString();
                    
                    return return_string;

                }


                    /// <summary>
                    /// Written. 2023.11.05 13:26. Gdansk. Home. <br></br>
                    /// Tested. Works. 2023.11.05 13:33. Gdansk. Home. 
                    /// </summary>
                    /// <param name="arr_in"></param>
                    /// <returns></returns>
                    public static char[] ToCharArray(Int32[] arr_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new char[0];
                    }
                    char[] arr_out = Array.ConvertAll(arr_in, Int32ToChar);
                    char Int32ToChar(Int32 number)
                    {
                        return System.Convert.ToChar(number);
                    }
                    return arr_out;
                }
                /// <summary>
                /// 2023.08.29 10:39. written. <br></br>
                /// 2023.08.29 10:39. tested. works. <br></br>
                /// what if the number > 255 ? 2023.12.14 12:36. Workplace
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static byte[] ToByteArray(Int32[] arr_in)
                {
                    byte[] arr_out = Array.ConvertAll(arr_in, Int32ToByte);
                    byte Int32ToByte(Int32 number)
                    {
                        return System.Convert.ToByte(number);
                    }
                    return arr_out;
                }
                /// <summary>
                /// 2023.08.29 10:39. written.
                /// 2023.08.29 10:39. not checked.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static byte[][] ConvertToByteArray(Int32[][] arr_in)
                {
                    byte[][] arr_out = new byte[arr_in.Length][];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = ToByteArray(arr_in[i]);
                    }
                    return arr_out;
                }
                /// <summary>
                /// not tested
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static string[] ToStringArray(Int32[] arr_in)
                {
                    // space for code. start.
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new string[0];
                    }
                    string[] arr_out = new string[arr_in.Length];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        try
                        {
                            arr_out[i] = System.Convert.ToString(arr_in[i]);
                        }
                        catch
                        {
                            ReportFunctions.ReportError(i);
                        }
                    }
                    // space for code. end.
                    return arr_out;
                }
               
            }


            /// <summary>
            /// Written. 2024.05.20 17:21. Warsaw. Workplace 
            /// </summary>

            public static class TwoDimensions
            {

				/// <summary>
				/// Written. 2024.05.20 17:21. Warsaw. Workplace.
				/// </summary>
				public static class Generate
				{
					/// <summary>
					/// Generate Int32[][] filled with random numbers from min to max number. <br></br>
					/// Written. 2023.07.10 - 2023.07.20. 10 - 15 o'clock. <br></br>
					/// Tested. Works. 2023.22.08 15:10. <br></br>
					/// </summary>
					/// <param name="numbers_num_x"></param>
					/// <param name="numbers_num_y"></param>
					/// <param name="min"></param>
					/// <param name="max"></param>
					/// <returns></returns>
					public static Int32[][] RandomMinMaxValue(Int32 numbers_num_x, Int32 numbers_num_y, Int32 min, Int32 max)
					{
                        // Moved to TwoDimensions. 2024.05.20 17:24.Warsaw.Workplace.						
                        Int32[][] arr_out = new Int32[numbers_num_x][];
						for (Int32 i = 0; i < arr_out.Length; i++)
						{
							arr_out[i] = new Int32[numbers_num_y];
							for (Int32 j = 0; j < arr_out[i].Length; j++)
							{
								arr_out[i][j] = _internal_random.Next(min, max + 1);
							}
						}
						return arr_out;
					}
					/// <summary>
					///  Generate Int32[][] filled with numbers starting from 0 to (Rows x Cols - 1)<br></br>
					///  2023.10.30 13:49. Written.
					///  2023.10.30 13:50.
					/// </summary>
					/// <param name="numbers_num_x"></param>
					/// <param name="numbers_num_y"></param>
					/// <returns></returns>
					public static Int32[][] WithValueIncreasesBy1(Int32 numbers_num_x, Int32 numbers_num_y)
					{
						Int32[][] arr_out = new Int32[numbers_num_x][];
						Int32 num_for_arr = 0;
						for (Int32 i = 0; i < arr_out.Length; i++)
						{
							arr_out[i] = new Int32[numbers_num_y];
							for (Int32 j = 0; j < arr_out[i].Length; j++)
							{
								arr_out[i][j] = num_for_arr;
								num_for_arr++;
							}
						}
						return arr_out;
					}
				}



				/// <summary>
				/// Written. 2024.05.20 17:21. Warsaw. Workplace 
				/// </summary>
				public static class Convert
				{

					
				
				
					/// <summary>
					/// 2023.08.29 10:39. written.
					/// 2023.08.29 10:39. not checked.
					/// </summary>
					/// <param name="arr_in"></param>
					/// <returns></returns>
					public static byte[][] ConvertToByteArray(Int32[][] arr_in)
					{
						byte[][] arr_out = new byte[arr_in.Length][];
						for (Int32 i = 0; i < arr_in.Length; i++)
						{
							arr_out[i] = Int32Array.Convert.ToByteArray(arr_in[i]);
						}
						return arr_out;
					}
					/// <summary>
					/// not tested
					/// </summary>
					/// <param name="arr_in"></param>
					/// <returns></returns>
				
					public static class ToStringArray
					{

						/// <summary>
						/// Written. 2024.05.20 16:49. Warsaw. Workplace 
						/// </summary>
						/// <param name="arr_in"></param>
						/// <param name="add_to_start">0x or can be left empty</param>
						/// <param name="padding_with_0"></param>
						/// <returns></returns>
						public static string[][] ToHEX(Int32[][] arr_in, bool padding_by_zero = true, bool upper_case = true, string add_to_start = "")
						{
							float execution_time_ms_start = 0;
							if (TimeExecutionShow == true)
							{
								execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
							}
							



							if (arr_in.Length == 0)
							{
								ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
								return new string[0][];
							}

							string[][] arr_out = new string[arr_in.Length][];
							for (Int32 i = 0; i < arr_in.Length; i++)
							{
								arr_out[i] = new string[arr_in[i].Length];
								for (Int32 j = 0; j < arr_in[i].Length; j++)
								{
									try
									{
										arr_out[i][j] = System.Convert.ToString(arr_in[i][j], 16);
										if (padding_by_zero == true)
										{
											if (upper_case == true)
											{
												arr_out[i][j] = arr_out[i][j].PadLeft(8, '0').ToUpper();
												if (add_to_start != "")
												{
													arr_out[i][j] += add_to_start;
												}
											}
											else
											{
												arr_out[i][j] = arr_out[i][j].PadLeft(8, '0').ToLower();
												if (add_to_start != "")
												{
													arr_out[i][j] += add_to_start;
												}
											}
										}
									}
									catch
									{
										ReportFunctions.ReportError(i, j);
									}
								}
							}
							if (TimeExecutionShow == true)
							{
								float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
								TimeExecutionMessage(nameof(TwoDimensions.Convert.ToStringArray.ToHEX), execution_time_ms_stop - execution_time_ms_start);
							}
							return arr_out;
						}



						public static string[][] ToDecimal(Int32[][] arr_in, bool padding = true, bool right_padding = true)
						{
							// space for code. start.
							if (arr_in.Length == 0)
							{
								ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
								return new string[0][];
							}

							// 2024.05.20 16:44. Warsaw. Workplace. Added.
							int padding_length = 0;
							if (padding == true)
							{
								Int32[] max_of_column = new Int32[arr_in.Length];
								Int32[] min_of_column = new Int32[arr_in.Length];
								for (int i = 0; i < arr_in.Length; i++)
								{
									max_of_column[i] = arr_in[i].Max();
									min_of_column[i] = arr_in[i].Min();
								}

								string max_str = max_of_column.Max().ToString();
								string min_str = min_of_column.Min().ToString();

								padding_length = max_str.Length;
								if (min_str.Length > max_str.Length)
								{
									padding_length = min_str.Length;
								}
							}



							string[][] arr_out = new string[arr_in.Length][];
							for (Int32 i = 0; i < arr_in.Length; i++)
							{
								arr_out[i] = new string[arr_in[i].Length];
								for (Int32 j = 0; j < arr_in[i].Length; j++)
								{
									try
									{
										arr_out[i][j] = System.Convert.ToString(arr_in[i][j], 10);
										if (padding == true)
										{
											if (right_padding == true)
											{
												arr_out[i][j] = arr_out[i][j].PadRight(padding_length, ' ');
											}
											else
											{
												arr_out[i][j] = arr_out[i][j].PadLeft(padding_length, ' ');
											}
										}
									}
									catch
									{
										ReportFunctions.ReportError(i, j);
									}
								}
							}
							// space for code. end.
							return arr_out;
						}
					}
				}


			}



        }
        public static class ObjectArray
        {
            public static class Convert
            {
                /// <summary>
                /// not tested
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static string[] ToStringArray(object[] arr_in)
                {
                    // space for code. start.
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new string[0];
                    }
                    string[] arr_out = new string[arr_in.Length];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        try
                        {
                            arr_out[i] = (string)arr_in[i];
                        }
                        catch
                        {
                            ReportFunctions.ReportError(i);
                        }
                    }
                    // space for code. end.
                    return arr_out;
                }
                /// <summary>
                /// not tested
                /// 2023-08-04 14:31
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static string[][] ToStringArray(object[][] arr_in)
                {
                    // space for code. start.
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new string[0][];
                    }
                    string[][] arr_out = new string[arr_in.Length][];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = new string[arr_in[i].Length];
                        for (Int32 j = 0; j < arr_in[i].Length; j++)
                        {
                            try
                            {
                                arr_out[i][j] = (string)arr_in[i][j];
                            }
                            catch
                            {
                                ReportFunctions.ReportError(i, j);
                            }
                        }
                    }
                    // space for code. end.
                    return arr_out;
                }
                /// <summary>
                /// not tested
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static float[] TodoubleArray(object[] arr_in)
                {
                    // space for code. start.
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new float[0];
                    }
                    float[] arr_out = new float[arr_in.Length];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        try
                        {
                            arr_out[i] = (float)arr_in[i];
                        }
                        catch
                        {
                            ReportFunctions.ReportError(i);
                        }
                    }
                    // space for code. end.
                    return arr_out;
                }
                /// <summary>
                /// not tested
                /// 2023-08-04 14:31
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static float[][] TodoubleArray(object[][] arr_in)
                {
                    // space for code. start.
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new float[0][];
                    }
                    float[][] arr_out = new float[arr_in.Length][];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = new float[arr_in[i].Length];
                        for (Int32 j = 0; j < arr_in[i].Length; j++)
                        {
                            try
                            {
                                arr_out[i][j] = (float)arr_in[i][j];
                            }
                            catch
                            {
                                ReportFunctions.ReportError(i, j);
                            }
                        }
                    }
                    // space for code. end.
                    return arr_out;
                }
            }
            /// <summary>
            /// not tested
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static Int32[] ToInt32Array(object[] arr_in)
            {
                // space for code. start.
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return new Int32[0];
                }
                Int32[] arr_out = new Int32[arr_in.Length];
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    try
                    {
                        arr_out[i] = (int)arr_in[i];
                    }
                    catch
                    {
                        ReportFunctions.ReportError(i);
                    }
                }
                // space for code. end.
                return arr_out;
            }
            /// <summary>
            /// not tested
            /// 2023-08-04 14:31
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static Int32[][] ToInt32Array(object[][] arr_in)
            {
                // space for code. start.
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return new Int32[0][];
                }
                Int32[][] arr_out = new Int32[arr_in.Length][];
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    arr_out[i] = new Int32[arr_in[i].Length];
                    for (Int32 j = 0; j < arr_in[i].Length; j++)
                    {
                        try
                        {
                            arr_out[i][j] = (int)arr_in[i][j];
                        }
                        catch
                        {
                            ReportFunctions.ReportError(i, j);
                        }
                    }
                }
                // space for code. end.
                return arr_out;
            }
            /// <summary>
            /// Sort object array according to indexes provided
            /// 2023.08.12 12:18. not tested.
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="indexes_in"></param>
            /// <returns></returns>
            public static object[][] Sort(object[][] arr_in, Int32[] indexes_in)
            {
                return ArrayFunctions.Sort(arr_in, indexes_in);
            }
            /// <summary>
            /// Sort object array according to indexes provided
            /// 2023.08.12 12:17. not tested.
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="indexes_in"></param>
            /// <returns></returns>
            public static object[] Sort(object[] arr_in, Int32[] indexes_in)
            {
                return ArrayFunctions.Sort(arr_in, indexes_in);
            }
        }
        /// <summary>
        /// Written. 2023.11.05 12:08. Gdansk. 
        /// </summary>
        public static class CharArray
        {
           
            /// <summary>
            /// Prints char[][] in console. <br></br>
            /// Written. 2024.03.08 21:13. Warsaw. Hostel. <br></br>
            /// Tested. Works. 2024.03.08 21:16. Warsaw. Hostel.
            /// </summary>
            /// <param name="arr_in"></param>
            public static void ToConsole(char[][] arr_in)
            {
                if (arr_in.Length == 0)
                {
                    Console.WriteLine("Attention! Array is empty");
                    return;
                }

                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }

                
                string[] strings_to_console = Convert.ToStrings(arr_in);
                Console.WriteLine("Array " + typeof(char).Name.ToString() + "[][]. Length is " + arr_in.Length.ToString() + "x" + arr_in[0].Length.ToString());
                StringArray.ToConsole(strings_to_console);

                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(CharArray.ToConsole), execution_time_ms_stop - execution_time_ms_start);
                }

            }

            /// <summary>
            /// Written. 2023.11.05 13:05. Gdansk. Home.
            /// Tested. Works. 2023.11.05 13:17. Gdansk. Home.
            /// </summary>
            public static class Convert
            {

                /// <summary>
                /// Converts char[][] to string[]. 
                /// Written. 2024.03.08 21:06. Warsaw. Hostel. <br></br>
                /// Tested. Works. 2024.03.08 21:17. Warsaw. Hostel.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static string[] ToStrings(char[][] arr_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new string[0];
                    }

                    float execution_time_ms_start = 0;
                    if (TimeExecutionShow == true)
                    {
                        execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                    }
                   


                    string[] arr_out = new string[arr_in[0].Length];
                    StringBuilder make_string = new StringBuilder();
                    for (Int32 j = 0; j < arr_in[0].Length; j++)
                    {
                        make_string.Clear();
                        arr_out[j] = "";

                        for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                       
                        
                        
                            make_string.Append(arr_in[i][j]);
                        }
                        arr_out[j] = make_string.ToString();
                    }

                    if (TimeExecutionShow == true)
                    {
                        float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                        TimeExecutionMessage(nameof(Convert.ToStrings), execution_time_ms_stop - execution_time_ms_start);
                    }

                    return arr_out;
                }


                    public static Int32[] ToInt32Array(char[] arr_in)
                {
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new Int32[0];
                    }
                    Int32[] arr_out = new Int32[arr_in.Length];
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        arr_out[i] = System.Convert.ToInt32(arr_in[i]);
                    }
                    return arr_out;
                }
            }
        }
        public static class StringArray
        {
            /// <summary>
            /// Written. 2024.06.06 17:01. Warsaw. Workplace 
            /// </summary>
            public static class Generate
            {

				/// <summary>
				/// Written. 2024.06.15 21:17. Gdansk. Home 
				/// </summary>
				/// <param name="arr_size"></param>
				/// <param name="string_length"></param>
				/// <returns></returns>
				public static string[] StringWithNumber(uint arr_size, string str_start, uint start_number = 0)
				{

					float execution_time_ms_start = 0;
					if (TimeExecutionShow == true)
					{
						execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
					}

                    uint add_number = start_number;
					string[] arr_out = new string[arr_size];
					for (int i = 0; i < arr_out.Length; i++)
					{
						arr_out[i] = str_start + add_number.ToString();
                        add_number += 1;
					}

					if (TimeExecutionShow == true)
					{
						float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
						TimeExecutionMessage(nameof(Generate.StringWithNumber), execution_time_ms_stop - execution_time_ms_start);
					}

					return arr_out;
				}








				/// <summary>
				/// Written. 2024.06.06 17:01. Warsaw. Workplace.
				/// </summary>
				/// <param name="arr_size"></param>
				/// <param name="string_length"></param>
				/// <returns></returns>
				public static string[] RandomString(uint arr_size, uint string_length)
                {

					float execution_time_ms_start = 0;
					if (TimeExecutionShow == true)
					{
						execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
					}
					

					string[] arr_out = new string[arr_size];
                    for (int i = 0; i < arr_out.Length; i++)
                    {
                        arr_out[i] = StringFunctions.Generate.EN_Letters_UpperCase_Numbers(string_length);
					}

					if (TimeExecutionShow == true)
					{
						float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
						TimeExecutionMessage(nameof(Generate.RandomString), execution_time_ms_stop - execution_time_ms_start);
					}

					return arr_out;
                }
            }

			/// <summary>
			/// Written. 2024.06.15 21:13. Gdansk. Home 
			/// </summary>
			/// <param name="arr_in"></param>
			/// <param name="strings_to_find"></param>
			/// <returns></returns>
			public static string[] Exclude(string[] arr_in, string[] strings_to_exclude)
			{
				float execution_time_ms_start = 0;
				if (TimeExecutionShow == true)
				{
					execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
				}

				// counting.
				int str_exclude_count = 0;
				for (int i = 0; i < strings_to_exclude.Length; i++)
				{
					if (Find.IsStringInArray(arr_in, strings_to_exclude[i]) == true)
					{
						str_exclude_count += 1;
					}
				}

				// filing array
				int fill_index = 0;
				string[] arr_out = new string[arr_in.Length - str_exclude_count];
				for (int i = 0; i < arr_in.Length; i++)
				{
					if (Find.IsStringInArray(strings_to_exclude, arr_in[i]) == false)
					{
						arr_out[fill_index] = arr_in[i];
						fill_index += 1;
					}
				}

				if (TimeExecutionShow == true)
				{
					float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
					TimeExecutionMessage(nameof(StringArray.Exclude), execution_time_ms_stop - execution_time_ms_start);
				}

				return arr_out;
			}



			/// <summary>
			/// Written. 2024.06.06 16:57. Warsaw. Workplace.
            /// 
			/// </summary>
			public static class Find
			{
                

					/// <summary>
					/// Written. 2024.06.15 21:08. Gdansk. Home
                    /// Tested. Works. Warsaw. Workplace. 2024.07.08 17:15. 
					/// </summary>
					/// <param name="arr_in"></param>
					/// <param name="strings_to_find"></param>
					/// <returns></returns>
					public static string[] StringsNotInArray(string[] arr_in, string[] strings_to_find)
					{
						float execution_time_ms_start = 0;
						if (TimeExecutionShow == true)
						{
							execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
						}

						// counting.
						int str_not_found = 0;
						for (int i = 0; i < strings_to_find.Length; i++)
						{
							if (Find.IsStringInArray(arr_in, strings_to_find[i]) == false)
							{
								str_not_found += 1;
							}
						}

						// filing array
						int fill_index = 0;
						string[] arr_out = new string[str_not_found];
						for (int i = 0; i < strings_to_find.Length; i++)
						{
							if (Find.IsStringInArray(arr_in, strings_to_find[i]) == false)
							{
								arr_out[fill_index] = strings_to_find[i];
								fill_index += 1;
							}
						}

						if (TimeExecutionShow == true)
						{
							float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
							TimeExecutionMessage(nameof(Find.StringsNotInArray), execution_time_ms_stop - execution_time_ms_start);
						}

						return arr_out;
					}






					/// <summary>
					/// Written. 2024.06.15 20:57. Gdansk. Home.
                    /// Tested. Works. Warsaw. Workplace. 2024.07.08 16:59. 
					/// </summary>
					/// <param name="arr_in"></param>
					/// <param name="strings_to_find"></param>
					/// <returns></returns>
					public static string[] StringsInArray(string[] arr_in, string[] strings_to_find)
					{
						float execution_time_ms_start = 0;
						if (TimeExecutionShow == true)
						{
							execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
						}

                        // counting.
                        int str_found = 0;
						for (int i = 0; i < strings_to_find.Length; i++)
						{
							if (Find.IsStringInArray(arr_in, strings_to_find[i]) == true)
							{
                                str_found += 1;
							}
						}

						// filing array
						int fill_index = 0;
						string[] arr_out = new string[str_found];
						for (int i = 0; i < strings_to_find.Length; i++)
						{
							if (Find.IsStringInArray(arr_in, strings_to_find[i]) == true)
							{
								arr_out[fill_index] = strings_to_find[i];
                                fill_index += 1;
							}
                           
						}

						if (TimeExecutionShow == true)
						{
							float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
							TimeExecutionMessage(nameof(Find.StringsInArray), execution_time_ms_stop - execution_time_ms_start);
						}

						return arr_out;
					}



					/// <summary>
					/// Written. 2024.06.15 20:45. Gdansk. Home 
					/// </summary>
					/// <param name="arr_in"></param>
					/// <param name="strings_to_find"></param>
					/// <returns></returns>
					public static bool AreStringInArray(string[] arr_in, string[] strings_to_find)
					{
						float execution_time_ms_start = 0;
						if (TimeExecutionShow == true)
						{
							execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
						}


						bool search_result = true;
						for (int i = 0; i < strings_to_find.Length; i++)
						{
							if (Find.IsStringInArray(arr_in, strings_to_find[i]) != true)
							{
								search_result = false;
								break;
							}
						}


						if (TimeExecutionShow == true)
						{
							float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
							TimeExecutionMessage(nameof(Find.AreStringInArray), execution_time_ms_stop - execution_time_ms_start);
						}

						return search_result;
					}


				
                
                

				/// <summary>
				/// Written. 2024.06.15 20:42. Gdansk. Home 
				/// </summary>
				/// <param name="arr_in"></param>
				/// <param name="str_in"></param>
				/// <returns></returns>
				public static bool IsStringInArray(string[] arr_in, string str_in)
				{
					float execution_time_ms_start = 0;
					if (TimeExecutionShow == true)
					{
						execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
					}






					bool search_result = false;
                    for (int i = 0; i < arr_in.Length; i++)
                    {
                        if (arr_in[i] == str_in)
                        {
                            search_result = true;
                            break;
                        }
                    }


					if (TimeExecutionShow == true)
					{
						float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
						TimeExecutionMessage(nameof(Find.IsStringInArray), execution_time_ms_stop - execution_time_ms_start);
					}

					return search_result;
				}


				/// <summary>
				/// Written. 2024.06.15 20:38. Gdansk. Home 
				/// </summary>
				/// <param name="arr_in"></param>
				/// <param name="str_in"></param>
				/// <returns></returns>
				public static bool Contain(string[] arr_in, string str_in)
                {
                    return arr_in.Contains(str_in);
                }
				// 2024.06.06 16:57. Warsaw. Workplace 
				// By length comparison
				// Then letter by letter.
				// If there is need in increase of search speed.
				// 
				// Search using str.Contains() in array with 10000 string of 100 length.
				// element was with index 9000
				// 0,1484 ms
				// 0,148 ms
				// 0,1546 ms
				// 
				// element was with index 1000				
				// 0,022 ms
				// 0,0201 ms
				// 0,0213 ms
                // 0,0326 ms
                // 0,0302 ms
			}


			/// <summary>
			/// Written. 2024.02.06 12:20. Warsaw. Workplace. 
			/// </summary>
			public static class Rows
            {

                /// <summary>
                /// Removes empty strings from string[] and returns according string[]. <br></br>
                /// Written. 2024.02.06 12:24. Warsaw. Workplace. <br></br>
                /// Tested. Works. 2024.02.06 12:26. Warsaw. Workplace. 
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static string[] RemoveEmptyStrings(string[] arr_in)
                {
                    string[] arr_out = new string[arr_in.Length];
                    Int32 fill_count = 0;
                    for (Int32 i = 0; i < arr_in.Length;i++)
                    {
                        if (arr_in[i] != "")
                        {
                            fill_count++;
                            arr_out[fill_count - 1] = arr_in[i];
                        }
                    }
                    Array.Resize(ref arr_out, fill_count);
                    return arr_out;
                }

            }

            /*
            // commented because the path is long. 2024.01.19 15:00. Warsaw. Workplace. 
            /// <summary>
            /// Written. 2024.01.19 14:35. Warsaw. Workplace. 
            /// </summary>
            public static class Elements
            {
            */

                /// <summary>
                /// Apply certain function to each element of string[] <br></br>
                /// Written. 2024.01.19 14:37. Warsaw. Workplace. 
                /// </summary>
                public static class EachElement
                {
                    

                public static class Add
                {
                    public static string[] ToStart(string[] arr_in, string string_to_add)
                    {
                        string[] arr_out = new string[arr_in.Length];
                        for (int i = 0; i < arr_out.Length; i++)
                        {
                            arr_out[i] = string_to_add + arr_in[i];
                        }
                        return arr_out;
                        // Written. Warsaw. Workplace. 2024-07-23 15-42. 
                    }



                    // Written. Warsaw. Workplace. 2024-07-23 15-40. 
                }

                /// <summary>
                /// Written. 2024.01.31 12:27. Warsaw. Workplace.  
                /// </summary>
                public static class Trim
                {

                    /// <summary>
                    /// Written. 2024.01.31 12:31. Warsaw. Workplace. 
                    /// </summary>
                    public static class StartEnd
                    {
                        
                        /// <summary>
                        /// Written. 2024.31 12:33. Warsaw. Workplace. 
                        /// Tested. Works. 2024.01.31 14:39. Warsaw. Workplace. 
                        /// </summary>
                        /// <param name="strings_in"></param>
                        /// <returns></returns>
                        public static string[] Space_Tab_CR_LF(string[] strings_in)
                        {
                            string[] arr_out = new string[strings_in.Length];

                            for (Int32 i = 0; i < strings_in.Length; i++)
                            {
                                arr_out[i] = strings_in[i].Trim(' ', '\t', '\r', '\n');
                            }

                            return arr_out;

                        }
                    }





                }


                /// <summary>                  
                ///
                /// Written. 2024.01.19 14:38. Warsaw. Workplace. 
                /// </summary>
                public static class Cut
                    {


                    /// <summary>
                    /// 2024-07-06 15-18. Warsaw. Hostel 3.
                    /// </summary>
                    public static char[] EndOfString =
                    {
                        ' ',    // space
                        ',',    // comma
                        '\t',   // tab
                        '\r',   // new line - CR
                        '\n',   // new line - LF
                        '.'     // dot
                    };

                    /// <summary>
                    /// 2024-07-06 15-18. Warsaw. Hostel 3.
                    /// </summary>
                    /// <param name="str_arr"></param>
                    /// <param name="str_start"></param>
                    /// <param name="end_of_string"></param>
                    /// <returns></returns>
                    public static string[] FromStartToSpecificEnd(string[] str_arr, string str_start, char[] end_of_string)
                    {
                        string[] str_out = new string[str_arr.Length];
                        for (int i = 0; i < str_out.Length; i++)
                        {
                            str_out[i] = StringFunctions.Cut.CutFromTextToEnd(str_arr[i], str_start, EndOfString);
                        }
                        return str_out;
                    }

                    /// <summary>
                    /// Written. 2024.01.30 05:08. Warsaw. Hostel.
                    /// 
                    /// Returns string[] with each element contains strings before string provided. <br></br> 
                    /// "" if elements does not contain the text. <br></br>
                    /// Written. 2024.01.30 05:12. Warsaw. Hostel. <br></br>
                    /// 
                    /// </summary>
                    /// <param name="strings_in"></param>
                    /// <param name="text_start"></param>
                    /// <returns></returns>
                    public static string[] BeforeText(string[] strings_in, string text_start)
                    {
                        string[] arr_out = new string[strings_in.Length];

                        for (Int32 i = 0; i < strings_in.Length; i++)
                        {
                            arr_out[i] = StringFunctions.Cut.BeforeText(strings_in[i], text_start);
                        }

                        return arr_out;

                    }



                    /// <summary>
                    /// Returns string[] with each element starts from text provided or 
                    /// "" if elements does not contain the text. <br></br>
                    /// Written. 2024.01.19 14:39. Warsaw. Workplace. <br></br>
                    /// Tested. Works. 2024.01.19 15:11. Warsaw. Workplace. 
                    /// </summary>
                    /// <returns></returns>
                    public static string[] FromText(string[] strings_in, string text_start)
                        {
                            string[] arr_out = new string[strings_in.Length];
                            
                            for (Int32 i = 0; i < strings_in.Length; i++)
                            {
                                arr_out[i] = StringFunctions.Cut.StartingFromText(strings_in[i], text_start);
                            }

                            return arr_out;

                        }
                    }
                }
            

            
                /// <summary>
                /// written. 2023.09.09 11:38.
                /// note. addstring and addstrings or alone addstrings with 1 string to add.
                /// </summary>
                public static class AddString
            {
                /// <summary>
                /// Adds string to the start of file.
                /// 2023.10.02 16:16. written. <br></br>
                /// 2023.10.03 14:53. tested. works.
                /// </summary>
                /// <param name="string_in"></param>
                /// <param name="file_path"></param>
                public static string[] ToEnd(string string_in, string[] arr_in)
                {
                    string[] strings_out = new string[arr_in.Length + 1];
                    if (arr_in.Length == 0)
                    {
                        // 2023.10.03 15:05. no need. the function will work properly
                    }
                    Array.Copy(arr_in, strings_out, arr_in.Length);
                    strings_out[strings_out.Length - 1] = string_in;
                    return strings_out;
                }
                /// <summary>
                /// Adds string to the start of string array
                /// 2023.10.02 16:17. written<br></br>
                /// 2023.10.03 14:52. tested. works.
                /// </summary>
                /// <param name="string_in"></param>
                /// <param name="file_path"></param>
                public static string[] ToStart(string string_in, string[] arr_in)
                {
                    string[] strings_out = new string[arr_in.Length + 1];
                    Array.Copy(arr_in, 0, strings_out, 1, arr_in.Length);
                    strings_out[0] = string_in;
                    return strings_out;
                }
                /// <summary>
                /// 2023.10.02 16:46. written <br></br>
                /// 2023.10.03 15:00. tested. works.
                /// </summary>
                /// <param name="string_in"></param>
                /// <param name="line_number">this is not index</param>
                /// <param name="file_path"></param>
                public static string[] AfterLine(string string_in, UInt32 line_number, string[] arr_in)
                {
                    string[] strings_out = new string[arr_in.Length + 1];
                    Array.Copy(arr_in, 0, strings_out, 0, line_number);
                    Array.Copy(arr_in, strings_out, line_number);
                    strings_out[line_number - 1 + 1] = string_in;
                    Array.Copy(
                        arr_in, line_number - 1 + 1,
                        strings_out, line_number - 1 + 1 + 1, arr_in.Length - line_number);
                    return strings_out;
                }
            }
            /// <summary>
            /// PrInt32 string array in console
            /// 2023.08.22 14:28
            /// </summary>
            /// <param name="str_arr"></param>
            static public void ToConsole(string[] str_arr)
            {
                for (Int32 sn = 0; sn < str_arr.Length; sn++)
                {
                    Console.WriteLine(str_arr[sn]);
                }
            }
            /// <summary>
            /// Prints string[][] to console. <br></br>
            /// 2023.08.22 16:05. written. <br></br>
            /// 2023.08.22 16:05. tested. works. 
            /// </summary>
            /// <param name="arr_in"></param>
            public static void ToConsole(string[][] arr_in)
            {
                // Type type = typeof(int);
                if (arr_in.Length == 0)
                {
                    Console.WriteLine("Attention! Array is empty");
                    return;
                }
                Console.WriteLine("Array " + typeof(string).Name.ToString() + ". Length is " + arr_in.Length.ToString() + "x" + arr_in[0].Length.ToString());
                string[] all_strings = ArrayFunctions.Merge.NxM_To_A(arr_in);
                string longest_string = all_strings.OrderByDescending(s => s.Length).First();
                Int32 pad_size = longest_string.Length;
                string[][] str_arr = StringArray.Pad.Right(arr_in, pad_size);
                string str_for_console = StringArray.Convert.ToFileString(str_arr, "".PadRight(3, ' '));
                Console.WriteLine(str_for_console);
            }
            //public static bool InfoToConsole = false;
            public static class Pad
            {
                /// <summary>
                /// Adds certain amount of symbol to the right of each string
                /// 2023.07.20 - 2023.08.20. 10 - 15 o'clock. written.
                /// 2023.08.22 15:26. tested. works. 
                /// </summary>
                /// <param name="str_arr"></param>
                /// <param name="total_width"></param>
                /// <param name="pad_char"></param>
                /// <returns></returns>
                public static string[][] Right(string[][] str_arr, Int32 total_width, char pad_char = ' ')
                {
                    if (str_arr.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new string[0][];
                    }
                    string[][] arr_out = new string[str_arr.Length][];
                    for (Int32 i = 0; i < str_arr.Length; i++)
                    {
                        arr_out[i] = new string[str_arr[i].Length];
                        for (Int32 j = 0; j < str_arr[i].Length; j++)
                        {
                            arr_out[i][j] = str_arr[i][j].PadRight(total_width, pad_char);
                        }
                    }
                    return arr_out;
                }
                /// <summary>
                /// Adds certain amount of symbol to the left of each string
                /// 2023.07.20 - 2023.08.20. 10 - 15 o'clock. written.
                /// 2023.08.29 11:36. tested. works.
                /// </summary>
                /// <param name="str_arr"></param>
                /// <param name="total_width"></param>
                /// <param name="pad_char"></param>
                /// <returns></returns>
                public static string[][] Left(string[][] str_arr, Int32 total_width, char pad_char = ' ')
                {
                    if (str_arr.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new string[0][];
                    }
                    string[][] arr_out = new string[str_arr.Length][];
                    for (Int32 i = 0; i < str_arr.Length; i++)
                    {
                        arr_out[i] = new string[str_arr[i].Length];
                        for (Int32 j = 0; j < str_arr[i].Length; j++)
                        {
                            arr_out[i][j] = str_arr[i][j].PadLeft(total_width, pad_char);
                        }
                    }
                    return arr_out;
                }
            }
            public static class Convert
            {
                /// <summary>
                /// Writes String[][] to file using delimer between each string in the row
                /// Tested. Works. 2023.12.20 16:32. Workplace. <br></br>
                /// Moved to MyArrayFunctions.StringArray. 2023.12.20 16:19. Workplace. <br></br>                /// 
                /// </summary>
                /// <param name="strings_in"></param>
                /// <param name="key_value_delimer"></param>
                /// <returns></returns>
                static public string StringsAxBToFilestring(string[][] strings_in, char key_value_delimer = '\t')
                {
                    string str_for_return = "";
                    Int32 count_done = 0;
                    StringBuilder string_make = new StringBuilder(strings_in.Length * strings_in[0].Length * 2);
                    string_make.Clear();
                    for (Int32 i = 0; i < strings_in[0].Length; i++)
                    {
                        // 2023.12.20 16:30. Workplace.
                        // there should be \r\n at the end of each line. the need of no \r\n at the last line came from Split.
                        // Replace "\r\n" by "\n" and then Split('\n')
                        /*
                        if (i != 0)
                        {
                            string_make.Append("\r\n");
                            //str_for_return += "\r\n";
                        }
                        */
                        for (Int32 j = 0; j < strings_in.Length; j++)
                        {
                            if (j != 0)
                            {
                                string_make.Append(key_value_delimer);
                                // str_for_return += key_value_delimer.ToString();
                            }
                            string_make.Append(strings_in[j][i]);
                            //str_for_return += strings_in[j][i];
                        }
                        string_make.Append("\r\n");
                        // No need to see performance. 2023.12.20 16:26.
                        /*
                        count_done++;
                        if (count_done >= 0.01 * strings_in[0].Length)
                        {
                            count_done = 0;
                            Console.WriteLine("Completed " + ((float)i / (float)strings_in[0].Length).ToString());
                        }
                        */
                    }
                    str_for_return = string_make.ToString();
                    return str_for_return;
                }
                /// <summary>
                /// 2023.10.10 14:46. moved here. Workplace. <br></br>
                /// 2023.10.10 14:51. Tested. Works.
                /// </summary>
                /// <param name="str_arr"></param>
                /// <param name="delimer"></param>
                /// <returns></returns>
                static public string[][] ToStringArrayNxM(string[] str_arr, char delimer)
                {
                    // 1. if array has zero length, it may indicate that the code is wrong.
                    if (str_arr.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new string[0][];
                    }
                    // 2. columns number
                    Int32 columns_num = str_arr[0].Split(delimer).Length;
                    // 3. array Int32[][]
                    string[][] arr_out = new string[columns_num][];
                    for (Int32 i = 0; i < columns_num; i++)
                    {
                        arr_out[i] = new string[str_arr.Length];
                    }
                    // 4. filling array
                    for (Int32 i = 0; i < str_arr.Length; i++)
                    {
                        string[] to_put_in_arr = str_arr[i].Split(delimer);
                        if (to_put_in_arr.Length != columns_num)
                        {
                            ReportFunctions.ReportError("File text line has different amount of columns" + "\r\n" +
                                "file line is " + i.ToString() + "\r\n" +
                                "line has " + to_put_in_arr.Length.ToString() + " columns" + "\r\n" +
                                "number of columns should be " + columns_num.ToString());
                            return arr_out;
                        }
                        for (Int32 j = 0; j < columns_num; j++)
                        {
                            arr_out[j][i] = to_put_in_arr[j];
                        }
                    }
                    // 5. return array string[][]
                    return arr_out;
                }
                /// <summary>
                /// Written. 2023.8.06 01:35
                /// Tested. Works. 2024.01.21 16:48. Warsaw. Hostel. 
                /// </summary>
                /// <param name="str_arr"></param>
                /// <returns></returns>
                public static string ToFileString(string[] str_arr)
                {
                    StringBuilder str_out = new StringBuilder();
                    for (Int32 i = 0; i < str_arr.Length; i++)
                    {
                        // 2024.01.21 16:46. Warsaw. Hostel. 
                        // "\r\n" is at the end of each line.
                        /*
                        if (i != 0)
                        {
                            str_out.Append("\r\n");
                        }
                        */
                        str_out.Append(str_arr[i]);
                        str_out.Append("\r\n");
                    }
                    return str_out.ToString();
                }
                /// <summary>
                /// Converts string[][] to string for file
                /// 2023.05.01 - 2023.06.01. 10-15 o'clock. written.
                /// 2023.08.22 15:24. tested. works.
                /// </summary>
                /// <remarks>
                /// Works if arrays of the same length
                /// 2023.08.29 13:56
                /// </remarks>
                /// <param name="str_arr"></param>
                /// <param name="delimer_in"></param>
                /// <returns></returns>
                public static string ToFileString(string[][] str_arr, string delimer_in = "\t")
                {
                    if (str_arr.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return "";
                    }
                    Int32 arr_length_check = str_arr[0].Length;
                    for (Int32 a = 0; a < str_arr.Length; a++)
                    {
                        if (str_arr[a].Length != arr_length_check)
                        {
                            ReportFunctions.ReportError(ReportFunctions.ErrorMessage.LengthDifferent);
                            return "";
                        }
                    }
                    StringBuilder str_out = new StringBuilder();
                    for (Int32 j = 0; j < str_arr[0].Length; j++)
                    {
                        for (Int32 i = 0; i < str_arr.Length; i++)
                        {
                            if (i != 0)
                            {
                                str_out.Append(delimer_in);
                            }
                            str_out.Append(str_arr[i][j]);
                        }
                        str_out.Append("\r\n");
                    }
                    return str_out.ToString();
                }

                [Obsolete]
                /// <summary>
                /// To remove. Not proper location. 2024.01.30 12:27
                /// </summary>
                /// <param name="arr_in"></param>
                /// <param name="delimer"></param>
                /// <returns></returns>
                public static Int32[][] ToInt32Array(string[] arr_in, char delimer)
                {
                    // 1. zero length may indicate that there is wrong code and
                    // therefore there is notification to console.
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new Int32[0][];
                    }
                    // 2. array Int32[][] for return
                    Int32[][] arr_out = new Int32[arr_in.Length][];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        try
                        {
                            // arr_out[i] = System.Convert.ToInt32(arr_in[i]);
                        }
                        catch
                        {
                            ReportFunctions.ReportError(i);
                        }
                    }
                    // space for code. end.
                    return arr_out;
                }


                /// <summary>
                /// Written. 2024.01.30 12:28. Warsaw. Workplace. <br></br>
                /// Tested. Works. 2024.01.30 13:33. Warsaw. Workplace. <br></br>
                /// <br></br>
                /// Note. if there is negative number then the number will be 0. <br></br>
                /// Conversion from string is not as cast of negative Int32 number to unit
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static UInt32[] ToUInt32Array(string[] arr_in)
                {                    
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new UInt32[0];
                    }
                    UInt32[] arr_out = new UInt32[arr_in.Length];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        try
                        {
                            arr_out[i] = System.Convert.ToUInt32(arr_in[i]);
                        }
                        catch
                        {
                            ReportFunctions.ReportError(i);
                        }
                    }                    
                    return arr_out;
                }



                /// <summary>
                /// Tested. Works. 2024.01.30 13:36. Warsaw. Workplace.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static Int32[] ToInt32Array(string[] arr_in)
                {
                    // space for code. start.
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new Int32[0];
                    }
                    Int32[] arr_out = new Int32[arr_in.Length];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        try
                        {
                            arr_out[i] = System.Convert.ToInt32(arr_in[i]);
                        }
                        catch
                        {
                            ReportFunctions.ReportError(i);
                        }
                    }
                    // space for code. end.
                    return arr_out;
                }
                /// <summary>
                /// not tested
                /// 2023-08-04 15:21
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static Int32[][] ToInt32Array(string[][] arr_in)
                {
                    // space for code. start.
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new Int32[0][];
                    }
                    Int32[][] arr_out = new Int32[arr_in.Length][];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = new Int32[arr_in[i].Length];
                        for (Int32 j = 0; j < arr_in[i].Length; j++)
                        {
                            try
                            {
                                arr_out[i][j] = System.Convert.ToInt32(arr_in[i][j]);
                            }
                            catch
                            {
                                ReportFunctions.ReportError(i, j);
                            }
                        }
                    }
                    // space for code. end.
                    return arr_out;
                }
                /// <summary>
                /// string is char[] and each char is 2 bytes
                /// 2023-08-01 15:27
                /// </summary>
                /// <param name="str_in"></param>
                /// <returns></returns>
                /// 
                // to move or remove
                public static Int16[] ToInt16ByChar(string str_in)
                {
                    Int16[] arr_out = Array.ConvertAll(str_in.ToArray(), CharToInt16);
                    Int16 CharToInt16(char char_in)
                    {
                        return (Int16)char_in;
                    }
                    return arr_out;
                }
                public static Int16[] ToInt16Array(string[] strings_in)
                {
                    // space for code. start.
                    if (strings_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new Int16[0];
                    }
                    Int16[] arr_out = new Int16[strings_in.Length];
                    for (Int32 i = 0; i < strings_in.Length; i++)
                    {
                        try
                        {
                            arr_out[i] = System.Convert.ToInt16(strings_in[i]);
                        }
                        catch
                        {
                            ReportFunctions.ReportError(i);
                        }
                    }
                    // space for code. end.
                    return arr_out;
                }
                public static byte[] ToByteArray(string[] strings_in, Int32 base_of_byte = 16)
                {
                    // space for code. start.
                    byte[] arr_out = new byte[strings_in.Length];
                    for (Int32 i = 0; i < strings_in.Length; i++)
                    {
                        try
                        {
                            arr_out[i] = System.Convert.ToByte(strings_in[i], base_of_byte);
                        }
                        catch
                        {
                            ReportFunctions.ReportError(i);
                        }
                    }
                    // space for code. end.
                    return arr_out;
                }
                /// <summary>
                /// Accepts: object[]
                /// 2023-08-01 15:26
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                /// 
                // !to move in object array
                public static string[] ToStringArray(object[] arr_in)
                {
                    string[] arr_out = new string[arr_in.Length];
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        try
                        {
                            arr_out[i] = (string)arr_in[i];
                        }
                        catch
                        {
                            ReportFunctions.ReportError("Cast failed at " + i.ToString());
                        }
                    }
                    return arr_out;
                }
            }
            public static class Merge
            {

                /// <summary>
                /// Takes string[n] and string[n] and returns string[2][n]. <br></br>
                /// Written. 2024.01.16 10:22. Warsaw. Workplace. <br></br>
                /// Tested. Works. 2024.01.16 10:55. Warsaw. Workplace.
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="arr_1"></param>
                /// <param name="arr_2"></param>
                /// <returns></returns>
                public static string[][] A_B_To_NxM(string[] arr_1, string[] arr_2)
                {
                    return ArrayFunctions.Merge.A_B_To_NxM(arr_1, arr_2);
                }


                public static string[] A_B_To_C(string[] arr_1, string[] arr_2)
                {
                    return ArrayFunctions.Merge.A_B_To_C(arr_1, arr_2);
                }
                public static string[] NxM_To_A(string[][] arr_in)
                {
                    return ArrayFunctions.Merge.NxM_To_A(arr_in);
                }
            }
        }
        public static class ByteArray
        {
            public static class AddByte
            {
                /// <summary>
                /// Adds byte to the end of array.
                /// Written. 2023.12.12 17:47. Workplace <br></br>
                /// Tested. Works. 2023.12.12 17:56. Workplace.
                /// </summary>
                /// <param name="string_in"></param>
                /// <param name="file_path"></param>
                public static byte[] ToEnd(byte byte_in, byte[] arr_in)
                {
                    byte[] bytes_out = new byte[arr_in.Length + 1];
                    Array.Copy(arr_in, bytes_out, arr_in.Length);
                    bytes_out[bytes_out.Length - 1] = byte_in;
                    return bytes_out;
                }
                /// <summary>
                /// Adds byte to the start of array.
                /// Written. 2023.12.12 17:50. Workplace <br></br>
                /// Tested. Works. 2023.12.12 17:55. Workplace.
                /// </summary>
                /// <param name="string_in"></param>
                /// <param name="file_path"></param>
                public static byte[] ToStart(byte byte_in, byte[] arr_in)
                {
                    byte[] bytes_out = new byte[arr_in.Length + 1];
                    Array.Copy(arr_in, 0, bytes_out, 1, arr_in.Length);
                    bytes_out[0] = byte_in;
                    return bytes_out;
                }
                /// <summary>
                /// Adds byte after certain byte in array
                /// Written. 2023.12.12 17:52. Workplace <br></br>
                /// not tested.
                /// </summary>
                /// <param name="string_in"></param>
                /// <param name="file_path"></param>
                public static byte[] AfterIndex(byte byte_in, Int32 byte_index, byte[] arr_in)
                {
                    byte[] bytes_out = new byte[arr_in.Length + 1];
                    Array.Copy(arr_in, 0, bytes_out, 0, byte_index);
                    Array.Copy(arr_in, bytes_out, byte_index);
                    bytes_out[byte_index - 1 + 1] = byte_in;
                    Array.Copy(
                        arr_in, byte_index - 1 + 1,
                        bytes_out, byte_index - 1 + 1 + 1, arr_in.Length - byte_index);
                    return bytes_out;
                }
            }
            /// <summary>
            /// Written. 2023.11.27 08:42. Warsaw. Workplace. 
            /// Tested. Works. 2023.11.27 08:50. Warsaw. Workplace.
            /// </summary>
            public class NextByteCyclically
            {
                byte[] bytes_arr = null;
                public NextByteCyclically(byte[] arr_in)
                {
                    bytes_arr = arr_in;
                }
                Int32 iofbyte = 0;
                public byte NextByte()
                {
                    byte byte_out = bytes_arr[iofbyte];
                    iofbyte++;
                    if (iofbyte > (bytes_arr.Length - 1))
                    {
                        iofbyte = 0;
                    }
                    return byte_out;
                }
            }
            /// <summary>
            /// Written. 2023.11.23 11:44. Warsaw. Workplace. <br></br>
            /// Tested. Works. 2023.11.23 11:57. Warsaw. Workplace. 
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static byte Average(byte[][] arr_in)
            {
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Length_is_0);
                    return 0;
                }
                // 2023.11.23 11:33. Warsaw. Workplace. Deviding by 0 is error
                // so there should be non zero array
                bool is_non_zero = false;
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    if (arr_in[i].Length != 0)
                    {
                        is_non_zero = true;
                        break;
                    }
                }
                if (is_non_zero == false)
                {
                    ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Length_is_0);
                    return 0;
                }
                Int32 size_of_array = 0;
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    size_of_array += arr_in[i].Length;
                    if (size_of_array > 8000000)
                    {
                        break;
                    }
                }
                // 2023.11.23 11:38. Warsaw. Workplace. See average for byte[].
                if (arr_in.Length > 8000000)
                {
                    ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Length_is_exceeded);
                    return 0;
                }
                Int32 byte_selector(byte byte_in)
                {
                    return (int)byte_in;
                }
                Int32 sum_bytes = 0;
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    sum_bytes += arr_in[i].Sum(byte_selector);
                }
                Int32 average_bytes = sum_bytes / size_of_array;
                if (average_bytes > 255)
                {
                    // 2023.11.23 11:44. Warsaw. Workplace. Average from numbers
                    // that are no larger than 255 can not be larger than 255
                    ReportFunctions.ReportError("This should not occur. Check the code");
                    return 0;
                }
                return (byte)average_bytes;
            }
            /// <summary>
            /// Written. 2023.11.23 10:46. Warsaw. Workplace. <br></br>
            /// Tested. Works. 2023.11.23 11:12. Warsaw.  Warsaw. Workplace. 
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            // Notes. 2023.11.23 10:25. Warsaw. Workplace. 
            // 1. 2023.11.23 10:27. Warsaw. Workplace. UInt32 can be 4,294,967,295
            // and with 255 max value of byte, it gives 16,843,009 bytes for averaging
            // 2. 2023.11.23 10:27. Warsaw. Workplace. Check is added in the function
            // 3. 2023.11.23 10:36. Warsaw. Workplace. C# function works with int
            // so there is number 2,147,483,647 giving 8,421,504 bytes for averaging
            // check was corrected.
            // 4. 2023.11.23 10:38. Warsaw. Workplace. note from function.
            // 2023.11.23 10:29. Warsaw. Workplace. 16777216 is 1 in the 1st bit of 4th byte
            // 5. 2023.11.23 11:11. Warsaw.  Warsaw. Workplace. Averaging from 1 to N
            // gives N/2 - 1
            public static byte Average(byte[] arr_in)
            {
                if (arr_in.Length == 0)
                {
                    ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Length_is_0);
                    return 0;
                }
                if (arr_in.Length > 8000000)
                {
                    // 2023.11.23 10:29. Warsaw. Workplace. 16777216 is  
                    // 1 in the 1st bit of 4th byte
                    ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Length_is_exceeded);
                    return 0;
                }
                Int32 byte_selector(byte byte_in)
                {
                    return (int)byte_in;
                }
                Int32 sum_bytes = arr_in.Sum(byte_selector);
                Int32 average_bytes = sum_bytes / arr_in.Length;
                if (average_bytes > 255)
                {
                    // 2023.11.23 10:54. Warsaw.  Warsaw. Workplace. Average from numbers
                    // that are no larger than 255 can not be larger than 255
                    ReportFunctions.ReportError("This should not occur. Check the code");
                    return 0;
                }
                return (byte)average_bytes;
            }
            /// <summary>
            /// 2023.09.02 19:43. written.
            /// 2023.09.02 19:43. tested. works.
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static byte[] Copy(byte[] arr_in)
            {
                return ArrayFunctions.Copy(arr_in);
            }


            /// <summary>
            /// Written. 2024.01.31 16:49. Warsaw. Workplace.
            /// not tested.
            /// </summary>
            /// <param name="numbers_num_x"></param>
            /// <param name="numbers_num_y"></param>
            /// <param name="min"></param>
            /// <param name="max"></param>
            /// <returns></returns>
            public static byte[][] Generate(Int32 numbers_num_x, Int32 numbers_num_y, byte min, byte max)
            {
                // 2024.01.31 16:51. Warsaw. Workplace.  
                // note. There is method. Generate Int32[][] and then convert to byte[][].
                // it will take more time and therefore it is not used here.

                byte[][] arr_out = new byte[numbers_num_x][];
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    arr_out[i] = new byte[numbers_num_y];
                    for (Int32 j = 0; j < arr_out[i].Length; j++)
                    {
                        
                        arr_out[i][j] = (byte)_internal_random.Next(min, max + 1);
                    }
                }
                return arr_out;
            }







            /// <summary>
            /// Creates byte[] filled with certain number <br></br>
            /// Written. 2023.12.14 12:37. Workplace. <br></br>
            /// Tested. Works. 2023.12.14 12:38. Workplace.
            /// </summary>
            /// <param name="arr_size"></param>
            /// <param name="value_in"></param>
            /// <returns></returns>
            public static byte[] Generate(Int32 arr_size, byte value_in)
            {
                byte[] byte_array = new byte[arr_size];
                for (Int32 i = 0; i < arr_size; i++)
                {
                    byte_array[i] = value_in;
                }
                return byte_array;
            }
            /// <summary>
            /// 2023.08.29 11:18. written.
            /// 2023.08.29 11:19. tested. works.
            /// </summary>
            /// <param name="arr_size"></param>
            /// <returns></returns>
            public static byte[] Generate(Int32 arr_size)
            {
                // note. it may work until 255. 2023.12.14 12:35. Workplace
                // 2024.02.04 15:20. Warsaw. Hostel. There were problem. If array size was larger than 255
                // the number could be converted to byte. The function was written not for large array.
                // Fixed. Generate(arr_size) -> Generate(arr_size,0,255);

                Int32[] int32_array = ArrayFunctions.Int32Array.Generate.RandomMinMaxValue(arr_size,0,255);
                byte[] byte_array = ArrayFunctions.Int32Array.Convert.ToByteArray(int32_array);
                return byte_array;
            }
            /// <summary>
            /// 2023.08.29 11:24. written. <br></br>
            /// 2023.08.29 11:28. tested. works.
            /// </summary>
            /// <param name="amount_in_column"></param>
            /// <param name="columns_number"></param>
            /// <param name="continue_number">
            /// 2023.11.23 11:52. Warsaw. Workplace. that parameter
            /// was not finished. each column is not started from 1.
            /// the number continues. 
            /// For example: 1 column - 1 to 10, 2 column - 11 - 20
            /// 
            /// </param>
            /// <returns></returns>
            public static byte[][] Generate(Int32 amount_in_column, Int32 columns_number, bool continue_number)
            {
                byte[][] arr_out = new byte[columns_number][];
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    arr_out[i] = Int32Array.Convert.ToByteArray(Int32Array.Generate.WithValueIncreasesBy1(amount_in_column));
                }
                return arr_out;
            }
            public static class Convert
            {

                /// <summary>
                /// Convert byte[][] to char[][]. <br></br>
                /// Written. 2024.03.08 21:00. Warsaw. Hostel. <br></br>
                /// Tested. Works. 2024.03.08 21:23. Warsaw. Hostel.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static char[][] ToCharArray(byte[][] arr_in)
                {
                   if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new char[0][];
                    }

                    float execution_time_ms_start = 0;
                    if (TimeExecutionShow == true)
                    {
                        execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                    }
                   




                    char[][] char_arr_out = new char[arr_in.Length][];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        char_arr_out[i] = new char[arr_in[i].Length];
                        for (Int32 j = 0; j < arr_in.Length; j++)
                        {
                            char_arr_out[i][j] = System.Convert.ToChar(arr_in[i][j]);
                        }
                    }


                    if (TimeExecutionShow == true)
                    {
                        float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                        TimeExecutionMessage(nameof(Convert.ToCharArray), execution_time_ms_stop - execution_time_ms_start);
                    }
                    return char_arr_out;
                }



                /// <summary>
                /// Converts byte[] to HEX (ASCII) string.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static string ToStringHEX(byte[] arr_in)
                {
                    string str_out = "";
                    StringBuilder str_make = new StringBuilder();
                    str_make.Capacity = arr_in.Length * 2 * 2;
                    for (int i = 0; i < arr_in.Length; i++)
                    {
                        str_make.Append(System.Convert.ToString(arr_in[i], 16));
                    }
                    str_out = str_make.ToString();
                    return str_out;

                    // Written. Warsaw. Workplace. 2024-08-09 12:54. 

                }

                    /// <summary>
                    /// Converts byte[][] to byte[][]
                    /// 2023.08.25 10:41. written.
                    /// 2023.08.25 10:41. not tested
                    /// </summary>
                    /// <param name="arr_in"></param>
                    /// <returns></returns>
                    public static string[][] ToStringArray(byte[][] arr_in, Int32 base_to_convert = 16)
                {
                    // space for code. start.
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new string[0][];
                    }
                    string[][] arr_out = new string[arr_in.Length][];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = new string[arr_in[i].Length];

                        // 2024.03.22 10:28. Warsaw. Workplace.
                        // ToString("x2"). Not checked. It should convert byte to HEX string with left padding with char '0'


                        for (Int32 j = 0; j < arr_in[i].Length; j++)
                        {
                            try
                            {
                                arr_out[i][j] = System.Convert.ToString(arr_in[i][j], base_to_convert);
                            }
                            catch
                            {
                                ReportFunctions.ReportError(i, j);
                            }
                        }
                    }
                    // space for code. end.
                    return arr_out;
                }



                /// <summary>
                /// Converts byte[] to float[] using 4 bytes for float. <br></br>
                /// Array size should be divisible by 4 without reminder (reminder is 0). <br></br>
                /// Written. 2024.01.16 09:06. Warsaw. Workplace.
                /// Tested. Works. 2024.01.16 10:52. Warsaw. Workplace.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>

                public static float[] ToFloatArrayByBitConversion(byte[] arr_in)
                {
                    if (arr_in.Length % 4 != 0)
                    {
                        ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Length_is_Wrong);
                        return new float[0];
                    }
                    float[] arr_out = new float[arr_in.Length / 4];
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        byte[] float_bytes = ByteArray.ExtractArray.FromIndexByLength(arr_in, (uint)(i * 4), 4);
                        UInt32 number_for_float = MathFunctions.UInt32Number.BytesToUInt32(float_bytes);
                        arr_out[i] = MathFunctions.UInt32Number.Convert.ToFloatByBitConversion(number_for_float);
                    }
                    return arr_out;
                }




                /// <summary>
                /// Converts byte[] into UInt16[] by using 2 bytes for UInt16.<br></br>
                /// Array size should be divisible by 2 without reminder (reminder is 0). <br></br>
                /// Written. 2024.01.16 08:41. Warsaw. Workplace. <br></br>
                /// <br></br>
                /// Tested. Works. 2024.01.16 10:52. Warsaw. Workplace.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static UInt16[] ToUInt16Merge2Bytes(byte[] arr_in)
                {
                    if (arr_in.Length % 2 != 0)
                    {
                        ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Length_is_Wrong);
                        return new UInt16[0];
                    }
                    UInt16[] arr_out = new UInt16[arr_in.Length / 2];
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        arr_out[i] = (UInt16)(arr_in[i * 2] << 8);
                        arr_out[i] |= (UInt16)(arr_in[i * 2 + 1] << 0);
                    }
                    return arr_out;
                }













                /// <summary>
                /// Converts byte[] into Int16[] by using 2 bytes for Int16.<br></br>
                /// Array size should be divisible by 2 without reminder (reminder is 0). <br></br>
                /// Written. 2023.12.14 12:30. Workplace. <br></br>
                /// <br></br>
                /// Tested. Works. 2023.12.14 12:41. Warsaw. Workplace.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static Int16[] ToInt16Merge2Bytes(byte[] arr_in)
                {
                    if (arr_in.Length % 2 != 0)
                    {
                        ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Length_is_Wrong);
                        return new Int16[0];
                    }
                    Int16[] arr_out = new Int16[arr_in.Length / 2];
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        arr_out[i] = (Int16)(arr_in[i * 2] << 8);
                        arr_out[i] |= (Int16)(arr_in[i * 2 + 1] << 0);
                    }
                    return arr_out;
                }
                /// <summary>
                /// Converts byte[][] to Int32[][]
                /// 2023.08.23 16:46. written.
                /// 2023.08.23 16:46. not tested
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static Int32[][] ToInt32ArrayAxB(byte[][] arr_in)
                {
                    // space for code. start.
                    if (arr_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new Int32[0][];
                    }
                    Int32[][] arr_out = new Int32[arr_in.Length][];
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = new Int32[arr_in[i].Length];
                        for (Int32 j = 0; j < arr_in[i].Length; j++)
                        {
                            try
                            {
                                arr_out[i][j] = System.Convert.ToInt32(arr_in[i][j]);
                            }
                            catch
                            {
                                ReportFunctions.ReportError(i, j);
                            }
                        }
                    }
                    // space for code. end.
                    return arr_out;
                }
            }
            public static class Split
            {
                /// <summary>
                /// 2023.08.28 16:57. written.
                /// 2023.08.28 16:57. tested. works.
                /// </summary>
                /// <param name="array_in"></param>
                /// <param name="split_value"></param>
                /// <returns></returns>
                public static Int32[] IndexesToSplit(byte[] array_in, byte split_value)
                {
                    // space for code. start
                    // counting
                    Int32 split_indexes_count = 0;
                    for (Int32 i = 0; i < array_in.Length; i++)
                    {
                        if (array_in[i] == split_value)
                        {
                            split_indexes_count++;
                        }
                    }
                    Int32[] arr_out = new Int32[split_indexes_count];
                    Int32 arr_out_index = 0;
                    for (Int32 i = 0; i < array_in.Length; i++)
                    {
                        if (array_in[i] == split_value)
                        {
                            arr_out[arr_out_index] = i;
                            arr_out_index++;
                        }
                    }
                    return arr_out;
                }
                /// <summary>
                /// 2023.08.28 17:16. written.
                /// 2023.08.28 17:16. tested. works.
                /// </summary>
                /// <remarks>Not equal length will be made equal and filled with 0</remarks>
                /// <param name="array_in"></param>
                /// <param name="split_indexes"></param>
                /// <returns></returns>
                public static byte[][] ByIndex(byte[] array_in, Int32[] split_indexes, bool same_length = true)
                {
                    byte[][] arr_out = new byte[split_indexes.Length + 1][];
                    Int32 size_arr_copy = 0;
                    Int32 arr_out_index = 0;
                    size_arr_copy = split_indexes[0] + 1;
                    arr_out[arr_out_index] = new byte[size_arr_copy];
                    Array.Copy(array_in, arr_out[arr_out_index], size_arr_copy);
                    arr_out_index++;
                    for (Int32 i = 0; i < split_indexes.Length - 1; i++)
                    {
                        size_arr_copy = split_indexes[i + 1] - split_indexes[i] + 1;
                        arr_out[arr_out_index] = new byte[size_arr_copy];
                        Array.Copy(array_in, split_indexes[i], arr_out[arr_out_index], 0, size_arr_copy);
                        arr_out_index++;
                    }
                    size_arr_copy = array_in.Length - 1 - split_indexes[split_indexes.Length - 1] + 1;
                    arr_out[arr_out.Length - 1] = new byte[size_arr_copy];
                    Array.Copy(array_in, split_indexes[split_indexes.Length - 1], arr_out[arr_out.Length - 1], 0, size_arr_copy);
                    // 2023.08.29 13:51. arrays to equal length
                    if (same_length == true)
                    {
                        Int32 max_length = 0;
                        for (Int32 i = 0; i < arr_out.Length; i++)
                        {
                            if (arr_out[i].Length > max_length)
                            {
                                max_length = arr_out[i].Length;
                            }
                        }
                        for (Int32 i = 0; i < arr_out.Length; i++)
                        {
                            Array.Resize(ref arr_out[i], max_length);
                        }
                    }
                    return arr_out;
                }
                public enum SplitOption
                {
                    NoOption,
                    AddSplitToEnd,
                    AddSplitToBeginning
                }
                /// <summary>
                /// 2023.08.27 15:49. 
                /// </summary>
                /// <param name="array_in"></param>
                /// <param name="delimer"></param>
                /// <returns></returns>
                public static byte[][] ByDelimer(byte[] array_in, byte delimer, SplitOption option = SplitOption.NoOption)
                {
                    if (array_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new byte[0][];
                    }
                    string str_from_byte = Encoding.UTF7.GetString(array_in);
                    char char_split = Encoding.UTF7.GetString(new byte[] { delimer })[0];
                    string[] split_strings = str_from_byte.Split(char_split);
                    byte[][] arr_out = new byte[split_strings.Length][];
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        char[] chars_str = split_strings[i].ToCharArray();
                        if (option == SplitOption.AddSplitToBeginning)
                        {
                            arr_out[i] = new byte[split_strings[i].Length + 1];
                            arr_out[i][0] = delimer;
                            for (Int32 a = 1; a < arr_out[i].Length; a++)
                            {
                                arr_out[i][a] = (byte)chars_str[a];
                            }
                        }
                        if (option == SplitOption.AddSplitToEnd)
                        {
                            arr_out[i] = new byte[split_strings[i].Length + 1];
                            arr_out[i][arr_out[i].Length - 1] = delimer;
                            for (Int32 a = 0; a < arr_out[i].Length - 1; a++)
                            {
                                arr_out[i][a] = (byte)chars_str[a];
                            }
                        }
                        if (option == SplitOption.NoOption)
                        {
                            arr_out[i] = new byte[split_strings[i].Length];
                            for (Int32 a = 0; a < arr_out[i].Length - 1; a++)
                            {
                                arr_out[i][a] = (byte)chars_str[a];
                            }
                        }
                    }
                    return arr_out;
                }
                /// <summary>
                /// 2023.08.27 16:03. written.
                /// </summary>
                /// <param name="array_in"></param>
                /// <param name="delimer"></param>
                /// <returns></returns>
                public static byte[][] ByDelimer(byte[] array_in, byte[] delimer, SplitOption option = SplitOption.NoOption)
                {
                    if (array_in.Length == 0)
                    {
                        ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new byte[0][];
                    }
                    string str_from_byte = Encoding.UTF7.GetString(array_in);
                    string string_split = Encoding.UTF7.GetString(delimer);
                    string[] split_strings = str_from_byte.Split(new string[] { string_split }, StringSplitOptions.None);
                    byte[][] arr_out = new byte[split_strings.Length][];
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        char[] chars_str = split_strings[i].ToCharArray();
                        if (option == SplitOption.AddSplitToBeginning)
                        {
                            arr_out[i] = new byte[split_strings[i].Length + delimer.Length];
                            Array.Copy(delimer, arr_out[i], delimer.Length);
                            for (Int32 a = delimer.Length - 1; a < arr_out[i].Length; a++)
                            {
                                arr_out[i][a] = (byte)chars_str[a];
                            }
                        }
                        if (option == SplitOption.AddSplitToEnd)
                        {
                            arr_out[i] = new byte[split_strings[i].Length + delimer.Length];
                            Array.Copy(delimer, 0, arr_out[i], arr_out[i].Length - delimer.Length - 1 + 1, delimer.Length);
                            for (Int32 a = 0; a < arr_out[i].Length - delimer.Length; a++)
                            {
                                arr_out[i][a] = (byte)chars_str[a];
                            }
                        }
                        if (option == SplitOption.NoOption)
                        {
                            arr_out[i] = new byte[split_strings[i].Length];
                            for (Int32 a = 0; a < arr_out[i].Length; a++)
                            {
                                arr_out[i][a] = (byte)chars_str[a];
                            }
                        }
                    }
                    return arr_out;
                }
                /// <summary>
                /// Split byte[] to byte[n][], where n - required number of columns
                /// 2023.08.23 16:34. written.
                /// 2023.08.23 10:00. not tested
                /// </summary>
                /// <param name="array_in"></param>
                /// <param name="columns_number"></param>
                /// <param name="columns_length_equal"></param>
                /// <returns></returns>
                public static byte[][] A_To_MxN(byte[] array_in, Int32 columns_number, bool columns_length_equal = true)
                {
                    return ArrayFunctions.Split.A_To_MxN(array_in, columns_number, columns_length_equal);
                }
            }
            /// <summary>
            /// Prints in console array where each row has certain bytes printed
            /// </summary>
            /// <param name="byte_arr"></param>
            /// <param name="count_to_show"></param>
            /// <param name="line_size"></param>
            /// <param name="to_base"></param>
            static public void ToConsoleAxB(byte[] byte_arr, bool space_between = false, Int32 line_size = 16, Int32 count_to_show = -1, Int32 to_base = 16)
            {
                Int32 symbol_count = 0;
                Int32 num_to_show = count_to_show;
                if (num_to_show == -1)
                {
                    num_to_show = byte_arr.Length;
                }
                string str_to_console = "";
                str_to_console += "Byte array. size is " + byte_arr.Length.ToString() + "\r\n";
                // 2023.6.20 21:51 implemeted because large array was 10 seconds to show
                for (Int32 sn = 0; sn < byte_arr.Length; sn++)
                {
                    if (sn >= num_to_show)
                    {
                        str_to_console += "\r\n";
                        break;
                    }
                    str_to_console += System.Convert.ToString(byte_arr[sn], to_base).PadLeft(2, '0');
                    if (space_between == true)
                    {
                        str_to_console += " ";
                    }
                    symbol_count++;
                    if (symbol_count >= line_size)
                    {
                        symbol_count = 0;
                        str_to_console += "\r\n";
                    }
                }
                Console.WriteLine(str_to_console);
                Console.WriteLine();
            }
            public static byte[] MergeMultipleArrays(byte[][] bytes_in)
            {
                return MergeMultipleArrays(bytes_in);
            }
            public static class ExtractArray
            {


                /// <summary>
                /// Extracts byte array starting from defined index and with defined length. <br></br>
                /// Written. 2024.01.16 08:33. Warsaw. Workplace. <br></br>
                /// Tested. Works. 2024.01.16 08:36. Warsaw. Workplace.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <param name="index"></param>
                /// <param name="length"></param>
                /// <returns></returns>
                public static byte[] FromIndexByLength(byte[] arr_in, UInt32 index, UInt32 length)
                {

                    return ArrayFunctions.Extract.FromIndexByLength(arr_in, index, length);
                }

                /// <summary>
                /// Extacts byte array of certain length from the start of array <br></br>
                /// Written. 2024.01.15 17:25. Warsaw. Workplace. <br></br>
                /// Tested. Works. 2024.01.15 17:28. Warsaw. Workplace.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <param name="length_arr"></param>
                /// <returns></returns>
                public static byte[] FromStartByLength(byte[] arr_in, UInt32 length_arr)
                {
                    return ArrayFunctions.Extract.FromStartByLength(arr_in, length_arr);
                }



                /// <summary>
                /// Extracts byte[] below defined index. <br></br>
                /// Tested. Works. 2023.12.14 12:21. Workplace.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <param name="index"></param>
                /// <returns></returns>
                public static byte[] BelowIndex(byte[] arr_in, Int32 index)
                {
                    // 2023-07-03 16:17 length to index, then - start_index, then index to length
                    byte[] arr_out = new byte[arr_in.Length - (index + 1)];
                    Array.Copy(arr_in, index + 1, arr_out, 0, arr_out.Length);
                    return arr_out;
                }
                public static byte[] AboveIndex(byte[] arr_in, Int32 index)
                {
                    // 2023-07-03 16:33 index to length and then before index
                    byte[] arr_out = new byte[index + 1 - 1];
                    Array.Copy(arr_in, arr_out, arr_out.Length);
                    return arr_out;
                }
            }
            public static byte[] MergeArrays(byte[] arr_1, byte[] arr_2)
            {
                byte[] arr_out = new byte[arr_1.Length + arr_2.Length];
                Array.Copy(arr_1, arr_out, arr_1.Length);
                Array.Copy(arr_2, 0, arr_out, arr_1.Length, arr_2.Length);
                return arr_out;
            }
            public static Int32 LastIndexOf(byte[] data_in, byte[] pattern_in)
            {
                Int32 patternIndex = IndexOf(data_in, pattern_in);
                if (patternIndex != -1)
                {
                    return patternIndex + pattern_in.Length - 1;
                }
                return -1;
            }
            public static Int32 IndexOf(byte[] data_in, byte[] bytes_to_search)
            {
                Int32 patternIndex = 0;
                for (Int32 dataIndex = 0; dataIndex < data_in.Length; dataIndex++)
                {
                    if (data_in[dataIndex] != bytes_to_search[0]) continue;
                    for (patternIndex = 0; patternIndex < bytes_to_search.Length; patternIndex++)
                    {
                        if (data_in[dataIndex + 1 + patternIndex] != bytes_to_search[patternIndex]) break;
                        if (patternIndex == bytes_to_search.Length - 1) return dataIndex;
                    }
                }
                return -1;
            }
            /// <summary>
            /// 2023.08.29 11:27. written.
            /// 2023.08.29 11:28. tested. works.
            /// </summary>
            /// <param name="arr_in"></param>
            public static void ToConsole(byte[][] arr_in, Int32 base_for_conversion = 16)
            {
                // 2024.02.18 12:10. Warsaw. Hostel.
                // the code may be usefull. it is for string so can be used to make filestring
                // byte byte_test = 200;
                // Console.WriteLine(byte_test.ToString("x2").ToUpper());


                if (arr_in.Length == 0)
                {
                    Console.WriteLine("Attention! Array is empty");
                    return;
                }
                Console.WriteLine("Array " + typeof(byte).Name.ToString() + ". Length is " + arr_in.Length.ToString() + "x" + arr_in[0].Length.ToString());
                byte[] array_all_values = Merge.NxM_To_A(arr_in);
                string min_num = array_all_values.Min().ToString();
                string max_num = array_all_values.Max().ToString();
                Int32 pad_size = max_num.Length;
                if (max_num.Length < min_num.Length)
                {
                    pad_size = min_num.Length;
                }
                string[][] str_arr = Convert.ToStringArray(arr_in, base_for_conversion);
                if (base_for_conversion == 10)
                {
                    str_arr = StringArray.Pad.Right(str_arr, pad_size);
                }
                if (base_for_conversion == 16)
                {
                    pad_size = 2;
                    str_arr = StringArray.Pad.Left(str_arr, pad_size, '0');
                }
                string str_for_console = StringArray.Convert.ToFileString(str_arr, "".PadRight(3, ' '));
                Console.WriteLine(str_for_console);
            }
            /// <summary>
            /// 2023.06.29 10:51. written.
            /// 2023.08.29 10:51. tested. works.
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="in_row"></param>
            public static void ToConsole(byte[] arr_in, bool in_row = false)
            {
                string str_to_console = "";
                str_to_console += "Byte array. size is " + arr_in.Length.ToString() + "\r\n";
                for (Int32 j = 0; j < arr_in.Length; j++)
                {
                    str_to_console += arr_in[j].ToString();
                    if (in_row == false)
                    {
                        str_to_console += "\r\n";
                    }
                    else
                    {
                        str_to_console += " ";
                    }
                }
                Console.WriteLine(str_to_console);
            }
            /// <summary>
            /// 2023.08.23 16:57. started.
            /// </summary>
            public static class Merge
            {
                /// <summary>
                /// Written. 2023.12.13 10:14. Workplace. 
                /// Tested. 2023.12.13 10:30. Workplace.
                /// </summary>
                /// <param name="arr_1"></param>
                /// <param name="arr_2"></param>
                /// <returns></returns>
                public static byte[] A_B_To_C(byte[] arr_1, byte[] arr_2)
                {
                    byte[] arr_out = new byte[arr_1.Length + arr_2.Length];
                    Array.Copy(arr_1, arr_out, arr_1.Length);
                    Array.Copy(arr_2, 0, arr_out, arr_1.Length, arr_2.Length);
                    return arr_out;
                }
                /// <summary>
                /// 2023.08.25 10:45. not tested
                /// </summary>
                /// <param name="array_in"></param>
                /// <returns></returns>
                public static byte[] NxM_To_A(byte[][] array_in)
                {
                    return ArrayFunctions.Merge.NxM_To_A(array_in);
                }
                /// <summary>
                /// Prints byte[][] in console
                /// 2023.08.23 16:48. written.
                /// 2023.08.23 16:48. not tested.
                /// </summary>
                /// <param name="arr_in"></param>
                public static void ToConsole(byte[][] arr_in)
                {
                    if (arr_in.Length == 0)
                    {
                        Console.WriteLine("Attention! Array is empty");
                        return;
                    }
                    Console.WriteLine("Array" + typeof(byte).Name.ToString() + ". Length is " + arr_in.Length.ToString() + "x" + arr_in[0].Length.ToString());
                    byte[] array_all_values = Merge.NxM_To_A(arr_in);
                    string min_num = array_all_values.Min().ToString();
                    string max_num = array_all_values.Max().ToString();
                    Int32 pad_size = max_num.Length;
                    if (max_num.Length > min_num.Length)
                    {
                        pad_size = min_num.Length;
                    }
                    string[][] str_arr = Convert.ToStringArray(arr_in);
                    str_arr = StringArray.Pad.Right(str_arr, pad_size);
                    string str_for_console = StringArray.Convert.ToFileString(str_arr, "".PadRight(3, ' '));
                    Console.WriteLine(str_for_console);
                }
            }
            public static void ArrayZeroing(ref byte[] arr_in)
            {
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    arr_in[i] = 0;
                }
            }
            public static void ArraySetValue(ref byte[] arr_in, byte value_in)
            {
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    arr_in[i] = value_in;
                }
            }
            /// <summary>
            /// Cut Array A on B arrays of equal sizes
            /// </summary>
            /// <param name="array_in"></param>
            /// <param name="num_arr"></param>
            /// <returns></returns>
            public static byte[][] CutOnArrays(byte[] array_in, Int32 num_arr)
            {
                byte[][] arr_out = new byte[0][];
                if (num_arr == 0)
                {
                    ReportFunctions.ReportError("Arrays number is 0. Division by 0");
                    return arr_out;
                }
                Int32 num_arr_out = num_arr;
                Int32 arr_size = array_in.Length / num_arr;
                while (array_in.Length > (num_arr_out * arr_size))
                {
                    arr_size++;
                }
                Int32 arr_1_index = 0;
                Int32 arr_2_index = 0;
                arr_out = new byte[num_arr][];
                arr_out[arr_1_index] = new byte[arr_size];
                ArrayZeroing(ref arr_out[arr_1_index]);
                for (Int32 i = 0; i < array_in.Length; i++)
                {
                    if (arr_2_index >= arr_size)
                    {
                        arr_1_index++;
                        arr_out[arr_1_index] = new byte[arr_size];
                        ArrayZeroing(ref arr_out[arr_1_index]);
                        arr_2_index = 0;
                    }
                    arr_out[arr_1_index][arr_2_index] = array_in[i];
                    arr_2_index++;
                }
                return arr_out;
            }
        }


        /// <summary>
        /// Written. 2024.01.16 13:33. Warsaw. Workplace. 
        /// </summary>
        public static class BoolArray
        {


            /// <summary>
            /// Written. 2024.01.16 13:43. Warsaw. Workplace. 
            /// </summary>
            public static class Elements
            {
                /// <summary>
                /// Sets values of elements of array with certain value. <br></br>
                /// Written. 2024.01.16 14:06. Warsaw. Workplace.
                /// not tested
                /// </summary>
                /// <param name="arr_in"></param>
                /// <param name="value"></param>
                public static void SetValues(ref bool[] arr_in, bool value)
                {
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_in[i] = value;
                    }
                }

            }


            /// <summary>
            /// Creates array of defined size and defined values in the array. <br></br>
            /// Written. 2024.01.16 13:36. Warsaw. Workplace. <br></br>
            /// Tested. Works. 2024.01.16 14:11. Warsaw. Workplace. 
            /// </summary>
            /// <param name="size"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static bool[] CreateArray(UInt32 size, bool value)
            {
                bool[] arr_out = new bool[size];
                for (Int32 i = 0; i <  size; i++) 
                {
                    arr_out[i] = value;                
                }
                return arr_out;
            }


        }

    }
}

