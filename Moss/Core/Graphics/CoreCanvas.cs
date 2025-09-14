using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using IL2CPU.API.Attribs;
using Moss.Core.Interfaces;

namespace Moss.Core.Graphics;

public static class CoreCanvas
{
    private static readonly Canvas Canvas;
    public static readonly Font Font;

    public static readonly int Columns;
    public static readonly int Rows;

    public static uint ScreenWidth = 640;
    public static uint ScreenHeight = 480;

    [ManifestResourceStream(ResourceName = "Moss.Core.Assets.font.bmp")]
    private static byte[] _defaultFont;
    
    private static Vector2 _cursorPos = new(0, 0);

    static CoreCanvas()
    {
        Canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(ScreenWidth, ScreenHeight, ColorDepth.ColorDepth32));
        Canvas.Clear(Color.Black);

        Columns = (int)ScreenWidth / Font.Width;
        Rows = (int)ScreenHeight / Font.Height;
        
       
        
        // If the system font is changed, this needs to be updated
        Font = new Font
        {
            Width = 8,
            Height = 16,
            BmpImage = new BmpImage(_defaultFont),
            GlyphGap = 0,
            FirstCode = 0,
        };
    }

    public static void Display()
    {
        Canvas.Display();   
    }

    public static void Write(char ch, Color? color = null, int? posX = null, int? posY = null, Color? backColor = null)
    {
        Write(ch.ToString(), color, posX, posY, backColor);
    }
    public static void Write(string text, Color? color = null, int? posX = null, int? posY = null, Color? backColor = null)
    {
        
        var x = (posX ?? (int)_cursorPos.X) * Font.Width;
        var y = (posY ?? (int)_cursorPos.Y) * Font.Height;
        var c = color ?? Color.White;

        if (backColor != null)
            Canvas.DrawFilledRectangle(backColor.Value, x, y, Font.Width * text.Length, Font.Height, true);

        int cursorX = x;
        foreach (var ch in text)
        {
            DrawGlyph(ch, cursorX, y, c);
            cursorX += Font.Width;
        }
        
    }

    private static void DrawGlyph(char c, int x, int y, Color color)
    {

        int index = (byte)c - Font.FirstCode;
        if (index < 0 || index > 255) return; // Out of range

        int glyphsPerRow = Font.BmpImage.Width / (Font.Width + Font.GlyphGap);

        int glyphX = (index % glyphsPerRow) * (Font.Width + Font.GlyphGap);
        int glyphY = (index / glyphsPerRow) * (Font.Height + Font.GlyphGap);

        // Assume 24-bit bmp
        int bytesPerPixel = 3;
        int stride = (Font.BmpImage.Width * bytesPerPixel + 3) & ~3;

        for (int py = 0; py < Font.Height; py++)
        {
            for (int px = 0; px < Font.Width; px++)
            {
                int pixelIndex = ((Font.BmpImage.Height - 1 - (glyphY + py)) * stride +
                                  (glyphX + px) * bytesPerPixel);

                byte r = Font.BmpImage.PixelData[pixelIndex];
                byte g = Font.BmpImage.PixelData[pixelIndex + 1];
                byte b = Font.BmpImage.PixelData[pixelIndex + 2];

                // Treat white/light pixels as "on"
                if ((r + g + b) / 3 > 128)
                    Canvas.DrawPoint(color, x + px, y + py);
            }
        }
    }

    public static void DrawRectangle(Color color, int x, int y, int width, int height)
    {
        Canvas.DrawRectangle(color, x, y, width, height);
    }

    public static void SetCursorPosition(int x, int y)
    {
        _cursorPos = new Vector2(x, y);
    }

    public static void Clear(Color? color = null)
    {
        Canvas.Clear(color ?? Color.Black);
    }
}