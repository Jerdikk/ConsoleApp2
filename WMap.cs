using SFML.Graphics;
using SFML.System;
//using System.Drawing;
using System.IO;


namespace ConsoleApp2
{
    public class WMap
    {
        Image mapTileImage;
        Texture texture1;
        Sprite mapSprite;

        RenderTexture rTexture;

        int NumTilesX;
        int NumTilesY;
        int CurrentPositionOnTileMapLeft;
        int CurrentPositionOnTileMapTop;
        int TileMapWindowsWidth;
        int TileMapWindowsHeight;
        Vector2i CursorPositionOnMap;

        //описание картинок на карте
        byte[,] TileMap = new byte[256, 256]; //tmap

        public WMap(uint width, uint height)
        {
            mapTileImage = new Image("NEW_DARK.BPX1.jpg");
            LoadMap("map13h.mpf");
            mapSprite = new Sprite();
            texture1 = new Texture(mapTileImage);
            mapSprite.Texture = texture1;
            rTexture = new RenderTexture(width, height);
            TileMapWindowsWidth = (int)(rTexture.Texture.Size.X / 32);
            TileMapWindowsHeight = (int)(rTexture.Texture.Size.Y / 32);
            CurrentPositionOnTileMapLeft = 0;
            CurrentPositionOnTileMapTop = 0;
            CursorPositionOnMap = new Vector2i(0, 0);
        }
        public Vector2i GetCursorPositionOnMap(float x, float y)
        {
            CursorPositionOnMap.X = (int)(x / 32.0f + CurrentPositionOnTileMapLeft);
            CursorPositionOnMap.Y = (int)(y / 32.0f + CurrentPositionOnTileMapTop);
            return CursorPositionOnMap;
        }
        public int LoadMap(string s)
        {
            try
            {
                byte[] data = File.ReadAllBytes(s);

                using (MemoryStream m = new MemoryStream(data))
                {
                    using (BinaryReader reader = new BinaryReader(m))
                    {
                        int t1 = reader.ReadInt32();
                        if (t1 == 0x1B46504D)
                        {
                            NumTilesX = reader.ReadInt32();
                            NumTilesY = reader.ReadInt32();
                            int t2 = reader.ReadInt32();
                            int t3 = reader.ReadInt32();
                            for (int i = 0; i < NumTilesY; i++)
                            {
                                for (int j = 0; j < NumTilesX; j++)
                                {
                                    TileMap[i, j] = reader.ReadByte();
                                }
                            }
                        }
                    }
                }
                return 1;
            }
            catch { return -1; }
        }
        public int DrawMap()
        {
            try
            {
                int X = CurrentPositionOnTileMapLeft;
                int Y = CurrentPositionOnTileMapTop;
                if (TileMapWindowsWidth * TileMapWindowsHeight == 0)
                    return 0;
                Vector2i position = new Vector2i(0, 0);
                Vector2i size = new Vector2i(32, 32);
                IntRect r2 = new IntRect(position, size);

                int NumTile;
                rTexture.Clear();
                int numtilesx = TileMapWindowsWidth;
                int numtilesy = TileMapWindowsHeight;
                for (int i = 0; i < numtilesx; i++)
                    for (int j = 0; j < numtilesy; j++)
                    {
                        try //if (i * j < 250)
                        {
                            NumTile = TileMap[i + X, j + Y];
                            r2.Top = NumTile * 32;// +j * 32 * numtilesx;
                            mapSprite.TextureRect = r2;
                            mapSprite.Position = new Vector2f(0 + i * 32, 0 + j * 32);
                            rTexture.Draw(mapSprite);
                        }
                        catch { }
                    }
                rTexture.Display();
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        public RenderTexture GetMapRenderTexture()
        {
            return rTexture;
        }

        public int MapLeft(int delta)
        {
            try
            {
                CurrentPositionOnTileMapLeft += delta;
                if (CurrentPositionOnTileMapLeft <= 0)
                    CurrentPositionOnTileMapLeft = 0;
                if (CurrentPositionOnTileMapLeft > (NumTilesX - TileMapWindowsWidth))
                    CurrentPositionOnTileMapLeft = NumTilesX - TileMapWindowsWidth;

                return 1;
            }
            catch
            {
                return -1;
            }
        }
        public int MapUp(int delta)
        {
            try
            {
                CurrentPositionOnTileMapTop += delta;
                if (CurrentPositionOnTileMapTop <= 0)
                    CurrentPositionOnTileMapTop = 0;

                if (CurrentPositionOnTileMapTop > (NumTilesY - TileMapWindowsHeight))
                    CurrentPositionOnTileMapTop = (NumTilesY - TileMapWindowsHeight);

                return 1;
            }
            catch
            {
                return -1;
            }
        }
    }
}