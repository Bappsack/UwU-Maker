using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace UwU_Maker
{
    public partial class Form1 : Form
    {

        private List<int> HWndList = new List<int>();
        private List<IntPtr> WindowList = new List<IntPtr>();
        private List<Thread> BotList = new List<Thread>();

        public bool Reset { get; private set; }
        public bool IsStarted { get; private set; }

        [DllImport("user32.dll")]
        static extern bool SetWindowText(IntPtr hWnd, string text);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern int EnumWindows(CallbackDef callback, int lParam);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(int hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetClientRect(IntPtr hwnd, out SearchImage.RECT lpRect);


        private delegate bool CallbackDef(int hWnd, int lParam);

        private void RefreshHandle()
        {
            this.HWndList.Clear();
            this.listBox1.Items.Clear();
            CallbackDef callback = new CallbackDef(this.ShowWindowHandler);
            EnumWindows(callback, 0);
        }

        public enum MinigameList
        {
            FishPond = 1,
            SawMill = 2,
            Quarry = 3,
            ShootingField = 4
        }

        private bool ShowWindowHandler(int hWnd, int lParam)
        {
            StringBuilder stringBuilder = new StringBuilder(255);
            GetWindowText(hWnd, stringBuilder, 255);
            string text = stringBuilder.ToString();
            if (text.Contains("NosTale"))
            {
                this.HWndList.Insert(0, hWnd);
                this.listBox1.Items.Insert(0, "NosTale - (" + hWnd.ToString() + ")");
                if (!Reset)
                {
                    SetWindowText((IntPtr)hWnd, "NosTale - (" + hWnd.ToString() + ")");
                }
                else
                {
                    SetWindowText((IntPtr)hWnd, "NosTale");
                }
            }
            return true;
        }


        public Form1()
        {

          
            InitializeComponent();
            if (!Properties.Settings.Default.Disclaimer)
            {
                MessageBox.Show("DISCLAIMER:\n\n IF U PAID FOR THIS SOFTWARE U GOT SCAMMED!!!\n\n\n" +
                  "This Bot was made by Panda~ from Elitepvpers.com.\nNo Bot can gurantee u won't get banned for using Bots, so if u get banned for using this Software its your own fault!\n" +
                  "This Software is designed for Semi Modern Computers, if you're running a slow machine u might run into issues.\n"
                  );
                Properties.Settings.Default.Disclaimer = true;
                Properties.Settings.Default.Save();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshHandle();
            textBox2.Text = Properties.Settings.Default.LootTimeA;
            textBox3.Text = Properties.Settings.Default.LootTimeB;
            textBox4.Text = Properties.Settings.Default.FailChance.ToString();

            //  RefreshClients(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RefreshHandle();
        }

        private void Form_Closed(object sender, FormClosedEventArgs e)
        {

            Properties.Settings.Default.LootTimeA = textBox2.Text;
            Properties.Settings.Default.LootTimeB = textBox3.Text;
            Properties.Settings.Default.FailChance = Convert.ToDouble(textBox4.Text);
            Properties.Settings.Default.Save();

            Reset = true;
            Program.botRunning = false;
            RefreshHandle();
        }

        private void button5_Click(object sender, EventArgs e)
        {


            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("No Client selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            IntPtr ClientHWND = FindWindow("TNosTaleMainF", listBox1.SelectedItem.ToString());



            SearchImage.RECT rect;
            GetClientRect(ClientHWND, out rect);
            string Resolution = (rect.Right.ToString() + "x" + rect.Bottom.ToString());

            if (Resolution != "1024x768")
            {
                MessageBox.Show("Game need to be on 1024x768 Resolution!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (WindowList.Contains(ClientHWND))
            {
                MessageBox.Show("This Client is already in the List!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            WindowList.Add(ClientHWND);

            MinigameList Game = GetWantedMinigame();

            string BotID = listBox1.SelectedItem.ToString().Replace("NosTale", "").Replace("- (", "").Replace(")", "");
            string Title = listBox1.SelectedItem.ToString();
            int Amount = Convert.ToInt32(textBox1.Text);

            if ((int)Game == 1) { BotList.Add(new Thread(() => new Minigames.FishPond().RunTask(FindWindow("TNosTaleMainF", Title), Amount, rect, Convert.ToInt32(BotID), checkBox1.Checked, checkBox2.Checked, Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox3.Text), Convert.ToDouble("0." + textBox4.Text)))); }
            if ((int)Game == 2) { BotList.Add(new Thread(() => new Minigames.SawMill().RunTask(FindWindow("TNosTaleMainF", Title), Amount, rect, Convert.ToInt32(BotID), checkBox1.Checked))); }
            if ((int)Game == 3) { BotList.Add(new Thread(() => new Minigames.Quarry().RunTask(FindWindow("TNosTaleMainF", Title), Amount, rect, Convert.ToInt32(BotID), checkBox1.Checked))); }
            //if ((int)Game == 4) { BotList.Add(new Thread(() => new Minigames.ShootingField().RunTask(FindWindow("TNosTaleMainF",Title), Amount, rect, Convert.ToInt32(BotID), checkBox1.Checked))); }

            /*
            if ((int)Game == 4)
            {
                MessageBox.Show("Shooting Field isn't supported yet!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                radioButton1.Checked = true;
                return;
            }
            */


            string[] row = { BotID, GetWantedMinigame().ToString(), comboBox1.Text, checkBox1.Checked.ToString(), textBox1.Text };
            var bot = new ListViewItem(row);
            listView1.Items.Add(bot);

            MessageBox.Show($"{listBox1.SelectedItem.ToString()} added to the Bot List!\n\nMinigame: {Game}\nWanted Level: {comboBox1.Text}\nAmount: {textBox1.Text}\nUse Reward Coupons: {checkBox1.Checked.ToString()}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private MinigameList GetWantedMinigame()
        {
            int wantedGame = 0;
            if (radioButton1.Checked) { wantedGame = 1; }
            if (radioButton2.Checked) { wantedGame = 2; }
            if (radioButton3.Checked) { wantedGame = 3; }
            if (radioButton4.Checked) { wantedGame = 4; }

            return (MinigameList)Enum.Parse(typeof(MinigameList), wantedGame.ToString(), true);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (BotList.Count == 0)
            {
                MessageBox.Show("No Bots Added!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (!IsStarted)
            {
                MessageBox.Show("Bots aren't started yet!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int Amount = BotList.Count;
            foreach (Thread Bot in BotList)
            {
                Bot.Abort(1);
            }
            Program.botRunning = false;
            BotList.Clear();
            WindowList.Clear();
            listView1.Items.Clear();
            IsStarted = false;
            MessageBox.Show($"{Amount} Bots have been Stopped and removed from List!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (BotList.Count == 0)
            {
                MessageBox.Show("No Bots Added!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(IsStarted)
            {
                MessageBox.Show("Already Started!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Program.botRunning = true;

            Invoke(new Action(() =>
            {
                int Amount = BotList.Count;
                foreach (Thread Bot in BotList)
                {
                    Bot.Start();
                }
                MessageBox.Show($"{Amount} Bots have been started!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                IsStarted = true;
            }));


        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.elitepvpers.com/forum/nostale/");
        }
    }
}
