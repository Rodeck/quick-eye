using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    [SerializeField]
    Image courtine;

    [SerializeField]
    [Range(0.0f, 2.0f)]
    float fadeTime = 1.0f;

    [SerializeField]
    [Range(0.001f, 1.0f)]
    float fadeStep = 0.1f;

	public void StartGame()
    {
        StartCoroutine(Fade());
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    IEnumerator Fade()
    {
        courtine.gameObject.SetActive(true);

        while(courtine.color.a <= 1.0f)
        {
            Color imageColor = courtine.color;
            imageColor.a += fadeStep;
            courtine.color = imageColor;
            yield return new WaitForSeconds(fadeTime * fadeStep);
        }


    }
}
