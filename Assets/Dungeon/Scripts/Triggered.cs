using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Triggered : MonoBehaviour
{
    public GameObject _StartTransition;
    public GameObject _EndTransition;

    public string sceneName;

    private void Start()
    {
      

    }


    private void OnTriggerEnter(Collider other)
    {

        _StartTransition.SetActive(true);
        new WaitForSecondsRealtime(5f);
        _StartTransition.SetActive(false);
        _EndTransition.SetActive(true);
        new WaitForSecondsRealtime(5f);
        _EndTransition.SetActive(false);

        Debug.Log("Funguju?");

        SceneManager.LoadScene(sceneName);
    }
}
