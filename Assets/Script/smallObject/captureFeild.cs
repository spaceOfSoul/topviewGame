using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class captureFeild : MonoBehaviour,IPunObservable
{
    public Text scoreSee;
    public Image[] captureMoment;
    public AudioClip gaugeIncrease;
    AudioSource speaker;
    public float[] captureScore;//durlqnxj
    public byte state;
    public float captureTime;
    public static byte winner;
    byte rMembwr, bMember;

    private void Start()
    {
        captureScore = new float[]{0, 0}; winner = 0;
        captureTime = 0;
        state = 0;
        speaker = GetComponent<AudioSource>();
        rMembwr = 0; bMember = 0;
    }
    private void Update()
    {
        if (state == 1)
        {
            captureScore[0] += Time.deltaTime;
            if (bMember>0)
                if (captureScore[0] >= 99 && captureScore[0] < 100)
                    captureScore[0] = 99;
        }
        else if (state == 2)
        {
            captureScore[1] += Time.deltaTime;
            if (rMembwr>0)
                if (captureScore[1] >= 99 && captureScore[1] < 100)
                    captureScore[1] = 99;
        }
        captureMoment[0].fillAmount = captureTime / 2f;
        captureMoment[1].fillAmount = -captureTime / 2f;
        scoreSee.text = captureScore[1].ToString("N0") + " : " + captureScore[0].ToString("N0");
        if(captureScore[0] >= 100)
        {
            winner = 1;
            captureScore[0] = 100;
        }
        else if (captureScore[1] >= 100)
        {
            winner = 2;
            captureScore[1] = 100;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "rTeam")
        {
            rMembwr += 1;
        }
        else if (collision.gameObject.tag == "bTeam")
        {
            bMember += 1;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "rTeam")
        {
            captureTime += Time.deltaTime;
            if (captureTime >= 2f)
            {
                captureTime = 2;
                state = 1;
            }
        }
        else if (collision.gameObject.tag == "bTeam")
        {
            captureTime -= Time.deltaTime;
            if (captureTime <= -2f)
            {
                captureTime = -2;
                state = 2;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "rTeam")
        {
            rMembwr -= 1;
        }else if (collision.gameObject.tag == "bTeam")
        {
            bMember -= 1;
        }
    }
    void playSound(AudioSource a, AudioClip c)
    {
        a.clip = c;
        a.Play();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(captureScore);
            stream.SendNext(state);
            stream.SendNext(captureTime);
            stream.SendNext(rMembwr);
            stream.SendNext(bMember);
        }
        else
        {
            captureScore = (float[])stream.ReceiveNext();
            state = (byte)stream.ReceiveNext();
            captureTime = (float)stream.ReceiveNext();
            rMembwr = (byte)stream.ReceiveNext();
            bMember = (byte)stream.ReceiveNext();
        }
    }
}
