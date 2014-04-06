using OpenTK.Input;

namespace engine.data
{
    public static class InputConfig
    {
        public const Key MoveForward = Key.W;
        public const Key MoveBackward = Key.S;
        public const Key MoveLeft = Key.A;
        public const Key MoveRight = Key.D;
        public const Key MoveUp = Key.LShift;
        public const Key MoveDown = Key.LControl;
        public const Key RollLeft = Key.Q;
        public const Key RollRight = Key.E;
        public const Key MoveFaster = Key.Plus;
        public const Key MoveSlower = Key.Minus;
        public static float MouseSensitivity = 0.001f;
    }

    public static class MyMath
    {
        public static float TwoPi = 6.28f;
        public static float ThreePiOverTwo = 4.17f;
        public static float Pi = 3.14f;
        public static float PiOverTwo = 1.57f;
    }
}
