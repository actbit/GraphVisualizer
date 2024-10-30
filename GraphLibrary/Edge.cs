using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GraphLibrary
{
    public class Edge
    {
        public string ID;
        internal Node Source {  get; set; }
        internal Node Target { get; set; }
        internal int? Weight { get; set; }
        internal string? Color { get; set; }
        internal Edge(Node source,Node target)
        {
            ID = Guid.NewGuid().ToString();
            Source = source;
            Target = target;
        }
        internal Edge(Node source, Node target, int? weight = null,string? color = null)
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
                source = Source.id,
                target = Target.id,
                weight = wstr
            };

        }
    }

    public class InternalEdge
    {
        public string id { get; set; }
        public string source { get; set; }
        public string target { get; set; }
        public string weight { get; set; }

    }
}
