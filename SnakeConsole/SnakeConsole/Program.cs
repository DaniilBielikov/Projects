using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using static SnakeConsole.ConsoleHelper;

namespace SnakeConsole
{
    internal class Program
    {
        static Snake snake;
        static void Main(string[] args)
        {
            Console.Title = "Snake \\^_^/";
            int consoleWidth = 100;
            int consoleHeight = 70;
            Console.SetWindowSize(consoleWidth, consoleHeight);
            Console.SetBufferSize(consoleWidth, consoleHeight);
            Console.CursorVisible = false;

            ConsoleHelper.SetCurrentFont("Terminal", 11);

            ConsoleHelper.DeleteMenu(ConsoleHelper.GetSystemMenu(ConsoleHelper.GetConsoleWindow(), false), ConsoleHelper.SC_MAXIMIZE, ConsoleHelper.MF_BYCOMMAND);
            ConsoleHelper.DeleteMenu(ConsoleHelper.GetSystemMenu(ConsoleHelper.GetConsoleWindow(), false), ConsoleHelper.SC_SIZE, ConsoleHelper.MF_BYCOMMAND);
            ConsoleHelper.DeleteMenu(ConsoleHelper.GetSystemMenu(ConsoleHelper.GetConsoleWindow(), false), ConsoleHelper.SC_CLOSE, ConsoleHelper.MF_BYCOMMAND);

            PrintPoint.consoleWidth = consoleWidth - 1;
            PrintPoint.consoleHeight = consoleHeight - 2;
            PlayArea.GetPlayArea(0, 0, PrintPoint.consoleHeight, PrintPoint.consoleWidth, '-', '|', Convert.ToInt32(PrintPoint.consoleHeight * 0.9));
            PlayArea.PrintMessage(" Press 'Crlt + C' to exit!", ConsoleColor.DarkYellow);

            snake = Snake.GetSnake(ConsoleColor.DarkYellow, ConsoleColor.Blue, 100);
            snake.StartMove();
            Console.CancelKeyPress += Console_CancelKeyPress;
        }
        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            snake.StopPlay();
            PlayArea.PrintMessage(new string(' ', PlayArea.MaxMessageLength));
            PlayArea.PrintMessage(" Thank you for playing. The game is still in development. \\^_^/", ConsoleColor.Blue);
            Thread.Sleep(2000);
            Environment.Exit(0);
        }
    }
}
