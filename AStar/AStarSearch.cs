using System;
using System.Collections.Generic;

namespace ConsoleApp2.AStar
{
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
        public WeightedGraph<Location> graph;

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

        public AStarSearch()
        {
        }

        public void FindPath()//WeightedGraph<Location> graph, Location start, Location goal)
        {
            //this.start = start;
            Location goal = end;
            //this.end = goal;
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

            int gg = 1;
        
        }
    }
}
