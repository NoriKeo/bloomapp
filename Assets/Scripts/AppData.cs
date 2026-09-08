using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[System.Serializable]
  public class UserData
  {
    public string username;
    public string password;
  }
  
  [System.Serializable]
  public class MiasData
  {
    public string timestamp;
    public int reactionTime;
    public int bugExperiments;
    public int bugTrapped;
    public int bugEscape;

  }
  
  [System.Serializable]
  public class GameData
  {
    public int flowerCount;
    public int[] meadows = new int[]{1,3,4};
    [FormerlySerializedAs("currentLevel")] public int currentLevelIndex = 1;

  }
  
  [System.Serializable]
  public class AppData
  {
    public List<UserData> users = new List<UserData>();
    public List<MiasData> mias = new List<MiasData>();
    public List<GameData> game = new List<GameData>();
  }

