namespace DrawStringGeneticAlgorithm
{
    public class GeneticScoreKeeper
    {
        public List<int> Results = new List<int>();
        private readonly int _minIterations;
        private readonly int _maxIterations;

        public GeneticScoreKeeper(int minIterations, int maxIterations)
        {
            _minIterations = minIterations;
            _maxIterations = maxIterations;
        }

        public bool IsNextIterationRequired()
        {
            if (Results.Count < _minIterations)
            {
                return true;
            }

            if (Results.Count > _maxIterations)
            {
                return true;
            }

            var initialImrouvement = 1.0 * Math.Abs(Results[0] - Results[9]) / _minIterations ;
            return Math.Abs(Results[^1] - Results[^2]) > initialImrouvement / 1000;
        }

        public void AddScore(int score)
        {
            if (Results.Any() && Results[^1] != score || !Results.Any())
            {
                Results.Add(score);
            }
        }
    }
}