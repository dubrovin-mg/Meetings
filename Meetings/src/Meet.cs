using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq;

namespace Meetings
{
    class Meet
    {
        /* Форматы для даты/времени и интервала */
        private const string dateTimeFormat = "dd.MM.yyyy HH:mm";
        private const string timeSpanFormat = "HH:mm";

        /* Наименование встречи */
        public string Name { get; set; }
        /* Дата начала встречи */
        public DateTime StartDate { get; set; }
        /* Дата окончания встречи */
        public DateTime EndDate { get; set; }
        /* Признак, указывающий необходимо ли уведомление о встрече */
        public bool NeedAlarm { get; set; }
        /* Время, за которое нужно уведомить о встрече */
        public TimeSpan AlarmTime { get; set; }
        
        /* Конструктор класса */
        private Meet(string name, DateTime startDate, DateTime endDate, bool needAlarm, TimeSpan alarmTime)
        {
            this.Name = name;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.NeedAlarm = needAlarm;
            this.AlarmTime = alarmTime;
        }

        /* Создание объекта встречи */
        public static Meet CreateObject()
        {
            Console.WriteLine("Введите наименование встречи:");
            // Проверить условие ввода непустого имени 
            string name;
            while (!CheckMeetName(Console.ReadLine(), out name) )
            {
                Console.WriteLine("Наим-ие встречи не введено. Попробуйте еще раз");
            }

            Console.WriteLine("Введите дату начала в формате {0}:", dateTimeFormat);
            DateTime startDate;
            // Проверить условие ввода корректных даты/времени 
            while (!CheckCorrectDate(Console.ReadLine(), out startDate))
            {
                Console.WriteLine("Некорректное значение даты. Попробуйте еще раз");
            }

            Console.WriteLine("Введите дату окончания в формате {0}:", dateTimeFormat);
            DateTime endDate;
            // Проверить условие ввода корректных даты/времени 
            while (!CheckCorrectDate(Console.ReadLine(), out endDate))
            {
                Console.WriteLine("Некорректное значение даты. Попробуйте еще раз");
            }

            // Проверить условия отсутствия пересечения дат 
            CheckIntersectDates(startDate, endDate);

            Console.WriteLine("Хотите ли вы создать уведомление о встрече? Введите число: 1 - Да, 2 - Нет");
            // Проверить условия корректного ввода ответа;
            bool needAlarm;
            while (!CheckAlarmAnswer(Console.ReadLine(), out needAlarm))
            {
                Console.WriteLine("Выбрано некорректное число. Пожалуйста, введите число: 1 - Да, 2 - Нет");
            }

            TimeSpan alarmTime = new TimeSpan(0);
            if (needAlarm)
            {
                Console.WriteLine("За какое время нужно прислать уведомление? Укажите в формате {0}", timeSpanFormat);
                // Проверить ввод корректного интервала
                while (!CheckCorrectInterval(Console.ReadLine(), out alarmTime))
                {
                    Console.WriteLine("Некорректное значение интервала об уведомлении. Попробуйте еще раз");
                }
            }

            return new Meet(name, startDate, endDate, needAlarm, alarmTime);
        }

        /* Проверка корректности ввода наименования */
        private static bool CheckMeetName(string inputText, out string name)
        {
            name = inputText;
            if (String.IsNullOrEmpty(name))
                return false;
            else
                return true;
        }

        /* Проверка условия ввода корректных даты/времени */
        internal static bool CheckCorrectDate(string inputText, out DateTime resultDate)
        {
            // Условие корректных даты/времени 
            // Условие планирования встречи только на будущее время 
            if (!DateTime.TryParseExact(inputText,
                       dateTimeFormat,
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out resultDate) || resultDate <= DateTime.Now)
                return false;
            else
                return true;
        }

        /* Проверка условия отсутствия пересечения дат */
        private static void CheckIntersectDates(DateTime startDate, DateTime endDate)
        {
            // Проверить условие даты окончания встречи больше даты начала
            if (endDate <= startDate)
            {
                throw new Exception("Дата окончания не может быть меньше даты начала");
            }

            // Проверка условия отсутствия пересечения встреч 
            var intersectMeets =
                from item in Scheduler.ReadSchedule
                where (startDate > item.StartDate
                       && startDate < item.EndDate)
                      || (endDate > item.StartDate
                          && endDate < item.EndDate)
                select item;
            if (intersectMeets.Count() > 0)
            {
                throw new Exception("На выбранное время уже существуют записи в расписании");
            }
        }

