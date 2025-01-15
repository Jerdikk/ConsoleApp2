using SFML.Graphics;
using SFML.System;

namespace ConsoleApp2.Nodes
{
    public class RectangleNode : Node2D
    {
        RectangleShape shape;
        string name;

        public RectangleNode(RectangleShape shape)
        {
            this.shape = shape;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            shape.Draw(target, states);
        }

        public string getName()
        {
            return name;
        }

        public Vector2f getPosition()
        {
            return shape.Position;
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public void setPosition(Vector2f position)
        {
            shape.Position = position;
        }
    }
}
