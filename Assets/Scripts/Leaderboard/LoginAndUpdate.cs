using UnityEngine;
using LootLocker.Requests;
using System.Collections;

public class LoginAndUpdate : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LeaderboardLogin());
    }

    IEnumerator LeaderboardLogin()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                //Debug.Log("Player Login success");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                //Debug.Log("Player Login failed");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
    public void StartLeaderboardUpdate(float score)
    {
        int testscore = Mathf.FloorToInt(score * 100);
        Debug.Log(testscore);
        StartCoroutine(LeaderboardUpdate(testscore));
    }
    public IEnumerator LeaderboardUpdate(int score)
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, score, LeaderboardID.currentLeaderboardID.ToString(), (responce) =>
        {
            if (responce.success)
            {
                //Debug.Log("Score updated");
                done = true;
            }
            else
            {
                //Debug.Log("Score update failed");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);

    }
}
