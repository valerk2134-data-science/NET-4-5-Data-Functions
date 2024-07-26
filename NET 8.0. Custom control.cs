using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Http.Headers;
using ImageFunctionsNameSpace;
using System.Data.Common;
using Timer = System.Windows.Forms.Timer;

// The code is available publicly. The default copyright is applied.

namespace CustomControlNamespace
{

	// 2024.06.10 15:50. Warsaw. Workplace.
	// Importance 3. C# requires certain amount of code for designer -
	// to see shape, color visualized in designer.


	/// <summary>
	/// Written. 2024.06.13 16:32. Gdansk. Home 
	/// </summary>
	public static class SupportFunctions
	{
		/// <summary>
		/// Written. 2024.06.13 16:32. Gdansk. Home 
		/// </summary>
		/// <param name="graphics_path_in"></param>
		/// <returns></returns>
		public static bool[][] GraphicPathPixels(GraphicsPath graphics_path_in, Size size_in)
		{
			Bitmap image_from_path = new Bitmap(size_in.Width, size_in.Height);
			using (Graphics gr = Graphics.FromImage(image_from_path))
			{
				gr.FillRectangle(Brushes.White, new Rectangle(0, 0, image_from_path.Width, image_from_path.Height));
				gr.DrawPath(Pens.Black, graphics_path_in);
			}
			uint top_length = ImageFunctions.Find.LengthOfBackground.Top(image_from_path, Color.White);

			uint bottom_length = ImageFunctions.Find.LengthOfBackground.Bottom(image_from_path, Color.White);

			uint left_length = ImageFunctions.Find.LengthOfBackground.Left(image_from_path, Color.White);

			uint right_length = ImageFunctions.Find.LengthOfBackground.Right(image_from_path, Color.White);

			int[][] arr = ImageFunctions.Convert.BitmapToInt32ArrayAxB(image_from_path, 0);
		//	ImageFunctions.ToPictureBox.FromBitmap(image_from_path, Form.ActiveForm);

			bool[][] arr_out = new bool[arr.Length][];

			for (int i = 0; i < arr.Length; i++)
			{
				arr_out[i] = new bool[arr[i].Length];
				for (int j = 0; j < arr[i].Length; j++)
				{
					arr_out[i][j] = false;
					if (arr[i][j] == 0)
					{
						arr_out[i][j] = true;
					}
				}
			}


			return arr_out;
		}
	}



    /// <summary>
    /// Written. 2024.06.10 14:57. Warsaw. Workplace.
    /// </summary>
    public static class ColorOfControl
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
    /// Written. 2024.06.03 10:00 - 15:00. Warsaw. Workplace.
    /// Tested. Works. 2024.06.03 15:59. Warsaw. Workplace.
    /// </summary>
    class TileArrayVertical : Control
	{
		// 2024.06.03 10:34. Warsaw. Workplace. 
		// There is no need to use PictureBox. Control with Backcolor changed will do the same.
		// Note. Although there is support of transparency in backcolor, it will show some of the form
		// while there is fill with transparent color and background.





		// 2024.06.03 10:06. Warsaw. Workplace.
		// For InitializeComponent();
		// It is not predictable in which order properties are set in InitializeComponent();
		// Therefore there is 1 by default PictureBox to not cause error - object is null.

		// 2024.06.03 13:19. Warsaw. Workplace.
		// PictureBox[] TileArray was replaced with TileArray[]

		public Control[] TileArray = new Control[1];

		// 2024.06.03 10:07. Warsaw. Workplace.
		// Furthermore. There is counter of how many properties are set and after the defined number 
		// there will be pictureboxes added to control.
		int _property_set_count = 0;

		/*
		public void Draw(int chain_size = 5)
		{
			Draw(FindForm(), chain_size);
		}

		public void Draw(Form form_that_added, int chain_size = 5)
		{
			FormCalled = form_that_added;
			this.Hide();

			ArrayOfPictureBoxes = new PictureBox[chain_size];
			for (int i = 0; i < ArrayOfPictureBoxes.Length; i++)
			{
				ArrayOfPictureBoxes[i] = new PictureBox();
				ArrayOfPictureBoxes[i].BorderStyle = BorderStyle.FixedSingle;
				FormCalled.Controls.Add(ArrayOfPictureBoxes[i]);
			}
			SizeSet();
			LocationSet();
		}
		*/

		protected override void OnPaint(PaintEventArgs e)
		{

		}


		int _count = 5;
		[Category("Chain property")]
		[DefaultValue(5)]
		public int Count
		{
			get { return _count; }
			set
			{
				_count = value;
				_property_set_count += 1;
				if (_property_set_count >= 4)
				{
					CreatePictureBoxes();
					SizeSet();
					LocationSet();
					BackcolorSet();
				}
			}
		}

		Color _background_color = Color.White;
		[Category("Chain property")]

		public Color BackgroundColor
		{
			get { return _background_color; }
			set
			{
				_background_color = value;
				_property_set_count += 1;
				if (_property_set_count >= 4)
				{
					CreatePictureBoxes();
					SizeSet();
					LocationSet();
					BackcolorSet();
				}
			}
		}

		Color _switch_color = Color.SlateBlue;
		[Category("Chain property")]

		public Color SwitchColor
		{
			get { return _switch_color; }
			set
			{
				_switch_color = value;
			}
		}




		public TileArrayVertical()
		{
			TileArray[0] = new Control();
			this.SizeChanged += PictureBoxChain_SizeChanged;
			TimerSwitch.Tick += TimerSwitch_Tick;
		}

		private void TileMouseDown(object sender, MouseEventArgs e)
		{
			Control tile_clicked = (Control)sender;
			if (_tile_current.ToString() != tile_clicked.Name)
			{
				Debug.WriteLine(tile_clicked.Name);
			}
			else
			{
				Debug.WriteLine("Hit on " + tile_clicked.Name);
			}
		}

		int _tile_current = 0;
		bool _bounce_end = false;
		private void TimerSwitch_Tick(object sender, EventArgs e)
		{
			if (Direction == DirectionList.UpToDown)
			{
				StepDown();
				if (_tile_current > TileArray.Length - 1)
				{
					_tile_current = 0;
				}
				return;
			}

			if (Direction == DirectionList.DownToUp)
			{
				StepUp();
				if (_tile_current < 0)
				{
					_tile_current = TileArray.Length - 1;
				}
				return;
			}

			// 2024.06.03 15:07. Warsaw. Workplace.
			// There is double time to wait when the border is reached.
			// It is noticable if there is slow switch time 0.5s or more.
			if (Direction == DirectionList.Bouncing)
			{
				if (_bounce_end == false)
				{
					StepDown();
					if (_tile_current > TileArray.Length - 1)
					{
						_tile_current = TileArray.Length - 1;
						_bounce_end = true;
					}
				}
				else
				{
					StepUp();
					if (_tile_current < 0)
					{
						_tile_current = 0;
						_bounce_end = false;
					}
				}
				return;
			}
		}

		private void StepUp()
		{

			TileArray[_tile_current].BackColor = SwitchColor;
			if (_tile_current != TileArray.Length - 1)
			{
				TileArray[_tile_current + 1].BackColor = BackgroundColor;
			}
			else
			{
				TileArray[0].BackColor = BackgroundColor;
			}

			_tile_current -= 1;

			return;

		}


		private void StepDown()
		{

			TileArray[_tile_current].BackColor = SwitchColor;
			if (_tile_current != 0)
			{
				TileArray[_tile_current - 1].BackColor = BackgroundColor;
			}
			else
			{
				TileArray[TileArray.Length - 1].BackColor = BackgroundColor;
			}

			_tile_current += 1;

			return;

		}


		private void PictureBoxChain_SizeChanged(object sender, EventArgs e)
		{
			_property_set_count += 1;
			if (_property_set_count >= 4)
			{
				CreatePictureBoxes();
				SizeSet();
				LocationSet();
				BackcolorSet();
			}
		}


		Timer TimerSwitch = new Timer();
		int _timer_interval = 200;
		[Category("Chain property")]
		[DefaultValue(200)]
		public int TimerInterval
		{
			get { return _timer_interval; }
			set
			{
				_timer_interval = value;
				TimerSwitch.Interval = value;
			}
		}

		public void Start()
		{
			TimerSwitch.Start();
		}


		Size _size_of_picturebox = new Size(40, 20);
		[Category("Chain property")]

		public Size SizeOfPictureBox
		{
			get { return _size_of_picturebox; }
			set
			{
				_size_of_picturebox = value;
				_property_set_count += 1;
				if (_property_set_count >= 4)
				{
					CreatePictureBoxes();
					SizeSet();
					LocationSet();
					BackcolorSet();
				}
			}
		}

		public enum DirectionList
		{
			UpToDown,
			DownToUp,
			Bouncing
		}
		DirectionList _direction = DirectionList.UpToDown;
		[Category("Chain property")]
		[DefaultValue(DirectionList.UpToDown)]
		public DirectionList Direction
		{
			get { return _direction; }
			set
			{
				_direction = value;
				if (value == DirectionList.UpToDown)
				{
					_tile_current = 0;
				}
				if (value == DirectionList.DownToUp)
				{
					_tile_current = TileArray.Length - 1;
				}
				if (value == DirectionList.Bouncing)
				{
					_tile_current = 0;
				}
			}
		}



		private void CreatePictureBoxes()
		{

			// for C# designer. 2024.06.03 13:14. Warsaw. Workplace.
			for (int i = 0; i < TileArray.Length; i++)
			{
				this.Controls.Remove(TileArray[i]);
			}
			// 2024.06.03 13:15. Warsaw. Workplace.
			// With the following code it was checked that there is no error in removing control while it has been removed.
			/*
			for (int i = 0; i < TileArray.Length; i++)
			{
				this.Controls.Remove(TileArray[i]);
			}
			for (int i = 0; i < TileArray.Length; i++)
			{
				this.Controls.Remove(TileArray[i]);
			}
			*/


			TileArray = new Control[_count];
			for (int i = 0; i < TileArray.Length; i++)
			{
				TileArray[i] = new Control();
				TileArray[i].Name = i.ToString();
				TileArray[i].MouseDown += TileMouseDown;
				this.Controls.Add(TileArray[i]);
			}
		}

