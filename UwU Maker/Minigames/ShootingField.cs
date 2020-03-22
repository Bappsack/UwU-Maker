using System;
using System.Drawing;
using System.Threading.Tasks;

namespace UwU_Maker.Minigames
{
    class ShootingField
    {
        private SearchImage SearchImage = new SearchImage();
        private bool IsPlaying { get; set; }
        private bool IsFinished { get; set; }
        private bool IsFailed { get; set; }
        private int Times { get; set; }

        private Color SawColorLeft = ColorTranslator.FromHtml("#5D4116");
        private Color SawColorRight = ColorTranslator.FromHtml("#5D4116");


        private Color GameStart = ColorTranslator.FromHtml("#72D323");
        private Color GameEnd = ColorTranslator.FromHtml("#9557A3");

        private int[] SawLeft = { 466, 428 };
        private int[] SawRight = { 469, 530 };



        public async void RunTask(IntPtr hWnd, int Amount, SearchImage.RECT rect, int BotID, bool UseRewardCoupon)
        {
            Console.WriteLine("SawMill Bot created for HWID: {0}, Bot ID: {1}, Times: {2}", hWnd, BotID, Amount);
            IsFinished = true;
            IsFailed = false;
            Times = 0;
            Point Scan;
            Bitmap target;

            while (Program.botRunning)
            {
                int RandomStep = new Random().Next(0, 5);

                target = SearchImage.CaptureWindow(hWnd, rect.Right, rect.Bottom);


                if (!SearchImage.Find(target, Resources.chicken_right, out Scan).IsEmpty)
                {
                    await Task.Delay(10);
                    await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_RIGHT, 5);
                }



                await Task.Delay(10);
            }
            Console.WriteLine("SawMill Bot Stopped for HWID: {0}, Bot ID: {1}, Times played: {2}/{3}", hWnd, BotID, Times, Amount);
        }
    }
}
