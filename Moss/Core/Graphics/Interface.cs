using System;
using Cosmos.System;
using Moss.Core.Interfaces;
using Console = System.Console;
using Date = Cosmos.HAL.RTC;

namespace Moss.Core.Shell;

public class Interface : ICoreScript
{
    private int width = Console.WindowWidth;
    private int height = Console.WindowHeight;

    private string _dateTimeString;
    private Thread topbarThread;
    private bool running = true;
    

    public void Start()
    {
        Console.Clear();

        // start the thread to update the date/time every second
        topbarThread = new Thread(UpdateTopbar);
        topbarThread.Start();
    }

    public void Update()
    {
        
    }
    
    private void UpdateTopbar()
    {
        while (running)
        {
            _dateTimeString = $"{Date.Hour:D2}:{Date.Minute:D2}:{Date.Second:D2} {Date.DayOfTheMonth:D2}/{Date.Month:D2}/{Date.Year}";
            
            var left = $"MOSS v{Kernel.Version}";
            var right = _dateTimeString ?? "";
            var space = width - left.Length - right.Length;
            if (space < 0) space = 0;
            var line = left + new string(' ', space) + right;

            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(line.PadRight(width));
            
            Thread.Sleep(1000);
        }
    }

    public void Stop()
    {
        // stop the date/time thread safely
        running = false;
    }
}