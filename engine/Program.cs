namespace engine
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var game = new Game(GameWorldFactory.BuildNewVertexBuffer()))
            {
                game.Run();
            }
        }
    }
}