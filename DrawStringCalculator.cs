using SkiaSharp;
using StringArt;

namespace DrawStringGeneticAlgorithm
{
    public class DrawStringCalculator
    {
        public DrawStringCalculator(DrawStringParameters drawStringParameters)
        {
            P = drawStringParameters;
            _stringLineStyle = new SKPaint()
            {
                IsAntialias = true,
                StrokeWidth = 4f,
                Color = SKColors.Gray,
                Style = SKPaintStyle.Fill,
                BlendMode = SKBlendMode.Multiply
            };
            _nailStyle = new SKPaint()
            {
                IsAntialias = true,
                StrokeWidth = 1f,
                Color = SKColors.DarkGray,
                Style = SKPaintStyle.Fill
            };
        }


        public void Solve(SKBitmap etalon, SKBitmap coefficients)
        {
            var bestPair = (0, 1);
            var bestResult = int.MaxValue;
            for (int i = 0; i < P.QNails - 1; i++)
            {
                for (int j = 0; j < P.QNails - 1; j++)
                {
                    if (NeihborNails(i, j))
                    {
                        continue;
                    }
                    var workBitmap = GetEmptyBitmap();
                    DrawLine(workBitmap, i, j);
                    var diff = BitmapDiffCalculator.CalculateGrayscaleDifference(workBitmap, etalon, coefficients);
                    if (diff < bestResult)
                    {
                        bestResult = diff;
                        bestPair = (i, j);
                        Console.WriteLine($"Result:{bestResult}, Pair:{bestPair}");
                    }
                }
            }
        }

        public (int i, int j, int score) GetBestLine(SKBitmap etalon, SKBitmap startBitmap, SKBitmap coefficients)
        {
            var bestResult = (start: 0, end: 0, score: int.MaxValue);
            for (int i = 0; i < P.QNails - 1; i++)
            {
                for (int j = i + 1; j < P.QNails - 1; j++)
                {
                    if (NeihborNails(i, j))
                    {
                        continue;
                    }

                    using var workBitmap = GetEmptyBitmap();
                    startBitmap.CopyTo(workBitmap, SKColorType.Gray8);
                    DrawLine(workBitmap, i, j);
                    var diff = BitmapDiffCalculator.CalculateGrayscaleDifference(workBitmap, etalon, coefficients);
                    if (diff < bestResult.score)
                    {
                        bestResult = (i, j, diff);
                        Console.WriteLine($"Result:{bestResult}");
                    }
                }
            }
            return bestResult;
        }

        public (int next, int score) GetBestLine(SKBitmap etalon, SKBitmap startBitmap, SKBitmap coefficients, int startNail)
        {
            var bestResult = (next: 0, score: int.MaxValue);
            for (int i = 0; i < P.QNails - 1; i++)
            {
                if (NeihborNails(startNail, i))
                {
                    continue;
                }

                using var workBitmap = GetEmptyBitmap();
                startBitmap.CopyTo(workBitmap, SKColorType.Gray8);
                DrawLine(workBitmap, i, startNail);
                var diff = BitmapDiffCalculator.CalculateGrayscaleDifference(workBitmap, etalon, coefficients);
                if (diff < bestResult.score)
                {
                    bestResult = (i, diff);
                    Console.WriteLine($"Result:{bestResult}");
                }

            }
            return bestResult;
        }

        private static bool NeihborNails(int startNail, int i)
        {
            return false;
            var limit = Random.Shared.Next(2, 10);
            return Math.Abs(i - startNail) < limit;
        }

        public void DrawNails(SKBitmap bitmap)
        {
            using var canvas = new SKCanvas(bitmap);
            for (int i = 0; i < P.QNails; i++)
            {
                canvas.DrawCircle(GetPoint(i), 5f, _nailStyle);
            }
        }

        private SKPoint GetPoint(int nail)
        {
            float angle = 2f * (float)Math.PI * nail / P.QNails;
            float x = P.CenterX + P.Radius * (float)Math.Cos(angle);
            float y = P.CenterY + P.Radius * (float)Math.Sin(angle);
            return new SKPoint(x, y);
        }

        public void DrawLine(SKBitmap bitmap, int from, int to)
        {
            using var canvas = new SKCanvas(bitmap);
            canvas.DrawLine(GetPoint(from), GetPoint(to), _stringLineStyle);
        }

        public SKBitmap GetEmptyBitmap()
        {
            SKImageInfo info = new(P.Width, P.Height, SKColorType.Gray8);
            SKBitmap grayscaleBitmap = new(info);
            using (SKCanvas canvas = new(grayscaleBitmap))
            {
                canvas.Clear(SKColors.White);
            }
            return grayscaleBitmap;
        }

        private readonly SKPaint _stringLineStyle;
        private readonly SKPaint _nailStyle;
        private DrawStringParameters P { get; }

    }
}
