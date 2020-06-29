using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CharacterSelect : MonoBehaviour {
	public int DiffMode = 0;
    public GameObject easy;
    public GameObject medium;
    public GameObject Hard;

    public GameObject Opt1;
    public GameObject Opt2;

	public void Adaptive()
	{
		DiffMode = 0;
		PlayerPrefs.SetInt("Difficulty", DiffMode);
		SceneManager.LoadScene ("Dissertation");
	}
    public void Fixed()
	{
        Opt1.SetActive(false);
        Opt2.SetActive(false);
        easy.SetActive(true);
        medium.SetActive(true);
        Hard.SetActive(true);
	}

    public void easyMode()
    {
        DiffMode = 1;
        PlayerPrefs.SetInt("Difficulty", DiffMode);
        SceneManager.LoadScene("Dissertation");
    }

    public void mediumMode()
    {
        DiffMode = 2;
        PlayerPrefs.SetInt("Difficulty", DiffMode);
        SceneManager.LoadScene("Dissertation");
    }

    public void hardMode()
    {
        DiffMode = 3;
        PlayerPrefs.SetInt("Difficulty", DiffMode);
        SceneManager.LoadScene("Dissertation");
    }
    void OnApplicationQuit()
	{
		PlayerPrefs.DeleteKey("Difficulty");
		PlayerPrefs.DeleteAll();
	}
}
