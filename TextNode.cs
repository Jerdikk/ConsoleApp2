using SFML.Graphics;
using SFML.System;

namespace ConsoleApp2
{
    public class TextNode : Node2D
    {
        Text text1;
        string name;

        public TextNode(Text text1)
        {
            this.text1 = text1;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            text1?.Draw(target, states);
        }

        public string getName()
        {
            return name;
        }

        public Vector2f getPosition()
        {
            return text1.Position;
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public void setPosition(Vector2f position)
        {
            text1.Position = position;
        }
    }
}
