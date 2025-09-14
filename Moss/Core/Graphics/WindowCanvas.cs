using System;
using System.Drawing;
using Cosmos.System;
using Moss.Core.Graphics.Elements;

namespace Moss.Core.Graphics;

public class WindowCanvas
{
    private readonly Window _window;
    public WindowCanvas(Window window) => _window = window;
    
    /// <summary>
    /// Write text to the window
    /// </summary>
    /// <param name="text">What text should be written</param>
    /// <param name="color">The color of the text</param>
    /// <param name="posX">X position of the text</param>
    /// <param name="posY">Y position of the text</param>
    /// <param name="backColor">The background/highlight color of the text</param>
    public void Write(string text, Color? color = null, int? posX = null, int? posY = null, Color? backColor = null)
    {
        int x = (posX ?? 0) + 1;
        int y = (posY ?? 0) + 2;

        x = Math.Min(x, _window.Width - 1);
        y = Math.Min(y, _window.Height - 1);

        if (x + text.Length > _window.Width - 1)
            text = text.Substring(0, _window.Width - 1 - x);

        CoreCanvas.Write(text, color, x, y, backColor);
    }

    public void DrawTextbox(Textbox textbox)
    {
        int x = textbox.posX + 1;
        int y = textbox.posY + 2;
        
        x = Math.Min(x, _window.Width - 1);
        y = Math.Min(y, _window.Height - 1);
        
        if (textbox.placeholder != null && textbox.text.Length == 0)
            CoreCanvas.Write(textbox.placeholder, Color.DimGray, x, y, null);
        
        CoreCanvas.DrawRectangle(textbox.color, x, y, textbox.width, CoreCanvas.Font.Height + 12);
        CoreCanvas.Write(textbox.text,  textbox.color, x, y);
    }

    /// <summary>
    /// Starts recording what the user types until they press enter
    /// </summary>
    /// <param name="state">Input state object</param>
    /// <param name="textbox">Optionally draws a textbox with the object provided. Refrain from creating the object in Update()</param>
    /// <returns></returns>
    public string? ReadLine(InputState state, Textbox? textbox = null)
    {
        if (state.IsComplete)
            return state.Buffer;

        var keyEvent = KeyboardManager.ReadKey();
        if (keyEvent.Key == ConsoleKeyEx.Enter)
        {
            state.IsComplete = true;
        }
        else if (keyEvent.Key == ConsoleKeyEx.Backspace)
        {
            if (state.Buffer.Length > 0)
                state.Buffer = state.Buffer.Substring(0, state.Buffer.Length - 1);
        }
        else if (keyEvent.KeyChar != '\0')
        {
            state.Buffer += keyEvent.KeyChar;
        }

        if (textbox != null)
        {
            textbox.text = state.Buffer;
            DrawTextbox(textbox);
        }

        return state.IsComplete ? state.Buffer : null;
    }
}

public class InputState
{
    public string Buffer = "";
    public bool IsComplete = false;
}