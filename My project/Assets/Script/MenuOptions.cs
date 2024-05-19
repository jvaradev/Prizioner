using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//Código para controlar el Menú de opciones del juego
public class MenuOptions : MonoBehaviour
{
    [SerializeField] private AudioMixer audio;
    
    //Método para cambiar a pantalla completa o a ventana.
    public void FullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

    //Método para controlar el volumen del juego.
    public void ChangeVolume(float volume)
    {
        audio.SetFloat("Volume", volume);
    }

    //Método para cambiar la calidad del juego.
    public void ChangeQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
}
