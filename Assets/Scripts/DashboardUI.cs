using TMPro;
using UnityEngine;

public class DashboardUI : MonoBehaviour
{
   public TMP_Text flowerCountText;
   public TMP_Text reactionTimeText;
   public TMP_Text bugExperimentsText;
   public TMP_Text bugTrappedText;
   public TMP_Text bugEscapeText;
   
   public TMP_Dropdown levelDropdown;

   private void Start()
   {
      UpdateDashboard();

      if (levelDropdown != null)
      {
         levelDropdown.onValueChanged.AddListener(OnLevelChanged);
      }
   }

   private void OnEnable()
   {
      UpdateDashboard();
   }

   private void UpdateDashboard()
   {

      if (JasonDataLoader.instance == null)
      {
         Debug.LogWarning("Jason Data Loader is missing");
         return;
      }
      AppData currentData = JasonDataLoader.instance != null ? JasonDataLoader.instance.appData : null;
      if (currentData == null)
      {
         Debug.Log("No data loaded");
         return;
      }

      if (currentData.game != null && currentData.game.Count > 0)
      {
         Debug.Log($"Current game: {currentData.game.Count}");
         if(flowerCountText != null)
         {
              flowerCountText.text = $"Final flowers: {currentData.game[0].flowerCount}";
         }
      }

      if (currentData.mias != null && currentData.mias.Count > 0)
      {
         Debug.Log($"Current mias: {currentData.mias.Count}");
         MiasData mia = currentData.mias[0];

         if (reactionTimeText != null)
         {
            reactionTimeText.text = $"Reaction time: {mia.reactionTime}";
         }

         if (bugExperimentsText != null)
         {
            bugExperimentsText.text = $"Bug experiments: {mia.bugExperiments}";
         }

         if (bugTrappedText != null)
         {
            bugTrappedText.text = $"Trapped: {mia.bugTrapped}";
         }

         if (bugEscapeText != null)
         {
            bugEscapeText.text = $"Escape: {mia.bugEscape}";
         }
      }
   }

   private void OnLevelChanged(int level)
   {
      if (JasonDataLoader.instance == null || JasonDataLoader.instance.appData.game.Count == 0)
      {
         GameData data = JasonDataLoader.instance.appData.game[0];

         switch (level)
         {
            case 0: data.meadows = new[] { 1,3,4 };
               break;
            case 1: data.meadows = new[] { 4,3,1 };
               break;
            case 2: data.meadows = new[] { 3,4,1 };
               break;
            
         }
         
         data.currentLevelIndex = 1;
         JasonDataLoader.instance.SaveData();
         Debug.Log("new Lavel stuff");
      }
   }
    
}
