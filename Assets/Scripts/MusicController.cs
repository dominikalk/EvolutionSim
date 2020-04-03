using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MusicController : MonoBehaviour
{
    public static MusicController Instance { get; private set; }

    public AudioClip[] music;
    public AudioSource audioSource;
    private int whatSong = 0;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        audioSource = GetComponent<AudioSource>();
        shuffleArray(music);
        StartCoroutine("playSongs");
    }

    IEnumerator playSongs()
    {
        while (true)
        {
            if(whatSong >= music.Length)
            {
                whatSong = 0;
            }
            audioSource.clip = music[whatSong];
            audioSource.Play();
            yield return new WaitForSecondsRealtime(music[whatSong].length + 2f);
            whatSong += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && SceneManager.GetActiveScene().name == "MainMenu")
        {
            if(audioSource.volume == 0)
            {
                audioSource.volume = 1;
            }
            else
            {
                audioSource.volume = 0;
            }
        }
    }

    void shuffleArray(AudioClip[] clips)
    {
        for (int i = 0; i < clips.Length; i++)
        {
            AudioClip temp = clips[i];
            int r = Random.Range(i, clips.Length);
            clips[i] = clips[r];
            clips[r] = temp;
        }
    }
}
