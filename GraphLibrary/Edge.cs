using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GraphLibrary
{
    public class Edge
    {
        internal static List<Edge> edges = new List<Edge>();
        public static IReadOnlyList<Edge> Edges
        {
            get { return edges; }
        }
        public static Action<Edge>? ColorChange;
        public static Action<Edge>? WeightChange;
        public static Action<Edge>? SourceNodeChange;
        public static Action<Edge>? TargetNodeChange;

        public static Action<Edge>? DeleteChange;
        public string ID { get; private set; }
        Node _Source;
        public Node Source
        {
            get
            {
                return _Source;
            }
            set
            {
                _Source = value;
                SourceNodeChange?.Invoke(this);
            }
        }
        Node _Target;
        public Node Target
        {
            get
            {
                return _Target;
            }
            set
            {
                _Target = value;
                TargetNodeChange?.Invoke(this);
            }
        }
        int? _Weight;
        public int? Weight
        {
            get
            {
                return _Weight;
            }
            set
            {
                _Weight = value;
                WeightChange?.Invoke(this);
            }
        }

        public string _Color = "#ccc";
        public string Color
        {
            get
            {
                return _Color;
            }
            set 
            { 
                _Color = value;
                ColorChange?.Invoke(this);  
            } 
        }


        internal Edge(Node source,Node target)
        {
            ID = Guid.NewGuid().ToString();
            Source = source;
            Target = target;
        }
        internal Edge(Node source, Node target, int? weight = null,string? color = "#ccc")
        {
            ID = Guid.NewGuid().ToString();
            Source = source;
            Target = target;
            Weight = weight;
            Color = color;
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
                id = ID,
                source = Source.ID,
                target = Target.ID,
                weight = wstr,
                color = this.Color
            };
        }
        public void Delete()
        {
            Edge.edges.Remove(this);
            DeleteChange?.Invoke(this);
        }
    }

    public class InternalEdge
    {
        public string id { get; set; }
        public string source { get; set; }
        public string target { get; set; }
        public string weight { get; set; }
        public string color { get;set; }

    }
}
