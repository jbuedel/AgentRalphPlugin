// Ignore - Not implemented.  This test will pass when we convert void f(out T parm) => T f().  That is,
// an out param is refactored to a return statement.
using System;

namespace AgentRalph.Tests.CloneCandidateDetectionTests.TestCases
{
    public class ReturnyThingyTester
    {
        private void Target(Menu[] menu)
        {
            /* BEGIN */
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
            /* END */
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

        class Menu
        {
            public string description;
            public string name;
        }
    }
}