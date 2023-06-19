using SkiaSharp;
using StringArt.GeneticAlgorithm;
using System.CommandLine;

namespace StringArt
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var widthOption = new Option<int>(
                name: "--width",
                getDefaultValue: () => 650,
                description: "The width of the object.")
            { IsRequired = false };

            var heightOption = new Option<int>(
                name: "--height",
                getDefaultValue: () => 650,
                description: "The height of the object.")
            { IsRequired = false };

            var diameterOption = new Option<int>(
                name: "--diameter",
                getDefaultValue: () => 650,
                description: "The diameter of the object.")
            { IsRequired = false };

            var nailsOption = new Option<int>(
                name: "--qnails",
                getDefaultValue: () => 300,
                description: "The number of nails.")
            { IsRequired = false };

            var maxLinesOption = new Option<int>(
                name: "--maxlines",
                getDefaultValue: () => 2000,
                description: "The maximum number of lines.")
            { IsRequired = false };

            var fileNamesArgument = new Argument<string[]>(
                name: "filenames",
                description: "The names of image files to process.");

            var rootCommand = new RootCommand("String Art calculator");
            rootCommand.AddOption(widthOption);
            rootCommand.AddOption(heightOption);
            rootCommand.AddOption(diameterOption);
            rootCommand.AddOption(nailsOption);
            rootCommand.AddOption(maxLinesOption);
            rootCommand.AddArgument(fileNamesArgument);

            rootCommand.SetHandler((width, height, diameter, nails, maxLines, filenames) =>
            {
                Console.WriteLine($"Width: {width}");
                Console.WriteLine($"Height: {height}");
                Console.WriteLine($"Diameter: {diameter}");
                Console.WriteLine($"Nails: {nails}");
                Console.WriteLine($"MaxLines: {maxLines}");
                Console.WriteLine($"Filenames: {string.Join(", ", filenames)}");

                var param = new DrawStringParameters(width, height, diameter, nails, maxLines);
                //                var images = new[] { @"Samples\MapleLeaf.jpeg" }; //, @"Samples\Queen Elizabeth.png", @"Samples\R.jpeg", @"Samples\pic-main.jpg", @"Samples\Face.png" };

                var stringArtCreator = new StringArtCreator(param);
                stringArtCreator.OnGeneticIteration += () => { Console.Write("."); };
                stringArtCreator.OnIteration += (int iteration, string imageFileName, SKBitmap bitmap, ChromosomeWithScore<DrawStringChromosome> ch) =>
                {
                    Console.WriteLine($"{iteration:000}:{ch.Score};");
                    BitmapUtils.Save(bitmap, $"FullFaceMask_{Path.GetFileNameWithoutExtension(imageFileName)} {iteration:000} ({ch.Score}).png");
                };

                foreach (var filename in filenames)
                {
                    Console.WriteLine($"{Environment.NewLine}{filename}");
                    stringArtCreator.Create(filename);
                }

            }, widthOption, heightOption, diameterOption, nailsOption, maxLinesOption, fileNamesArgument);
            rootCommand.Invoke(args);

        }
    }
}
