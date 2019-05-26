using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Camera_Maker : MonoBehaviour
{
    #region[잡다변수]
    [HideInInspector] public List<Vector3> camera_pos = new List<Vector3>() { new Vector3(+1,+1,0), new Vector3(+1, -1, 0) , new Vector3(-1, -1, 0), new Vector3(-1, +1, 0) };
    [HideInInspector] public bool awake = false;
    [HideInInspector] public bool showpoints = false;
    [HideInInspector] public Vector3 camera_pivot = Vector3.zero;
    [HideInInspector] public Transform default_camera_pos;
    [HideInInspector] public Transform goal_obj;
    [HideInInspector] public float camera_speed;

    #endregion

    #region[Update]
    void Update()
    {
        FollowCamera();
    }
    #endregion

    #region[OnDrawGizmos]
    private void OnDrawGizmos()
    {
        default_camera_pos = transform;
        #region[카메라 경로 보여주기]
        for (int i = 0; i < camera_pos.Count + 1; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(camera_pos[i % camera_pos.Count], camera_pos[(i + 1) % camera_pos.Count]);
        }
        #endregion

    }
    #endregion

    #region[폴리곤안에 있는 점 여부 검사 알고리즘]
    bool IsInside(Vector3 B, List<Vector3> polygon)
    {
        int crosses = 0;
        for (int i = 0; i < polygon.Count; i++)
        {
            int j = (i + 1) % polygon.Count;
            if ((polygon[i].y > B.y) != (polygon[j].y > B.y))
            {
                double atX = (polygon[j].x - polygon[i].x) * (B.y - polygon[i].y) / (polygon[j].y - polygon[i].y) + polygon[i].x;
                if (B.x < atX)
                    crosses++;
            }
        }
        return crosses % 2 > 0;
    }
    #endregion

    #region[직선의 교차점 알고리즘]
    //x1은 가장가까운점 , x2는 x1옆의 점 , x3는 플레이어좌표 
    Vector3 crosspos(Vector3 x1, Vector3 x2, Vector3 x3, Vector3 x4)
    {
        float fIncrease1 = 0;
        float fConstant1 = 0;
        float fSameValue1 = 0;
        float fIncrease2 = 0;
        float fConstant2 = 0;
        float fSameValue2 = 0;

        if (x1.x == x2.x)
            fSameValue1 = x1.x;
        else
        {
            fIncrease1 = (float)(x2.y - x1.y) / (x2.x - x1.x);
            fConstant1 = x1.y - fIncrease1 * x1.x;
        }

        if (x3.x == x4.x)
            fSameValue2 = x3.x;
        else
        {
            fIncrease2 = (float)(x4.y - x3.y) / (x4.x - x3.x);
            fConstant2 = x3.y - fIncrease2 * x3.x;
        }
        Vector3 pResult = x1;
        if (x1.x == x2.x && x3.x == x4.x)
        {
            pResult = x1;
        }
        if (x1.x == x2.x)
        {
            pResult = new Vector3(fSameValue1, fIncrease2 * fSameValue1 + fConstant2, x1.z);
        }
        else if (x3.x == x4.x)
        {
            pResult = new Vector3(fSameValue2, fIncrease1 * fSameValue2 + fConstant1, x1.z);
        }
        else
        {
            pResult = x1;
            pResult.x = -(fConstant1 - fConstant2) / (fIncrease1 - fIncrease2);
            pResult.y = fIncrease1 * pResult.x + fConstant1;
        }
       
        if(x1.x > x2.x)
        {
            if(pResult.x > x1.x || pResult.x < x2.x)
            {
                pResult = x1;
            }
        }
        else
        {
            if (pResult.x > x2.x || pResult.x < x1.x)
            {
                pResult = x1;
            }
        }

        if (x1.y > x2.y)
        {
            if (pResult.y > x1.y || pResult.y < x2.y)
            {
                pResult = x1;
            }
        }
        else
        {
            if (pResult.y > x2.y || pResult.y < x1.y)
            {
                pResult = x1;
            }
        }
        return pResult;
    }
    #endregion

    #region[목표오브젝트 추적]
    void FollowCamera()
    {
        if(IsInside(goal_obj.position, camera_pos))
        {
            if(camera_speed != 0)
            {
                Vector3 dic = goal_obj.position - (default_camera_pos.position - camera_pivot);
                dic = Vector3.Normalize(dic);
                default_camera_pos.position += new Vector3(dic.x, dic.y, 0) * camera_speed;
            }
            else
            {
                default_camera_pos.position = new Vector3(goal_obj.position.x, goal_obj.position.y, default_camera_pos.position.z);
            }
        }
        else
        {
            Vector3 dic = Vector3.zero;
            Vector3 goal = Vector3.zero;
            float min_length = Mathf.Infinity;
            for (int i = 0; i < camera_pos.Count; i++)
            {
                dic = camera_pos[i% camera_pos.Count] - camera_pos[(i + 1)% camera_pos.Count];
                dic = new Vector3(dic.y, -dic.x, 0);

                if (min_length > Vector3.Distance(goal_obj.position, crosspos(camera_pos[i % camera_pos.Count], camera_pos[(i + 1) % camera_pos.Count], goal_obj.position, goal_obj.position + dic)))
                {
                    min_length = Vector3.Distance(goal_obj.position, crosspos(camera_pos[i % camera_pos.Count], camera_pos[(i + 1) % camera_pos.Count], goal_obj.position, goal_obj.position + dic));
                    goal = crosspos(camera_pos[i % camera_pos.Count], camera_pos[(i + 1) % camera_pos.Count], goal_obj.position, goal_obj.position + dic);
                }
            }
            for (int i = 0; i < camera_pos.Count; i++)
            {
                if (min_length > Vector3.Distance(goal_obj.position, camera_pos[i % camera_pos.Count]))
                {
                    min_length = Vector3.Distance(goal_obj.position, camera_pos[i % camera_pos.Count]);
                    goal = camera_pos[i % camera_pos.Count];
                }
            }

            if (camera_speed != 0)
            {
                dic = goal - (default_camera_pos.position - camera_pivot);
                dic = Vector3.Normalize(dic);
                default_camera_pos.position += new Vector3(dic.x, dic.y, 0) * camera_speed;
            }
            else
            {
                default_camera_pos.position = new Vector3(goal.x, goal.y, default_camera_pos.position.z);
            }

        }
    }
    #endregion
}

