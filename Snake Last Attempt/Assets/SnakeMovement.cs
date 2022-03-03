using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Vector2;

public class SnakeMovement : MonoBehaviour {
    private Vector2 direction;
    private float timer;
    private SnakeList<GameObject> _snakeList = new SnakeList<GameObject>();
    private Vector2 _prevHeadPos;
    private Vector2 _prevDirection;
    private GameManager _gameManager;
    private Fruit _fruitScript;

    [SerializeField] private GameObject snakeNode;

    public SnakeList<GameObject> SnakeList => _snakeList;

    void Start() {
        timer = 0;
        _snakeList.Add(gameObject);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _fruitScript = GameObject.Find("GameManager").GetComponent<Fruit>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && _prevDirection != down) {
            direction = up;
        }
        
        if (Input.GetKeyDown(KeyCode.A) && _prevDirection != right) {
            direction = left;
        }
        
        if (Input.GetKeyDown(KeyCode.S) && _prevDirection != up) {
            direction = down;
        }
        
        if (Input.GetKeyDown(KeyCode.D) && _prevDirection != left) {
            direction = right;
        }
        
        timer += Time.deltaTime;

        if (timer >= 1) {
            MoveHead();
            MoveBody();
        }
        
        Rotate();
    }

    void Rotate() {
        transform.up = direction;
    }
    
    void MoveHead() {
        timer = 0;
        _prevHeadPos = transform.position;
        var position = _prevHeadPos;
        position = new Vector2(position.x + direction.x, position.y + direction.y);
        transform.position = position;

        if (transform.position.x > _gameManager.GridSize.x / 2 || transform.position.x < -_gameManager.GridSize.x / 2) {
            _gameManager.GameOver();
        }

        if (transform.position.y > _gameManager.GridSize.y / 2 || transform.position.y < -_gameManager.GridSize.y / 2) {
            _gameManager.GameOver();
        }
        
        _prevDirection = direction;
    }

    void MoveBody() {

        if (_snakeList.count <= 0) {
            return;
        }

        SnakeList<GameObject>.SnakeNode currentNode = _snakeList.tail;

        while (currentNode != null) {
            if (currentNode.data == _snakeList.head.data) {
                //currentNode.data.transform.position = _prevHeadPos;
            }
            
            else if (currentNode.index == 1) {
                currentNode.data.transform.position = _prevHeadPos;
            }

            else {
                currentNode.data.transform.position = currentNode.prev.data.transform.position;
            }

            currentNode = currentNode.prev;
        }
    }

    //Doesn't work 
    public List<Vector3> CheckAllPositions() {
        List<Vector3> position =  new List<Vector3>();
        
        for (int i = 0; i < _snakeList.count; i++) {
            SnakeList<GameObject>.SnakeNode currentNode = _snakeList.tail;
            position.Add(currentNode.data.transform.position);
            currentNode = currentNode.prev;
        }
        
        return position;
    }

    void Grow() {
        Vector2 currentDirection;
        Vector3 position;
        Vector3 headPos = transform.position;
                
        currentDirection = direction;
        
        if (_snakeList.count <= 0)
        {

            if (currentDirection == up) {
                position = new Vector3(headPos.x, headPos.y - 1, 0f);
            }
            
            else if (currentDirection == left) {
                position = new Vector3(headPos.x + 1, headPos.y, 0f);
            }
            
            else if (currentDirection == down) {
                position = new Vector3(headPos.x, headPos.y + 1, 0f);
            }

            else if (currentDirection == right) {
                position = new Vector3(headPos.x - 1, headPos.y, 0f);
            }

            else {
                position = new Vector3(headPos.x, headPos.y - 1, 0f);
            }
        }
        
        else if (_snakeList.count == 1) {
            position = _prevHeadPos;
        }

        else {
            position = _snakeList.tail.data.transform.position;
        }
        
        GameObject bodyPart = Instantiate(snakeNode, position, Quaternion.identity);
        _snakeList.Add(bodyPart);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Fruit")) {
            Grow();
            Destroy(col.gameObject);
            _fruitScript.SpawnFruit();
        }

        if (col.gameObject.CompareTag("Snake")) {
            _gameManager.GameOver();
        }
    }

}
