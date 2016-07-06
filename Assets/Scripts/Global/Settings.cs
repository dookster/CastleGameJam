using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class Settings : ScriptableObject
{

    public float playerMoveSpeed = 0.5f;
    public float playerTurnSpeed = 0.5f;

    public float creatureMoveSpeed = 0.5f;

    public float cameraZoomSpeed = 1f;

    public GameObject[] tilePrefabs;

    /// <summary>
    /// 0 : empty
    /// 1 : end line
    /// 2 : straight
    /// 3 : corner
    /// 4 : t - section
    /// 5 : cross
    /// </summary>
    public GameObject[] puzzlePiece;


    [Header("Sound")]
    public AudioClip switch1Audio;
    public AudioClip switch2Audio;

    public AudioClip[] footstepAudio;
    public AudioClip[] turnAudio;

    public AudioClip openHeadAudio;
    public AudioClip closeHeadAudio;

    public AudioClip openDoorAudio;
    public AudioClip swingWeaponAudio;
    public AudioClip pickUpAudio;

    public AudioClip creatureWalkAudio;
}
