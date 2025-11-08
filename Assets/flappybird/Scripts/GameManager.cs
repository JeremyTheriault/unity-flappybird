using extOSC;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    

    [SerializeField] private Player player;
    [SerializeField] private Spawner spawner;
    [SerializeField] private Parallax ground;
    [SerializeField] private Text scoreText;
   // [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameOver;


    public int score { get; private set; } = 0;

    private bool isPlaying = false;

    public OSCReceiver oscReceiver;
    
    public OSCTransmitter oscTransmitter;




    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }
    
    void TraiterOscKey(OSCMessage message)
    {
        // Si le message n'a pas d'argument ou l'argument n'est pas un Int on l'ignore
        if (message.Values.Count == 0)
        {
            Debug.Log("No value in OSC message");
            return;
        }

        if (message.Values[0].Type != OSCValueType.Int)
        {
            Debug.Log("Value in message is not an Int");
            return;
        }

        // Récupérer la valeur de l’angle depuis le message OSC
        int value = message.Values[0].IntValue;

        // EXEMPLE : utiliser la valeur pour appliquer une rotation
        // Adapter proportionnellement la valeur reçue
        //float angle = Proportion(value, 0, 4095, -180, 180);
        // Appliquer la rotation à l’objet
        //transform.rotation = Quaternion.Euler(0, angle, 0);
        if (!isPlaying && value == 0) {
            Play();
        }
    }

    private void Start()
    {
        Stop();
        oscReceiver.Bind("/key", TraiterOscKey);
    }

    public void Stop()
    {
        //Time.timeScale = 0f;
        player.enabled = false;
        spawner.enabled = false;
        ground.enabled = false;
        Pipes[] pipes = FindObjectsOfType<Pipes>();

        for (int i = 0; i < pipes.Length; i++) {
            pipes[i].enabled = false;
        }

        isPlaying = false;

        var oSCMessage = new OSCMessage("/pixel");

        // Populate values.
        oSCMessage.AddValue(OSCValue.Int(255));
        oSCMessage.AddValue(OSCValue.Int(0));
        oSCMessage.AddValue(OSCValue.Int(0));

        // Send message  
        oscTransmitter.Send(oSCMessage);
    }

    public void Play()
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

        for (int i = 0; i < pipes.Length; i++) {
            Destroy(pipes[i].gameObject);
        }

        isPlaying = true;

        var oSCMessage = new OSCMessage("/pixel");

        // Populate values.
        oSCMessage.AddValue(OSCValue.Int(0));
        oSCMessage.AddValue(OSCValue.Int(255));
        oSCMessage.AddValue(OSCValue.Int(0));

        // Send message  
        oscTransmitter.Send(oSCMessage);

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
        if (!isPlaying && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))) {
            Play();
        }
    }

}
