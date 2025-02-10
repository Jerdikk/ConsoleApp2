using SFML.Graphics;
using SFML.System;

namespace ConsoleApp2.Nodes
{
    public class SpriteNode : Node2D
    {
        //   Texture? texture;
        IntRect textureRext;
        Sprite? sprite;
      //  string path;
        string name;

        public SpriteNode(Sprite sprite, IntRect textureRext)
        {
            this.sprite = sprite;
            this.textureRext = textureRext;
        }
        /*  public SpriteNode(string path, string name)
          {
              this.path = path;
              this.setName(name);
              texture = new Texture(path);
              sprite = new Sprite(texture);

          }*/

        public void Draw(RenderTarget target, RenderStates states)
        {
            sprite?.Draw(target, states);
        }

        public string getName()
        {
            return name;
        }

        public Vector2f? getPosition()
        {
            return sprite?.Position;
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public void setPosition(Vector2f position)
        {
            sprite.Position = position;
        }

        Vector2f Node2D.getPosition()
        {
            return sprite.Position;
        }
    }
}
