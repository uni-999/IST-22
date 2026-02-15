using UnityEngine;

public class BounsScript : MonoBehaviour
{
    public int bonusKey;
    public AudioSource source;
    public AudioClip clip_apple;
    public AudioClip clip_bad_apple;
    public AudioClip clip_boost;
    public AudioClip clip_trap;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<MovmentScript>(out MovmentScript player))
        {
            setEffectPlayer(player);
        }
        else
        {
            BotScript bot = other.GetComponent<BotScript>();
            setEffectBot(bot);
        }

        this.gameObject.SetActive(false);
    }

    void setEffectPlayer(MovmentScript player)
    {
        switch (bonusKey)
        {
            case 1: 
                player.velocity += 5;
                source.PlayOneShot(clip_apple);
                break;
            case 2: 
                player.velocity -= 5;
                source.PlayOneShot(clip_bad_apple);
                break;
            case 3:
                player.velocity = 15;
                player.isboosted = true;
                source.PlayOneShot(clip_boost);
                break;
            case 4: 
                player.haveTrap = true;
                source.PlayOneShot(clip_trap);
                break;
            case 5:
                int bonus = Random.Range(1, 5);
                switch (bonus)
                {
                    case 1:
                        player.velocity += 5;
                        source.PlayOneShot(clip_apple);
                        break;
                    case 2:
                        player.velocity -= 5;
                        source.PlayOneShot(clip_bad_apple);
                        break;
                    case 3:
                        player.velocity = 15;
                        player.isboosted = true;
                        source.PlayOneShot(clip_boost);
                        break;
                    case 4:
                        player.haveTrap = true;
                        source.PlayOneShot(clip_trap);
                        break;
                }
                break;
            case 6: 
                player.velocity = 1.5f;
                break;
        }
    }

    void setEffectBot(BotScript bot)
    {
        switch (bonusKey)
        {
            case 1:
                bot.currentSpeed += 5;
                break;
            case 2:
                bot.currentSpeed -= 5;
                break;
            case 3:
                bot.currentSpeed = 15;
                break;
            case 4:
                Instantiate(bot.gameObject.GetComponent<BotScript>().trapPrefab, new Vector3(bot.gameObject.transform.GetChild(7).transform.position.x, 25.805f, bot.gameObject.transform.GetChild(7).transform.position.z), Quaternion.identity);
                break;
            case 5:
                int bonus = Random.Range(1, 5);
                switch (bonus)
                {
                    case 1:
                        bot.currentSpeed += 5;
                        break;
                    case 2:
                        bot.currentSpeed -= 5;
                        break;
                    case 3:
                        bot.currentSpeed = 15;
                        break;
                    case 4:
                        Instantiate(bot.gameObject.GetComponent<BotScript>().trapPrefab, new Vector3(bot.gameObject.transform.GetChild(7).transform.position.x, 25.805f, bot.gameObject.transform.GetChild(7).transform.position.z), Quaternion.identity);
                        break;
                }
                break;
            case 6:
                bot.currentSpeed = 1.5f;
                break;
        }
    }

}
