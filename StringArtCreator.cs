using StringArt.GeneticAlgorithm;
using StringArt.ScoreMasks;

namespace StringArt
{
    public class StringArtCreator
    {
        private readonly DrawStringParameters _drawStringParameters;

        public delegate void IterationEventHandler(int iteration, int score);

        public event EventHandler? OnGeneticIteration;
        public event IterationEventHandler? OnIteration;

        public StringArtCreator(DrawStringParameters drawStringParameters)
        {
            _drawStringParameters = drawStringParameters;
        }

        public void Create(string imageFileName)
        {
            var etalon = BitmapUtils.Resize(BitmapUtils.Load(imageFileName), _drawStringParameters.Width, _drawStringParameters.Height);
            var mask = new FaceFeaturesScoreMaskCreator().Create(etalon);
            BitmapUtils.Save(mask, Path.GetFileNameWithoutExtension(imageFileName) + "_FaceMask.jpg");

            var drawStringGeneticCalculator = new DrawStringFitness(_drawStringParameters, etalon, mask);
            var genetic = new DrawStringGenetic(
                drawStringGeneticCalculator,
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
            const int StopIfMaxIterationsReached = 100;
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

                OnIteration?.Invoke(i, lastResult);
                using var bitmap = drawStringGeneticCalculator.DrawChromosome(h.Chromosome);
                BitmapUtils.Save(bitmap, $"{Path.GetFileNameWithoutExtension(imageFileName)} {v:000} ({h.Score}).png");
            }
            Console.WriteLine($"{imageFileName} finished");
        }

        private void DoIterations(DrawStringGenetic genetic, int IterationsInTurn)
        {
            for (int i = 0; i < IterationsInTurn; i++)
            {
                OnGeneticIteration?.Invoke(this, null);
                genetic.NextIteration();
            }
        }
    }
}
