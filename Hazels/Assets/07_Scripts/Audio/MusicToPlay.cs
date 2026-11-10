using UnityEngine;

public class MusicToPlay : MonoBehaviour
{
    public int musicToPlay;
    
    void Start()
    {
        AudioManager.Instance.PlayMusic(musicToPlay);
    }
}
