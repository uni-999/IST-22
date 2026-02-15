using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LapScript : MonoBehaviour
{
    public int countOfLap;
    public int numerOfLap;

    public GameObject textLaps;
    public GameObject winScreen;
    public GameObject loseScreen;
    public AudioSource source;
    public AudioClip winClip;

    private bool playerWin;
    private bool botWin;

    // Update is called once per frame
    void Start()
    {
        countOfLap += 1;
        numerOfLap = 0;
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent<MovmentScript>(out MovmentScript player) && playerWin == false)
        {
            numerOfLap++;
            if (numerOfLap == countOfLap)
            {
                player.isRaceOver = true;
                player.velocity = 5;
                playerWin = true;
                if(botWin == false)
                {
                    winScreen.SetActive(true);
                    source.Stop();
                    source.PlayOneShot(winClip);
                }
                else
                {
                    loseScreen.SetActive(true);
                }
            }
            textLaps.GetComponent<TextMeshProUGUI>().text = "Круг: " + (numerOfLap - 1).ToString() + "/" + (countOfLap - 1).ToString();
        }
        else
        {
            BotScript bot = other.GetComponent<BotScript>();
            if(bot.countLaps == countOfLap)
            {
                bot.acceleration = 0.01f;
                bot.boost = 0.01f;
                bot.currentSpeed = 5f;
                botWin = true;
            }
        }



        
    }
    public void ChooseMenuMap()
    {
        SceneManager.LoadScene(0);
    }
}
