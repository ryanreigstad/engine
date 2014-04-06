using engine.graphics;

namespace engine.testificate
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //using (var game = new Game(GameWorldFactory.BuildNewVertexBuffer()))
            using (var game = new Game(GameWorldFactory.World(), new Renderer()))
            {
                game.Run();
            }
        }
    }
}