		private void BackcolorSet()
		{
			for (int i = 0; i < TileArray.Length; i++)
			{
				TileArray[i].BackColor = _background_color;
			}
		}

		private void SizeSet()
		{
			_size_of_picturebox = new Size(this.Width, (this.Size.Height / Count) - _space_between);
			for (int i = 0; i < TileArray.Length; i++)
			{
				TileArray[i].Size = _size_of_picturebox;
			}
		}

		int _space_between = 3;
		private void LocationSet()
		{
			for (int i = 0; i < TileArray.Length; i++)
			{
				TileArray[i].Location = new Point(0, (_size_of_picturebox.Height + _space_between) * i);
			}
		}
	}



	/// <summary>
	/// Written. 2024.06.03 16:02. Warsaw. Workplace.
	/// Tested. Works. 2024.06.03 16:11. Warsaw. Workplace.
	/// </summary>
	class TileArrayHorizontal : Control
	{
		public Control[] TileArray = new Control[1];
		int _property_set_count = 0;

		int _count = 5;
		[Category("Chain property")]
		[DefaultValue(5)]
		public int Count
		{
			get { return _count; }
			set
			{
				_count = value;
				_property_set_count += 1;
				if (_property_set_count >= 4)
				{
					CreatePictureBoxes();
					SizeSet();
					LocationSet();
					BackcolorSet();
				}
			}
		}

		Color _background_color = Color.White;
		[Category("Chain property")]
		public Color BackgroundColor
		{
			get { return _background_color; }
			set
			{
				_background_color = value;
				_property_set_count += 1;
				if (_property_set_count >= 4)
				{
					CreatePictureBoxes();
					SizeSet();
					LocationSet();
					BackcolorSet();
				}
			}
		}

		Color _switch_color = Color.SlateBlue;
		[Category("Chain property")]
		public Color SwitchColor
		{
			get { return _switch_color; }
			set
			{
				_switch_color = value;
			}
		}

		public TileArrayHorizontal()
		{
			TileArray[0] = new Control();
			this.SizeChanged += PictureBoxChain_SizeChanged;
			TimerSwitch.Tick += TimerSwitch_Tick;
		}

		private void TileMouseDown(object sender, MouseEventArgs e)
		{
			Control tile_clicked = (Control)sender;
			if (_tile_current.ToString() != tile_clicked.Name)
			{
				Debug.WriteLine(tile_clicked.Name);
			}
			else
			{
				Debug.WriteLine("Hit on " + tile_clicked.Name);
			}
		}

		int _tile_current = 0;
		bool _bounce_end = false;
		private void TimerSwitch_Tick(object sender, EventArgs e)
		{
			if (Direction == DirectionList.LeftToRight)
			{
				StepRight();
				if (_tile_current > TileArray.Length - 1)
				{
					_tile_current = 0;
				}
				return;
			}

			if (Direction == DirectionList.RightToLeft)
			{
				StepLeft();
				if (_tile_current < 0)
				{
					_tile_current = TileArray.Length - 1;
				}
				return;
			}

			if (Direction == DirectionList.Bouncing)
			{
				if (_bounce_end == false)
				{
					StepRight();
					if (_tile_current > TileArray.Length - 1)
					{
						_tile_current = TileArray.Length - 1;
						_bounce_end = true;
					}
				}
				else
				{
					StepLeft();
					if (_tile_current < 0)
					{
						_tile_current = 0;
						_bounce_end = false;
					}
				}
				return;
			}
		}

		private void StepLeft()
		{
			TileArray[_tile_current].BackColor = SwitchColor;
			if (_tile_current != TileArray.Length - 1)
			{
				TileArray[_tile_current + 1].BackColor = BackgroundColor;
			}
			else
			{
				TileArray[0].BackColor = BackgroundColor;
			}
			_tile_current -= 1;
			return;
		}

		private void StepRight()
		{
			TileArray[_tile_current].BackColor = SwitchColor;
			if (_tile_current != 0)
			{
				TileArray[_tile_current - 1].BackColor = BackgroundColor;
			}
			else
			{
				TileArray[TileArray.Length - 1].BackColor = BackgroundColor;
			}
			_tile_current += 1;
			return;
		}

		private void PictureBoxChain_SizeChanged(object sender, EventArgs e)
		{
			_property_set_count += 1;
			if (_property_set_count >= 4)
			{
				CreatePictureBoxes();
				SizeSet();
				LocationSet();
				BackcolorSet();
			}
		}

		Timer TimerSwitch = new Timer();
		int _timer_interval = 200;
		[Category("Chain property")]
		[DefaultValue(200)]
		public int TimerInterval
		{
			get { return _timer_interval; }
			set
			{
				_timer_interval = value;
				TimerSwitch.Interval = value;
			}
		}

		public void Start()
		{
			TimerSwitch.Start();
		}

		Size _size_of_picturebox = new Size(40, 20);
		[Category("Chain property")]
		public Size SizeOfPictureBox
		{
			get { return _size_of_picturebox; }
			set
			{
				_size_of_picturebox = value;
				_property_set_count += 1;
				if (_property_set_count >= 4)
				{
					CreatePictureBoxes();
					SizeSet();
					LocationSet();
					BackcolorSet();
				}
			}
		}

		public enum DirectionList
		{
			LeftToRight,
			RightToLeft,
			Bouncing
		}
		DirectionList _direction = DirectionList.LeftToRight;
		[Category("Chain property")]
		[DefaultValue(DirectionList.LeftToRight)]
		public DirectionList Direction
		{
			get { return _direction; }
			set
			{
				_direction = value;
				if (value == DirectionList.LeftToRight)
				{
					_tile_current = 0;
				}
				if (value == DirectionList.RightToLeft)
				{
					_tile_current = TileArray.Length - 1;
				}
				if (value == DirectionList.Bouncing)
				{
					_tile_current = 0;
				}
			}
		}

		private void CreatePictureBoxes()
		{
			// for C# designer. 2024.06.03 16:04. Warsaw. Workplace.
			for (int i = 0; i < TileArray.Length; i++)
			{
				this.Controls.Remove(TileArray[i]);
			}

			TileArray = new Control[_count];
			for (int i = 0; i < TileArray.Length; i++)
			{
				TileArray[i] = new Control();
				TileArray[i].Name = i.ToString();
				TileArray[i].MouseDown += TileMouseDown;
				this.Controls.Add(TileArray[i]);
			}
		}

		private void BackcolorSet()
		{
			for (int i = 0; i < TileArray.Length; i++)
			{
				TileArray[i].BackColor = _background_color;
			}
		}

		private void SizeSet()
		{
			_size_of_picturebox = new Size((this.Size.Width / Count) - _space_between, this.Height);
			for (int i = 0; i < TileArray.Length; i++)
			{
				TileArray[i].Size = _size_of_picturebox;
			}
		}

		int _space_between = 3;
		private void LocationSet()
		{
			for (int i = 0; i < TileArray.Length; i++)
			{
				TileArray[i].Location = new Point((_size_of_picturebox.Width + _space_between) * i, 0);
			}
		}
	}









	/// <summary>
	///  Hexagon shape. Custom PictureBox with hexagon shape based on C# PictureBox. <br></br>
	///  Written. 2024.05.28 16:00 - 18:00. Warsaw. Workplace. <br></br>
	///  Tested. Works. 2024.05.29 14:21. Warsaw. Workplace. <br></br>
	///  Note. Despite the rounding that creates assymetry by 1-2 pixels, overall quality is good. Checked by combining 10 hexagons in pattern.
	/// </summary>
	class PicutreBoxHexagon : PictureBox
	{

		// 2024.05.29 16:53. Warsaw. Workplace.
		// Lost accuracy. 
		// Round because of casting to int goes to lower number irrelevantly what float is - 20.94 or 20.19
		// it will be rounded to 20 while rounding of float would be 21 and 20.
		// 2024.05.29 16:58. Warsaw. Workplace. Rounding increases accuracy of the shape.

		public static Point[] Points(int height_of_hexagon)
		{
			// In work. 2024.05.29 17:55. Rounding of corner at 3rd dot.
			// note. Polygon needs 3 dots.
			Point[] points_arr_out = new Point[6];
			float width_of_hexagon = ((float)height_of_hexagon * (float)System.Math.Sqrt(3)) / (float)2;
			float half_of_width = width_of_hexagon / (float)2;
			//int half_of_width = width_of_hexagon / 2;
			float length_from_side = half_of_width / (float)System.Math.Sqrt(3);

			points_arr_out[0] = new Point(0, (int)half_of_width - 1);
			points_arr_out[1] = new Point((int)length_from_side - 1, 0);
			// 2024.05.29 17:55. Warsaw. Workplace.
			// +2 fixes angle of the side.
			points_arr_out[2] = new Point(height_of_hexagon - 1 - (int)length_from_side - 1 + 2, 0);
			points_arr_out[3] = new Point(height_of_hexagon - 1, (int)half_of_width - 1);
			points_arr_out[4] = new Point(height_of_hexagon - (int)length_from_side - 1, (int)half_of_width * 2 - 1);
			points_arr_out[5] = new Point((int)length_from_side - 1, (int)half_of_width * 2 - 1);
			return points_arr_out;
		}


		public PicutreBoxHexagon()
		{
			this.BorderStyle = BorderStyle.None;
			this.SizeChanged += PicutreBoxHexagon_SizeChanged;
		}

		private Size _size_current = new Size(-1, -1);

