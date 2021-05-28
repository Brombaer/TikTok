using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_TimeTrigger : MonoBehaviour
{
    private float _currentTime;
    private int _leftTime;
    private bool timeractive = false;
    [FMODUnity.EventRef]
    public string Event;
    [SerializeField]
    private float _startingTime;
 


    void Start()
    {
        _currentTime = _startingTime;
        timeractive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeractive)
        {
            if (_currentTime > 0)
            {
                _currentTime -= Time.deltaTime;
                _leftTime = Mathf.RoundToInt(_currentTime);
                
                if(_leftTime == _startingTime * 0.5)
                {
                    PlayOneShot();
                }

                if (_leftTime == _startingTime * 0.2)
                {
                    PlayOneShot();
                }

                if (_leftTime == _startingTime * 0.1)
                {
                    PlayOneShot();
                }
            }
            else
            {
                _currentTime = 0;
                timeractive = false;
            }
        }

    }
    public void PlayOneShot()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(Event, gameObject);
    }


}
