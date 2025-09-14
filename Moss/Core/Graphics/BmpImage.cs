using System;

public class BmpImage
{
    public readonly byte[] PixelData;
    public readonly int Width;
    public readonly int Height;

    public bool LoadedSuccessfully = false;

    public BmpImage(byte[] data)
    {
        if (data == null || data.Length < 54) return; // Minimal BMP header size

        if (data[0] != 'B' || data[1] != 'M') return; // Invalid BMP, safely exit

        Width = BitConverter.ToInt32(data, 18);
        Height = BitConverter.ToInt32(data, 22);
        int pixelDataOffset = BitConverter.ToInt32(data, 10);

        if (pixelDataOffset > data.Length) return; // Prevent overflow

        int pixelDataLength = data.Length - pixelDataOffset;
        PixelData = new byte[pixelDataLength];
        Array.Copy(data, pixelDataOffset, PixelData, 0, pixelDataLength);

        LoadedSuccessfully = true;
    }
}