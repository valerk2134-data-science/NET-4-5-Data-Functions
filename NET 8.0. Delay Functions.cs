using System;
using System.Threading.Tasks;




// Written. 2024.01.04 14:55. Gdansk. Home.
namespace DelayFunctionsNamespace
{
    public static class DelayFunctions
    {
        /// <summary>
        /// Written. 2024.01.04 15:07. Gdansk. Home. 
        /// </summary>
        public class Delay
        {
            System.Windows.Forms.Timer timer_delay = new System.Windows.Forms.Timer();
            public Delay() 
            {
                timer_delay.Tick += Timer_delay_Tick;
            }
            bool TimerTicked = false;
            private void Timer_delay_Tick(object sender, EventArgs e)
            {
                timer_delay.Stop();
                TimerTicked = true;
            }

            // Added. 2024.01.04 15:24. Gdansk. Home.
            async Task UseDelay(Int32 delay_ms)
            {
                await Task.Delay(delay_ms);
            }

            /*
            2024.01.04 15:39. Gdansk. Home. 
            The code works. The button changes color and the form does not freeze. The other button
            is selectable during color change, during await Task.Delay(500).
            
            async private void button1_Click(object sender, EventArgs e)
            {
                for (var i = 0; i < 10; i++)
                {
                    button1.BackColor = Color.Red;
                    await Task.Delay(500);
                    button1.BackColor = Color.Yellow;
                    await Task.Delay(500);
                }
            }
            */






            public void DelayStart(Int32 delay_ms)
            {                
                timer_delay.Interval = delay_ms;
                TimerTicked = false;
                timer_delay.Start();
                while (TimerTicked == false)
                {

                }
            }
        }
        


    }
}
