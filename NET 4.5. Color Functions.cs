using System;
using System.Drawing;
using System.Windows.Forms;




// Started. 2023.11.23 12:10. Warsaw. Workplace. 
namespace ColorFunctionsNamespace
{
    /// <summary>
    /// Written. 2023.11.23 12:10. Warsaw. Workplace. 
    /// </summary>
    public static class ColorFunctions
    {

        /// <summary>
        /// Written. 2024.06.10 14:57. Warsaw. Workplace.
        /// </summary>
        public static class ColorList
        {
            /// <summary>
            /// Soft selection. rated: 2/3.
            /// Added. 2024.06.10 14:57. Warsaw. Workplace. <br></br>
            /// </summary>
            public static Color GraySoft = Color.FromArgb(20, 150, 150, 150);

            /// <summary>
            /// Solid selection with transparancy. rated: 2/3.
            /// Added. 2024.06.10 15:01. Warsaw. Workplace.  <br></br>
            /// </summary>
            public static Color GraySolidTransparent = Color.FromArgb(70, 150, 150, 150);

            /// <summary>
            /// Solid selection with transparancy. rated: 2/3.
            /// Added. 2024.06.10 15:06. Warsaw. Workplace. <br></br>
            /// </summary>
            public static Color LightBlueSoft = Color.FromArgb(70, 192, 192, 255);



        }


        /// <summary>
        /// Written. 2024.01.21 16:57. Warsaw. Hostel. 
        /// </summary>
        public static class Convert
        {

            /// <summary>
            /// Written. 2024.01.21 16:57. Warsaw. Hostel.
            /// not tested.
            /// </summary>
            /// <param name="color_in"></param>
            /// <returns></returns>
            public static Int32 ColorToInt32(Color color_in, bool no_alpha = true)
            {
                Int32 num_out = 0;
                if (no_alpha == false)
                { 
                num_out = color_in.ToArgb();
                }
                else
                {
                    num_out = color_in.ToArgb();
                    num_out &= (0 << 24);
                }

                return num_out;

            }


            /// <summary>
            /// Written. 2024.01.21 17:04. Warsaw. Hostel.  
            /// not tested.
            /// </summary>
            /// <param name="color_in"></param>
            /// <returns></returns>
            public static Int32[] ColorsToInt32Array(Color[] colors_in, bool no_alpha = true)
            {
                Int32[] arr_out = new Int32[colors_in.Length];


                for (Int32 i = 0; i < colors_in.Length; i++)
                {
                    arr_out[i] = ColorToInt32(colors_in[i]);

                }

                return arr_out;

            }






        }


        /// <summary>
        /// Written. 2023.11.23 12:11. Warsaw. Workplace. 
        /// </summary>
        public static class Extract
        {

            /// <summary>
            /// Written. 2024.03.07 07:57. Warsaw. Workplace. 
            /// </summary>
            public static class FromArray
            {

                /// <summary>
                /// Written. 2023.11.23 12:13. Warsaw. Workplace. <br></br>
                /// </summary>
                /// <param name="color_in"></param>
                /// <returns></returns>
                public static byte[] Red(Color[] color_in)
                {
                    byte[] arr_out = new byte[color_in.Length];
                    for (Int32 i = 0; i < color_in.Length; i++)
                    {
                        arr_out[i] = color_in[i].R;
                    }
                    return arr_out;
                }
                /// <summary>
                /// Written. 2023.11.23 12:13. Warsaw. Workplace. <br></br>
                /// </summary>
                /// <param name="color_in"></param>
                /// <returns></returns>
                public static byte[] Green(Color[] color_in)
                {
                    byte[] arr_out = new byte[color_in.Length];
                    for (Int32 i = 0; i < color_in.Length; i++)
                    {
                        arr_out[i] = color_in[i].G;
                    }
                    return arr_out;
                }
                /// <summary>
                /// Written. 2023.11.23 12:13. Warsaw. Workplace. <br></br>
                /// </summary>
                /// <param name="color_in"></param>
                /// <returns></returns>
                public static byte[] Blue(Color[] color_in)
                {
                    byte[] arr_out = new byte[color_in.Length];
                    for (Int32 i = 0; i < color_in.Length; i++)
                    {
                        arr_out[i] = color_in[i].B;
                    }
                    return arr_out;
                }
            }
        }
        public static Form FormOutput = null;
        public static PictureBox PictureBoxOutput = null;
        /// <summary>
        /// Written. 2023.11.23 16:59. Warsaw. Workplace.<br></br>
        /// Tested. Works. 2023.11.23 17:10. Warsaw. Workplace. 
        /// </summary>
        /// <param name="color_arr_in"></param>
        public static void ToPictureBox(Color[] color_arr_in)
        {
            FormOutput = new Form();
            FormOutput.AutoSize = false;
            FormOutput.ClientSize = new Size(100, color_arr_in.Length);
            PictureBoxOutput = new PictureBox();
            PictureBoxOutput.Location =
                new Point(FormOutput.ClientRectangle.Location.X + 50, FormOutput.ClientRectangle.Location.Y);
            PictureBoxOutput.ClientSize = new Size(1, color_arr_in.Length);
            Bitmap bitmap_out = new Bitmap(1, color_arr_in.Length);
            for (Int32 i = 0; i < color_arr_in.Length; i++)
            {
                bitmap_out.SetPixel(0, i, color_arr_in[i]);
            }
            PictureBoxOutput.Image = bitmap_out;
            FormOutput.Controls.Add(PictureBoxOutput);
            FormOutput.Show();
        }
        /// <summary>
        /// Written. 2023.11.23 12:17. Warsaw. Workplace. 
        /// </summary>
        public static class Generate
        {
            /// <summary>
            /// Creates Color[] with colors RGB (all equal) from 0 to 255<br></br>
            /// Written. 2023.11.23 12:20. Warsaw. Workplace. <br></br>
            /// Tested. Works. 2023.11.23 17:10. Warsaw. Workplace. 
            /// </summary>
            /// <returns></returns>
            public static Color[] Black_White_Line_0_255()
            {
                Color[] arr_out = new Color[256];
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    arr_out[i] = Color.FromArgb(i, i, i);
                }
                return arr_out;
            }
        }
    }
}
