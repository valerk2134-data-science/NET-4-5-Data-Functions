using ReportFunctionsNamespace;
using System.Media;




namespace SoundFunctionsNamespace
{
    public static class SoundFunctions
    {
        public static void PlaySound(string file_path)
        {
            try
            {
                SoundPlayer sound_player = new SoundPlayer();
                sound_player.SoundLocation = file_path;
                sound_player.Play();
            }
            catch
            {
                ReportFunctions.ReportError("Sound was not played\r\nSound path: " + file_path);
            }
        }
    }
}