		// 2024.05.29 10:39. Warsaw. Workplace.
		// for form designer.
		// 1. works well if height or width is changed.
		// 2. flickering during size change is seen if both are changed (change by using corner of picturebox).
		// note. it does not affect the shape. 
		private void PicutreBoxHexagon_SizeChanged(object sender, EventArgs e)
		{
			// 2024.05.29 10:32. Warsaw. Workplace.
			// for form designer.
			if (_size_current.Width == -1)
			{
				_size_current = this.Size;
			}

			// 2024.05.29 10:24. Warsaw. Workplace.
			// by the largest change in side
			this.SizeChanged -= PicutreBoxHexagon_SizeChanged;
			int width_is = this.Width;
			int height_is = this.Height;
			int width_change = _size_current.Width - width_is;
			int height_change = _size_current.Height - height_is;
			int size_is = this.Width;
			if (Math.Abs(width_change) < Math.Abs(height_change))
			{
				size_is = height_is;
			}
			int width_of_hexagon = (int)(((float)size_is * (float)System.Math.Sqrt(3)) / (float)2);

			// 2024.05.29 10:37. Warsaw. Workplace.
			// +1 is +1 pixel for height
			this.Size = new Size(size_is, width_of_hexagon);
			_size_current = this.Size;
			this.SizeChanged += PicutreBoxHexagon_SizeChanged;
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			GraphicsPath shape_path = new GraphicsPath();
			pe.Graphics.SmoothingMode = SmoothingMode.None;

			shape_path.AddPolygon(Points(this.Width));
			shape_path.CloseFigure();
			this.Region = new Region(shape_path);

			base.OnPaint(pe);
		}
	}





    /// <summary>
    ///  Round shape. Custom PictureBox with  based on C# PictureBox. <br></br>
    ///  Written. 2024.05.29 14:31. Warsaw. Workplace. <br></br>
    ///  Tested. Works. 2024.05.29 14:43. Warsaw. Workplace.
    /// </summary>
    class PicutreBoxRound : PictureBox
    {
        public PicutreBoxRound()
        {

            // 2024.06.25 23:34. Gdansk. Home. 
            // There is working solution to make round image at run time and at design time but it requires
            // reload image, close and open solution.
            // There is following: it loads full image, then there is round shape from image made and set to be image 
            // of picturebox and this round image is now in property and the initial image is lost.
            // closing and opening solution does not work because in the image propery round image is saved and loaded when solution is opened.



            // 2024.06.25 21:10. Gdansk. Home. 
            // changing region of picturebox was not selected as solution to have no trouble with drawing border


            // 2024.05.29 14:41. Warsaw. Workplace.
            // for form designer.
            // The initial shape is rectangle and it is drawn that way first time.
            // Second time the shape is round. The first time there is at the border seen dashes line
            // which I assume from 1st draw.
            // Border = FixedSingle allows to remove it.
            // this.BorderStyle = BorderStyle.FixedSingle;

            // 2024.05.29 15:38. Warsaw. Workplace.
            // Border trouble. C# Designer uses region area to draw border and 
            // if there is no border it places dashed border in the designer which will not be seen/shown
            // when form is run.
            // Important. The designer draws only vertical and horizontal border - if the region
            // has area to draw horizontal, vertical line then the border will be shown in the designer.

            // 2024.05.29 15:59. Warsaw. Workplace.
            // Usage of Width - 1, Height - 1 can be used to prevent seeing dahsed border.
            // note. seeing border at certain size comes from shape calculation and the need to make
            // the part as line because of rounding to nearest pixel - 99.6, 99.7, 99.8 and there is 1 dot that is 100
            // or may be there is no dot with 100 value because of the size of step.
            // These value I assume are rounded to 100 and the shape is drawn with flat line at the end.
            this.BorderStyle = BorderStyle.None;

            this.Size = new Size(100, 100);

            ImageSet = new Bitmap(this.Width, this.Height);
            using (Graphics gr = Graphics.FromImage(ImageSet))
            {
                gr.FillEllipse(Brushes.White, new Rectangle(0, 0, this.Width, this.Height));
            }
            this.Image = ImageSet;
            this.Paint += PicutreBoxRound_Paint;
            this.SizeChanged += PicutreBoxRound_SizeChanged;
            this.WaitOnLoad = true;
            //this.LoadCompleted += PicutreBoxRound_LoadCompleted;
        }

        private void PicutreBoxRound_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            PictureBoxDrawImage();
        }

        public Color BorderPenColor = Color.MediumSlateBlue;
        [Category("Border")]

        public Color BorderColor
        {
            get
            {
                return BorderPenColor;
            }
            set
            {
                BorderPenColor = value;
                //   PictureBoxDrawImage();
            }
        }


        bool _button_border = true;
        [Category("Border")]
        [Description("Sets if to show border")]
        [DefaultValue(true)]
        public bool BorderOn
        {
            get
            {
                return _button_border;
            }
            set
            {
                _button_border = value;
                //  PictureBoxDrawImage();
            }
        }



        int _border_width = 1;
        [Category("Border")]
        public int BorderWidth
        {
            get
            {
                return _border_width;
            }
            set
            {
                _border_width = value;
                //  PictureBoxDrawImage();
            }
        }

        public void PictureBoxDrawImage()
        {
            this.Load();
            GraphicsPath shape_path = new GraphicsPath();

            shape_path.AddEllipse(new Rectangle(0, 0, this.Width, this.Width));
            shape_path.CloseFigure();

            //	Image img = this.InitialImage;

            Brush brush_draw = new TextureBrush(this.Image);
            Bitmap image_for_box = new Bitmap(this.Width, this.Width);
            //	pe.Graphics.Clear(Color.FromArgb(0, Color.White));
            using (Graphics gr = Graphics.FromImage(image_for_box))
            {
                gr.Clear(Color.FromArgb(0, Color.White));
                gr.FillPath(brush_draw, shape_path);

                if (BorderOn == true)
                {
                    using (Pen pen_draw = new Pen(BorderColor, BorderWidth))
                    {
                        pen_draw.Alignment = PenAlignment.Inset;
                        gr.DrawPath(pen_draw, shape_path);
                    }
                }

            }



            this.Image = image_for_box;

        }

        Bitmap ImageSet = null;
        private void PicutreBoxRound_Paint(object sender, PaintEventArgs e)
        {


            this.Paint -= PicutreBoxRound_Paint;
            //ImageSet = new Bitmap(this.Image);
            //	ImageFunctions.ToFile.ToBMP(ImageSet, "123.bmp");
            PictureBoxDrawImage();


        }

