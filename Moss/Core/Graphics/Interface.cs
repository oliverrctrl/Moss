using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Cosmos.System;
using Cosmos.System.Graphics;
using Moss.Core.Interfaces;
using Console = System.Console;
using Date = Cosmos.HAL.RTC;

namespace Moss.Core.Graphics;

public class Interface : ICoreScript
{
    private readonly int _width = CoreCanvas.Columns;
    private readonly int _height = CoreCanvas.Rows;

    private string _dateTimeString;
    private Thread _topbarThread;
    private bool _running = true;

    private List<Window> Windows;
    private List<Window> TilingWindows;
    public Window selectedWindow;

    
    
    private Exception _exception;

    public void Start()
    {
        CoreCanvas.Clear();
        
        // start the thread to update the date/time every second
        _topbarThread = new Thread(DateTimeThread);
        _topbarThread.Start();
        
        
        Windows = new List<Window>();
        TilingWindows =  new List<Window>();
    }

    public void Update()
    {
        if (_exception != null)
        {
            Thread.Sleep(1000); // Slows down the update loop to calm the CPU down
            return;
        }

        try
        {
            CoreCanvas.Clear(Color.Black);
            DrawTopbar();
            DrawWindows();
            if (KeyboardManager.ControlPressed)
            {
                var keyInfo = KeyboardManager.ReadKey();

                if (keyInfo.Key == ConsoleKeyEx.T)
                {
                    var newWindow = new Window("Terminal", CoreCanvas.Columns, CoreCanvas.Rows - 2, 0, 1, true);
                    Windows.Add(newWindow);
                    selectedWindow =  newWindow;
                }
            }
            CoreCanvas.Display();
        }
        catch (Exception e)
        {
            TriggerException(e);
        }
    }

    private void DrawWindows()
    {
        TilingWindows.Clear();
        foreach (var w in Windows.Where(w => w.Mode == WindowMode.Tiling))
        {
            TilingWindows.Add(w);
        }
        
        switch (TilingWindows.Count)
        {
            case 0:
                CoreCanvas.Write("Press CONTROL + T to open a terminal.", Color.White, 0, 1);
                break;
            case 1:
                TilingWindows[0].Draw(CoreCanvas.Columns,  CoreCanvas.Rows - 2, 0, 1, selectedWindow == TilingWindows[0]);
                break;
            case 2:
                TilingWindows[0].Draw(CoreCanvas.Columns / 2,  CoreCanvas.Rows - 2, 0, 1, selectedWindow == TilingWindows[0]);
                TilingWindows[1].Draw(CoreCanvas.Columns / 2,  CoreCanvas.Rows - 2, CoreCanvas.Columns / 2, 1, selectedWindow == TilingWindows[1]);
                break;
            case 3:
                TilingWindows[0].Draw(CoreCanvas.Columns / 2, CoreCanvas.Rows - 2, 0, 1, selectedWindow == TilingWindows[0]); 
                TilingWindows[1].Draw(CoreCanvas.Columns / 2, (CoreCanvas.Rows - 2) / 2, CoreCanvas.Columns / 2, 1, selectedWindow == TilingWindows[1]);
                TilingWindows[2].Draw(CoreCanvas.Columns / 2, (CoreCanvas.Rows - 2) / 2, CoreCanvas.Columns / 2, 1 + (CoreCanvas.Rows - 2) / 2, selectedWindow == TilingWindows[2]);
                break;
            case 4:
                TilingWindows[0].Draw(CoreCanvas.Columns / 2, (CoreCanvas.Rows - 2) / 2, 0, 1, selectedWindow == TilingWindows[0]);
                TilingWindows[1].Draw(CoreCanvas.Columns / 2, (CoreCanvas.Rows - 2) / 2, 0, 1 + (CoreCanvas.Rows - 2) / 2, selectedWindow == TilingWindows[1]);
                TilingWindows[2].Draw(CoreCanvas.Columns / 2, (CoreCanvas.Rows - 2) / 2, CoreCanvas.Columns / 2, 1, selectedWindow == TilingWindows[2]);
                TilingWindows[3].Draw(CoreCanvas.Columns / 2, (CoreCanvas.Rows - 2) / 2, CoreCanvas.Columns / 2, 1 + (CoreCanvas.Rows - 2) / 2, selectedWindow == TilingWindows[3]);
                break;
            default:
                CoreCanvas.Write("Interface does not support more than 4 windows. Please reset", Color.Red, 0, 1);
                break;
        }
    }
    private void DrawTopbar()
    {
        var left = $" MOSS v{Kernel.Version}";
        var right = _dateTimeString + " ";
        var space = _width - left.Length - right.Length;
        if (space < 0) space = 0;
        var line = left + new string(' ', space) + right;

        CoreCanvas.Write(line.PadRight(_width), Color.White, 0, 0, Color.Blue);
    }

    private void DateTimeThread()
    {
        while (_running)
        {
            _dateTimeString = $"{Date.Hour:D2}:{Date.Minute:D2}:{Date.Second:D2} {Date.DayOfTheMonth:D2}/{Date.Month:D2}/{Date.Year}";
            
            Thread.Sleep(1000);
        }
    }

    public void TriggerException(Exception ex)
    {
        _exception = ex;
        Stop();

        FullScreenCanvas.GetFullScreenCanvas().Disable();
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Clear();
        Console.WriteLine("A serious exception in the graphical system has occurred and the OS has been halted to prevent damage. Necessary debugging information will be printed below. Please restart your computer to continue.");
        Console.WriteLine("== System Crash ==");
        Console.WriteLine(_exception.Message);
        foreach (System.Collections.DictionaryEntry entry in _exception.Data)
        {
            Console.WriteLine($"{entry.Key}: {entry.Value}");
        }
    }

    public void Stop()
    {
        _running = false;
    }
}