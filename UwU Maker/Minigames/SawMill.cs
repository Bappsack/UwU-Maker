using System;
using System.Drawing;
using System.Threading.Tasks;
using UwU_Maker;

namespace UwU_Maker.Minigames
{
    class SawMill
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

                if (!SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 425, 399, 475, 449), SawColorLeft).IsEmpty && !IsFinished)
                {
                    await Task.Delay(10);
                    BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_LEFT, 5);
                }

                if (!SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 427, 500, 479, 547), SawColorLeft).IsEmpty && !IsFinished)
                {
                    await Task.Delay(10);
                    BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_RIGHT, 5);
                }

                if (!SearchImage.Find(target, Resources.Score_SawMill, out Scan).IsEmpty)
                {
                    Console.WriteLine("BOT: " + BotID + "(SawMill): " + " Enough Points!");
                    await Task.Delay(3000);
                    IsFinished = true;
                }

                if (!SearchImage.Find(target, Resources.Failed_SawMill, out Scan).IsEmpty && !IsFinished)
                {
                    Console.WriteLine("BOT: " + BotID + "(SawMill): " + " Failed, Retry!");
                    BackgroundHelper.SendClick(hWnd, Scan.X + 5, Scan.Y + 5, 250);
                    await Task.Delay(1500);
                    IsFinished = !IsFinished;
                }


                if (IsFinished)
                {

                    if (!SearchImage.Find(target, Resources.Reward_Button_SawMill, out Scan).IsEmpty)
                    {
                        Console.WriteLine("BOT: " + BotID + "(SawMill): " + "Click Reward Button!");
                        BackgroundHelper.SendClick(hWnd, Scan.X + 25, Scan.Y + 15, 250);
                        await Task.Delay(1000);
                    }

                    if (!SearchImage.Find(target, Resources.Level5, out Scan).IsEmpty)
                    {
                        Console.WriteLine("BOT: " + BotID + "(SawMill): " + "Click Level 5 Button!");
                        //BackgroundHelper.SendClick(hWnd, Scan.X +5, Scan.Y +5, 250);
                        await Task.Delay(1000);
                    }

                    if (!SearchImage.Find(target, Resources.TryAgain, out Scan).IsEmpty)
                    {
                        if (Times >= Amount)
                        {
                            Console.WriteLine("Done!");

                            while (Program.botRunning) { await Task.Delay(100); }
                        }
                        Times++;
                        Console.WriteLine("BOT: " + BotID + "(SawMill): " + "Click Try Again Button!");
                        BackgroundHelper.SendClick(hWnd, Scan.X + 20, Scan.Y + 5, 250);
                        await Task.Delay(1000);
                    }

                    if (!SearchImage.Find(target, Resources.Start_SawMill, out Scan).IsEmpty)
                    {
                        Console.WriteLine("BOT: " + BotID + "(SawMill): " + "Click Start Button!");
                        await Task.Delay(500);
                        BackgroundHelper.SendClick(hWnd, Scan.X + 50, Scan.Y + 30, 250);
                        await Task.Delay(2000);
                        IsFinished = !IsFinished;
                    }

                    if (!SearchImage.Find(target, Resources.NoPoints, out Scan).IsEmpty)
                    {
                        Console.WriteLine("BOT: " + BotID + "(SawMill): " + "No Points Left!");
                    }
                }
                await Task.Delay(10);
            }
            Console.WriteLine("SawMill Bot Stopped for HWID: {0}, Bot ID: {1}, Times played: {2}/{3}", hWnd, BotID, Times, Amount);
        }


        private async Task<bool> RandomMiss(int min, int max)
        {
            bool RandomMiss = false;
            int rng = new Random().Next(min, max);
            if (rng == 10)
            {
                await Task.Delay(1000);
                RandomMiss = true;
            }

            return RandomMiss;
        }

    }
}
