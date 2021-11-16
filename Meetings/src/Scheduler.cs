using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Meetings
{
    static class Scheduler
    {
        /* Расписание, представляющее собой список встреч */
        private static List<Meet> Schedule = new List<Meet>();

        /* Расписание в режиме чтения */
        public static IReadOnlyList<Meet> ReadSchedule
        {
            get
            {
                return Schedule.AsReadOnly();
            }
        }

        /* Добавить объект встречи в расписание */
        public static void CreateMeetAndAddToList()
        {
            var meet = Meet.CreateObject();

            Schedule.Add(meet);
            Console.WriteLine("Встреча успешно создана и добавлена в расписание");
            Console.WriteLine(meet);
        }

        /* Проверка на отсутствие записей в расписании - True если записи отсутствуют */
        private static bool IsEmptyList()
        {
            if (Schedule.Count == 0)
            {
                Console.WriteLine("На данный момент встреч не назначено");
                return true;
            }
            return false;
        }

        /* Проверка корректности выбранного номера из расписания */
        private static bool CheckSelectedIndex(string inputText, out int index)
        {
            return int.TryParse(inputText, out index) && index > 0 && index <= Schedule.Count;
        }

        /* Изменить объект встречи в расписании */
        public static void EditMeetInList()
        {
            if (IsEmptyList()) return;
            ShowList();

            Console.WriteLine("Введите № встречи из списка для редактирования.");

            int index;

                while (!CheckSelectedIndex(Console.ReadLine(), out index))
            {
                Console.WriteLine("Встреча с таким номером не найдена. Попробуйте еще раз");
            }

            var meet = Schedule[index - 1];
            Meet.EditData(meet);

            Console.WriteLine("Изменение выполнено успешно");
            Console.WriteLine(meet);
        }

        /* Удалить встречу из расписания */
        public static void RemoveMeetFromList()
        {
            if (IsEmptyList()) return;
            ShowList();
            Console.WriteLine("Введите № встречи из списка для удаления.");

            int index;
            while (!CheckSelectedIndex(Console.ReadLine(), out index))
            {
                Console.WriteLine("Встреча с таким номером не найдена. Попробуйте еще раз");
            }

            Schedule.RemoveAt(index - 1);
            Console.WriteLine("Удаление выполнено успешно");
            ShowList();
        }
        
        /* Отобразить в консоли список встреч */
        public static void ShowList()
        {
            if (IsEmptyList()) return;
            Console.WriteLine();
            foreach (var item in Schedule)
            {
                Console.WriteLine($"№ встречи: {Schedule.IndexOf(item) + 1}");
                Console.WriteLine(item);
            } 
        }

        /* Экспорт расписания в текстовый файл */       
        public static void ExportList()
        {
            if (IsEmptyList()) return;
            Console.WriteLine("Введите имя файла для экспорта (без расширения)");

            // Проверка корректного имени файла
            string name;
            while (!CheckFileName(Console.ReadLine(), out name))
            {
                Console.WriteLine("Наим-ие не корректно. Попробуйте еще раз");
            }

            var builder = new StringBuilder();

            foreach (var item in Schedule)
            {
                builder.Append($"№ встречи: {Schedule.IndexOf(item) + 1}\n");
                builder.Append(item);
            }

            File.WriteAllText(name + ".txt", builder.ToString());

            Console.WriteLine("Экспорт выполнен успешно");
        }
 
        /*Проверка корректного имени файла, введенного с консоли */
        internal static bool CheckFileName(string inputText, out string name)
        {
            // Проверка на непустое имя
            // Проверка на отсутствие недопустимых символов
            name = inputText;
            Regex badFileChars = new Regex("["+Regex.Escape(new string (Path.GetInvalidFileNameChars() ) )+"]");
            return !(String.IsNullOrEmpty(name) || badFileChars.IsMatch(name));
        }

        /*Проверка наступления времени уведомления о встрече и вывод информации на консоль */
        public static void CheckAlarmsInList(Object o)
        {

            if (Schedule.Count == 0)
                return;

                foreach (var item in Schedule)
                {   
                    if (item.NeedAlarm)
                    {   //Проверка на соответствие текущего времени и времени начала встречи -(минус) интервал, за который нужно выдать уведомление 
                        if ( (item.StartDate - item.AlarmTime).ToString("dd.MM.yyyy HH:mm") 
                                               == DateTime.Now.ToString("dd.MM.yyyy HH:mm") 
                           )
                            Console.WriteLine($"Получено уведомление о предстоящей встрече! Через {item.AlarmTime:hh\\:mm} запланирована встреча {item.Name}, " +
                                              $"которая начнётся {item.StartDate:dd.MM.yyyy HH:mm} ");
                    }

                }    
        }

    }
}
