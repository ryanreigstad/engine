using System;
using engine.data.exceptions;
using engine.util.extensions;
using OpenTK.Input;

namespace engine.data.config
{
    public class InputConfig : AbstractSingletonConfig
    {
        private const string Filename = "input.config";

        private static InputConfig _instance;
        private static InputConfig i
        {
            get
            {
                if (_instance == null)
                    throw new ImplementationException("You cannot access config files ('{0}') before they have been loaded.".Formatted(Filename));
                return _instance;
            }
        }
        private static InputConfig GetDefaultInstance()
        {
            return new InputConfig
            {
                _quit = Key.Escape,
                _moveForward = Key.W,
                _moveBackward = Key.S,
                _moveLeft = Key.A,
                _moveRight = Key.D,
                _moveUp = Key.LShift,
                _moveDown = Key.LControl,
                _rollLeft = Key.Q,
                _rollRight = Key.E,
                _moveFaster = Key.Plus,
                _moveSlower = Key.Minus,
                _mouseSensitivity = 0.001f,
            };
        }

        private InputConfig()
            : base()
        {
        }

        private Key _quit;
        public static Key Quit
        {
            get { return i._quit; }
            set
            {
                if (i._quit != value)
                {
                    Key oldvalue = i._quit, newvalue = value;
                    i._quit = newvalue;
                    i.NotifyOnChanged(oldvalue, newvalue);
                }
            }
        }

        private Key _moveForward;
        public static Key MoveForward
        {
            get { return i._moveForward; }
            set
            {
                if (i._moveForward != value)
                {
                    Key oldvalue = i._moveForward, newvalue = value;
                    i._moveForward = newvalue;
                    i.NotifyOnChanged(oldvalue, newvalue);
                }
            }
        }

        private Key _moveBackward;
        public static Key MoveBackward
        {
            get { return i._moveBackward; }
            set
            {
                if (i._moveBackward != value)
                {
                    Key oldvalue = i._moveBackward, newvalue = value;
                    i._moveBackward = newvalue;
                    i.NotifyOnChanged(oldvalue, newvalue);
                }
            }
        }

        private Key _moveLeft;
        public static Key MoveLeft
        {
            get { return i._moveLeft; }
            set
            {
                if (i._moveLeft != value)
                {
                    Key oldvalue = i._moveLeft, newvalue = value;
                    i._moveLeft = newvalue;
                    i.NotifyOnChanged(oldvalue, newvalue);
                }
            }
        }

        private Key _moveRight;
        public static Key MoveRight
        {
            get { return i._moveRight; }
            set
            {
                if (i._moveRight != value)
                {
                    Key oldvalue = i._moveRight, newvalue = value;
                    i._moveRight = newvalue;
                    i.NotifyOnChanged(oldvalue, newvalue);
                }
            }
        }

        private Key _moveUp;
        public static Key MoveUp
        {
            get { return i._moveUp; }
            set
            {
                if (i._moveUp != value)
                {
                    Key oldvalue = i._moveUp, newvalue = value;
                    i._moveUp = newvalue;
                    i.NotifyOnChanged(oldvalue, newvalue);
                }
            }
        }

        private Key _moveDown;
        public static Key MoveDown
        {
            get { return i._moveDown; }
            set
            {
                if (i._moveDown != value)
                {
                    Key oldvalue = i._moveDown, newvalue = value;
                    i._moveDown = newvalue;
                    i.NotifyOnChanged(oldvalue, newvalue);
                }
            }
        }

        private Key _rollLeft;
        public static Key RollLeft
        {
            get { return i._rollLeft; }
            set
            {
                if (i._rollLeft != value)
                {
                    Key oldvalue = i._rollLeft, newvalue = value;
                    i._rollLeft = newvalue;
                    i.NotifyOnChanged(oldvalue, newvalue);
                }
            }
        }

        private Key _rollRight;
        public static Key RollRight
        {
            get { return i._rollRight; }
            set
            {
                if (i._rollRight != value)
                {
                    Key oldvalue = i._rollRight, newvalue = value;
                    i._rollRight = newvalue;
                    i.NotifyOnChanged(oldvalue, newvalue);
                }
            }
        }

        private Key _moveFaster;
        public static Key MoveFaster
        {
            get { return i._moveFaster; }
            set
            {
                if (i._moveFaster != value)
                {
                    Key oldvalue = i._moveFaster, newvalue = value;
                    i._moveFaster = newvalue;
                    i.NotifyOnChanged(oldvalue, newvalue);
                }
            }
        }

        private Key _moveSlower;
        public static Key MoveSlower
        {
            get { return i._moveSlower; }
            set
            {
                if (i._moveSlower != value)
                {
                    Key oldvalue = i._moveSlower, newvalue = value;
                    i._moveSlower = newvalue;
                    i.NotifyOnChanged(oldvalue, newvalue);
                }
            }
        }

        private float _mouseSensitivity;
        public static float MouseSensitivity
        {
            get { return i._mouseSensitivity; }
            set
            {
                if (Math.Abs(i._mouseSensitivity - value) > 0.0001f)
                {
                    float oldvalue = i._mouseSensitivity, newvalue = value;
                    i._mouseSensitivity = newvalue;
                    i.NotifyOnChanged(oldvalue, newvalue);
                }
            }
        }
    }
}
