using System.Collections.Generic;
using UnityEngine;

public class CameraSpectating : MonoBehaviour
{
    public float followSpeed = 5f; // Vitesse de suivi de la caméra
    public float switchSpeed = 2f; // Vitesse du déplacement lors du changement de cible
    public Vector3 offset = new Vector3(0, 5, -10); // Décalage initial par rapport à l'entité suivie
    public Vector3 additionalOffset = Vector3.zero; // Offset dynamique supplémentaire

    public float minZoom = 5f;  // Distance de zoom minimale
    public float maxZoom = 20f; // Distance de zoom maximale
    public float zoomSpeed = 2f; // Sensibilité du zoom

    public float rotationSpeed = 2f; // Sensibilité de la rotation
    private Vector3 currentRotation = Vector3.zero; // Stocke la rotation actuelle

    private List<EntitySubject> entities = new List<EntitySubject>();
    private int currentIndex = 0;
    private Transform currentTarget;
    private float currentZoom;

    void Start()
    {
        currentZoom = offset.magnitude; // Initialisation du zoom
        UpdateEntityList();
        if (entities.Count > 0)
        {
            currentTarget = entities[currentIndex].transform;
            currentRotation = Quaternion.LookRotation(offset).eulerAngles; // Stocke la rotation initiale
        }
    }

    void Update()
    {
        // Supprime les entités nulles
        CleanEntityList();

        // Changer de cible avec les flèches gauche/droite
        if (Input.GetKeyDown(KeyCode.RightArrow)) SwitchTarget(1);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) SwitchTarget(-1);

        // Gestion du zoom avec la molette
        HandleZoom();

        // Gestion de la rotation avec le clic droit
        HandleRotation();

        // Suivre la cible actuelle
        if (currentTarget != null)
        {
            Quaternion rotation = Quaternion.Euler(currentRotation);
            Vector3 targetPosition = currentTarget.position + rotation * (Vector3.back * currentZoom) + additionalOffset;

            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            transform.LookAt(currentTarget.position + additionalOffset);
        }
    }

    void UpdateEntityList()
    {
        entities.Clear();
        entities.AddRange(FindObjectsByType<EntitySubject>(FindObjectsSortMode.None));
        Debug.Log($"[CameraSpectating] {entities.Count} entité(s) trouvée(s).");

        if (entities.Count > 0)
        {
            currentIndex = Mathf.Clamp(currentIndex, 0, entities.Count - 1);
            currentTarget = entities[currentIndex].transform;
        }
        else
        {
            currentTarget = null;
        }
    }

    void CleanEntityList()
    {
        entities.RemoveAll(entity => entity == null);
        if (entities.Count == 0) currentTarget = null;
    }

    void SwitchTarget(int direction)
    {
        if (entities.Count == 0) return;

        currentIndex = (currentIndex + direction + entities.Count) % entities.Count;
        currentTarget = entities[currentIndex].transform;
        Debug.Log($"[CameraSpectating] Nouvelle cible: {currentTarget.name}");
    }

    void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            currentZoom -= scrollInput * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
            Debug.Log($"[CameraSpectating] Zoom réglé à : {currentZoom}");
        }
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(1)) // Clic droit maintenu
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed; // Inversé pour une navigation intuitive

            currentRotation.y += mouseX;
            currentRotation.x = Mathf.Clamp(currentRotation.x + mouseY, -80f, 80f); // Empêche une inversion totale de l'axe vertical
        }
    }
}