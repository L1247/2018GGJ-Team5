using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ChangeTo(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
