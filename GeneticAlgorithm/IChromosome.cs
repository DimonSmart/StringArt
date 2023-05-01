namespace StringArt.GeneticAlgorithm
{
    public interface IChromosome<T>
    {
        T Mutate();
        T Crossover(T source);
    }
}