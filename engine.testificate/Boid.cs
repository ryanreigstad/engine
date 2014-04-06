//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using OpenTK;
//using OpenTK.Graphics;
//using OpenTK.Graphics.OpenGL;
//using OpenTK.Input;

//namespace engine.entities
//{
//    internal class Boid : Entity
//    {
//        public Boid(Vector3 position, Quaternion rotation, Vector3 scale, int number)
//            : base(position, rotation, scale)
//        {
//            Nearby = new List<Boid>();
//            Angle = 0.1f;
//            TargetOrientation = rotation;
//            Acceleration = 0;
//            PitchRotation = Quaternion.FromAxisAngle(Vector3.UnitX, 0.0f);
//            YawRotation = Quaternion.FromAxisAngle(Vector3.UnitY, 0.0f);
//            RollRotation = Quaternion.FromAxisAngle(Vector3.UnitZ, 0.0f);
//            Number = number;
//            PitchAngle = 0.0f;
//            YawAngle = 0.0f;
//            RollAngle = 0.0f;
//            Max = 4.0f;
//        }

//        public int Number { get; set; }

//        private float Max { get; set; }

//        public List<Boid> Nearby { get; set; }
//        private Quaternion TargetOrientation { get; set; }
//        private float Acceleration { get; set; }

//        private Quaternion PitchRotation { get; set; }
//        private Quaternion YawRotation { get; set; }
//        private Quaternion RollRotation { get; set; }

//        public float PitchAngle { get; set; }
//        public float YawAngle { get; set; }
//        public float RollAngle { get; set; }

//        public float Angle { get; set; }

//        public override void Update(KeyboardState keyboard, MouseState mouse)
//        {
//            if (keyboard.IsKeyDown(Key.F))
//            {
//                //Console.WriteLine("F");
//                Rotate();
//            }
//            Rotate();
//            Move();
//            Accelerate();
//            Nearby.Clear();
//            //Console.WriteLine(PitchAngle + ", " + YawAngle + ", " + RollAngle);
//        }

//        public override void Render()
//        {
//            //var m = Matrix3.CreateFromQuaternion(Rotation);
//            //Vector3 pos = m.Row0;
//           // Vector3 up = m.Row1;
//           // Vector3 forward = m.Row2;
//            const float u = -.10f;
//            GL.PushMatrix();
//            ApplyTransform();

//            GL.Begin(PrimitiveType.Triangles);
//            {
//                GL.Color4(Color4.Red);
//                //horizontal triangle (lies on the x and z plane)
//                GL.Vertex3(u, 0, u);
//                GL.Color4(Color4.Blue);
//                GL.Vertex3(0, 0, -u);
//                GL.Color4(Color4.Red);
//                GL.Vertex3(-u, 0, u);
//                GL.Color4(Color4.Green);
//                //vertical triangle (lies on the y and z plane
//                GL.Vertex3(0, -u, u);
//                GL.Color4(Color4.Blue);
//                GL.Vertex3(0, 0, -u);
//                GL.Color4(Color4.Green);
//                GL.Vertex3(0, u, u);

//            }
//            GL.End();

//            //GL.PopMatrix();

//            //GL.PushMatrix();
//            //ApplyTransform();
//            //GL.Begin(PrimitiveType.Lines);
//            //{
//            //    GL.Color4(Color4.Red);
//            //    GL.Vertex3(1, 0, 0);
//            //    GL.Color4(Color4.White);
//            //    GL.Vertex3(-1, 0, 0);
//            //    GL.Color4(Color4.Green);
//            //    GL.Vertex3(0, 1, 0);
//            //    GL.Color4(Color4.White);
//            //    GL.Vertex3(0, -1, 0);
//            //    GL.Color4(Color4.Blue);
//            //    GL.Vertex3(0, 0, 1);
//            //    GL.Color4(Color4.White);
//            //    GL.Vertex3(0, 0, -1);

//            //    //GL.Color4(Color4.Violet);
//            //    //GL.Vertex3(0, 0, 0);
//            //    //GL.Vertex3(forward.X, forward.Y, forward.Z);
//            //    //GL.Color4(Color4.Cyan);
//            //    //GL.Vertex3(0, 0, 0);
//            //    //GL.Vertex3(up.X, up.Y, up.Z); 
//            //    //GL.Color4(Color4.Chocolate);
//            //    //GL.Vertex3(0, 0, 0);
//            //    //GL.Vertex3(pos.X, pos.Y, pos.Z);

