using SFML.Graphics;
using SFML.System;

namespace ConsoleApp2.Nodes
{
    public interface Node2D : Drawable
    {
        public string getName();
        public void setName(string name);
        public Vector2f getPosition();
        public void setPosition(Vector2f position);

    }
}
