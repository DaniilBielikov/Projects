using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeConsole
{
    public struct PointXY
    {
        public int X;
        public int Y;
        public PointXY(int x, int y)
        {
            X = x; 
            Y = y; 
        }
    }
    public class Point
    {
        public PointXY XY;
        public char Symbol { set; get; }
        public ConsoleColor Color { set; get; }
        public int X
        { 
            get { return XY.X; }
            set { XY.X = value; } }
        public int Y
        { 
            get { return XY.Y; }
            set { XY.Y = value; }
        }
        public Point(int x, int y, char symbol, ConsoleColor color)
        {
            XY = new PointXY(x, y);
            Symbol = symbol;
            Color = color;
        }
        public Point(PointXY XY, char symbol, ConsoleColor color)
        {
            this.XY = XY;
            Symbol = symbol;
            Color = color;
        }
    }
}
