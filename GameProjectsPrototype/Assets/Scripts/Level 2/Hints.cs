using Assets.Scripts.UI;
using UnityEngine;

public class Hints : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private TutorialUI _tutorialUI;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        _tutorialUI.SetHint("Too dark? Maybe you can make it brighter?");
    }
}
