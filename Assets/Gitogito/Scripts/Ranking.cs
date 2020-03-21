using System.Collections.Generic;
using UnityEngine;
using NCMB;
using UnityEngine.UI;

// ランキングのクラス
public class Ranking : UIManager
{
    // ランカーオブジェクト
    [SerializeField] private GameObject rankerPrefab;

    private void Start ()
    {
        // NCMBのキーを設定
        NCMBSettings.ApplicationKey = MyStrings.NCMB_APPKEY;
        NCMBSettings.ClientKey = MyStrings.NCMB_CLIENTKEY;
    }

    // ランキングを初期化
    public void Init ()
    {
        GameObject rankingUI = GameObject.Find ("UI/Ranking/Panel");
        rankingUI.transform.Find ("BestScore").GetComponent<Text> ().text = "ベストスコア : " + DataManager.GetBestScore ();
        Transform rankerParent = rankingUI.transform.Find ("LeaderBoard/Viewport/Content");
        for (int i = 0 ; i < rankerParent.childCount ; i++)
        {
            Destroy (rankerParent.GetChild (i).gameObject);
        }
        GetRanking ();
    }

    // ランキングをサーバーから取得し適用
    public void GetRanking ()
    {
        Loading ();
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("Ranking");
        query.OrderByAscending ("Time");
        query.Limit = 30;
        query.FindAsync ((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e != null)
            {
                ShowToast ("エラーが発生しました。");
            }
            else
            {
                // ランカーの内容を適用していく
                Transform rankerParent = GameObject.Find ("UI/Ranking/Panel/LeaderBoard/Viewport/Content").transform;
                int r = 0;
                foreach (NCMBObject obj in objList)
                {
                    r++;
                    float s = (float)System.Convert.ToDouble (obj["Time"]);
                    string n = System.Convert.ToString (obj["Name"]);
                    GameObject ranker = Instantiate (rankerPrefab, rankerParent);
                    ranker.transform.GetChild (0).GetComponent<Text> ().text = r.ToString ();
                    ranker.transform.GetChild (1).GetComponent<Text> ().text = n;
                    ranker.transform.GetChild (2).GetComponent<Text> ().text = s.ToString ();
                }
            }
            LoadEnd ();
        });
    }

    // スコアをアップロード
    public void UploadScore ()
    {
        // タイムがないとアップロードできない
        if (DataManager.GetBestScore () < 0.01f)
        {
            ShowToast ("スコアがありません。");
            return;
        }
        // 名前を入力してないとアップロードできない
        string upName = GameObject.Find ("UI/Ranking/Panel/InputField").GetComponent<InputField> ().text;
        if (string.IsNullOrEmpty (upName))
        {
            ShowToast ("名前を入力してください。");
            return;
        }
        // ローディングのクルクルを表示
        Loading ();
        // アップロード
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("Ranking");
        query.WhereEqualTo ("Name", upName);
        query.FindAsync ((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e == null)
            {
                // 新規登録
                if (objList.Count == 0)
                {
                    NCMBObject obj = new NCMBObject ("Ranking");
                    obj["Name"] = upName;
                    obj["Time"] = DataManager.GetBestScore ();
                    obj.SaveAsync ((NCMBException ee) =>
                    {
                        if (ee == null)
                        {
                            Init ();
                        }
                        else
                        {
                            LoadEnd ();
                            ShowToast ("エラーが発生しました。");
                        }
                    });
                }
                else // 更新
                {
                    // サーバーの方が遅い時更新できる
                    float cloudScore = (float)System.Convert.ToDouble (objList[0]["Time"]);
                    if (DataManager.GetBestScore () < cloudScore)
                    {
                        objList[0]["Time"] = DataManager.GetBestScore ();
                        objList[0].SaveAsync ((NCMBException ee) =>
                        {
                            if (ee == null)
                            {
                                Init ();
                            }
                            else
                            {
                                LoadEnd ();
                                ShowToast ("エラーが発生しました。");
                            }
                        });
                    }
                    else
                    {
                        // 初期化
                        Init ();
                    }
                }
            }
            else
            {
                LoadEnd ();
                ShowToast ("エラーが発生しました。");
            }
        });
    }
}
