namespace StringArt.GeneticAlgorithm
{
    public interface IFitness<in T>
    {
        int GetScore(T chromosome);
    }
}