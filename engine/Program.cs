namespace engine
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //using (var game = new Game(GameWorldFactory.BuildDottedSphere(5f, 0.1f, 0.3f, 1 << 7, 1 << 6)))
            using (var game = new Game(GameWorldFactory.BuildCube()))
            {
                game.Run();
            }
        }
    }
}
