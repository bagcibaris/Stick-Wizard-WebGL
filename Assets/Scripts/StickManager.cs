using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class StickManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SendScore(string score);         //Server a score gönderme..

    [SerializeField] private StickController stickPrefab;
    [SerializeField] private PillarManager pillarManager;
    [SerializeField] private Transform targetRotate;
    [SerializeField] private AnimationController animationController;
    [SerializeField] private ColliderDetect colliderDetect;
    [SerializeField] private float offsetX;
    [SerializeField] private GameObject btnpanel;
    private bool doOnPointDown = true;
    private bool thisDo = true;

    public StickController _current;
    private GameManager gameManager;

    private void Start()
    {
        Create();
    }

    public void Create()
    {
        var position = pillarManager.CurrentPillarPosition;
        position.x += offsetX;
        var stick = Instantiate(stickPrefab, position, quaternion.identity);
        _current = stick;
        doOnPointDown = true;
    }

    private void Update()
    {
        if (doOnPointDown == true)
        {
            btnpanel.SetActive(true);
        }
        else
        {
            btnpanel.SetActive(false);
        }


    }


    public void OnPointerDown()
    {
        if (doOnPointDown)
        {
            _current.Grow = true;
        }

    }

    public void OnPointerUp()
    {
        _current.Grow = false;
        doOnPointDown = false;
        IEnumerator Do()
        {
            thisDo = false;
            var rotate = animationController.Rotate(_current.transform, targetRotate);
            yield return rotate;
            yield return null;
            colliderDetect.LevelController(_current.colliderPosition.position);
            yield return new WaitForSeconds(.1f);
            if (colliderDetect.LevelPass)
            {
                pillarManager.NextLevel();
                thisDo = true;
            }
            else
            {
                StartCoroutine(GameOverDelay());
            }
        }
        if (thisDo)
        {
            StartCoroutine(Do());
        }

    }

    IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(.1f);   //ölüm sahnesi öncesi bekleme
        SceneManager.LoadScene("Dead");
        SendScore(gameManager.Score.ToString());        //score göndericiyi çağırıyor
    }
}
