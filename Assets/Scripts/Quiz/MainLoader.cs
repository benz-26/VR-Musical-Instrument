using System.Collections;
using UnityEngine;

public class MainLoader : MonoBehaviour
{
    public GameObject LoadingMenuPanel;
    public static bool Loading = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public void Awake()
    {
        LoadingMenuPanel.SetActive(true);
        LoadingStart();
    }

    public void LoadingStart()
    {
        if (Loading == false)
        {
            StartCoroutine(QuestionChecker());
            Loading = true;
        }
        else if (Loading == true)
        {
            LoadingMenuPanel.SetActive(false);
            Loading = true;
        }
    }

    IEnumerator QuestionChecker()
    {
        yield return new WaitForSeconds(5f);
        LoadingMenuPanel.SetActive(false);
    }
}
