using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using engine.data;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace engine.graphics
{
    public static class MeshLibrary
    {
        public static readonly Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>();
        private const string ModelPath = @"\..\..\Models\";

        public static Mesh GetMesh(string name)
        {
            if (Meshes.ContainsKey(name))
                return Meshes[name];
            if (File.Exists(Environment.CurrentDirectory + ModelPath + name))
                switch (name.Substring(name.LastIndexOf('.') + 1))
                {
                    case "obj":
                        return LoadObjFile(name);
                }
            return null;
        }

        public static void AddMesh(Mesh mesh, string name)
        {
            Meshes[name] = mesh;
        }

        #region From File
        /// <summary>
        /// http://en.wikipedia.org/wiki/Wavefront_.obj_file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Mesh LoadObjFile(string filename)
        {
            string src;
            using (var stream = new StreamReader(Environment.CurrentDirectory + ModelPath + filename))
            {
                src = stream.ReadToEnd();
            }

            List<string> lines = src.Split('\n').Where(s => !s.StartsWith("#")).ToList();

            var vertexs = new List<Vector3>();
            var vertexTextures = new List<Vector2>();
            var vertexNormals = new List<Vector3>();

            foreach (
                var xyzw in
                    lines.Where(s => s.StartsWith("v "))
                         .Select(s => s.Substring(s.IndexOf(' ') + 1).Split(' '))
                         .ToList())
            {
                string x = "", y = "", z = "", w = "";
                if (xyzw.Length >= 1)
                    x = xyzw[0];
                if (xyzw.Length >= 2)
                    y = xyzw[1];
                if (xyzw.Length >= 3)
                    z = xyzw[2];
                if (xyzw.Length == 4)
                    w = xyzw[3];
                // TODO: Vertex4 for position in the Vertex class
                vertexs.Add(new Vector3(float.Parse(x), float.Parse(y), float.Parse(z)));
            }

            // TODO: .mtl and that stuff
            foreach (
                var uvw in
                    lines.Where(s => s.StartsWith("vt "))
                         .Select(s => s.Substring(s.IndexOf(' ') + 1).Split(' '))
                         .ToList())
            {
                string u = "", v = "", w = "";
                if (uvw.Length >= 1)
                    u = uvw[0];
                if (uvw.Length >= 2)
                    v = uvw[1];
                if (uvw.Length == 3)
                    w = uvw[3];
                // TODO: Vertex3 for texture in the Vertex class
                vertexTextures.Add(new Vector2(float.Parse(u), float.Parse(v)));
            }

            foreach (
                var xyz in
                    lines.Where(s => s.StartsWith("vn "))
                         .Select(s => s.Substring(s.IndexOf(' ') + 1).Split(' '))
                         .ToList())
            {
                string x = "", y = "", z = "";
                if (xyz.Length >= 1)
                    x = xyz[0];
                if (xyz.Length >= 2)
                    y = xyz[1];
                if (xyz.Length >= 3)
                    z = xyz[2];
                vertexNormals.Add(new Vector3(float.Parse(x), float.Parse(y), float.Parse(z)).Normalized());
            }

            // TODO: lines starting with vp

            var vertices = new List<Vertex>();
            var indices = new List<int>();

            foreach (
                var face in
                    lines.Where(s => s.StartsWith("f "))
                         .Select(s => s.Substring(s.IndexOf(' ') + 1).Split(' '))
                         .ToList())
            {
                for (int i = 0; i < face.Length - 2; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        string[] vert = face[i + j].Split('/');
                        int v = 0, t = 0, n = 0;
                        v = int.Parse(vert[0]);
                        if (vert.Length >= 2 && vert[1] != string.Empty)
                            t = int.Parse(vert[1]);
                        if (vert.Length == 3)
                            n = int.Parse(vert[2]);

                        Vector3 position = vertexs[v - 1];
                        Vector2 texture = Vector2.Zero;
                        if (t != 0)
                            texture = vertexTextures[t - 1];
                        Vector3 normal = Vector3.Zero;
                        if (n != 0)
                            normal = vertexNormals[n - 1];

                        vertices.Add(new Vertex(position, normal, texture));
                        indices.Add(indices.Count);
                    }
                }
            }

            var mesh = new Mesh(
                BufferLibrary.CreateVertexBuffer(filename, vertices.ToArray()),
                BufferLibrary.CreateIndexBuffer(
                    filename, indices.Select(i => (uint) i).ToArray(), PrimitiveType.Triangles),
                indices.Count, PrimitiveType.Triangles);
            AddMesh(mesh, filename);
            return mesh;
        }
        #endregion
    }
}
