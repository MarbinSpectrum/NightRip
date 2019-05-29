using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingSceneScript : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(End_Trigger());
    }

    public IEnumerator End_Trigger()
    {
        //SoundManager.OffBGM();
        //SoundManager.EndBGM(true);
        yield return new WaitForSeconds(72);
        SoundManager.OffBGM();
        GameManager.time = 0;
        GameManager.fadeout = true;
        SceneManager.LoadScene("Title");
    }
}
