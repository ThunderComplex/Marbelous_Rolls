using UnityEngine;
using TMPro;
using LootLocker.Requests;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResultScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalTimeText;
    [SerializeField] private TMP_InputField inputField;

    [SerializeField] private GameObject rankPrefab;
    [SerializeField] private Transform rankPrefabRoot;

    private bool commitedTime;
    private float newTime;

    public void SetGameoverScreen(float _newTime)
    {
        newTime = _newTime;
        StartCoroutine(SetScoreText());
        StartCoroutine(IngameLeaderboardUpdate());
    }
    private IEnumerator IngameLeaderboardUpdate()
    {
        if (rankPrefabRoot.childCount != 0)
        {
            for (int i = rankPrefabRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(rankPrefabRoot.GetChild(i).gameObject);
            }
        }

        bool done = false;
        LootLockerSDKManager.GetScoreList(LeaderboardID.currentLeaderboardID.ToString(), 10, (response) =>
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

                    float time = (float)members[i].score / 100;
                    int milliseconds = Mathf.FloorToInt(time * 100);
                    milliseconds = Mathf.FloorToInt(milliseconds % 100);
                    int seconds = Mathf.FloorToInt(time % 60);
                    int minutes = Mathf.FloorToInt(time / 60 % 60);
                    prefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
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
    private IEnumerator SetScoreText()
    {
        float time = newTime;
        int milliseconds = Mathf.FloorToInt(time * 100);
        milliseconds = Mathf.FloorToInt(milliseconds % 100);
        int seconds = Mathf.FloorToInt(time % 60);
        int minutes = Mathf.FloorToInt(time / 60 % 60);

        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.GetMemberRank(LeaderboardID.currentLeaderboardID.ToString(), playerID, (response) =>
        {
            if (response.success)
            {

                float oldtime = (float)response.score / 100;
                int oldmilliseconds = Mathf.FloorToInt(oldtime * 100);
                oldmilliseconds = Mathf.FloorToInt(oldmilliseconds % 100);
                int oldseconds = Mathf.FloorToInt(oldtime % 60);
                int oldminutes = Mathf.FloorToInt(oldtime / 60 % 60);

                if (newTime >= oldtime)
                {

                    finalTimeText.text = "Final Time:\n" + string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
                    finalTimeText.text += "\n\n Personal Best:\n (Rank " + response.rank +") " + string.Format("{0:00}:{1:00}.{2:00}", oldminutes, oldseconds, oldmilliseconds);
                }
                else
                {
                    finalTimeText.text = "New Rekord!\n" + string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
                    finalTimeText.text += "\n\n Old Personal Best:\n (Rank " + response.rank + ") " + string.Format("{0:00}:{1:00}.{2:00}", oldminutes, oldseconds, oldmilliseconds);
                }
                done = true;
            }
            else
            {
                finalTimeText.text = "New Personal Best!\n" + string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
    public void CommitScore()
    {
        if (inputField.text == string.Empty)
        {
            Debug.Log("inputField is empty");
            return;
        }

        if (commitedTime == false)
        {
            LootLockerSDKManager.SetPlayerName(inputField.text, (respone) =>
            {
                if (respone.success)
                {
                    Debug.Log("Changed name");
                }
                else
                {
                    Debug.Log("Changed name failed");
                }
            });
            MenuController.Instance.GetComponent<LoginAndUpdate>().StartLeaderboardUpdate(newTime);
            AudioController.Instance.PlaySoundOneshot((int)AudioController.Sounds.menuButton);
            commitedTime = true;

            StartCoroutine(IngameLeaderboardUpdate());
        }
    }
}
