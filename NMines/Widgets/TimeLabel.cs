using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;


namespace NMines.Widgets
{
    public class TimeLabel : Label
    {
        private System.Timers.Timer gameTimer;
        private int gameTime;

        public TimeLabel()
        {
            Text = "00:00";
            MinimumSize = new Size(40, 25);
            Dock = DockStyle.Fill;
            TextAlign = ContentAlignment.MiddleCenter;
            Font = new Font("Segoe UI", 18);

            InitTimer();
        }

        private void InitTimer()
        {
            gameTimer = new System.Timers.Timer(1000);
            gameTimer.Elapsed += GameTimer_Tick;
            gameTimer.AutoReset = true;
        }

        private void GameTimer_Tick(object sender, ElapsedEventArgs e)
        {
            gameTime++;
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateTimer));
                return;
            }
            TimeSpan time = TimeSpan.FromSeconds(gameTime);
            Text = time.ToString(@"mm\:ss");
        }


        public string GetGameTime()
        {
            return gameTime.ToString();
        }


        public void StartTimer()
        {
            gameTime = 0;
            UpdateTimer();
            gameTimer.Start();
        }

        public void StopTimer()
        {
            gameTimer.Stop();
        }

        public void RestartTimer()
        {
            gameTime = 0;
            UpdateTimer();
            StopTimer();
            StartTimer();
        }
    }
}
