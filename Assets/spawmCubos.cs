using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class spawmCubos : MonoBehaviour
{ 
    [Header("GUI")]
    [SerializeField] Text score;
    [SerializeField] Text timer;
    int pnts=0;
    float seconds = 60;
    [Header("VelocityCube ")]
    [SerializeField] Transform _player;
    [SerializeField][Range(0,12)] float velocity;
    [SerializeField] [Range(0, 5)] float acelerate;
    [Header("Objects: ")]
    [SerializeField] float timeToSpwam;
    [SerializeField] public subo cubos;
    [SerializeField] GameObject[] cubosR1;
    [SerializeField] GameObject[] cubosR2;
    [SerializeField] GameObject[] cubosR3;
    [Header("Arduino: ")]
    [SerializeField] Arduino _arduino;

    void Start()
    {
        cubos.esconderme += SpawmRotos;
        StartCoroutine(spwam());
        _arduino.Escudo = ChangeToEscudo;
        _arduino.ArmaDistancia = ChangeToRange;

    }

    private void Update()
    {
        seconds -= Time.deltaTime;
        score.text = "Score: " + pnts;
        timer.text = "Time: " + (int)seconds;
    }

    IEnumerator spwam()
    {
        yield return new WaitForSecondsRealtime(timeToSpwam);
        GameObject tmp = Instantiate(cubos.gameObject, transform.position, Quaternion.identity);
        Vector3 tmpV = new Vector3(_player.position.x - transform.position.x, _player.position.y - transform.position.y, _player.position.z - transform.position.z);
        float ac = acelerate * Time.deltaTime;
        tmp.GetComponent<Rigidbody>().velocity = tmpV * (velocity + ac);
        tmp.GetComponent<subo>().esconderme += SpawmRotos;
        StartCoroutine(spwam());
    }
    public void SpawmRotos(subo c)
    {
        pnts += 10;
        float random = Random.Range(1,4);
        Debug.Log(random);
        switch (random)
        {
            case 1:

                Instantiate(cubosR1[0], c.gameObject.transform.position, Quaternion.identity);
                Instantiate(cubosR1[1], c.gameObject.transform.position, Quaternion.identity);
                break;
            case 2:

                Instantiate(cubosR2[0], c.gameObject.transform.position, Quaternion.identity);
                Instantiate(cubosR2[1], c.gameObject.transform.position, Quaternion.identity);
                break;
            case 3:

                Instantiate(cubosR3[0], c.gameObject.transform.position, Quaternion.identity);
                Instantiate(cubosR3[1], c.gameObject.transform.position, Quaternion.identity);
                break;
            default:
                break;
        }
    }

    public void ChangeToRange()
    {

    }
    public void ChangeToEscudo()
    {

    }
}
