using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace management213
{
    internal class Voice
    {
        System.Media.SoundPlayer startVoiceNew;

        IDictionary<string,System.Media.SoundPlayer> rayons = new Dictionary<string, System.Media.SoundPlayer>();


        public Voice() : base()
        {

            #region районы

            IDictionary<string, System.Media.SoundPlayer> temp;
            temp = new Dictionary<string, System.Media.SoundPlayer>();

            rayons["admkir all"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Полный состав\Адмиралтейский и Кировский.wav");
            rayons["admkir without"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме\Адмиралтейский и Кировский.wav");
            rayons["admkir withoutEnd"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме прошедших\Адмиралтейский и Кировский.wav");

            rayons["vas all"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Полный состав\Василеостровский.wav");
            rayons["vas without"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме\Василеостровский.wav");
            rayons["vas withoutEnd"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме прошедших\Василеостровский.wav");

            rayons["vyb all"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Полный состав\Выборгский.wav");
            rayons["vyb without"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме\Выборгский.wav");
            rayons["vyb withoutEnd"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме прошедших\Выборгский.wav");

            rayons["kal all"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Полный состав\Калининский.wav");
            rayons["kal without"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме\Калининский.wav");
            rayons["kal withoutEnd"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме прошедших\Калининский.wav");

            rayons["kolp all"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Полный состав\Колпинский.wav");
            rayons["kolp without"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме\Колпинский.wav");
            rayons["kolp withoutEnd"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме прошедших\Колпинский.wav");

            rayons["push all"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Полный состав\Пушкинский.wav");
            rayons["push without"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме\Пушкинский.wav");
            rayons["push withoutEnd"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме прошедших\Пушкинский.wav");

            rayons["krgv all"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Полный состав\Красногвардейский.wav");
            rayons["krgv without"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме\Красногвардейский.wav");
            rayons["krgv withoutEnd"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме прошедших\Красногвардейский.wav");

            rayons["krsel all"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Полный состав\Красносельский.wav");
            rayons["krsel without"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме\Красносельский.wav");
            rayons["krsel withoutEnd"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме прошедших\Красносельский.wav");

            rayons["krkur all"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Полный состав\Кронштадтский и Курортный.wav");
            rayons["krkur without"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме\Кронштадтский и Курортный.wav");
            rayons["krkur withoutEnd"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме прошедших\Кронштадтский и Курортный.wav");

            rayons["mos all"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Полный состав\Московский.wav");
            rayons["mos without"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме\Московский.wav");
            rayons["mos withoutEnd"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме прошедших\Московский.wav");

            rayons["nev all"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Полный состав\Невский.wav");
            rayons["nev without"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме\Невский.wav");
            rayons["nev withoutEnd"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме прошедших\Невский.wav");

            rayons["pgr all"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Полный состав\Петроградский.wav");
            rayons["pgr without"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме\Петроградский.wav");
            rayons["pgr withoutEnd"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме прошедших\Петроградский.wav");

            rayons["pdv all"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Полный состав\Петродворцовый.wav");
            rayons["pdv without"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме\Петродворцовый.wav");
            rayons["pdv withoutEnd"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме прошедших\Петродворцовый.wav");

            rayons["prim all"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Полный состав\Приморский.wav");
            rayons["prim without"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме\Приморский.wav");
            rayons["prim withoutEnd"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме прошедших\Приморский.wav");

            rayons["frun all"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Полный состав\Фрунзенский.wav");
            rayons["frun without"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме\Фрунзенский.wav");
            rayons["frun withoutEnd"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме прошедших\Фрунзенский.wav");
            
            rayons["centr all"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Полный состав\Центральный.wav");
            rayons["centr without"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме\Центральный.wav");
            rayons["centr withoutEnd"] = new System.Media.SoundPlayer(@"C:\management 4\audio\Кроме прошедших\Центральный.wav");

            #endregion

        }
        public async Task<string> playAudio(List<string> vs)
        {
            await Task.Run(() =>
            {
                foreach (string v in vs)
                {
                    switch (v)
                    {
                        case string s when v.Contains("rayons "):
                            s = s.Remove(0, s.IndexOf(' ') + 1);
                            rayons[s].PlaySync();
                            break;
                        case "наушники":
                            new System.Media.SoundPlayer(@"C:\management 4\audio\Снимите наушники.wav").PlaySync();
                            break;
                        case "инструкция этаж":
                            new System.Media.SoundPlayer(@"C:\management 4\audio\Инструкция этаж.wav").PlaySync();
                            break;
                        case "инструкция коридор":
                            new System.Media.SoundPlayer(@"C:\management 4\audio\Инструкция коридор.wav").PlaySync();
                            break;
                        case "приглашение":
                            new System.Media.SoundPlayer(@"C:\management 4\audio\приглашение.wav").PlaySync();
                            break;
                        default:
                            break;
                    }
                }
            });
            return "0";
        }

        public string[] getTime()
        {
            #region создаём массив для ответа
            int countArray;
            if (DateTime.Now.Hour > 20)
                countArray = 3;
            else
                countArray = 2;
            string[] nowTimeHour = new string[countArray];

            if ((DateTime.Now.Minute > 20) && (DateTime.Now.Minute % 10 != 0))
                countArray = 3;
            else
                countArray = 2;
            string[] nowTimeMinute = new string[countArray];

            #endregion

            switch (DateTime.Now.Hour)
            {
                case 0:
                    nowTimeHour[0] = "0";
                    nowTimeHour[1] = "chasov";
                    break;
                case 1:
                    nowTimeHour[0] = "odin";
                    nowTimeHour[1] = "chas";
                    break;
                case 2:
                    nowTimeHour[0] = "dva";
                    nowTimeHour[1] = "chasa";
                    break;
                case 3:
                    nowTimeHour[0] = "3";
                    nowTimeHour[1] = "chasa";
                    break;
                case 4:
                    nowTimeHour[0] = "4";
                    nowTimeHour[1] = "chasa";
                    break;
                case 5:
                    nowTimeHour[0] = "5";
                    nowTimeHour[1] = "chasov";
                    break;
                case 6:
                    nowTimeHour[0] = "6";
                    nowTimeHour[1] = "chasov";
                    break;
                case 7:
                    nowTimeHour[0] = "7";
                    nowTimeHour[1] = "chasov";
                    break;
                case 8:
                    nowTimeHour[0] = "8";
                    nowTimeHour[1] = "chasov";
                    break;
                case 9:
                    nowTimeHour[0] = "9";
                    nowTimeHour[1] = "chasov";
                    break;
                case 10:
                    nowTimeHour[0] = "10";
                    nowTimeHour[1] = "chasov";
                    break;
                case 11:
                    nowTimeHour[0] = "11";
                    nowTimeHour[1] = "chasov";
                    break;
                case 12:
                    nowTimeHour[0] = "12";
                    nowTimeHour[1] = "chasov";
                    break;
                case 13:
                    nowTimeHour[0] = "13";
                    nowTimeHour[1] = "chasov";
                    break;
                case 14:
                    nowTimeHour[0] = "14";
                    nowTimeHour[1] = "chasov";
                    break;
                case 15:
                    nowTimeHour[0] = "15";
                    nowTimeHour[1] = "chasov";
                    break;
                case 16:
                    nowTimeHour[0] = "16";
                    nowTimeHour[1] = "chasov";
                    break;
                case 17:
                    nowTimeHour[0] = "17";
                    nowTimeHour[1] = "chasov";
                    break;
                case 18:
                    nowTimeHour[0] = "18";
                    nowTimeHour[1] = "chasov";
                    break;
                case 19:
                    nowTimeHour[0] = "19";
                    nowTimeHour[1] = "chasov";
                    break;
                case 20:
                    nowTimeHour[0] = "20";
                    nowTimeHour[1] = "chasov";
                    break;
                case 21:
                    nowTimeHour[0] = "20";
                    nowTimeHour[1] = "odin";
                    nowTimeHour[2] = "chas";
                    break;
                case 22:
                    nowTimeHour[0] = "20";
                    nowTimeHour[1] = "dva";
                    nowTimeHour[2] = "chasa";
                    break;
                case 23:
                    nowTimeHour[0] = "20";
                    nowTimeHour[1] = "3";
                    nowTimeHour[2] = "chasa";
                    break;

            }

            if ((DateTime.Now.Minute <= 20) || (DateTime.Now.Minute % 10 == 0))
            {
                switch (DateTime.Now.Minute)
                {
                    case 0:
                        nowTimeMinute[0] = "0";
                        nowTimeMinute[1] = "minut";
                        break;
                    case 1:
                        nowTimeMinute[0] = "odna";
                        nowTimeMinute[1] = "minuta";
                        break;
                    case 2:
                        nowTimeMinute[0] = "dve";
                        nowTimeMinute[1] = "minuty";
                        break;
                    case 3:
                        nowTimeMinute[0] = "3";
                        nowTimeMinute[1] = "minuty";
                        break;
                    case 4:
                        nowTimeMinute[0] = "4";
                        nowTimeMinute[1] = "minuty";
                        break;
                    case 5:
                        nowTimeMinute[0] = "5";
                        nowTimeMinute[1] = "minut";
                        break;
                    case 6:
                        nowTimeMinute[0] = "6";
                        nowTimeMinute[1] = "minut";
                        break;
                    case 7:
                        nowTimeMinute[0] = "7";
                        nowTimeMinute[1] = "minut";
                        break;
                    case 8:
                        nowTimeMinute[0] = "8";
                        nowTimeMinute[1] = "minut";
                        break;
                    case 9:
                        nowTimeMinute[0] = "9";
                        nowTimeMinute[1] = "minut";
                        break;
                    case 10:
                        nowTimeMinute[0] = "10";
                        nowTimeMinute[1] = "minut";
                        break;
                    case 11:
                        nowTimeMinute[0] = "11";
                        nowTimeMinute[1] = "minut";
                        break;
                    case 12:
                        nowTimeMinute[0] = "12";
                        nowTimeMinute[1] = "minut";
                        break;
                    case 13:
                        nowTimeMinute[0] = "13";
                        nowTimeMinute[1] = "minut";
                        break;
                    case 14:
                        nowTimeMinute[0] = "14";
                        nowTimeMinute[1] = "minut";
                        break;
                    case 15:
                        nowTimeMinute[0] = "15";
                        nowTimeMinute[1] = "minut";
                        break;
                    case 16:
                        nowTimeMinute[0] = "16";
                        nowTimeMinute[1] = "minut";
                        break;
                    case 17:
                        nowTimeMinute[0] = "17";
                        nowTimeMinute[1] = "minut";
                        break;
                    case 18:
                        nowTimeMinute[0] = "18";
                        nowTimeMinute[1] = "minut";
                        break;
                    case 19:
                        nowTimeMinute[0] = "19";
                        nowTimeMinute[1] = "minut";
                        break;
                    case 20:
                        nowTimeMinute[0] = "20";
                        nowTimeMinute[1] = "minut";
                        break;
                    case 30:
                        nowTimeMinute[0] = "30";
                        nowTimeMinute[1] = "minut";
                        break;
                    case 40:
                        nowTimeMinute[0] = "40";
                        nowTimeMinute[1] = "minut";
                        break;
                    case 50:
                        nowTimeMinute[0] = "50";
                        nowTimeMinute[1] = "minut";
                        break;
                }
            }
            else
            {
                if ((DateTime.Now.Minute > 20) && (DateTime.Now.Minute < 30))
                {
                    nowTimeMinute[0] = "20";
                }
                if ((DateTime.Now.Minute > 30) && (DateTime.Now.Minute < 40))
                {
                    nowTimeMinute[0] = "30";
                }
                if ((DateTime.Now.Minute > 40) && (DateTime.Now.Minute < 50))
                {
                    nowTimeMinute[0] = "40";
                }
                if ((DateTime.Now.Minute > 50) && (DateTime.Now.Minute < 60))
                {
                    nowTimeMinute[0] = "50";
                }
                switch (Convert.ToInt32(DateTime.Now.Minute.ToString()[1].ToString()))
                {
                    case 1:
                        nowTimeMinute[1] = "odna";
                        nowTimeMinute[2] = "minuta";
                        break;
                    case 2:
                        nowTimeMinute[1] = "dve";
                        nowTimeMinute[2] = "minuty";
                        break;
                    case 3:
                        nowTimeMinute[1] = "3";
                        nowTimeMinute[2] = "minuty";
                        break;
                    case 4:
                        nowTimeMinute[1] = "4";
                        nowTimeMinute[2] = "minuty";
                        break;
                    case 5:
                        nowTimeMinute[1] = "5";
                        nowTimeMinute[2] = "minut";
                        break;
                    case 6:
                        nowTimeMinute[1] = "6";
                        nowTimeMinute[2] = "minut";
                        break;
                    case 7:
                        nowTimeMinute[1] = "7";
                        nowTimeMinute[2] = "minut";
                        break;
                    case 8:
                        nowTimeMinute[1] = "8";
                        nowTimeMinute[2] = "minut";
                        break;
                    case 9:
                        nowTimeMinute[1] = "9";
                        nowTimeMinute[2] = "minut";
                        break;
                }
            }

            string[] timeString = new string[nowTimeHour.Length + nowTimeMinute.Length];
            int i = 0;
            foreach (string hour in nowTimeHour)
            {
                timeString[i] = hour;
                i++;
            }
            foreach (string minute in nowTimeMinute)
            {
                timeString[i] = minute;
                i++;
            }


            return timeString;
        }
    }
}
