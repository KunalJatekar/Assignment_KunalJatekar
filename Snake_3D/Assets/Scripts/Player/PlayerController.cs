using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public PlayerDirection playerDirection;

    [HideInInspector] public float stepLength = 1;

    [HideInInspector] public float movementFrequency = 0.2f;

    //[HideInInspector] public bool createNodeAtTail;

    [SerializeField] GameObject tailPrefab;

    List<Vector3> deltaPosition;
    List<Rigidbody> nodes;
    Rigidbody mainBody;
    Rigidbody headBody;
    Transform tr;

    float counter;
    bool move;

    void Awake()
    {
        tr = transform;
        mainBody = GetComponent<Rigidbody>();

        InItSnakeNodes();
        InItPlayer();

        deltaPosition = new List<Vector3>()
        {
            new Vector3(-stepLength,0f,0f),
            new Vector3(0f,0f,stepLength),
            new Vector3(stepLength,0f,0f),
            new Vector3(0f,0f,-stepLength)
        };
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovementFrequency();
    }

    void FixedUpdate()
    {
        if (move)
        {
            move = false;
            Move();
        }
    }

    void InItSnakeNodes()
    {
        nodes = new List<Rigidbody>();

        nodes.Add(tr.GetChild(0).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(1).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(2).GetComponent<Rigidbody>());

        headBody = nodes[0];
    }

    void SetDirectionRandom()
    {
        int dirRandom = Random.Range(0, (int)PlayerDirection.COUNT);
        playerDirection = (PlayerDirection)dirRandom;
    }

    void InItPlayer()
    {
        SetDirectionRandom();

        switch (playerDirection)
        {
            case PlayerDirection.RIGHT:

                nodes[1].position = nodes[0].position + new Vector3(Matrics.node, 0f, 0f);
                nodes[2].position = nodes[0].position + new Vector3(Matrics.node * 2, 0f, 0f);
                break;

            case PlayerDirection.LEFT:

                nodes[1].position = nodes[0].position - new Vector3(Matrics.node, 0f, 0f);
                nodes[2].position = nodes[0].position - new Vector3(Matrics.node * 2, 0f, 0f);
                break;

            case PlayerDirection.UP:

                nodes[1].position = nodes[0].position + new Vector3(0f, 0f, Matrics.node);
                nodes[2].position = nodes[0].position + new Vector3(0f, 0f, Matrics.node * 2);
                break;

            case PlayerDirection.DOWN:

                nodes[1].position = nodes[0].position - new Vector3(0f, 0f, Matrics.node);
                nodes[2].position = nodes[0].position - new Vector3(0f, 0f, Matrics.node * 2);
                break;
        }
    }

    void Move()
    {
        Vector3 dPosition = deltaPosition[(int)playerDirection];

        Vector3 parentPos = headBody.position;
        Vector3 prevPosition;

        mainBody.position = mainBody.position + dPosition;
        headBody.position = headBody.position + dPosition;

        for(int i = 1; i < nodes.Count; i++)
        {
            prevPosition = nodes[i].position;
            nodes[i].position = parentPos;
            parentPos = prevPosition;
        }
    }

    void CheckMovementFrequency()
    {
        counter += Time.deltaTime;
        if (counter >= movementFrequency)
        {
            counter = 0;
            move = true;
        }
    }

    public void SetUpInputDirection(PlayerDirection dir)
    {
        if(dir==PlayerDirection.UP && playerDirection==PlayerDirection.DOWN ||
            dir == PlayerDirection.DOWN && playerDirection == PlayerDirection.UP ||
            dir == PlayerDirection.RIGHT && playerDirection == PlayerDirection.LEFT ||
            dir == PlayerDirection.LEFT && playerDirection == PlayerDirection.RIGHT)
        {
            return;
        }

        playerDirection = dir;
        ForceMove();
    }

    void ForceMove()
    {
        counter = 0;
        move = false;
        Move();
    }

    public void AddBodyPart()
    {
        GameObject newNode = Instantiate(tailPrefab, nodes[nodes.Count - 1].position, Quaternion.identity);
        newNode.transform.SetParent(transform, true);
        nodes.Add(newNode.GetComponent<Rigidbody>());
    }
}
