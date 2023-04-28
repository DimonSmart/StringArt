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

            //var param = new DrawStringParameters(150, 150, 148, 200)
            //{
            //    MaxLines = 500
            //};


            var images = new[] { "R.jpeg", "pic-main.jpg", "Face.png" };
            var stringArtCreator = new StringArtCreator(param);
            stringArtCreator.OnGeneticIteration += (s, e) => { Console.Write("."); };
            stringArtCreator.OnIteration += (int iteration, int score) => Console.WriteLine($"{iteration:000}:{score};");

            foreach (var image in images)
            {
                Console.WriteLine($"{Environment.NewLine}{image}");
                stringArtCreator.Create(image);
            }

            //var etalon = BitmapUtils.Load("Face.png");
            //etalon = BitmapUtils.Resize(etalon, param.Width, param.Height);
            //var drawStringCalculator = new DrawStringCalculator(param);
            //drawStringCalculator.DrawNails(etalon);

            //var workBitmap = drawStringCalculator.GetEmptyBitmap();

            //var bestLine = drawStringCalculator.GetBestLine(etalon, workBitmap);
            //drawStringCalculator.DrawLine(workBitmap, bestLine.i, bestLine.j);
            //int i = 0;

            //BitmapUtils.Save(workBitmap, $"{i:000} Start.png");

            //var optionA = drawStringCalculator.GetBestLine(etalon, workBitmap, bestLine.i);
            //var optionB = drawStringCalculator.GetBestLine(etalon, workBitmap, bestLine.j);
            //var startPoint = optionA.score < optionB.score ? bestLine.i : bestLine.j;

            //for (int v = 1; v < 1000; v++)
            //{
            //    var nextNail = drawStringCalculator.GetBestLine(etalon, workBitmap, startPoint);
            //    drawStringCalculator.DrawLine(workBitmap, startPoint, nextNail.next);
            //    startPoint = nextNail.next;
            //    BitmapUtils.Save(workBitmap, $"{v:000} step.png");
            //}
        }
    }
}
