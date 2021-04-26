using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// Класс являющийся представлением графа истории, где узлы являются состояниями мира, а грани - действиями.
    /// </summary>
    class StoryGraph
    {
        // Ссылка на корневой узел.
        private StoryNode startNode;

        // Список узлов.
        private HashSet<StoryNode> nodes;

        // Список граней.
        private HashSet<Edge> edges;

        /// <summary>
        /// Метод-конструктор для графа истории, без параметров.
        /// </summary>
        public StoryGraph()
        {
            startNode = new StoryNode();
            nodes = new HashSet<StoryNode>();
            edges = new HashSet<Edge>();
        }

        /// <summary>
        /// Добавляет узел в список узлов графа истории.
        /// </summary>
        /// <param name="newNode"></param>
        public void AddNode(StoryNode newNode)
        {
            nodes.Add(newNode);
        }

        /// <summary>
        /// Добавляет грань в список граней графа истории.
        /// </summary>
        /// <param name="newEdge"></param>
        public void AddEdge(Edge newEdge)
        {
            edges.Add(newEdge);
        }

        // TODO
        public void ExpandNode(StoryworldConvergence storyworldConvergence)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Возвращает список узлов графа истории.
        /// </summary>
        public HashSet<StoryNode> GetNodes()
        {
            return nodes;
        }

        /// <summary>
        /// Возвращает корневой узел графа истории.
        /// </summary>
        public StoryNode GetRoot()
        {
            return startNode;
        }

        /// <summary>
        /// Возвращает последний узел из списка узлов графа истории.
        /// </summary>
        public StoryNode GetLastNode()
        {
            return nodes.Last();
        }

        public StoryNode GetNode(int index)
        {
            return nodes.ElementAt(index);
        }
    }
}
