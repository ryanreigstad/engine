using OpenTK.Input;

namespace engine
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
    }
}
