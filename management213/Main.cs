using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Web;
using NAudio.Wave;

namespace management213
{

    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        class ButtonClass
        {
            public string idButton;
            public string nameButton;
            public string pathButton;
        }
        class Profile
        {
            public string id;
            public string name;
            public string arrayPaths;
        }

        MestoNumber mesto = new MestoNumber();

        #region переменные для работы чата
        bool alive = false;
        UdpClient client;

        const int LOCALPORT = 8001;
        const int REMOTEPORT = 8001;
        const int TTL = 20;
        const string HOST = "235.5.5.1";
        IPAddress groupAddress;
        string userName;
        #endregion

        #region КЛИК ПО КООРДИНАТАМ
        public static void ClickSomePoint(int x, int y)
        {
            // Set the cursor position
            System.Windows.Forms.Cursor.Position = new Point(x, y);

            DoClickMouse(0x2); // Left mouse button down
            DoClickMouse(0x4); // Left mouse button up
        }
        static void DoClickMouse(int mouseButton)
        {
            var input = new INPUT()
            {
                dwType = 0, // Mouse input
                mi = new MOUSEINPUT() { dwFlags = mouseButton }
            };

            if (SendInput(1, ref input, Marshal.SizeOf(input)) == 0)
            {
                throw new Exception();
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            int dx;
            int dy;
            int mouseData;
            public int dwFlags;
            int time;
            IntPtr dwExtraInfo;
        }
        struct INPUT
        {
            public uint dwType;
            public MOUSEINPUT mi;
        }
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint cInputs, ref INPUT input, int size);
        #endregion

        #region Нажатие клавиш
        static class KeyboardSend
        {
            [DllImport("user32.dll")]
            private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

            private const int KEYEVENTF_EXTENDEDKEY = 1;
            private const int KEYEVENTF_KEYUP = 2;

            public static void KeyDown(Keys vKey)
            {
                keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY, 0);
            }

