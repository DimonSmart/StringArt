using SkiaSharp;

namespace StringArt
{
    public static class BitmapUtils
    {
        public static SKBitmap Load(string fileName)
        {
            using var stream = File.OpenRead(fileName);
            using var bitmap = SKBitmap.Decode(stream);
            var grayBitmap = new SKBitmap(bitmap.Width, bitmap.Height, SKColorType.Gray8, SKAlphaType.Opaque);
            bitmap.CopyTo(grayBitmap, SKColorType.Gray8);
            return grayBitmap;
        }

        public static void Save(SKBitmap bitmap, string fileName)
        {
            SKData skData = bitmap.Encode(SKEncodedImageFormat.Png, 100);
            using Stream stream = File.OpenWrite(fileName);
            skData.SaveTo(stream);
        }

        public static SKBitmap Resize(SKBitmap source, int width, int height)
        {
            SKBitmap newBitmap = new SKBitmap(width, height, SKColorType.Gray8, SKAlphaType.Opaque);

            source.Resize(new SKImageInfo(width, height, SKColorType.Gray8), SKFilterQuality.Medium);

            using (SKCanvas canvas = new SKCanvas(newBitmap))
            {
                canvas.DrawBitmap(source, SKRect.Create(width, height));
            }

            return newBitmap;
        }
    }
}
