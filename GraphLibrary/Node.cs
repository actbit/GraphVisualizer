using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace GraphLibrary
{
    public class Node
    {
        public static Action<Node>? ColorChange = null;
        public static Action<Node>? CreateNode = null;
        public static Action<Node>? DeleteNode = null;
        internal static List<Edge> edges = new List<Edge>();
        public static IReadOnlyList<Edge> Edges
        {
            get { return edges; }
        }
        public string id {  get; private set; }
        public string title { get; private set; }
        string _color;
        public string color { 
            get
            { 
                return _color;
            }
            set 
            {
                _color = value;
                ColorChange?.Invoke(this);  
            } 
        }
        public bool IsVisited { get; private set; } = false;
        public bool IsDirected { get; set; }
        internal static List<Node> nodes = new List<Node>();
        
        public static IReadOnlyList<Node> Nodes
        {
            get { return nodes; }
        }
        public IReadOnlyList<ToEdge> ToEdges
        {
            get
            {
                var sourceThis =  edges.Where(x=>x.Source == this).Select(x=>new ToEdge(x,true));
                var targetThis = Enumerable.Empty<ToEdge>();
                if (!IsDirected)
                {
                    targetThis = edges.Where(x => x.Target == this).Select(x=>new ToEdge(x,false));

                }
                return sourceThis.Concat(targetThis).ToList();
            }
        }
        
        internal Node(string id,string title = "")
        {
            this.id = id;
            this.title = title;
            
        }
        internal Node(string id,string color, string title = "")
        {
            this.id = id;
            this.title = title;
            this.color = color;
        }
        public static Node Create(string id, string title = "")
        {

            var node = new Node(id, title);
            nodes.Add(node);
            CreateNode?.Invoke(node);
            return node;
        }

        public static Node Create(string id,string color, string title = "")
        {
            var node = new Node(id, color, title);
            nodes.Add(node );
            CreateNode?.Invoke(node);

            return node;
        }
      



        public void NewTo(Node node,int weight)
        {
            edges.Add(new Edge(this, node, weight));
        }

        public void Delete()
        {
            var deleteEdges = edges.Where(x => x.Source == this).Concat(edges.Where(x => x.Target == this));
            foreach(var edge in deleteEdges)
            {
                edges.Remove(edge);
            }
        }
        public InternalNode ToInternalNode()
        {
            return new InternalNode
            {
                id = this.id,
                title = this.title,
                color = this._color
            };
        }
    }

    public class InternalNode
    {
        public string id { get; set; }
        public string title { get; set; }
        public string color { get; set; }
    }

    public class ToEdge
    {
        public static Action<Edge>? ColorChange;
        public static Action<Edge>? WeightChange;
        public static Action<Edge>? ToNodeChange;

        public static Action<Edge>? DeleteChange;

        bool IsTarget = false;
        internal ToEdge(Edge edge, bool isTarget = true)
        {
            Edge = edge;
            IsTarget = isTarget;
        }
        internal Edge Edge;
        public void Delete()
        {
            Node.edges.Remove(Edge);
            DeleteChange?.Invoke(Edge);
        }
        public string? Color
        {
            get
            {
                return Edge.Color;
            }
            set
            {
                Edge.Color = value;
                ColorChange?.Invoke(Edge);
            }
        }
        public int? Weight
        {
            get
            {
                return Edge.Weight;
            }
            set
            {
                Edge.Weight = value;
                WeightChange?.Invoke(Edge);
            }
        }
        public Node ToNode { 
            get
            {
                if (IsTarget)
                {
                    return Edge.Target;
                }
                return Edge.Source;
            }
            set
            {
                if (IsTarget)
                {
                    Edge.Target = value;
                }
                else
                {
                    Edge.Source = value;
                }
                ToNodeChange?.Invoke(Edge);
            }
        }

        public InternalEdge ToInternalEdge()
        {
            string wstr = "";
            if (!(Weight == null || Weight < 0))
            {
                wstr = Weight.Value.ToString();
            }
            return new InternalEdge
            {
                id = Edge.ID,
                source = Edge.Source.id,
                target = Edge.Target.id,
                weight = wstr,
                color = this.Color
            };

        }
    }
}
