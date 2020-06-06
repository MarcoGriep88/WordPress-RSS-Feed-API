using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WordPressRSSApi;

namespace RSSFeed
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            WordpressFeedController wp = WordpressFeedController.Instance;

            wp.LoadRSS("http://www.its-all-about.de/rss");

            var rssItem = wp.GetNewestItem();

            this.label1.Text = rssItem.Title;
            this.richTextBox1.Text = wp.RemoveHTMLFromText(rssItem.Summary.Substring(0, 300)) + "...";

            this.button1.Click += (s, e) => {
                Process.Start(rssItem.Id);
            };
        }

        protected override void OnLoad(EventArgs e)
        {
            PlaceLowerRight();
            base.OnLoad(e);
        }

        private void PlaceLowerRight()
        {
            Screen rightmost = Screen.AllScreens[0];
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.Right > rightmost.WorkingArea.Right)
                    rightmost = screen;
            }

            this.Left = rightmost.WorkingArea.Right - this.Width;
            this.Top = rightmost.WorkingArea.Bottom - this.Height;
        }
    }
}