            public static void KeyUp(Keys vKey)
            {
                keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
            }
        }
        #endregion

        string whoSayScreen = "";
        #region ЧАТ
        private void ReceiveMessages()
        {
            alive = true;
            try
            {
                while (alive)
                {
                    IPEndPoint remoteIp = null;
                    byte[] data = client.Receive(ref remoteIp);
                    string message = Encoding.Unicode.GetString(data);

                    try
                    {
                        if ((((message.Split(':')[1]).Split('%')[0]).ToString().Trim()) == userNameTextBox.Text)
                        {
                            string command = ((message.Split(':')[1]).Split('%')[1]).ToString().Trim();
                            if (command == "shutdown")
                            {
                                message = "Выключить ПК";
                                messageTextBox.Text = "Выключение ПК";
                                sendButton.PerformClick();

                                Process.Start("shutdown", "/s /t 0");
                            }
                            if (command == "ENTER")
                            {
                                message = "Нажать ENTER";
                                messageTextBox.Text = "Нажимаю ENTER";
                                sendButton.PerformClick();

                                KeyboardSend.KeyDown(Keys.Enter);
                                KeyboardSend.KeyUp(Keys.Enter);
                            }
                            if (command.Contains("playVoice"))
                            {
                                string[] tempArrayString = message.Split('*')[1].Split(';');
                                List<string> whatPlay = tempArrayString.ToList<string>();

                                Voice voice = new Voice();
                                voice.playAudio(whatPlay);

                                message = "Проиграть звуки";
                                messageTextBox.Text = "Проигрываю задание";
                                sendButton.PerformClick();
                            }
                            if (command.Contains("play_inst_etaz"))
                            {
                                List<string> whatPlay = new List<string>();
                                whatPlay.Add("инструкция этаж");

                                Voice voice = new Voice();
                                voice.playAudio(whatPlay);

                                message = "Проиграть инструкцию для этажа";
                                messageTextBox.Text = "Проигрываю инструкцию для этажа";
                                sendButton.PerformClick();
                            }
                            if (command.Contains("play_inst_koridor"))
                            {
                                List<string> whatPlay = new List<string>();
                                whatPlay.Add("инструкция коридор");

                                Voice voice = new Voice();
                                voice.playAudio(whatPlay);

                                message = "Проиграть инструкцию для коридора";
                                messageTextBox.Text = "Проигрываю инструкцию для коридора";
                                sendButton.PerformClick();
                            }
                            if (command.Contains("play_snimi"))
                            {
                                List<string> whatPlay = new List<string>();
                                whatPlay.Add("наушники");

                                Voice voice = new Voice();
                                voice.playAudio(whatPlay);

                                message = "Проиграть снятие наушников";
                                messageTextBox.Text = "Проигрываю снятие наушников";
                                sendButton.PerformClick();
                            }
                            if (command == "numberPlace_open")
                            {
                                message = "Открыть номер места";
                                messageTextBox.Text = "Открываю номер места";
                                sendButton.PerformClick();

                                mesto.Show();
                                mesto.Activate();
                                mesto.Focus();
                                mesto.Activate();
                                mesto.Focus();
                                mesto.Activate();
                                mesto.Focus();
                                ClickSomePoint(50, 50);
                            }
                            if (command == "numberPlace_close")
                            {
                                message = "Закрыть номер места";
                                messageTextBox.Text = "закрываю номер места";
                                sendButton.PerformClick();

                                mesto.Hide();
                            }
                            if (command == "onMic1")
                            {
                                message = "Включить микрофон 1";
                                messageTextBox.Text = "Включаю микрофон 1";
                                sendButton.PerformClick();

                                muteControl1.Value = false;
                            }
                            if (command == "offMic1")
                            {
                                message = "Выключить микрофон 1";
                                messageTextBox.Text = "Выключаю микрофон 1";
                                sendButton.PerformClick();

                                muteControl1.Value = true;
                            }
                            if (command == "onMic2")
                            {
                                message = "Включить микрофон 1";
                                messageTextBox.Text = "Включаю микрофон 1";
                                sendButton.PerformClick();

                                muteControl2.Value = false;
                            }
                            if (command == "offMic2")
                            {
                                message = "Выключить микрофон 2";
                                messageTextBox.Text = "Выключаю микрофон 2";
                                sendButton.PerformClick();

                                muteControl2.Value = true;
                            }
                            if (command.Contains("updatePlan"))
                            {
                                UpdatePlan();
                            }
                            if (command.Contains("startRMI"))
                            {
                                ClickSomePoint(1307, 146);
                                ClickSomePoint(1307, 146);
                                timer1.Start();
                            }
                            if (command.Contains("screen"))
                            {
                                whoSayScreen = ((message.Split(':')[0]).Split('%'))[0].Trim();
                                MakeScreenshot();
                            }
                            if (command.Contains("state"))
                            {
                                string[] tempState = message.Split(':');

                                TextBox tbEdit = new TextBox();
                                if (tempState[2] == "192.168.168.2")
                                    tbEdit = tb_state_RMI1;
                                if (tempState[2] == "192.168.168.3")
                                    tbEdit = tb_state_RMI2;
                                if (tempState[2] == "192.168.168.4")
                                    tbEdit = tb_state_RMI3;
                                if (tempState[2] == "192.168.168.7")
                                    tbEdit = tb_state_RMI4;
                                if (tempState[2] == "192.168.168.8")
                                    tbEdit = tb_state_RMI5;
                                if (tempState[2] == "192.168.168.9")
                                    tbEdit = tb_state_RMI6;

                                tbEdit.Text = tempState[3];
                            }
                        }
                    }
                    catch { }

                    if (chatTextBox.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            chatTextBox.Text = message + "\r\n" + chatTextBox.Text;
                        }));
                    }
                    else
                        chatTextBox.Text = message + "\r\n" + chatTextBox.Text;
                }
            }
            catch (ObjectDisposedException)
            {
                if (!alive)
                    return;
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region микрофоны

        WaveInEvent waveInEvent1;
        NAudio.Mixer.MixerLine mixer1;
        NAudio.Mixer.BooleanMixerControl muteControl1;

        WaveInEvent waveInEvent2;
        NAudio.Mixer.MixerLine mixer2;
        NAudio.Mixer.BooleanMixerControl muteControl2;

        #endregion

        List<ButtonClass> buttons = new List<ButtonClass>();
        List<Profile> profiles = new List<Profile>();
        static string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\192.168.168.168\docs\others\@Прочее\management 4\db.mdb";
        string whatEdit = "";
        string pathNew = "";

        private void btn_Click(object sender, EventArgs e)
        {
            int i = 0;
            try
            {
                for (i = 0; i < buttons.Count; i++)
                {
                    if (buttons[i].idButton == ((Button)sender).Name)
                    {
                        Process.Start(buttons[i].pathButton);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Произошла ошибка при открытии папки. Скорее всего, это связано с её отсутствием.\r\n\r\nСообщите следующую информацию разработчику программы:\r\nОписание ошибки: {1}.\r\nИдентификатор ошибки: {0}\r\nВызываемым файл: {2}", ex.StackTrace, ex.Message, buttons[i].pathButton), "Ошибка при открытии папки", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        async void UpdatePlan()
        {
            Task.Run(async delegate
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    OleDbCommand cmd;
                    OleDbDataReader reader;

                    string date = DateTime.Now.Date.ToShortDateString();

                    cmd = new OleDbCommand("SELECT * FROM plan WHERE datePlan='" + date + "'", conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (label1.InvokeRequired)
                        {
                            this.Invoke(new MethodInvoker(() =>
                            {
                                label1.Text = "Запланировано на сегодня: " + reader["countPlan"].ToString();
                            }));
                        }
                        else
                            label1.Text = "Запланировано на сегодня: " + reader["countPlan"].ToString();

                    }
                    reader.Close();
                    conn.Close();
                    conn.Dispose();
                }
            });
        }

        private void Main_Load(object sender, EventArgs e)
        {
            #region подготовка чата
            userNameTextBox.Text = System.Net.Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

            loginButton.Enabled = true;
            logoutButton.Enabled = false;
            sendButton.Enabled = false;
            chatTextBox.ReadOnly = true;
            groupAddress = IPAddress.Parse(HOST);

            loginButton.PerformClick();
            #endregion

            mesto.Owner = this;
            mesto.Show();
            mesto.Hide();

            this.Hide();
            LoadForm loadForm = new LoadForm();
            loadForm.Owner = this;
            loadForm.ShowDialog();
            loadForm.Dispose();

            UpdatePlan();


            #region добавление профилей
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM profiles ORDER BY id", conn);
                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Profile profile = new Profile();
                    profile.id = reader["id"].ToString();
                    profile.name = reader["nameProfile"].ToString();

                    //object temptemp = JsonConvert.DeserializeObject((String) reader["arrayProfile"]);

                    profile.arrayPaths = reader["arrayProfile"] as String;
                    profiles.Add(profile);
                    cb_profiles.Items.Add(profile.name);
                }
                reader.Close();
                conn.Close();
                conn.Dispose();
            }
            #endregion
            #region добавление классов микрофона
            for (int deviceIndex = 0; deviceIndex < WaveIn.DeviceCount; deviceIndex++)
            {
                var device = WaveIn.GetCapabilities(deviceIndex);
            }

            try
            {
                waveInEvent1 = new WaveInEvent();
                waveInEvent1.DeviceNumber = 0;
                mixer1 = waveInEvent1.GetMixerLine();
                muteControl1 = mixer1.Controls.FirstOrDefault(x => x.ControlType == NAudio.Mixer.MixerControlType.Mute) as NAudio.Mixer.BooleanMixerControl;
                muteControl1.Value = true;
            }
            catch (Exception ex)
            {
                if (userNameTextBox.Text == "192.168.168.6")
                    MessageBox.Show("Не обнаружен микрофон 1");
            }

            try
            {
                waveInEvent2 = new WaveInEvent();
                waveInEvent2.DeviceNumber = 1;
                mixer2 = waveInEvent2.GetMixerLine();
                muteControl2 = mixer2.Controls.FirstOrDefault(x => x.ControlType == NAudio.Mixer.MixerControlType.Mute) as NAudio.Mixer.BooleanMixerControl;
                muteControl2.Value = true;
            }
            catch (Exception ex)
            {
                if (userNameTextBox.Text == "192.168.168.6")
                    MessageBox.Show("Не обнаружен микрофон 2");
            }
            #endregion
            cb_profiles.SelectedIndex = 0;


        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            userName = userNameTextBox.Text;
            userNameTextBox.ReadOnly = true;

            try
            {
                client = new UdpClient(LOCALPORT);
                client.JoinMulticastGroup(groupAddress, TTL);
                Task receiveTask = new Task(ReceiveMessages);
                receiveTask.Start();

                string message = userName + " вошёл в чат";
                byte[] data = Encoding.Unicode.GetBytes(message);
                client.Send(data, data.Length, HOST, REMOTEPORT);
                loginButton.Enabled = false;
                logoutButton.Enabled = true;
                sendButton.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            string message = userName + " покинул чат";
            byte[] data = Encoding.Unicode.GetBytes(message);
            client.Send(data, data.Length, HOST, REMOTEPORT);
            client.DropMulticastGroup(groupAddress);

            alive = false;
            client.Close();

            loginButton.Enabled = true;
            logoutButton.Enabled = false;
            sendButton.Enabled = false;
            userNameTextBox.ReadOnly = false;
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            try
            {
                string message = String.Format("{0}: {1}", userName, messageTextBox.Text);
                byte[] data = Encoding.Unicode.GetBytes(message);
                client.Send(data, data.Length, HOST, REMOTEPORT);
                messageTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cb_profiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttons = JsonConvert.DeserializeObject<List<ButtonClass>>(profiles[cb_profiles.SelectedIndex].arrayPaths);

            List<Button> buttonsFrom = new List<Button> { btn_1_1, btn_1_2, btn_1_3, btn_1_4, btn_2_1, btn_2_2, btn_2_3, btn_2_4,
                                                          btn_m_1_1, btn_m_1_2, btn_m_1_3, btn_m_1_4, btn_m_2_1, btn_m_2_2, btn_m_2_3, btn_m_2_4 };

            for (int i = 0; i < buttonsFrom.Count; i++)
            {
                buttonsFrom[i].Text = buttons.Find(x => x.idButton == buttonsFrom[i].Name).nameButton;
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                whatEdit = "план";
                tb_edit.Visible = true;
                btn_save.Visible = true;
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (whatEdit == "план")
            {
                string[] plan = tb_edit.Text.Split(';');

                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    OleDbCommand cmd;
                    OleDbDataReader reader;

                    for (int i = 0; i < plan.Length; i++)
                    {
                        string date = (DateTime.Now.Date.AddDays(i)).ToShortDateString();

                        cmd = new OleDbCommand("SELECT COUNT(*) FROM plan WHERE datePlan='" + date + "'", conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader[0].ToString() == "0")
                            {
                                cmd = new OleDbCommand("INSERT INTO plan(datePlan, countPlan) VALUES('" + date + "', '" + plan[i] + "')", conn);
                                reader = cmd.ExecuteReader();
                            }
                            else
                            {
                                cmd = new OleDbCommand("UPDATE plan SET countPlan='" + plan[i] + "' WHERE datePlan='" + date + "'", conn);
                                reader = cmd.ExecuteReader();
                            }
                        }
                        reader.Close();
                    }
                    conn.Close();
                    conn.Dispose();
                }

                UpdatePlan();
            }
            if (whatEdit == "профиль")
            {
                if (cb_profiles.FindStringExact(tb_edit.Text) == -1)
                {
                    using (OleDbConnection conn = new OleDbConnection(connectionString))
                    {
                        conn.Open();
                        OleDbCommand cmd;
                        OleDbDataReader reader;

                        cmd = new OleDbCommand("INSERT INTO profiles(nameProfile, arrayProfile) VALUES('" + tb_edit.Text + "', '" + profiles[0].arrayPaths + "')", conn);
                        reader = cmd.ExecuteReader();

                        reader.Close();
                        conn.Close();
                        conn.Dispose();
                    }

                    using (OleDbConnection conn = new OleDbConnection(connectionString))
                    {
                        conn.Open();
                        OleDbCommand cmd;
                        OleDbDataReader reader;

                        cmd = new OleDbCommand("SELECT * FROM profiles WHERE nameProfile='" + tb_edit.Text + "'", conn);
                        reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Profile profile = new Profile();
                            profile.id = reader["id"].ToString();
                            profile.name = reader["nameProfile"].ToString();
                            profile.arrayPaths = reader["arrayProfile"] as String;
                            profiles.Add(profile);
                            cb_profiles.Items.Add(profile.name);
                        }

                        reader.Close();
                        conn.Close();
                        conn.Dispose();
                    }
                }
                else
                {
                    MessageBox.Show("Профиль с таким именем уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (whatEdit.IndexOf("кнопка") != -1)
            {
                string nameButton = whatEdit.Split(':')[1];
                int idButton = -1;

                for (int i = 0; i < buttons.Count; i++)
                {
                    if (buttons[i].idButton == nameButton)
                    {
                        idButton = i;
                    }
                }
                if (idButton == -1)
                {
                    MessageBox.Show("Произошла ошибка");
                    return;
                }

                buttons[idButton].nameButton = tb_edit.Text;
                if (pathNew != "")
                {
                    buttons[idButton].pathButton = pathNew;
                }

                #region формирование JSON
                List<ButtonClass> buttons2 = new List<ButtonClass>();

                ButtonClass button = new ButtonClass();

                List<Button> buttonsFrom = new List<Button> { btn_1_1, btn_1_2, btn_1_3, btn_1_4, btn_2_1, btn_2_2, btn_2_3, btn_2_4,
                                                          btn_m_1_1, btn_m_1_2, btn_m_1_3, btn_m_1_4, btn_m_2_1, btn_m_2_2, btn_m_2_3, btn_m_2_4 };

                for (int i = 0; i < buttonsFrom.Count; i++)
                {
                    int tempIDButton = -1;
                    button = new ButtonClass();

                    button.idButton = buttonsFrom[i].Name;

                    for (int j = 0; j < buttons.Count; j++)
                    {
                        if (buttons[j].idButton == button.idButton)
                        {
                            tempIDButton = j;
                            break;
                        }
                    }
                    button.nameButton = buttons[tempIDButton].nameButton;
                    button.pathButton = buttons[tempIDButton].pathButton;
                    buttons2.Add(button);
                }
                
                string json = JsonConvert.SerializeObject(buttons2);
                List<ButtonClass> m = JsonConvert.DeserializeObject<List<ButtonClass>>(json);

                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    OleDbCommand cmd;
                    OleDbDataReader reader;

                    cmd = new OleDbCommand("UPDATE profiles SET arrayProfile='" + json + "' WHERE nameProfile='" + cb_profiles.Text + "'", conn);
                    reader = cmd.ExecuteReader();

                    reader.Close();
                    conn.Close();
                    conn.Dispose();
                }

                buttons.Clear();
                buttons.AddRange(m);

                for (int i = 0; i < profiles.Count; i++)
                {
                    if (profiles[i].name == cb_profiles.Text)
                    {
                        profiles[i].arrayPaths = json;
                    }
                }
                int tempSelect = cb_profiles.SelectedIndex;
                cb_profiles.SelectedIndex = 0;
                cb_profiles.SelectedIndex = tempSelect;
                #endregion

                pathNew = "";
            }
            tb_edit.Visible = false;
            btn_path.Visible = false;
            btn_save.Visible = false;
            whatEdit = "";
            tb_edit.Text = "";
        }

        private void cb_profiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (cb_profiles.FindStringExact(cb_profiles.Text) == -1)
                {
                    MessageBox.Show("Необходимо создать профиль. Для этого нажмите колёсиком мышки на поле выбора профиля.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cb_profiles.SelectedIndex = 0;
                }
                else
                {
                    cb_profiles.SelectedIndex = cb_profiles.FindStringExact(cb_profiles.Text);
                    btn_1_1.Focus();
                }
            }
        }

        private void cb_profiles_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                whatEdit = "профиль";
                tb_edit.Visible = true;
                btn_save.Visible = true;
            }
        }

        private void btn_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                if (cb_profiles.Text == "По умолчанию")
                {
                    MessageBox.Show("Редактирование профиля \"По умолчанию\" запрещено.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                btn_save.Visible = true;
                tb_edit.Visible = true;
                btn_path.Visible = true;
                whatEdit = "кнопка :" + (sender as Button).Name;
            }
        }

        private void btn_path_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                pathNew = folderBrowserDialog1.SelectedPath;
            }
        }

        private void tb_edit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                tb_edit.Visible = false;
                btn_path.Visible = false;
                btn_save.Visible = false;
                whatEdit = "";
                tb_edit.Text = "";
            }
        }

        private void btn_profotbor_Click(object sender, EventArgs e)
        {
            ProfotborDB profotborDB = new ProfotborDB();
            profotborDB.Owner = this;
            profotborDB.Show();
        }

        private void pb_info_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Включить инструкцию по использованию программы?", "Требуется подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

            }
        }

        private void btn_goTo_Click(object sender, EventArgs e)
        {
            GotoSelect gotoSelect = new GotoSelect();
            gotoSelect.Owner = this;
            gotoSelect.Show();
        }

        int tick = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            tick++;
            if (tick== 15)
            {
                timer1.Stop();
                tick = 0;

                KeyboardSend.KeyDown(Keys.Enter);
                KeyboardSend.KeyUp(Keys.Enter);
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private bool IsBlack(Color pixel)
        {
            if ((pixel.R < 50) && (pixel.G < 50) && (pixel.B < 50))
                return true;
            return false;
        }

        private void MakeScreenshot()
        {
            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();

            Rectangle bounds = Screen.GetBounds(Point.Empty);

            using (var bitmap2 = new Bitmap(bounds.Width, bounds.Height))
            {
                using (var g = Graphics.FromImage(bitmap2))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }

                bitmap2.Save("temp.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            Image temp = Image.FromFile("temp.jpg");

            Bitmap bitmap = new Bitmap(temp);

            #region получаем число

            #region 0-9

            #region 0
            if ((IsBlack(bitmap.GetPixel(710, 705))) && (IsBlack(bitmap.GetPixel(711, 705))) &&
                (IsBlack(bitmap.GetPixel(712, 705))) && (IsBlack(bitmap.GetPixel(710, 713))) &&
                (IsBlack(bitmap.GetPixel(711, 713))) && (IsBlack(bitmap.GetPixel(712, 713))) &&
                (IsBlack(bitmap.GetPixel(709, 706))) && (IsBlack(bitmap.GetPixel(709, 707))) &&
                (IsBlack(bitmap.GetPixel(709, 708))) && (IsBlack(bitmap.GetPixel(709, 709))) &&
                (IsBlack(bitmap.GetPixel(709, 710))) && (IsBlack(bitmap.GetPixel(709, 711))) &&
                (IsBlack(bitmap.GetPixel(709, 712))) &&
                (IsBlack(bitmap.GetPixel(713, 706))) && (IsBlack(bitmap.GetPixel(713, 707))) &&
                (IsBlack(bitmap.GetPixel(713, 708))) && (IsBlack(bitmap.GetPixel(713, 709))) &&
                (IsBlack(bitmap.GetPixel(713, 710))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                (IsBlack(bitmap.GetPixel(713, 712))))
            {
                if (textBox1.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        textBox1.Text = "0";
                    }));
                }
                else
                    textBox1.Text = "0";
            }
            #endregion

            #region 1
            if ((IsBlack(bitmap.GetPixel(709, 706))) && (IsBlack(bitmap.GetPixel(710, 706))) &&
                (IsBlack(bitmap.GetPixel(711, 705))) && (IsBlack(bitmap.GetPixel(711, 706))) &&
                (IsBlack(bitmap.GetPixel(711, 707))) && (IsBlack(bitmap.GetPixel(711, 708))) &&
                (IsBlack(bitmap.GetPixel(711, 709))) && (IsBlack(bitmap.GetPixel(711, 710))) &&
                (IsBlack(bitmap.GetPixel(711, 711))) && (IsBlack(bitmap.GetPixel(711, 712))) &&
                (IsBlack(bitmap.GetPixel(711, 713))))
            {
                if (textBox1.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        textBox1.Text = "1";
                    }));
                }
                else
                    textBox1.Text = "1";
            }
            #endregion

            #region 2
            if ((IsBlack(bitmap.GetPixel(709, 706))) &&
                (IsBlack(bitmap.GetPixel(710, 705))) && (IsBlack(bitmap.GetPixel(711, 705))) &&
                (IsBlack(bitmap.GetPixel(712, 705))) &&
                (IsBlack(bitmap.GetPixel(713, 706))) && (IsBlack(bitmap.GetPixel(713, 707))) &&
                (IsBlack(bitmap.GetPixel(713, 708))) &&
                (IsBlack(bitmap.GetPixel(712, 709))) && (IsBlack(bitmap.GetPixel(711, 710))) &&
                (IsBlack(bitmap.GetPixel(710, 711))) && (IsBlack(bitmap.GetPixel(709, 712))) &&
                (IsBlack(bitmap.GetPixel(709, 713))) && (IsBlack(bitmap.GetPixel(710, 713))) &&
                (IsBlack(bitmap.GetPixel(711, 713))) && (IsBlack(bitmap.GetPixel(712, 713))) &&
                (IsBlack(bitmap.GetPixel(713, 713))))
            {
                if (textBox1.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        textBox1.Text = "2";
                    }));
                }
                else
                    textBox1.Text = "2";
            }
            #endregion

            #region 3
            if ((IsBlack(bitmap.GetPixel(709, 706))) &&
                (IsBlack(bitmap.GetPixel(710, 705))) && (IsBlack(bitmap.GetPixel(711, 705))) &&
                (IsBlack(bitmap.GetPixel(712, 705))) &&
                (IsBlack(bitmap.GetPixel(713, 706))) && (IsBlack(bitmap.GetPixel(713, 707))) &&
                (IsBlack(bitmap.GetPixel(713, 708))) &&
                (IsBlack(bitmap.GetPixel(711, 709))) && (IsBlack(bitmap.GetPixel(712, 709))) &&
                (IsBlack(bitmap.GetPixel(713, 710))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                (IsBlack(bitmap.GetPixel(713, 712))) &&
                (IsBlack(bitmap.GetPixel(710, 713))) && (IsBlack(bitmap.GetPixel(711, 713))) &&
                (IsBlack(bitmap.GetPixel(712, 713))) && (IsBlack(bitmap.GetPixel(709, 712))))
            {
                if (textBox1.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        textBox1.Text = "3";
                    }));
                }
                else
                    textBox1.Text = "3";
            }
            #endregion

            #region 4
            if ((IsBlack(bitmap.GetPixel(712, 705))) && (IsBlack(bitmap.GetPixel(712, 705))) &&
                (IsBlack(bitmap.GetPixel(712, 707))) && (IsBlack(bitmap.GetPixel(712, 708))) &&
                (IsBlack(bitmap.GetPixel(712, 709))) && (IsBlack(bitmap.GetPixel(712, 710))) &&
                (IsBlack(bitmap.GetPixel(712, 711))) && (IsBlack(bitmap.GetPixel(712, 712))) &&
                (IsBlack(bitmap.GetPixel(712, 713))) &&
                (IsBlack(bitmap.GetPixel(709, 711))) && (IsBlack(bitmap.GetPixel(710, 711))) &&
                (IsBlack(bitmap.GetPixel(711, 711))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                (IsBlack(bitmap.GetPixel(711, 706))) && (IsBlack(bitmap.GetPixel(711, 707))) &&
                (IsBlack(bitmap.GetPixel(710, 708))) && (IsBlack(bitmap.GetPixel(710, 709))) &&
                (IsBlack(bitmap.GetPixel(709, 710))))
            {
                if (textBox1.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        textBox1.Text = "4";
                    }));
                }
                else
                    textBox1.Text = "4";
            }
            #endregion

            #region 5
            if ((IsBlack(bitmap.GetPixel(709, 705))) && (IsBlack(bitmap.GetPixel(710, 705))) &&
                (IsBlack(bitmap.GetPixel(711, 705))) && (IsBlack(bitmap.GetPixel(712, 705))) &&
                (IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(709, 706))) &&
                (IsBlack(bitmap.GetPixel(709, 707))) && (IsBlack(bitmap.GetPixel(709, 708))) &&
                (IsBlack(bitmap.GetPixel(709, 709))) && (IsBlack(bitmap.GetPixel(710, 708))) &&
                (IsBlack(bitmap.GetPixel(711, 708))) && (IsBlack(bitmap.GetPixel(712, 708))) &&
                (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(713, 710))) &&
                (IsBlack(bitmap.GetPixel(713, 711))) && (IsBlack(bitmap.GetPixel(713, 712))) &&
                (IsBlack(bitmap.GetPixel(710, 713))) && (IsBlack(bitmap.GetPixel(711, 713))) &&
                (IsBlack(bitmap.GetPixel(712, 713))) && (IsBlack(bitmap.GetPixel(709, 712))))
            {
                if (textBox1.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        textBox1.Text = "5";
                    }));
                }
                else
                    textBox1.Text = "5";
            }
            #endregion

            #region 7
            if ((IsBlack(bitmap.GetPixel(709, 705))) && (IsBlack(bitmap.GetPixel(710, 705))) &&
                (IsBlack(bitmap.GetPixel(711, 705))) && (IsBlack(bitmap.GetPixel(712, 705))) &&
                (IsBlack(bitmap.GetPixel(713, 705))) &&
                (IsBlack(bitmap.GetPixel(713, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                (IsBlack(bitmap.GetPixel(712, 708))) && (IsBlack(bitmap.GetPixel(711, 709))) &&
                (IsBlack(bitmap.GetPixel(711, 710))) && (IsBlack(bitmap.GetPixel(710, 711))) &&
                (IsBlack(bitmap.GetPixel(710, 712))))
            {
                if (textBox1.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        textBox1.Text = "7";
                    }));
                }
                else
                    textBox1.Text = "7";
            }
            #endregion

            #region 8
            if ((IsBlack(bitmap.GetPixel(710, 705))) && (IsBlack(bitmap.GetPixel(711, 705))) &&
                (IsBlack(bitmap.GetPixel(712, 705))) &&
                (IsBlack(bitmap.GetPixel(710, 709))) && (IsBlack(bitmap.GetPixel(711, 709))) &&
                (IsBlack(bitmap.GetPixel(712, 709))) &&
                (IsBlack(bitmap.GetPixel(710, 713))) && (IsBlack(bitmap.GetPixel(711, 713))) &&
                (IsBlack(bitmap.GetPixel(712, 713))) &&
                (IsBlack(bitmap.GetPixel(709, 706))) && (IsBlack(bitmap.GetPixel(709, 707))) &&
                (IsBlack(bitmap.GetPixel(709, 708))) &&
                (IsBlack(bitmap.GetPixel(713, 706))) && (IsBlack(bitmap.GetPixel(713, 707))) &&
                (IsBlack(bitmap.GetPixel(713, 708))) &&
                (IsBlack(bitmap.GetPixel(709, 710))) && (IsBlack(bitmap.GetPixel(709, 711))) &&
                (IsBlack(bitmap.GetPixel(709, 712))) &&
                (IsBlack(bitmap.GetPixel(713, 710))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                (IsBlack(bitmap.GetPixel(713, 712))))
            {
                if (textBox1.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        textBox1.Text = "8";
                    }));
                }
                else
                    textBox1.Text = "8";
            }
            #endregion

            #region 9
            if ((IsBlack(bitmap.GetPixel(710, 705))) && (IsBlack(bitmap.GetPixel(711, 705))) &&
                (IsBlack(bitmap.GetPixel(712, 705))) &&
                (IsBlack(bitmap.GetPixel(709, 706))) && (IsBlack(bitmap.GetPixel(709, 707))) &&
                (IsBlack(bitmap.GetPixel(709, 708))) &&
                (IsBlack(bitmap.GetPixel(710, 709))) && (IsBlack(bitmap.GetPixel(711, 709))) &&
                (IsBlack(bitmap.GetPixel(712, 709))) &&
                (IsBlack(bitmap.GetPixel(713, 706))) && (IsBlack(bitmap.GetPixel(713, 707))) &&
                (IsBlack(bitmap.GetPixel(713, 708))) && (IsBlack(bitmap.GetPixel(713, 709))) &&
                (IsBlack(bitmap.GetPixel(713, 710))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                (IsBlack(bitmap.GetPixel(713, 712))) &&
                (IsBlack(bitmap.GetPixel(710, 713))) && (IsBlack(bitmap.GetPixel(711, 713))) &&
                (IsBlack(bitmap.GetPixel(712, 713))) &&
                (IsBlack(bitmap.GetPixel(709, 712))))
            {
                if (textBox1.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        textBox1.Text = "9";
                    }));
                }
                else
                    textBox1.Text = "9";
            }
            #endregion
            #endregion

            #region 10-19

            //проверяем что первая цифра 1

            if ((IsBlack(bitmap.GetPixel(706, 706))) && (IsBlack(bitmap.GetPixel(707, 706))) &&
                (IsBlack(bitmap.GetPixel(708, 705))) && (IsBlack(bitmap.GetPixel(708, 706))) &&
                (IsBlack(bitmap.GetPixel(708, 707))) && (IsBlack(bitmap.GetPixel(708, 708))) &&
                (IsBlack(bitmap.GetPixel(708, 709))) && (IsBlack(bitmap.GetPixel(708, 710))) &&
                (IsBlack(bitmap.GetPixel(708, 711))) && (IsBlack(bitmap.GetPixel(708, 712))) &&
                (IsBlack(bitmap.GetPixel(708, 713))))
            {
                #region 0
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) && (IsBlack(bitmap.GetPixel(713, 713))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))) && (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) && (IsBlack(bitmap.GetPixel(712, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) && (IsBlack(bitmap.GetPixel(716, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "10";
                        }));
                    }
                    else
                        textBox1.Text = "10";
                }
                #endregion

                #region 1
                if ((IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(713, 706))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(714, 706))) &&
                    (IsBlack(bitmap.GetPixel(714, 707))) && (IsBlack(bitmap.GetPixel(714, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 709))) && (IsBlack(bitmap.GetPixel(714, 710))) &&
                    (IsBlack(bitmap.GetPixel(714, 711))) && (IsBlack(bitmap.GetPixel(714, 712))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "11";
                        }));
                    }
                    else
                        textBox1.Text = "11";
                }
                #endregion

                #region 2
                if ((IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) && (IsBlack(bitmap.GetPixel(714, 710))) &&
                    (IsBlack(bitmap.GetPixel(713, 711))) && (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(712, 713))) && (IsBlack(bitmap.GetPixel(713, 713))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))) && (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(716, 713))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "12";
                        }));
                    }
                    else
                        textBox1.Text = "12";
                }
                #endregion

                #region 3
                if ((IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 709))) && (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) && (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "13";
                        }));
                    }
                    else
                        textBox1.Text = "13";
                }
                #endregion

                #region 4
                if ((IsBlack(bitmap.GetPixel(715, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 707))) && (IsBlack(bitmap.GetPixel(715, 708))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) && (IsBlack(bitmap.GetPixel(715, 710))) &&
                    (IsBlack(bitmap.GetPixel(715, 711))) && (IsBlack(bitmap.GetPixel(715, 712))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 711))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                    (IsBlack(bitmap.GetPixel(714, 711))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(714, 706))) && (IsBlack(bitmap.GetPixel(714, 707))) &&
                    (IsBlack(bitmap.GetPixel(713, 708))) && (IsBlack(bitmap.GetPixel(713, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "14";
                        }));
                    }
                    else
                        textBox1.Text = "14";
                }
                #endregion

                #region 5
                if ((IsBlack(bitmap.GetPixel(712, 705))) && (IsBlack(bitmap.GetPixel(713, 705))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 705))) && (IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(712, 707))) && (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(712, 709))) && (IsBlack(bitmap.GetPixel(713, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 708))) && (IsBlack(bitmap.GetPixel(715, 708))) &&
                    (IsBlack(bitmap.GetPixel(716, 709))) && (IsBlack(bitmap.GetPixel(716, 710))) &&
                    (IsBlack(bitmap.GetPixel(716, 711))) && (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) && (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "15";
                        }));
                    }
                    else
                        textBox1.Text = "15";
                }
                #endregion

                #region 6
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) && (IsBlack(bitmap.GetPixel(712, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "16";
                        }));
                    }
                    else
                        textBox1.Text = "16";
                }
                #endregion

                #region 7
                if ((IsBlack(bitmap.GetPixel(712, 705))) && (IsBlack(bitmap.GetPixel(713, 705))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(715, 707))) &&
                    (IsBlack(bitmap.GetPixel(715, 708))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(714, 710))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                    (IsBlack(bitmap.GetPixel(713, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "17";
                        }));
                    }
                    else
                        textBox1.Text = "17";
                }
                #endregion

                #region 8
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "18";
                        }));
                    }
                    else
                        textBox1.Text = "18";
                }
                #endregion

                #region 9
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) && (IsBlack(bitmap.GetPixel(716, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "19";
                        }));
                    }
                    else
                        textBox1.Text = "19";
                }
                #endregion
            }
            #endregion

            #region 20-29

            //проверяем что первая цифра 2

            if ((IsBlack(bitmap.GetPixel(706, 706))) &&
                    (IsBlack(bitmap.GetPixel(707, 705))) && (IsBlack(bitmap.GetPixel(708, 705))) &&
                    (IsBlack(bitmap.GetPixel(709, 705))) &&
                    (IsBlack(bitmap.GetPixel(710, 706))) && (IsBlack(bitmap.GetPixel(710, 707))) &&
                    (IsBlack(bitmap.GetPixel(710, 708))) &&
                    (IsBlack(bitmap.GetPixel(709, 709))) && (IsBlack(bitmap.GetPixel(708, 710))) &&
                    (IsBlack(bitmap.GetPixel(707, 711))) && (IsBlack(bitmap.GetPixel(706, 712))) &&
                    (IsBlack(bitmap.GetPixel(706, 713))) && (IsBlack(bitmap.GetPixel(707, 713))) &&
                    (IsBlack(bitmap.GetPixel(708, 713))) && (IsBlack(bitmap.GetPixel(709, 713))) &&
                    (IsBlack(bitmap.GetPixel(710, 713))))
            {
                #region 0
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) && (IsBlack(bitmap.GetPixel(713, 713))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))) && (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) && (IsBlack(bitmap.GetPixel(712, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) && (IsBlack(bitmap.GetPixel(716, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "20";
                        }));
                    }
                    else
                        textBox1.Text = "20";
                }
                #endregion

                #region 1
                if ((IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(713, 706))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(714, 706))) &&
                    (IsBlack(bitmap.GetPixel(714, 707))) && (IsBlack(bitmap.GetPixel(714, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 709))) && (IsBlack(bitmap.GetPixel(714, 710))) &&
                    (IsBlack(bitmap.GetPixel(714, 711))) && (IsBlack(bitmap.GetPixel(714, 712))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "21";
                        }));
                    }
                    else
                        textBox1.Text = "21";
                }
                #endregion

                #region 2
                if ((IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) && (IsBlack(bitmap.GetPixel(714, 710))) &&
                    (IsBlack(bitmap.GetPixel(713, 711))) && (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(712, 713))) && (IsBlack(bitmap.GetPixel(713, 713))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))) && (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(716, 713))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "22";
                        }));
                    }
                    else
                        textBox1.Text = "22";
                }
                #endregion

                #region 3
                if ((IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 709))) && (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) && (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "23";
                        }));
                    }
                    else
                        textBox1.Text = "23";
                }
                #endregion

                #region 4
                if ((IsBlack(bitmap.GetPixel(715, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 707))) && (IsBlack(bitmap.GetPixel(715, 708))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) && (IsBlack(bitmap.GetPixel(715, 710))) &&
                    (IsBlack(bitmap.GetPixel(715, 711))) && (IsBlack(bitmap.GetPixel(715, 712))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 711))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                    (IsBlack(bitmap.GetPixel(714, 711))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(714, 706))) && (IsBlack(bitmap.GetPixel(714, 707))) &&
                    (IsBlack(bitmap.GetPixel(713, 708))) && (IsBlack(bitmap.GetPixel(713, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "24";
                        }));
                    }
                    else
                        textBox1.Text = "24";
                }
                #endregion

                #region 5
                if ((IsBlack(bitmap.GetPixel(712, 705))) && (IsBlack(bitmap.GetPixel(713, 705))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 705))) && (IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(712, 707))) && (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(712, 709))) && (IsBlack(bitmap.GetPixel(713, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 708))) && (IsBlack(bitmap.GetPixel(715, 708))) &&
                    (IsBlack(bitmap.GetPixel(716, 709))) && (IsBlack(bitmap.GetPixel(716, 710))) &&
                    (IsBlack(bitmap.GetPixel(716, 711))) && (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) && (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "25";
                        }));
                    }
                    else
                        textBox1.Text = "25";
                }
                #endregion

                #region 6
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) && (IsBlack(bitmap.GetPixel(712, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "26";
                        }));
                    }
                    else
                        textBox1.Text = "26";
                }
                #endregion

                #region 7
                if ((IsBlack(bitmap.GetPixel(712, 705))) && (IsBlack(bitmap.GetPixel(713, 705))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(715, 707))) &&
                    (IsBlack(bitmap.GetPixel(715, 708))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(714, 710))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                    (IsBlack(bitmap.GetPixel(713, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "27";
                        }));
                    }
                    else
                        textBox1.Text = "27";
                }
                #endregion

                #region 8
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "28";
                        }));
                    }
                    else
                        textBox1.Text = "28";
                }
                #endregion

                #region 9
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) && (IsBlack(bitmap.GetPixel(716, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "29";
                        }));
                    }
                    else
                        textBox1.Text = "29";
                }
                #endregion
            }
            #endregion

            #region 30-39

            //проверяем что первая цифра 3

            if ((IsBlack(bitmap.GetPixel(706, 706))) &&
                    (IsBlack(bitmap.GetPixel(707, 705))) && (IsBlack(bitmap.GetPixel(708, 705))) &&
                    (IsBlack(bitmap.GetPixel(709, 705))) &&
                    (IsBlack(bitmap.GetPixel(710, 706))) && (IsBlack(bitmap.GetPixel(710, 707))) &&
                    (IsBlack(bitmap.GetPixel(710, 708))) &&
                    (IsBlack(bitmap.GetPixel(708, 709))) && (IsBlack(bitmap.GetPixel(709, 709))) &&
                    (IsBlack(bitmap.GetPixel(710, 710))) && (IsBlack(bitmap.GetPixel(710, 711))) &&
                    (IsBlack(bitmap.GetPixel(710, 712))) &&
                    (IsBlack(bitmap.GetPixel(707, 713))) && (IsBlack(bitmap.GetPixel(708, 713))) &&
                    (IsBlack(bitmap.GetPixel(709, 713))) && (IsBlack(bitmap.GetPixel(706, 712))))
            {
                #region 0
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) && (IsBlack(bitmap.GetPixel(713, 713))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))) && (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) && (IsBlack(bitmap.GetPixel(712, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) && (IsBlack(bitmap.GetPixel(716, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "30";
                        }));
                    }
                    else
                        textBox1.Text = "30";
                }
                #endregion

                #region 1
                if ((IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(713, 706))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(714, 706))) &&
                    (IsBlack(bitmap.GetPixel(714, 707))) && (IsBlack(bitmap.GetPixel(714, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 709))) && (IsBlack(bitmap.GetPixel(714, 710))) &&
                    (IsBlack(bitmap.GetPixel(714, 711))) && (IsBlack(bitmap.GetPixel(714, 712))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "31";
                        }));
                    }
                    else
                        textBox1.Text = "31";
                }
                #endregion

                #region 2
                if ((IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) && (IsBlack(bitmap.GetPixel(714, 710))) &&
                    (IsBlack(bitmap.GetPixel(713, 711))) && (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(712, 713))) && (IsBlack(bitmap.GetPixel(713, 713))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))) && (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(716, 713))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "32";
                        }));
                    }
                    else
                        textBox1.Text = "32";
                }
                #endregion

                #region 3
                if ((IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 709))) && (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) && (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "33";
                        }));
                    }
                    else
                        textBox1.Text = "33";
                }
                #endregion

                #region 4
                if ((IsBlack(bitmap.GetPixel(715, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 707))) && (IsBlack(bitmap.GetPixel(715, 708))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) && (IsBlack(bitmap.GetPixel(715, 710))) &&
                    (IsBlack(bitmap.GetPixel(715, 711))) && (IsBlack(bitmap.GetPixel(715, 712))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 711))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                    (IsBlack(bitmap.GetPixel(714, 711))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(714, 706))) && (IsBlack(bitmap.GetPixel(714, 707))) &&
                    (IsBlack(bitmap.GetPixel(713, 708))) && (IsBlack(bitmap.GetPixel(713, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "34";
                        }));
                    }
                    else
                        textBox1.Text = "34";
                }
                #endregion

                #region 5
                if ((IsBlack(bitmap.GetPixel(712, 705))) && (IsBlack(bitmap.GetPixel(713, 705))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 705))) && (IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(712, 707))) && (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(712, 709))) && (IsBlack(bitmap.GetPixel(713, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 708))) && (IsBlack(bitmap.GetPixel(715, 708))) &&
                    (IsBlack(bitmap.GetPixel(716, 709))) && (IsBlack(bitmap.GetPixel(716, 710))) &&
                    (IsBlack(bitmap.GetPixel(716, 711))) && (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) && (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "35";
                        }));
                    }
                    else
                        textBox1.Text = "35";
                }
                #endregion

                #region 6
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) && (IsBlack(bitmap.GetPixel(712, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "36";
                        }));
                    }
                    else
                        textBox1.Text = "36";
                }
                #endregion

                #region 7
                if ((IsBlack(bitmap.GetPixel(712, 705))) && (IsBlack(bitmap.GetPixel(713, 705))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(715, 707))) &&
                    (IsBlack(bitmap.GetPixel(715, 708))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(714, 710))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                    (IsBlack(bitmap.GetPixel(713, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "37";
                        }));
                    }
                    else
                        textBox1.Text = "37";
                }
                #endregion

                #region 8
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "38";
                        }));
                    }
                    else
                        textBox1.Text = "38";
                }
                #endregion

                #region 9
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) && (IsBlack(bitmap.GetPixel(716, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "39";
                        }));
                    }
                    else
                        textBox1.Text = "39";
                }
                #endregion
            }
            #endregion

            #region 40-49

            //проверяем что первая цифра 4

            if ((IsBlack(bitmap.GetPixel(709, 705))) && (IsBlack(bitmap.GetPixel(709, 705))) &&
                    (IsBlack(bitmap.GetPixel(709, 707))) && (IsBlack(bitmap.GetPixel(709, 708))) &&
                    (IsBlack(bitmap.GetPixel(709, 709))) && (IsBlack(bitmap.GetPixel(709, 710))) &&
                    (IsBlack(bitmap.GetPixel(709, 711))) && (IsBlack(bitmap.GetPixel(709, 712))) &&
                    (IsBlack(bitmap.GetPixel(709, 713))) &&
                    (IsBlack(bitmap.GetPixel(706, 711))) && (IsBlack(bitmap.GetPixel(707, 711))) &&
                    (IsBlack(bitmap.GetPixel(708, 711))) && (IsBlack(bitmap.GetPixel(710, 711))) &&
                    (IsBlack(bitmap.GetPixel(708, 706))) && (IsBlack(bitmap.GetPixel(708, 707))) &&
                    (IsBlack(bitmap.GetPixel(707, 708))) && (IsBlack(bitmap.GetPixel(707, 709))) &&
                    (IsBlack(bitmap.GetPixel(706, 710))))
            {
                #region 0
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) && (IsBlack(bitmap.GetPixel(713, 713))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))) && (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) && (IsBlack(bitmap.GetPixel(712, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) && (IsBlack(bitmap.GetPixel(716, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "40";
                        }));
                    }
                    else
                        textBox1.Text = "40";
                }
                #endregion

                #region 1
                if ((IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(713, 706))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(714, 706))) &&
                    (IsBlack(bitmap.GetPixel(714, 707))) && (IsBlack(bitmap.GetPixel(714, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 709))) && (IsBlack(bitmap.GetPixel(714, 710))) &&
                    (IsBlack(bitmap.GetPixel(714, 711))) && (IsBlack(bitmap.GetPixel(714, 712))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "41";
                        }));
                    }
                    else
                        textBox1.Text = "41";
                }
                #endregion

                #region 2
                if ((IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) && (IsBlack(bitmap.GetPixel(714, 710))) &&
                    (IsBlack(bitmap.GetPixel(713, 711))) && (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(712, 713))) && (IsBlack(bitmap.GetPixel(713, 713))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))) && (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(716, 713))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "42";
                        }));
                    }
                    else
                        textBox1.Text = "42";
                }
                #endregion

                #region 3
                if ((IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 709))) && (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) && (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "43";
                        }));
                    }
                    else
                        textBox1.Text = "43";
                }
                #endregion

                #region 4
                if ((IsBlack(bitmap.GetPixel(715, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 707))) && (IsBlack(bitmap.GetPixel(715, 708))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) && (IsBlack(bitmap.GetPixel(715, 710))) &&
                    (IsBlack(bitmap.GetPixel(715, 711))) && (IsBlack(bitmap.GetPixel(715, 712))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 711))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                    (IsBlack(bitmap.GetPixel(714, 711))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(714, 706))) && (IsBlack(bitmap.GetPixel(714, 707))) &&
                    (IsBlack(bitmap.GetPixel(713, 708))) && (IsBlack(bitmap.GetPixel(713, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "44";
                        }));
                    }
                    else
                        textBox1.Text = "44";
                }
                #endregion

                #region 5
                if ((IsBlack(bitmap.GetPixel(712, 705))) && (IsBlack(bitmap.GetPixel(713, 705))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 705))) && (IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(712, 707))) && (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(712, 709))) && (IsBlack(bitmap.GetPixel(713, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 708))) && (IsBlack(bitmap.GetPixel(715, 708))) &&
                    (IsBlack(bitmap.GetPixel(716, 709))) && (IsBlack(bitmap.GetPixel(716, 710))) &&
                    (IsBlack(bitmap.GetPixel(716, 711))) && (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) && (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "45";
                        }));
                    }
                    else
                        textBox1.Text = "45";
                }
                #endregion

                #region 6
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) && (IsBlack(bitmap.GetPixel(712, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "46";
                        }));
                    }
                    else
                        textBox1.Text = "46";
                }
                #endregion

                #region 7
                if ((IsBlack(bitmap.GetPixel(712, 705))) && (IsBlack(bitmap.GetPixel(713, 705))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(715, 707))) &&
                    (IsBlack(bitmap.GetPixel(715, 708))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(714, 710))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                    (IsBlack(bitmap.GetPixel(713, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "47";
                        }));
                    }
                    else
                        textBox1.Text = "47";
                }
                #endregion

                #region 8
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "48";
                        }));
                    }
                    else
                        textBox1.Text = "48";
                }
                #endregion

                #region 9
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) && (IsBlack(bitmap.GetPixel(716, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "49";
                        }));
                    }
                    else
                        textBox1.Text = "49";
                }
                #endregion
            }
            #endregion

            #region 50-59

            //проверяем что первая цифра 5

            if ((IsBlack(bitmap.GetPixel(706, 705))) && (IsBlack(bitmap.GetPixel(707, 705))) &&
                    (IsBlack(bitmap.GetPixel(708, 705))) && (IsBlack(bitmap.GetPixel(709, 705))) &&
                    (IsBlack(bitmap.GetPixel(710, 705))) && (IsBlack(bitmap.GetPixel(706, 706))) &&
                    (IsBlack(bitmap.GetPixel(706, 707))) && (IsBlack(bitmap.GetPixel(706, 708))) &&
                    (IsBlack(bitmap.GetPixel(706, 709))) && (IsBlack(bitmap.GetPixel(707, 708))) &&
                    (IsBlack(bitmap.GetPixel(708, 708))) && (IsBlack(bitmap.GetPixel(709, 708))) &&
                    (IsBlack(bitmap.GetPixel(710, 709))) && (IsBlack(bitmap.GetPixel(710, 710))) &&
                    (IsBlack(bitmap.GetPixel(710, 711))) && (IsBlack(bitmap.GetPixel(710, 712))) &&
                    (IsBlack(bitmap.GetPixel(707, 713))) && (IsBlack(bitmap.GetPixel(708, 713))) &&
                    (IsBlack(bitmap.GetPixel(709, 713))) && (IsBlack(bitmap.GetPixel(706, 712))))
            {
                #region 0
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) && (IsBlack(bitmap.GetPixel(713, 713))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))) && (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) && (IsBlack(bitmap.GetPixel(712, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) && (IsBlack(bitmap.GetPixel(716, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "50";
                        }));
                    }
                    else
                        textBox1.Text = "50";
                }
                #endregion

                #region 1
                if ((IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(713, 706))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(714, 706))) &&
                    (IsBlack(bitmap.GetPixel(714, 707))) && (IsBlack(bitmap.GetPixel(714, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 709))) && (IsBlack(bitmap.GetPixel(714, 710))) &&
                    (IsBlack(bitmap.GetPixel(714, 711))) && (IsBlack(bitmap.GetPixel(714, 712))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "51";
                        }));
                    }
                    else
                        textBox1.Text = "51";
                }
                #endregion

                #region 2
                if ((IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) && (IsBlack(bitmap.GetPixel(714, 710))) &&
                    (IsBlack(bitmap.GetPixel(713, 711))) && (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(712, 713))) && (IsBlack(bitmap.GetPixel(713, 713))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))) && (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(716, 713))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "52";
                        }));
                    }
                    else
                        textBox1.Text = "52";
                }
                #endregion

                #region 3
                if ((IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 709))) && (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) && (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "53";
                        }));
                    }
                    else
                        textBox1.Text = "53";
                }
                #endregion

                #region 4
                if ((IsBlack(bitmap.GetPixel(715, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 707))) && (IsBlack(bitmap.GetPixel(715, 708))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) && (IsBlack(bitmap.GetPixel(715, 710))) &&
                    (IsBlack(bitmap.GetPixel(715, 711))) && (IsBlack(bitmap.GetPixel(715, 712))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 711))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                    (IsBlack(bitmap.GetPixel(714, 711))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(714, 706))) && (IsBlack(bitmap.GetPixel(714, 707))) &&
                    (IsBlack(bitmap.GetPixel(713, 708))) && (IsBlack(bitmap.GetPixel(713, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "54";
                        }));
                    }
                    else
                        textBox1.Text = "54";
                }
                #endregion

                #region 5
                if ((IsBlack(bitmap.GetPixel(712, 705))) && (IsBlack(bitmap.GetPixel(713, 705))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 705))) && (IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(712, 707))) && (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(712, 709))) && (IsBlack(bitmap.GetPixel(713, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 708))) && (IsBlack(bitmap.GetPixel(715, 708))) &&
                    (IsBlack(bitmap.GetPixel(716, 709))) && (IsBlack(bitmap.GetPixel(716, 710))) &&
                    (IsBlack(bitmap.GetPixel(716, 711))) && (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) && (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "55";
                        }));
                    }
                    else
                        textBox1.Text = "55";
                }
                #endregion

                #region 6
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) && (IsBlack(bitmap.GetPixel(712, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "56";
                        }));
                    }
                    else
                        textBox1.Text = "56";
                }
                #endregion

                #region 7
                if ((IsBlack(bitmap.GetPixel(712, 705))) && (IsBlack(bitmap.GetPixel(713, 705))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(715, 707))) &&
                    (IsBlack(bitmap.GetPixel(715, 708))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(714, 710))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                    (IsBlack(bitmap.GetPixel(713, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "57";
                        }));
                    }
                    else
                        textBox1.Text = "57";
                }
                #endregion

                #region 8
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "58";
                        }));
                    }
                    else
                        textBox1.Text = "58";
                }
                #endregion

                #region 9
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) && (IsBlack(bitmap.GetPixel(716, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "59";
                        }));
                    }
                    else
                        textBox1.Text = "59";
                }
                #endregion
            }
            #endregion

            #region 60-69

            //проверяем что первая цифра 6

            if ((IsBlack(bitmap.GetPixel(707, 705))) && (IsBlack(bitmap.GetPixel(708, 705))) &&
                    (IsBlack(bitmap.GetPixel(709, 705))) &&
                    (IsBlack(bitmap.GetPixel(710, 706))) &&
                    (IsBlack(bitmap.GetPixel(706, 706))) && (IsBlack(bitmap.GetPixel(706, 707))) &&
                    (IsBlack(bitmap.GetPixel(706, 708))) && (IsBlack(bitmap.GetPixel(706, 709))) &&
                    (IsBlack(bitmap.GetPixel(706, 710))) && (IsBlack(bitmap.GetPixel(706, 711))) &&
                    (IsBlack(bitmap.GetPixel(706, 712))) &&
                    (IsBlack(bitmap.GetPixel(707, 709))) && (IsBlack(bitmap.GetPixel(708, 709))) &&
                    (IsBlack(bitmap.GetPixel(709, 709))) &&
                    (IsBlack(bitmap.GetPixel(707, 713))) && (IsBlack(bitmap.GetPixel(708, 713))) &&
                    (IsBlack(bitmap.GetPixel(709, 713))) &&
                    (IsBlack(bitmap.GetPixel(710, 710))) && (IsBlack(bitmap.GetPixel(710, 711))) &&
                    (IsBlack(bitmap.GetPixel(710, 712))))
            {
                #region 0
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) && (IsBlack(bitmap.GetPixel(713, 713))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))) && (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) && (IsBlack(bitmap.GetPixel(712, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) && (IsBlack(bitmap.GetPixel(716, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "60";
                        }));
                    }
                    else
                        textBox1.Text = "60";
                }
                #endregion

                #region 1
                if ((IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(713, 706))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(714, 706))) &&
                    (IsBlack(bitmap.GetPixel(714, 707))) && (IsBlack(bitmap.GetPixel(714, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 709))) && (IsBlack(bitmap.GetPixel(714, 710))) &&
                    (IsBlack(bitmap.GetPixel(714, 711))) && (IsBlack(bitmap.GetPixel(714, 712))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "61";
                        }));
                    }
                    else
                        textBox1.Text = "61";
                }
                #endregion

                #region 2
                if ((IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) && (IsBlack(bitmap.GetPixel(714, 710))) &&
                    (IsBlack(bitmap.GetPixel(713, 711))) && (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(712, 713))) && (IsBlack(bitmap.GetPixel(713, 713))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))) && (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(716, 713))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "62";
                        }));
                    }
                    else
                        textBox1.Text = "62";
                }
                #endregion

                #region 3
                if ((IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 709))) && (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) && (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "63";
                        }));
                    }
                    else
                        textBox1.Text = "63";
                }
                #endregion

                #region 4
                if ((IsBlack(bitmap.GetPixel(715, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 707))) && (IsBlack(bitmap.GetPixel(715, 708))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) && (IsBlack(bitmap.GetPixel(715, 710))) &&
                    (IsBlack(bitmap.GetPixel(715, 711))) && (IsBlack(bitmap.GetPixel(715, 712))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 711))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                    (IsBlack(bitmap.GetPixel(714, 711))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(714, 706))) && (IsBlack(bitmap.GetPixel(714, 707))) &&
                    (IsBlack(bitmap.GetPixel(713, 708))) && (IsBlack(bitmap.GetPixel(713, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "64";
                        }));
                    }
                    else
                        textBox1.Text = "64";
                }
                #endregion

                #region 5
                if ((IsBlack(bitmap.GetPixel(712, 705))) && (IsBlack(bitmap.GetPixel(713, 705))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 705))) && (IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(712, 707))) && (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(712, 709))) && (IsBlack(bitmap.GetPixel(713, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 708))) && (IsBlack(bitmap.GetPixel(715, 708))) &&
                    (IsBlack(bitmap.GetPixel(716, 709))) && (IsBlack(bitmap.GetPixel(716, 710))) &&
                    (IsBlack(bitmap.GetPixel(716, 711))) && (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) && (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "65";
                        }));
                    }
                    else
                        textBox1.Text = "65";
                }
                #endregion

                #region 6
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) && (IsBlack(bitmap.GetPixel(712, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "66";
                        }));
                    }
                    else
                        textBox1.Text = "66";
                }
                #endregion

                #region 7
                if ((IsBlack(bitmap.GetPixel(712, 705))) && (IsBlack(bitmap.GetPixel(713, 705))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(715, 707))) &&
                    (IsBlack(bitmap.GetPixel(715, 708))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(714, 710))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                    (IsBlack(bitmap.GetPixel(713, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "67";
                        }));
                    }
                    else
                        textBox1.Text = "67";
                }
                #endregion

                #region 8
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "68";
                        }));
                    }
                    else
                        textBox1.Text = "68";
                }
                #endregion

                #region 9
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) && (IsBlack(bitmap.GetPixel(716, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "69";
                        }));
                    }
                    else
                        textBox1.Text = "69";
                }
                #endregion
            }
            #endregion

            #region 70-79

            //проверяем что первая цифра 7

            if ((IsBlack(bitmap.GetPixel(706, 705))) && (IsBlack(bitmap.GetPixel(707, 705))) &&
                    (IsBlack(bitmap.GetPixel(708, 705))) && (IsBlack(bitmap.GetPixel(709, 705))) &&
                    (IsBlack(bitmap.GetPixel(710, 705))) &&
                    (IsBlack(bitmap.GetPixel(710, 706))) && (IsBlack(bitmap.GetPixel(709, 707))) &&
                    (IsBlack(bitmap.GetPixel(709, 708))) && (IsBlack(bitmap.GetPixel(708, 709))) &&
                    (IsBlack(bitmap.GetPixel(708, 710))) && (IsBlack(bitmap.GetPixel(707, 711))) &&
                    (IsBlack(bitmap.GetPixel(707, 712))))
            {
                #region 0
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) && (IsBlack(bitmap.GetPixel(713, 713))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))) && (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) && (IsBlack(bitmap.GetPixel(712, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) && (IsBlack(bitmap.GetPixel(716, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "70";
                        }));
                    }
                    else
                        textBox1.Text = "70";
                }
                #endregion

                #region 1
                if ((IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(713, 706))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(714, 706))) &&
                    (IsBlack(bitmap.GetPixel(714, 707))) && (IsBlack(bitmap.GetPixel(714, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 709))) && (IsBlack(bitmap.GetPixel(714, 710))) &&
                    (IsBlack(bitmap.GetPixel(714, 711))) && (IsBlack(bitmap.GetPixel(714, 712))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "71";
                        }));
                    }
                    else
                        textBox1.Text = "71";
                }
                #endregion

                #region 2
                if ((IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) && (IsBlack(bitmap.GetPixel(714, 710))) &&
                    (IsBlack(bitmap.GetPixel(713, 711))) && (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(712, 713))) && (IsBlack(bitmap.GetPixel(713, 713))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))) && (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(716, 713))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "72";
                        }));
                    }
                    else
                        textBox1.Text = "72";
                }
                #endregion

                #region 3
                if ((IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 709))) && (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) && (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "73";
                        }));
                    }
                    else
                        textBox1.Text = "73";
                }
                #endregion

                #region 4
                if ((IsBlack(bitmap.GetPixel(715, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 707))) && (IsBlack(bitmap.GetPixel(715, 708))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) && (IsBlack(bitmap.GetPixel(715, 710))) &&
                    (IsBlack(bitmap.GetPixel(715, 711))) && (IsBlack(bitmap.GetPixel(715, 712))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 711))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                    (IsBlack(bitmap.GetPixel(714, 711))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(714, 706))) && (IsBlack(bitmap.GetPixel(714, 707))) &&
                    (IsBlack(bitmap.GetPixel(713, 708))) && (IsBlack(bitmap.GetPixel(713, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "74";
                        }));
                    }
                    else
                        textBox1.Text = "74";
                }
                #endregion

                #region 5
                if ((IsBlack(bitmap.GetPixel(712, 705))) && (IsBlack(bitmap.GetPixel(713, 705))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 705))) && (IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(712, 707))) && (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(712, 709))) && (IsBlack(bitmap.GetPixel(713, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 708))) && (IsBlack(bitmap.GetPixel(715, 708))) &&
                    (IsBlack(bitmap.GetPixel(716, 709))) && (IsBlack(bitmap.GetPixel(716, 710))) &&
                    (IsBlack(bitmap.GetPixel(716, 711))) && (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) && (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "75";
                        }));
                    }
                    else
                        textBox1.Text = "75";
                }
                #endregion

                #region 6
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) && (IsBlack(bitmap.GetPixel(712, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "76";
                        }));
                    }
                    else
                        textBox1.Text = "76";
                }
                #endregion

                #region 7
                if ((IsBlack(bitmap.GetPixel(712, 705))) && (IsBlack(bitmap.GetPixel(713, 705))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(715, 707))) &&
                    (IsBlack(bitmap.GetPixel(715, 708))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(714, 710))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                    (IsBlack(bitmap.GetPixel(713, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "77";
                        }));
                    }
                    else
                        textBox1.Text = "77";
                }
                #endregion

                #region 8
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "78";
                        }));
                    }
                    else
                        textBox1.Text = "78";
                }
                #endregion

                #region 9
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) && (IsBlack(bitmap.GetPixel(716, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "79";
                        }));
                    }
                    else
                        textBox1.Text = "79";
                }
                #endregion
            }
            #endregion

            #region 80-89

            //проверяем что первая цифра 8

            if ((IsBlack(bitmap.GetPixel(707, 705))) && (IsBlack(bitmap.GetPixel(708, 705))) &&
                    (IsBlack(bitmap.GetPixel(709, 705))) &&
                    (IsBlack(bitmap.GetPixel(707, 709))) && (IsBlack(bitmap.GetPixel(708, 709))) &&
                    (IsBlack(bitmap.GetPixel(709, 709))) &&
                    (IsBlack(bitmap.GetPixel(707, 713))) && (IsBlack(bitmap.GetPixel(708, 713))) &&
                    (IsBlack(bitmap.GetPixel(709, 713))) &&
                    (IsBlack(bitmap.GetPixel(706, 706))) && (IsBlack(bitmap.GetPixel(706, 707))) &&
                    (IsBlack(bitmap.GetPixel(706, 708))) &&
                    (IsBlack(bitmap.GetPixel(710, 706))) && (IsBlack(bitmap.GetPixel(710, 707))) &&
                    (IsBlack(bitmap.GetPixel(710, 708))) &&
                    (IsBlack(bitmap.GetPixel(706, 710))) && (IsBlack(bitmap.GetPixel(706, 711))) &&
                    (IsBlack(bitmap.GetPixel(706, 712))) &&
                    (IsBlack(bitmap.GetPixel(710, 710))) && (IsBlack(bitmap.GetPixel(710, 711))) &&
                    (IsBlack(bitmap.GetPixel(710, 712))))
            {
                #region 0
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) && (IsBlack(bitmap.GetPixel(713, 713))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))) && (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) && (IsBlack(bitmap.GetPixel(712, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) && (IsBlack(bitmap.GetPixel(716, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "80";
                        }));
                    }
                    else
                        textBox1.Text = "80";
                }
                #endregion

                #region 1
                if ((IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(713, 706))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(714, 706))) &&
                    (IsBlack(bitmap.GetPixel(714, 707))) && (IsBlack(bitmap.GetPixel(714, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 709))) && (IsBlack(bitmap.GetPixel(714, 710))) &&
                    (IsBlack(bitmap.GetPixel(714, 711))) && (IsBlack(bitmap.GetPixel(714, 712))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "81";
                        }));
                    }
                    else
                        textBox1.Text = "81";
                }
                #endregion

                #region 2
                if ((IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) && (IsBlack(bitmap.GetPixel(714, 710))) &&
                    (IsBlack(bitmap.GetPixel(713, 711))) && (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(712, 713))) && (IsBlack(bitmap.GetPixel(713, 713))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))) && (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(716, 713))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "82";
                        }));
                    }
                    else
                        textBox1.Text = "82";
                }
                #endregion

                #region 3
                if ((IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 709))) && (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) && (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "83";
                        }));
                    }
                    else
                        textBox1.Text = "83";
                }
                #endregion

                #region 4
                if ((IsBlack(bitmap.GetPixel(715, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 707))) && (IsBlack(bitmap.GetPixel(715, 708))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) && (IsBlack(bitmap.GetPixel(715, 710))) &&
                    (IsBlack(bitmap.GetPixel(715, 711))) && (IsBlack(bitmap.GetPixel(715, 712))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 711))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                    (IsBlack(bitmap.GetPixel(714, 711))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(714, 706))) && (IsBlack(bitmap.GetPixel(714, 707))) &&
                    (IsBlack(bitmap.GetPixel(713, 708))) && (IsBlack(bitmap.GetPixel(713, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "84";
                        }));
                    }
                    else
                        textBox1.Text = "84";
                }
                #endregion

                #region 5
                if ((IsBlack(bitmap.GetPixel(712, 705))) && (IsBlack(bitmap.GetPixel(713, 705))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 705))) && (IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(712, 707))) && (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(712, 709))) && (IsBlack(bitmap.GetPixel(713, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 708))) && (IsBlack(bitmap.GetPixel(715, 708))) &&
                    (IsBlack(bitmap.GetPixel(716, 709))) && (IsBlack(bitmap.GetPixel(716, 710))) &&
                    (IsBlack(bitmap.GetPixel(716, 711))) && (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) && (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "85";
                        }));
                    }
                    else
                        textBox1.Text = "85";
                }
                #endregion

                #region 6
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) && (IsBlack(bitmap.GetPixel(712, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "86";
                        }));
                    }
                    else
                        textBox1.Text = "86";
                }
                #endregion

                #region 7
                if ((IsBlack(bitmap.GetPixel(712, 705))) && (IsBlack(bitmap.GetPixel(713, 705))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(715, 707))) &&
                    (IsBlack(bitmap.GetPixel(715, 708))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(714, 710))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                    (IsBlack(bitmap.GetPixel(713, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "87";
                        }));
                    }
                    else
                        textBox1.Text = "87";
                }
                #endregion

                #region 8
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "88";
                        }));
                    }
                    else
                        textBox1.Text = "88";
                }
                #endregion

                #region 9
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) && (IsBlack(bitmap.GetPixel(716, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "89";
                        }));
                    }
                    else
                        textBox1.Text = "89";
                }
                #endregion
            }
            #endregion

            #region 90-99

            //проверяем что первая цифра 9

            if ((IsBlack(bitmap.GetPixel(707, 705))) && (IsBlack(bitmap.GetPixel(708, 705))) &&
                    (IsBlack(bitmap.GetPixel(709, 705))) &&
                    (IsBlack(bitmap.GetPixel(706, 706))) && (IsBlack(bitmap.GetPixel(706, 707))) &&
                    (IsBlack(bitmap.GetPixel(706, 708))) &&
                    (IsBlack(bitmap.GetPixel(707, 709))) && (IsBlack(bitmap.GetPixel(708, 709))) &&
                    (IsBlack(bitmap.GetPixel(709, 709))) &&
                    (IsBlack(bitmap.GetPixel(710, 706))) && (IsBlack(bitmap.GetPixel(710, 707))) &&
                    (IsBlack(bitmap.GetPixel(710, 708))) && (IsBlack(bitmap.GetPixel(710, 709))) &&
                    (IsBlack(bitmap.GetPixel(710, 710))) && (IsBlack(bitmap.GetPixel(710, 711))) &&
                    (IsBlack(bitmap.GetPixel(710, 712))) &&
                    (IsBlack(bitmap.GetPixel(707, 713))) && (IsBlack(bitmap.GetPixel(708, 713))) &&
                    (IsBlack(bitmap.GetPixel(709, 713))) &&
                    (IsBlack(bitmap.GetPixel(706, 712))))
            {
                #region 0
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) && (IsBlack(bitmap.GetPixel(713, 713))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))) && (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) && (IsBlack(bitmap.GetPixel(712, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) && (IsBlack(bitmap.GetPixel(716, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "90";
                        }));
                    }
                    else
                        textBox1.Text = "90";
                }
                #endregion

                #region 1
                if ((IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(713, 706))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(714, 706))) &&
                    (IsBlack(bitmap.GetPixel(714, 707))) && (IsBlack(bitmap.GetPixel(714, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 709))) && (IsBlack(bitmap.GetPixel(714, 710))) &&
                    (IsBlack(bitmap.GetPixel(714, 711))) && (IsBlack(bitmap.GetPixel(714, 712))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "91";
                        }));
                    }
                    else
                        textBox1.Text = "91";
                }
                #endregion

                #region 2
                if ((IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) && (IsBlack(bitmap.GetPixel(714, 710))) &&
                    (IsBlack(bitmap.GetPixel(713, 711))) && (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(712, 713))) && (IsBlack(bitmap.GetPixel(713, 713))) &&
                    (IsBlack(bitmap.GetPixel(714, 713))) && (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(716, 713))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "92";
                        }));
                    }
                    else
                        textBox1.Text = "92";
                }
                #endregion

                #region 3
                if ((IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 709))) && (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) && (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "93";
                        }));
                    }
                    else
                        textBox1.Text = "93";
                }
                #endregion

                #region 4
                if ((IsBlack(bitmap.GetPixel(715, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 707))) && (IsBlack(bitmap.GetPixel(715, 708))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) && (IsBlack(bitmap.GetPixel(715, 710))) &&
                    (IsBlack(bitmap.GetPixel(715, 711))) && (IsBlack(bitmap.GetPixel(715, 712))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 711))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                    (IsBlack(bitmap.GetPixel(714, 711))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(714, 706))) && (IsBlack(bitmap.GetPixel(714, 707))) &&
                    (IsBlack(bitmap.GetPixel(713, 708))) && (IsBlack(bitmap.GetPixel(713, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "94";
                        }));
                    }
                    else
                        textBox1.Text = "94";
                }
                #endregion

                #region 5
                if ((IsBlack(bitmap.GetPixel(712, 705))) && (IsBlack(bitmap.GetPixel(713, 705))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 705))) && (IsBlack(bitmap.GetPixel(712, 706))) &&
                    (IsBlack(bitmap.GetPixel(712, 707))) && (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(712, 709))) && (IsBlack(bitmap.GetPixel(713, 708))) &&
                    (IsBlack(bitmap.GetPixel(714, 708))) && (IsBlack(bitmap.GetPixel(715, 708))) &&
                    (IsBlack(bitmap.GetPixel(716, 709))) && (IsBlack(bitmap.GetPixel(716, 710))) &&
                    (IsBlack(bitmap.GetPixel(716, 711))) && (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) && (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "95";
                        }));
                    }
                    else
                        textBox1.Text = "95";
                }
                #endregion

                #region 6
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) && (IsBlack(bitmap.GetPixel(712, 709))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "96";
                        }));
                    }
                    else
                        textBox1.Text = "96";
                }
                #endregion

                #region 7
                if ((IsBlack(bitmap.GetPixel(712, 705))) && (IsBlack(bitmap.GetPixel(713, 705))) &&
                    (IsBlack(bitmap.GetPixel(714, 705))) && (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 705))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(715, 707))) &&
                    (IsBlack(bitmap.GetPixel(715, 708))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(714, 710))) && (IsBlack(bitmap.GetPixel(713, 711))) &&
                    (IsBlack(bitmap.GetPixel(713, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "97";
                        }));
                    }
                    else
                        textBox1.Text = "97";
                }
                #endregion

                #region 8
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) &&
                    (IsBlack(bitmap.GetPixel(712, 710))) && (IsBlack(bitmap.GetPixel(712, 711))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "98";
                        }));
                    }
                    else
                        textBox1.Text = "98";
                }
                #endregion

                #region 9
                if ((IsBlack(bitmap.GetPixel(713, 705))) && (IsBlack(bitmap.GetPixel(714, 705))) &&
                    (IsBlack(bitmap.GetPixel(715, 705))) &&
                    (IsBlack(bitmap.GetPixel(712, 706))) && (IsBlack(bitmap.GetPixel(712, 707))) &&
                    (IsBlack(bitmap.GetPixel(712, 708))) &&
                    (IsBlack(bitmap.GetPixel(713, 709))) && (IsBlack(bitmap.GetPixel(714, 709))) &&
                    (IsBlack(bitmap.GetPixel(715, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 706))) && (IsBlack(bitmap.GetPixel(716, 707))) &&
                    (IsBlack(bitmap.GetPixel(716, 708))) && (IsBlack(bitmap.GetPixel(716, 709))) &&
                    (IsBlack(bitmap.GetPixel(716, 710))) && (IsBlack(bitmap.GetPixel(716, 711))) &&
                    (IsBlack(bitmap.GetPixel(716, 712))) &&
                    (IsBlack(bitmap.GetPixel(713, 713))) && (IsBlack(bitmap.GetPixel(714, 713))) &&
                    (IsBlack(bitmap.GetPixel(715, 713))) &&
                    (IsBlack(bitmap.GetPixel(712, 712))))
                {
                    if (textBox1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            textBox1.Text = "99";
                        }));
                    }
                    else
                        textBox1.Text = "99";
                }
                #endregion
            }
            #endregion

            #endregion

            if ((bitmap.GetPixel(444, 342).R > 200))
            {
                if (textBox1.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        textBox1.Text = "Ошибка";
                    }));
                }
                else
                    textBox1.Text = "Ошибка";
            }

            if ((bitmap.GetPixel(444, 397).G == 198))
            {
                if (textBox1.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        textBox1.Text = "Конец";
                    }));
                }
                else
                    textBox1.Text = "Конец";
            }

            if (textBox1.Text == "")
            {
                if (textBox1.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        textBox1.Text = "Неизвестно";
                    }));
                }
                else
                    textBox1.Text = "Неизвестно";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (textBox1.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        messageTextBox.Text = whoSayScreen + "%state:" + userNameTextBox.Text + ":" + textBox1.Text;
                    }));
                }
                else
                    messageTextBox.Text = whoSayScreen + "%state:" + userNameTextBox.Text + ":" + textBox1.Text;

                sendButton.PerformClick();
                textBox1.Text = "";
            }
        }

        private void tb_state_RMI1_TextChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Текущее состояние РМИ 1: " + tb_state_RMI1.Text, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tb_state_RMI2_TextChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Текущее состояние РМИ 2: " + tb_state_RMI2.Text, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tb_state_RMI3_TextChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Текущее состояние РМИ 3: " + tb_state_RMI3.Text, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tb_state_RMI4_TextChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Текущее состояние РМИ 4: " + tb_state_RMI4.Text, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tb_state_RMI5_TextChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Текущее состояние РМИ 5: " + tb_state_RMI5.Text, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tb_state_RMI6_TextChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Текущее состояние РМИ 6: " + tb_state_RMI6.Text, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
