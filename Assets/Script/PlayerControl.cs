using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;

public class PlayerControl : MonoBehaviourPunCallbacks, IPunObservable
{
    GameObject hpGauge;

    public Transform firePoint_1;
    public Transform firePoint_2;
    public Transform laserPoint;
    
    GameObject bulletCount;
    GameObject RunCount;
    GameObject dyText;
    GameObject shotgunCooltimeText;

    float speed;
    Rigidbody2D rb;
    SpriteRenderer spr;
    GameObject came;
    Camera cam;
    PhotonView pv;

    Vector2 movement;
    Vector2 mousePos;
    Vector2 lookdir;
    float shootingTerm = 0f;
    float angle;
    int hp;
    public static bool alive;
    bool i = false;
    int mainBullet;
    byte weaponState;

    AudioSource speaker;
    public AudioClip Gun_Sound_1;
    public AudioClip pg;
    public AudioClip ReloadSound;
    public AudioClip DYS;
    public AudioClip shotgunSound;
    public AudioClip charging;
    public AudioClip bigLasor;
    float RealodTime;

    Vector3 networkPosition;
    float networkRotation;
    bool reloadYuboo;
    float runTime;
    float RightClickCooldown;
    bool runTimeReL;
    bool danbalYeonsa;
    bool shotCooldown;
    float poCount;
    bool shootLaserCheck;
    bool iCan;

    public static string killer;
    public Text nickName;
    public Image TeamColor;

    void Start()
    {
        speed = 7f;
        rb = gameObject.GetComponent<Rigidbody2D>();
        came = GameObject.Find("Main Camera");
        cam = came.GetComponent<Camera>();
        speaker = GetComponent<AudioSource>();
        spr = GetComponent<SpriteRenderer>();
        hpGauge = GameObject.Find("hpGauge");
        hpGauge.GetComponent<Image>().fillAmount = 1;
        hpGauge.GetComponent<Image>().fillAmount = 1;
        hp = 200;
        if (byeonsooDirector.character == 1)
        {
            mainBullet = 20;
            RightClickCooldown = 5;
        }
        else if (byeonsooDirector.character == 2)
        {
            mainBullet = 35;
            RightClickCooldown = 15;
        }else if (byeonsooDirector.character == 3)
        {
            mainBullet = 4;
        }
        bulletCount = GameObject.Find("bulletCount_1");
        RunCount = GameObject.Find("skill_1TimeCounter");
        dyText = GameObject.Find("DYcheck");
        shotgunCooltimeText = GameObject.Find("skill_2TimeCounter");
        pv = GetComponent<PhotonView>();
        alive = true;
        RealodTime = 0;
        reloadYuboo = false;
        runTime = 3;
        danbalYeonsa = false;
        shotCooldown = false;
        poCount = 0; shootLaserCheck = false; iCan = true; weaponState = 0;
        if (pv.IsMine && PhotonNetwork.LocalPlayer.GetTeam() == PunTeams.Team.red)
        {
            pv.RPC("addTagR", RpcTarget.AllBuffered);
            pv.RPC("GetRColor", RpcTarget.AllBuffered);
        }
        else if (pv.IsMine && PhotonNetwork.LocalPlayer.GetTeam() == PunTeams.Team.blue)
        {
            pv.RPC("addTagB", RpcTarget.AllBuffered);
            pv.RPC("GetBColor", RpcTarget.AllBuffered);
        }
    }

