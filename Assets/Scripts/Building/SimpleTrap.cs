using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrap : MonoBehaviour
{
    [SerializeField] private Rigidbody[] rigid;
    [SerializeField] private GameObject go_Meat;
    [SerializeField] private int damage;

    private bool isActivated = false;

    private AudioSource theAudio;
    [SerializeField] private AudioClip sound_Activate;

    private StatusController theStatusController;

    private void Start()
    {
        rigid = GetComponentsInChildren<Rigidbody>();
        theAudio = GetComponentInChildren<AudioSource>();
        theStatusController = FindObjectOfType<StatusController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActivated)
        {
            if(!other.transform.CompareTag("Untagged"))
            {
                Debug.Log("함정 발동");
                isActivated = true;
                theAudio.clip = sound_Activate;
                theAudio.Play();
                Destroy(go_Meat);

                for (int i = 0; i < rigid.Length; i++)
                {
                    rigid[i].useGravity = true;
                    rigid[i].isKinematic = false;
                }

                if(other.transform.name == "Player")
                {
                    Debug.Log("함정 충돌 데미지 입음");
                    theStatusController.DecreaseHp(damage);
                }
            }
        }
    }

}
