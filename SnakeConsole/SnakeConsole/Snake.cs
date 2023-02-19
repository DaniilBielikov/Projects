using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SnakeConsole
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
    public sealed class Snake : AbstractSnake
    {
        private static Snake snake;
        static Semaphore semaphore = new Semaphore(1, 1);
        static readonly Direction StartHead = Direction.Right;
        static readonly List<Point> StartPoints = new List<Point>{ new Point(5, 3, '*', headColor)};
        static int StartMoveSleep;
        public static List<Point> Points { set; get; }
        static Direction headDirection = StartHead;
        public static ConsoleColor headColor { get; set; }
        public static ConsoleColor bodyColor { get; set; }
        static int MoveSleep { set; get; }
        static Thread Control;
        static Thread Moving;
        private Snake() { }
        public static Snake GetSnake(ConsoleColor head, ConsoleColor body, int moveTimeInterval)
        {
            if (snake == null)
            {
                snake = new Snake();
                StartMoveSleep = moveTimeInterval;
                MoveSleep = moveTimeInterval;
                headColor = head;
                bodyColor = body;
                Points = new List<Point>(StartPoints);
                Control = new Thread(ReadDirection);
                Control.Start();

            }
            return snake;
        }
        public void StopPlay()
        {
            if (Control.ThreadState == ThreadState.Running)
                Control.Abort();
            if (Moving.ThreadState == ThreadState.Running)
                Moving.Abort();
        }
        public static void ReadDirection()
        {
            while (true)
            {
                Direction direction = headDirection;
                ConsoleKey key = Console.ReadKey().Key;

                if (key == ConsoleKey.UpArrow)
                {
                    direction = Direction.Up;
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    direction = Direction.Right;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    direction = Direction.Down;
                }
                else if (key == ConsoleKey.LeftArrow)
                {
                    direction = Direction.Left;
                }
                if (direction != headDirection && !IsOppositeDirections(direction, headDirection))
                {
                    semaphore.WaitOne();
                    headDirection = direction;
                    semaphore.Release();
                }
            }
        }
        static bool IsOppositeDirections(Direction direction1, Direction direction2)
        {
            if (direction1 == Direction.Left && direction2 == Direction.Right ||
                direction2 == Direction.Left && direction1 == Direction.Right ||
                direction1 == Direction.Up && direction2 == Direction.Down||
                direction2 == Direction.Up && direction1 == Direction.Down)
                return true;
            else
                return false;
        }
        public override void Move()
        {
            bool IsMadeFood = false;
            Point foodPoint = FoodFactory.GetRandomFood(new PointXY(PlayArea.startXY.X + 1, PlayArea.startXY.Y + 1),
                    new PointXY(PlayArea.Width - PlayArea.startXY.X - 1, PlayArea.ScoreInfoHeight - PlayArea.startXY.Y));
            long score = 0;
            PlayArea.PrintScore(score);
            while (true)
            {
                if (IsMadeFood == false)
                {
                    foodPoint = FoodFactory.GetRandomFood(new PointXY(PlayArea.startXY.X + 1, PlayArea.startXY.Y + 1),
                    new PointXY(PlayArea.Width - PlayArea.startXY.X - 1, PlayArea.ScoreInfoHeight - PlayArea.startXY.Y));
                    PlayArea.PrintInGameArea(foodPoint);
                    IsMadeFood = true;
                }

                semaphore.WaitOne();
                if (headDirection == Direction.Up)
                {
                    Points.Insert(0, new Point(Points[0].X, Points[0].Y - 1, Points[0].Symbol, headColor));
                    Points[1].Color = bodyColor;
                }
                else if (headDirection == Direction.Right)
                {
                    Points.Insert(0, new Point(Points[0].X + 1, Points[0].Y, Points[0].Symbol, headColor));
                    Points[1].Color = bodyColor;
                }
                else if (headDirection == Direction.Down)
                {
                    Points.Insert(0, new Point(Points[0].X, Points[0].Y + 1, Points[0].Symbol, headColor));
                    Points[1].Color = bodyColor;
                }
                else if (headDirection == Direction.Left)
                {
                    Points.Insert(0, new Point(Points[0].X - 1, Points[0].Y, Points[0].Symbol, headColor));
                    Points[1].Color = bodyColor;
                }
                semaphore.Release();
                for(int i = 1; i < Points.Count; i++)
                {
                    if (Points[0].X == Points[i].X && Points[0].Y == Points[i].Y)
                        Die();
                }
                for (int i = 0; i < Points.Count - 1; i++)
                {
                    try
                    {
                        PlayArea.PrintInGameArea(Points[i]);
                    }
                    catch
                    {
                        Die();
                    }
                }

                if(foodPoint.X == Points[0].X && foodPoint.Y == Points[0].Y)
                {
                    score++;
                    PlayArea.PrintScore(score);
                    IsMadeFood = false;
                    if (score % 10 == 0)
                    {
                        if (MoveSleep > 50)
                            MoveSleep -= 5;
                    }
                    else if (score % 100 == 0)
                    {
                        if (MoveSleep > 30)
                            MoveSleep -= 5;
                    }
                }
                else
                {
                    try
                    {
                        PlayArea.PrintInGameArea(Points[Points.Count - 1].XY, ' ');
                    }
                    catch
                    {
                        Die();
                    }
                    Points.RemoveAt(Points.Count - 1);
                }

                PlayArea.PrintPCTime();
                Thread.Sleep(MoveSleep);
            }
        }
        public void StartMove()
        {
            Control = new Thread(ReadDirection);
            Control.Start();
            Moving = new Thread(Move);
            Moving.Start();
        }
        public override void Die()
        {
            PlayArea.PrintMessage(" You died! Press 'Space' to try again (Doesn't work! In development!) or 'Crlt + C' to exit.", ConsoleColor.DarkYellow);

            if (Control.ThreadState == ThreadState.Running)
                Control.Suspend();
            if (Moving.ThreadState == ThreadState.Running)
                Moving.Suspend();

            //ConsoleKey key = Console.ReadKey().Key;
            //if (key == ConsoleKey.Spacebar)
            //{
            //    Points = new List<Point>(StartPoints);
            //    headDirection = StartHead;
            //    MoveSleep = StartMoveSleep;
            //    Control.Join();
            //}
        }

    }
}
