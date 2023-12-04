using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class Arduino : MonoBehaviour
{
    [Header("Armas")]
    public GameObject armaMele;
    public GameObject escudo;
    public GameObject armaRange;
    public GameObject bullet;
    [Header("Arduino")]
    public float velocityB;
    SerialPort arduinoPort = new SerialPort("COM3");
    public Vector3 angle;
    public float x;
    public float y;
    public float z;
    public Action Escudo;
    public Action ArmaDistancia;
    public float accelVector;
    public bool Range;
    public bool EscudoB;
    public bool Arma;
    //public float countx, county, countz;
    private void Awake()
    {
        Range = false;
        EscudoB = false;
        Arma = true;
        // Configura el puerto serial
        arduinoPort.BaudRate = 9600;
        arduinoPort.Parity = Parity.None;
        arduinoPort.StopBits = StopBits.One;
        arduinoPort.DataBits = 8;
        arduinoPort.Handshake = Handshake.None;
    }

    void Start()
    {
        arduinoPort.Open();        
    }

    void Update()
    {
        if (arduinoPort.IsOpen)
        {
            try
            {
              
                //MPU6050
                string[] data = arduinoPort.ReadLine().Split(',');  // Lee una línea de datos y separa por comas

                if (Arma)
                {
                    armaMele.SetActive(true);
                    armaRange.SetActive(false);
                    escudo.SetActive(false);
                    if (data.Length >= 3)
                    {
                        // Limpia los valores de ángulo eliminando posibles espacios
                        string cleanedAngleX = data[0].Substring(2).Trim();
                        string cleanedAngleY = data[1].Substring(2).Trim();
                        string cleanedAngleZ = data[2].Substring(2).Trim();

                        if (float.TryParse(cleanedAngleX, out float angleX) &&
                            float.TryParse(cleanedAngleY, out float angleY) &&
                            float.TryParse(cleanedAngleZ, out float angleZ))
                        {
                            Debug.Log(angleX + " " + angleY + " " + angleZ);
                            x = angleX / 1f;
                            y = angleY / 1;
                            z = angleZ / 0.5f;
                            Vector3 targetRotation = new Vector3(-x, -y, z);
                          transform.rotation = Quaternion.Euler(targetRotation);
                        }
                        else
                        {
                            Debug.LogWarning("Error al analizar los valores de ángulo desde Arduino.");
                        }
                    }
                }
                if (Range)
                {
                    armaRange.SetActive(true);
                    escudo.SetActive(false);
                    armaMele.SetActive(false);
                    transform.localRotation = Quaternion.Euler(new Vector3(6.52999878f, 16.4499989f, 340.100006f));
                    if (accelVector >= 175)
                    {
                        GameObject tmp = Instantiate(bullet, transform.position, transform.localRotation);
                        tmp.GetComponent<Rigidbody>().velocity = Vector3.forward * velocityB;
                    }
                }
                if (EscudoB)
                {
                    escudo.SetActive(true);
                    armaRange.SetActive(false); 
                    armaMele.SetActive(false);
                    transform.localRotation = Quaternion.Euler(new Vector3(87.6100082f, 85.4600372f, 346.800049f));

                }
                //Buttons
                if (data.Length >= 5 && data[3].Contains("B1:") && data[4].Contains("B2:"))
                {
                    // Leer los datos de los botones
                    int buttonValue1, buttonValue2;

                    if (int.TryParse(data[3].Substring(3), out buttonValue1) &&
                        int.TryParse(data[4].Substring(3), out buttonValue2))
                    {

                        // Realizar acciones correspondientes a los botones
                        if (buttonValue1 == 1 && buttonValue2 == 0)
                        {
                            // Acción cuando el botón 1 está presionado y el botón 2 no
                            Debug.Log("Escudo");
                            EscudoB = true;
                            Arma = false;
                            Range = false;
                            StartCoroutine(TimeToChange());
                        
                        }
                        else if (buttonValue1 == 0 && buttonValue2 == 1)
                        {
                            // Acción cuando el botón 2 está presionado y el botón 1 no
                            Debug.Log("Arma");
                            EscudoB = false;
                            Arma = false;
                            Range = true;
                            StartCoroutine(TimeToChange());
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Error al convertir los valores de los botones a enteros.");
                    }
                }

                // AccelVector
                if (data.Length >= 6 && data[5].Contains("A:"))
                {
                    

                    if (float.TryParse(data[5].Substring(2), out accelVector))
                    {
                        // Realizar acciones correspondientes al valor del accelVector
                        Debug.Log("Valor del accelVector: " + accelVector);
                    }
                    else
                    {
                        Debug.LogWarning("Error al analizar el valor de accelVector desde Arduino.");
                    }
                }
                else
                {
                    Debug.LogWarning("Datos incompletos recibidos desde Arduino.");
                }

            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Error de lectura del puerto serie: " + e.Message);
            }
        }
    }

    void OnDestroy()
    {
        if (arduinoPort.IsOpen)
        {
            arduinoPort.Close();
        }
    }

    IEnumerator TimeToChange()
    {
        yield return new WaitForSecondsRealtime(10);
        EscudoB = false;
        Arma = true;
        Range = false;
    }
}
