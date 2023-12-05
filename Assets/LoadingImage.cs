using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadingImage : MonoBehaviour
{
    public int loadToTimer;

    public bool cargado = false;
    public float tiempo = 0;
    public int maxTiempo = 3;
    public List<GameObject> fotos;
    public int positivos = 3;
    public int cont = 1;
    public bool removed = false;
    private int lastImage;
    [SerializeField] private RawImage RawImage1;
    [SerializeField] private RawImage RawImage2;
    [SerializeField] private RawImage RawImage3;
    [SerializeField] private RawImage RawImage4;
    [SerializeField] private RawImage RawImage5;
    [SerializeField] private RawImage RawImage6;
    [SerializeField] private RawImage RawImage7;
    [SerializeField] private RawImage RawImage8;
    [SerializeField] private RawImage RawImage9;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(timerToLoad());

    }

    // Update is called once per frame
    private void Update()
    {
        if (cargado == false)
        {
            tiempo += Time.deltaTime;
            if (tiempo > maxTiempo)
            {
                int randomNumber = Random.Range(0, fotos.Count);  
                tiempo = 0;
                this.RawImage1.gameObject.SetActive(false);
                this.RawImage2.gameObject.SetActive(false);
                this.RawImage3.gameObject.SetActive(false);
                this.RawImage4.gameObject.SetActive(false);
                this.RawImage5.gameObject.SetActive(false);
                this.RawImage6.gameObject.SetActive(false);
                this.RawImage7.gameObject.SetActive(false);
                this.RawImage8.gameObject.SetActive(false);
                this.RawImage9.gameObject.SetActive(false);
                fotos[randomNumber].SetActive(true);
                lastImage = randomNumber;
                if (cont >= positivos - 1)
                {
                    cont = 0;
                }
                if (cont >= 1 && removed == false)
                {
                    fotos.Remove(fotos[0]);
                    removed = true;
                }
                else cont++;
                fotos.Remove(fotos[lastImage]);

            }
        }
    }
    IEnumerator timerToLoad()
    {

        while (loadToTimer > 0)
        {
            yield return new WaitForSeconds(1f);
            loadToTimer = loadToTimer - 1;
        }
        yield return new WaitForSeconds(1f);
        //Cargar siguiente escena
        Debug.Log("IR A JUEGO");
    }
}
