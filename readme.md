# GraphVisualizer

GraphVisualizerはグラフ理論で用いられるアルゴリズムなどを可視化することができるツールです。

## webサイトを開く

下記リンクよりもWebサイトを開くことができます。

[GraphVisualizer](https://actbit.github.io/GraphVisualizer/)

## 操作方法

サイトを開くと以下のような画面が表示されます。

![メインページの画像](./images/main_page.png)

それぞれの画面構成について説明します。

画面は以下の画像のように分けることができます。

![](./images/page_classification.png)

### グラフの描画(赤色の四角)

赤色四角で囲んだ部分にグラフが表示されます。

ノード(頂点)をドラッグアンドドロップで頂点を移動することができます。

### グラフの設定(黄色の四角)

黄色の四角で囲まれた部分でグラフの追加・削除・編集ができます。
以下の有効グラフのチェックボックスにチェックをいれると有効グラフに、チェックを消すと無向グラフになります。

![alt text](./images/Directed_button.png)

#### nodeの追加・削除・編集

nodeの欄からnodeの追加・削除・編集が行えます。

nodeの欄は以下のようになっています。
![nodeの設定](./images/node-1.png)

リストボックスにはグラフに存在するすべてのnode(頂点)が表示されており、`id:title`の形で表示されています。

##### nodeの追加

nodeの追加の欄にidとnodeのタイトルを入力することでnodeを追加することができます。

##### nodeの選択

リストボックス内のnodeを選択することで以下のように画面の表示が切り替わります。

![nodeの設定](./images/node-2.png)

##### nodeの削除

リストボックス内のnodeを選択後、nodeの削除ボタンを押すことでnodeを削除することができます。
この際nodeにつながるedge(枝・辺)も削除されます。

##### nodeの編集

リストボックス内のnodeを選択後、`選択中のnodeの削除`の欄からnodeのタイトルを編集することができるようになっています。
`ID`は編集することができません


#### edgeの追加・削除・編集

nodeを選択すると以下のようにedgeの選択・追加画面が表示されるようになります。
![edgeの設定](./images/edge-1.png)

現在選択中のnodeが`1:1`ですので`id:1`にから移動できるedgeの一覧が表示されます。
有効グラフの場合**相手から自分方向のedgeは表示されません**のでご注意ください。

##### edgeの追加

edgeを追加するには、`edgeを追加`の欄より、`edgeの重み`と`接続先のnode`を選択し、`edgeを追加`ボタンをクリックすることで追加することができます。

重みは-1にしておくことで無効化されます

##### edgeの選択

edgeもnode同様選択することで以下のように画面が切り替わり、編集・削除が行えるようになります。

![edgeの設定](./images/edge-2.png)

##### edgeの削除

edgeの削除の欄から`edgeの削除`ボタンを押すことでedgeの削除を行えます。


##### edgeの編集

edgeの編集の欄より重みおよび接続先のnodeの編集を行うことができます。


### プログラムの記述(全画面の緑の四角)

画面説明の緑の四角の部分でグラフの探索を行うためのプログラムを記述することができます。

もし、エラーが発生した場合にはオレンジ色の四角で囲われた部分にエラーが表示されます。

詳しくはAPIの説明にて説明します。

### プログラムの実行

画面上部の`実行`ボタンを押すことでプログラムを実行することができます。
実行が開始すると`次へ`ボタンが有効化されるので、`次へ`ボタンを押すことで次の関数を実行することができるようになっています。

### データの保存

GraphVisualizerはURLにプログラムやノード設定を保存します。
そのためにはURLを生成する必要があります。

URLの生成ボタンを押すことで右側のテキストボックスにURLが生成されるので、そのURLをコピーし、共有してください。

コピーしたURLにアクセスすることでデータが復元されます。
