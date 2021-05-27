using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    //Variable
    private float _currentTime;
    [SerializeField]
    private float _startingTime;
    [SerializeField]
    private Image _image;
    private Color _targetColor;
    [SerializeField]
    private Text _text;


    //function
    public void Start()
    {
        _image = gameObject.GetComponent<Image>();
        _currentTime = _startingTime;
    }



    public void Update()
    {
        
        if (_currentTime != 0)
        {
            
            _currentTime -= Time.deltaTime;
            _text.text = _currentTime.ToString();
            _image.fillAmount = _currentTime / _startingTime;
            if (_currentTime < 10)
            {
                _image.color = new Color32(255, 255, 0, 100);
            }

        }
        if (_currentTime == 0)
        {
            SceneManager.LoadScene(1);
        }
    }
}
