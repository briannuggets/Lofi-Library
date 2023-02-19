using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Script for displaying dialogue on NPC click
public class NPCOnClick : MonoBehaviour, IPointerClickHandler
{
    private GameObject player;
    private NPC npc;
    [SerializeField]
    private List<string> chatLines;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        npc = GetComponent<NPC>();
    }

    // Displays chat lines on right-click
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            if (Vector2.Distance(player.transform.position, transform.position) < 2f) {
                string line = chatLines[Random.Range(0, chatLines.Count)];
                npc.OnClickChat(line);
            }
        }
    }
}
