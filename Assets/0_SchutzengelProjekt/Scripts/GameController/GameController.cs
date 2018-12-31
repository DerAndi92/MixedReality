using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public List<MonoBehaviour> eventSubscribedScripts = new List<MonoBehaviour>();
    public int gameEventID = 0;

    public bool eventFire = false;
    public bool isFire = false;
    public bool fireCarSend = false;
    public bool trackedFireCarTarget = false;

    public bool eventUfo = false;
    public bool isUfo = false;

    public bool eventCarCrash = false;
    public bool isCarCrash = false;

    public bool fireCareOnEvent = false;


    private static GameController instance;


    public static GameController Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<GameController>();
            }
            return instance;
        }
    }



	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}

    private void Update()
    {
    }

    public void SubscribeScriptToGameEventUpdates(MonoBehaviour pScript) {
        eventSubscribedScripts.Add(pScript);
    }

    public void DeSubscribeScriptToGameEventUpdates(MonoBehaviour pScript)
    {
        while(eventSubscribedScripts.Contains(pScript)) {
            eventSubscribedScripts.Remove(pScript);
    
        }
    }

    public void PlayerActivatedEvent() {
        gameEventID++;
        Debug.Log("Gamecontrolle playerPassedEvent" + gameEventID);
        foreach(MonoBehaviour _script in eventSubscribedScripts) {
            _script.Invoke("gameEventUpdated", 0);
        }

    }

}
