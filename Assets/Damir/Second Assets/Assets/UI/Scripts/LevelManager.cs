using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class SceneLoader : MonoBehaviour
{
    public GameObject mainUI;
    public GameObject mainPlace;

    public GameObject mapUI;
    public GameObject mapPlace;

    public GameObject skinUI;
    public GameObject skinPlace;

    public GameObject mainSkin;
    public GameObject skinSkin;
    public Texture textureOne;
    public Texture textureTwo;
    public Texture textureThree;
    public Texture textureFour;
    public Texture textureFive;
    public Texture textureSix;

    private void Start()
    {
        PlayerPrefs.SetInt("NumberSkin", 1);
        ChooseMenu();

    }

    public void ChooseDesert()
    {
        SceneManager.LoadScene(1);
    }
    public void ChooseForest()
    {
        SceneManager.LoadScene(2);
    }
    public void ChooseSpace()
    {
        SceneManager.LoadScene(3);
    }
    public void ChooseMenuMap()
    {
        SceneManager.LoadScene(0);
    }

    public void ChooseMap()
    {
        mainPlace.SetActive(false);
        mainUI.SetActive(false);

        mapPlace.SetActive(true);
        mapUI.SetActive(true);

        skinPlace.SetActive(false);
        skinUI.SetActive(false);
    }
    public void ChooseSkin()
    {
        mainPlace.SetActive(false);
        mainUI.SetActive(false);

        mapPlace.SetActive(false);
        mapUI.SetActive(false);

        skinPlace.SetActive(true);
        skinUI.SetActive(true);
    }

    public void ChooseMenu()
    {
        mainPlace.SetActive(true);
        mainUI.SetActive(true);

        mapPlace.SetActive(false);
        mapUI.SetActive(false);

        skinPlace.SetActive(false);
        skinUI.SetActive(false);
    }

    public void SkinOne()
    {
        PlayerPrefs.SetInt("NumberSkin", 1);
        mainSkin.GetComponent<Renderer>().material.mainTexture = textureOne;
        skinSkin.GetComponent<Renderer>().material.mainTexture = textureOne;
    }
    public void SkinTwo()
    {
        PlayerPrefs.SetInt("NumberSkin", 2);
        mainSkin.GetComponent<Renderer>().material.mainTexture = textureTwo;
        skinSkin.GetComponent<Renderer>().material.mainTexture = textureTwo;

    }
    public void SkinThree()
    {
        PlayerPrefs.SetInt("NumberSkin", 3);
        mainSkin.GetComponent<Renderer>().material.mainTexture = textureThree;
        skinSkin.GetComponent<Renderer>().material.mainTexture = textureThree;

    }
    public void SkinFour()
    {
        PlayerPrefs.SetInt("NumberSkin", 4);
        mainSkin.GetComponent<Renderer>().material.mainTexture = textureFour;
        skinSkin.GetComponent<Renderer>().material.mainTexture = textureFour;

    }
    public void SkinFive()
    {
        PlayerPrefs.SetInt("NumberSkin", 5);
        mainSkin.GetComponent<Renderer>().material.mainTexture = textureFive;
        skinSkin.GetComponent<Renderer>().material.mainTexture = textureFive;

    }
    public void SkinSix()
    {
        PlayerPrefs.SetInt("NumberSkin", 6);
        mainSkin.GetComponent<Renderer>().material.mainTexture = textureSix;
        skinSkin.GetComponent<Renderer>().material.mainTexture = textureSix;

    }

}
