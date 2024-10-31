using GraphLibrary;

namespace GraphVisualizer
{
    public class InNodeObject
    {
        public InternalNode data { get; set; }
    }
    public class InEdgeObject
    {
        public InternalEdge data { get; set; }
    }

    public class EdgeAndNode
    {
        public List<InNodeObject> Nodes { get; set; }
        public List<InEdgeObject> Edges{ get; set; }
    }
    public class JsonNodeEdge
    {
        public bool ToDirect = false;
        public List<InternalNode> Nodes { get; set; }
        public List<InternalEdge> Edges { get; set; }

    }
}
