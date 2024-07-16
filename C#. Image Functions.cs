using ArrayFunctionsNamespace;
using ColorFunctionsNamespace;
using FileFunctionsNamespace;
using MathFunctionsNamespace;
using ReportFunctionsNamespace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace ImageFunctionsNameSpace
{
    public class ImageCompression
    {
        Bitmap _internal_Bitmap = null;
        public ImageCompression() { }
        public ImageCompression(string file_in)
        {
            _internal_Bitmap = new Bitmap(file_in);
            _internal_Pixels_Values = ImageFunctions.BitmapToInt32ArrayAxB(_internal_Bitmap);
            Size.BeforeCompression = _internal_Pixels_Values.Length * _internal_Pixels_Values[0].Length * 4;
            _size_width = _internal_Pixels_Values[0].Length;
            _size_height = _internal_Pixels_Values.Length;
        }
        public void PixelsToConsole()
        {
            ImageFunctions.ColorArrayAxBToConsole(_internal_Pixels_Values);
        }
        public class SizeOfImage
        {
            public SizeOfImage() { }
            public Int32 BeforeCompression = 0;
            public Int32 AfterCompression = 0;
        }
        public SizeOfImage Size = new SizeOfImage();
        Int32[][] _internal_Pixels_Values = null;
        // 2023.7.23 21:42 it should be stored as 16 bit
        Int32 _size_width = 0;
        Int32 _size_height = 0;
        // 2023.7.23 21:43 decompression relies on the information about width and heigth.
        // stage 1. pixel and line size in int
        /// <summary>
        /// .lcpic - line compress picture
        /// 2023-07-24 10:12
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="extension"></param>
        public void CompressLines_v1(string filename, string extension = ".lcpic")
        {
            Int32[] file_bytes = CompressLines_v1();
            FileFunctions.TextFile.Int32ToFile(file_bytes, filename + extension);
        }
        /// <summary>
        /// Not recomended.
        /// v2 is more efficient. it is left as part of development.
        /// Compresses lines and comparing to v2 write line data is the line is finished.
        /// </summary>
        /// <returns></returns>
        Int32[] CompressLines_v1()
        {
            Int32[] color_and_size = new Int32[(Size.BeforeCompression / 4) * 2 + 2];
            Int32 _color_size_index = 2;
            Int32 pixel_color = _internal_Pixels_Values[0][0];
            Int32 size_line = 0;
            color_and_size[0] = _size_height;
            color_and_size[1] = _size_width;
            for (Int32 i = 0; i < _internal_Pixels_Values.Length; i++)
            {
                for (Int32 j = 0; j < _internal_Pixels_Values[0].Length; j++)
                {
                    if (pixel_color == _internal_Pixels_Values[i][j])
                    {
                        size_line++;
                    }
                    else
                    {
                        color_and_size[_color_size_index] = pixel_color;
                        _color_size_index++;
                        color_and_size[_color_size_index] = size_line;
                        _color_size_index++;
                        // 2023.7.23 21:56 index will Point to not used Int32 after for ends.
                        pixel_color = _internal_Pixels_Values[i][j];
                        size_line = 1;
                    }
                    if (j == (_internal_Pixels_Values[i].Length - 1))
                    {
                        // 2023.7.23 22:20 end of line
                        color_and_size[_color_size_index] = pixel_color;
                        _color_size_index++;
                        color_and_size[_color_size_index] = size_line;
                        _color_size_index++;
                        size_line = 0;
                        if (i != (_internal_Pixels_Values.Length - 1))
                        {
                            pixel_color = _internal_Pixels_Values[i + 1][0];
                        }
                    }
                    //   _internal_Pixels_Values[i][j] = -5;
                    //   Console.Clear();
                    //  MyImageMethods.ColorArrayAxBToConsole(_internal_Pixels_Values);
                    //  MyFileFunctions.Int32ArrayToConsole(color_and_size);
                    // Console.ReadKey();
                }
            }
            _color_size_index -= 1;
            Array.Resize(ref color_and_size, _color_size_index + 1);
            Size.AfterCompression = color_and_size.Length * 4;
            return color_and_size;
        }
        /// <summary>
        /// Compresses lines of image
        /// </summary>
        /// <returns></returns>
        public Int32[] CompressLinesMethod_v2()
        {
            Int32[] color_and_size = new Int32[(Size.BeforeCompression / 4) * 2];
            Int32 _color_size_index = 0;
            Int32 pixel_color = _internal_Pixels_Values[0][0];
            Int32 size_line = 0;
            for (Int32 i = 0; i < _internal_Pixels_Values.Length; i++)
            {
                for (Int32 j = 0; j < _internal_Pixels_Values[0].Length; j++)
                {
                    if (pixel_color == _internal_Pixels_Values[i][j])
                    {
                        size_line++;
                    }
                    else
                    {
                        color_and_size[_color_size_index] = pixel_color;
                        _color_size_index++;
                        color_and_size[_color_size_index] = size_line;
                        _color_size_index++;
                        // 2023.7.23 21:56 index will Point to not used Int32 after for ends.
                        pixel_color = _internal_Pixels_Values[i][j];
                        size_line = 1;
                    }
                }
            }
            // 2023.7.23 22:00 end the end there is dot or line in analysis
            color_and_size[_color_size_index] = pixel_color;
            _color_size_index++;
            color_and_size[_color_size_index] = size_line;
            Array.Resize(ref color_and_size, _color_size_index + 1);
            Size.AfterCompression = color_and_size.Length * 4;
            return color_and_size;
        }
        /// <summary>
        /// Compresses lines. Uses v2 and then Int32 for size is replaced by Int16.
        /// Pixel color is stored in two Int16.
        /// </summary>
        /// <returns></returns>
        public Int16[] CompressLinesMethod_v3()
        {
            Int32[] compressed_pixels = CompressLinesMethod_v2();
            Int32 size_in_int16 = 0;
            // 2023.7.23 22:25 step by step
            // 2023.7.23 22:25 1. Int32 pixels
            size_in_int16 = compressed_pixels.Length / 2;
            // 2023.7.23 22:26 2. 2 Int16 for pixel
            size_in_int16 = size_in_int16 * 2;
            // 2023.7.23 22:26 3. size of line
            size_in_int16 += compressed_pixels.Length / 2;
            Int16[] compressed_pixels_int16 = new Int16[size_in_int16];
            Int32 _in_int16_index = 0;
            for (Int32 i = 0; i < compressed_pixels.Length; i += 2)
            {
                compressed_pixels_int16[_in_int16_index] = (Int16)(compressed_pixels[i] >> 16);
                _in_int16_index++;
                compressed_pixels_int16[_in_int16_index] = (Int16)(compressed_pixels[i] >> 0);
                _in_int16_index++;
                compressed_pixels_int16[_in_int16_index] = (Int16)compressed_pixels[i + 1];
                _in_int16_index++;
                // 2023.7.23 22:31 index will Point to not used Int16
                // 2023.7.23 22:32 the size will be defined so there is no need to do anything
            }
            Size.AfterCompression = compressed_pixels_int16.Length * 2;
            return compressed_pixels_int16;
        }
        // 2023.7.23 23:09 to do.
        // file format .lcpic line coding picture.
    }
    public class ImageDecompression
    {
        public Bitmap BitmapDecompressed = null;
        string extension_compress_lines = ".lcpic";
        public ImageDecompression() { }
        public ImageDecompression(string file_in)
        {
            string extension = Path.GetExtension(file_in);
            if (extension != extension_compress_lines)
            {
                ReportFunctions.ReportError("Wrong file. File is " + extension + ". File should be " + extension_compress_lines);
                return;
            }
            _internal_File_Bytes = FileFunctions.TextFile.FileToBytes(file_in);
            Size.BeforeDecompression = _internal_File_Bytes.Length;
            byte[] height_bytes = ArrayFunctions.Extract.AboveIndex(_internal_File_Bytes, 4);
            height_bytes = ArrayFunctions.Extract.BelowIndex(height_bytes, 1);
            byte[] width_bytes = ArrayFunctions.Extract.AboveIndex(_internal_File_Bytes, 8);
            width_bytes = ArrayFunctions.Extract.BelowIndex(_internal_File_Bytes, 5);
            _size_height = MathFunctions.BytesToInt16(height_bytes);
            _size_width = MathFunctions.BytesToInt16(width_bytes);
        }
        byte[] _internal_File_Bytes = null;
        public class SizeOfImage
        {
            public SizeOfImage() { }
            public Int32 BeforeDecompression = 0;
            public Int32 AfterDecompression = 0;
        }
        public SizeOfImage Size = new SizeOfImage();
        Int32[][] _internal_Pixels_Values = null;
        Int32 _size_width = 0;
        Int32 _size_height = 0;
        /// <summary>
        /// Decompresses file ".lcpic" to Bitmap. Bitmap can be used after this fucntion.
        /// 2023-07-24 11:34
        /// </summary>
        public void LinesMethod_Version_1_Decompress_Bitmap()
        {
            BitmapDecompressed = ImageFunctions.Int32ArrayAxBToBitmap(LinesMethod_Version_1_Decompress_Pixels());
        }
        public void BitmapDecompressedToFileBMP(string filename)
        {
            ImageFunctions.BitmapToFileBMP(BitmapDecompressed, filename);
        }
        Int32[][] LinesMethod_Version_1_Decompress_Pixels()
        {
            Int32[][] Pixels = new Int32[_size_height][];
            Int32 PixelColor = 0;
            Int32 LineSize = 0;
            Int32 Pixel_height_index = 0;
            Int32 Pixel_width_index = 0;
            Pixels[0] = new Int32[_size_width];
            for (Int32 i = 8; i < _internal_File_Bytes.Length; i += 8)
            {
                if (i > _internal_File_Bytes.Length - 16)
                {
                    // it was for debug. 2024.02.10 18:52. Warsaw. Hostel.
                    // Int32 b = 0;
                }
                byte[] pixel_color_bytes = ArrayFunctions.Extract.FromIndexToIndex(_internal_File_Bytes, i, i + 3);
                byte[] line_size_bytes = ArrayFunctions.Extract.FromIndexToIndex(_internal_File_Bytes, i + 6, i + 7);
                PixelColor = MathFunctions.Int32Number.BytesToInt32(pixel_color_bytes);
                LineSize = MathFunctions.BytesToInt16(line_size_bytes);
                for (Int32 j = 0; j < LineSize; j++)
                {
                    Pixels[Pixel_height_index][Pixel_width_index] = PixelColor;
                    Pixel_width_index++;
                }
                // 2023-07-24 09:08 after for index points at empty int. it can be out of range
                Pixel_width_index -= 1;
                if (Pixel_width_index >= (_size_width - 1))
                {
                    Pixel_width_index = 0;
                    Pixel_height_index++;
                    if (Pixel_height_index < _size_height)
                    {
                        Pixels[Pixel_height_index] = new Int32[_size_width];
                    }
                }
                else
                {
                    Pixel_width_index++;
                }
            }
            Size.AfterDecompression = _size_height * _size_width * 4;
            return Pixels;
        }
        public Int32[] CompressLinesMethod_v2()
        {
            Int32[] color_and_size = new Int32[(Size.BeforeDecompression / 4) * 2];
            Int32 _color_size_index = 0;
            Int32 pixel_color = _internal_Pixels_Values[0][0];
            Int32 size_line = 0;
            for (Int32 i = 0; i < _internal_Pixels_Values.Length; i++)
            {
                for (Int32 j = 0; j < _internal_Pixels_Values[0].Length; j++)
                {
                    if (pixel_color == _internal_Pixels_Values[i][j])
                    {
                        size_line++;
                    }
                    else
                    {
                        color_and_size[_color_size_index] = pixel_color;
                        _color_size_index++;
                        color_and_size[_color_size_index] = size_line;
                        _color_size_index++;
                        // 2023.7.23 21:56 index will Point to not used Int32 after for ends.
                        pixel_color = _internal_Pixels_Values[i][j];
                        size_line = 1;
                    }
                }
            }
            // 2023.7.23 22:00 end the end there is dot or line in analysis
            color_and_size[_color_size_index] = pixel_color;
            _color_size_index++;
            color_and_size[_color_size_index] = size_line;
            Array.Resize(ref color_and_size, _color_size_index + 1);
            Size.AfterDecompression = color_and_size.Length * 4;
            return color_and_size;
        }
        public Int16[] CompressLinesMethod_v3()
        {
            Int32[] compressed_pixels = CompressLinesMethod_v2();
            Int32 size_in_int16 = 0;
            // 2023.7.23 22:25 step by step
            // 2023.7.23 22:25 1. Int32 pixels
            size_in_int16 = compressed_pixels.Length / 2;
            // 2023.7.23 22:26 2. 2 Int16 for pixel
            size_in_int16 = size_in_int16 * 2;
            // 2023.7.23 22:26 3. size of line
            size_in_int16 += compressed_pixels.Length / 2;
            Int16[] compressed_pixels_int16 = new Int16[size_in_int16];
            Int32 _in_int16_index = 0;
            for (Int32 i = 0; i < compressed_pixels.Length; i += 2)
            {
                compressed_pixels_int16[_in_int16_index] = (Int16)(compressed_pixels[i] >> 16);
                _in_int16_index++;
                compressed_pixels_int16[_in_int16_index] = (Int16)(compressed_pixels[i] >> 0);
                _in_int16_index++;
                compressed_pixels_int16[_in_int16_index] = (Int16)compressed_pixels[i + 1];
                _in_int16_index++;
                // 2023.7.23 22:31 index will Point to not used Int16
                // 2023.7.23 22:32 the size will be defined so there is no need to do anything
            }
            Size.AfterDecompression = compressed_pixels_int16.Length * 2;
            return compressed_pixels_int16;
        }
    }
    public static class ImageFunctions
    {
        // Note made in Warsaw. Workplace. 2024.07.04 14:16. 
        // Importance 3. DrawRectungle, DrawEllipse. Width, Height is not length. It is index of end point.

        static Random _internal_random = new Random();
        // template for code execution. 2024.03.06 16:00. Warsaw. Workplace. 
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
        /// 1. Added. start. 2024.03.05 10:59. Warsaw. Workplace. <br></br>
        /// 2. Modifed. 2024.03.06 15:58. Warsaw. Workplace. 
        /// 3. Tested after 2. Works. 2024.03.06 16:36. Warsaw. Workplace. 
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
        // added. end.
        /// <summary>
        /// Written. 2024.03.05 11:14. Warsaw. Workplace. <br></br>
        /// Tested. Works. 2024.03.05 11:14. Warsaw. Workplace. <br></br>
        /// 
        /// Note. Low time function can be excluded from messages in console. <br></br>
        /// </summary>
        /// <param name="function_name"></param>
        static void TimeExecutionMessage(string function_name, float total_ms_passed)
        {
            _time_execution_count += 1;
            Console.WriteLine(_time_execution_count.ToString() + ". " + DateTime.Now.ToString("HH:mm:ss") + " " + function_name +
                " exectuion time: " + total_ms_passed.ToString("0.000") + " ms");
        }


        /// <summary>
        /// Written. Note made in Warsaw. Workplace. 2024.07.03 11:10. 
        /// </summary>
        public class DotDrawSettings
        {
            
            // Note made in Warsaw. Workplace. 2024.07.04 12:13. 
            // 16 looks ok. not to small and to large.
            
            public int DotSize = 16;
            
            public Color FillColor = Color.MediumSlateBlue;
            public Color BorderColor = Color.Black;
            public uint BorderWidth = 1;
            public DotShapeList DotShape = DotShapeList.Round;
            public enum DotShapeList
            {
                Round,
                Square
            }
            
            /// <summary>
            /// Written. Note made in Warsaw. Workplace. 2024.07.03 11:17. 
            /// Note. internal brush and pen for performance increase.
            /// </summary>
            /// <param name="bitmap_in"></param>
            /// <param name="point_in"></param>
            public void DrawDot(ref Bitmap bitmap_in, Point point_in)
            {
                using (Graphics draw_graphics = Graphics.FromImage(bitmap_in))
                {

                    // fill
                    if (DotShape == DotShapeList.Round)
                    {
                        draw_graphics.FillEllipse(new SolidBrush(FillColor), new Rectangle(new Point(point_in.X - DotSize / 2, point_in.Y - DotSize / 2), new Size(DotSize - 1, DotSize - 1)));
                    }

                    if (DotShape == DotShapeList.Square)
                    {
                        draw_graphics.FillRectangle(new SolidBrush(FillColor), new Rectangle(new Point(point_in.X - DotSize / 2, point_in.Y - DotSize / 2), new Size(DotSize - 1, DotSize - 1)));
                    }

                    // border
                    Pen pen_draw = new Pen(BorderColor, BorderWidth);
                    pen_draw.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                    if (DotShape == DotShapeList.Round)
                    {
                        draw_graphics.DrawEllipse(pen_draw, new Rectangle(new Point(point_in.X - DotSize / 2, point_in.Y - DotSize / 2), new Size(DotSize - 1, DotSize - 1)));
                    }

                    if (DotShape == DotShapeList.Square)
                    {
                        draw_graphics.DrawRectangle(pen_draw, new Rectangle(new Point(point_in.X - DotSize / 2, point_in.Y - DotSize / 2), new Size(DotSize - 1, DotSize - 1)));
                    }

                }
            }
        }

        /// <summary>
        /// Written. 2024.06.12 11:51. Gdansk. Home 
        /// </summary>
        public static class Shape
        {

			/// <summary>
			/// Written. 2024.06.05 12:47. Warsaw. Workplace.
			/// </summary>
			public static class RoundCorner
			{

				// 2024.06.05 17:22. Warsaw. Workplace.
				// 1. y and x axises are not zero length (width). each axis has 1 pixel width and
				// it has to be included somewhere. In the part of circle with y positive and x positive to get symmetry.
				// 2. for rounding corners it is not important. the quality is ok with it.
				// if circle is made from rounded corners there will be line of 2 pixels width at 4 sides of circle.
				// 3. if there is circle is requires the center of each arc should be placed on each other to get symmetry.
				// 4. importance 3. assumption. the symmetry may be important for lower size shape of 20 pixels when not symmetry can be seen.




				/// <summary>
				/// Written. 2024.06.05 12:47. Warsaw. Workplace. <br></br>
				/// Tested. Works. 2024.06.05 12:47. Warsaw. Workplace.
				/// </summary>
				/// <param name="radius"></param>
				/// <param name="steps_per_pixel">
				/// 1. 5 is good value. <br></br>
				/// 2. If there is no satisfied quality then it can be set from 3 to 10. <br></br>
				/// 3. Value 1 and 2 was tested to give not good quality. It can be set if there is need to do that.
				/// </param>
				/// <returns></returns>
				/// <exception cref="ArgumentOutOfRangeException"></exception>
				public static Point[] TopLeftPoints(int x_center, int y_center, int radius, int steps_per_pixel = 5)
				{

					if (radius < 2)
					{
						throw new ArgumentOutOfRangeException("radius of arc", "value should be at least 2");
						//return new Point[0];
					}
					Point[] arc_points = BottomRightPoints(0, 0, radius, steps_per_pixel);
					Point[] arc_points_return = new Point[arc_points.Length];
					for (int i = 0; i < arc_points.Length; i++)
					{
						arc_points_return[i] = new Point(-1 * arc_points[i].X + x_center, -1 * arc_points[i].Y + y_center);
					}
					return arc_points_return;
				}


				/// <summary>
				/// Written. 2024.06.05 12:45. Warsaw. Workplace. <br></br>
				/// Tested. Works. 2024.06.05 12:45. Warsaw. Workplace.
				/// </summary>
				/// <param name="radius"></param>
				/// <param name="steps_per_pixel">
				/// 1. 5 is good value. <br></br>
				/// 2. If there is no satisfied quality then it can be set from 3 to 10. <br></br>
				/// 3. Value 1 and 2 was tested to give not good quality. It can be set if there is need to do that.
				/// </param>
				/// <returns></returns>
				/// <exception cref="ArgumentOutOfRangeException"></exception>
				public static Point[] BottonLeftPoints(int x_center, int y_center, int radius, int steps_per_pixel = 5)
				{

					if (radius < 2)
					{
						throw new ArgumentOutOfRangeException("radius of arc", "value should be at least 2");
						//return new Point[0];
					}
					Point[] arc_points = BottomRightPoints(0, 0, radius, steps_per_pixel);
					Point[] arc_points_return = new Point[arc_points.Length];
					for (int i = 0; i < arc_points.Length; i++)
					{
						arc_points_return[i] = new Point(-1 * arc_points[i].X + x_center, arc_points[i].Y + y_center);
					}
					return arc_points_return;
				}


				/// <summary>
				/// Written. 2024.06.05 12:43. Warsaw. Workplace. <br></br>
				/// Tested. Works. 2024.06.05 12:43. Warsaw. Workplace.
				/// </summary>
				/// <param name="radius"></param>
				/// <param name="steps_per_pixel">
				/// 1. 5 is good value. <br></br>
				/// 2. If there is no satisfied quality then it can be set from 3 to 10. <br></br>
				/// 3. Value 1 and 2 was tested to give not good quality. It can be set if there is need to do that.
				/// </param>
				/// <returns></returns>
				/// <exception cref="ArgumentOutOfRangeException"></exception>
				public static Point[] TopRightPoints(int x_center, int y_center, int radius, int steps_per_pixel = 5)
				{

					if (radius < 2)
					{
						throw new ArgumentOutOfRangeException("radius of arc", "value should be at least 2");
						//return new Point[0];
					}
					Point[] arc_points = BottomRightPoints(0, 0, radius, steps_per_pixel);
					Point[] arc_points_return = new Point[arc_points.Length];
					for (int i = 0; i < arc_points.Length; i++)
					{
						arc_points_return[i] = new Point(arc_points[i].X + x_center, -1 * arc_points[i].Y + y_center);
					}
					return arc_points_return;
				}

				/// <summary>
				/// Written. 2024.06.05 12:19. Warsaw. Workplace. 
				/// </summary>
				/// <param name="radius"></param>
				/// <param name="steps_per_pixel">
				/// 1. 5 is good value. <br></br>
				/// 2. If there is no satisfied quality then it can be set from 3 to 10. <br></br>
				/// 3. Value 1 and 2 was tested to give not good quality. It can be set if there is need to do that.
				/// </param>
				/// <returns></returns>
				/// <exception cref="ArgumentOutOfRangeException"></exception>
				public static Point[] BottomRightPoints(int x_center, int y_center, int radius, int steps_per_pixel = 5)
				{
					// 2024.06.05 11:01. Warsaw. Workplace.
					// 0 radius is not accepted - there is no arc.
					// 1 radius is not good because line requires 2 points.
					if (radius < 2)
					{
						throw new ArgumentOutOfRangeException("radius of arc", "value should be at least 2");
						//return new Point[0];
					}


					// 2024.06.05 09:28. Shape is circle but with noise/not smooth.
					/*
					double r_circle = 50;
					Point[] points2 = new Point[200];          
					for (int i = 0; i < points2.Length; i++)
					{
						double x = ((double)r_circle / (double)points2.Length) * (double)i;
						double y = Math.Sqrt(Math.Pow(r_circle, 2) - Math.Pow(x, 2));             
						points2[i] = new Point((int)x + 50, (int)y + 50);
					}
					*/


					// 2024.06.05 09:57. This provides good smooth arc
					// 2024.06.05 09:58. It can be put in use.
					//
					// Note. Arc is formed by horizontal lines while there is no coding for that was.
					// Answer. It is the line drawn that way. length 7-8 at x and 1 at y

					// 2024.06.05 10:12. Warsaw. Workplace.
					// Points for other arcs can be found by multiplying y by -1, x by -1.


					// 
					int r_arc = radius;
					Point[] arc_points = new Point[r_arc * steps_per_pixel];

					int y_last = 0;
					int fill_count = 0;
					for (int i = 0; i < arc_points.Length; i++)
					{
						double x = ((double)r_arc / (double)arc_points.Length) * (double)i;
						double y = System.Math.Round(System.Math.Sqrt(System.Math.Pow(r_arc, 2) - System.Math.Pow(x, 2))) + (double)0.1;
						if (i == 0)
						{
							y_last = (int)y;
							fill_count += 1;
							arc_points[fill_count - 1] = new Point((int)(System.Math.Round(x) + (double)0.1) + x_center, (int)y + y_center);

						}
						else
						{
							if (y_last == (int)y)
							{
								continue;
							}
							else
							{
								fill_count += 1;
								arc_points[fill_count - 1] = new Point((int)(System.Math.Round(x) + (double)0.1) + x_center, (int)y + y_center);
								y_last = (int)y;
							}
						}

					}
					if (arc_points[fill_count - 1].Y != 0)
					{
						Array.Resize(ref arc_points, fill_count + 1);
						arc_points[arc_points.Length - 1] = new Point(r_arc + x_center, 0 + y_center);
					}
					else
					{
						Array.Resize(ref arc_points, fill_count);
					}




					// 2024.06.05 10:49. Warsaw. Workplace.
					// There were different length between line from arc points and line with points set manually.
					// This was because 2nd line from arc point continued 1st line and it looked like the line drawn from arc points
					// was longer than line from points that are set manually.

					/*
					g.DrawLines(Pens.Black, arc_points);
					g.DrawLines(Pens.Blue, points);
					*/

					// 2024.06.05 10:15. Warsaw. Workplace. 
					// Last point may not be needed because the previous point will be connected to straight line with the same x coordinate.
					// Point last_p1 = new Point(arc_points[arc_points.Length - 1].X, arc_points[arc_points.Length - 1].Y);

					// 2024.06.05 10:14. Warsaw. Workplace. 
					// That part to find points for other arc.
					/*            
					points2 = new Point[1000];
					x_previous = 0;
					y_last = 0;
					fill_count = 0;
					for (int i = 0; i < points2.Length; i++)
					{
						double x = ((double)r_circle / (double)points2.Length) * (double)i;
						double y = Math.Round(-Math.Sqrt(Math.Pow(r_circle, 2) - Math.Pow(x, 2))) + (double)0.01;
						if (i == 0)
						{                   
							y_last = (int)y;
							fill_count += 1;
							points2[fill_count - 1] = new Point((int)(Math.Round(x) + (double)0.01) + 100, (int)y + 100);

						}
						else
						{
							if (y_last == (int)y)
							{
								continue;
							}
							else
							{
								fill_count += 1;
								points2[fill_count - 1] = new Point((int)(Math.Round(x) + (double)0.01) + 100, (int)y + 100);
								y_last = (int)y;
							}
						}

					}
					Array.Resize(ref points2, fill_count);
					g.DrawLines(Pens.Black, points2);
					g.DrawLine(Pens.Black, points2[points2.Length - 1], last_p1);
					*/

					return arc_points;
				}
			}


			/// <summary>
			/// 
			/// Written. 2024.05.28 16:25. Warsaw.
			/// </summary>
			public static class Hexagon
			{
				public static Point[] Points(int height_of_hexagon)
				{
					Point[] points_arr_out = new Point[height_of_hexagon];
					points_arr_out[0] = new Point(0, height_of_hexagon / 2);
					points_arr_out[1] = new Point((int)(((float)height_of_hexagon / (float)2) / ((float)System.Math.Sqrt(3))), 0);
					points_arr_out[2] = new Point((int)((float)height_of_hexagon - (float)2 * (((float)height_of_hexagon / (float)2) / ((float)System.Math.Sqrt(3)))), 0);
					points_arr_out[3] = new Point(height_of_hexagon, height_of_hexagon / 2);
					points_arr_out[4] = new Point((int)(((float)height_of_hexagon / (float)2) / ((float)System.Math.Sqrt(3))), height_of_hexagon);
					points_arr_out[5] = new Point((int)((float)height_of_hexagon - (float)2 * (((float)height_of_hexagon / (float)2) / ((float)System.Math.Sqrt(3)))), height_of_hexagon);
					return points_arr_out;
				}
			}






		}



		/// <summary>
		/// Written. Warsaw. In train to workplace. 2024.04.22 06:22. 
		/// </summary>
		public static class Math
        {
            
            
            
            /// <summary>
            /// Written. Note made in Warsaw. Hostel. 2024.04.22 06:23. 
            /// </summary>
            public static class Line
            {
                /// <summary>
                /// Written. Note made in Warsaw. Hostel. 2024.04.22 06:28. 
                /// </summary>
                /// <param name="line_in"></param>
                /// <param name="total_points"></param>
                /// <returns></returns>
                public static Point[] GetAdditionalPoints(Point[] line_in, int total_points)
                {
                    int x_start = line_in[0].X;
                    int x_end = line_in[1].X;
                    int y_start = line_in[0].Y;
                    int y_end = line_in[1].Y;

                    float m_of_line = (y_end - y_start)/(x_end - x_start);
                    Point[] arr_out = new Point[total_points];
                    for (int i = 0; i < arr_out.Length; i++)
                    {
                        int x_value = ((x_end - x_start) / total_points)*i;
                        arr_out[i] = new Point(x_value, (int)(m_of_line * (float)x_value));
                    }
                    return arr_out;
                }
            }


        }








        /// <summary>
        /// Written. 2024.03.06 17:11. Warsaw. Workplace. 
        /// </summary>
        public static class AverageColor
        {
            /// <summary>
            /// Written. 2024.03.06 17:12. Warsaw. Workplace. <br></br>
            /// Tested. Works. 2024.03.07 12:45. Warsaw. Workplace. <br></br>
            /// <br></br>
            /// 2024.03.07 12:48. Warsaw. Workplace. <br></br> 
            /// Note. black and white level is obtained from black and white Bitmap <br></br>
            /// that is made in this function during the work.<br></br>
            /// <br></br>
            /// 2024.03.07 12:48. Warsaw. Workplace. <br></br>
            /// Execution time can be found in Convert.ToBlackWhiteBitmap <br></br>
            /// which is 15-20 ms for 100x100 and 2-3 s for 1600x900
            /// </summary>
            /// <param name="bitmap_in"></param>
            /// <returns></returns>
            public static Int32 BlackWhite(Bitmap bitmap_in)
            {
                // 2024.03.06 17:32. Warsaw. Workplace. 
                // during writing the code.
                // Averaging from certain color moves the color to white but via non-gray color. 
                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }
                // 2024.03.07 12:31. Warsaw. Workplace. 
                // red green blue are the same for a pixel for black and white image
                // averaging red will give the same as averaging blue, green
                Bitmap bitmap_white_black = Convert.ToBlackWhiteBitmap(bitmap_in);
                Color[] color_arr = Convert.BitmapToColorArray(bitmap_white_black);
                byte[] red_bytes = ColorFunctions.Extract.FromArray.Red(color_arr);
                byte[] green_bytes = ColorFunctions.Extract.FromArray.Green(color_arr);
                byte[] blue_bytes = ColorFunctions.Extract.FromArray.Blue(color_arr);
                byte red_average = ArrayFunctions.ByteArray.Average(red_bytes);
                byte green_average = ArrayFunctions.ByteArray.Average(green_bytes);
                byte blue_average = ArrayFunctions.ByteArray.Average(blue_bytes);
                // 2024.03.07 08:04. Warsaw. Workplace. 
                // note. negative int32 can not be casted to uint.
                UInt32 UInt32_for_out = (uint)0xFF << 24;
                UInt32_for_out |= ((uint)red_average << 16);
                UInt32_for_out |= ((uint)green_average << 8);
                UInt32_for_out |= ((uint)blue_average << 0);
                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(AverageColor.BlackWhite), execution_time_ms_stop - execution_time_ms_start);
                }
                return (int)UInt32_for_out;
            }
        }
        /// <summary>
        /// Written. 2024.02.09 10:01. Warsaw. Workplace.
        /// </summary>
        public static class Generate
        {
            /// <summary>
            /// Written. 2024.02.25 19:10. Warsaw. Hostel.
            /// </summary>
            public static class PointsOutput
            {
                /// <summary>
                /// Generates random points and return Point[] of defined size. <br></br>
                /// Written. 2024.02.25 19:13. Warsaw. Hostel. <br></br>
                /// Tested. Works. 2024.02.25 21:38. Warsaw. Hostel.
                /// </summary>
                /// <param name="points_count"></param>
                /// <param name="x_start"></param>
                /// <param name="x_end"></param>
                /// <param name="y_start"></param>
                /// <param name="y_end"></param>
                /// <returns></returns>
                public static Point[] Random(Int32 points_count = 10, UInt32 x_start = 0, UInt32 x_end = 200, UInt32 y_start = 0, UInt32 y_end = 200)
                {
                    Point[] points_out = new Point[points_count];
                    for (Int32 i = 0; i < points_out.Length; i++)
                    {
                        points_out[i] = new Point(_internal_random.Next((int)x_start, (int)x_end + 1), _internal_random.Next((int)y_start, (int)y_end + 1));
                    }
                    return points_out;
                }
            }
            // 2024.03.06 17:22. Warsaw. Workplace. 
            // If there is different than Bitmap output then it is placed is seperate class
            //public static class BitmapOutput
            //{
            /// <summary>
            /// Generates Bitmap with random dots of certain size of certain color.
            /// Written. 2024.02.25 21:45. Warsaw. Hostel. <br></br>
            /// Tested. Works. 2024.02.25 21:51. Warsaw. Hostel.
            /// </summary>
            /// <param name="points_arr_in"></param>
            /// <param name="point_width"></param>
            /// <param name="point_color"></param>
            /// <param name="form_call_from"></param>
            /// <returns></returns>
            public static Bitmap Random_Dots_Certain_Color(Color point_color, Int32 points_count = 10, UInt32 point_width = 1, UInt32 x_start = 0, UInt32 x_end = 200, UInt32 y_start = 0, UInt32 y_end = 200)
            {
                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }
                if (point_width == 0)
                {
                    ReportFunctions.ReportError("Point radius error");
                    Bitmap bitmap_error = Generate.Rectungular_Checkboard(200, 200);
                    return bitmap_error;
                }
                Point[] points_arr = Generate.PointsOutput.Random(points_count, x_start, x_end, y_start, y_end);
                Int32 max_x = Extract.FromPoints.XValues(points_arr).Max();
                Int32 max_y = Extract.FromPoints.YValues(points_arr).Max();
                Bitmap bitmap_out = new Bitmap(max_x, max_y);
                Graphics draw_bitmap = Graphics.FromImage(bitmap_out);
                Pen pen_draw = new Pen(point_color, point_width);
                for (Int32 i = 0; i < points_arr.Length; i++)
                {
                    draw_bitmap.DrawArc(pen_draw, new Rectangle(points_arr[i], new Size((int)point_width, (int)point_width)), 0, 360);
                }
                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(Random_Dots_Certain_Color), execution_time_ms_stop - execution_time_ms_start);
                }
                return bitmap_out;
            }
            /// <summary>
            /// Creates Bitmap with the color of each pixel white-gray-black. <br></br>
            /// Written. 2024.02.09 13:11. Warsaw. Workplace. <br></br>
            /// Tested. Works. 2024.02.09 13:14. Warsaw. Workplace. 
            /// </summary>
            /// <param name="side_a"></param>
            /// <param name="side_b"></param>
            /// <returns></returns>
            public static Bitmap Rectungular_Random_0_255(Int32 side_a, Int32 side_b)
            {
                Bitmap bitmap_out = new Bitmap(side_a, side_b);
                for (Int32 j = 0; j < side_b; j++)
                {
                    for (Int32 i = 0; i < side_a; i++)
                    {
                        byte byte_for_color = (byte)_internal_random.Next(0, byte.MaxValue + 1);
                        bitmap_out.SetPixel(i, j, Color.FromArgb(255, byte_for_color, byte_for_color, byte_for_color));
                    }
                }
                return bitmap_out;
            }
            /// <summary>
            /// Written. 2024.03.06 17:18. Warsaw. Workplace. 
            /// </summary>
            /// <param name="color_in"></param>
            /// <param name="side_length"></param>
            /// <returns></returns>
            public static Bitmap Square_Certain_Color(Color color_in, Int32 side_length = 100)
            {
                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }
                Bitmap bitmap_out = new Bitmap(side_length, side_length);
                // 2024.03.06 17:18. Warsaw. Workplace. 
                // There is fill tool in Graphics.
                for (Int32 j = 0; j < side_length; j++)
                {
                    for (Int32 i = 0; i < side_length; i++)
                    {
                        bitmap_out.SetPixel(i, j, color_in);
                    }
                }
                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(Generate.Square_Certain_Color), execution_time_ms_stop - execution_time_ms_start);
                }
                return bitmap_out;
            }
            /// <summary>
            /// Creates Bitmap with random color of each pixel. <br></br>
            /// Written. 2024.02.09 13:02. Warsaw. Workplace. <br></br>
            /// Tested. Works. 2024.02.09 13:08. Warsaw. Workplace. 
            /// </summary>
            /// <param name="side_a"></param>
            /// <param name="side_b"></param>
            /// <returns></returns>
            public static Bitmap Rectungular_Random_Color(Int32 side_a, Int32 side_b)
            {
                Bitmap bitmap_out = new Bitmap(side_a, side_b);
                for (Int32 j = 0; j < side_b; j++)
                {
                    for (Int32 i = 0; i < side_a; i++)
                    {
                        bitmap_out.SetPixel(i, j, Color.FromArgb(255, _internal_random.Next(0, byte.MaxValue + 1),
                            _internal_random.Next(0, byte.MaxValue + 1), _internal_random.Next(0, byte.MaxValue + 1)));
                    }
                }
                return bitmap_out;
            }
            /// <summary>
            /// Creates Bitmap with black and white dots of checkboard pattern.
            /// Written. 2024.02.09 10:01. Warsaw. Workplace. <br></br>
            /// Tested. Works. 2024.02.09 10:29. Warsaw. Workplace. 
            /// </summary>
            /// <param name="side_a"></param>
            /// <param name="side_b"></param>
            /// <returns></returns>
            public static Bitmap Rectungular_Checkboard(Int32 side_a, Int32 side_b)
            {
                Bitmap bitmap_out = new Bitmap(side_a, side_b);
                bool color_white = false;
                for (Int32 j = 0; j < side_b; j++)
                {
                    if (j > 0)
                    {
                        if (color_white == false)
                        {
                            if (bitmap_out.GetPixel(0, j - 1).G == 0)
                            {
                                color_white = true;
                            }
                        }
                        else
                        {
                            if (bitmap_out.GetPixel(0, j - 1).G == 255)
                            {
                                color_white = false;
                            }
                        }
                    }
                    for (Int32 i = 0; i < side_a; i++)
                    {
                        if (color_white == true)
                        {
                            bitmap_out.SetPixel(i, j, Color.FromArgb(255, 255, 255, 255));
                            color_white = false;
                        }
                        else
                        {
                            bitmap_out.SetPixel(i, j, Color.FromArgb(255, 0, 0, 0));
                            color_white = true;
                        }
                    }
                }
                return bitmap_out;
            }
            /// <summary>
            /// Creates Bitmap with line - colors RGB (all equal) from 0 to 255<br></br>
            /// Written. 2024.02.09 10:38. Warsaw. Workplace.  <br></br>
            /// Tested. Works. 2024.02.09 10:38. Warsaw. Workplace. 
            /// </summary>
            /// <returns></returns>
            public static Bitmap Line_0_255()
            {
                Bitmap bitmap_out = new Bitmap(1, 256);
                for (Int32 i = 0; i < 256; i++)
                {
                    bitmap_out.SetPixel(0, i, Color.FromArgb(i, i, i));
                }
                return bitmap_out;
            }
            /// <summary>
            /// Written. 2024.02.10 12:58. Warsaw. Hostel 
            /// </summary>
            /// <param name="circle_radius"></param>
            /// <param name="line_width"></param>
            /// <param name="outside_space"></param>
            /// <returns></returns>
            public static Bitmap CircleWithOutsideSpace(UInt32 circle_radius, UInt32 line_width, UInt32 outside_space, Color background_color)
            {
                Bitmap bitmap_out = new Bitmap((int)circle_radius * 2 + (int)outside_space * 2, (int)circle_radius * 2 + (int)outside_space * 2);
                using (Graphics graphics_draw = Graphics.FromImage(bitmap_out))
                {
                    Brush brush_draw = new SolidBrush(background_color);
                    graphics_draw.FillRectangle(brush_draw, 0, 0, ((int)circle_radius + (int)outside_space) * 2, ((int)circle_radius + (int)outside_space) * 2);
                    Pen pen_draw = new Pen(Color.Black, line_width);
                    graphics_draw.DrawArc(pen_draw, (int)outside_space, (int)outside_space,
                        (int)circle_radius * 2, (int)circle_radius * 2, 0, 360);
                }
                return bitmap_out;
            }
        }
        /// <summary>
        /// Written. 2024.03.06 16:03. Warsaw. Workplace. 
        /// </summary>
        public static class ToFiles
        {
            /// <summary>
            /// Written. 2024.03.06 16:03. Warsaw. Workplace. 
            /// Tested. Works. 2024.03.06 16:34. Warsaw. Workplace. 
            /// </summary>
            /// <param name="bitmap_arr_in"></param>
            /// <param name="filename_base"></param>
            /// <param name="start_number"></param>
            public static void BitmapArrayToFilesBMP(Bitmap[] bitmap_arr_in, string filename_base, Int32 start_number = 0)
            {
                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }
                for (Int32 i = 0; i < bitmap_arr_in.Length; i++)
                {
                    ToFile.ToBMP(bitmap_arr_in[i], filename_base + (start_number + i).ToString() + ".bmp");
                }
                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(ToFiles.BitmapArrayToFilesBMP), execution_time_ms_stop - execution_time_ms_start);
                }
            }
        }
        /// <summary>
        /// Written. 2024.03.06 15:34. Warsaw. Workplace. 
        /// </summary>
        public static class ToFile
        {
            /// <summary>
            /// Written. 2024.03.06 15:34. Warsaw. Workplace. 
            /// Tested. Works. 2024.03.06 16:34. Warsaw. Workplace. 
            /// </summary>
            /// <param name="bitmap_in"></param>
            /// <param name="filename"></param>
            public static void ToBMP(Bitmap bitmap_in, string filename)
            {
                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }
                FileFunctions.ImageFile.WriteFile.BitmapToFileBMP(bitmap_in, filename);
                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(ToFile.ToBMP), execution_time_ms_stop - execution_time_ms_start);
                }
            }
        }
        /// <summary>
        /// Written. 2024.04.19 15:50. Warsaw. Workplace.
        /// </summary>
        public static class ToPointsOfBitmap
        {
            /// <summary>
            /// Written. Warsaw. Hostel. 2024.04.21 12:36. <br></br>
            /// Tested. Works. Warsaw. Hostel. 2024.04.21 12:38. 
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="image_heigh"></param>
            /// <param name="step_in_pixels"></param>
            /// <param name="max_width"></param>
            /// <returns></returns>
            public static Point[] Y_Scaled_X_PixelStep(Int32[] arr_in, int image_heigh, int step_in_pixels = 1, int max_width = 640)
            {
                if (arr_in.Length > max_width)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayMaxLength);
                    return new Point[0];
                }
                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }
                int min_number = arr_in.Min();
                if (min_number >= 0)
                {
                    min_number = 0;
                }
                int max_number = arr_in.Max();
                float scale_number = (float)(max_number - min_number) / (float)image_heigh;
                Int32[] arr_scaled = ArrayFunctions.Int32Array.Math.Multiply(arr_in, (float)1 / scale_number);
                Point[] point_arr_out = new Point[arr_scaled.Length];
                for (int i = 0; i < point_arr_out.Length; i++)
                {
                    point_arr_out[i] = new Point(i * step_in_pixels, arr_scaled[i]);
                }
                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(ToPointsOfBitmap.Y_Scaled_X_PixelStep), execution_time_ms_stop - execution_time_ms_start);
                }
                return point_arr_out;
            }
            /// <summary>
            /// Written. 2024.04.19 10:00 - 15:00. Warsaw. Workplace. <br></br>
            /// Tested. Works. Warsaw. Hostel. 2024.04.21 12:33. 
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="image_heigh"></param>
            /// <param name="max_width"></param>
            /// <returns></returns>
            public static Point[] Y_Scaled_X_OnePixelStep(Int32[] arr_in, int image_heigh, int max_width = 640)
            {
                if (arr_in.Length > max_width)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayMaxLength);
                    return new Point[0];
                }
                return Y_Scaled_X_PixelStep(arr_in, image_heigh, 1, max_width);
                // Warsaw. Hostel. 2024.04.21 12:41. 
                // The code below was used to write Y_Scaled_X_PixelStep and this function
                // is called to get result for Y_Scaled_X_OnePixelStep.
                /*
                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }
                int min_number = arr_in.Min();
                if (min_number >= 0)
                {
                    min_number = 0;
                }
                int max_number = arr_in.Max();
                float scale_number = (float)(max_number - min_number)/(float)image_heigh;
                Int32[] arr_scaled = ArrayFunctions.Int32Array.Math.Multiply(arr_in, (float)1/scale_number);
                Point[] point_arr_out = new Point[arr_scaled.Length];
                for (int i = 0; i < point_arr_out.Length; i++)
                {
                    point_arr_out[i] = new Point(i, arr_scaled[i]);
                }
                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(ToPointsOfBitmap.Y_Scaled_X_OnePixelStep), execution_time_ms_stop - execution_time_ms_start);
                }
                */
            }
        }
        /// <summary>
        /// Written. 2024.02.25 22:05. Warsaw. Hostel.
        /// </summary>
        public static class ToBitmap
        {
            /// <summary>
            /// Creates black and white Bitmap from Int32[][].
            /// </summary>
            /// <param name="array_in"></param>
            /// <returns></returns>
            public static Bitmap Int32ArrayBlackWhite(Int32[][] array_in)
            {
                Bitmap bitmap_return = new Bitmap(array_in.Length, array_in[0].Length);
                Int32 max_value = Int32ArrayFunctions.Find.Max(array_in);
                for (int c = 0; c < array_in.Length; c++)
                {
                    for (int r = 0; r < array_in[0].Length; r++)
                    {
                        byte byte_for_color = (byte)(((float)(array_in[c][r])/(float)max_value)*(float)byte.MaxValue);
                        bitmap_return.SetPixel(r, c, Color.FromArgb(byte_for_color, byte_for_color, byte_for_color));
                    }
                }
                return bitmap_return;
                // Written. Warsaw. Workplace. 2024-07-16 15-07. 
                // Tested. Works. Warsaw. Workplace. 2024-07-16 15-21. 
            }


            /// <summary>
            /// Creates Bitmap with lines and dots at the start and end of each line. <br></br>
            /// Written. Warsaw. Hostel. 2024.04.20 14:59. <br></br>
            /// Tested. Works. Warsaw. Hostel. 2024.04.21 12:25. <br></br>
            /// </summary>
            /// <param name="points_arr_in"></param>
            /// <param name="line_color"></param>
            /// <param name="point_color"></param>
            /// <param name="line_width"></param>
            /// <param name="point_diameter"></param>
            /// <returns></returns>
            public static Bitmap LineArrayWithPoints(Point[] points_arr_in, Color line_color, Color point_color,
                UInt32 line_width = 1, UInt32 point_diameter = 2)
            {
                if (point_diameter < 2)
                {
                    ReportFunctions.ReportError("Point diameter error. Min width and height is 2. Rectangle 1x1 is not drawn");
                    Bitmap bitmap_error = Generate.Rectungular_Checkboard(200, 200);
                    return bitmap_error;
                }
                if (line_width == 0)
                {
                    ReportFunctions.ReportError("Line width error");
                    Bitmap bitmap_error = Generate.Rectungular_Checkboard(200, 200);
                    return bitmap_error;
                }
                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }
                Int32 max_x = Extract.FromPoints.XValues(points_arr_in).Max();
                Int32 max_y = Extract.FromPoints.YValues(points_arr_in).Max();
                Bitmap bitmap_out = new Bitmap(max_x, max_y);
                Graphics draw_bitmap = Graphics.FromImage(bitmap_out);
                Pen pen_draw = new Pen(line_color, line_width);
                Brush brush_draw = new SolidBrush(point_color);
                for (Int32 i = 1; i < points_arr_in.Length; i++)
                {
                    draw_bitmap.DrawLine(pen_draw, points_arr_in[i - 1], points_arr_in[i]);
                    Rectangle rect_of_point = new Rectangle(new Point(points_arr_in[i].X - (int)(point_diameter / 2), points_arr_in[i].Y - (int)(point_diameter / 2)),
                        new Size((int)point_diameter, (int)point_diameter));
                    draw_bitmap.FillEllipse(brush_draw, rect_of_point);
                }
                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(ToBitmap.LineArrayWithPoints), execution_time_ms_stop - execution_time_ms_start);
                }
                return bitmap_out;
            }


            public static Bitmap XYValues(int[] y_values, int[] x_values, DotDrawSettings dot_settings, UInt32 pixels_per_x_value = 8, UInt32 pixels_per_y_value = 8)
            {
                if (y_values.Length != x_values.Length)
                {
                    ReportFunctions.ReportError(ReportFunctions.ErrorMessage.LengthDifferent);
                    Bitmap bitmap_error = Generate.Rectungular_Checkboard(200, 200);
                    return bitmap_error;
                }

                if (dot_settings.DotSize < 2)
                {
                    ReportFunctions.ReportError("Point diameter error. Min width and height is 2. Rectangle 1x1 is not drawn");
                    Bitmap bitmap_error = Generate.Rectungular_Checkboard(200, 200);
                    return bitmap_error;
                }

                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }


                Point[] points_arr = new Point[y_values.Length];

                
                for (int i = 0; i < y_values.Length; i++)
                {
                    points_arr[i] = new Point(x_values[i], y_values[i]);
                }
                Bitmap bitmap_out = ToBitmap.PointArray(points_arr, dot_settings, pixels_per_x_value, pixels_per_y_value);
                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(ToBitmap.XYValues), execution_time_ms_stop - execution_time_ms_start);
                }



                return bitmap_out;

            }



            /// <summary>
            /// Written. Note made in Warsaw. Workplace. 2024.07.02 13:21. 
            /// </summary>
            /// <param name="y_values"></param>
            /// <param name="x_values"></param>
            /// <param name="point_color"></param>
            /// <param name="point_diameter"></param>
            /// <returns></returns>
            public static Bitmap XYValues(int[] y_values, int[] x_values, Color point_color, UInt32 point_diameter = 2, UInt32 pixels_per_x_value = 1)
            {

                if (y_values.Length != x_values.Length)
                {
                    ReportFunctions.ReportError(ReportFunctions.ErrorMessage.LengthDifferent);
                    Bitmap bitmap_error = Generate.Rectungular_Checkboard(200, 200);
                    return bitmap_error;
                }

                if (point_diameter < 2)
                {
                    ReportFunctions.ReportError("Point diameter error. Min width and height is 2. Rectangle 1x1 is not drawn");
                    Bitmap bitmap_error = Generate.Rectungular_Checkboard(200, 200);
                    return bitmap_error;
                }

                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }

                
                Point[] points_arr = new Point[y_values.Length];

                int y_max = y_values.Max();
               
                for (int i = 0; i < y_values.Length; i++)
                {
                    points_arr[i] = new Point(x_values[i], y_max - y_values[i]);
                }
                Bitmap bitmap_out = ToBitmap.PointArray(points_arr, point_color, point_diameter, pixels_per_x_value);
                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(ToBitmap.PointArray), execution_time_ms_stop - execution_time_ms_start);
                }



                return bitmap_out;

            }



            /// <summary>
            /// Creates Bitmap with point of certain color and certain radius
            /// Written. Warsaw. Hostel. 2024.04.20 14:47. <br></br>
            /// Tested. Works. Warsaw. Hostel. 2024.04.20 14:52. 
            /// needs fixing. the point are from 0.
            /// note. change point location to the middle
            /// </summary>
            /// <param name="points_arr_in"></param>
            /// <param name="point_color"></param>
            /// <param name="point_diameter"></param>
            /// <returns></returns>
            public static Bitmap PointArray(Point[] points_arr_in, Color point_color, UInt32 point_diameter = 2, UInt32 pixels_per_x_point = 4, UInt32 pixels_per_y_point = 4)
            {
                if (point_diameter < 2)
                {
                    ReportFunctions.ReportError("Point diameter error. Min width and height is 2. Rectangle 1x1 is not drawn");
                    Bitmap bitmap_error = Generate.Rectungular_Checkboard(200, 200);
                    return bitmap_error;
                }
                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }
                int[] x_values = Extract.FromPoints.XValues(points_arr_in);
                int[] y_values = Extract.FromPoints.YValues(points_arr_in);
                Int32 max_x = x_values.Max();
                Int32 max_y = y_values.Max();

                Int32 min_x = x_values.Min();
                Int32 min_y = y_values.Min();


                
                
                Bitmap bitmap_out = new Bitmap((max_x - min_x)*(int)pixels_per_x_point + (int)point_diameter, (max_y - min_y)*(int)pixels_per_y_point + (int)point_diameter);
                Graphics draw_bitmap = Graphics.FromImage(bitmap_out);
                Brush brush_draw = new SolidBrush(point_color);
                // Note made in Warsaw. Workplace. 2024.07.02 16:50. 
                // from 1 because there is line and there is need of point[0] and point[1] for 1st line
                // so array starts with 1 to have it.
                for (Int32 i = 1; i < points_arr_in.Length; i++)
                {
                    Rectangle rect_of_point = new Rectangle(new Point((points_arr_in[i].X - (int)point_diameter / 2 - min_x)*(int)pixels_per_x_point, 
                        points_arr_in[i].Y - (int)point_diameter / 2 - min_y + (int)point_diameter/2), new Size((int)point_diameter, (int)point_diameter));
                    draw_bitmap.FillEllipse(brush_draw, rect_of_point);
                }
                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(ToBitmap.PointArray), execution_time_ms_stop - execution_time_ms_start);
                }
                return bitmap_out;
            }



            /// <summary>
            /// Creates Bitmap with point of certain color and certain radius
            /// Written. Note made in Warsaw. Workplace. 2024.07.03 11:24. 
            /// </summary>
            /// <param name="points_arr_in"></param>
            /// <param name="point_color"></param>
            /// <param name="point_diameter"></param>
            /// <returns></returns>
            public static Bitmap PointArray(Point[] points_arr_in, DotDrawSettings dot_settings, UInt32 pixels_per_x_point = 4, UInt32 pixels_per_y_point = 4)
            {
                if (dot_settings.DotSize < 2)
                {
                    ReportFunctions.ReportError("Point diameter error. Min width and height is 2. Rectangle 1x1 is not drawn");
                    Bitmap bitmap_error = Generate.Rectungular_Checkboard(200, 200);
                    return bitmap_error;
                }
                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }
                int[] x_values = Extract.FromPoints.XValues(points_arr_in);
                int[] y_values = Extract.FromPoints.YValues(points_arr_in);
                Int32 max_x = x_values.Max();
                Int32 max_y = y_values.Max();

                Int32 min_x = x_values.Min();
                Int32 min_y = y_values.Min();
                Int32 range_y = max_y - min_y;


                // Note made in Warsaw. Workplace. 2024.07.04 12:05. 
                // +1 is added. There is no explanation why to do this.
                // Note made in Warsaw. Workplace. 2024.07.04 14:19.
                // Width is index of end point and not length. That is why the calculation was not correct.

                // Note made in Warsaw. Workplace. 2024.07.04 14:21. 
                // By math it is: max_x - min_x + 1 - 1
                // +1 to include start point and -1 because spaces are by 1 less than amount of dots,
                Bitmap bitmap_out = new Bitmap((max_x - min_x) * (int)pixels_per_x_point + (int)dot_settings.DotSize, (max_y - min_y)*(int)pixels_per_y_point + (int)dot_settings.DotSize);
                
                
                // Note made in Warsaw. Workplace. 2024.07.04 12:06. 
                // Coordinate. it +1 to convert to length and then it is -1 because amount of spaces is less by 1 than amount of dots (1 dot does not take space in pixels).
                for (Int32 i = 0; i < points_arr_in.Length; i++)
                {
                    dot_settings.DrawDot(ref bitmap_out, new Point((int)dot_settings.DotSize / 2 + (points_arr_in[i].X - min_x) * (int)pixels_per_x_point - 1, (int)dot_settings.DotSize / 2 + (range_y - (points_arr_in[i].Y - min_y)) * (int)pixels_per_y_point- 1));
                }
                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(ToBitmap.PointArray), execution_time_ms_stop - execution_time_ms_start);
                }
                return bitmap_out;
            }



            /// <summary>
            /// Written. 2024.06.13 17:11. Gdansk. Home 
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static Bitmap BoolMask(bool[][] arr_in)
            {
                Bitmap image_out = new Bitmap(arr_in.Length, arr_in[0].Length);
                for (int i = 0; i < arr_in.Length; i++)
                {
                    for (int j = 0; j < arr_in[i].Length; j++)
                    {
                        if (arr_in[i][j] == true)
                        {
                            image_out.SetPixel(i, j, Color.Black);
                        }
                        else
                        {
							image_out.SetPixel(i, j, Color.White);
						}
                    }
                }
                return image_out;
            }

				/// <summary>
				/// Written. 2024.02.25 22:06. Warsaw. Hostel. <br></br>
				/// Tested. Works. 2024.02.25 22:08. Warsaw. Hostel.
				/// </summary>
				/// <param name="points_arr_in"></param>
				/// <param name="point_width"></param>
				/// <param name="line_color"></param>
				/// <returns></returns>
				public static Bitmap LineArray(Point[] points_arr_in, Color line_color, UInt32 line_width = 1)
            {
                if (line_width == 0)
                {
                    ReportFunctions.ReportError("Line width error");
                    Bitmap bitmap_error = Generate.Rectungular_Checkboard(200, 200);
                    return bitmap_error;
                }
                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }
                Int32 max_x = Extract.FromPoints.XValues(points_arr_in).Max();
                Int32 max_y = Extract.FromPoints.YValues(points_arr_in).Max();
                Bitmap bitmap_out = new Bitmap(max_x, max_y);
                Graphics draw_bitmap = Graphics.FromImage(bitmap_out);
                Pen pen_draw = new Pen(line_color, line_width);
                for (Int32 i = 1; i < points_arr_in.Length; i++)
                {
                    draw_bitmap.DrawLine(pen_draw, points_arr_in[i - 1], points_arr_in[i]);
                }
                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(ToBitmap.LineArray), execution_time_ms_stop - execution_time_ms_start);
                }
                return bitmap_out;
            }
        }
        /// <summary>
        /// Written. 2024.02.09 09:26. Warsaw. Workplace. 
        /// </summary>
        public static class ToPictureBox
        {

            public static void FromXYValues(int[] y_values, int[] x_values, DotDrawSettings dot_settings, Form form_call_from, UInt32 pixels_per_x_value = 4, UInt32 pixels_per_y_value = 4)
            {
                if (dot_settings.DotSize == 0)
                {
                    ReportFunctions.ReportError("Point radius error");
                    Bitmap bitmap_error = Generate.Rectungular_Checkboard(200, 200);
                    FromBitmap(bitmap_error, form_call_from);
                    return;
                }



                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }


                Bitmap bitmap_to_show = ToBitmap.XYValues(y_values, x_values, dot_settings, pixels_per_x_value, pixels_per_y_value);


                FromBitmap(bitmap_to_show, form_call_from);

                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(ToPictureBox.FromXYValues), execution_time_ms_stop - execution_time_ms_start);
                }


            }




            public static void FromXYValues(int[] y_values, int[] x_values, UInt32 point_width, Color point_color, Form form_call_from, UInt32 pixels_per_x_value = 1)
            {
                if (point_width == 0)
                {
                    ReportFunctions.ReportError("Point radius error");
                    Bitmap bitmap_error = Generate.Rectungular_Checkboard(200, 200);
                    FromBitmap(bitmap_error, form_call_from);
                    return;
                }



                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }
                

                Bitmap bitmap_to_show = ToBitmap.XYValues(y_values, x_values, point_color, point_width, pixels_per_x_value); 

                
                FromBitmap(bitmap_to_show, form_call_from);

                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(ToPictureBox.FromXYValues), execution_time_ms_stop - execution_time_ms_start);
                }


            }



            static Form FormOutput = null;
            static PictureBox PictureBoxOutput = null;
            static Int32 XYOfset = 10;

            /// <summary>
            /// Shows in PictureBox black and white Bitmap from Int32[][].
            /// </summary>
            /// <param name="array_in"></param>
            /// <param name="form_called_from"></param>
            public static void FromInt32ArrayBlackWhite(Int32[][] array_in, Form form_called_from)
            {
                FromBitmap(ToBitmap.Int32ArrayBlackWhite(array_in), form_called_from);
                // Written. Warsaw. Workplace. 2024-07-16 15-11. 
                // Tested. Works. Warsaw. Workplace. 2024-07-16 15-21. 
            }


            /// <summary>
            /// Written. 2024.02.25 21:33. Warsaw. Hostel. <br></br>
            /// Tested. Works. 2024.02.25 21:54. Warsaw. Hostel.
            /// </summary>
            /// <param name="points_arr_in"></param>
            /// <param name="point_width"></param>
            /// <param name="point_color"></param>
            /// <param name="form_call_from"></param>
            public static void FromPointArray(Point[] points_arr_in, UInt32 point_width, Color point_color, Form form_call_from)
            {
                if (point_width == 0)
                {
                    ReportFunctions.ReportError("Point radius error");
                    Bitmap bitmap_error = Generate.Rectungular_Checkboard(200, 200);
                    FromBitmap(bitmap_error, form_call_from);
                    return;
                }
                Int32 max_x = Extract.FromPoints.XValues(points_arr_in).Max();
                Int32 max_y = Extract.FromPoints.YValues(points_arr_in).Max();
                Bitmap bitmap_out = new Bitmap(max_x, max_y);
                Graphics draw_bitmap = Graphics.FromImage(bitmap_out);
                Pen pen_draw = new Pen(point_color, point_width);
                for (Int32 i = 0; i < points_arr_in.Length; i++)
                {
                    draw_bitmap.DrawArc(pen_draw, new Rectangle(points_arr_in[i], new Size((int)point_width, (int)point_width)), 0, 360);
                }
                FromBitmap(bitmap_out, form_call_from);
            }

            /// <summary>
            /// Written. 2024.06.13 17:13. Gdansk. Home 
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="form_call_from"></param>
            public static void FromBoolMask(bool[][] arr_in, Form form_call_from)
            {
                Bitmap image_from_arr = ToBitmap.BoolMask(arr_in);
                FromBitmap(image_from_arr, form_call_from);
            }

				/// <summary>
				/// Shows Forms with Picture box with provided Bitmap. <br></br>
				/// Written. 2024.02.09 10:06. Warsaw. Workplace. <br></br>
				/// Tested. Works. 2024.02.09 10:35. Warsaw. Workplace. 
				/// </summary>
				/// <param name="bitmap_in"></param>
				/// <param name="form_call_from"></param>
				public static void FromBitmap(Bitmap bitmap_in, Form form_call_from)
            {
                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }
                Int32 form_length = bitmap_in.Width;
                Int32 form_height = bitmap_in.Height;
                Form FormOutput = new Form();
                FormOutput.AutoSize = false;
                FormOutput.ClientSize = new Size(form_length + XYOfset * 2, form_height + XYOfset * 2);
                PictureBoxOutput = new PictureBox();
                PictureBoxOutput.Location =
                    new Point(FormOutput.ClientRectangle.Location.X + XYOfset, FormOutput.ClientRectangle.Location.Y + XYOfset);
                PictureBoxOutput.ClientSize = new Size(form_length, form_height);
                // copy of bitmap may be required. 2024.02.09 10:07. Warsaw. Workplace. 
                PictureBoxOutput.Image = bitmap_in;
                FormOutput.Controls.Add(PictureBoxOutput);
                FormOutput.Show();
                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(FromBitmap), execution_time_ms_stop - execution_time_ms_start);
                }
            }
            /// <summary>
            /// Written. 2024.02.09 09:26. Warsaw. Workplace. 
            /// </summary>
            /// <param name="color_arr_in"></param>
            /// <param name="row_length"></param>
            /// <param name="form_call_from"></param>
            public static void FromColorArray(Color[] color_arr_in, Int32 row_length, Form form_call_from)
            {
                Form FormOutput = new Form();
                FormOutput.AutoSize = false;
                FormOutput.ClientSize = new Size(row_length + XYOfset * 2, (color_arr_in.Length / row_length) + XYOfset * 2);
                PictureBoxOutput = new PictureBox();
                PictureBoxOutput.Location =
                    new Point(FormOutput.ClientRectangle.Location.X + XYOfset, FormOutput.ClientRectangle.Location.Y + XYOfset);
                PictureBoxOutput.ClientSize = new Size(1, color_arr_in.Length);
                Bitmap bitmap_out = new Bitmap(row_length, (color_arr_in.Length / row_length));
                Int32 rows_count = (color_arr_in.Length / row_length);
                for (Int32 j = 0; j < rows_count; j++)
                {
                    for (Int32 i = 0; i < row_length; i++)
                    {
                        bitmap_out.SetPixel(i, j, color_arr_in[j * row_length + i]);
                    }
                }
                PictureBoxOutput.Image = bitmap_out;
                FormOutput.Controls.Add(PictureBoxOutput);
                FormOutput.Show();
            }
        }


        /// <summary>
        /// Written. Warsaw. Workplace. 2024.04.22 07:18. 
        /// </summary>
        public static class Grid
        {

            /// <summary>
            /// Written. Warsaw. Workplace. 2024.04.22 07:18. 
            /// </summary>
            /// <param name="bitmap_in"></param>
            /// <param name="points_in"></param>
            /// <param name="x_div"></param>
            /// <param name="y_div"></param>
            /// <param name="color_of_grid"></param>
            /// <param name="width_of_grid"></param>
            /// <returns></returns>
            public static Bitmap Draw(Bitmap bitmap_in, Point[] points_in, int x_div, int y_div, Color color_of_grid, int width_of_grid = 1)
            {
                if (points_in.Length < 2)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.LengthMaybeWrong);
                    Point[] points_new_arr = new Point[2];
                    points_new_arr[0] = new Point(0, 0);
                    points_new_arr[1] = new Point(points_in[0].X, points_in[0].Y);
                    points_in = points_new_arr;
                }
                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }



                Bitmap bitmap_out = ImageFunctions.Copy(bitmap_in);
                Int32[] x_values = Extract.FromPoints.XValues(points_in);
                Int32[] y_values = Extract.FromPoints.YValues(points_in);

                int x_grid_step = (x_values.Max() - x_values.Min()) / x_div;
                int y_grid_step = (y_values.Max() - y_values.Min()) / y_div;

                if (((x_values.Max() - x_values.Min()) % x_div) != 0)
                {
                    x_grid_step += 1;
                }

                if (((y_values.Max() - y_values.Min()) % y_div) != 0)
                {
                    y_grid_step += 1;
                }

                Pen pen_draw = new Pen(color_of_grid, width_of_grid);
                Graphics draw_line = Graphics.FromImage(bitmap_out);    
                // draw y lines
                for (int i = 0; i < y_grid_step; i++)
                {
                    draw_line.DrawLine(pen_draw, new Point(0, y_grid_step*i), new Point(bitmap_out.Width, y_grid_step*i));
                }
                // draw x lines
                for (int i = 0; i < x_grid_step; i++)
                {
                    draw_line.DrawLine(pen_draw, new Point(x_grid_step * i, 0), new Point(x_grid_step * i, bitmap_out.Height));
                }

                pen_draw.Dispose();
                draw_line.Dispose();

                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(Grid.Draw), execution_time_ms_stop - execution_time_ms_start);
                }

                return bitmap_out;
            }

        }





            /// <summary>
            /// Written. Warsaw. Hostel. 2024.04.21 22:06. 
            /// </summary>
            public static class Line
        {
            /// <summary>
            /// It can be large amount of code.
            /// </summary>
            public enum LineStyle
            {
                SolidLine,
                /// <summary>
                /// size of line defines size of dot.
                /// </summary>
                Dots_1_1,
            }
            /// <summary>
            /// Written. Warsaw. Hostel. 2024.04.21 22:26. <br></br>
            /// Tested. Works. Warsaw. Hostel. 2024.04.21 22:32. <br></br>
            /// <br></br>
            /// Note. It works faster by 4-5 times than via copy Bitmap. <br></br>
            /// 0.050 - 0.080 ms vs 0.200 - 0.300 ms. <br></br>
            /// </summary>
            /// <param name="bitmap_in"></param>
            /// <param name="points_of_line"></param>
            /// <param name="color_of_line"></param>
            /// <param name="width_of_line"></param>
            public static void DrawOneLine(ref Bitmap bitmap_in, Point[] points_of_line, Color color_of_line, LineStyle style_of_line, int width_of_line = 1)
            {
                if (points_of_line.Length != 2)
                {
                    ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Length_is_Wrong);
                    return;
                }
                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }
                Graphics draw_graphics = Graphics.FromImage(bitmap_in);
                // float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                // TimeExecutionMessage(nameof(Line.DrawOneLine) + " Graphics", execution_time_ms_stop - execution_time_ms_start);
                Pen pen_draw = new Pen(color_of_line, width_of_line);

                if (style_of_line == LineStyle.SolidLine)
                {
                    draw_graphics.DrawLine(pen_draw, points_of_line[0], points_of_line[1]);
                }


                // 2024.04.22. 6:20. Trouble. Line at angle is not easy to draw if it is dashed.
                if (style_of_line == LineStyle.Dots_1_1)
                {
                    int line_length = points_of_line[1].X - points_of_line[0].X;
                    bool length_is_exceeded = false;
                    int current_length = 0;
                    while (length_is_exceeded == false)
                    {
                      //  draw_graphics.FillRectangle(new SolidBrush(color_of_line), 
                      //      new Rectangle(new Point(current_length, ))

                    }
                    
                    for (int i = 0; i < line_length; i++)
                    {
                        draw_graphics.DrawLine(pen_draw, points_of_line[0], points_of_line[1]);
                    }
                    
                }



                // execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                // TimeExecutionMessage(nameof(Line.DrawOneLine) + " Draw Line", execution_time_ms_stop - execution_time_ms_start);
                draw_graphics.Dispose();
                pen_draw.Dispose();
                // execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                // TimeExecutionMessage(nameof(Line.DrawOneLine) + " Dispose", execution_time_ms_stop - execution_time_ms_start);
                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(Line.DrawOneLine), execution_time_ms_stop - execution_time_ms_start);
                }


                /*
                
                1. 01:42:14 DrawOneLine exectuion time: 1.605 ms
                2. 01:42:14 FromBitmap exectuion time: 31.038 ms
                3. 01:42:15 DrawOneLine exectuion time: 0.056 ms
                4. 01:42:15 FromBitmap exectuion time: 11.180 ms
                5. 01:42:15 DrawOneLine exectuion time: 0.044 ms
                6. 01:42:15 FromBitmap exectuion time: 14.688 ms
                7. 01:42:15 DrawOneLine exectuion time: 0.041 ms
                8. 01:42:15 FromBitmap exectuion time: 11.696 ms
                9. 01:42:16 DrawOneLine exectuion time: 0.044 ms
                10. 01:42:16 FromBitmap exectuion time: 15.125 ms
                11. 01:42:16 DrawOneLine exectuion time: 0.045 ms
                12. 01:42:16 FromBitmap exectuion time: 13.347 ms
                13. 01:42:16 DrawOneLine exectuion time: 0.088 ms
                14. 01:42:16 FromBitmap exectuion time: 13.175 ms
                15. 01:42:16 DrawOneLine exectuion time: 0.067 ms
                16. 01:42:16 FromBitmap exectuion time: 15.584 ms
                17. 01:42:19 DrawOneLine exectuion time: 0.063 ms
                18. 01:42:19 FromBitmap exectuion time: 17.571 ms

                */

                /*

                1. 01:44:11 Copy exectuion time: 1.655 ms
                2. 01:44:11 DrawOneLine exectuion time: 5.965 ms
                3. 01:44:11 FromBitmap exectuion time: 18.207 ms
                4. 01:44:12 Copy exectuion time: 0.478 ms
                5. 01:44:12 DrawOneLine exectuion time: 0.700 ms
                6. 01:44:12 FromBitmap exectuion time: 13.379 ms
                7. 01:44:16 Copy exectuion time: 0.479 ms
                8. 01:44:16 DrawOneLine exectuion time: 0.810 ms
                9. 01:44:16 FromBitmap exectuion time: 12.806 ms
                10. 01:44:16 Copy exectuion time: 0.519 ms
                11. 01:44:16 DrawOneLine exectuion time: 0.799 ms
                12. 01:44:16 FromBitmap exectuion time: 14.960 ms
                13. 01:44:16 Copy exectuion time: 0.442 ms
                14. 01:44:16 DrawOneLine exectuion time: 0.655 ms
                15. 01:44:16 FromBitmap exectuion time: 14.298 ms
                16. 01:44:16 Copy exectuion time: 0.497 ms
                17. 01:44:16 DrawOneLine exectuion time: 0.657 ms
                18. 01:44:16 FromBitmap exectuion time: 12.770 ms
                19. 01:44:16 Copy exectuion time: 0.460 ms
                20. 01:44:16 DrawOneLine exectuion time: 0.738 ms
                21. 01:44:16 FromBitmap exectuion time: 13.413 ms
                22. 01:44:16 Copy exectuion time: 0.622 ms
                23. 01:44:16 DrawOneLine exectuion time: 0.853 ms
                24. 01:44:16 FromBitmap exectuion time: 13.758 ms
                25. 01:44:16 Copy exectuion time: 0.470 ms
                26. 01:44:16 DrawOneLine exectuion time: 0.695 ms
                27. 01:44:16 FromBitmap exectuion time: 10.933 ms
                28. 01:44:17 Copy exectuion time: 0.450 ms
                29. 01:44:17 DrawOneLine exectuion time: 0.626 ms
                30. 01:44:17 FromBitmap exectuion time: 18.429 ms






                /*
                Warsaw. Hostel. 2024.04.21 22:39. 
                Important at 1st load there is long execution time. It may be because of memory allocation.
                After allocation the execution time decreases.
                However, there were Bitmap array used of 20 bitmap to check if it memory allocation and
                there were previous result - 1st time execution time is long and after it decareases. 
                1. 22:37:37 DrawOneLine Graphics exectuion time: 0.404 ms
                2. 22:37:37 DrawOneLine Draw Line exectuion time: 4.344 ms
                3. 22:37:37 DrawOneLine Dispose exectuion time: 4.949 ms
                4. 22:37:37 DrawOneLine exectuion time: 5.015 ms
                5. 22:37:37 FromBitmap exectuion time: 25.374 ms
                6. 22:38:10 DrawOneLine Graphics exectuion time: 0.012 ms
                7. 22:38:10 DrawOneLine Draw Line exectuion time: 0.305 ms
                8. 22:38:10 DrawOneLine Dispose exectuion time: 0.438 ms
                9. 22:38:10 DrawOneLine exectuion time: 0.551 ms
                10. 22:38:11 FromBitmap exectuion time: 21.234 ms
                11. 22:38:21 DrawOneLine Graphics exectuion time: 0.012 ms
                12. 22:38:21 DrawOneLine Draw Line exectuion time: 0.180 ms
                13. 22:38:21 DrawOneLine Dispose exectuion time: 0.297 ms
                14. 22:38:21 DrawOneLine exectuion time: 0.422 ms
                15. 22:38:21 FromBitmap exectuion time: 16.828 ms
                16. 22:38:26 DrawOneLine Graphics exectuion time: 0.016 ms
                17. 22:38:26 DrawOneLine Draw Line exectuion time: 0.309 ms
                18. 22:38:26 DrawOneLine Dispose exectuion time: 0.398 ms
                19. 22:38:26 DrawOneLine exectuion time: 0.527 ms
                20. 22:38:26 FromBitmap exectuion time: 13.398 ms
                21. 22:38:27 DrawOneLine Graphics exectuion time: 0.012 ms
                22. 22:38:27 DrawOneLine Draw Line exectuion time: 0.266 ms
                23. 22:38:27 DrawOneLine Dispose exectuion time: 0.363 ms
                24. 22:38:27 DrawOneLine exectuion time: 0.461 ms
                25. 22:38:27 FromBitmap exectuion time: 17.297 ms
                26. 22:38:32 DrawOneLine Graphics exectuion time: 0.016 ms
                27. 22:38:32 DrawOneLine Draw Line exectuion time: 0.309 ms
                28. 22:38:32 DrawOneLine Dispose exectuion time: 0.449 ms
                29. 22:38:32 DrawOneLine exectuion time: 0.551 ms 
                 */
            }

            /// <summary>
            /// Written. Warsaw. Hostel. 2024.04.21 22:06. 
            /// Tested. Works. Warsaw. Hostel. 2024.04.21 22:31.
            /// note. via set pixel for dashed line. 
            /// via cast to array.
            /// </summary>
            /// <param name="bitmap_in"></param>
            /// <param name="points_of_line"></param>
            /// <param name="color_of_line"></param>
            /// <param name="width_of_line"></param>
            /// <returns></returns>
            public static Bitmap DrawOneLine(Bitmap bitmap_in, Point[] points_of_line, Color color_of_line, int width_of_line = 1)
            {
                if (points_of_line.Length != 2)
                {
                    ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Length_is_Wrong);
                    return bitmap_in;
                }
                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }
                Bitmap bitmap_out = ImageFunctions.Copy(bitmap_in);
                Graphics draw_graphics = Graphics.FromImage(bitmap_out);
                // float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                // TimeExecutionMessage(nameof(Line.DrawOneLine) + " Graphics", execution_time_ms_stop - execution_time_ms_start);
                Pen pen_draw = new Pen(color_of_line, width_of_line);
                draw_graphics.DrawLine(pen_draw, points_of_line[0], points_of_line[1]);
                // execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                // TimeExecutionMessage(nameof(Line.DrawOneLine) + " Draw Line", execution_time_ms_stop - execution_time_ms_start);
                draw_graphics.Dispose();
                pen_draw.Dispose();
                // execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                // TimeExecutionMessage(nameof(Line.DrawOneLine) + " Dispose", execution_time_ms_stop - execution_time_ms_start);
                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(Line.DrawOneLine), execution_time_ms_stop - execution_time_ms_start);
                }
                return bitmap_out;
                /*                
                Warsaw. Hostel. 2024.04.21 22:49.                 
                Making copy of bitmap does not lead to significant increase of execution time.
                Note that large execution time is at 1st load as it is for function with ref.
                Exdcution time with ref is faster. It appears that working new newly made bitmap
                takes more time than with the bitmap that was created before and Graphics was used
                for that.
                1. 22:47:40 Copy exectuion time: 2.487 ms
                2. 22:47:40 DrawOneLine Graphics exectuion time: 7.173 ms
                3. 22:47:40 DrawOneLine Draw Line exectuion time: 8.558 ms
                4. 22:47:40 DrawOneLine Dispose exectuion time: 10.245 ms
                5. 22:47:41 DrawOneLine exectuion time: 13.964 ms
                6. 22:47:41 FromBitmap exectuion time: 20.871 ms
                7. 22:47:45 Copy exectuion time: 0.435 ms
                8. 22:47:45 DrawOneLine Graphics exectuion time: 0.655 ms
                9. 22:47:45 DrawOneLine Draw Line exectuion time: 0.826 ms
                10. 22:47:45 DrawOneLine Dispose exectuion time: 0.937 ms
                11. 22:47:45 DrawOneLine exectuion time: 1.052 ms
                12. 22:47:45 FromBitmap exectuion time: 12.562 ms
                13. 22:48:00 Copy exectuion time: 0.451 ms
                14. 22:48:00 DrawOneLine Graphics exectuion time: 0.693 ms
                15. 22:48:00 DrawOneLine Draw Line exectuion time: 0.830 ms
                16. 22:48:00 DrawOneLine Dispose exectuion time: 0.947 ms
                17. 22:48:00 DrawOneLine exectuion time: 1.059 ms
                18. 22:48:00 FromBitmap exectuion time: 18.576 ms
                19. 22:48:07 Copy exectuion time: 0.453 ms
                20. 22:48:07 DrawOneLine Graphics exectuion time: 0.666 ms
                21. 22:48:07 DrawOneLine Draw Line exectuion time: 0.785 ms
                22. 22:48:07 DrawOneLine Dispose exectuion time: 0.898 ms
                23. 22:48:07 DrawOneLine exectuion time: 1.008 ms
                24. 22:48:07 FromBitmap exectuion time: 17.123 ms
                25. 22:48:08 Copy exectuion time: 0.453 ms
                26. 22:48:08 DrawOneLine Graphics exectuion time: 0.709 ms
                27. 22:48:08 DrawOneLine Draw Line exectuion time: 0.846 ms
                28. 22:48:08 DrawOneLine Dispose exectuion time: 0.963 ms
                29. 22:48:08 DrawOneLine exectuion time: 1.092 ms
                30. 22:48:08 FromBitmap exectuion time: 19.016 ms
                */
            }
        }
        /// <summary>
        /// Written. 2024.02.10 14:33. Warsaw. Hostel.
        /// </summary>
        public static class Find
        {
            /// <summary>
            /// Written. 2024.02.11 09:44. Warsaw. Hostel.
            /// </summary>
            public static class Location
            {
                /// <summary>
                /// Return Rectangle[] of object that are in the Bitmap. <br></br>
                /// Written. 2024.02.11 09:44. Warsaw. Hostel. <br></br>
                /// Tested. Works. 2024.02.11 13:45. Warsaw. Hostel. <br></br>
                /// </summary>
                public static class OfMultipleObjects
                {
                    public static Rectangle[] FromLeftToRight(Bitmap bitmap_in, Color color_background)
                    {
                        List<Rectangle> rect_found = new List<Rectangle>();
                        Int32[][] bitmap_int32 = Convert.BitmapToInt32ArrayAxB(bitmap_in);
                        Int32 rect_x_start = 0;
                        bool object_found = false;
                        for (Int32 i = 0; i < bitmap_in.Width; i++)
                        {
                            Int32[] column = ArrayFunctions.Column.Take(bitmap_int32, i);
                            if (ArrayFunctions.Int32Array.Math.ElementsTheSame(column) == true)
                            {
                                Int32 bitmap_pixel = bitmap_in.GetPixel(i, 0).ToArgb();
                                Int32 background_pixel = color_background.ToArgb();
                                if (bitmap_pixel == background_pixel)
                                {
                                    if (object_found == false)
                                    {
                                        rect_x_start = i;
                                        continue;
                                    }
                                }
                            }
                            if (object_found == false)
                            {
                                if (ArrayFunctions.Int32Array.Math.ElementsTheSame(column) == true)
                                {
                                    Int32 bitmap_pixel = bitmap_in.GetPixel(i, 0).ToArgb();
                                    Int32 background_pixel = color_background.ToArgb();
                                    if (bitmap_pixel != background_pixel)
                                    {
                                        rect_x_start = i;
                                        object_found = true;
                                        continue;
                                    }
                                }
                            }
                            if (object_found == true)
                            {
                                if (ArrayFunctions.Int32Array.Math.ElementsTheSame(column) == true)
                                {
                                    Int32 bitmap_pixel = bitmap_in.GetPixel(i, 0).ToArgb();
                                    Int32 background_pixel = color_background.ToArgb();
                                    if (bitmap_pixel == background_pixel)
                                    {
                                        object_found = false;
                                        rect_found.Add(new Rectangle(new Point(rect_x_start, 0), new Size(i - rect_x_start, bitmap_in.Height)));
                                        continue;
                                    }
                                }
                            }
                        }
                        if (object_found == true)
                        {
                            object_found = false;
                            rect_found.Add(new Rectangle(new Point(rect_x_start, 0), new Size(bitmap_in.Width - (rect_x_start + 1), bitmap_in.Height)));
                        }
                        // saved code. it does not work properly. 2024.02.11 11:04. Warsaw. Hostel.
                        // it was saved to see the approach.
                        /*
                        Int32 last_x = 0;
                        bool object_start_found = false;
                        bool object_end_found = false;
                        for (Int32 i = 0; i < bitmap_in.Width; i++)
                        {
                            Int32[] column = MyArrayFunctions.Column.Take(bitmap_int32, i);
                            // 1st condition. pixels are different. 2024.02.11 10:36. Warsaw. Hostel.
                            if (MyArrayFunctions.Int32Array.Math.ElementsTheSame(column) == false)
                            {
                                if (object_start_found == false)
                                {
                                    object_start_found = true;
                                }
                            }
                            else
                            {
                                Int32 bitmap_pixel = bitmap_in.GetPixel(i, 0).ToArgb();
                                Int32 background_pixel = color_background.ToArgb();
                                if (bitmap_pixel != background_pixel)
                                {
                                    if (object_start_found == false)
                                    {
                                        object_start_found = true;
                                    }                                    
                                }
                                else
                                {
                                    object_end_found = true;
                                }
                            }
                            if ((object_start_found == true) &&
                                (object_end_found == true))
                            {
                                object_start_found = false;
                                object_end_found = false;
                                rect_found.Add(new Rectangle(new Point(i, 0), new Size(i - last_x, bitmap_in.Height)));
                                last_x = i;
                            }
                        }
                        */
                        return rect_found.ToArray();
                    }
                }
                /// <summary>
                /// Return location of part of picture using provided color of background. <br></br>
                /// Written. 2024.02.10 19:04. Warsaw. Hostel. <br></br>
                /// Tested. Works. 2024.02.10 19:38. Warsaw. Hostel.
                /// </summary>
                /// <param name="bitmap_in"></param>
                /// <param name="color_background"></param>
                /// <returns></returns>
                public static Rectangle OfObject(Bitmap bitmap_in, Color color_background)
                {
                    Rectangle rectangle_out = new Rectangle();
                    Int32 left_of_rectangle = (int)LengthOfBackground.Left(bitmap_in, color_background);
                    Int32 right_of_rectangle = (int)LengthOfBackground.Right(bitmap_in, color_background);
                    Int32 top_of_rectangle = (int)LengthOfBackground.Top(bitmap_in, color_background);
                    Int32 bottom_of_rectangle = (int)LengthOfBackground.Bottom(bitmap_in, color_background);
                    rectangle_out.Location = new Point(left_of_rectangle - 1 + 1, top_of_rectangle - 1 + 1);
                    rectangle_out.Size = new Size(bitmap_in.Width - right_of_rectangle - left_of_rectangle,
                        bitmap_in.Height - bottom_of_rectangle - top_of_rectangle);
                    return rectangle_out;
                }
            }
            /// <summary>
            /// Written. 2024.02.10 14:34. Warsaw. Hostel.
            /// </summary>
            public static class LengthOfBackground
            {
                /// <summary>
                /// Find the length of background using provided color of background. <br></br>
                /// Written. 2024.02.10 16:46. Warsaw. Hostel. <br></br>
                /// Tested. 2024.02.10 17:06. Warsaw. Hostel. 
                /// </summary>
                /// <param name="bitmap_in"></param>
                /// <param name="background_color"></param>
                /// <returns></returns>
                public static UInt32 Right(Bitmap bitmap_in, Color background_color)
                {
                    Int32[][] ImageInt32Array = Convert.BitmapToInt32ArrayAxB(bitmap_in);
                    Int32 columns_count = ImageInt32Array.Length;
                    Int32 length_out = 0;
                    for (Int32 i = columns_count - 1; i >= 0; i--)
                    {
                        Int32[] row_int32 = ArrayFunctions.Column.Take(ImageInt32Array, i);
                        if (ArrayFunctions.Int32Array.Math.ElementsTheSame(row_int32) == true)
                        {
                            length_out += 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    return (uint)length_out;
                }
                /// <summary>
                /// Find the length of background using provided color of background. <br></br>
                /// Written. 2024.02.10 16:32. Warsaw. Hostel.
                /// Tested. Works. 2024.02.10 16:45. Warsaw. Hostel.
                /// </summary>
                /// <param name="bitmap_in"></param>
                /// <param name="background_color"></param>
                /// <returns></returns>
                public static UInt32 Left(Bitmap bitmap_in, Color background_color)
                {
                    Int32[][] ImageInt32Array = Convert.BitmapToInt32ArrayAxB(bitmap_in);
                    Int32 columns_count = ImageInt32Array.Length;
                    Int32 length_out = 0;
                    for (Int32 i = 0; i < columns_count; i++)
                    {
                        Int32[] row_int32 = ArrayFunctions.Column.Take(ImageInt32Array, i);
                        if (ArrayFunctions.Int32Array.Math.ElementsTheSame(row_int32) == true)
                        {
                            length_out += 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    return (uint)length_out;
                }
                /// <summary>
                /// Find the length of background using provided color of background. <br></br>
                /// Written. 2024.02.10 14:52. Warsaw. Hostel. <br></br>
                /// Tested. 2024.02.10 16:29. Warsaw. Hostel. 
                /// </summary>
                /// <param name="bitmap_in"></param>
                /// <param name="background_color"></param>
                /// <returns></returns>
                public static UInt32 Bottom(Bitmap bitmap_in, Color background_color)
                {
                    Int32[][] ImageInt32Array = Convert.BitmapToInt32ArrayAxB(bitmap_in);
                    Int32 rows_count = ImageInt32Array[0].Length;
                    Int32 length_out = 0;
                    for (Int32 i = rows_count - 1; i >= 0; i--)
                    {
                        Int32[] row_int32 = ArrayFunctions.Row.Take(ImageInt32Array, i);
                        if (ArrayFunctions.Int32Array.Math.ElementsTheSame(row_int32) == true)
                        {
                            length_out += 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    return (uint)length_out;
                }
                /// <summary>
                /// Written. 2024.02.10 14:40. Warsaw. Hostel.
                /// Tested. Works. 2024.02.10 14:51. Warsaw. Hostel.
                /// </summary>
                /// <param name="bitmap_in"></param>
                /// <param name="background_color"></param>
                /// <returns></returns>
                public static UInt32 Top(Bitmap bitmap_in, Color background_color)
                {
                    Int32[][] ImageInt32Array = Convert.BitmapToInt32ArrayAxB(bitmap_in);
                    Int32 rows_count = ImageInt32Array[0].Length;
                    Int32 length_out = 0;
                    for (Int32 i = 0; i < rows_count; i++)
                    {
                        Int32[] row_int32 = ArrayFunctions.Row.Take(ImageInt32Array, i);
                        if (ArrayFunctions.Int32Array.Math.ElementsTheSame(row_int32) == true)
                        {
                            length_out += 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    return (uint)length_out;
                }
            }
        }
        /// <summary>
        /// Written. 2024.02.10 13:10. Warsaw. Hostel 
        /// </summary>
        public static class Trim
        {
            /// <summary>
            /// Removes part of Bitmap from bottom according to provided length. <br></br>
            /// Written. 2024.02.10 14:55. Warsaw. Hostel. <br></br>
            /// Tested. Works. 2024.02.10 14:57. Warsaw. Hostel.
            /// <param name="bitmap_in"></param>
            /// <param name="trim_length"></param>
            /// <returns></returns>
            public static Bitmap Bottom(Bitmap bitmap_in, UInt32 trim_length)
            {
                Bitmap bitmap_out = new Bitmap(bitmap_in.Width, bitmap_in.Height - (int)trim_length);
                using (Graphics graphics_draw = Graphics.FromImage(bitmap_out))
                {
                    Rectangle source_rect = new Rectangle(new Point(0, 0), new Size(bitmap_in.Width, bitmap_in.Height - (int)trim_length));
                    Rectangle dest_rect = new Rectangle(new Point(0, 0), new Size(bitmap_out.Width, bitmap_out.Height));
                    graphics_draw.DrawImage(bitmap_in, dest_rect, source_rect, GraphicsUnit.Pixel);
                }
                return bitmap_out;
            }
            /// <summary>
            /// Removes part of Bitmap from left according to provided length. <br></br>
            /// Written. 2024.02.10 14:59. Warsaw. Hostel. <br></br>
            /// Tested. Works. 2024.02.10 15:01. Warsaw. Hostel. 
            /// </summary>
            /// <param name="bitmap_in"></param>
            /// <param name="trim_length"></param>
            /// <returns></returns>
            public static Bitmap Left(Bitmap bitmap_in, UInt32 trim_length)
            {
                Bitmap bitmap_out = new Bitmap(bitmap_in.Width - (int)trim_length, bitmap_in.Height);
                using (Graphics graphics_draw = Graphics.FromImage(bitmap_out))
                {
                    Rectangle source_rect = new Rectangle(new Point((int)trim_length - 1 + 1, 0), new Size(bitmap_in.Width - (int)trim_length, bitmap_in.Height));
                    Rectangle dest_rect = new Rectangle(new Point(0, 0), new Size(bitmap_out.Width, bitmap_out.Height));
                    graphics_draw.DrawImage(bitmap_in, dest_rect, source_rect, GraphicsUnit.Pixel);
                }
                return bitmap_out;
            }
            /// <summary>
            /// Removes part of Bitmap from right according to provided length. <br></br>
            /// Written. 2024.02.10 15:02. Warsaw. Hostel. <br></br>
            /// Tested. Works. 2024.02.10 15:04. Warsaw. Hostel.
            /// </summary>
            /// <param name="bitmap_in"></param>
            /// <param name="trim_length"></param>
            /// <returns></returns>
            public static Bitmap Right(Bitmap bitmap_in, UInt32 trim_length)
            {
                Bitmap bitmap_out = new Bitmap(bitmap_in.Width - (int)trim_length, bitmap_in.Height);
                using (Graphics graphics_draw = Graphics.FromImage(bitmap_out))
                {
                    Rectangle source_rect = new Rectangle(new Point(0, 0), new Size(bitmap_in.Width - (int)trim_length, bitmap_in.Height));
                    Rectangle dest_rect = new Rectangle(new Point(0, 0), new Size(bitmap_out.Width, bitmap_out.Height));
                    graphics_draw.DrawImage(bitmap_in, dest_rect, source_rect, GraphicsUnit.Pixel);
                }
                return bitmap_out;
            }
            /// <summary>
            /// Written. 2024.02.10 13:25. Warsaw. Hostel.
            /// Tested. Works. 2024.02.10 14:31. Warsaw. Hostel.
            /// </summary>
            /// <param name="bitmap_in"></param>
            /// <param name="trim_length"></param>
            /// <returns></returns>
            public static Bitmap Top(Bitmap bitmap_in, UInt32 trim_length)
            {
                Bitmap bitmap_out = new Bitmap(bitmap_in.Width, bitmap_in.Height - (int)trim_length);
                using (Graphics graphics_draw = Graphics.FromImage(bitmap_out))
                {
                    Rectangle source_rect = new Rectangle(new Point(0, (int)trim_length - 1 + 1), new Size(bitmap_in.Width, bitmap_in.Height - (int)trim_length));
                    Rectangle dest_rect = new Rectangle(new Point(0, 0), new Size(bitmap_out.Width, bitmap_out.Height));
                    graphics_draw.DrawImage(bitmap_in, dest_rect, source_rect, GraphicsUnit.Pixel);
                }
                return bitmap_out;
            }
        }
        /// <summary>
        /// Written. 2024.02.25 19:20. Warsaw. Hostel.
        /// </summary>
        public static class Extract
        {
            /// <summary>
            /// Written. 2024.02.25 19:20. Warsaw. Hostel.
            /// not tested.
            /// </summary>
            public static class FromPoints
            {
                public static Int32[] XValues(Point[] arr_in)
                {
                    Int32[] arr_out = new Int32[arr_in.Length];
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        arr_out[i] = arr_in[i].X;
                    }
                    return arr_out;
                }
                /// <summary>
                /// Written. 2024.02.25 19:23. Warsaw. Hostel.
                /// not tested.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static Int32[] YValues(Point[] arr_in)
                {
                    Int32[] arr_out = new Int32[arr_in.Length];
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        arr_out[i] = arr_in[i].Y;
                    }
                    return arr_out;
                }
            }
        }
        public static class Crop
        {
            /// <summary>
            /// Written. 2024.03.10 16:07. Warsaw. Hostel.
            /// Tested. Works. 2024.03.10 17:21. Warsaw. Hostel. 
            /// </summary>
            /// <param name="bitmap_in"></param>
            /// <param name="length_of_rectangle"></param>
            /// <returns></returns>
            public static Bitmap[] HorizontallyByLength(Bitmap bitmap_in, Int32 length_of_rectangle)
            {
                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }
                Int32 amount_rectangles = 0;
                Int32 remained_length = bitmap_in.Width;
                bool counting_rectangles = true;
                while (counting_rectangles == true)
                {
                    if (remained_length >= (length_of_rectangle + 1))
                    {
                        amount_rectangles += 1;
                        remained_length -= (length_of_rectangle);
                        continue;
                    }
                    if (remained_length >= length_of_rectangle)
                    {
                        amount_rectangles += 1;
                        counting_rectangles = false;
                        continue;
                    }
                    counting_rectangles = false;
                }
                Rectangle[] Rectangles_For_Crop = new Rectangle[amount_rectangles];
                for (Int32 i = 0; i < Rectangles_For_Crop.Length; i++)
                {
                    Rectangles_For_Crop[i] = new Rectangle(new Point(i * (length_of_rectangle - 1 + 1), 0), new Size(length_of_rectangle, bitmap_in.Height));
                }
                Bitmap[] Bitmap_Arr_Out = new Bitmap[amount_rectangles];
                Bitmap_Arr_Out = Crop.MultipleRectangles(bitmap_in, Rectangles_For_Crop);
                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(Crop.HorizontallyByLength), execution_time_ms_stop - execution_time_ms_start);
                }
                return Bitmap_Arr_Out;
            }
            /// <summary>
            /// Written. 2024.02.11 17:03. Warsaw. Hostel.
            /// Tested. Works. 2024.02.11 18:25. Warsaw. Hostel.
            /// </summary>
            /// <param name="bitmap_in"></param>
            /// <param name="mark_color"></param>
            /// <param name="color_background">
            /// 2024.03.06 16:20. Warsaw. Workplace. <br></br>
            /// It is needed to replace mark color with background color in working Bitmap. 
            /// </param>
            /// <returns></returns>
            public static Bitmap[] HorizontallyByMark(Bitmap bitmap_in, Color mark_color, Color color_background)
            {
                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }
                Bitmap bitmap_to_work = new Bitmap(bitmap_in);
                List<int> mark_values = new List<int>();
                // transparent is not error. it shows 0 can be moved to initialization. 2024.02.11 17:22. Warsaw. Hostel.
                mark_values.Add(0);
                Int32[][] bitmap_int32 = Convert.BitmapToInt32ArrayAxB(bitmap_to_work);
                // 1st row maked difficult to put the marks. 2024.02.14 12:43. Warsaw. Workplace. 
                /*
                Int32[] row_1st = MyArrayFunctions.Row.Take(bitmap_int32, 0);
                for (Int32 i = 0; i < row_1st.Length; i++)
                {
                */
                for (Int32 i = 0; i < bitmap_int32.Length; i++)
                {
                    Int32[] column_int32 = ArrayFunctions.Column.Take(bitmap_int32, i);
                    if (column_int32.Contains(mark_color.ToArgb()) == true)
                    {
                        mark_values.Add(i);
                        bitmap_to_work.SetPixel(i, Array.IndexOf(column_int32, mark_color.ToArgb()), color_background);
                    }
                }
                mark_values.Add(bitmap_to_work.Width - 1);
                Rectangle[] rectangles_crop = new Rectangle[mark_values.Count - 1];
                for (Int32 i = 1; i < mark_values.Count; i++)
                {
                    rectangles_crop[i - 1] = new Rectangle(new Point(mark_values[i - 1], 0), new Size(mark_values[i] - mark_values[i - 1], bitmap_to_work.Height));
                }
                Bitmap[] bitmaps_crop = MultipleRectangles(bitmap_to_work, rectangles_crop);
                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(HorizontallyByMark), execution_time_ms_stop - execution_time_ms_start);
                }
                return bitmaps_crop;
            }
            /// <summary>
            /// Return Bitmap[] after cropping image using provided crop areas array. <br></br>
            /// Written. 2024.02.11 11:39. Warsaw. Hostel.
            /// </summary>
            /// <param name="image_in"></param>
            /// <param name="rectangles_in"></param>
            /// <returns></returns>
            public static Bitmap[] MultipleRectangles(Bitmap image_in, Rectangle[] rectangles_in)
            {
                Bitmap[] bitmap_arr_out = new Bitmap[rectangles_in.Length];
                for (Int32 i = 0; i < bitmap_arr_out.Length; i++)
                {
                    bitmap_arr_out[i] = Rectangle(image_in, rectangles_in[i]);
                }
                return bitmap_arr_out;
            }
            /// <summary>
            /// Return part of Bitmap. <br></br>
            /// Written. 2024.02.10 18:50. Warsaw. Hostel. <br></br>
            /// Tested. Works. 2024.02.10 18:56. Warsaw. Hostel. 
            /// </summary>
            /// <param name="image_in"></param>
            /// <param name="rectangle_in"></param>
            /// <returns></returns>
            public static Bitmap Rectangle(Bitmap image_in, Rectangle rectangle_in)
            {
                return Rectangle(image_in, (uint)rectangle_in.Left, (uint)rectangle_in.Top, (uint)rectangle_in.Width, (uint)rectangle_in.Height);
            }
            /// <summary>
            /// Return part of Bitmap. <br></br>
            /// Written. 2023.11.08 21:06. Warsaw. Workplace. <br></br> 
            /// Tested. Works. 2024.02.10 17:43. Warsaw. Hostel.
            /// </summary>
            /// <param name="image_in"></param>
            /// <param name="w_start"></param>
            /// <param name="h_start"></param>
            /// <param name="w_size"></param>
            /// <param name="h_size"></param>
            /// <returns></returns>
            public static Bitmap Rectangle(Bitmap image_in, UInt32 w_start, UInt32 h_start, UInt32 w_size, UInt32 h_size)
            {
                // 2024.02.10 17:48. Warsaw. Hostel.
                // There static class Crop. There were no need to have class and therefore 
                // Crop is moved to MyImageFunctions.
                Int32[][] arr_image = BitmapToInt32ArrayAxB(image_in);
                Int32[][] arr_part_image = ArrayFunctions.Extract.PartAxBFromCxD(arr_image, w_start, w_size, h_start, h_size);
                return Int32ArrayAxBToBitmap(arr_part_image);
            }
        }
        /// <summary>
        /// Written. 2023.11.08 21:04. Warsaw. Workplace. 
        /// Maybe from class to functions. There is no need for class Crop. 2024.02.10 13:09. Warsaw. Hostel 
        /// </summary>
        public static class NotInUse
        {
            /// <summary>
            /// Written. 2023.11.08 21:05. Warsaw. Workplace. 
            /// Not Tesed. 2024.02.10 17:46. Warsaw. Hostel.
            /// There is MyArrayFunctions to with array. There is currently no need in this function.
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="w_start"></param>
            /// <param name="h_start"></param>
            /// <param name="w_size"></param>
            /// <param name="h_size"></param>
            /// <returns></returns>
            public static Bitmap FromInt32Array(Int32[][] arr_in, UInt32 w_start, UInt32 h_start, UInt32 w_size, UInt32 h_size)
            {
                Int32[][] arr_part_image = ArrayFunctions.Extract.PartAxBFromCxD(arr_in, w_start, w_size, h_start, h_size);
                return Int32ArrayAxBToBitmap(arr_part_image);
            }
        }
        /// <summary>
        /// Written. 2024.03.10 19:58. Warsaw. Hostel. 
        /// </summary>
        public static class SpecialConversion
        {
            /// <summary>
            /// Written. 2024.03.10 20:05. Warsaw. Hostel.
            /// Tested. Works. 2024.03.10 20:40. Warsaw. Hostel. 
            /// </summary>
            /// <param name="bitmap_in"></param>
            /// <param name="chars_in"></param>
            /// <returns></returns>
            public static char[][] BitmapToText(Bitmap bitmap_in, char[] chars_in)
            {
                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }
                Int32 level_count = chars_in.Length;
                Int32 level_of_color = 255 / level_count;
                Bitmap bitmap_black_white = Convert.ToBlackWhiteBitmap(bitmap_in);
                char[][] char_arr_out = new char[bitmap_in.Width][];
                for (Int32 i = 0; i < char_arr_out.Length; i++)
                {
                    char_arr_out[i] = new char[bitmap_in.Height];
                    for (Int32 j = 0; j < char_arr_out[i].Length; j++)
                    {
                        Color pixel_color = bitmap_black_white.GetPixel(i, j);
                        // 2024.03.10 20:14. Warsaw. Hostel. 
                        // Math is good but with Int32 and lost of accuracy because of
                        // devision causes the code work not correctly.
                        Int32 char_of_color = 1;
                        bool is_level_found = false;
                        while (is_level_found == false)
                        {
                            if ((level_of_color * char_of_color) < ((int)pixel_color.B))
                            {
                                char_of_color += 1;
                            }
                            else
                            {
                                is_level_found = true;
                            }
                            if (char_of_color >= chars_in.Length)
                            {
                                is_level_found = true;
                            }
                        }
                        if (is_level_found == false)
                        {
                            ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Length_is_Wrong);
                            return new char[0][];
                        }
                        char_arr_out[i][j] = chars_in[char_of_color - 1];
                    }
                }
                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(BitmapToText), execution_time_ms_stop - execution_time_ms_start);
                }
                return char_arr_out;
            }
        }
        /// <summary>
        /// Written. 2023.11.08 20:13. Warsaw. Workplace. 
        /// </summary>
        public static class Convert
        {
            /// <summary>
            /// Written. 2024.03.07 12:06. Warsaw. Workplace. <br></br>
            /// Tested. Works. 2024.03.07 12:28. Warsaw. Workplace. <br></br> 
            /// <br></br>
            /// Note. 1600x900. 5s execution time with 3 times GetPixel. 2024.03.07 12:25. Warsaw. Workplace. <br></br>
            /// 2.5s execution time with 1 time GetPixel <br></br>
            /// 15-20 ms for 100x100 image. 2024.03.07 12:43. Warsaw. Workplace. 
            /// </summary>
            /// <param name="image_in"></param>
            /// <returns></returns>
            static public Bitmap ToBlackWhiteBitmap(Bitmap image_in)
            {
                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }
                Bitmap bitmap_out = new Bitmap(image_in.Width, image_in.Height);
                for (Int32 i = 0; i < image_in.Height; i++)
                {
                    for (Int32 j = 0; j < image_in.Width; j++)
                    {
                        Color pixel_color = image_in.GetPixel(j, i);
                        byte average_pixels =
                            (byte)(((uint)pixel_color.R +
                            (uint)pixel_color.G +
                            (uint)pixel_color.B) / 3);
                        bitmap_out.SetPixel(j, i, Color.FromArgb(average_pixels, average_pixels, average_pixels));
                    }
                }
                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(ToBlackWhiteBitmap), execution_time_ms_stop - execution_time_ms_start);
                }
                return bitmap_out;
            }
            /// <summary>
            /// Written. 2024.03.07 07:46. Warsaw. Workplace. <br></br>
            /// Tested. Works. 2024.03.07 07:53. Warsaw. Workplace. 
            /// </summary>
            /// <param name="image_in"></param>
            /// <returns></returns>
            static public Color[] BitmapToColorArray(Bitmap image_in)
            {
                Color[] pixels_out = new Color[image_in.Width * image_in.Height];
                for (Int32 i = 0; i < image_in.Height; i++)
                {
                    for (Int32 j = 0; j < image_in.Width; j++)
                    {
                        pixels_out[i * image_in.Width + j] = image_in.GetPixel(j, i);
                    }
                }
                return pixels_out;
            }
            /// <summary>
            /// Written. 2024.03.06 17:07. Warsaw. Workplace. 
            /// </summary>
            /// <param name="image_in"></param>
            /// <param name="alpha_ch"></param>
            /// <returns></returns>
            public static Int32[] BitmapToInt32Array(Bitmap image_in, Int32 alpha_ch = -1)
            {
                float execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                }
                Int32[][] bitmap_int32_2D_array = BitmapToInt32ArrayAxB(image_in, alpha_ch);
                Int32[] arr_out = ArrayFunctions.Int32Array.Merge.AxB_To_C(bitmap_int32_2D_array);
                if (TimeExecutionShow == true)
                {
                    float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(BitmapToInt32Array), execution_time_ms_stop - execution_time_ms_start);
                }
                return arr_out;
            }
            /// <summary>
            /// Tested. Works. 2024.02.09 10:35. Warsaw. Workplace. <br></br>
            /// Modified. Added alpha_ch. 2024.02.14 13:13. Warsaw. Workplace. 
            /// </summary>
            /// <param name="image_in"></param>
            /// <returns></returns>
            public static Int32[][] BitmapToInt32ArrayAxB(Bitmap image_in, Int32 alpha_ch = -1)
            {
                Int32[][] arr_out = new Int32[image_in.Width][];
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    arr_out[i] = new Int32[image_in.Height];
                    for (Int32 j = 0; j < arr_out[i].Length; j++)
                    {
                        Color pixel_color = image_in.GetPixel(i, j);
                        arr_out[i][j] = pixel_color.ToArgb();
                        if (alpha_ch != -1)
                        {
                            arr_out[i][j] = Color.FromArgb(alpha_ch, (int)pixel_color.R, (int)pixel_color.G, (int)pixel_color.B).ToArgb();
                        }
                    }
                }
                return arr_out;
            }
            /// <summary>
            /// Written. 2024.02.09 09:33. Warsaw. Workplace. 
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static Bitmap ColorArrayToBitmap(Color[][] arr_in)
            {
                Bitmap image_out = new Bitmap(arr_in.Length, arr_in[0].Length);
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    for (Int32 j = 0; j < arr_in[i].Length; j++)
                    {
                        image_out.SetPixel(i, j, arr_in[i][j]);
                    }
                }
                return image_out;
            }
            /// <summary>
            /// Written. 2024.02.09 09:37. Warsaw. Workplace. 
            /// </summary>
            /// <param name="color_arr_in"></param>
            /// <returns></returns>
            public static Bitmap ColorArrayToBitmap(Color[] color_arr_in, Int32 row_length)
            {
                Bitmap bitmap_out = new Bitmap(row_length, (color_arr_in.Length / row_length));
                Int32 rows_count = (color_arr_in.Length / row_length);
                for (Int32 j = 0; j < rows_count; j++)
                {
                    for (Int32 i = 0; i < row_length; i++)
                    {
                        bitmap_out.SetPixel(i, j, color_arr_in[j * row_length + i]);
                    }
                }
                return bitmap_out;
            }
            /// <summary>
            /// Written. 2023.11.08 20:26. Warsaw. Workplace. 
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static Bitmap Int32ArrayToBitmap(Int32[][] arr_in)
            {
                Bitmap image_out = new Bitmap(arr_in.Length, arr_in[0].Length);
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    for (Int32 j = 0; j < arr_in[i].Length; j++)
                    {
                        image_out.SetPixel(i, j, Color.FromArgb(arr_in[i][j]));
                    }
                }
                return image_out;
            }
        }
        /// <summary>
        /// Written. Warsaw. Hostel. 2024.04.21 21:36.
        /// Tested. Works. Warsaw. Hostel. 2024.04.21 21:39. 
        /// </summary>
        /// <param name="bitmap_in"></param>
        /// <returns></returns>
        public static Bitmap Copy(Bitmap bitmap_in)
        {
            float execution_time_ms_start = 0;
            if (TimeExecutionShow == true)
            {
                execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
            }
            Bitmap bitmap_out = new Bitmap(bitmap_in);
            if (TimeExecutionShow == true)
            {
                float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                TimeExecutionMessage(nameof(ImageFunctions.Copy), execution_time_ms_stop - execution_time_ms_start);
            }
            return bitmap_out;
        }
        public static Bitmap Screenshot()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            }
            return bitmap;
        }
        /// <summary>
        /// JPG file as byte array to JPG file
        /// 2023.8.12 15:43
        /// </summary>
        /// <param name="arr_in"></param>
        /// <param name="filename"></param>
        static public void FileJPGByteArrayToFileJPG(byte[] arr_in, string filename)
        {
            try
            {
                FileStream file_read = System.IO.File.Open(filename, FileMode.OpenOrCreate);
                file_read.Write(arr_in, 0, arr_in.Length);
                file_read.Close();
            }
            catch
            {
                ReportFunctions.ReportError();
            }
        }
        static public Color Int32ToColor(Int32 value_in)
        {
            return Color.FromArgb(value_in);
        }
        static public Color Int32ToColor(byte[] bytes_in)
        {
            Int32 value = MathFunctions.Int32Number.BytesToInt32(bytes_in);
            return Color.FromArgb(value);
        }
        static public Color[][] Int32ArrayAxBToColorsAxB(Int32[][] values_in)
        {
            Color[][] colors_out = new Color[values_in.Length][];
            for (Int32 i = 0; i < values_in.Length; i++)
            {
                colors_out[i] = new Color[values_in[i].Length];
                for (Int32 j = 0; j < values_in[i].Length; j++)
                {
                    colors_out[i][j] = Color.FromArgb(values_in[i][j]);
                }
            }
            return colors_out;
        }
        static public void BitmapToConsole(string filename, Int32 spaces = 5)
        {
            Bitmap bitmap_from_file = FileBMPToBitmap(filename);
            BitmapToConsole(bitmap_from_file, spaces);
        }
        static public void BitmapToConsole(Bitmap image_in, Int32 spaces = 5)
        {
            Color[][] colors = BitmapToColorArrayAxB(image_in);
            ColorArrayAxBToConsole(colors, spaces);
        }
        static public void ColorArrayAxBToConsole(Int32[][] colors_in, Int32 spaces = 5)
        {
            ColorArrayAxBToConsole(Int32ArrayAxBToColorsAxB(colors_in));
        }
        static public void ColorArrayAxBToConsole(Color[][] colors_in, Int32 spaces = 5)
        {
            if (colors_in.Length == 0)
            {
                ReportFunctions.ReportError("Trouble. No colors\r\n" + "Colors Height is " + colors_in.Length.ToString() + "\r\n");
                return;
            }
            string str_out = "";
            str_out += "Colors. Height " + colors_in.Length.ToString() + ". Width " + colors_in[0].Length.ToString() + ".\r\n";
            string spaces_str = "".PadRight(5, ' ');
            for (Int32 i = 0; i < colors_in.Length; i++)
            {
                if (i != 0)
                {
                    str_out += "\r\n";
                }
                for (Int32 j = 0; j < colors_in[i].Length; j++)
                {
                    str_out += colors_in[i][j].ToArgb().ToString().PadLeft(10, ' ') + spaces_str;
                }
            }
            str_out += "\r\n";
            str_out += "\r\n";
            Console.Write(str_out);
        }
        static public void ColorToConsole(Color color_in)
        {
            Console.WriteLine("Color " + color_in.ToArgb().ToString());
            Console.WriteLine("Color bytes " + color_in.A.ToString() +
                color_in.R.ToString() + color_in.G.ToString() + color_in.B.ToString());
            Console.WriteLine();
        }
        static public Bitmap Int32ArrayAxBToBitmap(Int32[][] values_in)
        {
            if (values_in.Length == 0)
            {
                ReportFunctions.ReportError("Array is empty. Array size is " + values_in.Length.ToString());
                // Bitmap(0,0) gives error that such values can not be used. 2024.02.10 19:13. Warsaw. Hostel.
                return new Bitmap(1, 1);
            }
            Bitmap image_out = new Bitmap(values_in[0].Length, values_in.Length);
            for (Int32 i = 0; i < image_out.Height; i++)
            {
                for (Int32 j = 0; j < image_out.Width; j++)
                {
                    image_out.SetPixel(j, i, Color.FromArgb(values_in[i][j]));
                }
            }
            return image_out;
        }
        static public Int32[][] BitmapToInt32ArrayAxB(Bitmap image_in)
        {
            Int32[][] pixels_out = new Int32[image_in.Height][];
            for (Int32 i = 0; i < image_in.Height; i++)
            {
                pixels_out[i] = new Int32[image_in.Width];
                for (Int32 j = 0; j < image_in.Width; j++)
                {
                    pixels_out[i][j] = image_in.GetPixel(j, i).ToArgb();
                }
            }
            return pixels_out;
        }
        static public Color[][] BitmapToColorArrayAxB(Bitmap image_in)
        {
            Color[][] pixels_out = new Color[image_in.Height][];
            for (Int32 i = 0; i < image_in.Height; i++)
            {
                pixels_out[i] = new Color[image_in.Width];
                for (Int32 j = 0; j < image_in.Width; j++)
                {
                    pixels_out[i][j] = image_in.GetPixel(j, i);
                }
            }
            return pixels_out;
        }
        static public byte[] BitmapToFileJPGByteArray(Bitmap bitmap_in)
        {
            byte[] arr_out = new byte[0];
            try
            {
                string filename = "picture" + "_" + nameof(BitmapToFileJPGByteArray) + ".jpg";
                BitmapToFileJPG(bitmap_in, filename);
                arr_out = FileJPGToFileJPGByteArray(filename);
            }
            catch
            {
                ReportFunctions.ReportError();
            }
            return arr_out;
        }
        // 2024.02.09 20:34. Warsaw. Hostel 
        [Obsolete]
        static public void BitmapToFileBMP(Bitmap bitmap_in, string filename)
        {
            // code moved. 2024.02.09 20:35. Warsaw. Hostel 
            FileFunctions.ImageFile.WriteFile.BitmapToFileBMP(bitmap_in, filename);
        }
        static public void BitmapToFileJPG(Bitmap bitmap_in, string filename)
        {
            try
            {
                ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder myEncoder =
                System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder,
                    90L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                //  img.Save(filename, jgpEncoder,   myEncoderParameters);
                bitmap_in.Save(filename, jgpEncoder, myEncoderParameters);
            }
            catch
            {
                ReportFunctions.ReportError();
            }
        }
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        static public byte[] FileJPGToFileJPGByteArray(string filename)
        {
            byte[] arr_out = new byte[0];
            try
            {
                FileStream file_read = System.IO.File.Open(filename, FileMode.OpenOrCreate);
                arr_out = new byte[file_read.Length];
                file_read.Read(arr_out, 0, arr_out.Length);
                file_read.Close();
            }
            catch
            {
                ReportFunctions.ReportError();
            }
            return arr_out;
        }
        public static Bitmap JPGByteArrayToBitmap(byte[] arr_in)
        {
            // not tested 2023-01-24 16:03
            Bitmap bitmap_for_return = null;
            try
            {
                MemoryStream memory_write = new MemoryStream();
                memory_write.Write(arr_in, 0, (int)arr_in.Length);
                bitmap_for_return = Bitmap.FromStream(memory_write) as Bitmap;
            }
            catch
            {
                ReportFunctions.ReportError();
            }
            return bitmap_for_return;
        }
        /// <summary>
        /// JPG image as byte array to Bitmap
        /// 2023.8.12 15:36
        /// </summary>
        /// <param name="arr_in"></param>
        /// <returns></returns>
        public static Bitmap JPGFileByteArrayToBitmap(byte[] arr_in)
        {
            Bitmap bitmap_for_return = null;
            try
            {
                MemoryStream memory_write = new MemoryStream();
                memory_write.Write(arr_in, 0, (int)arr_in.Length);
                bitmap_for_return = Bitmap.FromStream(memory_write) as Bitmap;
            }
            catch
            {
                ReportFunctions.ReportError();
            }
            return bitmap_for_return;
        }
        //public static byte[] BitmapToFileJPGByteArray(Bitmap bitmap_in)
        //{
        //    byte[] arr_out = new byte[0];
        //    try
        //    {
        //        using (MemoryStream memory_write = new MemoryStream())
        //        {
        //            bitmap_in.Save(memory_write, System.Drawing.Imaging.ImageFormat.Bmp);
        //            arr_out = new byte[memory_write.Length];
        //            memory_write.Read(arr_out, 0, (int)memory_write.Length);
        //        }
        //    }
        //    catch
        //    {
        //        MyReportErrorMethods.ReportError();
        //    }
        //    return arr_out;
        //}
        /// <summary>
        /// Tested. Works. 2024.03.06 16:58. Warsaw. Workplace.<br></br>
        /// It keeps file not available for modification. 2024.03.07 07:52. Warsaw. Workplace. 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        static public Bitmap FileBMPToBitmap(string filename)
        {
            if (System.IO.File.Exists(filename) == false)
            {
                ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.FileDoesNotExist);
                return ImageFunctions.Generate.Rectungular_Checkboard(100, 100);
            }
            float execution_time_ms_start = 0;
            if (TimeExecutionShow == true)
            {
                execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
            }
            Bitmap bitmap_return = new Bitmap(filename);
            if (TimeExecutionShow == true)
            {
                float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                TimeExecutionMessage(nameof(FileBMPToBitmap), execution_time_ms_stop - execution_time_ms_start);
            }
            return bitmap_return;
        }
    }
}
