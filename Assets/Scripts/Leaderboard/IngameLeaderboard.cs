using UnityEngine;
using LootLocker.Requests;
using System.Collections;
using TMPro;

public class IngameLeaderboard : MonoBehaviour
{
    private int levelID;
    [SerializeField] private GameObject rankPrefab;
    [SerializeField] private Transform rankPrefabRoot;
    public void LeaderboardUpdate(int ID)
    {
        levelID = ID;
        StartCoroutine(IngameLeaderboardUpdate());
    }
    public IEnumerator IngameLeaderboardUpdate()
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList(levelID.ToString(), 10, (response) =>
        {
            if (response.success)
            {
                LootLockerLeaderboardMember[] members = response.items;

                for (int i = 0; i < members.Length; i++)
                {
                    GameObject prefab = Instantiate(rankPrefab, rankPrefabRoot);
                    prefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = members[i].rank + ". ";
                    if (members[i].player.name != "")
                    {
                        prefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text += members[i].player.name;
                    }
                    else
                    {
                        prefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text += members[i].player.id;
                    }
                    prefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = members[i].score.ToString();
                }
                done = true;
            }
            else
            {
                Debug.Log("load leaderboard failed");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
}
