
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace TM
{
    public class TileMap : Transformable, Drawable
    {
        VertexArray m_vertices = new VertexArray();
        Texture m_tileset;

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform = Transform;
            states.Texture = m_tileset;
            target.Draw(m_vertices, states);
        }

        public bool load(String tileSet, Vector2u tileSize, int[] tiles, uint width, uint height)
        {
            if (!File.Exists(tileSet))
                return false;
            else m_tileset = new Texture(tileSet);

            m_vertices.PrimitiveType = PrimitiveType.Quads;
            m_vertices.Resize(width * height * 4);

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    int tileNumber = tiles[i + j * width];
                    int tu = (int)(tileNumber % (m_tileset.Size.X / tileSize.X));
                    int tv = (int)(tileNumber / (m_tileset.Size.X / tileSize.X));

                    uint index = (uint)((i + j * width) * 4);
                    m_vertices[index + 0] = new Vertex(new Vector2f(i * tileSize.X, j * tileSize.Y), new Vector2f(tu * tileSize.X, tv * tileSize.Y));
                    m_vertices[index + 1] = new Vertex(new Vector2f((i + 1) * tileSize.X, j * tileSize.Y), new Vector2f((tu + 1) * tileSize.X, tv * tileSize.Y));
                    m_vertices[index + 2] = new Vertex(new Vector2f((i+1) * tileSize.X, (j + 1) * tileSize.Y), new Vector2f((tu+1) * tileSize.X, (tv + 1) * tileSize.Y));
                    m_vertices[index + 3] = new Vertex(new Vector2f(i * tileSize.X, (j + 1) * tileSize.Y), new Vector2f(tu * tileSize.X, (tv + 1) * tileSize.Y));
                }
            return true;
        }

        public class Program
        {
            public static void Main(string[] args)
            {
                RenderWindow window = new RenderWindow(new VideoMode(512, 256), "TileMap");
                int[] level = new int[]
                {
                    0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                    0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 2, 0, 0, 0, 0,
                    1, 1, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3,
                    0, 1, 0, 0, 2, 0, 3, 3, 3, 0, 1, 1, 1, 0, 0, 0,
                    0, 1, 1, 0, 3, 3, 3, 0, 0, 0, 1, 1, 1, 2, 0, 0,
                    0, 0, 1, 0, 3, 0, 2, 2, 0, 0, 1, 1, 1, 1, 2, 0,
                    2, 0, 1, 0, 3, 0, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1,
                    0, 0, 1, 0, 3, 2, 2, 2, 0, 0, 0, 0, 1, 1, 1, 1,
                };

                TileMap tileMap = new TileMap();
                if(!tileMap.load("assets\\graphics-vertex-array-tilemap-tileset.png",new Vector2u(32,32),level,16,8))
                return ;
                
                while(window.IsOpen)
                {
                    window.DispatchEvents();
                    if(Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                        window.Close();

                    window.Clear();
                    window.Draw(tileMap);
                    window.Display();

                }
            }
        }


    }
}