using UnityEngine;

public class FinalTrigger : MonoBehaviour
{
    [SerializeField] private FinalBoss bossScript;
    [SerializeField] private PlayerCon playerCon;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered) return; // agar hanya trigger 1x

        if (collision.CompareTag("Player"))
        {
            hasTriggered = true;

            if (bossScript != null)
            {
                bossScript.GetComponent<Animator>().SetBool("canMove", true);
                Debug.Log("Boss mulai bergerak!");
            }
            else
            {
                Debug.LogError("FinalBoss belum di-drag ke Inspector!");
            }

            if (playerCon != null)
            {
                Debug.Log("Player terdeteksi, siap bertarung!");
            }
            else
            {
                Debug.LogError("PlayerCon belum diisi!");
            }

            Destroy(gameObject); // hilangkan trigger setelah dipicu
        }
    }
}
