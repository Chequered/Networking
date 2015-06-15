using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Drill : MonoBehaviour {

    private List<PlayerInfo> m_playersInZone;
    private float m_captureCooldown;

    private void Start()
    {
        m_playersInZone = new List<PlayerInfo>();
        m_captureCooldown = Time.time + 2;
    }

	private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.transform.tag == "Player")
        {
            m_playersInZone.Add(coll.GetComponent<PlayerInfo>());

            bool allSameTeam = true;
            for (int i = 0; i < m_playersInZone.Count; i++)
            {
                int teamA = m_playersInZone[i].TeamID;
                for (int j = 0; j < m_playersInZone.Count; j++)
                {
                    if(teamA != m_playersInZone[j].TeamID)
                    {
                        allSameTeam = false;
                    }
                }
            }
            if(!allSameTeam)
                coll.GetComponent<PlayerInfo>().StartCapture(GetComponent<BuildingInfo>());
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.transform.tag == "Player")
        {
            m_playersInZone.Remove(coll.GetComponent<PlayerInfo>());
        }
    }

    private void Update()
    {
        if(Time.time > m_captureCooldown)
        {
            foreach (PlayerInfo player in m_playersInZone)
            {
                player.StartCapture(GetComponent<BuildingInfo>());
                m_captureCooldown = Time.time + 2;
            }
        }
    }
}
