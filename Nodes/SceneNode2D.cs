using SFML.Graphics;
using System.Collections.Generic;

namespace ConsoleApp2.Nodes
{
    public class SceneNode2D : Drawable
    {
        public string name;
        public long id;
        public Node2D entity;

        public List<SceneNode2D> children;
        public SceneNode2D() { }

        public SceneNode2D(string name)
        {
            this.name = name;
        }

        public SceneNode2D(string name, long id) : this(name)
        {
            this.id = id;
        }

        public SceneNode2D(string name, long id, Node2D entity) : this(name, id)
        {
            this.entity = entity;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            entity.Draw(target, states);
            if (children != null && children.Count > 0)
            {
                foreach (var next in children)
                {
                    next.Draw(target, states);
                }
            }
        }
    }
}
