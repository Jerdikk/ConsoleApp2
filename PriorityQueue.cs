using System;
using System.Collections.Generic;

namespace ConsoleApp2
{
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
}
