using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

    [SerializeField] float speed = 5f;
    [SerializeField] float jump = 300f;

    PhotonView pv;
    Rigidbody2D rb;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update ()
    {
        if (pv.IsMine) //chek if is the local player
        {
            ProcessInput();
        }
    }

    private void ProcessInput()
    {
        /*
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector2.left * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector2.right * Time.deltaTime * speed);
        if (Input.GetKeyDown(KeyCode.W))
            rb.velocity += Vector2.up * jump;
        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector2.down * Time.deltaTime * speed);
        */
        if (CrossPlatformInputManager.GetAxis("Horizontal") < 0)
            transform.Translate(Vector2.left * Time.deltaTime * speed);
        if (CrossPlatformInputManager.GetAxis("Horizontal") > 0)
            transform.Translate(Vector2.right * Time.deltaTime * speed);
        if (CrossPlatformInputManager.GetButtonUp("Jump"))
            rb.AddForce(new Vector2(0, jump));
    }
}
