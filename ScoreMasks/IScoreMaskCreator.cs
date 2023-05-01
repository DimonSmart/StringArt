using SkiaSharp;

namespace StringArt.ScoreMasks;

public interface IScoreMaskCreator
{
    public SKBitmap Create(SKBitmap sourceBitmap);
}
