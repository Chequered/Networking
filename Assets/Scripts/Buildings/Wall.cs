using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour, BuildingInterface
{
	private int _health = 100;
	
	void Start () 
	{
	
	}


	void Update () 
	{
		TakeDmg ();
		OnDeath ();
	
	}

	public void TakeDmg()
	{

	}

	public void OnDeath()
	{

	}
}
