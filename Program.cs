using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleApp2
{

    // Для A* нужен только WeightedGraph и тип точек L, и карта *не*
    // обязана быть сеткой. Однако в коде примера я использую сетку.
    public interface WeightedGraph<L>
    {
        double Cost(Location a, Location b);
        IEnumerable<Location> Neighbors(Location id);
    }


    public class Location
    {
        // Примечания по реализации: я использую Equals по умолчанию,
        // но это может быть медленно. Возможно, в реальном проекте стоит
        // заменить Equals и GetHashCode.
        public long id;
        public string name;

        public readonly int x, y;
        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;
            name = x.ToString() + "_" + y.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is Location location &&
                   x == location.x &&
                   y == location.y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public static bool operator ==(Location lhs, Location rhs)
        {
            bool status = false;
            if (lhs.x == rhs.x && lhs.y == rhs.y)
            {

                status = true;
            }
            return status;
        }
        public static bool operator !=(Location lhs, Location rhs)
        {

            return !(lhs == rhs);
        }
    }

    public class SquareGrid : WeightedGraph<Location>
    {
        // Примечания по реализации: для удобства я сделал поля публичными,
        // но в реальном проекте, возможно, стоит следовать стандартному
        // стилю и сделать их скрытыми.

        public static readonly Location[] DIRS = new[]
            {
            new Location(1, 0),
            new Location(0, -1),
            new Location(-1, 0),
            new Location(0, 1)
        };

        public int width, height;
        public HashSet<Location> walls = new HashSet<Location>();
        public HashSet<Location> forests = new HashSet<Location>();

        public SquareGrid(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public bool InBounds(Location id)
        {
            return 0 <= id.x && id.x < width
                && 0 <= id.y && id.y < height;
        }

        public bool Passable(Location id)
        {
            return !walls.Contains(id);
        }

        public double Cost(Location a, Location b)
        {
            return forests.Contains(b) ? 5 : 1;
        }

        public IEnumerable<Location> Neighbors(Location id)
        {
            foreach (var dir in DIRS)
            {
                Location next = new Location(id.x + dir.x, id.y + dir.y);
                if (InBounds(next) && Passable(next))
                {
                    yield return next;
                }
            }
        }
    }


    public class PriorityQueue<T>
    {
        // В этом примере я использую несортированный массив, но в идеале
        // это должна быть двоичная куча. Существует открытый запрос на добавление
        // двоичной кучи к стандартной библиотеке C#: https://github.com/dotnet/corefx/issues/574
        //
        // Но пока её там нет, можно использовать класс двоичной кучи:
        // * https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp
        // * http://visualstudiomagazine.com/articles/2012/11/01/priority-queues-with-c.aspx
        // * http://xfleury.github.io/graphsearch.html
        // * http://stackoverflow.com/questions/102398/priority-queue-in-net

        private List<Tuple<T, double>> elements = new List<Tuple<T, double>>();

        public int Count
        {
            get { return elements.Count; }
        }

        public void Enqueue(T item, double priority)
        {
            elements.Add(Tuple.Create(item, priority));
        }

        public T Dequeue()
        {
            int bestIndex = 0;

            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].Item2 < elements[bestIndex].Item2)
                {
                    bestIndex = i;
                }
            }

            T bestItem = elements[bestIndex].Item1;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }
    }


    /* Примечание о типах: в предыдущей статье в коде Python я использовал
     * для стоимости, эвристики и приоритетов просто числа. В коде C++
     * я использую для этого typedef, потому что может потребоваться int, double или
     * другой тип. В этом коде на C# я использую для стоимости, эвристики
     * и приоритетов double. Можно использовать int, если вы уверены, что значения
     * никогда не будут больше, или числа меньшего размера, если знаете, что
     * значения всегда будут небольшими. */

    public class AStarSearch
    {
        public Location start;
        public Location end;

        public Dictionary<Location, Location> cameFrom
            = new Dictionary<Location, Location>();
        public Dictionary<Location, double> costSoFar
            = new Dictionary<Location, double>();

        // Примечание: обобщённая версия A* абстрагируется от Location
        // и Heuristic
        static public double Heuristic(Location a, Location b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
            //return (a.x - b.x)* (a.x - b.x) + (a.y - b.y)* (a.y - b.y);
        }

        public AStarSearch(WeightedGraph<Location> graph, Location start, Location goal)
        {
            this.start = start;
            this.end = goal;
            var frontier = new PriorityQueue<Location>();
            frontier.Enqueue(start, 0);

            cameFrom[start] = start;
            costSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (current.Equals(goal))
                {
                    break;
                }

                foreach (var next in graph.Neighbors(current))
                {
                    double newCost = costSoFar[current]
                        + graph.Cost(current, next);
                    if (!costSoFar.ContainsKey(next)
                        || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        double priority = newCost + Heuristic(next, goal);
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }
        }
    }


    public interface Node2D : Drawable
    {
        public string getName();
        public void setName(string name);
        public Vector2f getPosition();
        public void setPosition(Vector2f position);

    }
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
            if ((children != null) && (children.Count > 0))
            {
                foreach (var next in children)
                {
                    next.Draw(target, states);
                }
            }
        }
    }

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
                                rectangleStartStop.FillColor = Color.Red;

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
                        else if (ptr.x == x + 1)
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
            var astar = new AStarSearch(grid, new Location(1, 4),
                                        new Location(8, 5));


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
