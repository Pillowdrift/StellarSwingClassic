using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

public class GameCenterSingleton
{
#region Singleton variables and functions
	private static GameCenterSingleton instance;
	
	public static GameCenterSingleton Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new GameCenterSingleton();
				instance.Initialize();
			}
			return instance;
		}
	}
#endregion
	
	private IAchievement[] achievements;
	
	public GameCenterSingleton (){}
	
	public void Initialize()
	{
#if UNITY_IPHONE
		if(!IsUserAuthenticated())
		{
			Social.localUser.Authenticate (ProcessAuthentication);
		}
#endif		
	}	
	
	public bool IsUserAuthenticated()
	{
#if UNITY_IPHONE
		if(Social.localUser.authenticated)
		{
			return true;
		}
		else
		{
			Debug.Log("User not Authenticated");
			return false;
		}
#else
		return false;
#endif
	}	
	public void ShowAchievementUI()
	{
#if UNITY_IPHONE
		if(IsUserAuthenticated())
		{
			Social.ShowAchievementsUI();
		}
#endif
	}
	public void ShowLeaderboardUI()
	{
#if UNITY_IPHONE
		if(IsUserAuthenticated())
		{
			Social.ShowLeaderboardUI();
		}
#endif
	}	
	public bool AddAchievementProgress(string achievementID, float percentageToAdd)
	{
#if UNITY_IPHONE
		IAchievement a = GetAchievement(achievementID);
		if(a != null)
		{
			return ReportAchievementProgress(achievementID, ((float)a.percentCompleted + percentageToAdd));
		}
		else
		{
			return ReportAchievementProgress(achievementID, percentageToAdd);
		}
#else
		return false;
#endif
	}	
	public bool ReportAchievementProgress(string achievementID, float progressCompleted)
	{
#if UNITY_IPHONE
		if(Social.localUser.authenticated)
		{
			if(!IsAchievementComplete(achievementID))
			{
				bool success = false;
				Social.ReportProgress(achievementID, progressCompleted, result => 
				{
		    		if (result)
					{
						success = true;
						LoadAchievements();
		        		Debug.Log ("Successfully reported progress");
					}
		    		else
					{
						success = false;
		        		Debug.Log ("Failed to report progress");
					}
				});
				
				return success;
			}
			else
			{
				return true;	
			}
		}
		else
		{
			Debug.Log("ERROR: GameCenter user not authenticated");
			return false;
		}
#else
		return false;
#endif
	}
	
	public void ResetAchievements()
	{
#if UNITY_IPHONE
		GameCenterPlatform.ResetAllAchievements(ResetAchievementsHandler);	
#endif
	}
	
	void LoadAchievements()
	{
		Social.LoadAchievements (ProcessLoadedAchievements);
	}
	void ProcessAuthentication(bool success)
	{
		if(success)
		{
			Debug.Log ("Authenticated, checking achievements");

            LoadAchievements();
			GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);	
		}
		else
		{
			Debug.Log ("Failed to authenticate");
		}
	}	
	void ProcessLoadedAchievements (IAchievement[] achievements) 
	{
		//Clear the list
		if(this.achievements != null)
		{
			this.achievements = null;	
		}
		
        if (achievements.Length == 0)
		{
            Debug.Log ("Error: no achievements found");
		}
        else
		{
            Debug.Log ("Got " + achievements.Length + " achievements");
			this.achievements = achievements;
		}
	}
	bool IsAchievementComplete(string achievementID)
	{
		if(achievements != null)
		{
			foreach(IAchievement a in achievements)
			{
				if(a.id == achievementID && a.completed)
				{
					return true;	
				}
			}
		}
		
		return false;
	}
	IAchievement GetAchievement(string achievementID)
	{
		if(achievements != null)
		{
			foreach(IAchievement a in achievements)
			{
				if(a.id == achievementID)
				{
					return a;	
				}
			}
		}
		return null;
	}
	void ResetAchievementsHandler(bool status)
	{
		if(status)
		{
			//Clear the list
			if(this.achievements != null)
			{
				this.achievements = null;	
			}
			
			LoadAchievements();
			
			Debug.Log("Achievements successfully resetted!");
		}
		else
		{
			Debug.Log("Achievements reset failure!");
		}
	}
}

