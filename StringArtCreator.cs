using SkiaSharp;
using StringArt.GeneticAlgorithm;
using StringArt.ScoreMasks;

namespace StringArt
{
    public class StringArtCreator
    {
        private readonly DrawStringParameters _drawStringParameters;
        private readonly FaceFeatureSettings _faceFeatureSettings;

        public delegate void IterationEventHandler(int iteration, string imageFileName, SKBitmap bitmap, ChromosomeWithScore<DrawStringChromosome> chromosomeWithScore);
        public delegate void GeneticIterationEventHandler();

        public event GeneticIterationEventHandler? OnGeneticIteration;
        public event IterationEventHandler? OnIteration;

        public StringArtCreator(DrawStringParameters drawStringParameters)
        {
            _drawStringParameters = drawStringParameters;
            _faceFeatureSettings = new FaceFeatureSettings(new[]
            {
                new FaceFeatureDescription(255, 5, FacePart.Jawline),
                new FaceFeatureDescription(255, 10, FacePart.RightEyebrow),
                new FaceFeatureDescription(255, 10, FacePart.LeftEyebrow),
                new FaceFeatureDescription(255, 10, FacePart.NoseBridge),
                new FaceFeatureDescription(255, 10, FacePart.NoseTip),
                new FaceFeatureDescription(255, 10, FacePart.RightEye),
                new FaceFeatureDescription(255, 10, FacePart.LeftEye),
                new FaceFeatureDescription(255, 10, FacePart.LipsOuterEdge),
                new FaceFeatureDescription(255, 10, FacePart.LipsInnerEdge)
            });
        }

        public void Create(string imageFileName)
        {
            var etalon = BitmapUtils.Resize(BitmapUtils.Load(imageFileName), _drawStringParameters.Width, _drawStringParameters.Height);
            var mask = new FaceFeaturesScoreMaskCreator(_faceFeatureSettings).Create(etalon);
            BitmapUtils.Save(mask, Path.GetFileNameWithoutExtension(imageFileName) + "_FaceMask.jpg");

            DrawStringFitness drawStringFitness = new DrawStringFitness(_drawStringParameters, etalon, mask);
            var genetic = new DrawStringGenetic(
                drawStringFitness,
                 new GeneticAlgorithmSettings()
                 {
                     Population = 100,
                     BestChromosomes = 50,
                     CrossoverChromosomes = 20,
                     Strategy = GeneticAlgorithmSettings.ScoreStrategy.Parallel
                 },
                 _drawStringParameters);

            int v = 0;
            const int IterationsInTurn = 100;
            const int StopIfNoProgressForXIteration = 3;
            const int StopIfMaxIterationsReached = 200;
            var queue = new Queue<int>();
            var lastResult = genetic.InitialScore;

            for (int i = 0; i < StopIfMaxIterationsReached; i++)
            {
                DoIterations(genetic, IterationsInTurn);

                var h = genetic.GetBestResult();
                queue.Enqueue(h.Score);
                if (queue.Count > StopIfNoProgressForXIteration)
                {
                    queue.Dequeue();
                }

                if (queue.Count == StopIfNoProgressForXIteration && !queue.Distinct().Skip(1).Any())
                {
                    break;
                }

                if (lastResult == h.Score)
                {
                    continue;
                }
                lastResult = h.Score;
                v++;

                using var bitmap = drawStringFitness.DrawChromosome(h.Chromosome);
                OnIteration?.Invoke(i, imageFileName, bitmap, h);
                
            }
            Console.WriteLine($"{imageFileName} finished");
        }

        private void DoIterations(DrawStringGenetic genetic, int IterationsInTurn)
        {
            for (int i = 0; i < IterationsInTurn; i++)
            {
                OnGeneticIteration?.Invoke();
                genetic.NextIteration();
            }
        }
    }
}
