using TMPro;
using UnityEngine;

public class EntityInfo : MonoBehaviour
{
    public Transform anchor, point;
    public float Height;
    public TMP_Text Text, PointsText;

    public GameObject leader, neutralize;

    private Camera m_Camera;

    void Awake()
    {
        m_Camera = Camera.main;

        PointsText.text = "No points";
    }

    void Update()
    {
        if (m_Camera != null && anchor != null)
        {
            point.LookAt(m_Camera.transform);
        }
    }

    public void Init(string name, float points, float h)
    {
        Text.text = $"{name} - {points}pts";
        Height = h;
        anchor.transform.localPosition = new Vector3(0, Height, 0);
    }

    public void SetLeader(bool isleader)
    {
        leader.SetActive(isleader);
    }

    public void SetNeutralized(bool neutralized)
    {
        neutralize.SetActive(neutralized);
    }

    public void SetPoints(int points)
    {
        PointsText.text = $"{points} point{(points > 1 ? "s" : "")}";
    }
}
