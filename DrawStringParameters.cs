namespace StringArt
{
    public record DrawStringParameters(int Width, int Height, int Diameter, int QNails, int MaxLines = 1000)
    {
        public int Radius => Diameter / 2;
        public float CenterX => Width / 2;
        public float CenterY => Height / 2;
    }
}
