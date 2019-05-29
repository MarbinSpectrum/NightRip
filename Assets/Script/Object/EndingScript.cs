using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScript : MonoBehaviour
{
    [HideInInspector]
    public bool end_start = false;

    public Animator startAni;

    public void EnddingStart()
    {
        StartCoroutine(End_Trigger());
    }

    public IEnumerator End_Trigger()
    {
        GameManager.Player_UI.SetActive(false);
        yield return new WaitForSeconds(2);
        StageManager.player_static.GetComponent<Player_Maker>().animator.SetTrigger("Pray");
        SoundManager.OffBGM();
        SoundManager.LoadZoneBGM(true);
        yield return new WaitForSeconds(2);
        startAni.SetTrigger("Start");
        yield return new WaitForSeconds(10);
        GameManager.time = 0;
        GameManager.fadeout = false;
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Ending");
    }
}
