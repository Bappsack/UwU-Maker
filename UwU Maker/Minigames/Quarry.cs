using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UwU_Maker.Minigames
{
    class Quarry
    {
        private SearchImage SearchImage = new SearchImage();
        private bool IsPlaying { get; set; }
        private bool IsFinished { get; set; }
        private bool IsFailed { get; set; }
        private int Times { get; set; }

        private Color WormColorLeft = ColorTranslator.FromHtml("#E6FD88");
        private Color WormColorRight = ColorTranslator.FromHtml("#E6FD88");


        private Color GameStart = ColorTranslator.FromHtml("#FAF7E6");
        private Color GameEnd = ColorTranslator.FromHtml("#9557A3");

        private int[] SawLeft = { 466, 428 };
        private int[] SawRight = { 469, 530 };


        public async void RunTask(IntPtr hWnd, int Amount, SearchImage.RECT rect, int BotID, bool UseRewardCoupon)
        {
            Console.WriteLine("Quarry Bot created for HWID: {0}, Bot ID: {1}, Times: {2}", hWnd, BotID, Amount);
            IsFinished = true;
            IsFailed = false;
            Times = 0;
            Point Scan;
            Bitmap target;

            while (Program.botRunning)
            {
                int RandomStep = new Random().Next(0, 5);

                target = SearchImage.CaptureWindow(hWnd, rect.Right, rect.Bottom);

                if (!IsFinished)
                {
                    await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_UP, 5);


                    if (!SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 387, 527, 422, 572), WormColorLeft).IsEmpty)
                    {
                        await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_LEFT, 5);
                        await Task.Delay(50);
                    }

                    if (!SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 580, 520, 635, 585), WormColorRight).IsEmpty)
                    {
                        await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_RIGHT, 5);
                        await Task.Delay(50);
                    }


                    if (!SearchImage.Find(target, Resources.Score_Stone, out Scan).IsEmpty)
                    {
                        Console.WriteLine("BOT: " + BotID + "(Quarry): " + "Enough Points!");
                        await Task.Delay(6000);
                        IsFinished = true;
                    }

                    if (!SearchImage.Find(target, Resources.Failed_Stone, out Scan).IsEmpty)
                    {
                        Console.WriteLine("BOT: " + BotID + "(Quarry): " + "Failed, Retry!");
                        await BackgroundHelper.SendClick(hWnd, 377, 435, 250);
                        await Task.Delay(1500);
                        IsFailed = true;
                        IsFinished = true;
                    }
                }

                if (IsFinished)
                {
                    //Console.WriteLine("Finished!");

                    if (!SearchImage.Find(target, Resources.Reward_Button_Stone, out Scan).IsEmpty && !IsFailed)
                    {
                        Console.WriteLine("BOT: " + BotID + "(Quarry): " + "Click Reward Button!");
                        await BackgroundHelper.SendClick(hWnd, Scan.X + 25, Scan.Y + 15, 250);
                        await Task.Delay(1000);
                    }

                    if (!SearchImage.Find(target, Resources.Level5, out Scan).IsEmpty)
                    {
                        Console.WriteLine("BOT: " + BotID + "(Quarry): " + "Click Level 5 Button!");
                        await BackgroundHelper.SendClick(hWnd, Scan.X + 5, Scan.Y + 5, 250);
                        await Task.Delay(500);
                        if (UseRewardCoupon)
                        {
                            await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_RETURN, 50);
                            await Task.Delay(250);
                        }
                    }

                    if (!SearchImage.Find(target, Resources.TryAgain_Stone, out Scan).IsEmpty)
                    {
                        Console.WriteLine("Bot:" + BotID + "(FishPond): Finished! Played: " + Times + "/" + Amount);

                        Console.WriteLine("BOT: " + BotID + "(Quarry): " + "Click Try Again Button!");
                        await BackgroundHelper.SendClick(hWnd, 456, 464, 250);
                        await Task.Delay(1000);
                        Times++;
                        if (Times >= Amount)
                        {
                            MessageBox.Show("Done!");

                            while (Program.botRunning) { await Task.Delay(100); }
                        }
                    }

                    if (!SearchImage.Find(target, Resources.Start_Stone, out Scan).IsEmpty)
                    {
                        Console.WriteLine("BOT: " + BotID + "(Quarry): " + "Click Start Button!");
                        await Task.Delay(500);
                        await BackgroundHelper.SendClick(hWnd, Scan.X + 50, Scan.Y + 30, 250);
                        await Task.Delay(2000);
                        IsFinished = false;
                        IsFailed = false;
                    }

                }
                await Task.Delay(10);
            }
            Console.WriteLine("Quarry Bot Stopped for HWID: {0}, Bot ID: {1}, Times played: {2}/{3}", hWnd, BotID, Times, Amount);
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