    void Update()
    {
        if (pv.IsMine)
        {
            if (alive == true)
            {
                if (iCan == true) {
                    movement.x = Input.GetAxisRaw("Horizontal");
                    movement.y = Input.GetAxisRaw("Vertical");

                    mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                }
                else movement = new Vector2(0,0);
                if (byeonsooDirector.character == 1)//ugle
                {
                    if (Input.GetKeyDown(KeyCode.B))
                    {
                        pv.RPC("playDYS", RpcTarget.AllBuffered);
                        if (danbalYeonsa == false)
                            danbalYeonsa = true;
                        else
                            danbalYeonsa = false;
                    }

                    if (Input.GetMouseButtonDown(1) && RightClickCooldown >= 5)
                    {
                        shotGun("bullet_1");
                        RightClickCooldown = 0;
                        shotCooldown = true;
                    }
                    if (shotCooldown && RightClickCooldown < 5)
                        RightClickCooldown += Time.deltaTime;
                    else if (shotCooldown)
                        RightClickCooldown = 5;

                    if (Input.GetMouseButtonDown(0) && mainBullet > 0 && danbalYeonsa == false)
                        shooting();

                    if (Input.GetMouseButton(0) && mainBullet > 0 && danbalYeonsa == true)
                    {
                        shootingTerm += Time.deltaTime;
                        if (shootingTerm > 0.1f)
                        {
                            shooting();
                            shootingTerm = 0;
                        }
                    }

                    if (Input.GetKey(KeyCode.LeftShift) && runTime > 0)
                    {
                        speed = 12f;
                        runTime -= Time.deltaTime;
                        runTimeReL = false;
                        if (runTime <= 0)
                            speed = 7f;
                    }
                    if (Input.GetKeyUp(KeyCode.LeftShift))
                    {
                        speed = 7f;
                        runTimeReL = true;
                    }

                    if (runTime < 3 && runTimeReL == true)
                        runTime += Time.deltaTime;
                    else if (runTimeReL == true)
                        runTime = 3f;

                    if (Input.GetKeyDown(KeyCode.R))
                        reloadYuboo = true;
                    if (mainBullet == 0 || reloadYuboo == true)
                    {
                        if (RealodTime == 0)
                            pv.RPC("playReloadSound", RpcTarget.AllBuffered);
                        RealodTime += Time.deltaTime;
                        if (RealodTime >= 2f)
                        {
                            mainBullet = 20;
                            RealodTime = 0;
                            reloadYuboo = false;
                        }
                    }
                }else if (byeonsooDirector.character == 2)//eire
                {
                    if (Input.GetKeyDown(KeyCode.B))
                    {
                        pv.RPC("playDYS", RpcTarget.AllBuffered);
                        if (danbalYeonsa == false)
                            danbalYeonsa = true;
                        else
                            danbalYeonsa = false;
                    }

                    if (Input.GetMouseButtonDown(1) && RightClickCooldown >= 15)
                    {
                        RightClickCooldown = 0;
                        PhotonNetwork.Instantiate("power-up-1", firePoint_2.position, firePoint_2.rotation);
                        pv.RPC("playCharging",RpcTarget.AllBuffered);
                        shootLaserCheck = true;
                        iCan = false;
                        speed = 0;
                    }
                    if (poCount >= 1)
                    {
                        PhotonNetwork.Instantiate("bigLaser_1", laserPoint.position, laserPoint.rotation);
                        poCount = 0;
                        shotCooldown = true;
                        shootLaserCheck = false;
                        iCan = true;
                        speed = 7;
                    }
                    if (shootLaserCheck == true)
                        poCount += Time.deltaTime;

                    if (shotCooldown && RightClickCooldown < 15)
                        RightClickCooldown += Time.deltaTime;
                    else if (shotCooldown)
                        RightClickCooldown = 15;

                    if (Input.GetMouseButtonDown(0) && mainBullet > 0 && danbalYeonsa == false)
                        LaserShoting();

                    if (Input.GetMouseButton(0) && mainBullet > 0 && danbalYeonsa == true)
                    {
                        shootingTerm += Time.deltaTime;
                        if (shootingTerm > 0.08f)
                        {
                            LaserShoting();
                            shootingTerm = 0;
                        }
                    }

                    if (Input.GetKey(KeyCode.LeftShift) && runTime > 0)
                    {
                        speed = 12f;
                        runTime -= Time.deltaTime;
                        runTimeReL = false;
                        if (runTime <= 0)
                            speed = 7f;
                    }
                    if (Input.GetKeyUp(KeyCode.LeftShift))
                    {
                        speed = 7f;
                        runTimeReL = true;
                    }

                    if (runTime < 3 && runTimeReL == true)
                        runTime += Time.deltaTime;
                    else if (runTimeReL == true)
                        runTime = 3f;

                    if (Input.GetKeyDown(KeyCode.R))
                        reloadYuboo = true;
                    if (mainBullet == 0 || reloadYuboo == true)
                    {
                        if (RealodTime == 0)
                            pv.RPC("playReloadSound", RpcTarget.AllBuffered);
                        RealodTime += Time.deltaTime;
                        if (RealodTime >= 1.45f)
                        {
                            mainBullet = 35;
                            RealodTime = 0;
                            reloadYuboo = false;
                        }
                    }
                }else if (byeonsooDirector.character == 3)//shielder
                {
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        weaponState = 0;
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        weaponState = 1;
                    }
                    if (Input.GetMouseButtonDown(0) && weaponState == 1) {
                        Debug.Log("stub!");
                    }
                    if (Input.GetMouseButtonDown(0) && weaponState == 0)
                    {
                        shotGun("");//총알 딴거로 하자
                    }
                }
                hpGauge.GetComponent<Image>().fillAmount = (float)hp/200;
            }
            if (hp <= 0)
            {
                alive = false;
                if (i == false)
                {
                    PhotonNetwork.Instantiate("boom", transform.position, Quaternion.identity);
                    i = true;
                }
                pv.RPC("DestroyG", RpcTarget.AllBuffered);
            }
            else
                alive = true;
            came.transform.position = new Vector3(transform.position.x, transform.position.y, -10);

            bulletCount.GetComponent<Text>().text = "bullet: " + mainBullet;
            RunCount.GetComponent<Text>().text = "Run " + runTime.ToString("N1");
            if (danbalYeonsa == true) dyText.GetComponent<Text>().text = "연사";
            else dyText.GetComponent<Text>().text = "단발";
            if (byeonsooDirector.character == 1)
            {
                if (RightClickCooldown != 5)
                    shotgunCooltimeText.GetComponent<Text>().text = "샷건 : " + (5 - RightClickCooldown).ToString("N1");
                else
                    shotgunCooltimeText.GetComponent<Text>().text = "샷건 : 사용가능";
            }
            else if (byeonsooDirector.character == 2)
            {
                if (RightClickCooldown != 15)
                    shotgunCooltimeText.GetComponent<Text>().text = "레이저포 : " + (15 - RightClickCooldown).ToString("N1");
                else
                    shotgunCooltimeText.GetComponent<Text>().text = "레이저포 : 사용가능";
            }
        }
        nickName.text = pv.Owner.NickName;
        if (!pv.IsMine)
            rb.position = Vector3.MoveTowards(rb.position, networkPosition, Time.fixedDeltaTime);
    }
    void FixedUpdate()
    {
        if (pv.IsMine || alive == true)
        {
            rb.position = rb.position + movement * speed * Time.fixedDeltaTime;
            pv.RPC("SeeMouse", RpcTarget.AllBuffered);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (pv.IsMine&&collision.gameObject.tag == "bullet")
        {
            hp -= 18;
            pv.RPC("playDestrotSound", RpcTarget.AllBuffered);
            killer = collision.gameObject.GetComponent<bullet_1>()._playerId;
        }else if (pv.IsMine && collision.gameObject.tag == "laser")
        {
            hp -= 9;
            pv.RPC("playDestrotSound", RpcTarget.AllBuffered);
            killer = collision.gameObject.GetComponent<laserBullet>()._playerId;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pv.IsMine && collision.gameObject.tag == "bullet")
        {
            hp -= 18;
            pv.RPC("playDestrotSound", RpcTarget.AllBuffered);
            killer = collision.gameObject.GetComponent<bullet_1>()._playerId;
        }
        else if (pv.IsMine && collision.gameObject.tag == "laser")
        {
            hp -= 9;
            pv.RPC("playDestrotSound", RpcTarget.AllBuffered);
            killer = collision.gameObject.GetComponent<laserBullet>()._playerId;
        }else if (pv.IsMine && collision.gameObject.tag == "bigLaser")//트리거만 있음
        {
            hp -= 100;
            pv.RPC("playDestrotSound", RpcTarget.AllBuffered);
            killer = collision.gameObject.GetComponent<laserBullet>()._playerId;
        }
    }
    void shooting()//ugle
    {
        GameObject bullet = PhotonNetwork.Instantiate("bullet_1", firePoint_1.position, firePoint_1.rotation);
        Rigidbody2D bb = bullet.GetComponent<Rigidbody2D>();
        bullet.GetComponent<bullet_1>()._playerId = PhotonNetwork.LocalPlayer.NickName;
        bb.AddForce(firePoint_1.up * 30f, ForceMode2D.Impulse);
        mainBullet -= 1;
        pv.RPC("playMainGunSound", RpcTarget.AllBuffered);
    }
    void LaserShoting()//eire
    {
        GameObject bullet = PhotonNetwork.Instantiate("LaserBullet", firePoint_1.position, firePoint_1.rotation);
        Rigidbody2D bb = bullet.GetComponent<Rigidbody2D>();
        bullet.GetComponent<laserBullet>()._playerId = PhotonNetwork.LocalPlayer.NickName;
        bb.AddForce(firePoint_1.up * 50f, ForceMode2D.Impulse);
        mainBullet -= 1;
        pv.RPC("playMainGunSound", RpcTarget.AllBuffered);
    }
    void shotGun(string bName)//ugle
    {
        GameObject[] bullet = new GameObject[5];
        Rigidbody2D[] bb = new Rigidbody2D[5];
        float direction = -1f;
        for (int i = 0; i < 5; i++)
        {
            bullet[i] = PhotonNetwork.Instantiate(bName, firePoint_2.position, firePoint_2.rotation);
            bb[i] = bullet[i].GetComponent<Rigidbody2D>();
            bullet[i].GetComponent<bullet_1>()._playerId = PhotonNetwork.LocalPlayer.NickName;
            switch (i)
            {
                case 0:
                    bb[i].AddForce((firePoint_2.up + firePoint_2.right * -1) * 30f, ForceMode2D.Impulse);
                    break;
                case 1:
                    bb[i].AddForce((firePoint_2.up + firePoint_2.right * -0.5f) * 30f, ForceMode2D.Impulse);
                    break;
                case 2:
                    bb[i].AddForce((firePoint_2.up) * 30f, ForceMode2D.Impulse);
                    break;
                case 3:
                    bb[i].AddForce((firePoint_2.up + firePoint_2.right * 0.5f) * 30f, ForceMode2D.Impulse);
                    break;
                case 4:
                    bb[i].AddForce((firePoint_2.up + firePoint_2.right) * 30f, ForceMode2D.Impulse);
                    break;
            }
            pv.RPC("playShG", RpcTarget.AllBuffered);
            direction += 0.5f;
        }
    }
    void playSound(AudioSource source, AudioClip clp)
    {
        source.clip = clp;
        source.Play();
    }
    #region punRPC
    [PunRPC]
    void addTagR() => gameObject.tag = "rTeam";
    [PunRPC]
    void addTagB() => gameObject.tag = "bTeam";
    [PunRPC]
    void GetRColor() => TeamColor.color = Color.red;
    [PunRPC]
    void GetBColor() => TeamColor.color = Color.blue;
    [PunRPC]
    void DestroyG() => Destroy(gameObject);
    //sound
    [PunRPC]
    void playMainGunSound() => playSound(speaker,Gun_Sound_1);
    [PunRPC]
    void playDestrotSound() => playSound(speaker,pg);
    [PunRPC]
    void playReloadSound() => playSound(speaker, ReloadSound);
    [PunRPC]
    void playDYS() => playSound(speaker, DYS);
    [PunRPC]
    void playShG() => playSound(speaker, shotgunSound);
    [PunRPC]
    void playCharging() => playSound(speaker,charging);
    //movement and reward for delay
    [PunRPC]
    void SeeMouse()
    {
        lookdir = mousePos - rb.position;
        angle = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
    [PunRPC]
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rb.position);
            stream.SendNext(rb.rotation);
            stream.SendNext(hp);
        }
        else
        {
            rb.position = (Vector3)stream.ReceiveNext();
            rb.rotation = (float)stream.ReceiveNext();
            hp = (int)stream.ReceiveNext();
        }
    }
    #endregion
}
