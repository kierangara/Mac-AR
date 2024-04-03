using UnityEngine;
    /// <summary>
    /// Acts as a buffer between receiving requests to display error messages to the player and running the pop-up UI to do so.
    /// </summary>
public class LogHandlerSettings : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Only logs of this level or higher will appear in the console.")]
    private LogMode m_editorLogVerbosity = LogMode.Critical;

    [SerializeField]
    private PopUpUI m_popUp;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        InvokeRepeating(nameof(CheckNetwork), 5.0f, 5.0f);
    }
    public void CheckNetwork() {
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            SpawnErrorPopup("Error not connected to the internet");
        }
    }

    public static LogHandlerSettings Instance
    {
        get
        {
            if (s_LogHandlerSettings != null) return s_LogHandlerSettings;
            return s_LogHandlerSettings = FindFirstObjectByType<LogHandlerSettings>();
        }
    }
    static LogHandlerSettings s_LogHandlerSettings;
    private void Awake()
    {
        LogHandler.Get().mode = m_editorLogVerbosity;
        Debug.Log($"Starting project with Log Level : {m_editorLogVerbosity.ToString()}");
    }


    /// <summary>
    /// For convenience while in the Editor, update the log verbosity when its value is changed in the Inspector.
    /// </summary>
    public void OnValidate()
    {
        LogHandler.Get().mode = m_editorLogVerbosity;
    }


    public void SpawnErrorPopup(string errorMessage)
    {
        Debug.Log("popUpSpawn");
        m_popUp.ShowPopup(errorMessage);
    }
}

