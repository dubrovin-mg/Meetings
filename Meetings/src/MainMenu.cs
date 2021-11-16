using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static Meetings.Scheduler;
/* -----------------------------------------------------------
* Проект: Meetings
* Описание: Приложение для управления личными встречами
* Разработчик: Дубровин М.Г.
* Дата создания: *.11.2021
* Версия .Net: Core 3.1
------------------------------------------------------------ */ 

namespace Meetings
{
    class MainMenu
    {   
        private static Timer timer = null;
        /* Время обновления таймера */
        private const int oneMin = 60000;

        /* Создание пользовательского меню */
        private static Dictionary<int, MenuItem> Menu = new Dictionary<int, MenuItem>
            {
            {1, new MenuItem {Description="Запланировать встречу", Execute = CreateMeetAndAddToList } },
            {2, new MenuItem {Description="Изменить встречу", Execute = EditMeetInList } },
            {3, new MenuItem {Description="Удалить встречу", Execute = RemoveMeetFromList } },
            {4, new MenuItem {Description="Просмотреть расписание", Execute = ShowList } },
            {5, new MenuItem {Description="Экспортировать расписание", Execute = ExportList } },
            {6, new MenuItem {Description="Выйти из программы", Execute = () => Environment.Exit(0) } }
            };

        static void Main(string[] args)
        {
            // Таймер используется для вывода уведомления о предстоящей встрече
            timer = new Timer(CheckAlarmsInList, null, 0, oneMin);

            DisplayHeader();
            DisplayMenu();
    
            int itemIndex = 0;
        
            while(true)
            {
                while (!CheckCorrectMenuItem(Console.ReadLine(), out itemIndex))
                {
                    Console.WriteLine("Выбран некорректный пункт меню. Пожалуйста, повторите выбор");
                }

                try
                {   // Переход к последней строке консоли для очистки 
                    Console.SetCursorPosition(0, Console.CursorTop-1);
                    Console.WriteLine($"{itemIndex}: {Menu[itemIndex]} ");
                    Menu[itemIndex].Execute();
                    GoToMenu();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка: {e.Message}");
                    GoToMenu();
                }
            }

        }

        /* Шапка программы */
        private static void DisplayHeader()
        {
            Console.WriteLine("+----------------------------------------------------------+");
            Console.WriteLine(" Добро пожаловать в программу управления личными встречами ");
            Console.WriteLine("+----------------------------------------------------------+");
        }

        /* Отображение меню */
        private static void DisplayMenu()
        {
            Console.WriteLine("+----------------------------------------------------------+");
            foreach (var item in Menu)
            {
                Console.WriteLine(item.Key + ". " + item.Value);
            }
            Console.WriteLine("+----------------------------------------------------------+");
            Console.WriteLine($"Выберите пункт меню для выполнения действия (1 - {Menu.Count})");
        }

        /* Переход к стартовому меню */
        private static void GoToMenu()
        {
            Console.Write("Нажмите любую клавишу для перехода к меню\n");
            Console.ReadKey(intercept:true);
            Console.SetCursorPosition(0, Console.CursorTop-1);
            DisplayMenu();
        }

        /* Проверка на корректность выбранного пункта - число в возможном диапазоне значений */
        internal static bool CheckCorrectMenuItem(string inputText, out int result)
        {
            return int.TryParse(inputText, out result) && result >= Menu.Keys.Min() && result <= Menu.Keys.Max();
        }

    }
}
