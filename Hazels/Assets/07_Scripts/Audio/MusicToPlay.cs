using UnityEngine;

public class MusicToPlay : MonoBehaviour
{
    [SerializeField] int musicToPlay;
    
    void Start()
    {
        AudioManager.Instance.PlayMusic(musicToPlay);
    }
}
