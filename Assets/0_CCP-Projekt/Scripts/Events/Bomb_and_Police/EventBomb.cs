using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBomb : MonoBehaviour
{
    public GameObject[] thiefs;
    public GameObject[] bombs;
    public GameObject[] hostlies;

    private GameObject mallComplete;
    private GameObject mallBurned;

    private bool thiefsRun = false;
    private bool hostileRun = false;

    private AudioSource audioAlarm;
    private AudioSource audioScream;
    private AudioSource audioExplosion;
    private AudioSource audioCountdown;
    private AudioSource audioThiefsRun;

    private ParticleSystem explosion;
    private ParticleSystem smoke_1;
    private ParticleSystem smoke_2;
    private ParticleSystem smoke_3;

    // Start is called before the first frame update
    void Start()
    {

        // Alle Bomben aktivieren, damit man sie nicht sehen kann
        foreach (GameObject p in bombs)
        {
            p.SetActive(false);
        }

        // Alle Geißeln deaktivieren, damit man sie nicht sehen kann
        foreach (GameObject p in hostlies)
        {
            p.SetActive(false);
        }

        audioAlarm = GameObject.Find("Shop_Alarm").GetComponent<AudioSource>();
        audioScream = GameObject.Find("Scream_Hostiles").GetComponent<AudioSource>();
        audioExplosion = GameObject.Find("Explosion_Shop").GetComponent<AudioSource>();
        audioCountdown = GameObject.Find("Bomb_Countdown").GetComponent<AudioSource>();
        audioThiefsRun = GameObject.Find("Thiefs_Run").GetComponent<AudioSource>();
        audioAlarm.Stop();
        audioScream.Stop();
        audioExplosion.Stop();

        explosion = GameObject.Find("ThiefBombsExplosion").GetComponent<ParticleSystem>();
        smoke_1 = GameObject.Find("mall_burned_smoke_1").GetComponent<ParticleSystem>();
        smoke_2 = GameObject.Find("mall_burned_smoke_2").GetComponent<ParticleSystem>();
        smoke_3 = GameObject.Find("mall_burned_smoke_3").GetComponent<ParticleSystem>();
        mallComplete = GameObject.Find("Mall_Complete");
        mallBurned = GameObject.Find("Mall_Burned");  
    }

    void Update()
    {
        // Ist das Bombenaevent aktiv UND NICHT bereits durchgelaufen?
        if (!GameController.Instance.eventBombDone && GameController.Instance.eventBomb)
        {

            // Sind die Räuber bereits losgerannt?
            if (!thiefsRun)
            {
                // Verbrecher laufen los
                foreach (GameObject p in thiefs)
                {
                    p.GetComponent<Moveit>().start();
                    p.GetComponent<Moveit>().run();
                    p.GetComponent<Waypoints>().isMoving = true;
                }
                thiefsRun = true;
                audioThiefsRun.Play();
            }

            // Wenn die Räuber ihre Bomben platziert haben (Ausgelöst durch Trigger-Event)
            // Dann können die Geißel losrennen. Außerdem startet der Countdown von 22,6 Sekunden.
            // Nach Ablauf des Countdowns exlodieren die Bomben, wenn das Polizei-Event nicht vorher ausgelöst wurde.
            else if (!hostileRun && GameController.Instance.eventBombPlaced == 4)
            {
                audioAlarm.Play();
                audioScream.Play();
                audioCountdown.Play();
                Invoke("countdownOver", 22.6f);

                // Die Geißeln rennen alle mit einem zufälligen Abstand voneinander los
                hostileRun = true;
                foreach (GameObject p in hostlies)
                {
                    StartCoroutine(RunHostile(p, Random.Range(0.1f, 2.3f)));
                }
            }

            // Wenn die Polizei bei der Mall während des Countdowns angekommen ist, wird die Explosion verhindert.
            // Das event wird beendet
            else if(GameController.Instance.eventPoliceAtMall)
            {
                audioAlarm.Stop();
                audioCountdown.Stop();
                GameController.Instance.eventBombDone = true;
                GameController.Instance.eventBomb = false;
            }
        }
    }

    private void countdownOver()
    {
        // Wenn der Countdown abgelaufen ist und die Polizei NICHT gerade bei der Mall angekommen ist, 
        // explodiert die Bombe. Animation wird abgespielt, die Mall als Objekt entfernt und die zerstörte Mal als Objekt aktiviert.
        if(!GameController.Instance.eventPoliceAtMall) { 
            audioExplosion.Play();
            audioAlarm.Stop();
            explosion.Play(); 

            Destroy(mallComplete);
            mallBurned.transform.localScale = new Vector3(1, 1, 1);
            smoke_1.Play();
            smoke_2.Play();
            smoke_3.Play();
            foreach (GameObject p in bombs)
            {
                Destroy(p);
            }

            GameController.Instance.eventBombDone = true;
            GameController.Instance.eventBomb = false;
        }
    }

    // Die Geißeln rennen mit einem bestimmten Delay los, damit nicht alle auf einmal loslaufen.
    IEnumerator RunHostile(GameObject p, float delay)
    {
        yield return new WaitForSeconds(delay);
        p.SetActive(true);
        p.GetComponent<Moveit>().start();
        p.GetComponent<Moveit>().run();
        p.GetComponent<Waypoints>().isMoving = true;
    }
}
