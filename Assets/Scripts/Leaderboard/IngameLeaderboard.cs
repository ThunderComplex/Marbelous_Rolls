using UnityEngine;
using LootLocker.Requests;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IngameLeaderboard : MonoBehaviour
{
    private int levelID;
    private int levelIndex;

    [SerializeField] private GameObject rankPrefab;
    [SerializeField] private Transform rankPrefabRoot;

    [SerializeField] private TextMeshProUGUI leaderboardText;
    [SerializeField] private TextMeshProUGUI personalHighscoreText;
    [SerializeField] private TextMeshProUGUI startGameText;

    [SerializeField] private Button level1Button;

    private bool skipFirstSound;

    private void OnEnable()
    {
        skipFirstSound = true;
        level1Button.onClick.Invoke();
    }
    public void LeaderboardUpdate(int ID)
    {
        levelID = ID;
        StartCoroutine(IngameLeaderboardUpdate());
        StartCoroutine(PersonalHighscore());

        if (skipFirstSound == true)
        {
            skipFirstSound = false;
            return;
        }
        else AudioController.Instance.PlaySoundOneshot((int)AudioController.Sounds.menuButton);
    }
    public void SetLevelIndex(int index)
    {
        levelIndex = index;
        leaderboardText.text = "Leaderboard\n" + "Level " + index;
        startGameText.text = "Start Level " + index;
    }
    public IEnumerator IngameLeaderboardUpdate()
    {
        if (rankPrefabRoot.childCount != 0)
        {
            for (int i = rankPrefabRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(rankPrefabRoot.GetChild(i).gameObject);
            }
        }


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

                    float time = (float)members[i].score / 100;
                    int milliseconds = Mathf.FloorToInt(time * 100);
                    milliseconds = Mathf.FloorToInt(milliseconds % 100);
                    int seconds = Mathf.FloorToInt(time % 60);
                    int minutes = Mathf.FloorToInt(time / 60 % 60);
                    prefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
                    //prefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = members[i].score.ToString();
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
    public IEnumerator PersonalHighscore()
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.GetMemberRank(levelID.ToString(), playerID, (response) =>
        {
            if (response.success)
            {
                if(response.score == 0)
                {
                    personalHighscoreText.text = "Personal best: ---";
                }
                else
                {
                    float time = (float)response.score / 100;
                    int milliseconds = Mathf.FloorToInt(time * 100);
                    milliseconds = Mathf.FloorToInt(milliseconds % 100);
                    int seconds = Mathf.FloorToInt(time % 60);
                    int minutes = Mathf.FloorToInt(time / 60 % 60);
                    personalHighscoreText.text = "Personal best: " + string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
                }
                done = true;
            }
            else
            {
                personalHighscoreText.text = "Personal best: ---";
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
    public void StartGame()
    {
        AudioController.Instance.PlaySoundOneshot((int)AudioController.Sounds.menuButton);
        Time.timeScale = 1;

        SceneManager.LoadScene(levelIndex);
    }
}
