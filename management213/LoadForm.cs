using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace management213
{
    public partial class LoadForm : Form
    {
        public LoadForm()
        {
            InitializeComponent();
        }
        Main owner;
        int successPing = 0;
        int errorPing = 0;
        int tick = 0;

        private void pingServer()
        {
            using (Ping ping = new System.Net.NetworkInformation.Ping())
            {
                PingReply pingReply = null;

                pingReply = ping.Send(IPAddress.Parse("192.168.168.168"));
                if (pingReply.Status == IPStatus.Success)
                {
                    progressBar1.Value += 20;
                    successPing++;

                    if (progressBar1.Value == 20)
                    {
                        tb_status.Text = "Подключение есть, настраиваем портал";
                    }
                    if (progressBar1.Value == 40)
                    {
                        tb_status.Text = "Рисуем кнопки";
                    }
                    if (progressBar1.Value == 60)
                    {
                        tb_status.Text = "Создаём пасхалки";
                    }
                    if (progressBar1.Value == 80)
                    {
                        tb_status.Text = "Проверяем Ваш профиль";
                    }
                    if (progressBar1.Value == 100)
                    {
                        tb_status.Text = "Проверяем файлы. Это может занять время.";
                        updateVoid(false);
                    }
                }
                else
                {
                    progressBar1.Value += 25;
                    errorPing++;

                    if (progressBar1.Value == 25)
                    {
                        tb_status.Text = "Нет ответа, пробуем ещё";
                    }
                    if (progressBar1.Value == 50)
                    {
                        tb_status.Text = "Перенастраиваем";
                    }
                    if (progressBar1.Value == 75)
                    {
                        tb_status.Text = "Перекур";
                    }
                    if (progressBar1.Value == 100)
                    {
                        tb_status.Text = "Ничего не вышло";
                    }
                }
            }
        }

        private void updateVoid(bool update = true)
        {
            progressBar1.Style = ProgressBarStyle.Blocks;
            progressBar1.Value = 0;

            #region формирование словаря файлов
            List<string> fileList = new List<string> { };

            #region районы

            #region Кроме
            List<string> temp = new List<string>{ "Кроме\\Адмиралтейский и Кировский.wav", "Кроме\\Василеостровский.wav",
                "Кроме\\Выборгский.wav", "Кроме\\Калининский.wav", "Кроме\\Колпинский.wav", "Кроме\\Пушкинский.wav", "Кроме\\Красногвардейский.wav",
                "Кроме\\Красносельский.wav", "Кроме\\Кронштадтский и Курортный.wav", "Кроме\\Московский.wav", "Кроме\\Невский.wav",
                "Кроме\\Петроградский.wav", "Кроме\\Петродворцовый.wav", "Кроме\\Приморский.wav", "Кроме\\Фрунзенский.wav", "Кроме\\Центральный.wav"};
            fileList.AddRange(temp);
            #endregion

            #region Кроме прошедших
            temp = new List<string>{ "Кроме прошедших\\Адмиралтейский и Кировский.wav", "Кроме прошедших\\Василеостровский.wav",
                "Кроме прошедших\\Выборгский.wav", "Кроме прошедших\\Калининский.wav", "Кроме прошедших\\Колпинский.wav", "Кроме прошедших\\Пушкинский.wav",
                "Кроме прошедших\\Красногвардейский.wav", "Кроме прошедших\\Красносельский.wav", "Кроме прошедших\\Кронштадтский и Курортный.wav",
                "Кроме прошедших\\Московский.wav", "Кроме прошедших\\Невский.wav", "Кроме прошедших\\Петроградский.wav",
                "Кроме прошедших\\Петродворцовый.wav", "Кроме прошедших\\Приморский.wav", "Кроме прошедших\\Фрунзенский.wav", "Кроме прошедших\\Центральный.wav"};
            fileList.AddRange(temp);
            #endregion

            #region Кроме
            temp = new List<string>{ "Полный состав\\Адмиралтейский и Кировский.wav", "Полный состав\\Василеостровский.wav",
                "Полный состав\\Выборгский.wav", "Полный состав\\Калининский.wav", "Полный состав\\Колпинский.wav", "Полный состав\\Пушкинский.wav",
                "Полный состав\\Красногвардейский.wav", "Полный состав\\Красносельский.wav", "Полный состав\\Кронштадтский и Курортный.wav",
                "Полный состав\\Московский.wav", "Полный состав\\Невский.wav", "Полный состав\\Петроградский.wav", "Полный состав\\Петродворцовый.wav",
                "Полный состав\\Приморский.wav", "Полный состав\\Фрунзенский.wav", "Полный состав\\Центральный.wav"};
            fileList.AddRange(temp);
            #endregion

            #endregion

            #region профотбор
            temp = new List<string> { "Инструкция коридор.wav", "приглашение.wav", "Инструкция этаж.wav", "Снимите наушники.wav" };
            fileList.AddRange(temp);
            #endregion

            temp.Clear();
            #endregion

            double stepProgressBar = 100.0 / fileList.Count;
            double progressBarTemp = 0;

            #region проверка директорий
            tb_status.Text = "Проверяем директории. Это может занять время.";
            tb_status.Refresh();
            System.Threading.Thread.Sleep(10);
            if (Directory.Exists(@"C:\management 4\") == false)
            {
                Directory.CreateDirectory(@"C:\management 4\");
            }
            progressBar1.Value = 20;
            if (Directory.Exists(@"C:\management 4\audio\") == false)
            {
                Directory.CreateDirectory(@"C:\management 4\audio\");
            }
            progressBar1.Value = 40;
            if (Directory.Exists(@"C:\management 4\audio\Кроме\") == false)
            {
                Directory.CreateDirectory(@"C:\management 4\audio\Кроме\");
            }
            progressBar1.Value = 60;
            if (Directory.Exists(@"C:\management 4\audio\Кроме прошедших\") == false)
            {
                Directory.CreateDirectory(@"C:\management 4\audio\Кроме прошедших\");
            }
            progressBar1.Value = 80;
            if (Directory.Exists(@"C:\management 4\audio\Полный состав\") == false)
            {
                Directory.CreateDirectory(@"C:\management 4\audio\Полный состав\");
            }
            progressBar1.Value = 100;
            #endregion

            #region проверка файлов
            tb_status.Text = "Проверяем звуковые файлы. Это может занять время.";
            tb_status.Refresh();
            progressBar1.Value = 0;
            System.Threading.Thread.Sleep(10);

            #region скачивание
            for (int i = 0; i < fileList.Count; i++)
            {
                System.Threading.Thread.Sleep(10);
                if (File.Exists(@"C:\management 4\audio\" + fileList[i]) == false)
                {
                    File.Copy(@"\\192.168.168.168\docs\others\@Прочее\management 4\audio\" + fileList[i], @"C:\management 4\audio\" + fileList[i]);
                }
                progressBarTemp += stepProgressBar;
                progressBar1.Value = Convert.ToInt32(progressBarTemp);
            }
            #endregion

            #region удаление
            var files = Directory.GetFiles(@"C:\management 4\audio\");

            stepProgressBar = 100.0 / files.Count();
            progressBar1.Value = 0;
            progressBarTemp = 0.0;

            foreach (var file in files)
            {
                System.Threading.Thread.Sleep(10);
                if (fileList.Contains(Path.GetFileName(file)) == false)
                {
                    File.Delete(file);
                }
                progressBarTemp += stepProgressBar;
                progressBar1.Value = Convert.ToInt32(progressBarTemp);
            }
            #endregion
            #endregion

            if (update)
            {
                #region сообщение об обновлении
                if ((((Main)(this.Owner)).userNameTextBox.Text != "192.168.168.2") &&
                    (((Main)(this.Owner)).userNameTextBox.Text != "192.168.168.3") &&
                    (((Main)(this.Owner)).userNameTextBox.Text != "192.168.168.4") &&
                    (((Main)(this.Owner)).userNameTextBox.Text != "192.168.168.7") &&
                    (((Main)(this.Owner)).userNameTextBox.Text != "192.168.168.8") &&
                    (((Main)(this.Owner)).userNameTextBox.Text != "192.168.168.9"))
                {
                    UpdateInfo updateInfo = new UpdateInfo();
                    this.Hide();
                    updateInfo.ShowDialog();
                }
                management213.Properties.Settings.Default.versionUser = management213.Properties.Settings.Default.versionNew;
                management213.Properties.Settings.Default.Save();
                #endregion
            }
            this.Owner.Show();
            this.Close();

        }

        private void tmr_ping_Tick(object sender, EventArgs e)
        {
            tick++;

            if (progressBar1.Value != 100)
            {
                System.Threading.Thread.Sleep(25);
                pingServer();
            }

            if (tick == 5)
            {
                if (successPing > errorPing)
                {
                    if (management213.Properties.Settings.Default.versionNew != management213.Properties.Settings.Default.versionUser)
                    {
                        updateVoid();
                        tmr_ping.Stop();
                    }
                    else
                    {
                        this.Owner.Show();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Не смогли подключиться к серверу. Без подключения к нему программа не имеет смысла. Закрываемся.");
                    Application.Exit();
                }
            }
        }

        private void LoadForm_Load(object sender, EventArgs e)
        {
            owner = (Main)this.Owner;
        }
    }
}
