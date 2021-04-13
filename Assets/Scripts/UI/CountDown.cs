using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    //Variable
    [SerializeField]
    private float _currentTime;
    [SerializeField]
    private float _startingTime;
    [SerializeField]
    private Text _countDownText;
        


    //Function
    void Start()
    {
        _currentTime = _startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime -= 1 * Time.deltaTime;
        _countDownText.text = _currentTime.ToString("0");
        
        if (_currentTime <= 0)
        {
            _currentTime = 0;
            SceneManager.LoadScene(1);
        }
    }
}
