using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleApp2
{

    class Program
    {
        static float CellSize = 32.0f;
        static Scene2D scene;
        static Font font;
        static void Main(string[] args)
        {
            scene = new Scene2D();
            scene.name = "Main";

            scene.Nodes = new List<SceneNode2D>();


            static void DrawGrid(SquareGrid grid, AStarSearch astar)
            {
                Vector2f position = new Vector2f(0.0f, 0.0f);

                for (var y = 0; y < 10; y++)
                {
                    for (var x = 0; x < 10; x++)
                    {
                        Location testLocation = new Location(x, y);
                        Location ptr = testLocation;
                        if (!astar.cameFrom.TryGetValue(testLocation, out ptr))
                        {
                            ptr = testLocation;
                        }

                        if (grid.walls.Contains(testLocation))
                        {
                            SceneNode2D sceneNode2D = scene.GetSceneNode(testLocation.name);
                            if (sceneNode2D == null)
                            {
                                Vector2f position1 = new Vector2f(0.0f, 0.0f);
                                RectangleShape rectangleShapeWall = new RectangleShape(new Vector2f(CellSize, CellSize));
                                rectangleShapeWall.FillColor = Color.Cyan;

                                sceneNode2D = new SceneNode2D();
                                Node2D node2D = new RectangleNode(rectangleShapeWall);
                                node2D.setName("wall" + testLocation.name);
                                sceneNode2D.entity = node2D;
                                sceneNode2D.name = testLocation.name;
                                position.X = testLocation.x * CellSize;
                                position.Y = testLocation.y * CellSize;
                                sceneNode2D.entity.setPosition(position);

                                scene.Nodes.Add(sceneNode2D);
                            }
                            else
                            {
                                position.X = testLocation.x * CellSize;
                                position.Y = testLocation.y * CellSize;
                                sceneNode2D.entity.setPosition(position);

                            }
                            // rectangleShapeWall.Draw(renderTexture, renderStates);
                            //  Console.Write("##");
                        }
                        else if (grid.forests.Contains(testLocation))
                        {
                            SceneNode2D sceneNode2D = scene.GetSceneNode(testLocation.name);
                            if (sceneNode2D == null)
                            {
                                Vector2f position1 = new Vector2f(0.0f, 0.0f);
                                RectangleShape rectangleShapeWall = new RectangleShape(new Vector2f(CellSize, CellSize));
                                rectangleShapeWall.FillColor = Color.Green;

                                sceneNode2D = new SceneNode2D();
                                Node2D node2D = new RectangleNode(rectangleShapeWall);
                                node2D.setName("forest" + testLocation.name);
                                sceneNode2D.entity = node2D;
                                sceneNode2D.name = testLocation.name;
                                position.X = testLocation.x * CellSize;
                                position.Y = testLocation.y * CellSize;
                                sceneNode2D.entity.setPosition(position);

                                scene.Nodes.Add(sceneNode2D);
                            }
                            else
                            {
                                position.X = testLocation.x * CellSize;
                                position.Y = testLocation.y * CellSize;
                                sceneNode2D.entity.setPosition(position);

                            }

                        }

                        if ((astar.start == testLocation) || (astar.end == testLocation))
                        {

                            SceneNode2D sceneNode2D = scene.GetSceneNode(testLocation.name);
                            if (sceneNode2D == null)
                            {
                                Vector2f position2 = new Vector2f(0.0f, 0.0f);
                                RectangleShape rectangleStartStop = new RectangleShape(new Vector2f(CellSize, CellSize));
                                rectangleStartStop.FillColor = Color.White;

                                sceneNode2D = new SceneNode2D();
                                Node2D node2D = new RectangleNode(rectangleStartStop);
                                if (astar.start == testLocation)
                                    node2D.setName("start" + testLocation.name);
                                else
                                    node2D.setName("end" + testLocation.name);
                                sceneNode2D.entity = node2D;
                                sceneNode2D.name = testLocation.name;
                                position.X = testLocation.x * CellSize;
                                position.Y = testLocation.y * CellSize;
                                sceneNode2D.entity.setPosition(position);

                                scene.Nodes.Add(sceneNode2D);

                            }
                            else
                            {
                                position.X = testLocation.x * CellSize;
                                position.Y = testLocation.y * CellSize;
                                sceneNode2D.entity.setPosition(position);

                            }


                            //textt.Draw(renderTexture, renderStates);
                        }

                        if (ptr.x == x + 1)
                        {
                            // Console.Write("R ");
                            SceneNode2D sceneNode2D = scene.GetSceneNode(testLocation.name);
                            if (sceneNode2D == null)
                            {
                                Vector2f position2 = new Vector2f(0.0f, 0.0f);
                                Text textt = new Text("R", font);
                                textt.FillColor = Color.Red;
                                textt.CharacterSize = 20;

                                sceneNode2D = new SceneNode2D();
                                Node2D node2D = new TextNode(textt);
                                node2D.setName("right" + testLocation.name);
                                sceneNode2D.entity = node2D;
                                sceneNode2D.name = testLocation.name;
                                position.X = testLocation.x * CellSize;
                                position.Y = testLocation.y * CellSize;
                                sceneNode2D.entity.setPosition(position);

                                scene.Nodes.Add(sceneNode2D);

                            }
                            else
                            {
                                if (sceneNode2D.entity.getName().Contains("right"))
                                {
                                    position.X = testLocation.x * CellSize;
                                    position.Y = testLocation.y * CellSize;
                                    sceneNode2D.entity.setPosition(position);
                                }
                                else
                                {
                                    if (sceneNode2D.children == null)
                                    {
                                        Vector2f position2 = new Vector2f(0.0f, 0.0f);
                                        Text textt = new Text("R", font);
                                        textt.FillColor = Color.Red;
                                        textt.CharacterSize = 20;

                                        SceneNode2D sceneNode2D1 = new SceneNode2D();
                                        Node2D node2D = new TextNode(textt);
                                        node2D.setName("right" + testLocation.name);
                                        sceneNode2D1.entity = node2D;
                                        sceneNode2D1.name = testLocation.name;
                                        position.X = testLocation.x * CellSize;
                                        position.Y = testLocation.y * CellSize;
                                        sceneNode2D1.entity.setPosition(position);

                                        sceneNode2D.children = new List<SceneNode2D>();
                                        sceneNode2D.children.Add(sceneNode2D1);
                                    }
                                }

                            }
                        }
                        else if (ptr.x == x - 1)
                        {
                            //   Console.Write("L ");
                            SceneNode2D sceneNode2D = scene.GetSceneNode(testLocation.name);
                            if (sceneNode2D == null)
                            {
                                Vector2f position2 = new Vector2f(0.0f, 0.0f);
                                Text textt = new Text("L", font);
                                textt.FillColor = Color.Red;
                                textt.CharacterSize = 20;

                                sceneNode2D = new SceneNode2D();
                                Node2D node2D = new TextNode(textt);
                                node2D.setName("left" + testLocation.name);
                                sceneNode2D.entity = node2D;
                                sceneNode2D.name = testLocation.name;
                                position.X = testLocation.x * CellSize;
                                position.Y = testLocation.y * CellSize;
                                sceneNode2D.entity.setPosition(position);

                                scene.Nodes.Add(sceneNode2D);

                            }
                            else
                            {
                                if (sceneNode2D.entity.getName().Contains("left"))
                                {
                                    position.X = testLocation.x * CellSize;
                                    position.Y = testLocation.y * CellSize;
                                    sceneNode2D.entity.setPosition(position);
                                }
                                else
                                {
                                    if (sceneNode2D.children == null)
                                    {
                                        Vector2f position2 = new Vector2f(0.0f, 0.0f);
                                        Text textt = new Text("L", font);
                                        textt.FillColor = Color.Red;
                                        textt.CharacterSize = 20;

                                        SceneNode2D sceneNode2D1 = new SceneNode2D();
                                        Node2D node2D = new TextNode(textt);
                                        node2D.setName("left" + testLocation.name);
                                        sceneNode2D1.entity = node2D;
                                        sceneNode2D1.name = testLocation.name;
                                        position.X = testLocation.x * CellSize;
                                        position.Y = testLocation.y * CellSize;
                                        sceneNode2D1.entity.setPosition(position);

                                        sceneNode2D.children = new List<SceneNode2D>();
                                        sceneNode2D.children.Add(sceneNode2D1);
                                    }
                                }
                            }
                        }
                        else if (ptr.y == y + 1)
                        {
                            //  Console.Write("D ");
                            SceneNode2D sceneNode2D = scene.GetSceneNode(testLocation.name);
                            if (sceneNode2D == null)
                            {
                                Vector2f position2 = new Vector2f(0.0f, 0.0f);
                                Text textt = new Text("D", font);
                                textt.FillColor = Color.Red;
                                textt.CharacterSize = 20;

                                sceneNode2D = new SceneNode2D();
                                Node2D node2D = new TextNode(textt);
                                node2D.setName("down" + testLocation.name);
                                sceneNode2D.entity = node2D;
                                sceneNode2D.name = testLocation.name;
                                position.X = testLocation.x * CellSize;
                                position.Y = testLocation.y * CellSize;
                                sceneNode2D.entity.setPosition(position);

                                scene.Nodes.Add(sceneNode2D);

                            }
                            else
                            {
                                if (sceneNode2D.entity.getName().Contains("down"))
                                {
                                    position.X = testLocation.x * CellSize;
                                    position.Y = testLocation.y * CellSize;
                                    sceneNode2D.entity.setPosition(position);
                                }
                                else
                                {
                                    if (sceneNode2D.children == null)
                                    {
                                        Vector2f position2 = new Vector2f(0.0f, 0.0f);
                                        Text textt = new Text("D", font);
                                        textt.FillColor = Color.Red;
                                        textt.CharacterSize = 20;

                                        SceneNode2D sceneNode2D1 = new SceneNode2D();
                                        Node2D node2D = new TextNode(textt);
                                        node2D.setName("down" + testLocation.name);
                                        sceneNode2D1.entity = node2D;
                                        sceneNode2D1.name = testLocation.name;
                                        position.X = testLocation.x * CellSize;
                                        position.Y = testLocation.y * CellSize;
                                        sceneNode2D1.entity.setPosition(position);

                                        sceneNode2D.children = new List<SceneNode2D>();
                                        sceneNode2D.children.Add(sceneNode2D1);
                                    }
                                }

                            }
                        }
                        else if (ptr.y == y - 1)
                        {
                            SceneNode2D sceneNode2D = scene.GetSceneNode(testLocation.name);
                            if (sceneNode2D == null)
                            {
                                Vector2f position2 = new Vector2f(0.0f, 0.0f);
                                Text textt = new Text("U", font);
                                textt.FillColor = Color.Red;
                                textt.CharacterSize = 20;

                                sceneNode2D = new SceneNode2D();
                                Node2D node2D = new TextNode(textt);
                                node2D.setName("up" + testLocation.name);
                                sceneNode2D.entity = node2D;
                                sceneNode2D.name = testLocation.name;
                                position.X = testLocation.x * CellSize;
                                position.Y = testLocation.y * CellSize;
                                sceneNode2D.entity.setPosition(position);

                                scene.Nodes.Add(sceneNode2D);

                            }
                            else
                            {

                                if (sceneNode2D.entity.getName().Contains("up"))
                                {
                                    position.X = testLocation.x * CellSize;
                                    position.Y = testLocation.y * CellSize;
                                    sceneNode2D.entity.setPosition(position);
                                }
                                else
                                {
                                    if (sceneNode2D.children == null)
                                    {
                                        Vector2f position2 = new Vector2f(0.0f, 0.0f);
                                        Text textt = new Text("U", font);
                                        textt.FillColor = Color.Red;
                                        textt.CharacterSize = 20;

                                        SceneNode2D sceneNode2D1 = new SceneNode2D();
                                        Node2D node2D = new TextNode(textt);
                                        node2D.setName("up" + testLocation.name);
                                        sceneNode2D1.entity = node2D;
                                        sceneNode2D1.name = testLocation.name;
                                        position.X = testLocation.x * CellSize;
                                        position.Y = testLocation.y * CellSize;
                                        sceneNode2D1.entity.setPosition(position);

                                        sceneNode2D.children = new List<SceneNode2D>();
                                        sceneNode2D.children.Add(sceneNode2D1);
                                    }
                                }
                            }
                        }
                        else
                        {
                            SceneNode2D sceneNode2D = scene.GetSceneNode(testLocation.name);
                            if (sceneNode2D == null)
                            {
                                Vector2f position2 = new Vector2f(0.0f, 0.0f);
                                RectangleShape rectangleShapeBlue = new RectangleShape(new Vector2f(CellSize, CellSize));
                                rectangleShapeBlue.FillColor = Color.Blue;


                                sceneNode2D = new SceneNode2D();
                                Node2D node2D = new RectangleNode(rectangleShapeBlue);
                                sceneNode2D.entity = node2D;
                                sceneNode2D.name = testLocation.name;
                                position.X = testLocation.x * CellSize;
                                position.Y = testLocation.y * CellSize;
                                sceneNode2D.entity.setPosition(position);

                                scene.Nodes.Add(sceneNode2D);

                            }
                            else
                            {
                                position.X = testLocation.x * CellSize;
                                position.Y = testLocation.y * CellSize;
                                sceneNode2D.entity.setPosition(position);

                            }
                            // RectangleShape rectangleShapeBlue = new RectangleShape(new Vector2f(CellSize, CellSize));
                            //   rectangleShapeBlue.FillColor = Color.Blue;


                            // position.X = testLocation.x * CellSize;
                            // position.Y = testLocation.y * CellSize;
                            //  rectangleShapeBlue.Position = position;
                            //rectangleShapeBlue.Draw(renderTexture, renderStates);
                            //Console.Write("* ");
                        }
                    }
                    // Console.WriteLine();
                }
            }

            VideoMode mode = new VideoMode(800, 600);
            RenderWindow window = new RenderWindow(mode, "SFML.NET");

            window.Closed += (obj, e) => { window.Close(); };
            window.KeyPressed +=
                (sender, e) =>
                {
                    Window window = (Window)sender;
                    if (e.Code == Keyboard.Key.Escape)
                    {
                        window.Close();
                    }
                };

            font = new Font("C:/Windows/Fonts/arial.ttf");
            Text text = new Text("Приветмир!", font);
            text.CharacterSize = 40;
            float textWidth = text.GetLocalBounds().Width;
            float textHeight = text.GetLocalBounds().Height;
            float xOffset = text.GetLocalBounds().Left;
            float yOffset = text.GetLocalBounds().Top;
            text.Origin = new Vector2f(textWidth / 2f + xOffset, textHeight / 2f + yOffset);
            text.Position = new Vector2f(window.Size.X / 2f, window.Size.Y / 2f);

            RenderTexture renderTexture = new RenderTexture(800, 600);

            RenderStates renderStates = RenderStates.Default;

            Clock clock = new Clock();
            int currentTime = 0;
            int prevTime = 0;
            //  float angle = 0f;
            //  float angleSpeed = 90f;

            Sprite sprite;

            var grid = new SquareGrid(10, 10);
            for (var x = 3; x < 5; x++)
            {
                for (var y = 3; y < 7; y++)
                {
                    grid.walls.Add(new Location(x, y));
                }
            }

            for (int x = 0; x < 10; x++)
            {
                grid.walls.Add(new Location(x, 0));
                grid.walls.Add(new Location(x, 9));
                grid.walls.Add(new Location(0, x));
                grid.walls.Add(new Location(9, x));
            }

            grid.forests = new HashSet<Location>
            {
                new Location(3, 4), new Location(3, 5),
                new Location(4, 1), new Location(4, 2),
                new Location(4, 3), new Location(4, 4),
                new Location(4, 5), new Location(4, 6),
                new Location(4, 7), new Location(4, 8),
                new Location(5, 1), new Location(5, 2),
               // new Location(5, 3), new Location(5, 4),
                new Location(5, 5), new Location(5, 6),
              //  new Location(5, 7), new Location(5, 8),
                new Location(6, 2), new Location(6, 3),
                new Location(6, 4), new Location(6, 5),
                new Location(6, 6), new Location(6, 7),
                new Location(7, 3), new Location(7, 4),
                new Location(7, 5)
            };

            // Выполнение A*
            AStarSearch astar = new AStarSearch();

            astar.start = new Location(1, 4);
            astar.end = new Location(8, 5);
            astar.graph = grid;

            astar.FindPath();

            sprite = new Sprite();

            while (window.IsOpen)
            {
                window.DispatchEvents();

                currentTime = clock.ElapsedTime.AsMilliseconds();

                if ((currentTime - prevTime) >= 17)
                {
                    prevTime = currentTime;
                }
                else
                {
                    Thread.Sleep(1);
                    continue;
                }


                renderTexture.Clear();

                DrawGrid(grid, astar);

                scene.Draw(renderTexture, renderStates);

                renderTexture.Display();


                sprite.Texture = renderTexture.Texture;

                //currentTime = clock.Restart().AsSeconds();
                // angle += angleSpeed * currentTime;
                //window.Clear();
                //window.Draw(rectangleShapeBlue);
                // text.Rotation = angle;
                // window.Draw(text);

                window.Draw(sprite);
                window.Display();

            }
            window.Close();
            window.Dispose();
            renderTexture.Dispose();
            //   rectangleShapeBlue.Dispose();
            sprite.Dispose();
            text.Dispose();
            clock.Dispose();
        }
    }
}
