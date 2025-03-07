using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance;
    public GameObject playerPrefab;
    public Transform spawnPoint;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnPlayer(CharacterData data)
    {
        GameObject player = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        PlayerCharacter character = player.GetComponent<PlayerCharacter>();

        if (character != null)
        {
            character.SetupCharacter(data);
        }
    }
}
