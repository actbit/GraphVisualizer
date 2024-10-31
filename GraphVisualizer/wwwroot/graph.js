
var CY

function drawGraph(divId, edgeAndNode) {
    elementsData = edgeAndNode.nodes.concat(edgeAndNode.edges);
    CY = cytoscape({

        container: document.getElementById(divId), // container to render in

        elements: elementsData,

        style: [ // the stylesheet for the graph
            {
                selector: 'node',
                style: {
                    'background-color': 'data(color)',
                    'label': 'data(title)'
                }
            },

            {
                selector: 'edge',
                style: {
                    'width': 3,
                    'line-color': 'data(color)',
                    'target-arrow-color': 'data(color)',
                    'target-arrow-shape': "none",
                    'curve-style': 'bezier',
                    'label':'data(weight)'
                }
            }
        ],
        layout: {
            name: "random"
        }

    });
    // 全てのノードのデータを取得
    var nodes = CY.nodes().map(node => node.data());
    // 全てのエッジのデータを取得
    var edges = CY.edges().map(edge => edge.data());

    console.log(nodes);
    console.log(edges);
    //CY.fit();
    var graphData = {};
    graphData.nodes = nodes;
    graphData.edges = edges;
    return graphData;
}
function getRandomPosition() {
    var pan = CY.pan();
    var zoom = CY.zoom();
    var width = CY.width();
    var height = CY.height();
    return {

        x: pan.x + Math.random() * width / zoom,
        y: pan.y + Math.random() * height / zoom
    };

}
function removeAtId(id) {
    CY.getElementById(id).remove();
}

function nodeAdd(nodedata) {
    CY.add(
        {
            data: nodedata,
            position: getRandomPosition()
        });
}
function edgeAdd(edgedata) {
    CY.add({
        data:edgedata
    });
}
function changeNodeColor(id, color) {
    CY.getElementById(id).style("background-color", color);
}


function changeEdgeSource(id, sourceId) {
    var edge = CY.getElementById(id);
    edge.move({ source: sourceId });
}
function moveEdge(data) {
    var edge = CY.getElementById(data.id);
    edge.move({ source: data.source,target:data.target });

}
function updateNode(data) {
    var node = CY.getElementById(data.id);
    node.data(data);
}

function updateEdge(data) {
    var edge = CY.getElementById(data.id);
    edge.data(data);
    edge.move({ source: data.source, target: data.target });
}
function changeEdgeTarget(id, target) {
    var edge = CY.getElementById(id);
    edge.move({ target: targetId });
}
function getSize() {
    return { width: CY.width(), height: CY.height() }
}

function isDirected(directed) {
    type = "none";
    if (directed) {
        type = "triangle"
    }
    CY.style()
        .selector("edge")
        .style({
            'target-arrow-shape': type
        })
        .update();
}