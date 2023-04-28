namespace DrawStringGeneticAlgorithm
{
    public interface IChromosome<T>
    {
        T Mutate();
        T Crossover(T source);
    }
}