using OpenTK;
using OpenTK.Input;

namespace engine
{
    public class Entity
    {
        public Entity(string meshName, string textureName)
            : this(meshName, textureName, Vector3.Zero, Quaternion.Identity, Vector3.One)
        {
        }

        public Entity(string meshName, string textureName, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;

            MeshName = meshName;
            TextureName = textureName;
        }

        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public string MeshName { get; set; }
        public string TextureName { get; set; }

        public Matrix4 Transform
        {
            get
            {
                return Matrix4.CreateScale(Scale)
                       * Matrix4.CreateFromQuaternion(Rotation)
                       * Matrix4.CreateTranslation(Position);
            }
        }

        public Matrix4 RotationMatrix
        {
            get { return Matrix4.CreateFromQuaternion(Rotation); }
        }

        public virtual void OnLoad()
        {
        }

        public virtual void OnUpdate(KeyboardState keys, MouseState mouse)
        {
        }

        public void Move(Vector3 delta)
        {
            Position += delta;
        }

        public void MoveLocal(Vector3 delta)
        {
            Position += MakeLocal(delta);
        }

        /// <summary>
        ///     Pitch in local frame
        /// </summary>
        /// <param name="radians"></param>
        public void RotateX(float radians)
        {
            Rotation *= Quaternion.FromAxisAngle(Vector3.UnitX, radians);
            Rotation.Normalize();
        }

        /// <summary>
        ///     Yaw in local frame
        /// </summary>
        /// <param name="radians"></param>
        public void RotateY(float radians)
        {
            Rotation *= Quaternion.FromAxisAngle(Vector3.UnitY, radians);
            Rotation.Normalize();
        }

        /// <summary>
        ///     Roll in local frame
        /// </summary>
        /// <param name="radians"></param>
        public void RotateZ(float radians)
        {
            Rotation *= Quaternion.FromAxisAngle(Vector3.UnitZ, radians);
            Rotation.Normalize();
        }

        /// <summary>
        ///     Rotates and Scales a given vector
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector3 MakeLocal(Vector3 v)
        {
            return Vector3.Transform(v, Matrix4.CreateFromQuaternion(Rotation) * Matrix4.CreateScale(Scale));
        }
    }
}
