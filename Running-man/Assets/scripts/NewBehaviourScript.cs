using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System;

public class NewBehaviourScript : MonoBehaviour
{
    #region
    [Header("跳躍次數"), Range(1, 10), Tooltip("角色可以跳躍的次數")]
    int jumpCount = 2;

    [Header("跳躍高度")]
    public int jumpHight = 100;

    [Header("是否在地板")]
    bool isGround = true;

    [Header("角色名子")]
    string player = "edie";

    [Header("角色速度"), Range(0, 10f)]
    public float speed = 0.1f;

    [Header("拼接地圖")]
    public Tilemap mapProp;

    [Header("血量")]
    public float hp = 500;
    public float hp_Max=0;

    [Header("碰撞傷害")]
    public float damage = 50;
    [Header("受傷顏色")]
    public Color col;

    [Header("扣血速度")]
    public float blood_lost = 5;

    private SpriteRenderer sp;
    private CapsuleCollider2D cap;
    public Transform cam;
    public Rigidbody2D rig;
    private Animator player_state;

    [Header("血條")]
    public Image imag;
    [Header("計算鑽石")]
    public Text tex;
    public int count_Diamond=0;
    [Header("角色Animator")]
    public int state = 0;//角色狀態
    public GameObject final;
    public Text textDiamond, textTime, textTotal;
    //public int scoreDiamond, scoreTime, scoreTotal;
    public int[] score = new int[4];
    public int gameTime;//紀錄遊戲時間
    #endregion
    void Start()
    {
        cap = GetComponent<CapsuleCollider2D>();
        player_state = GetComponent<Animator>();
        imag = GameObject.Find("血條").GetComponent<Image>();
        rig = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        
        hp_Max = hp;
       
    }

    void Update()
    {
        
        DogMove();//角色移動
        CameraMove();//相機移動
        Character_state();//播放角色動畫
        HP();//血調控制
        //Final();
    }

    #region 方法
    /// <summary>
    /// 角色碰撞控制
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.name=="地板" && state!=2)
        {
          
            jumpCount = 0;
            state = 0;
        }
       
        if (collision.gameObject.name == "道具")
        {
            
            EatFoot(collision);
        }
        if (collision.gameObject.tag == "鑽石")
        {
            EatDiamond(collision);

        }
        if (collision.gameObject.name == "死亡區域")
        {
            Dead();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damage(collision);
    }


    /// <summary>
    /// 吃道具
    /// </summary>
    private void EatFoot(Collision2D collision)
    {

        // 先取得碰到的物件拼接地圖
        Tilemap tilemap = collision.gameObject.GetComponent<Tilemap>();

        // 宣告區域三維向量
        Vector3 hitPosition = Vector3.zero;
        // 取取得碰撞資訊
        ContactPoint2D hit = collision.contacts[0];
        // 演算法：x = 法線 x / 100
        // 演算法：y = 法線 y / 100
        hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
        hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
        
        // 設定圖塊 (位置，無)
        tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
        hp += 50;
    }

    /// <summary>
    /// 控制角色動畫(0跑步,1跳躍,2滑行，4死亡)
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

    /// <summary>
    /// 角色跳躍
    /// </summary>
    public void DogJump()
    {
        if (jumpCount < 1)
        {
            state = 1;
           // transform.Translate(0, jumpHight * Time.deltaTime*2f, 0);
            jumpCount += 1;
            rig.AddForce(transform.up * jumpHight*15);
        }
        
    }
    /// <summary>
    /// 吃鑽石
    /// </summary>
    public void EatDiamond(Collision2D collision)
    {
        count_Diamond++;
        tex.text = count_Diamond.ToString();
        Destroy(collision.gameObject);
    }
  
    /// <summary>
    /// 角色滑行
    /// </summary>
    public void DogSlideDown()
    {
        Vector2 shap = new Vector2(0.9f,0.9f);
        state = 2;
        cap.size = shap;
    }
    public void DogSlideUP()
    {
        Vector2 shap = new Vector2(0.9f, 1.5f);
        state = 0;
        cap.size = shap;
    }
    /// <summary>
    /// 角色血條控制
    /// </summary>
    public void HP()
    {
        if (imag.fillAmount > 0)
        {
            hp -= Time.deltaTime * blood_lost;
            imag.fillAmount = hp / hp_Max;
        }
        else
        {
            Dead();
        }
        
    }

    public void  Render()
    {
        sp.color = Color.HSVToRGB(255, 255, 255,false);
        CancelInvoke("Render");
    }

   /* private void CancelInvoke(string v1, float v2)
    {
        throw new NotImplementedException();
    }*/

    /// <summary>
    /// 碰撞傷害
    /// </summary>
    public void Damage(Collider2D collision)
    {
        if (collision.gameObject.name == "障礙物")
        {
            hp = hp - damage;
            imag.fillAmount = hp / hp_Max;
            sp.color = col;
            Invoke("Render",0.3F);
        }
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public void Dead()
    {
        speed = 0;

        state = 4;
        Final();
    }
    /// <summary>
    /// 結束畫面
    /// </summary>
    public void Final()
    {
        gameTime = (int)Time.timeSinceLevelLoad;// 紀錄遊戲時間
        if (final.activeInHierarchy == false)
        {
            final.SetActive(true);
            StartCoroutine(other(count_Diamond,0, 100,textDiamond));
            
            StartCoroutine(other(gameTime, 1, 100, textTime));
        }
        else return;
    }

  /// <summary>
  /// 計算總分
  /// </summary>
  /// <param name="count">鑽石數量</param>
  /// <param name="score">存放分數</param>
  /// <param name="addscore">增加分數</param>
  /// <param name="text">顯示分數顯示文字</param>
  /// <returns></returns>
    private IEnumerator other(int count,int scoreIndex,int addscore,Text text)
    {
        while (count > 0)
        {
            count--;
            score[scoreIndex] += addscore;
            text.text = score[scoreIndex].ToString();
            yield return new WaitForSeconds(0.1f);

        }
       
        if (scoreIndex == 1)
        {
            score[2]=(score[0] + score[1])/100;
           StartCoroutine( other(score[2], 3, 100, textTotal));
        }
       
       
    }
    #endregion
}
