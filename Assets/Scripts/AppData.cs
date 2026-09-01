using System;
using System.Collections.Generic;
using UnityEngine;


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
    public int[] meadows;
    
  }
  
  [System.Serializable]
  public class AppData
  {
    public List<UserData> users = new List<UserData>();
    public List<MiasData> mias = new List<MiasData>();
    public List<GameData> game = new List<GameData>();
  }

