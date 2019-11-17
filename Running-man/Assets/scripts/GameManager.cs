using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Image imageLoad;
    public Text textLoad;
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
            textLoad.text = ao.progress + "/100";
            yield return null;
        }
    }

}