        /* Проверка на корректность ввода ответа о создании уведомления */
        private static bool CheckAlarmAnswer(string inputText, out bool needAlarm)
        {
            int result;
            needAlarm = false;
            if (!int.TryParse(inputText, out result)
                || result > 2
                    || result < 1)
            {
                return false;
            }
            else
            {
                needAlarm = result == 1 ? true : false;
                return true;
            }
        }

        /* Проверить условие ввода корректного интервала. */
        private static bool CheckCorrectInterval(string inputText, out TimeSpan alarmTime)
        {
            alarmTime = new TimeSpan(0);

            if (!TimeSpan.TryParseExact(inputText, @"hh\:mm", null, out alarmTime))
                return false;
            else
                return true;
        }

        /* Суммарная информация о встрече */
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("--------------------------------\n");
            builder.Append($"Наименование встречи: {Name}\n");
            builder.Append($"Дата начала: {StartDate:dd.MM.yyyy HH:mm}, Дата окочания: {EndDate:dd.MM.yyyy HH:mm}\n");
            var textAlarm = NeedAlarm ? "Да" : "Нет";
            builder.Append($"Уведомление: {textAlarm}");
            if (NeedAlarm)
            {
                builder.Append($", за {AlarmTime:hh\\:mm} до встречи");
            }
            builder.Append("\n--------------------------------\n");

            return builder.ToString();
        }

        /* Изменение данных выбранной встречи */
        public static void EditData(Meet meet)
        {
            Console.WriteLine("Введите новое наименование встречи:");
            // Проверить условие ввода непустого имени 
            string name;
            while (!CheckMeetName(Console.ReadLine(), out name))
            {
                Console.WriteLine("Наим-ие встречи не введено. Попробуйте еще раз");
            }

            Console.WriteLine("Введите дату начала в формате {0}:", dateTimeFormat);
            DateTime startDate;
            // Проверить условие ввода корректных даты/времени 
            while (!CheckCorrectDate(Console.ReadLine(), out startDate))
            {
                Console.WriteLine("Некорректное значение даты. Попробуйте еще раз");
            }

            Console.WriteLine("Введите дату окончания в формате {0}:", dateTimeFormat);
            DateTime endDate;
            // Проверить условие ввода корректных даты/времени 
            while (!CheckCorrectDate(Console.ReadLine(), out endDate))
            {
                Console.WriteLine("Некорректное значение даты. Попробуйте еще раз");
            }

            // Проверить условия отсутствия пересечения дат 
            CheckIntersectDates(startDate, endDate);

            Console.WriteLine("Хотите ли вы создать уведомление о встрече? Введите число: 1 - Да, 2 - Нет");
            // Проверить условия корректного ввода ответа;
            bool needAlarm;
            while (!CheckAlarmAnswer(Console.ReadLine(), out needAlarm))
            {
                Console.WriteLine("Выбрано некорректное число. Пожалуйста, введите число: 1 - Да, 2 - Нет");
            }

            TimeSpan alarmTime = new TimeSpan(0);
            if (needAlarm)
            {
                Console.WriteLine("За какое время нужно прислать уведомление? Укажите в формате {0}", timeSpanFormat);
                // Проверить ввод корректного интервала
                while (!CheckCorrectInterval(Console.ReadLine(), out alarmTime))
                {
                    Console.WriteLine("Некорректное значение интервала об уведомлении");
                }
            }
            // Изменение объекта
            if (meet.Name != name)
                meet.Name = name;
            if (meet.StartDate != startDate)
                meet.StartDate = startDate;
            if (meet.EndDate != endDate)
                meet.EndDate = endDate;
            if (meet.NeedAlarm != needAlarm)
                meet.NeedAlarm = needAlarm;
            if (meet.AlarmTime != alarmTime)
                meet.AlarmTime = alarmTime;
        }

    }






}
