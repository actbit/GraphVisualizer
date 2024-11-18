using GraphLibrary;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace GraphVisualizer.Pages
{
    public partial class Graph:ComponentBase
    {
        [SupplyParameterFromQuery]
        [Parameter]
        public string? code { get; set; }
        [SupplyParameterFromQuery]
        [Parameter]
        public string? base64code { get; set; }
        [SupplyParameterFromQuery]
        [Parameter]
        public string? graph { get; set; }
        string url = "";
        string output = "";
        string sourceCode = "";
        string? controlChecked = null;
        string baseAddress = "";
        async Task CreateURL()
        {
            clear();
            var baseurl = navigation.Uri.Split('?').First();
            JsonNodeEdge jsonNodeEdge = new JsonNodeEdge();
            jsonNodeEdge.ToDirect = isDirected;
            jsonNodeEdge.Edges = Edge.Edges.Select(x => x.ToInternalEdge()).ToList();
            jsonNodeEdge.Nodes = Node.Nodes.Select(x => x.ToInternalNode()).ToList();
            var jsonString = JsonSerializer.Serialize(jsonNodeEdge, options);
            var d = JsonSerializer.Deserialize<JsonNodeEdge>(jsonString, options);
            string json = await CodeChange.CodeToParameter(jsonString);
            string source = await CodeChange.CodeToParameter(sourceCode);
            string sss = await CodeChange.ParameterToCode(source);

            url = baseurl + $"?code={source}&graph={json}";

        }
        bool ControllerMenuVisible
        {
            get
            {
                if (controlChecked == "checked")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                controlChecked = value ? "checked" : null;
            }
        }
        string? programChecked = null;
        bool ProgramMenuVisible
        {
            get
            {
                if (programChecked == "checked")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                programChecked = value ? "checked" : null;
            }
        }

        bool isInterval = false;
        bool _isDirected = false;
        bool isDirected
        {
            get
            {
                return _isDirected;
            }
            set
            {
                _isDirected = value;
                Node.IsDirected = value;
            }
        }
        double RunInterval = 0;
        string createEdgeWarning = "";
        string createEdgeWeighInput = "-1";
        string CreateToEdgeNodeSelected;
        string EditToEdgeNodeSelected;
        string editEdgeWeighInput = "-1";
        List<Node> Nodes = new List<Node>();
        ToEdge SelectedToEdge;
        Node SelectedNode;
        string editEdgeWarning = "";
        bool IsInitialized = false;
        string createNodeTitle = "";
        string createNodeId = "";
        string editNodeTitle = "";
        string warningCreateNode = "";
        string outputColor = "black";

        void StopProgram()
        {

            NowNode = null;
            NowAction = null;
            wait_button = false;
        }
        void clear()
        {
            Nodes.ForEach(x => { x.Color = "#666"; x.IsVisited = false; x.State = 0; });
            Nodes.ForEach(x => x.ToEdges.ToList().ForEach(x => x.Color = "#ccc"));
        }
        async void RunProgram()
        {
            clear();
            outputColor = "black";
            output = "";
            Assembly? assembly;
            var error = Compiler.Compile(sourceCode, out assembly);
            if (error.Length > 0)
            {
                foreach (var e in error)
                {
                    output += e + "\n";
                }
                outputColor = "red";
                await JSRuntime.InvokeVoidAsync("moveCursorToEnd", "output-area");

                return;
            }
            var methodinfo = Compiler.RunMethodGet(typeof(GraphAction), assembly, "Action");
            var type = methodinfo.DeclaringType;
            instance = Activator.CreateInstance(type);
            Func<Node, Node> action = methodinfo.CreateDelegate<Func<Node?, Node>>(instance);
            var startnode = Nodes.Where(x => x.ID == "start").FirstOrDefault();
            if (startnode == null)
            {
                startnode = Nodes.FirstOrDefault();
            }
            if (startnode == null)
            {
                return;
            }
            NowAction = action;
            NowNode = NowAction.Invoke(startnode);
            wait_button = true;
            if (NowNode != null)
            {
                wait_button = true;
            }
            else
            {
                StopProgram();

            }
        }
        Object instance;
        Node NowNode = null;
        Func<Node?, Node>? NowAction = null;
        void nextStep()
        {
            wait_button = false;
            NowNode = NowAction.Invoke(NowNode);
            if (NowNode != null)
            {
                wait_button = true;
            }
            else
            {
                StopProgram();

            }
            StateHasChanged();
        }

        bool isMoved = false;
        void clickViewNodeSetting()
        {
            ControllerMenuVisible = !ControllerMenuVisible;
            if (ControllerMenuVisible)
            {
                ProgramMenuVisible = false;
            }
            StateHasChanged();
        }
        void clickProgramSetting()
        {
            ProgramMenuVisible = !ProgramMenuVisible;
            if (ProgramMenuVisible)
            {
                ControllerMenuVisible = false;
            }
        }

        bool _isAutoRunning;


        bool isAutoRunning
        {
            get
            {
                return _isAutoRunning;
            }
            set
            {
                _isAutoRunning = value;
            }
        }

        void clickNodeDelete()
        {
            SelectedNode.Delete();
        }
        void clickNodeCreate()
        {
            if (Nodes.Where(x => x.ID == createNodeId).Count() > 0)
            {
                warningCreateNode = "存在するIDです。";

                return;
            }
            Node.Create(createNodeId, createNodeTitle);
            createNodeId = "";
            createNodeTitle = "";
        }
        void clickNodeEdit()
        {
            SelectedNode.Title = editNodeTitle;
        }
        void SelectToEdge(ToEdge toEdge)
        {

            SelectedToEdge = toEdge;
            editEdgeWeighInput = toEdge.Weight.ToString();
            EditToEdgeNodeSelected = toEdge.ToNode.ID;
            StateHasChanged();
        }

        void clickEdgeCreate()
        {
            int weight = 0;
            if (!int.TryParse(createEdgeWeighInput, out weight))
            {
                createEdgeWarning = "edgeの重みは数値にしてください";
                StateHasChanged();

                return;
            }
            var targetNode = Nodes.Where(x => x.ID == CreateToEdgeNodeSelected).FirstOrDefault();
            if (targetNode == default(Node))
            {
                if (CreateToEdgeNodeSelected == null || CreateToEdgeNodeSelected == "")
                {
                    createEdgeWarning = "nodeを選択してください";

                }
                else
                {
                    createEdgeWarning = "削除されたnodeです";

                }

                return;
            }
            SelectedNode.CreateToEdge(targetNode, weight);
            StateHasChanged();
        }
        void clickEdgeDelete()
        {
            if (SelectedToEdge != null)
            {
                SelectedToEdge.Delete();
            }
        }
        void clickEdgeEdit()
        {
            int weight = 0;
            if (!int.TryParse(editEdgeWeighInput, out weight))
            {
                editEdgeWarning = "edgeの重みは数値にしてください";
                return;
            }
            var targetNode = Nodes.Where(x => x.ID == EditToEdgeNodeSelected).FirstOrDefault();
            if (targetNode == default(Node))
            {
                if (EditToEdgeNodeSelected == null || EditToEdgeNodeSelected == "")
                {
                    editEdgeWarning = "nodeを選択してください";

                }
                else
                {
                    editEdgeWarning = "削除されたnodeです";

                }
                return;
            }
            SelectedToEdge.ToNode = targetNode;
            SelectedToEdge.Weight = weight;
            StateHasChanged();
        }
        void SelectNode(Node node)
        {
            if (node == SelectedNode)
            {
                return;
            }
            SelectedNode = node;
            SelectedToEdge = null;
            createEdgeWeighInput = "-1";
            CreateToEdgeNodeSelected = "";
            editNodeTitle = node.Title;

        }
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = false,
            PropertyNameCaseInsensitive = true,

        };
        Compiler Compiler;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

                Compiler = new Compiler(ScriptService);
                IsInitialized = false;
                if (code == null)
                {
                    code = base64code;
                }
                if (code != null)
                {
                    string code = "";
                    try
                    {
                        code = await CodeChange.ParameterToCode(this.code);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    sourceCode = code;
                }
                else
                {
                    sourceCode = Compiler.BaseCode;
                }
                bool direct = false;
                if (graph != null)
                {

                    graph = await CodeChange.ParameterToCode(graph);
                    var data = JsonSerializer.Deserialize<JsonNodeEdge>(graph, options);
                    direct = data.ToDirect;

                    data.Nodes.ForEach(x => Node.Create(x.id, x.color, x.title));
                    var nodes = Node.Nodes.ToList();
                    data.Edges.ForEach(x =>
                    {
                        var node1 = nodes.Where(y => x.source == y.ID).FirstOrDefault();
                        var node2 = nodes.Where(y => x.target == y.ID).FirstOrDefault();

                        if (node1 != null)
                        {
                            node1.CreateToEdge(node2, int.TryParse(x.weight, out int w) ? w : -1);//重みの設定がない場合(InternalEdge以外では-1として処理される？)、InternalEdgeのweightは空の文字列となるため、int.TryParseで判定する
                        }
                    });
                }
                else
                {
                    var node0 = Node.Create("start", "#666", "start");
                    var node1 = Node.Create("1", "#666", "1");
                    var node2 = Node.Create("2", "#666", "2");
                    var node3 = Node.Create("3", "#666", "3");
                    var node4 = Node.Create("4", "#666", "4");
                    var node5 = Node.Create("5", "#666", "5");
                    var node6 = Node.Create("6", "#666", "6");
                    var node7 = Node.Create("end", "#666", "end");
                    node0.CreateToEdge(node1, -1);
                    node0.CreateToEdge(node2, -1);
                    node2.CreateToEdge(node3, -1);
                    node3.CreateToEdge(node0, -1);
                    node3.CreateToEdge(node4, -1);
                    node4.CreateToEdge(node5, -1);
                    node3.CreateToEdge(node6, -1);
                    node3.CreateToEdge(node5, -1);

                    node6.CreateToEdge(node1, -1);
                    node4.CreateToEdge(node7, -1);
                }



                EdgeAndNode en = new EdgeAndNode();
                Nodes = Node.Nodes.ToList();

                en.Nodes = Nodes.Select(x => new InNodeObject { data = x.ToInternalNode() }).ToList();
                en.Edges = Edge.Edges.Select(x => new InEdgeObject { data = x.ToInternalEdge() }).ToList();
                await JSRuntime.InvokeVoidAsync("drawGraph", "cy", en);
                Node.ColorChange = NodeColorChange;
                Node.CreateNode = CreateNode;
                Node.DeleteNode = DeleteNode;
                ToEdge.ColorChange = UpdateEdge;
                ToEdge.DeleteChange = EdgeDelete;
                ToEdge.WeightChange = UpdateEdge;
                ToEdge.ToNodeChange = UpdateEdge;
                Node.CreateEdge = CreateEdge;
                Node.TitleChange = NodeTitleChange;
                Node.DirectedChange = DirectedChange;
                Edge.TargetNodeChange = UpdateEdge;
                Edge.SourceNodeChange = UpdateEdge;
                Edge.WeightChange = UpdateEdge;
                Edge.ColorChange = UpdateEdge;
                Edge.DeleteChange = EdgeDelete;
                InternalClass.PrintAfter = Print;
                isDirected = direct;
                StateHasChanged();
            }

        }
        async void Print(string str)
        {
            this.output += str + "\n";
            await JSRuntime.InvokeVoidAsync("moveCursorToEnd", "output-area");
            StateHasChanged();

        }

        bool tmp = false;
        string? wait_button_str = "disabled";
        bool wait = true;
        bool wait_button
        {
            get
            {
                if (wait_button_str == "disabled")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                wait_button_str = value ? null : "disabled";
            }
        }


        void WaitProgram()
        {
            if (isInterval)
            {

                Task.Delay((int)(1000 * RunInterval));
                StateHasChanged();

            }
            else
            {
                wait_button = true;
                wait = true;
                while (wait)
                {
                    Task.Delay(100);


                }
            }

        }

        async Task click()
        {

        }
        async void DirectedChange(bool directed)
        {
            await JSRuntime.InvokeVoidAsync("isDirected", directed);

        }
        async void NodeTitleChange(Node node)
        {
            await JSRuntime.InvokeVoidAsync("updateNode", node.ToInternalNode());

        }

        async void EdgeDelete(Edge edge)
        {
            if (SelectedNode.ToEdges.Where(x => x.ID == edge.ID).Count() > 0)
            {
                if (SelectedToEdge.ID == edge.ID)
                {
                    SelectedToEdge = null;
                }
                StateHasChanged();
            }
            await JSRuntime.InvokeVoidAsync("removeAtId", edge.ID);
        }
        async void UpdateEdge(Edge edge)
        {
            await JSRuntime.InvokeVoidAsync("updateEdge", edge.ToInternalEdge());

        }

        async void NodeColorChange(Node node)
        {
            await JSRuntime.InvokeVoidAsync("updateNode", node.ToInternalNode());

        }

        async void CreateNode(Node node)
        {
            Nodes = Node.Nodes.ToList();
            await JSRuntime.InvokeVoidAsync("nodeAdd", node.ToInternalNode());

            StateHasChanged();
        }

        async void CreateEdge(Edge edge)
        {
            Nodes = Node.Nodes.ToList();
            await JSRuntime.InvokeVoidAsync("edgeAdd", edge.ToInternalEdge());
            StateHasChanged();
        }
        async void DeleteNode(Node node)
        {

            Nodes = Node.Nodes.ToList();
            if (!Nodes.Contains(SelectedNode))
            {
                SelectedNode = null;
            }
            await JSRuntime.InvokeVoidAsync("removeAtId", node.ID);

            StateHasChanged();
        }
    }
}
