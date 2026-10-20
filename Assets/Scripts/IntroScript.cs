using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Threading;

public class IntroScript : MonoBehaviour
{
    int TextIndex = 0;
    TextMeshProUGUI Text;

    // Start is called before the first frame update
    void Start()
    {
        Text = gameObject.GetComponent<TextMeshProUGUI>();
        Dialogue();
    }

    public void Dialogue()
    {
        if (TextIndex == 0)
        {
            Text.text = "You are the fated hero of this world! The Dark Lord stands beyond a great towering door infront of you.";
        }
        else if (TextIndex == 1)
        {
            Text.text = "It creaks open slowly, revealing the Dark Lord in the middle of a terrible ritual.";
        }
        else if (TextIndex == 2)
        {
            Text.text = "You ready your blade, determined to strike them down! But they do not react to your presence.";
        }
        else if (TextIndex == 3)
        {
            Text.text = "As soon as you take another step forwards, a surge of energy rushes towards you. The sinister spell twists and morphs you, your body feels weak and heavy.";
        }
        else if (TextIndex == 4)
        {
            Text.text = "Everything fades to black...";
        }
        else if (TextIndex == 5)
        {
            Text.text = "When your vision returns, you find yourself in the middle of a field. All of your gear is gone and you have been transformed into a simple, pathetic slime.";
        }
        else if (TextIndex == 6)
        {
            Text.text = "Despite this, however, you remain determined to save the world, even in this form. This is where your journey begins.";
        }
        else if (TextIndex == 7)
        {
            SceneManager.LoadScene("Plains 1");
        }
        TextIndex += 1;
    }

    public void Skip()
    {
        SceneManager.LoadScene("Plains 1");
    }
}