//            //}
//            //GL.End();
//            GL.PopMatrix();
//        }

//        public double DistanceTo(Boid c)
//        {

//            var tempX = c.Position.X - Position.X;
//            var tempY = c.Position.Y - Position.Y;
//            var tempZ = c.Position.Z - Position.Z;
//            return Math.Sqrt(tempX * tempX + tempY * tempY + tempZ * tempZ);
//        }

//        public void Decelerate()
//        {
//            if (Acceleration > 0)
//                Acceleration -= .001f;
//        }

//        private void Accelerate()
//        {
//            if (Acceleration < .05f)
//                Acceleration += 0.005f;
//        }
//        private void Move()
//        {
//            //if (Nearby.Count > 0)
//                Position = Wrap(0.1f);
//        }

//        //move towards average flock heading
//        private void Cohesion()
//        {
//            var pitch = PitchAngle;
//            var yaw = YawAngle;
//            var roll = RollAngle;
//            foreach (var v in Nearby)
//            {
//                pitch += v.PitchAngle;
//                yaw += v.YawAngle;
//                roll += v.RollAngle;
//            }
//            pitch /= Nearby.Count + 1;
//            yaw /= Nearby.Count + 1;
//            roll /= Nearby.Count + 1;

//            PitchAngle = pitch;
//            YawAngle = yaw;
//            RollAngle = roll;
//            Pitch();
//            Yaw();
//            Roll();
//        }

//        private void Rotate()
//        {
//            var i = 1;// GameWorldFactory.randy.Next(50);

//            if (Nearby.Count > 0)
//                Cohesion();

//            if (i < 25)
//                RotateRandom(.7f);

//            TargetOrientation = YawRotation * PitchRotation * RollRotation;
//            Rotation = Quaternion.Slerp(Rotation, TargetOrientation, 0.1f);
//        }

//        private Vector3 Wrap(float speed)
//        {
//            var temp = Position;
//            var x = temp.X;
//            var y = temp.Y;
//            var z = temp.Z;
//            var wrap = false;
//            if (x > Max)
//            {
//                x = -Max;
//                wrap = true;
//            }
//            if (x < -Max)
//            {
//                x = Max;
//                wrap = true;
//            }
//            if (y > Max)
//            {
//                y = -Max;
//                wrap = true;
//            }
//            if (y < -Max)
//            {
//                y = Max;
//                wrap = true;
//            }
//            if (z > Max)
//            {
//                z = -Max;
//                wrap = true;
//            }
//            if (z < -Max)
//            {
//                z = Max;
//                wrap = true;
//            }
//            if (wrap)
//                temp = new Vector3(x, y, z);
//            else
//            {
//                var m = Matrix3.CreateFromQuaternion(Rotation);
//                var r = m.Row2;
//                var v = new Vector3(r.X, r.Y, r.Z);
//                temp = Vector3.Lerp(Position, Position + v, Acceleration);
//            } 
//            return temp;
//        }

//        private void Pitch()
//        {
//            if (PitchAngle > 360.0f)
//                PitchAngle = PitchAngle - 360.0f;

//            PitchRotation = Quaternion.FromAxisAngle(Vector3.UnitX, PitchAngle);
//        }

//        private void Yaw()
//        {
//            if (YawAngle > 360.0f)
//                YawAngle = 360.0f - YawAngle;

//            YawRotation = Quaternion.FromAxisAngle(Vector3.UnitY, YawAngle);
//        }

//        private void Roll()
//        {
//            if (RollAngle > 360.0f)
//                RollAngle = 360.0f - RollAngle;

//            RollRotation = Quaternion.FromAxisAngle(Vector3.UnitZ, RollAngle);
//        }

//        private void RotateRandom(float angle)
//        {
//            var i = 1;// GameWorldFactory.randy.Next(7);
//            if (i == 1)
//                PitchAngle += angle;
//            if (i == 2)
//                YawAngle += angle;
//            if (i == 3)
//                RollAngle += angle;
//            if (i == 4)
//                PitchAngle -= angle;
//            if (i == 5)
//                YawAngle -= angle;
//            if (i == 6)
//                RollAngle -= angle;

//            Pitch();
//            Yaw();
//            Roll();
//        }
//    }
//}


