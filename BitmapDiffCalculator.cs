using SkiaSharp;

namespace StringArt
{
    public static class BitmapDiffCalculator
    {
        public static int CalculateGrayscaleDifference(SKBitmap bitmap1, SKBitmap bitmap2, SKBitmap coefficients)
        {
            // Check that the bitmaps have the same size and format
            if (bitmap1.Width != bitmap2.Width || bitmap1.Height != bitmap2.Height || bitmap1.Width != coefficients.Width || bitmap1.Height != coefficients.Height)
                throw new ArgumentException("Bitmaps must have the same size");

            if (bitmap1.ColorType != SKColorType.Gray8 || bitmap2.ColorType != SKColorType.Gray8 || coefficients.ColorType != SKColorType.Gray8)
                throw new ArgumentException("Bitmaps must be in 8-bit grayscale format");

            var resize = false;
            if (resize)
            {

                int newWidth = 150;  // bitmap1.Width / 4;
                int newHeight = 150; // bitmap1.Height / 4;

                // Create a new bitmap with the new size
                SKBitmap newBitmap1 = new SKBitmap(newWidth, newHeight, SKColorType.Gray8, SKAlphaType.Opaque);
                SKBitmap newBitmap2 = new SKBitmap(newWidth, newHeight, SKColorType.Gray8, SKAlphaType.Opaque);

                // Resize the original bitmap to the new size
                bitmap1.Resize(new SKImageInfo(newWidth, newHeight, SKColorType.Gray8), SKFilterQuality.Medium);
                bitmap2.Resize(new SKImageInfo(newWidth, newHeight, SKColorType.Gray8), SKFilterQuality.Medium);

                // Draw the resized bitmap onto the new bitmap
                using (var canvas = new SKCanvas(newBitmap1))
                {
                    canvas.DrawBitmap(bitmap1, SKRect.Create(newWidth, newHeight));
                }

                using (var canvas = new SKCanvas(newBitmap2))
                {
                    canvas.DrawBitmap(bitmap2, SKRect.Create(newWidth, newHeight));
                }

                bitmap1 = newBitmap1;
                bitmap2 = newBitmap2;
            }
            // Get the pixel spans for the bitmaps
            ReadOnlySpan<byte> pixels1 = bitmap1.Bytes;
            ReadOnlySpan<byte> pixels2 = bitmap2.Bytes;
            ReadOnlySpan<byte> coeff = coefficients?.Bytes;

            // Calculate the sum of absolute differences between the grayscale values of each pixel
            int sum = 0;
            for (int i = 0; i < pixels1.Length; i++)
            {
                byte value1 = pixels1[i];
                byte value2 = pixels2[i];

                int difference = Math.Abs(value1 - value2);
                difference = ScaleDown(difference, coeff[i]);
                sum += difference;
            }

            if (resize)
            {
                bitmap1.Dispose();
                bitmap2.Dispose();
            }

            return sum;
        }

        public static int ScaleDown(int initialDifference, int scale)
        {
            int scaledDifference;

            if (scale == 0)
            {
                scaledDifference = initialDifference / 2;
            }
            else if (scale == 255)
            {
                scaledDifference = initialDifference;
            }
            else
            {
                scaledDifference = (initialDifference * (scale + 128)) / 255;
            }

            return scaledDifference;
        }

    }
}
