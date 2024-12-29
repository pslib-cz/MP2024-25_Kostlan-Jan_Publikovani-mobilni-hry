using UnityEngine;
using SmokeTest;

public class AchievmentCall : MonoBehaviour
{
    private string mStatus;
    [SerializeField] private string mAchievementID;
    void Start()
    {
        DoAchievementUnlock();
    }

    public void DoAchievementUnlock()
    {
        SetStandBy("Unlocking achievement...");
        Social.ReportProgress(
            mAchievementID,
            100.0f,
            (bool success) =>
            {
                EndStandBy();
                mStatus = success ? "Unlocked successfully." : "*** Failed to unlock ach.";
                ShowEffect(success);
            });
        Debug.Log("Achievment unlocked");
    }

    public void ShowEffect(bool success)
    {
        Camera.main.backgroundColor =
            success ? new Color(0.0f, 0.0f, 0.8f, 1.0f) : new Color(0.8f, 0.0f, 0.0f, 1.0f);
    }

    private void SetStandBy(string msg)
    {
        mStatus = msg;
    }

    private void EndStandBy()
    {
        mStatus += " (Done!)";
    }
}