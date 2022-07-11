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
    public partial class MestoNumber : Form
    {
        public MestoNumber()
        {
            InitializeComponent();
        }

        private void MestoNumber_Shown(object sender, EventArgs e)
        {
            Main owner = (Main)this.Owner;
            string IP = owner.userNameTextBox.Text;
            switch (IP)
            {
                case "192.168.168.2":
                    lbl_number.Text = "№1";
                    break;
                case "192.168.168.3":
                    lbl_number.Text = "№2";
                    break;
                case "192.168.168.4":
                    lbl_number.Text = "№3";
                    break;
                case "192.168.168.7":
                    lbl_number.Text = "№4";
                    break;
                case "192.168.168.8":
                    lbl_number.Text = "№5";
                    break;
                case "192.168.168.9":
                    lbl_number.Text = "№6";
                    break;
                default:
                    lbl_RM.ForeColor = Color.Red;
                    lbl_number.ForeColor = Color.Red;
                    lbl_number.Text = "№?";
                    lbl_RM.PerformLayout();
                    lbl_number.PerformLayout();
                    break;
            }
        }

        private void MestoNumber_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void MestoNumber_Load(object sender, EventArgs e)
        {

        }
    }
}
