using UnityEngine;
using System.Collections;

using UnityEngine.UI;
public class Game : MonoBehaviour
{
   public Camera mainCamera;
   public float zoomedIn = 2.5f;
   private float defaultCameraSize;
   private Vector3 defaultCameraPosition;

   public GameObject colorPaletteUI;
   public Image targetImage;
   public Sprite flowerTarget;

   private Transform currentFlower;

   private void Start()
   {
      defaultCameraSize =  mainCamera.orthographicSize;
      defaultCameraPosition = mainCamera.transform.position;

      if (colorPaletteUI)
      {
         colorPaletteUI.SetActive(false);
      }

      if (targetImage)
      {
         targetImage.gameObject.SetActive(false);
      }
      
   }

   public void SelectFlower(Transform flower, Sprite targetPreview)
   {
      currentFlower = flower;
      StartCoroutine(Zoom(targetPreview));
   }

   private IEnumerator Zoom(Sprite targetPreview)
   {
      Vector3 targetPos = new Vector3(currentFlower.position.x, currentFlower.position.y, mainCamera.transform.position.z);
      float elapsedTime = 0f;
      float duration = 0.5f;

      while (elapsedTime < duration)
      {
         mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, elapsedTime / duration);
         mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomedIn, elapsedTime / duration);
         elapsedTime += Time.deltaTime;
         yield return null;
         
      }
      mainCamera.transform.position = targetPos;
      mainCamera.orthographicSize = zoomedIn;

      if (targetImage != null && targetPreview != null)
      {
         targetImage.sprite = targetPreview;
         targetImage.gameObject.SetActive(true);
         
         yield return new WaitForSeconds(2.0f);
         targetImage.gameObject.SetActive(false);
      }

      if (colorPaletteUI)
      {
         colorPaletteUI.SetActive(true);
      }
      
   }


}
