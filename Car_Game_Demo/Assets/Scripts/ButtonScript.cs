using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

    public GameObject theColourButton;
    public GameObject colourHolder;

    private int i = 0;
    private string colour = "blue";

    public void Awake()
    {
        theColourButton.GetComponent<Image>().color = Color.blue;
        colourHolder.GetComponent<Image>().color = Color.blue;
        GameObject.DontDestroyOnLoad(colourHolder);
    }

    public void onStartClicked()
    {
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("SampleScene");
    }

    public void onColourClicked()
    {
        i++;

        if (i % 3 == 0)
        {
            theColourButton.GetComponent<Image>().color = Color.blue;
            colourHolder.GetComponent<Image>().color = Color.blue;
            colour = "blue";
        }

        else if (i % 3 == 1)
        {
            theColourButton.GetComponent<Image>().color = Color.green;
            colourHolder.GetComponent<Image>().color = Color.green;
            colour = "green";
        }

        else if (i % 3 == 2)
        {
            theColourButton.GetComponent<Image>().color = Color.black;
            colourHolder.GetComponent<Image>().color = Color.black;
            colour = "black";
        }
    }

    public string getColour()
    {
        return colour;
    }
}
