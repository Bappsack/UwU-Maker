using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UwU_Maker;

namespace UwU_Maker.Minigames
{
    class FishPond
    {
        private SearchImage SearchImage = new SearchImage();
        private Bitmap target;
        private Bitmap target2;

        private bool IsPlaying { get; set; }

        private bool IsFinished { get; set; }
        private bool IsFailed { get; set; }

        private bool ProductionNeeded { get; set; }
        public int Times { get; private set; }

        private bool FastModeEnabled = false;

        private int PressTime = 409;
        private int WaitTime = 30;

        private Color FishColor = ColorTranslator.FromHtml("#C6F7FF");
        private Color FishCatechedColor = ColorTranslator.FromHtml("#FF0400");
        private Color StupidCarpWiggle = ColorTranslator.FromHtml("#FC9B13");
        private Color FishColorEvent = ColorTranslator.FromHtml("#3578AF");
        private Color DamonColorRight = ColorTranslator.FromHtml("#1D0809"); //1D0809
        private Color DamonColorLeft = ColorTranslator.FromHtml("#1A2B2E"); // 1a2B2e
        private Color DamonColorLeftLeft = ColorTranslator.FromHtml("#7F1716");

        public async void RunTask(IntPtr hWnd, int Amount, SearchImage.RECT rect, int BotID, bool UseRewardCoupon, bool UseProductionCoupon, int LootTimeA, int LootTimeB, double FailChance)
        {
            Console.WriteLine("FishPond Bot Created for Hwid: {0}, Bot ID: {1}, Times: {2}", hWnd, BotID, Amount);
            IsFinished = true;
            IsFailed = false;
            ProductionNeeded = false;
            Times = 0;
            Point Scan;
            System.Timers.Timer FastMode = new System.Timers.Timer();
            FastMode.Elapsed += TriggerFastMode;
            FastMode.Interval += 100000;
            FastMode.AutoReset = false;

            Thread ScreenCap = new Thread(delegate () { ScreenCapEvent(hWnd, rect); });
            ScreenCap.Start();
            await Task.Delay(1000);
            // FastMode Timer            

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();


            while (Program.botRunning)
            {
                if (!IsFinished)
                {
                    if (!SearchImage.Find(target, Resources.Reward_FishPond, out Scan).IsEmpty)
                    {
                        await Task.Delay(500);
                        await BackgroundHelper.SendClick(hWnd, 375, 441, 250);
                        await Task.Delay(2000);
                        IsFailed = true;
                        IsFinished = true;
                    }

                    if (!SearchImage.Find(target, Resources.score_FishPond, out Scan).IsEmpty)
                    {
                        await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_LEFT, 5);
                        await Task.Delay(1000);
                        await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_RIGHT, 5);
                        await Task.Delay(1000);
                        await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_UP, 5);
                        await Task.Delay(1000);
                        await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_DOWN, 5);
                        await Task.Delay(1000);
                        await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_UP, 5);
                        await Task.Delay(1000);
                        await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_LEFT, 5);
                        await Task.Delay(1000);

                        IsFinished = true;
                    }

                    await IsFishComboEvent(hWnd, rect);



                    #region Catch Fish Detection
                    if (!SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 664, 371, 668, 375), FishColor).IsEmpty &&
                        SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 639, 289, 647, 297), FishCatechedColor).IsEmpty &&
                        SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 312, 386, 320, 394), FishColorEvent).IsEmpty &&
                        SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 620, 306, 670, 361), StupidCarpWiggle).IsEmpty &&
                        SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 617, 376, 632, 395), DamonColorRight).IsEmpty)
                    {
                        await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_RIGHT, PressTime);
                        await Task.Delay(WaitTime);
                        await IsFishComboEvent(hWnd, rect);
                    }

                    if (!SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 505, 425, 510, 430), FishColor).IsEmpty &&
                        SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 483, 344, 491, 352), FishCatechedColor).IsEmpty &&
                        SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 312, 386, 320, 394), FishColorEvent).IsEmpty &&
                        SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 463, 370, 513, 420), StupidCarpWiggle).IsEmpty &&
                        SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 463, 434, 482, 459), DamonColorLeft).IsEmpty)
                    {
                        await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_DOWN, PressTime);
                        await Task.Delay(WaitTime);
                        await IsFishComboEvent(hWnd, rect);
                    }
                    //++
                    if (!SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 388, 367, 396, 375), FishColor).IsEmpty &&
                        SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 371, 285, 379, 293), FishCatechedColor).IsEmpty &&
                        SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 312, 386, 320, 394), FishColorEvent).IsEmpty &&
                        SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 355, 320, 392, 355), StupidCarpWiggle).IsEmpty &&
                        SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 381, 368, 397, 405), DamonColorLeftLeft).IsEmpty)
                    {
                        await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_LEFT, PressTime);
                        await Task.Delay(WaitTime);
                        await IsFishComboEvent(hWnd, rect);
                    }
                    //++
                    if (!SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 544, 319, 552, 327), FishColor).IsEmpty &&
                        SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 520, 241, 528, 249), FishCatechedColor).IsEmpty &&
                        SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 312, 386, 320, 394), FishColorEvent).IsEmpty &&
                        SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 507, 276, 547, 317), StupidCarpWiggle).IsEmpty &&
                        SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 494, 330, 507, 345), DamonColorRight).IsEmpty)
                    {
                        await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_UP, PressTime);
                        await Task.Delay(WaitTime);
                        await IsFishComboEvent(hWnd, rect);
                    }
                    #endregion
                }
                else
                {
                    if (!ProductionNeeded)
                    {
                        if (!SearchImage.Find(target, Resources.Reward_FishPond, out Scan).IsEmpty && !IsFailed)
                        {
                            /*
                            if( new Random().NextDouble() < Convert.ToInt32(FailChance))
                            {
                                Console.WriteLine("BOT: " + BotID + "(FishPond): " + " Fake Fail, Retry!");
                                await Task.Delay(500);
                                await BackgroundHelper.SendClick(hWnd, 375, 441, 250);
                                await Task.Delay(2000);
                                IsFailed = true;
                                IsFinished = true;
                            }
                            */
                            Console.WriteLine("BOT: " + BotID + "(FishPond): " + " Get Reward!");
                            await Task.Delay(250 + new Random().Next(500, 1500));
                            await Task.Delay(new Random().Next(LootTimeA, LootTimeB));
                            await BackgroundHelper.SendClick(hWnd, 640,438, 250);
                            await Task.Delay(1000);
                        }


                        if (!SearchImage.Find(target, Resources.TryAgain_FishPond, out Scan).IsEmpty && !IsFailed)
                        {
                            Console.WriteLine("BOT: " + BotID + "(FishPond): " + "Click Try Again!");
                            await Task.Delay(250);
                            await BackgroundHelper.SendClick(hWnd, 457, 466, 250);
                            await Task.Delay(1000);
                        }

                        if (!SearchImage.Find(target, Resources.Start_FishPond, out Scan).IsEmpty)
                        {
                            Console.WriteLine("BOT: " + BotID + "(FishPond): " + "Start Playing!");
                            await Task.Delay(250);
                            await BackgroundHelper.SendClick(hWnd,514,550, 250);
                            await Task.Delay(2000);
                            FastModeEnabled = false;
                            FastMode.Stop();
                            PressTime = 409;
                            WaitTime = 20;
                            FastMode.Start();

                            stopwatch.Stop();
                            TimeSpan time = stopwatch.Elapsed;
                            stopwatch.Reset();
                            Console.WriteLine("Time Taken: " + time.Minutes + ":" + time.Seconds);
                            stopwatch.Start();
                            IsFinished = false;
                            IsFailed = false;
                        }

                        if (!SearchImage.Find(target, Resources.Production_Coupon_Trigger, out Scan).IsEmpty && !IsFailed)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_ESCAPE, 250);
                            }
                            if (UseProductionCoupon)
                            {
                                ProductionNeeded = true;
                            }
                            else
                            {
                                MessageBox.Show("No Points Left, stopped.");
                                while (Program.botRunning) { await Task.Delay(100); }
                            }
                        }


                        if (!SearchImage.Find(target, Resources.Level5, out Scan).IsEmpty && !IsFailed)
                        {
                            Console.WriteLine("BOT: " + BotID + "(FishPond): " + " Get Level 5 Reward!");
                            await Task.Delay(250);
                            await BackgroundHelper.SendClick(hWnd, Scan.X + 10, Scan.Y + 15, 250);
                            await Task.Delay(1000);
                            if (UseRewardCoupon)
                            {
                                await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_RETURN, 50);
                                await Task.Delay(250);
                            }
                            else
                            {
                                await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_ESCAPE, 50);
                                await Task.Delay(250);
                            }
                            Times++;
                            Console.WriteLine("Bot:" + BotID + "(FishPond): Finished! Played: " + Times + "/" + Amount);
                            if (Times >= Amount)
                            {
                                MessageBox.Show("Done!");

                                while (Program.botRunning) { await Task.Delay(100); }
                            }
                        }
                    }
                    else
                    {

                        if (!SearchImage.Find(target, Resources.Stop_Button, out Scan).IsEmpty && !IsFailed) // ORIG Stop Button
                        {
                            await Task.Delay(250);
                            await BackgroundHelper.SendClick(hWnd, 565, 467, 250); // ORIG 565, 467
                            await Task.Delay(2000);


                            if (!SearchImage.Find(target, Resources.Production_Coupon, out Scan).IsEmpty && !IsFailed)
                            {
                                await Task.Delay(250);
                                await BackgroundHelper.SendClick(hWnd, Scan.X, Scan.Y, 250);
                                await Task.Delay(500);
                                await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_RETURN, 50);
                                await Task.Delay(500);
                                await BackgroundHelper.SendClick(hWnd, 511, 371, 250);
                                await Task.Delay(500);
                                await BackgroundHelper.SendClick(hWnd, 478, 534, 250);
                                ProductionNeeded = false;
                                await Task.Delay(500);

                                if (SearchImage.Find(target, Resources.Start_FishPond, out Scan).IsEmpty)
                                {
                                    MessageBox.Show("Failed to use Production Coupons, stopped.");
                                    while (Program.botRunning) { await Task.Delay(100); }
                                }
                            }
                            else
                            {
                                MessageBox.Show("No Production Points Coupons found, stopped.");
                                while (Program.botRunning) { await Task.Delay(100); }
                            }
                        }
                        await Task.Delay(50);
                    }
                    await Task.Delay(50);

                }
                await Task.Delay(20);
            }
        }

        private async Task IsFishComboEvent(IntPtr hWnd, SearchImage.RECT rect)
        {
            Point Scan = Point.Empty;
            while (!SearchImage.PixelSearch(SearchImage.GetBitmapArea(target, 305, 380, 325, 400), FishColorEvent).IsEmpty && !IsFinished)
            {
                target2 = SearchImage.CaptureWindow(hWnd, rect.Right, rect.Bottom);
                if (!SearchImage.Find(target2, Resources.fish_Arrow_left, out Scan).IsEmpty && !IsFinished)
                {
                    await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_LEFT, 5);
                    await Task.Delay(80);
                }
                if (!SearchImage.Find(target2, Resources.fish_Arrow_top, out Scan).IsEmpty && !IsFinished)
                {
                    await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_UP, 5);
                    await Task.Delay(80);
                }
                if (!SearchImage.Find(target2, Resources.fish_Arrow_right, out Scan).IsEmpty && !IsFinished)
                {
                    await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_RIGHT, 5);
                    await Task.Delay(80);
                }
                if (!SearchImage.Find(target2, Resources.fish_Arrow_Bottom, out Scan).IsEmpty && !IsFinished)
                {
                    await BackgroundHelper.SendKey(hWnd, BackgroundHelper.KeyCodes.VK_DOWN, 5);
                    await Task.Delay(80);
                }
                await Task.Delay(20);
            }
        }

        private void TriggerFastMode(Object sender, System.Timers.ElapsedEventArgs e)
        {
            FastModeEnabled = true;
            PressTime = 20;
            WaitTime = 330;
        }

        private void ScreenCapEvent(IntPtr hWnd, SearchImage.RECT rect)
        {
            while (Program.botRunning)
            {
                target = SearchImage.CaptureWindow(hWnd, rect.Right, rect.Bottom);
                Thread.Sleep(10);
            }
        }
    }
}