        private void PicutreBoxRound_SizeChanged(object sender, EventArgs e)
        {
            this.SizeChanged -= PicutreBoxRound_SizeChanged;
            this.Size = new Size(this.Width, this.Width);
            this.SizeChanged += PicutreBoxRound_SizeChanged;
            PictureBoxDrawImage();

        }


    }



    /// <summary>
    ///  Round shape. Custom PictureBox with  based on C# PictureBox. <br></br>
    ///  Written. 2024.05.29 14:31. Warsaw. Workplace. <br></br>
    ///  Tested. Works. 2024.05.29 14:43. Warsaw. Workplace. <Br></Br>
    ///  Obsolete. 2024.06.26 14:34. Gdansk. Home. 
    /// </summary>
    [Obsolete]
    class PicutreBoxRound_v1 : PictureBox
	{
		public PicutreBoxRound_v1()
		{
			
			// 2024.06.25 23:34. Gdansk. Home. 
			// There is working solution to make round image at run time and at design time but it requires
			// reload image, close and open solution.
			// There is following: it loads full image, then there is round shape from image made and set to be image 
			// of picturebox and this round image is now in property and the initial image is lost.
			// closing and opening solution does not work because in the image propery round image is saved and loaded when solution is opened.
			
			
			
			// 2024.06.25 21:10. Gdansk. Home. 
			// changing region of picturebox was not selected as solution to have no trouble with drawing border
			
			
			// 2024.05.29 14:41. Warsaw. Workplace.
			// for form designer.
			// The initial shape is rectangle and it is drawn that way first time.
			// Second time the shape is round. The first time there is at the border seen dashes line
			// which I assume from 1st draw.
			// Border = FixedSingle allows to remove it.
			// this.BorderStyle = BorderStyle.FixedSingle;

			// 2024.05.29 15:38. Warsaw. Workplace.
			// Border trouble. C# Designer uses region area to draw border and 
			// if there is no border it places dashed border in the designer which will not be seen/shown
			// when form is run.
			// Important. The designer draws only vertical and horizontal border - if the region
			// has area to draw horizontal, vertical line then the border will be shown in the designer.

			// 2024.05.29 15:59. Warsaw. Workplace.
			// Usage of Width - 1, Height - 1 can be used to prevent seeing dahsed border.
			// note. seeing border at certain size comes from shape calculation and the need to make
			// the part as line because of rounding to nearest pixel - 99.6, 99.7, 99.8 and there is 1 dot that is 100
			// or may be there is no dot with 100 value because of the size of step.
			// These value I assume are rounded to 100 and the shape is drawn with flat line at the end.
			this.BorderStyle = BorderStyle.None;
		
			this.Size = new Size(100, 100);
            
            ImageSet = new Bitmap(this.Width, this.Height);
			using (Graphics gr = Graphics.FromImage(ImageSet))
			{
				gr.FillEllipse(Brushes.White, new Rectangle(0, 0, this.Width, this.Height));
			}
			this.Image = ImageSet;
            this.Paint += PicutreBoxRound_Paint;
           this.SizeChanged += PicutreBoxRound_SizeChanged;
            
		}

    

        public Color BorderPenColor = Color.MediumSlateBlue;
        [Category("Border")]

        public Color BorderColor
        {
            get
            {
                return BorderPenColor;
            }
            set
            {
                BorderPenColor = value;
             //   PictureBoxDrawImage();
            }
        }


        bool _button_border = true;
        [Category("Border")]
        [Description("Sets if to show border")]
        [DefaultValue(true)]
        public bool BorderOn
        {
            get
            {
                return _button_border;
            }
            set
            {
                _button_border = value;
              //  PictureBoxDrawImage();
            }
        }



		int _border_width = 1;
        [Category("Border")]
		public int BorderWidth
        {
            get
            {
                return _border_width;
            }
            set
            {
                _border_width = value;
              //  PictureBoxDrawImage();
            }
        }

		public void PictureBoxDrawImage()
		{
            GraphicsPath shape_path = new GraphicsPath();

            shape_path.AddEllipse(new Rectangle(0, 0, this.Width, this.Width));
            shape_path.CloseFigure();

			Image img = new Bitmap(ImageSet);

            Brush brush_draw = new TextureBrush(img);
            Bitmap image_for_box = new Bitmap(this.Width, this.Width);
            //	pe.Graphics.Clear(Color.FromArgb(0, Color.White));
            using (Graphics gr = Graphics.FromImage(image_for_box))
            {
                gr.Clear(Color.FromArgb(0, Color.White));
                gr.FillPath(brush_draw, shape_path);

				if (BorderOn == true)
				{
					using (Pen pen_draw = new Pen(BorderColor,BorderWidth))
					{
						pen_draw.Alignment = PenAlignment.Inset;
						gr.DrawPath(pen_draw, shape_path);
					}
				}

            }



            this.Image = image_for_box;
			Invalidate();
        }

		Bitmap ImageSet = null;
        private void PicutreBoxRound_Paint(object sender, PaintEventArgs e)
        {
		
            this.Paint -= PicutreBoxRound_Paint;
			ImageSet = new Bitmap(this.Image);
		//	ImageFunctions.ToFile.ToBMP(ImageSet, "123.bmp");
			PictureBoxDrawImage();


        }

        private void PicutreBoxRound_SizeChanged(object sender, EventArgs e)
		{
            this.SizeChanged -= PicutreBoxRound_SizeChanged;
            this.Size = new Size(this.Width, this.Width);
			this.SizeChanged += PicutreBoxRound_SizeChanged;
			Invalidate();
        
        }

		
	}


	/// <summary>
	/// Written. 2024.05.31 10:00 - 13:00. Warsaw. Workplace.
	/// Tested. Works. 2024.05.31 13:35. Warsaw. Workplace.
	/// </summary>
	public class ControlBorderSet : Control
	{


		int _corner_radius = 10;
		[Category("Border")]
	
		public int CornerRadius
		{
			get { return _corner_radius; }
			set
			{
				_corner_radius = value;
				if (value < 0)
				{
					_corner_radius = 0;
				}
				this.Invalidate();
			}
		}


		bool _border_on = true;
		[Category("Border")]
	
		public bool BorderON
		{
			get { return _border_on; }
			set
			{
				_border_on = value;
				this.Invalidate();
			}
		}


		Color _border_color = Color.Black;
		[Category("Border")]
		public Color BorderColor
		{
			get { return _border_color; }
			set
			{
				_border_color = value;
				this.Invalidate();
			}
		}

		

		Color _mouse_enter_color = Color.FromArgb(70,150,150,150);
		[Category("Color")]
		public Color MouseEnterColor
		{
			get { return _mouse_enter_color; }
			set
			{
				_mouse_enter_color = value;
				this.Invalidate();
			}
		}



		int _border_width = 1;
		[Category("Border")]
		[Description("Note. Border width 1 may cause not full border drawn")]
	
		public int BorderWidth
		{
			// 2024.05.31 11:04. Warsaw. Workplace.
			// width = 1 causes not full border drawn while width = 2 draws full border and the all border has width = 2.
			// I assume border width = 1 goes with control border which I assume has width = 1 and 
			// C# has to decide what to draw 1st and 2nd and with width = 1 decision is on the control border
			// while with width = 2 the decision is on the border with width = 2.
			get { return _border_width; }
			set
			{
				_border_width = value;
				if (value < 1)
				{
					_border_width = 1;
				}
				this.Invalidate();
			}
		}


		public ControlBorderSet()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.MouseEnter += ControlBorderSet_MouseEnter;
			this.MouseLeave += ControlBorderSet_MouseLeave;
		}

		private void ControlBorderSet_MouseLeave(object sender, EventArgs e)
		{
			_mouse_entered = false;
			Invalidate();
		}

		bool _mouse_entered = false;
		private void ControlBorderSet_MouseEnter(object sender, EventArgs e)
		{
			_mouse_entered = true;
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			base.OnPaint(pevent);

			


			GraphicsPath control_shape = new GraphicsPath();

			if (CornerRadius == 0)
			{
				control_shape.AddRectangle(new Rectangle(0, 0, this.Width, this.Height));
				control_shape.CloseFigure();
				this.Region = new Region(control_shape);
			}

			if (CornerRadius != 0)
			{
				control_shape.AddArc(new Rectangle(0, 0, CornerRadius * 2, CornerRadius * 2), 180, 90);
				control_shape.AddArc(new Rectangle(this.Width - CornerRadius * 2 - 1, 0, CornerRadius * 2, CornerRadius * 2), -90, 90);
				control_shape.AddArc(new Rectangle(this.Width - CornerRadius * 2 - 1, this.Height - CornerRadius * 2 - 1, CornerRadius * 2, CornerRadius * 2), 0, 90);
				control_shape.AddArc(new Rectangle(0, this.Height - CornerRadius * 2, CornerRadius * 2, CornerRadius * 2), 90, 90);
				control_shape.CloseFigure();
				this.Region = new Region(control_shape);
			}

			if (_mouse_entered == true)
			{
				pevent.Graphics.FillPath(new SolidBrush(MouseEnterColor), control_shape);
			}			

			if (BorderON == true)
			{
				Pen pen_draw = new Pen(BorderColor, BorderWidth);
				pen_draw.Alignment = PenAlignment.Inset;
				pevent.Graphics.DrawPath(pen_draw, control_shape);
			}
		}
	}

	/// <summary>
	/// 2024.05.16 09:29. Written and Tested. Works.
	/// </summary>
	public class ToggleButtonOnControl : Control
	{
		// 2024.05.16 10:27. Drawing animation was slow because of left mouse click event selection.
		// the event does not start if there is double click event.
		// the event mouse down was selected and the animation speed is ok now.
		private Color _button_on_backcolor = Color.MediumSlateBlue;
		private Color _button_on_color = Color.White;
		private Color _button_off_backcolor = Color.Gray;
		private Color _button_off_color = Color.White;
		private bool _button_pressed = false;

		[DefaultValue(false)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		[Category("Colors")]
		public Color ButtonOnBackColor
		{
			get { return _button_on_backcolor; }
			set
			{
				_button_on_backcolor = value;
				this.Invalidate();
			}
		}
		[Category("Colors")]
		public Color ButtonOnColor
		{
			get { return _button_on_color; }
			set
			{
				_button_on_color = value;
				this.Invalidate();
			}
		}

		[Category("Colors")]
		public Color ButtonOffBackColor
		{
			get { return _button_off_backcolor; }
			set
			{
				_button_off_backcolor = value;
				this.Invalidate();
			}
		}

		[Category("Colors")]
		public Color ButtonOffColor
		{
			get { return _button_off_color; }
			set
			{
				_button_off_color = value;
				this.Invalidate();
			}
		}

		[DefaultValue(false)]
		public bool Pressed
		{
			get
			{
				return _button_pressed;
			}
			set
			{
				_button_pressed = value;
			}
		}

		public ToggleButtonOnControl()
		{
			this.MinimumSize = new Size(40, 20);
			this.MouseDown += ToggleButton_Mouse_LeftClick;
		}

		private void ToggleButton_Mouse_LeftClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (this.Pressed == false)
				{
					this.Pressed = true;
					Invalidate();
					return;
				}
				if (this.Pressed == true)
				{
					this.Pressed = false;
					Invalidate();
					return;
				}
			}
		}

		private GraphicsPath GetFigurePath()
		{
			int arcSize = this.Height - 1;
			Rectangle leftArc = new Rectangle(0, 0, arcSize, arcSize);
			Rectangle rightArc = new Rectangle(this.Width - arcSize - 2, 0, arcSize, arcSize);
			GraphicsPath shape_path = new GraphicsPath();
			shape_path.AddArc(leftArc, 90, 180);
			shape_path.AddArc(rightArc, 270, 180);
			shape_path.CloseFigure();
			return shape_path;
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{

			int toggleSize = this.Height - 5;
			pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			pevent.Graphics.Clear(this.Parent.BackColor);
			if (this.Pressed == true)
			{

				pevent.Graphics.FillPath(new SolidBrush(_button_on_backcolor), GetFigurePath());
				pevent.Graphics.FillEllipse(new SolidBrush(_button_on_color),
				new Rectangle(this.Width - this.Height + 1, 2, toggleSize, toggleSize));
			}
			else
			{
				pevent.Graphics.FillPath(new SolidBrush(_button_off_backcolor), GetFigurePath());
				pevent.Graphics.FillEllipse(new SolidBrush(_button_off_color),
				new Rectangle(2, 2, toggleSize, toggleSize));
			}

		}
	}

	/// <summary>
	/// 2024.05.30 12-14. Written and Tested. Works.
	/// </summary>
	public class ProgressBarCustom : Control
	{
		private Color _backcolor = Color.White;
		private Color _fill_color = Color.MediumSlateBlue;

		[DefaultValue(false)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}




		[Category("Colors")]
		public Color BackgroundColor
		{
			get { return _backcolor; }
			set
			{
				_backcolor = value;
				// this.BackColor = value;
				this.Invalidate();
			}
		}
		[Category("Colors")]
		public Color FillColor
		{
			get { return _fill_color; }
			set
			{
				_fill_color = value;
				this.Invalidate();
			}
		}

		int _max_value = 100;
		[Category("Values")]
		[DefaultValue(100)]
		public int MaxValue
		{
			get { return _max_value; }
			set
			{
				_max_value = value;
				this.Invalidate();
			}
		}

		int _current_value = 20;
		[Category("Values")]
		[DefaultValue(20)]
		public int CurrentValue
		{
			get { return _current_value; }
			set
			{

				_current_value = value;
				if (value > MaxValue)
				{
					_current_value = MaxValue;
				}
				this.Invalidate();
			}
		}



		bool _border_on = true;
		[Category("Border")]
		[DefaultValue(true)]
		public bool BorderON
		{
			get { return _border_on; }
			set
			{
				_border_on = value;
				this.Invalidate();
			}
		}


		Color _border_color = Color.Black;
		[Category("Border")]
		public Color BorderColor
		{
			get { return _border_color; }
			set
			{
				_border_color = value;
				this.Invalidate();
			}
		}

		int _border_width = 1;
		[Category("Border")]
		[DefaultValue(1)]
		public int BorderWidth
		{
			get { return _border_width; }
			set
			{
				_border_width = value;
				if (value < 1)
				{
					_border_width = 1;
				}
				this.Invalidate();
			}
		}

		public enum BarOrientation
		{
			LeftToRight,
			RightToLeft,
			DownToUp,
			UpToDown
		}
		BarOrientation _orientation = BarOrientation.LeftToRight;
		[Category("Orienation")]
		[DefaultValue(BarOrientation.LeftToRight)]
		public BarOrientation Orientation
		{
			get { return _orientation; }
			set
			{
				_orientation = value;
				this.Invalidate();
			}
		}




		public ProgressBarCustom()
		{
			// 2024.05.31 11:47. Warsaw. Workplace.
			// Transparent color is supported by default.
			// Background transparency is needed to draw background transparent.
			// BackColor is connected to make transparent color.
			// important. setting back color vis OnPaint while not setting BackColor property
			// led to not good background color.
			// In detail.
			// There were transparent back color but the back color of control was not set and it was changing during work of programm.


			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			// 2024.05.31 12:12. Warsaw. Workplace.
			// Important. It shows background image
			// but it does not show another control on which 1st control is placed.
			// Solution. use of BringToFront, BringToBack.
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);

			// May lead to bug of not good coloring. It does not draw back color. the back color is filled (assumption) by
			// value from ram which are random by largerly black color because of 0 in the ram.
			// Setting transparent and not transparent did not solve it.
			// SetStyle(ControlStyles.Opaque, true);
		}

		private GraphicsPath Background_Path()
		{
			GraphicsPath shape_path = new GraphicsPath();
			shape_path.AddRectangle(new Rectangle(new Point(0, 0), new Size(this.Width - 1, this.Height - 1)));
			shape_path.CloseFigure();
			return shape_path;
		}

		private GraphicsPath Fill_Path_LeftToRight()
		{
			GraphicsPath shape_path = new GraphicsPath();
			int x_fill = (int)((float)this.Width * ((float)CurrentValue / (float)MaxValue));
			shape_path.AddRectangle(new Rectangle(new Point(0, 0), new Size(x_fill, this.Height)));
			shape_path.CloseFigure();
			return shape_path;
		}


		private GraphicsPath Fill_Path_RightToLeft()
		{
			GraphicsPath shape_path = new GraphicsPath();
			int x_fill = (int)((float)this.Width * ((float)CurrentValue / (float)MaxValue));
			shape_path.AddRectangle(new Rectangle(new Point(this.Width - x_fill - 1, 0), new Size(x_fill, this.Height)));
			shape_path.CloseFigure();
			return shape_path;
		}


		private GraphicsPath Fill_Path_DownToUp()
		{
			GraphicsPath shape_path = new GraphicsPath();
			int y_fill = (int)((float)this.Height * ((float)CurrentValue / (float)MaxValue));
			shape_path.AddRectangle(new Rectangle(new Point(0, this.Height - y_fill), new Size(this.Width, y_fill)));
			shape_path.CloseFigure();
			return shape_path;
		}

		private GraphicsPath Fill_Path_UpToDown()
		{
			GraphicsPath shape_path = new GraphicsPath();
			int y_fill = (int)((float)this.Height * ((float)CurrentValue / (float)MaxValue));
			shape_path.AddRectangle(new Rectangle(new Point(0, 0), new Size(this.Width, y_fill)));
			shape_path.CloseFigure();
			return shape_path;
		}


		protected override void OnPaint(PaintEventArgs pevent)
		{
			base.OnPaint(pevent);
			GraphicsPath background_path = null;
			GraphicsPath fill_path = null;
			if (Orientation == BarOrientation.LeftToRight)
			{
				background_path = Background_Path();
				fill_path = Fill_Path_LeftToRight();
			}
			if (Orientation == BarOrientation.RightToLeft)
			{
				background_path = Background_Path();
				fill_path = Fill_Path_RightToLeft();
			}
			if (Orientation == BarOrientation.DownToUp)
			{
				background_path = Background_Path();
				fill_path = Fill_Path_DownToUp();
			}
			if (Orientation == BarOrientation.UpToDown)
			{
				background_path = Background_Path();
				fill_path = Fill_Path_UpToDown();
			}




			// pevent.Graphics.Clear(this.Parent.BackColor);            
			pevent.Graphics.FillPath(new SolidBrush(BackgroundColor), background_path);
			pevent.Graphics.FillPath(new SolidBrush(FillColor), fill_path);

			if (BorderON == true)
			{
				Pen pen_draw = new Pen(BorderColor, BorderWidth);
				pen_draw.Alignment = PenAlignment.Center;
				pevent.Graphics.DrawPath(pen_draw, Background_Path());
			}

		}
	}



	/// <summary>
	/// 2024.05.15 12:49. Written and Tested. Works.
	/// </summary>
	public class ToggleButtonOnCheckBox : CheckBox
	{
		private Color _button_on_backcolor = Color.MediumSlateBlue;
		private Color _button_on_color = Color.White;
		private Color _button_off_backcolor = Color.Gray;
		private Color _button_off_color = Color.White;

		[DefaultValue(false)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		[Category("Colors")]
		public Color ButtonOnBackColor
		{
			get { return _button_on_backcolor; }
			set
			{
				_button_on_backcolor = value;
				this.Invalidate();
			}
		}
		[Category("Colors")]
		public Color ButtonOnColor
		{
			get { return _button_on_color; }
			set
			{
				_button_on_color = value;
				this.Invalidate();
			}
		}

		[Category("Colors")]
		public Color ButtonOffBackColor
		{
			get { return _button_off_backcolor; }
			set
			{
				_button_off_backcolor = value;
				this.Invalidate();
			}
		}

		[Category("Colors")]
		public Color ButtonOffColor
		{
			get { return _button_off_color; }
			set
			{
				_button_off_color = value;
				this.Invalidate();
			}
		}

		public ToggleButtonOnCheckBox()
		{
			this.MinimumSize = new Size(40, 20);
		}

		private GraphicsPath GetFigurePath()
		{
			int arcSize = this.Height - 1;
			Rectangle leftArc = new Rectangle(0, 0, arcSize, arcSize);
			Rectangle rightArc = new Rectangle(this.Width - arcSize - 2, 0, arcSize, arcSize);
			GraphicsPath shape_path = new GraphicsPath();
			shape_path.AddArc(leftArc, 90, 180);
			shape_path.AddArc(rightArc, 270, 180);
			shape_path.CloseFigure();
			return shape_path;
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			int toggleSize = this.Height - 5;
			pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			pevent.Graphics.Clear(this.Parent.BackColor);

			if (this.Checked == true)
			{

				pevent.Graphics.FillPath(new SolidBrush(_button_on_backcolor), GetFigurePath());
				pevent.Graphics.FillEllipse(new SolidBrush(_button_on_color),
				new Rectangle(this.Width - this.Height + 1, 2, toggleSize, toggleSize));
			}
			else
			{
				pevent.Graphics.FillPath(new SolidBrush(_button_off_backcolor), GetFigurePath());
				pevent.Graphics.FillEllipse(new SolidBrush(_button_off_color),
				new Rectangle(2, 2, toggleSize, toggleSize));
			}
		}
	}





	/// <summary>
	/// Written. 2024.06.25 19:31. Gdansk. Home. 
	/// Tested. Works. 2024.06.25 19:58. Gdansk. Home. 
	/// </summary>
    class ButtonRoundedCorners_v2 : Control
    {
       

        public ButtonRoundedCorners_v2()
        {
            
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            
            this.MouseEnter += Mybutton_MouseEnter;
            this.MouseLeave += Mybutton_MouseLeave;
            this.MouseDown += Mybutton_MouseDown;
            this.MouseUp += Mybutton_MouseUp;
            Border = new BorderProperties();
			this.BackColor = Color.FromArgb(0, Color.White);

        }

        class BorderProperties
        {
            public int CornerRadius = 20;
            public int Width = 2;
        }
        BorderProperties Border;
		Color BackGroundColor = Color.White;
        private void Mybutton_MouseUp(object sender, MouseEventArgs e)
        {
            this.BackGroundColor = MouseEnterColor;
            Invalidate();
        }
        private void Mybutton_MouseDown(object sender, MouseEventArgs e)
        {            
            this.BackGroundColor = MouseClickColor;
            Invalidate();
        }
        private void Mybutton_MouseLeave(object sender, EventArgs e)
        {
            this.BackGroundColor = MouseLeaveColor;
            Invalidate();
        }
        private void Mybutton_MouseEnter(object sender, EventArgs e)
        {
            this.BackGroundColor = MouseEnterColor;
			Invalidate();

        }
        
		
		public Color BorderPenColor = Color.Black;
		[Category("Border")]

        public Color BorderColor
        {
            get
            {
                return BorderPenColor;
            }
            set
            {
                BorderPenColor = value;
                Invalidate();
            }
        }


        bool _button_border = true;
        [Category("Border")]
        [Description("Sets if to show border")]
        [DefaultValue(true)]
        public bool ButtonBorder
        {
            get
            {
                return _button_border;
            }
            set
            {
                _button_border = value;
                Invalidate();
            }
        }



        [Category("Border")]
        [Description("Sets values for border")]
      
        public int CornerRadius
        {
            get
            {
                return Border.CornerRadius;
            }
            set
            {
                Border.CornerRadius = value;
                Invalidate();
            }
        }
        [Category("Border")]
      
        public int BorderWidth
        {
            get
            {
                return Border.Width;
            }
            set
            {
                Border.Width = value;
                Invalidate();
            }
        }
        private Color color_mouse_enter = Color.FromArgb(100, 192, 192, 255);
        [Category("Color")]
        [Description("Sets background when mouse in the button")]
        public Color MouseEnterColor
        {
            get
            {
                return color_mouse_enter;
            }
            set
            {
                color_mouse_enter = value;

                Invalidate();
            }
        }
        private Color color_mouse_leave = Color.WhiteSmoke;

        [Category("Color")]
        [Description("Sets background when mouse not in the button")]
        public Color MouseLeaveColor
        {
            get
            {
                return color_mouse_leave;
            }
            set
            {
                color_mouse_leave = value;
                BackGroundColor = value;
                Invalidate();
            }
        }

        private Color color_mouse_click = Color.White;

        [Category("Color")]
        [Description("Sets background when left mouse is clicked")]
        public Color MouseClickColor
        {
            get
            {
                return color_mouse_click;
            }
            set
            {
                color_mouse_click = value;

                Invalidate();
            }
        }




        protected override void OnPaint(PaintEventArgs pevent)
        {
			
			
			pevent.Graphics.SmoothingMode = SmoothingMode.None; 
            Size shape_size = new Size(this.Width, this.Height);
           
            GraphicsPath GraphPath = new GraphicsPath();
          

            if (Border.CornerRadius != 0)
            {
                GraphPath.StartFigure();
                GraphPath.AddArc(0, 0, Border.CornerRadius, Border.CornerRadius, -180, 90);
                GraphPath.AddArc(shape_size.Width - Border.CornerRadius - 1, 0, Border.CornerRadius, Border.CornerRadius, -90, 90);
                GraphPath.AddArc(shape_size.Width - Border.CornerRadius - 1, shape_size.Height - Border.CornerRadius - 1, Border.CornerRadius, Border.CornerRadius, 0, 90);
                GraphPath.AddArc(0, shape_size.Height - Border.CornerRadius - 1, Border.CornerRadius, Border.CornerRadius, 90, 90);
                GraphPath.CloseFigure();               
            }
            else
            {
                GraphPath.AddRectangle(new Rectangle(new Point(0, 0), shape_size));
                GraphPath.CloseFigure();                
            }

            using (Brush brush_draw = new SolidBrush(BackGroundColor))
            {                
                pevent.Graphics.FillPath(brush_draw, GraphPath);
            }

            if (ButtonBorder == true)
            {
                using (Pen pen = new Pen(BorderPenColor, Border.Width))
                {
                    pen.Alignment = PenAlignment.Inset;
                    pevent.Graphics.DrawPath(pen, GraphPath);
                }
            }

            Size clientSize = TextRenderer.MeasureText(Text, Font);
            int x_pos = (Width - clientSize.Width) / 2;
            int y_pos = (Height - clientSize.Height) / 2;
            pevent.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), new Point(x_pos, y_pos));
            base.OnPaint(pevent);
        }
    }




    /// <summary>
    /// Added. 2024.05.22 14:53. Warsaw. Workplace. <br></br>
    /// Work on rounding corner and information in internet showed that AddArc method is not <br></br>
    /// good for setting round corner and using this approach may be time consuming without <br></br>
    /// providing adequate result. <br></br>
	/// <br></br>
	/// Control is obsolete. 2024.06.25 17:41. Gdansk. Home. <br></br> 
    /// </summary>
    [Obsolete]
    class ButtonRoundedCorners : Control
	{
		// 2024.05.10 11:39. Gdansk. Home.
		// seperately each corner
		
		// 2024.06.12 22:48. Gdansk. Home.
		// There is width 2 is ok to use. 
		// There is trouble with width 1 and the trouble may not be fixable
		// GraphicPath gives shape according to the path and the shape is inside of the path
		// giving arc different from the arc provided and there is not each pixel match attempting
		// to draw such arc.
		
		
		public ButtonRoundedCorners()
		{
			// Added. 2024.05.22 17:03. Warsaw. Workplace.
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
        ControlStyles.AllPaintingInWmPaint |
        ControlStyles.ResizeRedraw, true);
            this.MouseEnter += Mybutton_MouseEnter;
			this.MouseLeave += Mybutton_MouseLeave;
			this.MouseDown += Mybutton_MouseDown;
			this.MouseUp += Mybutton_MouseUp;
			Border = new BorderProperties();
            this.SizeChanged += ButtonRoundedCorners_SizeChanged;
			
			
		}

        private void ButtonRoundedCorners_SizeChanged(object sender, EventArgs e)
        {
			int a = this.Size.Width;
        }


        /// <summary>
        /// Written. 2024.06.05 12:47. Warsaw. Workplace.
        /// </summary>
        public static class RoundCorner
		{

			public static Point[] Reverse(Point[] arr_in)
			{
				Point[] arr_out = new Point[arr_in.Length];
				for (int i = 0; i < arr_out.Length; i++)
				{
					arr_out[i] = arr_in[arr_in.Length - 1 - i];
				}
				return arr_out;
			}

			public static Point[] TopLeftPoints(int x_center, int y_center, int radius, int steps_per_pixel = 5)
			{

				if (radius < 2)
				{
					throw new ArgumentOutOfRangeException("radius of arc", "value should be at least 2");					
				}
				Point[] arc_points = BottomRightPoints(0, 0, radius, steps_per_pixel);
				Point[] arc_points_return = new Point[arc_points.Length];
				for (int i = 0; i < arc_points.Length; i++)
				{
					arc_points_return[i] = new Point(-1 * arc_points[i].X + x_center, -1 * arc_points[i].Y + y_center);
				}
				return arc_points_return;
			}


			public static Point[] BottonLeftPoints(int x_center, int y_center, int radius, int steps_per_pixel = 5)
			{

				if (radius < 2)
				{
					throw new ArgumentOutOfRangeException("radius of arc", "value should be at least 2");
				
				}
				Point[] arc_points = BottomRightPoints(0, 0, radius, steps_per_pixel);
				Point[] arc_points_return = new Point[arc_points.Length];
				for (int i = 0; i < arc_points.Length; i++)
				{
					arc_points_return[i] = new Point(-1 * arc_points[i].X + x_center, arc_points[i].Y + y_center);
				}
				return arc_points_return;
			}


			public static Point[] TopRightPoints(int x_center, int y_center, int radius, int steps_per_pixel = 5)
			{

				if (radius < 2)
				{
					throw new ArgumentOutOfRangeException("radius of arc", "value should be at least 2");
					
				}
				Point[] arc_points = BottomRightPoints(0, 0, radius, steps_per_pixel);
				Point[] arc_points_return = new Point[arc_points.Length];
				for (int i = 0; i < arc_points.Length; i++)
				{
					arc_points_return[i] = new Point(arc_points[i].X + x_center, -1 * arc_points[i].Y + y_center);
				}
				return arc_points_return;
			}

			public static Point[] BottomRightPoints(int x_center, int y_center, int radius, int steps_per_pixel = 5)
			{
				// 2024.06.05 11:01. Warsaw. Workplace.
				// 0 radius is not accepted - there is no arc.
				// 1 radius is not good because line requires 2 points.
				if (radius < 2)
				{
					throw new ArgumentOutOfRangeException("radius of arc", "value should be at least 2");
					
				}


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
						arc_points[fill_count - 1] = new Point(0 + x_center, (int)y + y_center - 1);

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
							arc_points[fill_count - 1] = new Point((int)(System.Math.Round(x) + (double)0.1) + x_center - 1, (int)y + y_center - 1);
							y_last = (int)y;
						}
					}

				}
				if (arc_points[fill_count - 1].Y != 0)
				{
					Array.Resize(ref arc_points, fill_count + 1);
					arc_points[arc_points.Length - 1] = new Point(r_arc + x_center - 1, y_center);
				}
				else
				{
					Array.Resize(ref arc_points, fill_count);
				}


				return arc_points;
			}
		}









		class BorderProperties
		{
			public int CornerRadius = 20;
			public int Width = 2;
		}
		BorderProperties Border;
		private void Mybutton_MouseUp(object sender, MouseEventArgs e)
		{			
			this.BackColor = MouseEnterColor;
		}
		private void Mybutton_MouseDown(object sender, MouseEventArgs e)
		{
			// 2024.05.07 10:23. Warsaw. Workplace.
			// The redraw of button is after this function. It was checked - border color was different.
			// note. to prevent usage of in built animation there is 2 pixel offset is required.
			
			this.BackColor = MouseClickColor;
		}
		private void Mybutton_MouseLeave(object sender, EventArgs e)
		{
			this.BackColor = MouseLeaveColor;
		}
		private void Mybutton_MouseEnter(object sender, EventArgs e)
		{
			this.BackColor = MouseEnterColor;
			
		}
		public Color BorderPenColor = Color.Black;

		// Added. 2024.05.22 17:03. Warsaw. Workplace.
		bool _button_border = true;
		[Category("Border")]
		[Description("Sets if to show border")]
		[DefaultValue(true)]
		public bool ButtonBorder
		{
			get
			{
				return _button_border;
			}
			set
			{
				_button_border = value;
				Invalidate();
			}
		}



		[Category("Border")]
		[Description("Sets values for border")]
	//	[DefaultValue(20)]
		public int CornerRadius
		{
			get
			{
				return Border.CornerRadius;
			}
			set
			{
				Border.CornerRadius = value;
				Invalidate();
			}
		}
		[Category("Border")]
	//	[DefaultValue(1)]
		public int BorderWidth
		{
			get
			{
				return Border.Width;
			}
			set
			{
				Border.Width = value;
				Invalidate();
			}
		}
		private Color color_mouse_enter = Color.FromArgb(100, 192, 192, 255);
		[Category("Color")]
		[Description("Sets background when mouse in the button")]
		public Color MouseEnterColor
		{
			get
			{
				return color_mouse_enter;
			}
			set
			{
				color_mouse_enter = value;
				
				Invalidate();
			}
		}
		private Color color_mouse_leave = Color.WhiteSmoke;
		
		[Category("Color")]
		[Description("Sets background when mouse not in the button")]
		public Color MouseLeaveColor
		{
			get
			{
				return color_mouse_leave;
			}
			set
			{
				color_mouse_leave = value;
				BackColor = value;
				Invalidate();
			}
		}

		private Color color_mouse_click = Color.White;

		[Category("Color")]
		[Description("Sets background when left mouse is clicked")]
		public Color MouseClickColor
		{
			get
			{
				return color_mouse_click;
			}
			set
			{
				color_mouse_click = value;
				
				Invalidate();
			}
		}




		protected override void OnPaint(PaintEventArgs pevent)
		{
			//;
			//pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			//   GraphicsPath gr = new GraphicsPath();
			// button. 
			// . . . . .
			// . . . . .
			// . . . . .
			// vertical line. left
			//    gr.AddLine(new Point(offset_size, round_size + offset_size), new Point(offset_size, ClientSize.Height - offset_size - round_size));
			// left. rounded.
			//   gr.AddArc(new Rectangle(new Point(offset_size, offset_size), new Size(round_size*2, round_size*2)), 0, -45);
			// horizontal line. up
			//	gr.AddLine(new Point(offset_size + round_size, offset_size), new Point(ClientSize.Width - offset_size - round_size, offset_size));
			// right. rounded.
			//   gr.AddArc(new Rectangle(new Point(ClientSize.Width - offset_size - round_size*2, ClientSize.Height - offset_size), new Size(round_size * 2, round_size * 2)), 90, 180);
			// vertical line. right
			//	gr.AddLine(new Point(ClientSize.Width - offset_size, round_size + offset_size), new Point(ClientSize.Width - offset_size, ClientSize.Height - offset_size - round_size));
			// 2024.05.05 17:56. Warsaw. Hostel. VDNH. Current code provides not symmetrical button.
			// 2024.05.05 19:17. Warsaw. Hostel. With the current code, it replaces the shape of the button
			// but click animation is still connected to rectangular shape. there is FlatStyle option
			// which allows to make click visible (Pop up) and button highlighted when mouse on the button (Flat)
			// C# correction by 1. 2024.05.09 20:52. Gdansk. Home.
			// Because of implementation of GraphicsPath
			int CorrectionBy1 = 1;
			// 2024.05.22 15:01. Warsaw. Workplace. There is no correction by 1.
			// There were anti alising on and therefore not good border drawing.
			pevent.Graphics.SmoothingMode = SmoothingMode.None;
		



			// 2024.06.04 16:36. Warsaw. Workplace.
			// 1. GraphicPath stores shapes to draw and 
			// region and draw by pen I assume uses different calculation for arc
			// therefore there is border with pixels missing.
			// Expecting visually the corners showed that the rounding is different
			// for region and for draw by pen despite using the same GraphicPath.
			//
			// 2. Border width 2 solves it but it is because of the way line with width 2
			// looks like. There are places in that line with width 1 - when the line
			// need to go further to the right (arc of 1st corner) and therefore
			// visually it is not seen that there is difference between region round corner
			// and border corner drawn by pen.
			//
			// 3. 2024.06.04 17:05. Warsaw. Workplace.
			// Analysing points of GraphicsPath showed that for radius 50 and for radius 20
			// there is the same amount of points.
			// I assume the arc is drawn by using two spline lines. Such line I assume requires 3-4 points.
			// assumption. spline has certain max curve radius and therefore arc requires 2 splines

			Size shape_size = new Size(this.Width/2, this.Height/2);
            Size border_size = new Size(this.Width/2, this.Height/2);
            GraphicsPath GraphPath = new GraphicsPath(FillMode.Winding);
			GraphicsPath GraphPathBorder = new GraphicsPath();

			if (Border.CornerRadius != 0)
			{
				GraphPath.StartFigure();

                GraphPath.AddArc(5, 5, (float)Border.CornerRadius, (float)Border.CornerRadius, -180, 90);
				GraphPath.AddArc(shape_size.Width - Border.CornerRadius - 1, 5, Border.CornerRadius, Border.CornerRadius, -90, 90);
				GraphPath.AddArc(shape_size.Width - Border.CornerRadius - 1, shape_size.Height - Border.CornerRadius -1, Border.CornerRadius, Border.CornerRadius, 0, 90);
				GraphPath.AddArc(5, shape_size.Height - Border.CornerRadius - 1, Border.CornerRadius, Border.CornerRadius, 90, 90);
				GraphPath.CloseFigure();
				if (ButtonBorder == true)
				{
					// 2024.06.25 15:44. Gdansk. Home. 
					// -1 for set coordinate. move from length to coordinate which is shorter by 1.
					
					GraphPathBorder.AddArc(2, 0, Border.CornerRadius, Border.CornerRadius, -180, 90);
					GraphPathBorder.AddArc(border_size.Width - Border.CornerRadius - 1, 0, Border.CornerRadius, Border.CornerRadius, -90, 90);
					GraphPathBorder.AddArc(border_size.Width - Border.CornerRadius - 1, border_size.Height - Border.CornerRadius - 1, Border.CornerRadius, Border.CornerRadius, 0, 90);
					GraphPathBorder.AddArc(2, border_size.Height - Border.CornerRadius - 1, Border.CornerRadius, Border.CornerRadius, 90, 90);
					GraphPathBorder.CloseFigure();
				//	ImageFunctions.ToPictureBox.FromBoolMask(SupportFunctions.GraphicPathPixels(GraphPathBorder, new Size(this.Width + 1, this.Height + 1)), Form.ActiveForm);
				}
			}
			else
			{
			

				GraphPath.AddRectangle(new Rectangle(new Point(0,0), shape_size));

				GraphPath.CloseFigure();
				if (ButtonBorder == true)
				{
					
					
					GraphPathBorder.AddRectangle(new Rectangle(new Point(0, 0), new Size(shape_size.Width, shape_size.Height)));
					GraphPathBorder.CloseFigure();
				}
			}
			// horizontal line. down
			//gr.AddLine(new Point(offset_size + round_size, ClientSize.Height - round_size - offset_size), new Point(ClientSize.Width - offset_size - round_size, ClientSize.Height - round_size - offset_size));
			//gr.AddEllipse(new Rectangle(5, 5, ClientSize.Width - 10, ClientSize.Height - 10));
			Region control_region = new Region(GraphPath);
			pevent.Graphics.SetClip(GraphPath);
			this.Region = control_region;
			if (ButtonBorder == true)
			{
				
				using (Pen pen = new Pen(BorderPenColor, Border.Width))
				{
					pen.Alignment = PenAlignment.Inset;
					pevent.Graphics.DrawPath(pen, GraphPathBorder);
				}
				
			}

			Size clientSize = TextRenderer.MeasureText(Text, Font);
			int x_pos = (Width - clientSize.Width) / 2;
			int y_pos = (Height - clientSize.Height) / 2;
            //	pevent.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), new Point(x_pos, y_pos));
            base.OnPaint(pevent);
        }
	}

	/// <summary>
	/// Added. 2024.05.22 14:53. Warsaw. Workplace.
	/// </summary>
	class TrackbarSelectorOnLine : Control
	{
		/// Written. 2024.05.22 11:24. Warsaw. Workplace.
		/// Tested. Works. 2024.05.22 11:24. Warsaw. Workplace.


		// 2. 2024.05.27 19:51. Warsaw. Hostel. Rostokino. 
		// To 1. There is no trouble in this trackbar because selector can be pulled by 1/2 and more
		// to the left from the box (rectungular box) - 1/2 and more will not be shown.
		// The same with the right side.
		// There is enough pixels provides to reach min and max value.

		// 1. 2024.05.22 11:22. Warsaw. Workplace.
		// There is trouble with max value. Converting pixels into value may result in no max value
		// because of insuffcient amount of last pixels.
		// solution 1. make trackbar larger by 1-2 to have the desired value to be set
		// solution 2. add code to include certain amount of pixel to width of control 2-5 pixels
		// and set check of max value is reached.


		public TrackbarSelectorOnLine()
		{
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.MouseEnter += Mybutton_MouseEnter;
			this.MouseLeave += Mybutton_MouseLeave;
			this.MouseDown += Mybutton_MouseDown;
			this.MouseUp += Mybutton_MouseUp;
			this.SizeChanged += TrackbarCustomV3_SizeChanged;

			this.MouseDown += TrackbarCustom_MouseDown;
			this.MouseUp += TrackbarCustom_MouseUp;
			this.MouseMove += TrackbarCustom_MouseMove;
			// added. 2024.06.10 16:05. Warsaw. Workplace 
			this.MouseMove += TrackbarSelectorOnLine_MouseMove;
		}


		private void TrackbarSelectorOnLine_MouseMove(object sender, MouseEventArgs e)
		{
			// added. 2024.06.10 16:13. Warsaw. Workplace.
			int _temp_x = SelectorCoordinates.XofSelector;
			bool _current_state = SelectorCoordinates.IsMouseIn;
			SelectorCoordinates.IsMouseIn = SelectorCoordinates.IsInSelector(e.Location);
			SelectorCoordinates.XofSelector = _temp_x;
			if (_current_state != SelectorCoordinates.IsMouseIn)
			{
				Invalidate();
			}
		}
		
		public event EventHandler ValueChanged;
		private void TrackbarCustomV3_SizeChanged(object sender, EventArgs e)
		{
			PixelsPerValue = (float)(this.Width - this.Height) / (float)(MaxValue - MinValue);
			SelectorCoordinates.XYSize = new Size(this.Height, this.Height);
			SelectorCoordinates.X = (int)((float)(CurrentValue - MinValue) * PixelsPerValue);
		}
		private void TrackbarCustom_MouseUp(object sender, MouseEventArgs e)
		{
			_mouse_down = false;
		}
		private void TrackbarCustom_MouseMove(object sender, MouseEventArgs e)
		{
			if (_mouse_down == false)
			{
				return;
			}
			if (SelectorCoordinates.IsClickedDown == true)
			{
				SelectorCoordinates.X = e.X - SelectorCoordinates.XofSelector;
				// 2024.05.12 15:52. Gdansk. Home. Placing 0 limit
				// moving to left was setting negative x coordinate and though value was set 0
				// the negative x coordinate was saved and do not allow to make click in the selector again.
				if (SelectorCoordinates.X < 0)
				{
					SelectorCoordinates.X = 0;
				}
				// 2024.05.12 16:20. Gdansk. Home. Placing max limit.
				/*
				if (SelectorCoordinates.X > (this.Width - this.Height - 1))
				{
					SelectorCoordinates.X = this.Width - this.Height - 1;
				}
				*/
				int value_on_move = (int)((float)SelectorCoordinates.X / PixelsPerValue) + MinValue;
				if (value_on_move > MaxValue)
				{
					value_on_move = MaxValue;
				}
				Debug.WriteLine(value_on_move.ToString());
				// 2024.05.27 20:28. Warsaw. Hostel. Rostokino. 
				// important. It was written not to update current value constantly
				// but there were trouble with mouse x coordinate -
				// once mouse crossed the rigth side it would start giving 
				// coordinate X larger and larger than width and MaxValue limit
				// was used only once and second time there were this condition below 
				// because of which there were X coordinate for selector returned (restored) and
				// that condition made selector not movable.
				/*
				if (value_on_move != CurrentValue)
				{
				*/
					CurrentValue = value_on_move;
					if (ValueChanged != null)
					{
						ValueChanged.Invoke(this, EventArgs.Empty);
					}
				
			}
		}
		bool _mouse_down = false;
		private void TrackbarCustom_MouseDown(object sender, MouseEventArgs e)
		{
			_mouse_down = true;
			SelectorCoordinates.IsClickedDown = false;
			if (SelectorCoordinates.IsInSelector(e.Location) == true)
			{
				SelectorCoordinates.IsClickedDown = true;
			}
		}
		private void Mybutton_MouseUp(object sender, MouseEventArgs e)
		{
		}
		private void Mybutton_MouseDown(object sender, MouseEventArgs e)
		{
		}
		private void Mybutton_MouseLeave(object sender, EventArgs e)
		{
			// added. 2024.06.10 16:13. Warsaw. Workplace 
			SelectorCoordinates.IsMouseIn = false;
			Invalidate();
		}
		private void Mybutton_MouseEnter(object sender, EventArgs e)
		{
		}
		float PixelsPerValue = 0;
		int _min_value = 0;
		[Category("Values")]
		[Description("Sets min value for trackbar")]
		[DefaultValue(0)]
		public int MinValue
		{
			get { return _min_value; }
			set
			{
				_min_value = value;
				PixelsPerValue = (float)(this.Width - this.Height) / (float)(MaxValue - value);
			}
		}
		Color selector_line_color = Color.Black;
		[Category("Selector")]
		[Description("Sets color for selector line")]
		public Color SelectorLineColor
		{
			get
			{
				return selector_line_color;
			}
			set
			{
				selector_line_color = value;
				Invalidate();
			}
		}
		int _selector_line_width = 1;
		[Category("Selector")]
		[Description("Sets width for selector line")]
		[DefaultValue(1)]
		public int SelectorLineWidth
		{
			get
			{
				return _selector_line_width;
			}
			set
			{
				_selector_line_width = value;
				Invalidate();
			}
		}
		int _max_value = 100;
		[Category("Values")]
		[Description("Sets max value for trackbar")]
		[DefaultValue(100)]
		public int MaxValue
		{
			get { return _max_value; }
			set
			{
				_max_value = value;
				PixelsPerValue = (float)(this.Width - this.Height) / (float)(value - MinValue);
			}
		}
		int _current_value = 10;
		[Category("Values")]
		[Description("Sets current value for trackbar")]
		[DefaultValue(10)]
		public int CurrentValue
		{
			get { return _current_value; }
			set
			{
				_current_value = value;
				SelectorCoordinates.X = (int)((float)(value - MinValue) * PixelsPerValue);
				Invalidate();
			}
		}
		/*
		Color _border_color = Color.Gray;
		[Category("Border")]
		[Description("Sets color of border")]
		public Color BorderColor
		{
			get
			{
				return _border_color;
			}
			set
			{
				_border_color = value;
				Invalidate();
			}
		}
		*/
		int _selector_width = 1;
		[Category("Selector")]
		[Description("Sets width of border")]
		public int SelectorWidth
		{
			get
			{
				return _selector_width;
			}
			set
			{
				_selector_width = value;
				Invalidate();
			}
		}
		public enum ShapeOfSelector
		{
			NotDefined,
			Square,
			Circle
		}
		ShapeOfSelector _selector_shape = ShapeOfSelector.Circle;
		[Category("Selector")]
		[Description("Sets shape of selector")]
		[DefaultValue(ShapeOfSelector.Circle)]
		public ShapeOfSelector SelectorShape
		{
			get
			{
				return _selector_shape;
			}
			set
			{
				_selector_shape = value;
				Invalidate();
			}
		}
		Color _selector_color = Color.White;
		[Category("Selector")]
		[Description("Sets color of selector")]
		public Color SelectorColor
		{
			get
			{
				return _selector_color;
			}
			set
			{
				_selector_color = value;
				Invalidate();
			}
		}


		Color _selector_enter = ColorOfControl.GraySolidTransparent;
		[Category("Selector")]
		[Description("Sets color of selector when mouse enters selector")]
		public Color MouseEnterColor
		{
			get
			{
				return _selector_enter;
			}
			set
			{
				_selector_enter = value;
				Invalidate();
			}
		}


		class SelectorXY
		{
			public int X;
			public int Y;
			public Size XYSize;
			public bool IsClickedDown = false;
			public bool IsMouseIn = false;
			public int XofSelector = 0;
			public bool IsInSelector(Point point_in)
			{
				bool check_return = false;
				if ((point_in.X >= X) &&
					(point_in.X <= X + XYSize.Width) &&
					(point_in.Y >= Y) &&
					(point_in.Y <= Y + XYSize.Height))
				{
					check_return = true;
					XofSelector = (point_in.X - X);
				}
				return check_return;
			}
		}
		SelectorXY SelectorCoordinates = new SelectorXY();
		protected override void OnPaint(PaintEventArgs pevent)
		{
			//pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			// 2024.05.22 09:41. Warsaw. Workplace. Selector line.
			GraphicsPath GraphPath = new GraphicsPath();


			// 2024.05.27 19:47. Warsaw. Hostel. Rostokino. about -1. There assymetry by 1 pixel between left and right side
			// important. Point takes coordinates which is from 0:
			// - line of length 1 has point with 0
			// - line with length 2 has points with 0 and 1
			// - etc.
			GraphPath.AddLine(new Point(this.Height / 2, this.Height / 2), new Point(-1 + this.Width - this.Height / 2, this.Height / 2));
			// check region and height. 2024.05.10 13:59. Gdansk. Home 
			GraphPath.CloseFigure();
			using (Pen pen = new Pen(SelectorLineColor, SelectorLineWidth))
			{
				//pen.Alignment = PenAlignment.Inset;
				pevent.Graphics.DrawPath(pen, GraphPath);
			}


			// 2024.05.22 09:42. Warsaw. Workplace. Selector.
			int x_loc = (int)(((float)(CurrentValue - MinValue) / (float)(MaxValue - MinValue)) * (float)(this.Width - this.Height));
			// - 1 for symmetry - 3 (1 1), 5 (2,2) etc
			if (SelectorShape == ShapeOfSelector.Square)
			{
				Rectangle RectSelector = new Rectangle(x_loc, 0, this.Height - 1, this.Height - 1);
				using (Brush brush = new SolidBrush(SelectorColor))
				{
					pevent.Graphics.FillRectangle(brush, RectSelector);
					if (SelectorCoordinates.IsMouseIn == true)
					{
						pevent.Graphics.FillRectangle(new SolidBrush(MouseEnterColor), RectSelector);
					}
				}
				using (Pen pen = new Pen(SelectorLineColor, SelectorWidth))
				{
					pen.Alignment = PenAlignment.Inset;
					pevent.Graphics.DrawRectangle(pen, RectSelector);
				}
			}
			if (SelectorShape == ShapeOfSelector.Circle)
			{
				Rectangle RectSelector = new Rectangle(x_loc, 0, this.Height-1, this.Height-1);
				using (Brush brush = new SolidBrush(SelectorColor))
				{
					pevent.Graphics.FillEllipse(brush, RectSelector);
					if (SelectorCoordinates.IsMouseIn == true)
					{
						pevent.Graphics.FillEllipse(new SolidBrush(MouseEnterColor), RectSelector);
					}
				}
				using (Pen pen = new Pen(SelectorLineColor, SelectorWidth))
				{
					pen.Alignment = PenAlignment.Inset;
					pevent.Graphics.DrawEllipse(pen, RectSelector);
				}
			}

			
			
			

			base.OnPaint(pevent);
		}
	}
	// 2024.05.14 15:08. Warsaw. Workplace. Finished. It is working.
	// Resizing trouble. Button can be resized but trackbar has size connected to values
	// and at the init control has Size(0,0) and calls size changed. not sure it happens at the start.
}
