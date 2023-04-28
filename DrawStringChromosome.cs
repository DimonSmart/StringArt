namespace DrawStringGeneticAlgorithm
{
    public sealed class DrawStringChromosome : IChromosome<DrawStringChromosome>
    {
        public readonly int[] Nails;

        public DrawStringChromosome(IEnumerable<int> nails)
        {
            Nails = nails.ToArray();
        }

        public DrawStringChromosome Crossover(DrawStringChromosome source)
        {
            var slicePosition = Random.Shared.Next(1, Nails.Length - 1);
            var newNails = Nails.Take(slicePosition).Concat(source.Nails.Skip(slicePosition));
            return new DrawStringChromosome(newNails);
        }

        DrawStringChromosome IChromosome<DrawStringChromosome>.Mutate()
        {
            var newChromosome = new DrawStringChromosome(Nails);
            var a = Random.Shared.Next(Nails.Length);
            var b = Random.Shared.Next(Nails.Length);
            (newChromosome.Nails[b], newChromosome.Nails[a]) = (newChromosome.Nails[a], newChromosome.Nails[b]);
            return newChromosome;
        }

        public override string ToString()
        {
            return string.Join(",", Nails);
        }
    }
}