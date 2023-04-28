using System.Diagnostics;

namespace GeneticAlgorithm
{
    [DebuggerDisplay("Score = {Score}")]

    public class ChromosomeWithScore<T>
    {
        public required T Chromosome;
        public required int Score;

        public override string ToString()
        {
            return $"Score: {Score}, Chromosome: {Chromosome}";
        }
    }
}