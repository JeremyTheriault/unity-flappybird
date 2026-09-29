using extOSC;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private Player player;

    [SerializeField]
    private Spawner spawner;

    [SerializeField]
    private Parallax ground;

    [SerializeField]
    private Text scoreText;

    // [SerializeField] private GameObject playButton;
    [SerializeField]
    private GameObject gameOver;

    public int score { get; private set; } = 0;

    private bool isPlaying = false;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }

    private void Start()
    {
        Stop();
    }

    public void Stop()
    {
        //Time.timeScale = 0f;
        player.enabled = false;
        spawner.enabled = false;
        ground.enabled = false;
        Pipes[] pipes = FindObjectsOfType<Pipes>();

        for (int i = 0; i < pipes.Length; i++)
        {
            pipes[i].enabled = false;
        }

        isPlaying = false;
    }

    public void StartGame()
    {
        if (!isPlaying)
        {
            score = 0;
            scoreText.text = score.ToString();

            //playButton.SetActive(false);
            gameOver.SetActive(false);

            //Time.timeScale = 1f;
            player.enabled = true;
            spawner.enabled = true;
            ground.enabled = true;

            Pipes[] pipes = FindObjectsOfType<Pipes>();

            for (int i = 0; i < pipes.Length; i++)
            {
                Destroy(pipes[i].gameObject);
            }

            isPlaying = true;
        }
    }

    public void GameOver()
    {
        //.SetActive(true);
        gameOver.SetActive(true);

        Stop();
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void Update()
    {
        
    }
}
