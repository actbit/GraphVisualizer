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


## プログラムの書き方

プログラムの記述方法を説明します。

### プログラムの基本的な考え

GraphVisualizerはC#でプログラムを記述することでアルゴリズムを表示することができるようになっています。

以下のコードは初期状態のプログラムです。

```cs
using System;
using GraphLibrary;

public class ActionAlgorithm:GraphAction
{
    public override Node? Action(Node node)
    {
        // ここにアルゴリズムを記載する。
    }
}
```
#### 関数について
`public class ActionAlgorithm:GraphAction`の部分は`GraphAction`という型を派生した`ActionAlgorithm`という型(クラス)を宣言しています。

```
public override Node? Action(Node node)
{
    // ここにアルゴリズムを記載する。
}
```
の部分で戻り値がNode型、引数がNode型のActionという関数を宣言しています。
このActionという関数は引数にターゲットとなる(注目している)Nodeが与えられます。
戻り値に指定したNodeで次のターゲットとなるNodeを指定することができます。
つまり、戻り値として返したNodeが次のActionの呼び出し時に引数として渡されるようになります。
戻り値をnullとした場合に探索を終了します。


実際の使い方としては探索などの際に次に探索するNodeの決定やデータNodeデータの保存などを行えるようになっています。

以下に深さ優先探索のプログラムの例を示します。
```cs
using System;
using GraphLibrary;
using System.Collections.Generic;
public class ActionAlgorithm:GraphAction
{
    // Nodeのスタックを作成
    Stack<Node> NodeStack = new Stack<Node>();
    public override Node? Action(Node node)
    {
        node.IsVisited = true; // 探索済みにする
        // nodeの色を変える
        node.Color = "red";
        if(node.ID == "end") // endを探す
        {
            Print("endを発見しました");
            return null;
        }
        // 次のnodeを入れる変数
        Node? nextNode = null;
        // nodeから移動できるedge一覧を取得
        var edges = node.ToEdges;
        // edge一覧をループで確認
        for(int i = 0;i< edges.Count;i++)
        {
            // edgeにつながるnodeが訪問済みか確認
            if(edges[i].ToNode.IsVisited == false)
            {
                //訪問済みではない場合

                // 次のnodeに設定
                nextNode = edges[i].ToNode;
                // edgeの色を変える
                edges[i].Color = "blue";
                // ループを抜ける
                break;
            }
        }

        // 次のnodeがあるか確認
        if(nextNode  == null)
        {
            // スタックの中身が0個か確認
            if(NodeStack.Count == 0)
            {
                // 見つからなかったので探索を終了する。
                Print("接続されていません");
                // nullを返すと探索終了
                return null;
            }
            // Stackからpopして次のnodeに設定
            nextNode = NodeStack.Pop();
        }
        else
        {
            // 現在のnodeをスタックにpush
            NodeStack.Push(node);
        }

        // nextNodeを返してメソッド終了
        return nextNode ;
    }
}
```

このプログラムは以下のリンクより実行可能です
[深さ優先探索のサンプルプログラム](https://actbit.github.io/GraphVisualizer/?code=KLUv_QBY9BoAtmiOQxCrOgdIciIAAABg~48sZtV5FRum4cwHfcPVVVF8_tr11g5wULG7smkzRrevuVch05TjEFaG5VUDok7x0RcRAAACBwF9AHgAfwCiDe2BsMCmuzXVu2BX73IyAqpnMa4bl9NkFFSwuw5QvcuRyPh0xAuREkry4cXMr5KfJN8lP4f8Mf_qXU5vW3e3xkADoCESLb6hb2Kiw7DzffpjP8840riPxRWsFZsQvFb81O~7FBuN48Ad7LEGRKMOrtA4jSqGHBcdAm04Bp24oIIYUKiJdZb5T_lPGpxmjvmbXEBJ3zeCDeJRrJjTYEpa_FP~ZSuVqWaYjAhabNHdrfJfgaBmfg~svW2twXhgxXTX9YAMyHIYr2JMd43vdylK_ibWaebPYEpj_ttW5lf5w_xd_Ess6GpltgJBe9uaqyMWu2uvBqiZMXX8PNxI7Ex5pTDXSiLnSoXTQ8Xfpanh1wF1x7kYmwQWQRmCOlFe6anMf_JH8WdpICAsJ4LjpdhIWC6iu7YtgjaDL~gDJZWtZi7_FPkeyIeRH9aQR~BidB3WkFcbdzZSLeeEkNPM72qlHSwvNeTVMv~Hv8sfVZ4dXDNT_jIkjdTdrXEm8knyP~Q3Ib_ln9o2UTiKwnEEe5GcLSGzGn5Vkxldi9bojl7~Kn8JLqmZ__vGCPkpePi3_E_5Uf4jG2YnIGjueAeyl6xhZuZ3~cf8S_wt~Jesjuiu67LjjDj5w2RpCstLzLvOgGyZP_ufNFJGp2IPBH~SBwF2qIGVUQiZIRESmU2l2CACQgxjdT0RlCyEIDJKEclIU1CQbIwB4Q9O~gJ61~HdCgZb5bJOpjBtgAXWGh~cGdjGoAGWIVBYh2o4pSqMZJM1aIDRjEP5MMTm~ZQ91txaW9lg2wNyMZA4jaAC6XqInA97yXVtrGZPP2ODSYnWrySR4An0eFaVfnyqc0d_YRMoSMXaUCKq~9ouzPKMwzv9c9k1J4ZxWURSgW0RpIr38mY1escSWz64DkzQHr8pBVbhmUy2NkrAnenuPIupgVXx1imfw0dsRid8hjLe_Qi~Ua6bqFAaJj6ht1ZeIwdGYixxowWHXccQEL__H3VlV67uO709rqNHePAO5gDdyiogJ5XVl05fEII~j9gGqC4Ml1~CE3A5Aw--&graph=KLUv_QBY5A0A1p1PHWBL2gEanH95VKdLYfpMwSAZMEmR9L7QeFpbLALjUwBFAEEAiYVyiL~xs7RparZlGA0NFiufdlWvwRI4CDEQBYQYS8FVMxHPuVNVAGMQD8dyhSKRkGhPWCAsEMdCoRYLBwk4FsJBDAdCCDEISMCx4CsxcBBiOAT45gzRrakfWu5By6P6X~U5o6fR~PdtmyJKprxoRyk2ZLXQ8nN6Pv_xF2GaOV1rVmiq5Jt5PXfv7j4wQPCGhRso1KIVDAHrijuLMm2SvWMy9EM7CNdMct5NlGj8S7ZSbtIYMq51YTRUdvN_RpgKVcmlS6__uO0r2x~jpTVo7rx9thxlTN5fAg9GASEGcrotd5Dx1s4nZVgWWnpKQ1eMp7vRxeuFS67htsaTzPXQCsozfuRXPJXJROv_VbGT0trSguaN2BhXj1XRNqCRfDJjEieBxEh6EEMKqrYBNagEeA_omEZQaRLNAtQVBiVBTy2gBJ2GoCcMFDIEcVCakS46TCOgMOiInopCOTBRPgxaRhtPIJtIB39G3_9j9cvsPUo3f6xmAp4l4CQJZ0k4S8IZSVrGUJZwlizSqEAyD2g9kZhMV4kC)