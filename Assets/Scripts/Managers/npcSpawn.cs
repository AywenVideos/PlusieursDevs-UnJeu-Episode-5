using UnityEngine;

public class npcSpawn : MonoBehaviour
{
	public GameObject npc;

	void Start()
	{
		npc.SetActive(false);
	}

	public void SpawnNpc()
	{
		npc.SetActive(true);
	}
}
