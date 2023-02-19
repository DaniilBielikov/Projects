using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeConsole
{
    public static class FoodFactory
    {
        private static Random random = new Random();
        private static string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public static Point GetRandomFood(PointXY leftUP, PointXY rightDown)
        {
            return new Point(random.Next(leftUP.X, rightDown.X), random.Next(leftUP.Y, rightDown.Y), 
                                Characters[random.Next(0, 61)], ChoseColor(random.Next(1, 14)));
        }
        static ConsoleColor ChoseColor(int val)
        {
            switch(val)
            {
                case 0:
                    return ConsoleColor.Black;
                case 1:
                    return ConsoleColor.Blue;
                case 2:
                    return ConsoleColor.Cyan;
                case 3:
                    return ConsoleColor.DarkBlue;
                case 4:
                    return ConsoleColor.DarkCyan;
                case 5:
                    return ConsoleColor.DarkGray;
                case 6:
                    return ConsoleColor.DarkGreen;
                case 7:
                    return ConsoleColor.DarkMagenta;
                case 8:
                    return ConsoleColor.DarkRed;
                case 9:
                    return ConsoleColor.DarkYellow;
                case 10:
                    return ConsoleColor.Gray;
                case 11:
                    return ConsoleColor.Green;
                case 12:
                    return ConsoleColor.Magenta;
                case 13:
                    return ConsoleColor.Red;
                default:
                    return ConsoleColor.White;
            }
        }
    }
}
