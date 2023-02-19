using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeConsole
{
    public sealed class PlayArea
    {
        public static PointXY startXY { get; private set; }
        public static int Height { get; private set; }
        public static int ScoreInfoHeight { get; private set; }
        public static int Width { get; private set; }
        public static char WidthSymbol { get; private set; }
        public static char HeightSymbol { get; private set; }
        public static int MaxMessageLength;

        private static PlayArea playArea;
        private PlayArea() { }
        public static PlayArea GetPlayArea(int startX, int startY, int height, int width, char widthSymbol, char heightSymbol, int scoreInfoHeight)
        {
            if (playArea == null)
            {
                playArea = new PlayArea();
                startXY = new PointXY(startX, startY);
                Height = height;
                Width = width;
                WidthSymbol = widthSymbol;
                HeightSymbol = heightSymbol;
                ScoreInfoHeight = scoreInfoHeight;
                MaxMessageLength = (Width - 2) * (Height - ScoreInfoHeight - 4);
                PrintBoards();
            }
            return playArea;
        }
        private static void PrintBoards()
        {
            for(int i = startXY.X; i < Width; i++)
            {
                PrintPoint.Print(new PointXY(i, startXY.Y), WidthSymbol);
            }
            for (int i = startXY.X; i < Width; i++)
            {
                PrintPoint.Print(new PointXY(i, startXY.Y + Height), WidthSymbol);
            }
            for (int i = startXY.X; i < Width; i++)
            {
                PrintPoint.Print(new PointXY(i, ScoreInfoHeight), WidthSymbol);
            }
            for (int i = startXY.Y+1; i < Height; i++)
            {
                PrintPoint.Print(new PointXY(startXY.X, i), HeightSymbol);
            }
            for (int i = startXY.Y+1; i < Height; i++)
            {
                PrintPoint.Print(new PointXY(startXY.X + Width - 1, i), HeightSymbol);
            }
        }
        public static void PrintInGameArea(Point point)
        {
            if(point.X > 0 && point.X < Width - 1 && point.Y > 0 && point.Y < ScoreInfoHeight)
            {
                PrintPoint.Print(point);
            }
            else
            {
                throw new Exception("Out of play area!");
            }
        }
        public static void PrintInGameArea(PointXY pointXY, char symbol)
        {
            if (pointXY.X > 0 && pointXY.X < Width - 1 && pointXY.Y > 0 && pointXY.Y < ScoreInfoHeight)
            {
                PrintPoint.Print(pointXY, symbol);
            }
            else
            {
                throw new Exception("Out of play area!");
            }
        }
        public static void PrintScore(long score, ConsoleColor color = ConsoleColor.Green)
        {
            PrintPoint.Print(new PointXY((Width/5)-4, ScoreInfoHeight + 2), "SCORE: " + Convert.ToString(score), color);
        }
        public static void PrintPCTime(ConsoleColor color = ConsoleColor.Green)
        {
            PrintPoint.Print(new PointXY(Width - (Width / 5) - 4, ScoreInfoHeight + 2), DateTime.Now.ToLongTimeString(), color);
        }
        public static void PrintMessage(string text, ConsoleColor color = ConsoleColor.Red)
        {
            if(text.Length > Width - 2)
            {
                if(text.Length <= ((Width-2) * (Height - ScoreInfoHeight - 4)))
                {
                    for(int i = 0; i < (Height - ScoreInfoHeight - 4); i++)
                    {
                        if (i * (Width - 2) + (Width - 2) <= text.Length)
                        {
                            PrintPoint.Print(new PointXY(1, ScoreInfoHeight + 4 + i), text.Substring(i * (Width - 2), Width - 2), color);
                        }
                        else
                        {
                            PrintPoint.Print(new PointXY(1, ScoreInfoHeight + 4 + i), text.Substring(i * (Width - 2)), color);
                            break;
                        }
                    }
                }
                else
                {
                    throw new Exception("So long text. Max length = " + ((Width - 2) * (Height - ScoreInfoHeight - 4)));
                }
            }
            else
            {
                PrintPoint.Print(new PointXY(1, ScoreInfoHeight + 4), text, color);
            }
        }
    }
}
