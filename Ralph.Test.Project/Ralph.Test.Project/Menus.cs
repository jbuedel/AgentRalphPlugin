using System;

namespace Ralph.Test.Project
{
    public class Menus
    {
        private Menu[] Target(Menu[] menu)
        {
            // This entire method body should match Expected.

            if (menu == null)
            {
                menu = new Menu[1];
                menu[0].name = "the name";
                menu[0].description = "description";
            }
            else
            {
                Menu[] temp = new Menu[3];
                Array.Copy(menu, 0, temp, 0, 2);
                temp[2].name = "the name";
                temp[2].description = "description";
                menu = temp;
            }

            if (menu == null)
            {
                menu = new Menu[1];
                menu[0].name = "other name";
                menu[0].description = "description";
            }
            else
            {
                Menu[] temp = new Menu[3];
                Array.Copy(menu, 0, temp, 0, 2);
                temp[2].name = "other name";
                temp[2].description = "description";
                menu = temp;
            }

            return menu;
        }

        private Menu[] Expected(Menu[] menu, string name, string desc)
        {
            if (menu == null)
            {
                menu = new Menu[1];
                menu[0].name = name;
                menu[0].description = desc;
            }
            else
            {
                Menu[] temp = new Menu[3];
                Array.Copy(menu, 0, temp, 0, 2);
                temp[2].name = name;
                temp[2].description = desc;
                menu = temp;
            }
            return menu;
        }
    }

    internal class Menu
    {
        public string description;
        public string name;
    }
}