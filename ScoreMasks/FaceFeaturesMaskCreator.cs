using DlibDotNet;
using SkiaSharp;

namespace StringArt.ScoreMasks;

public record FaceFeatureDescription (int Level, float Width, FacePart FacePart);

public record FaceFeatureSettings(FaceFeatureDescription[] Features);


public class FaceFeaturesScoreMaskCreator : IScoreMaskCreator
{
    private readonly FaceFeatureSettings settings;

    public FaceFeaturesScoreMaskCreator(FaceFeatureSettings settings)
    {
        //new List<FacePart> { Jawline, RightEyebrow, LeftEyebrow, NoseBridge, NoseTip, RightEye, LeftEye, LipsOuterEdge, LipsInnerEdge });
        this.settings = settings;
    }


    public static SKBitmap CreateMask(SKBitmap sourceBitmap, FaceFeatureSettings settings)
    {
        SKPaint maskPaint = new()
        {
            Color = SKColors.White,
            StrokeWidth = 5,
            IsAntialias = true,
            StrokeCap = SKStrokeCap.Round
        };

        SKBitmap bitmap = GetEmptyBitmap(sourceBitmap.Width, sourceBitmap.Width);
        SKPaint blackPaint = new()
        {
            Color = SKColors.Black
        };

        byte[] imageData = SKImage.FromBitmap(sourceBitmap).Encode().ToArray();
        var img = Dlib.LoadPng<byte>(imageData);

        using var fd = Dlib.GetFrontalFaceDetector();
        using var sp = ShapePredictor.Deserialize("shape_predictor_68_face_landmarks.dat");
        using (var canvas = new SKCanvas(bitmap))
        {
            canvas.DrawRect(new SKRect(0, 0, bitmap.Width, bitmap.Height), blackPaint);
            var faces = fd.Operator(img);
            foreach (var face in faces)
            {
                foreach (var feature in settings.Features)
                {
                    FullObjectDetection shape = sp.Detect(img, face);
                    maskPaint.Color = SKColor.FromHsv(0, 0, feature.Level);
                    maskPaint.StrokeWidth = face.Width * feature.Width / 100;
                    DrawFaceParts(maskPaint, canvas, shape, new List<FacePart> { feature.FacePart });
                }
            }
        }

        return bitmap;
    }

    static void DrawFaceParts(SKPaint blackPaint, SKCanvas canvas, FullObjectDetection shape, List<FacePart> faceParts)
    {
        foreach (FacePart facePart in faceParts)
        {
            var pointNumbers = FaceLandmarks.FacePartPoints[facePart];
            var pointFrom = GetSKPoint(shape, pointNumbers[0]);
            foreach (var pn in pointNumbers.Skip(1))
            {
                var pointTo = GetSKPoint(shape, pn);
                canvas.DrawLine(pointFrom, pointTo, blackPaint);
                pointFrom = pointTo;
            }
        }
    }

    private static SKPoint GetSKPoint(FullObjectDetection shape, uint i)
    {
        var p = shape.GetPart(i);
        return new SKPoint(p.X, p.Y);
    }

    public static SKBitmap GetEmptyBitmap(int width, int height)
    {
        SKImageInfo info = new(width, height, SKColorType.Gray8);
        SKBitmap grayscaleBitmap = new(info);
        using (SKCanvas canvas = new(grayscaleBitmap))
        {
            canvas.Clear(SKColors.White);
        }
        return grayscaleBitmap;
    }

    public SKBitmap Create(SKBitmap sourceBitmap)
    {
        return CreateMask(sourceBitmap, settings);
    }
}