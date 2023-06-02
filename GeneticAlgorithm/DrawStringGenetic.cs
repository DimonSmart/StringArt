using System.Text;

namespace StringArt.GeneticAlgorithm
{
    public class DrawStringGenetic : Genetic<DrawStringChromosome>
    {
        private readonly DrawStringFitness _drawStringFitness;
        private readonly DrawStringParameters _drawStringParameters;

        public DrawStringGenetic(
            DrawStringFitness drawStringGeneticCalculator,
            GeneticAlgorithmSettings geneticAlgorithmSettings,
            DrawStringParameters drawStringParameters)
            : base(geneticAlgorithmSettings)
        {
            _drawStringFitness = drawStringGeneticCalculator;
            _drawStringParameters = drawStringParameters;
        }

        protected override DrawStringChromosome GetNewChromosome()
        {
            return new DrawStringChromosome(Enumerable
                .Range(0, _drawStringParameters.MaxLines)
                .Select(i => Random.Shared.Next(_drawStringParameters.QNails)));
        }

        protected override IFitness<DrawStringChromosome> GetFitness()
        {
            return _drawStringFitness;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Points");
            sb.AppendLine(base.ToString());
            return sb.ToString();
        }
    }
}