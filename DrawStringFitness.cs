using GeneticAlgorithm;
using SkiaSharp;
using StringArt;

namespace DrawStringGeneticAlgorithm
{
    public class DrawStringFitness : IFitness<DrawStringChromosome>
    {
        private readonly SKBitmap _etalon;
        private readonly SKBitmap _coefficients;
        private readonly SKPoint[] _points;

        public DrawStringFitness(DrawStringParameters drawStringParameters, SKBitmap etalon)
        {
            P = drawStringParameters;
            _etalon = BitmapUtils.Resize(etalon, P.Width, P.Height);

            _stringLineStyle = new SKPaint()
            {
                IsAntialias = true,
                StrokeWidth = 1f,
                Color = SKColors.DarkGray,
                Style = SKPaintStyle.Fill,
                BlendMode = SKBlendMode.Multiply
            };

            _points = Enumerable.Range(0, P.QNails).Select(p => {
                float angle = 2f * (float)Math.PI * p / P.QNails;
                float x = P.CenterX + P.Radius * (float)Math.Cos(angle);
                float y = P.CenterY + P.Radius * (float)Math.Sin(angle);
                return new SKPoint(x, y);
            }).ToArray();

            _coefficients = CreateScoreMask();
        }

        public SKBitmap DrawChromosome(DrawStringChromosome chromosome)
        {
            var workBitmap = GetEmptyBitmap();
            using var canvas = new SKCanvas(workBitmap);
            for (int i = 0; i < chromosome.Nails.Length - 1; i++)
            {
                DrawLine(canvas, chromosome.Nails[i], chromosome.Nails[i + 1]);
            }
            return workBitmap;
        }

        private SKPoint GetPoint(int nail)
        {
            return _points[nail];
        }

        public void DrawLine(SKCanvas canvas, int from, int to)
        {
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

        public int GetScore(DrawStringChromosome chromosome)
        {
            using var workBitmap = DrawChromosome(chromosome);
            return BitmapDiffCalculator.CalculateGrayscaleDifference(workBitmap, _etalon, _coefficients);
        }

        private SKBitmap CreateScoreMask()
        {
            SKBitmap bitmap = GetEmptyBitmap();

            using (var canvas = new SKCanvas(bitmap))
            {
                // Draw black square
                SKPaint blackPaint = new()
                {
                    Color = SKColors.Black
                };
                canvas.DrawRect(new SKRect(0, 0, P.Width, P.Height), blackPaint);

                // Draw gradient circle
                var gradientPaint = new SKPaint
                {
                    Shader = SKShader.CreateRadialGradient(
                    new SKPoint(P.Width / 2, P.Height / 2), P.Height / 2,
                    new SKColor[] { SKColors.DarkGray, SKColors.Black, },
                    null, SKShaderTileMode.Clamp)
                };
                canvas.DrawCircle(new SKPoint(P.Width / 2, P.Height / 2), P.Radius, gradientPaint);
            }

            return bitmap;
        }


        private readonly SKPaint _stringLineStyle;
        private DrawStringParameters P { get; }
    }
}
