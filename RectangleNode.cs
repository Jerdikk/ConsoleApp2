using SFML.Graphics;
using SFML.System;

namespace ConsoleApp2
{
    public class SpriteNode : Node2D
    {
        Sprite sprite;
        string name;
        
        public SpriteNode(Texture texture)
        {
            sprite = new Sprite(texture);
        }

        public SpriteNode(RenderTexture renderTexture) 
        {
            sprite = new Sprite(renderTexture.Texture);
        }

        public SpriteNode(Texture texture, IntRect intRect)
        {
            sprite = new Sprite(texture, intRect);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            sprite.Draw(target, states);
        }

        public string getName()
        {
            return name;
        }

        public Vector2f getPosition()
        {
            return sprite.Position;
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public void setPosition(Vector2f position)
        {
            sprite.Position = position;
        }
    }
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
