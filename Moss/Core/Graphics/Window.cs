using System;
using System.Drawing;

namespace Moss.Core.Graphics;

public class Window
{
    public string Title;
    public int Width;
    public int Height;
    public int X;
    public int Y;
    public bool Selected;
    public WindowCanvas WindowCanvas;
    
    public WindowMode Mode = WindowMode.Tiling;

    public Window(string title, int width, int height, int x, int y, bool selected)
    {
        Title = title;
        Width = width;
        Height = height;
        X = x;
        Y = y;
        Selected = selected;
        WindowCanvas = new WindowCanvas(this);
    }
    
    /// <summary>
    /// Draws a window, optionally with parameters
    /// </summary>
    /// <param name="w">Window width</param>
    /// <param name="h">Window height</param>
    /// <param name="posX">Window X position based on top left</param>
    /// <param name="posY">Window Y position based on top left</param>
    /// <param name="selected">Whether to apply the selection decoration</param>
    public void Draw(int? w = null, int? h = null, int? posX = null, int? posY = null, bool selected = false)
    {
        Width = w ?? Width;
        Height = h ?? Height;
        X = posX ?? X;
        Y = posY ?? Y;

        char topLeft     = '\xC9'; // ╔
        char topRight    = '\xBB'; // ╗
        char bottomLeft  = '\xC8'; // ╚
        char bottomRight = '\xBC'; // ╝
        char horizontal  = '\xCD'; // ═
        char vertical    = '\xBA'; // ║

        for (var y = 0; y < Height; y++)
        {
            int screenY = Y + y;

            if (y == 0) // Top border
            {
                CoreCanvas.Write(topLeft, Color.White, X, screenY);

                int horChars = Width - 2;
                if (!string.IsNullOrEmpty(Title))
                {
                    if (selected)
                        CoreCanvas.Write(Title, Color.Black, X + 1, screenY, Color.White);
                    else
                        CoreCanvas.Write(Title, Color.White, X + 1, screenY);
                        
                    horChars -= Title.Length;
                }

                CoreCanvas.Write(new string(horizontal, horChars), Color.White, X + (Title?.Length ?? 0) + 1, screenY);
                CoreCanvas.Write(topRight, Color.White, X + Width - 1, screenY);
            }
            else if (y == Height - 1) // Bottom border
            {
                CoreCanvas.Write(bottomLeft, Color.White, X, screenY);
                CoreCanvas.Write(new string(horizontal, Width - 2), Color.White, X + 1, screenY);
                CoreCanvas.Write(bottomRight,  Color.White, X + Width - 1, screenY);
            }
            else // Sides
            {
                CoreCanvas.Write(vertical, Color.White, X, screenY);
                CoreCanvas.Write(vertical, Color.White, X + Width - 1, screenY);
            }
        }
    }
}

public enum WindowMode
{
    Tiling,
    Floating,
    Fullscreen,
}
