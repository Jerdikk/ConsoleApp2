using System;

namespace ConsoleApp2
{
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
}
