using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeConsole
{
    public static class PrintPoint
    {
        public static int consoleHeight;
        public static int consoleWidth;

        public static void Print(Point point)
        {
            if (point.X > 0 && point.X <= consoleWidth && point.Y > 0 && point.Y <= consoleHeight)
            {
                Console.ForegroundColor = point.Color;
                Console.SetCursorPosition(point.X, point.Y);
                Console.Write(point.Symbol);
            }
            else
            {
                throw new Exception("Point coordinates out of console window!");
            }
        }
        public static void Print(PointXY point, char symbol)
        {
            if (point.X >= 0 && point.X <= consoleWidth && point.Y >= 0 && point.Y <= consoleHeight)
            {
                Console.SetCursorPosition(point.X, point.Y);
                Console.Write(symbol);
            }
            else
            {
                throw new Exception("Point coordinates out of console window!");
            } 
        }
        public static void Print(PointXY point, char symbol, ConsoleColor color)
        {
            if (point.X >= 0 && point.X <= consoleWidth && point.Y >= 0 && point.Y <= consoleHeight)
            {
                Console.ForegroundColor = color;
                Console.SetCursorPosition(point.X, point.Y);
                Console.Write(symbol);
            }
            else
            {
                throw new Exception("Point coordinates out of console window!");
            }
        }
        public static void Print(PointXY point, string str, ConsoleColor color)
        {
            if (point.X >= 0 && point.X <= consoleWidth && point.Y >= 0 && point.Y <= consoleHeight)
            {
                Console.ForegroundColor = color;
                Console.SetCursorPosition(point.X, point.Y);
                Console.Write(str);
            }
            else
            {
                throw new Exception("Point coordinates out of console window!");
            }
        }
    }
}
