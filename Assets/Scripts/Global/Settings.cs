using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class Settings : ScriptableObject
{

    public float playerMoveSpeed = 0.5f;
    public float playerTurnSpeed = 0.5f;

    public GameObject[] tilePrefabs;

}
