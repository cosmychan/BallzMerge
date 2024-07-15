using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    public static BallController instance;

    public Vector2 initPos;
    public Vector2 lastPos;

    public LineRenderer line;
    public Rigidbody2D rigid;
    public Camera cam;

    public float dragLimit = 3f;
    public float forceToAdd = 10f;

    public bool isDragging;
    public bool startedMove;

    public ContactPoint2D[] contact;
    public AudioSource audio;
    public AudioClip hitSound;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("x5 Button").GetComponent<Button>().onClick.AddListener(() => { SpeedUpBy5(); });
        audio = GetComponent<AudioSource>();
        cam = Camera.main;
        line = GetComponent<LineRenderer>();
        rigid = GetComponent<Rigidbody2D>();

        initPos = gameObject.transform.position;
        lastPos = initPos;

        line.positionCount = 2;
        line.SetPosition(0, initPos);
        line.SetPosition(1, initPos);
        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.LogError("started move " + startedMove);

        if (MoveScoreCounter.instance.isMove)
        {
            
            if (Input.GetMouseButtonDown(0) && !isDragging)
            {
                DragStart();
            }

            if (isDragging)
            {
                Drag();
            }

            if (Input.GetMouseButtonUp(0) && isDragging)
            {
                DragEnd();
            }
        }
        else
        {
            return;
        }
        
    }

    public void SpeedUpBy5()
    {
        forceToAdd = 15f;
    }

    private void DragStart()
    {
        //Debug.LogError("DragStart");
        line.enabled = true;
        isDragging = true;
        line.SetPosition(0, lastPos);
    }

    private void Drag()
    {
        //Debug.LogError("Drag");
        Vector3 startPos = line.GetPosition(0);
        Vector3 currentPos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 distance = currentPos - startPos;

        if (distance.magnitude <= dragLimit)
        {
            line.SetPosition(1, currentPos);
        }
        else
        {
            Vector3 limitVector = startPos + (distance.normalized * dragLimit);
            line.SetPosition(1, limitVector);
        }
    }   

    private void DragEnd()
    {
        //Debug.LogError("DragEnd");
        isDragging = false;
        line.enabled = false;

        Vector3 startPos = line.GetPosition(0);
        Vector3 currentPos = line.GetPosition(1);

        Vector3 distance = currentPos - startPos;
        Vector3 finalForce = distance * forceToAdd;

        rigid.AddForce(finalForce, ForceMode2D.Impulse);
        //startedMove = true;


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        switch (collision.collider.tag)
        {

            case "Block":
                //play sound
                audio.PlayOneShot(hitSound);

                //hit block
                Vector3 blockInitPos = collision.gameObject.transform.position;
                Vector3 direction = collision.gameObject.GetComponent<Rigidbody2D>().velocity.normalized;

                //Debug.LogError("direction " + direction);
                
                if (direction.x > 0.8) {
                    direction = Vector3.right;
                } else if (direction.x < -0.8)
                {
                    direction = Vector3.left;
                }

                if (direction.y > 0.8)
                {
                    direction = Vector3.up;
                }
                else if (direction.y < -0.8)
                {
                    direction = Vector3.down;
                }
                collision.rigidbody.Sleep();
                collision.gameObject.GetComponent<Placement>().MoveBlock(direction);

                //Debug.LogError("pos " + blockInitPos);
                //collision.gameObject.transform.position = blockInitPos + direction;
                //Debug.LogError("pos2 " + (blockInitPos + direction));
                //collision.rigidbody.velocity = Vector2.zero;
                //collision.rigidbody.Sleep();

                break;

            case "StartZone":
                if (startedMove)
                {
                    //play sound
                    audio.PlayOneShot(hitSound);

                    //stop ball movement
                    rigid.Sleep();
                    //end move, reset ball last pos
                    lastPos = gameObject.transform.position;
                    //call endmove function
                    MoveScoreCounter.instance.EndMove();

                    startedMove = false;
                }
                break;

            case "Wall":
                //play sound
                audio.PlayOneShot(hitSound);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EndZone")
        {
            startedMove = true;
        }
    }
}
