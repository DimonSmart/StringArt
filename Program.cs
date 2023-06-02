using SkiaSharp;
using StringArt.GeneticAlgorithm;

namespace StringArt
{
    public class Program
    {
        static void Main(string[] args)
        {
            var param = new DrawStringParameters(650, 650, 650, 300)
            {
                MaxLines = 1500
            };

            var images = new[] { @"Samples\MapleLeaf.jpeg" }; //, @"Samples\Queen Elizabeth.png", @"Samples\R.jpeg", @"Samples\pic-main.jpg", @"Samples\Face.png" };
            var stringArtCreator = new StringArtCreator(param);
            stringArtCreator.OnGeneticIteration += () => { Console.Write("."); };
            stringArtCreator.OnIteration += (int iteration, string imageFileName, SKBitmap bitmap, ChromosomeWithScore<DrawStringChromosome> ch) => {
                Console.WriteLine($"{iteration:000}:{ch.Score};");
                BitmapUtils.Save(bitmap, $"FullFaceMask_{Path.GetFileNameWithoutExtension(imageFileName)} {iteration:000} ({ch.Score}).png");
            };

            foreach (var image in images)
            {
                Console.WriteLine($"{Environment.NewLine}{image}");
                stringArtCreator.Create(image);
            }
        }
    }
}
