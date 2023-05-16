using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public string sceneName;
    public GameObject player;
    public GameObject ForHP;

    public void Awake()
    {
        ForHP = GameObject.Find("ForHidingPurposes");
    }


    [SerializeField] RectTransform transition;
    private void OnTriggerEnter(Collider col)
    {
        if(col.name == "Player")
        {
            OpenCombatScene();
        }
    }

    public void OpenCombatScene()
    {
        transition.gameObject.SetActive(true);
        LeanTween.scale(transition, new Vector3(1, 1, 1), 0);
        LeanTween.scale(transition, Vector3.zero, 0.9f).setEase(LeanTweenType.easeInOutExpo).setOnComplete(() =>
        {
            transition.gameObject.SetActive(false);
           // Debug.Log("forHP >" + "[" + ForHP+ "]");
            ForHP.SetActive(false);
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
        });
        
    }

    public void OpenDungeonScene()
    {
        SceneManager.UnloadSceneAsync(1);
        transition.gameObject.SetActive(true);
        LeanTween.scale(transition, new Vector3(1, 1, 1), 0);
        LeanTween.scale(transition, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInOutExpo).setOnComplete(() =>
        {
            transition.gameObject.SetActive(false);
        });
        SceneManager.LoadScene(1);
    }
}
