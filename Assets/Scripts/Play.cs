using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    
    public void PlayGame()
    {
        
        SceneManager.LoadScene("SampleScene");
        StartCoroutine(play());
    }

    IEnumerator play()
        {
		    yield return new WaitForSeconds(2f);	//play sahnesi öncesi bekleme
        }
}
