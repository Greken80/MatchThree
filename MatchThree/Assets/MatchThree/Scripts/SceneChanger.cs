using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChanger : MonoBehaviour
{

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }


    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }


    public void ChangeSceneAsync(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }

    public void ChangeSceneAsync(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
    }


}
