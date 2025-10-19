using Unity.Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Lista de c�maras Cinemachine")]
    public CinemachineCamera[] cameras;

    private int currentCameraIndex = 0;

    void Start()
    {
        // Activar solo la primera c�mara al inicio
        SwitchCamera(currentCameraIndex);
    }

    void Update()
    {
        //// Avanzar con tecla D
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    NextCamera();
        //}

        //// Retroceder con tecla A
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    PreviousCamera();
        //}
    }

    // M�todo para avanzar a la siguiente c�mara
    public void NextCamera()
    {
        currentCameraIndex++;
        if (currentCameraIndex >= cameras.Length)
            currentCameraIndex = 0;

        SwitchCamera(currentCameraIndex);
    }

    // M�todo para retroceder a la c�mara anterior
    public void PreviousCamera()
    {
        currentCameraIndex--;
        if (currentCameraIndex < 0)
            currentCameraIndex = cameras.Length - 1;

        SwitchCamera(currentCameraIndex);
    }

    // Activar la c�mara actual y desactivar las dem�s
    private void SwitchCamera(int index)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == index);
        }

        Debug.Log($"C�mara activa: {cameras[index].name}");
    }
}
