using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace management213
{
    public partial class ProfotborRMI : Form
    {
        public ProfotborRMI()
        {
            InitializeComponent();
        }

        Main grandOwner;
        ProfotborDB owner;

        bool micUse = false;

        #region все ряды
        private void btn_allLeft_Click(object sender, EventArgs e)
        {
            if (cb_RMI4.Checked)
            {
                cb_RMI4.Checked = false;
                cb_RMI5.Checked = false;
                cb_RMI6.Checked = false;
            }
            else
            {
                cb_RMI4.Checked = true;
                cb_RMI5.Checked = true;
                cb_RMI6.Checked = true;
            }
        }

        private void btn_allRight_Click(object sender, EventArgs e)
        {
            if (cb_RMI1.Checked)
            {
                cb_RMI1.Checked = false;
                cb_RMI2.Checked = false;
                cb_RMI3.Checked = false;
            }
            else
            {
                cb_RMI1.Checked = true;
                cb_RMI2.Checked = true;
                cb_RMI3.Checked = true;
            }
        }
        #endregion

        private void ProfotborRMI_Load(object sender, EventArgs e)
        {
            owner = (ProfotborDB)this.Owner;
            grandOwner = (Main)owner.Owner;
        }

        private void btn_usingMic_Click(object sender, EventArgs e)
        {
            int typeMic;
            if (grandOwner.userNameTextBox.Text == "192.168.168.1")
                typeMic = 1;
            else if (grandOwner.userNameTextBox.Text == "192.168.168.15")
                typeMic = 2;
            else
                return;

            if (micUse)
            {
                grandOwner.messageTextBox.Text = "192.168.168.6%offMic" + typeMic.ToString();
                grandOwner.sendButton.PerformClick();
                micUse = false;
                btn_usingMic.Text = "Использовать микрофон";
            }
            else
            {
                grandOwner.messageTextBox.Text = "192.168.168.6%onMic" + typeMic.ToString();
                grandOwner.sendButton.PerformClick();
                micUse = true;
                btn_usingMic.Text = "Выключить микрофон";
            }
        }

        private void btn_play_Click(object sender, EventArgs e)
        {
            string typeRayon = "";
            if (rb_all.Checked)
                typeRayon = "all";
            else if (rb_withOut.Checked)
                typeRayon = "without";
            else
                typeRayon = "withoutEnd";

            int countSelect = 0;

            List<string> rayons = new List<string>();
            foreach (object comboBox in checkedListBox1.CheckedItems)
            {
                switch (comboBox.ToString())
                {
                    case "Адмиралтейский и Кировский":
                        rayons.Add("rayons admkir " + typeRayon);
                        countSelect++;
                        break;
                    case "Василеостровский":
                        rayons.Add("rayons vas " + typeRayon);
                        countSelect++;
                        break;
                    case "Выборгский":
                        rayons.Add("rayons vyb " + typeRayon);
                        countSelect++;
                        break;
                    case "Калининский":
                        rayons.Add("rayons kal " + typeRayon);
                        countSelect++;
                        break;
                    case "Колпинский":
                        rayons.Add("rayons kolp " + typeRayon);
                        countSelect++;
                        break;
                    case "Красногвардейский":
                        rayons.Add("rayons krgv " + typeRayon);
                        countSelect++;
                        break;
                    case "Красносельский":
                        rayons.Add("rayons krsel " + typeRayon);
                        countSelect++;
                        break;
                    case "Кронштадтский и Курортный":
                        rayons.Add("rayons krkur " + typeRayon);
                        countSelect++;
                        break;
                    case "Московский":
                        rayons.Add("rayons mos " + typeRayon);
                        countSelect++;
                        break;
                    case "Невский":
                        rayons.Add("rayons nev " + typeRayon);
                        countSelect++;
                        break;
                    case "Петроградский":
                        rayons.Add("rayons pgr " + typeRayon);
                        countSelect++;
                        break;
                    case "Петродворцовый":
                        rayons.Add("rayons pdv " + typeRayon);
                        countSelect++;
                        break;
                    case "Приморский":
                        rayons.Add("rayons prim " + typeRayon);
                        countSelect++;
                        break;
                    case "Пушкинский":
                        rayons.Add("rayons push " + typeRayon);
                        countSelect++;
                        break;
                    case "Фрунзенский":
                        rayons.Add("rayons frun " + typeRayon);
                        countSelect++;
                        break;
                    case "Центральный":
                        rayons.Add("rayons centr " + typeRayon);
                        countSelect++;
                        break;
                }
            }

            string whatPlay = "";
            whatPlay = "192.168.168.6%playVoice*";
            for (int i = 0; i < rayons.Count; i++)
            {
                whatPlay += rayons[i] + ";";
            }

            if (countSelect == 0)
            {
                whatPlay = "192.168.168.6%playVoice*приглашение";
            }

            grandOwner.messageTextBox.Text = whatPlay;
            grandOwner.sendButton.PerformClick();

            for (int i = 0; i < 16; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
        }

        private void btn_shutdown_Click(object sender, EventArgs e)
        {
            if (cb_RMI1.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.2%shutdown";
                grandOwner.sendButton.PerformClick();
                cb_RMI1.Checked = false;
            }
            if (cb_RMI2.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.3%shutdown";
                grandOwner.sendButton.PerformClick();
                cb_RMI2.Checked = false;
            }
            if (cb_RMI3.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.4%shutdown";
                grandOwner.sendButton.PerformClick();
                cb_RMI3.Checked = false;
            }
            if (cb_RMI4.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.7%shutdown";
                grandOwner.sendButton.PerformClick();
                cb_RMI4.Checked = false;
            }
            if (cb_RMI5.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.8%shutdown";
                grandOwner.sendButton.PerformClick();
                cb_RMI5.Checked = false;
            }
            if (cb_RMI6.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.9%shutdown";
                grandOwner.sendButton.PerformClick();
                cb_RMI6.Checked = false;
            }
        }

        private void btn_onPC_Click(object sender, EventArgs e)
        {
            if (cb_RMI1.Checked)
            {
                wakeFunction("B06EBF32ECDB");
                cb_RMI1.Checked = false;
            }
            if (cb_RMI2.Checked)
            {
                wakeFunction("B06EBF33030A");
                cb_RMI2.Checked = false;
            }
            if (cb_RMI3.Checked)
            {
                wakeFunction("B06EBF32D308");
                cb_RMI3.Checked = false;
            }
            if (cb_RMI4.Checked)
            {
                wakeFunction("B06EBF32EDC2");
                cb_RMI4.Checked = false;
            }
            if (cb_RMI5.Checked)
            {
                wakeFunction("B06EBF32EDCE");
                cb_RMI5.Checked = false;
            }
            if (cb_RMI6.Checked)
            {
                wakeFunction("B06EBF32EDCC");
                cb_RMI6.Checked = false;
            }
        }

        private void wakeFunction(string MAC_ADDRESS)
        {
            WOLClass client = new WOLClass();
            client.Connect(new IPAddress(0xffffffff), 0x2fff);
            client.SetClientToBrodcastMode();
            int counter = 0;
            byte[] bytes = new byte[1024];

            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;

            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
                    bytes[counter++] = byte.Parse(MAC_ADDRESS.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                    i += 2;
                }
            }

            int reterned_value = client.Send(bytes, 1024);
        }

        private void btn_instr_Click(object sender, EventArgs e)
        {
            string typeInst = "";
            if (rb_etaz.Checked)
                typeInst = "etaz";
            else
                typeInst = "koridor";
                if (cb_RMI1.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.2%play_inst_" + typeInst;
                grandOwner.sendButton.PerformClick();
                grandOwner.messageTextBox.Text = "192.168.168.2%numberPlace_close";
                grandOwner.sendButton.PerformClick();
                cb_RMI1.Checked = false;
            }
            if (cb_RMI2.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.3%play_inst_" + typeInst;
                grandOwner.sendButton.PerformClick();
                grandOwner.messageTextBox.Text = "192.168.168.3%numberPlace_close";
                grandOwner.sendButton.PerformClick();
                cb_RMI2.Checked = false;
            }
            if (cb_RMI3.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.4%play_inst_" + typeInst;
                grandOwner.sendButton.PerformClick();
                grandOwner.messageTextBox.Text = "192.168.168.4%numberPlace_close";
                grandOwner.sendButton.PerformClick();
                cb_RMI3.Checked = false;
            }
            if (cb_RMI4.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.7%play_inst_" + typeInst;
                grandOwner.sendButton.PerformClick();
                grandOwner.messageTextBox.Text = "192.168.168.7%numberPlace_close";
                grandOwner.sendButton.PerformClick();
                cb_RMI4.Checked = false;
            }
            if (cb_RMI5.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.8%play_inst_" + typeInst;
                grandOwner.sendButton.PerformClick();
                grandOwner.messageTextBox.Text = "192.168.168.8%numberPlace_close";
                grandOwner.sendButton.PerformClick();
                cb_RMI5.Checked = false;
            }
            if (cb_RMI6.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.9%play_inst_" + typeInst;
                grandOwner.sendButton.PerformClick();
                grandOwner.messageTextBox.Text = "192.168.168.9%numberPlace_close";
                grandOwner.sendButton.PerformClick();
                cb_RMI6.Checked = false;
            }
        }

        private void btn_naush_Click(object sender, EventArgs e)
        {
            if (cb_RMI1.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.2%play_snimi";
                grandOwner.sendButton.PerformClick();
                cb_RMI1.Checked = false;
            }
            if (cb_RMI2.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.3%play_snimi";
                grandOwner.sendButton.PerformClick();
                cb_RMI2.Checked = false;
            }
            if (cb_RMI3.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.4%play_snimi";
                grandOwner.sendButton.PerformClick();
                cb_RMI3.Checked = false;
            }
            if (cb_RMI4.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.7%play_snimi";
                grandOwner.sendButton.PerformClick();
                cb_RMI4.Checked = false;
            }
            if (cb_RMI5.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.8%play_snimi";
                grandOwner.sendButton.PerformClick();
                cb_RMI5.Checked = false;
            }
            if (cb_RMI6.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.9%play_snimi";
                grandOwner.sendButton.PerformClick();
                cb_RMI6.Checked = false;
            }
        }

        private void btn_showNumber_Click(object sender, EventArgs e)
        {
            if (cb_RMI1.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.2%numberPlace_open";
                grandOwner.sendButton.PerformClick();
                cb_RMI1.Checked = false;
            }
            if (cb_RMI2.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.3%numberPlace_open";
                grandOwner.sendButton.PerformClick();
                cb_RMI2.Checked = false;
            }
            if (cb_RMI3.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.4%numberPlace_open";
                grandOwner.sendButton.PerformClick();
                cb_RMI3.Checked = false;
            }
            if (cb_RMI4.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.7%numberPlace_open";
                grandOwner.sendButton.PerformClick();
                cb_RMI4.Checked = false;
            }
            if (cb_RMI5.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.8%numberPlace_open";
                grandOwner.sendButton.PerformClick();
                cb_RMI5.Checked = false;
            }
            if (cb_RMI6.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.9%numberPlace_open";
                grandOwner.sendButton.PerformClick();
                cb_RMI6.Checked = false;
            }
        }

        private void btn_closeNumber_Click(object sender, EventArgs e)
        {
            if (cb_RMI1.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.2%numberPlace_close";
                grandOwner.sendButton.PerformClick();
                cb_RMI1.Checked = false;
            }
            if (cb_RMI2.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.3%numberPlace_close";
                grandOwner.sendButton.PerformClick();
                cb_RMI2.Checked = false;
            }
            if (cb_RMI3.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.4%numberPlace_close";
                grandOwner.sendButton.PerformClick();
                cb_RMI3.Checked = false;
            }
            if (cb_RMI4.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.7%numberPlace_close";
                grandOwner.sendButton.PerformClick();
                cb_RMI4.Checked = false;
            }
            if (cb_RMI5.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.8%numberPlace_close";
                grandOwner.sendButton.PerformClick();
                cb_RMI5.Checked = false;
            }
            if (cb_RMI6.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.9%numberPlace_close";
                grandOwner.sendButton.PerformClick();
                cb_RMI6.Checked = false;
            }
        }

        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi)]
        protected static extern int mciSendString
            (string mciCommand,
            StringBuilder returnValue,
            int returnLength,
            IntPtr callback);
        async Task<string> openCloseAsync(string what)
        {
            await Task.Run(() =>
            {
                if (what == "open")
                    mciSendString("set cdaudio door open", null, 0, IntPtr.Zero);
                else
                    mciSendString("set cdaudio door closed", null, 0, IntPtr.Zero);
            });
            return "0";
        }

        private void btn_openCD_Click(object sender, EventArgs e)
        {
            openCloseAsync("open");
        }

        private void btn_closeCD_Click(object sender, EventArgs e)
        {
            openCloseAsync("closed");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cb_RMI1.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.2%startRMI";
                grandOwner.sendButton.PerformClick();
                cb_RMI1.Checked = false;
            }
            if (cb_RMI2.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.3%startRMI";
                grandOwner.sendButton.PerformClick();
                cb_RMI2.Checked = false;
            }
            if (cb_RMI3.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.4%startRMI";
                grandOwner.sendButton.PerformClick();
                cb_RMI3.Checked = false;
            }
            if (cb_RMI4.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.7%startRMI";
                grandOwner.sendButton.PerformClick();
                cb_RMI4.Checked = false;
            }
            if (cb_RMI5.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.8%startRMI";
                grandOwner.sendButton.PerformClick();
                cb_RMI5.Checked = false;
            }
            if (cb_RMI6.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.9%startRMI";
                grandOwner.sendButton.PerformClick();
                cb_RMI6.Checked = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cb_RMI1.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.2%ENTER";
                grandOwner.sendButton.PerformClick();
                cb_RMI1.Checked = false;
            }
            if (cb_RMI2.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.3%ENTER";
                grandOwner.sendButton.PerformClick();
                cb_RMI2.Checked = false;
            }
            if (cb_RMI3.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.4%ENTER";
                grandOwner.sendButton.PerformClick();
                cb_RMI3.Checked = false;
            }
            if (cb_RMI4.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.7%ENTER";
                grandOwner.sendButton.PerformClick();
                cb_RMI4.Checked = false;
            }
            if (cb_RMI5.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.8%ENTER";
                grandOwner.sendButton.PerformClick();
                cb_RMI5.Checked = false;
            }
            if (cb_RMI6.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.9%ENTER";
                grandOwner.sendButton.PerformClick();
                cb_RMI6.Checked = false;
            }
        }

        private void btn_status_Click(object sender, EventArgs e)
        {

            if (cb_RMI1.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.2%screen";
                grandOwner.sendButton.PerformClick();
                cb_RMI1.Checked = false;
            }
            if (cb_RMI2.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.3%screen";
                grandOwner.sendButton.PerformClick();
                cb_RMI2.Checked = false;
            }
            if (cb_RMI3.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.4%screen";
                grandOwner.sendButton.PerformClick();
                cb_RMI3.Checked = false;
            }
            if (cb_RMI4.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.7%screen";
                grandOwner.sendButton.PerformClick();
                cb_RMI4.Checked = false;
            }
            if (cb_RMI5.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.8%screen";
                grandOwner.sendButton.PerformClick();
                cb_RMI5.Checked = false;
            }
            if (cb_RMI6.Checked)
            {
                grandOwner.messageTextBox.Text = "192.168.168.9%screen";
                grandOwner.sendButton.PerformClick();
                cb_RMI6.Checked = false;
            }
        }
    }
}
