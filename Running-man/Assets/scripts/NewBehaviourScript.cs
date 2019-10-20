using System;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    #region
    [Header("跳躍次數"),Range(1,10),Tooltip("角色可以跳躍的次數")]
    int jumpCount = 2;
    [Header("跳躍高度")]
    public int jumpHight = 100;
    [Header("是否在地板")]
    bool isGround = true;
    [Header("角色名子")]
    string player = "edie";
    [Header("角色速度"),Range(1f,10f)]
    public float speed = 0.1f;
    public Transform dog, cam;
    public Animator player_state;
    int state = 0;
    #endregion
    void Start()
    {
        
    }

    void Update()
    {
        DogMove();
        CameraMove();
        Character_state();
     
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumpCount = 0;
        state = 0;
    }
    /// <summary>
    /// 控制角色動畫(0跑步,1跳躍,2滑行)
    /// </summary>
    public void Character_state()
    {
        player_state.SetInteger("state",state);
    }
    /// <summary>
    /// 角色移動
    /// </summary>
    public void DogMove()
    {
        transform.Translate(speed*Time.deltaTime ,0,0);
       
    }
    /// <summary>
    /// 相機移動
    /// </summary>
    public void CameraMove()
    {
        cam.Translate(speed * Time.deltaTime, 0, 0);
    }
    public void DogJump()
    {
        if (jumpCount < 1)
        {
            state = 1;
            transform.Translate(0, jumpHight * Time.deltaTime*2f, 0);
            jumpCount += 1;
        }
        
    }

}
