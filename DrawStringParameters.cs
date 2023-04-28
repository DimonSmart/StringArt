namespace StringArt
{
    public record DrawStringParameters(int Width, int Height, int Diameter, int QNails)
    {
        public int Radius => Diameter / 2;
        public float CenterX => Width / 2;
        public float CenterY => Height / 2;
        public int MaxLines { get; set; } = 1000;
    }
}
