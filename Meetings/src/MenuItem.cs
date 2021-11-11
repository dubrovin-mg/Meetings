using System;
using System.Collections.Generic;
using System.Text;

namespace Meetings
{
    class MenuItem
    {   
        /* Описание пункта меню */
        public string Description { get; set; }

        /* Делегат на метод, который необходимо вызвать при выборе пункта меню */
        public Action Execute { get; set; }

        /*Вывод описания пункта*/
        public override string ToString()
        {
            return Description;
        }
    }
}
