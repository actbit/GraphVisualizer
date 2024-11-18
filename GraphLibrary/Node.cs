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
        public static Action<Node>? TitleChange = null;
        public static Action<Node>? CreateNode = null;
        public static Action<Node>? DeleteNode = null;
        public static Action<Edge>? CreateEdge =null;
        public static Action<bool>? DirectedChange = null;

        internal static List<Edge> edges = new List<Edge>();
        public static IReadOnlyList<Edge> Edges
        {
            get { return edges; }
        }

        public static bool ContainsID(string id)
        {
            var node = nodes.Where(x => x.ID == id).FirstOrDefault();
            return node != null;
        }
        public int State{ get; set;}
        public string ID {  get; private set; }
        public string _title;
        public string Title { get { return _title; } set { _title =value;TitleChange?.Invoke(this); } }
        string _color = "#666";
        public string Color { 
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
        public bool IsVisited { get; set; } = false;
        static bool _IsDirected = false;
        public static bool IsDirected
        {
            get
            {
                return _IsDirected;
            }
            set
            {
                _IsDirected = value;
                DirectedChange?.Invoke(value);
            }
        }

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
        public void VisitedColorChange(string color)
        {
            foreach(var edge in ToEdges)
            {
                if (edge.ToNode.IsVisited)
                {
                    edge.ToNode.Color = color;
                    edge.Color = color;
                }
            }
        }
        internal Node(string id,string title = "")
        {
            this.ID = id;
            this.Title = title;
            
        }
        internal Node(string id,string color, string title = "")
        {
            this.ID = id;
            this.Title = title;
            this.Color = color;
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
      



        public void CreateToEdge(Node node,int weight)
        {
            var edge = new Edge(this, node, weight);
            edges.Add(edge);
            CreateEdge?.Invoke(edge);

        }

        public void Delete()
        {
            var deleteEdges = edges.Where(x => x.Source == this).Concat(edges.Where(x => x.Target == this)).ToList();
            foreach(var edge in deleteEdges)
            {
                edges.Remove(edge);
            }
            DeleteNode?.Invoke(this);
        }
        public InternalNode ToInternalNode()
        {
            return new InternalNode
            {
                id = this.ID,
                title = this.Title,
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
            _Edge = edge;
            IsTarget = isTarget;
        }
        internal Edge _Edge;
        public Edge Edge
        {
            get
            {
                return _Edge;
            }
        }
        public void Delete()
        {
            DeleteChange?.Invoke(_Edge);

            Node.edges.Remove(_Edge);
        }
        public string? ID
        {
            get
            {
                return _Edge.ID;
            }
        }
        public string? Color
        {
            get
            {
                return _Edge.Color;
            }
            set
            {
                _Edge.Color = value;
                ColorChange?.Invoke(_Edge);
            }
        }
        public int? Weight
        {
            get
            {
                return _Edge.Weight;
            }
            set
            {
                _Edge.Weight = value;
                WeightChange?.Invoke(_Edge);
            }
        }
        public Node ToNode { 
            get
            {
                if (IsTarget)
                {
                    return _Edge.Target;
                }
                return _Edge.Source;
            }
            set
            {
                if (IsTarget)
                {
                    _Edge.Target = value;
                }
                else
                {
                    _Edge.Source = value;
                }
                ToNodeChange?.Invoke(_Edge);
            }
        }

        public InternalEdge ToInternalEdge()
        {
            return _Edge.ToInternalEdge();

        }
        
    }
}
