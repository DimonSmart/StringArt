namespace DrawStringGeneticAlgorithm
{
    public record Range(int From, int To)
    {
        public static Range operator -(Range range, int offset)
        {
            return new Range(range.From - offset, range.From);
        }

        public override string ToString()
        {
            return $"F:{From}To:{To}";
        }
    }
}