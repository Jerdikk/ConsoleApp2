using System.Collections.Generic;

namespace ConsoleApp2
{
    // Для A* нужен только WeightedGraph и тип точек L, и карта *не*
    // обязана быть сеткой. Однако в коде примера я использую сетку.
    public interface WeightedGraph<L>
    {
        double Cost(Location a, Location b);
        IEnumerable<Location> Neighbors(Location id);
    }
}
