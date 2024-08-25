using SFML.Graphics;
using System.Collections.Generic;

namespace ConsoleApp2
{
    public class Scene2D : Drawable
    {
        public string name;
        public long currentMaxID;

        public List<SceneNode2D> Nodes;
        public Scene2D()
        {
            currentMaxID = 0;
        }

        public SceneNode2D GetSceneNode(int id)
        {
            SceneNode2D result = null;
            foreach (var node in Nodes)
            {
                if (node.id == id)
                    result = node;
            }
            return result;
        }

        public SceneNode2D GetSceneNode(string name)
        {
            SceneNode2D result = null;
            foreach (var node in Nodes)
            {
                if (node.name.Equals(name))
                    result = node;
            }
            return result;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (var node in Nodes)
            {
                node.Draw(target, states);
            }
        }
    }
}
