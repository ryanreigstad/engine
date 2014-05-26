using engine.core.rendering;

namespace engine.testificate
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            using (var game = new Game(GameWorldFactory.World(), new Renderer()))
            {
                game.Run();
            }
        }
    }
}
