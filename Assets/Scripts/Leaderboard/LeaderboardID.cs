using UnityEngine;

public class LeaderboardID : MonoBehaviour
{
    [SerializeField] private int ID;
    public static int currentLeaderboardID;
    void Start()
    {
        currentLeaderboardID = ID;
    }
}
