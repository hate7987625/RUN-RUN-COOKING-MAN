using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Image imageLoad;
    public Text textLoad;
    public Text anyKey;
    public void ReStart()
    {
        SceneManager.LoadScene("關卡1");
      
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void StartLoad()
    {
        StartCoroutine(Loading());

    }
    public IEnumerator Loading()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync("關卡1");
        ao.allowSceneActivation = false; //就算載入完成，先停留在原先場景

        while (ao.isDone == false)
        {
            textLoad.text = ao.progress/0.9f*100 + "/100";
            imageLoad.fillAmount = ao.progress / 0.9f;
            yield return null;
            if (ao.progress==0.9f)
            {
                anyKey.text = "請按任意鍵...";
            }
            if (ao.progress == 0.9f && Input.anyKey)
            {
                
                ao.allowSceneActivation = true;

            }

        }
       
       
    }

}
