namespace StringArt.GeneticAlgorithm
{
    public abstract class Genetic<T> where T : IChromosome<T>
    {
        public enum FitnessDirection
        {
            HigherIsBetter,
            LowerIsBetter
        }

        private readonly ChromosomeWithScore<T>[] _chromosomes;

        private readonly GeneticAlgorithmSettings _settings;

        protected Genetic(GeneticAlgorithmSettings geneticAlgorithmSettings)
        {
            _settings = geneticAlgorithmSettings;
            _chromosomes = new ChromosomeWithScore<T>[_settings.Population];
        }

        public FitnessDirection Direction { get; } = FitnessDirection.LowerIsBetter;

        public int InitialScore => Direction == FitnessDirection.HigherIsBetter ? int.MinValue : int.MaxValue;

        public ChromosomeWithScore<T> GetBestResult()
        {
            return _chromosomes.First();
        }

        public IEnumerable<ChromosomeWithScore<T>> GetResults()
        {
            return _chromosomes.ToList();
        }

        private bool _isInitialized = false;
        public void Initialize()
        {
            SetNewChromosomes(_settings.PopulationRange);
            UpdateScore();
            Sort();
        }

        public void NextIteration()
        {
            if (!_isInitialized)
            {
                Initialize();
                _isInitialized = true;
                return;
            }

            Mutate();
            AddNewChromosomes();
            Crossover();
            UpdateScore();
            Sort();
        }

        private void Crossover()
        {
            var from = _settings.Population - _settings.CrossoverChromosomes - _settings.NewChromosomes;
            var to = _settings.Population - _settings.NewChromosomes;
            for (var i = from; i < to; i++)
            {
                var a = _chromosomes[Random.Shared.Next(_chromosomes.Length)];
                var b = _chromosomes[Random.Shared.Next(_chromosomes.Length)];
                if (a == b)
                {
                    continue;
                }
                _chromosomes[i].Chromosome = a.Chromosome.Crossover(b.Chromosome);
            }
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            foreach (var chromosomeWithScore in _chromosomes)
            {
                sb.AppendFormat("G:{0} S:{1}", chromosomeWithScore.Chromosome, chromosomeWithScore.Score);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        protected abstract IFitness<T> GetFitness();

        protected abstract T GetNewChromosome();

        private void AddNewChromosomes()
        {
            SetNewChromosomes(_settings.NewChromosomesRange);
        }

        private void Mutate()
        {
            if (_settings.MutationChromosomes == 0)
                return;

            for (var i = _settings.MutationChromosomesRange.From; i < _settings.MutationChromosomesRange.To; i++)
            {
                var pos = Random.Shared.Next(0, _settings.BodyRange.To);
                _chromosomes[i].Score = InitialScore;
                _chromosomes[i].Chromosome = _chromosomes[pos].Chromosome.Mutate();
            }
        }

        private void SetNewChromosomes(Range range)
        {
            for (var i = range.From; i < range.To; i++)
            {
                _chromosomes[i] = new ChromosomeWithScore<T>
                {
                    Chromosome = GetNewChromosome(),
                    Score = InitialScore
                };
            }
        }

        private void Sort()
        {
            if (Direction == FitnessDirection.LowerIsBetter)
            {
                Array.Sort(_chromosomes, (a, b) => a.Score - b.Score);
            }
            else
            {
                Array.Sort(_chromosomes, (a, b) => b.Score - a.Score);
            }
        }

        private void UpdateScore()
        {
            var fitness = GetFitness();
            if (_settings.Strategy == GeneticAlgorithmSettings.ScoreStrategy.Parallel)
            {
                Parallel.ForEach(_chromosomes.Where(i => i.Score == InitialScore),
                    _ => _.Score = fitness.GetScore(_.Chromosome));
            }
            else
            {
                foreach (var chromosomeWithScore in _chromosomes.Where(i => i.Score == InitialScore))
                {
                    chromosomeWithScore.Score = fitness.GetScore(chromosomeWithScore.Chromosome);
                }
            }
        }
    }
}