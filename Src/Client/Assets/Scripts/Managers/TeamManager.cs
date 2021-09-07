using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : Singleton<TeamManager>
{
    public void Init()
    { 
    }

    public void UpdateTeamInfo(NTeamInfo team)
    {
        User.Instance.TeamInfo = team;
        ShowTeamUI(team != null);
    }

    public void ShowTeamUI(bool show)
    {
        if (UIMainCity.Instance != null)
        {
            UIMainCity.Instance.ShowTeamUI(show);
        }
    }
}
