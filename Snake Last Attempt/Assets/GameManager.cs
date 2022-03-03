using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    public bool gameStarted = false;
    
    [SerializeField] private Button _startGameBtn;
    [SerializeField] private Button _createGridBtn;
    [SerializeField] private InputField _inputField;

    private Vector2 gridSize;
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private Transform _tilesTransform;

    [SerializeField] private GameObject _snakeHead;
    
    private Camera _mainCamera;

    public Vector2 GridSize => gridSize;

    private void Start() {
        _mainCamera = Camera.main;

        Show();
    }

    public void Show() {
        _inputField.onValidateInput = (string text, int charIndex, char addedChar) => {
            return ValidateChar("0123456789", addedChar);
        };

        _createGridBtn.onClick.AddListener(CreateGrid);
        _startGameBtn.onClick.AddListener(StartGame);
    }

    private void CreateGrid() {
        foreach (Transform tile in _tilesTransform) {
            Destroy(tile.gameObject);
        }

        if (int.TryParse(_inputField.text, out var number)) {
            if (number > 64) {
                number = 64;
                _inputField.text = "64";
            }
            
            else if (number < 3) {
                number = 3;
                _inputField.text = "3";
            }
            
            
            gridSize.x = number;
            gridSize.y = number;
        }

        else {
            gridSize.x = 0;
            gridSize.y = 0;
        }
        
        float offsetLeft = (-gridSize.x/2f) + 0.5f;
        float offsetBottom = (-gridSize.x/2f) + 0.5f;
        Vector3 nextPosition = new Vector3(offsetLeft, offsetBottom, 0f);

        for (int x = 0; x < (int)gridSize.x; x++)
        {
            for (int y = 0; y < (int)gridSize.y; y++)
            {
                GameObject tileGo = Instantiate(_tilePrefab, nextPosition, Quaternion.identity, _tilesTransform);
                tileGo.name = $"Tile_({x}, {y})";

                nextPosition.x += 1;
            }

            nextPosition.x = offsetLeft;
            nextPosition.y += 1;
        }

        if (gridSize.x > 0)
            SetCameraSize();
    }

    private char ValidateChar(string validChar, char addedChar) {
        if (validChar.IndexOf(addedChar) != -1)
            return addedChar;

        return '\0';
    }
    
    private void SetCameraSize() {
        _mainCamera.orthographicSize = gridSize.x / 2;
    }

    private void StartGame() {
        //Hide UI
        _createGridBtn.gameObject.SetActive(false);
        _inputField.gameObject.SetActive(false);
        _startGameBtn.gameObject.SetActive(false);
        
        //Spawn Snake;
        int randomX = Random.Range(Mathf.RoundToInt(-gridSize.x / 2) + 1, Mathf.RoundToInt(gridSize.x / 2));
        int randomY = Random.Range(Mathf.RoundToInt(-gridSize.x / 2) + 1, Mathf.RoundToInt(gridSize.x / 2));
        float offset;

        if (gridSize.x % 2 == 0)
            offset = 0.5f;
        else
            offset = 0f;
        
        Vector2 spawnPoint = new Vector2(randomX + offset, randomY + offset);
        print(spawnPoint);

        Instantiate(_snakeHead, spawnPoint, Quaternion.identity);

        gameStarted = true;
        gameObject.GetComponent<Fruit>().SpawnFruit();
    }

    public void GameOver() {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
}
