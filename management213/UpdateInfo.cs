using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace management213
{
    public partial class UpdateInfo : Form
    {
        public UpdateInfo()
        {
            InitializeComponent();
        }

        int tick = 0;
        string click = "";

        private void timer1_Tick(object sender, EventArgs e)
        {
            tick++;

            if (tick == 10)
            {
                button2.BackColor = Color.LightSeaGreen;
                button2.ForeColor = Color.White;
                button2.Enabled = true;
                //Honeydew
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                click += "1";
            }
            if (e.Button == MouseButtons.Middle)
            {
                click += "2";
            }
            if (e.Button == MouseButtons.Right)
            {
                click += "3";
            }

            if (click == "12321")
            {
                button2.Enabled = true;
            }
        }
    }
}
