
var CY

function drawGraph(divId, graphType) {
    CY = cytoscape({

        container: document.getElementById(divId), // container to render in

        elements: [ // list of graph elements to start with
            { // node a
                data: { id: 'a' }
            },
            { // node b
                data: { id: 'b' }
            },
            { // edge ab
                data: { id: 'ab', source: 'a', target: 'b' }
            },
            { // edge ab
                data: { id: 'ab2', source: 'a', target: 'b' }
            },
            { // edge ab
                data: { id: 'ab3', source: 'a', target: 'b' }
            },

            { // edge ab
                data: { id: 'aaa', source: 'a', target: 'a' }
            },

            { // edge ab
                data: { id: 'aaa1', source: 'a', target: 'a' }
            },
            { // edge ab
                data: { id: 'aa', source: 'b', target: 'a' }
            }

        ],

        style: [ // the stylesheet for the graph
            {
                selector: 'node',
                style: {
                    'background-color': '#666',
                    'label': 'data(title)'
                }
            },

            {
                selector: 'edge',
                style: {
                    'width': 3,
                    'line-color': '#ccc',
                    'target-arrow-color': 'red',
                    'target-arrow-shape': graphType,
                    'curve-style': 'bezier'
                }
            }
        ],
        layout: {
            name: "random"
        }

    });
    var randomPosition = getRandomPosition(CY.width(), CY.height());
    CY.add(
        {
            data: { id: "t1" },
            position: randomPosition
        },

    )
    CY.add(
        {
            data: { id: "t2", source: "t1", target: 'a' },
            position: randomPosition

        }
    );
    var randomPosition = getRandomPosition(CY.width(), CY.height());

    CY.add(
        {
            data: { id: "t12", title: "test" },
            position: randomPosition

        },

    )
    CY.add(
        {
            data: { id: "t22", source: "t12", target: 'a' },
            position: randomPosition

        }
    );
    // 全てのノードのデータを取得
    var nodes = CY.nodes().map(node => node.data());
    CY.getElementById("t12").style("background-color", "blue")
    // 全てのエッジのデータを取得
    var edges = CY.edges().map(edge => edge.data());

    console.log(nodes);
    console.log(edges);
    CY.fit();
    //console.log(CY.elements());
    //var nodes = CY.nodes();
    //var txt1 = JSON.stringify(nodes);
    //console.log(txt1);

    //var txt =JSON.stringify(nodes);
    //console.log(txt);
    var graphData = {};
    graphData.nodes = nodes;
    graphData.edges = edges;
    return graphData;
}
function getRandomPosition(maxX, maxY) {
    return {
        x: maxX - Math.random() * maxX,
        y: maxY - Math.random() * maxY
    };
}
function removeAtId(id) {
    CY.getElementById(id).remove();
}

function add(dataArray) {
    CY.add(data);
}

function changeNodeColor(id, color) {
    CY.getElementById(id).style("background-color", color);
}
function changeEdgeColor(id, color) {
    var line = CY.getElementById(id);
    line.style("line-color", color);
    line.style("target-arrow-color", color);
}

function changeEdgeSource(id, sourceId) {
    var edge = CY.getElementById(id);
    edge.move({ source: sourceId });
}
function changeEdgeTarget(id, target) {
    var edge = CY.getElementById(id);
    edge.move({ target: targetId });
}
function getSize() {
    return { width: CY.width(), height: CY.height() }
}