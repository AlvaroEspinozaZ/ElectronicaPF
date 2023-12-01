using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
public class Arduino : MonoBehaviour
{
    SerialPort arduinoPort = new SerialPort("COM5");
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
            string dataFromArduino = arduinoPort.ReadLine();

            Debug.Log("Datos recibidos del Arduino: " +

            dataFromArduino);

            string[] values = dataFromArduino.Split('/');

            if (values.Length == 3)
            {
                string z = values[0];
                string x = values[1];
                string y = values[2];
                //print(x+"s"+y+"s"+z);
                float x1 = float.Parse(x) / 100;
                float y1 = float.Parse(y) / 100;
                float z1 = float.Parse(z) / 100;
                Vector3 targetRotation = new Vector3(-x1, -y1, z1);
                transform.localRotation

                = Quaternion.Euler(targetRotation);

            }
        }
            catch (Exception e)
            {
                Debug.LogError("Error al leer datos del Arduino: " +

                e.Message);
            }
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector3(1, 0, 0);
    }
    void OnDestroy()
    {
        if (arduinoPort.IsOpen)
        {
            arduinoPort.Close();
        }
    }
}
