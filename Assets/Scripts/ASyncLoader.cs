using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ASyncLoader : MonoBehaviour
{
    [SerializeField] private int _mainSceneIndex;

    private void Start()
    {
        float delay = 0.2f;
        Invoke("DelayedLoadLevel", delay);
    }

    private void DelayedLoadLevel()
    {
        LoadLevelASync(_mainSceneIndex);
    }
    
    private void LoadLevelASync(int indexOfLevel)
    {
        SceneManager.LoadSceneAsync(indexOfLevel);
    }
}
