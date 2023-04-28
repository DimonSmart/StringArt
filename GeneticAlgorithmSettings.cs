using System.Runtime.Serialization;
using System.Text;

namespace DrawStringGeneticAlgorithm
{
    [DataContract]
    [KnownType(typeof(ScoreStrategy))]
    public class GeneticAlgorithmSettings
    {
        public enum ScoreStrategy
        {
            Parallel,
            Sequental
        }

        public ScoreStrategy Strategy { get; set; } = ScoreStrategy.Sequental;

        [DataMember]
        public int BestChromosomes = 50;

        /// <summary>
        ///     Chromosomes generated with crossover function on each iteration
        ///     0 ... (Population - KeepGens - NewGens)
        /// </summary>
        /// 
        [DataMember]
        public int CrossoverChromosomes;

        [DataMember]
        public int MutationChromosomes = 20;

        /// <summary>
        /// New chromosomes added every iteration
        /// </summary>
        // [DataMember]
        public int NewChromosomes => Population - MutationChromosomes - CrossoverChromosomes - BestChromosomes;

        /// <summary>
        /// Total chromosomes quantity, i.e. Population
        /// </summary>
        [DataMember]
        public int Population = 100;
        public Range PopulationRange => new(0, Population);
        public Range NewChromosomesRange => new Range(Population - NewChromosomes, Population);
        public Range MutationChromosomesRange => NewChromosomesRange - MutationChromosomes;

        // NB! Update in case of add remove specialized range
        public Range BodyRange => new Range(BestChromosomes, MutationChromosomesRange.From);

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Population: {Population}");
            sb.AppendLine($"PopulationRange: {PopulationRange}");
            sb.AppendLine($"NewChromosomes: {NewChromosomes}");
            sb.AppendLine($"NewChromosomesRange: {NewChromosomesRange}");
            sb.AppendLine($"MutationChromosomes: {MutationChromosomes}");
            sb.AppendLine($"MutationChromosomesRange: {MutationChromosomesRange}");
            return sb.ToString();
        }
    }
}