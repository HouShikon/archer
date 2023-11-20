using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Arrow : MonoBehaviour
{
    public int speed = 15;
    private Rigidbody rgd;
    public RoleType roleType;
    public GameObject explosionEffect;
    public bool isLocal = false;
    // Start is called before the first frame update
    void Start()
    {
        rgd = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rgd.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameFacade.Instance.PlayNormalSound(AudioManager.Sound_ShootPerson);
            if (isLocal)
            {
                bool playerIsLocal = other.GetComponent<PlayerInfo>().isLocal;
                if (isLocal != playerIsLocal)
                {
                    GameFacade.Instance.SendAttack(15);
                }
            }
        }
        else
        {
            GameFacade.Instance.PlayNormalSound(AudioManager.Sound_Miss);
        }
        GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);
        GameObject.Destroy(this.gameObject);
    }
}
