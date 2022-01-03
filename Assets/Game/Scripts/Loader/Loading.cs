using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{    
    [SerializeField] private Button buttonPlay;

    AsyncOperation async;

    private void Start()
    {
        StartCoroutine(LaunchLevel());
        buttonPlay.gameObject.SetActive(false);
    }

    IEnumerator LaunchLevel()
    {
        async = SceneManager.LoadSceneAsync(1);
        async.allowSceneActivation = false;

        while (async.progress != 0.9f)
        {           
            yield return new WaitForSeconds(1.01f);
        }
       
        buttonPlay.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.anyKey && async.progress == 0.9f)
        {
            async.allowSceneActivation = true;
        }
    }

}
