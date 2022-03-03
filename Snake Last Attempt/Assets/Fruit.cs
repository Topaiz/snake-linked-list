using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fruit : MonoBehaviour {
    [SerializeField] private GameManager _manager;
    [SerializeField] private GameObject _fruitObj;
    [SerializeField] private SnakeMovement _snakeMovement;
    private GameObject _curFruit;

    public void SpawnFruit()
    {
        if (!_manager.gameStarted) {
            return;
        }

        int randomX = Random.Range(Mathf.RoundToInt(-_manager.GridSize.x / 2) + 1, Mathf.RoundToInt(_manager.GridSize.x / 2));
        int randomY = Random.Range(Mathf.RoundToInt(-_manager.GridSize.x / 2) + 1, Mathf.RoundToInt(_manager.GridSize.x / 2));
        float offset;

        if (_manager.GridSize.x % 2 == 0)
            offset = 0.5f;
        else
            offset = 0f;
        
        Vector2 spawnPoint = new Vector2(randomX + offset, randomY + offset);

        _curFruit = Instantiate(_fruitObj, spawnPoint, Quaternion.identity);

        foreach (Vector3 position in _snakeMovement.CheckAllPositions()) {
            if (_curFruit.transform.position == position) {
                Destroy(_curFruit);
                SpawnFruit();
            }
        }
    }
}
