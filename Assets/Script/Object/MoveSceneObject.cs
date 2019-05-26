using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveSceneObject : MonoBehaviour
{
    #region[잡다변수]
    [Header("다음 씬 이름")]
    public string scene_name;

    bool go = false;
    #endregion

    #region[씬이동]
    public void MoveScene()
    {
        GameManager.fadeout = false;
        go = true;
        GameManager.time = 0;
    }
    #endregion

    private void Update()
    {
        if (go && !GameManager.fadeout)
        {
            if (!GameManager.Static_Fade)
                SceneManager.LoadScene(scene_name);
            else if(GameManager.Static_Fade && GameManager.Static_Fade.color.a == 1)
                SceneManager.LoadScene(scene_name);
        }
    }
}
