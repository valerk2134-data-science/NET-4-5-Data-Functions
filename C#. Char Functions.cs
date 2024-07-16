﻿using System;




namespace CharFunctionsNamespace
{

    // 2024.03.10 17:36. Warsaw. Hostel.
    // to char data. 9x15 char of consolas 11. measured.


    public static class CharFunctions
    {

        /// <summary>
        /// Written. 2024.02.11 14:14. Warsaw. Hostel.
        /// </summary>
        public static class Generate
        {
            
            /// <summary>
            /// Written. 2024.02.11 14:14. Warsaw. Hostel.
            /// </summary>
            /// <returns></returns>
            public static char[] ASCIICharsArray()
            {
                char[] arr_out = new char[256];
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    arr_out[i] = (char)i;
                }
                return arr_out;
            }

            /// <summary>
            /// Written. 2024.02.11 14:15. Warsaw. Hostel.
            /// </summary>
            /// <returns></returns>
            public static string ASCIICharsString()
            {
                return new string(ASCIICharsArray());   
            }


        }



        /// <summary>
        /// 2023.10.06 13:41. Written. Warsaw. Workplace 
        /// </summary>
        public static class Convert
        {
            public static Int16 ToInt16(char char_in)
            {
                return (Int16)char_in;
            }
            /// <summary>
            /// 2023.10.06 13:45. Written. Warsaw. Workplace  <br></br>
            /// 2023.10.06 13:49. Tested. Works
            /// </summary>
            /// <param name="char_in"></param>
            /// <returns></returns>
            public static string SpecialCharacterToString(char char_in)
            {
                string str_out = "";
                if (char_in == '\t')
                {
                    str_out = "\\t";
                }
                if (char_in == '\n')
                {
                    str_out = "\\n";
                }
                if (char_in == '\r')
                {
                    str_out = "\\r";
                }
                if (char_in == '\0')
                {
                    str_out = "\\0";
                }
                return str_out;
            }
        }
    }
}
