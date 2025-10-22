using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading;

public class CardsController : MonoBehaviour
{
    private MemoryGameManager gameManager;

    [SerializeField] Card_Memory cardPrefab;
    [SerializeField] Transform gridTransform;
    [SerializeField] Sprite[] sprites;

    private List<Sprite> spritePairs = new List<Sprite>();

    private Card_Memory firstSelected;
    private Card_Memory secondSelected;
    private bool canSelect = false;

    public List<Card_Memory> cards = new List<Card_Memory>();

    private int pairsToUse = 3; // valor por defecto

    private void Start()
    {
        gameManager = Object.FindFirstObjectByType<MemoryGameManager>();
    }

    //  Llamado por el GameManager con el nivel actual
    public void StartGame(int level)
    {
        DontDestroyOnLoad(gameObject);
        ClearOldCards();

        //  Definir cantidad de pares según nivel
        switch (level)
        {
            case 1: pairsToUse = 3; break;
            case 2: pairsToUse = 4; break;
            case 3: pairsToUse = 5; break;
            default: pairsToUse = 3; break;
        }

        PrepareSprites();
        CreateCards();

        Debug.Log("Starting ... Show All Then Hide");
        //StartCoroutine(ShowAllThenHide());

        Debug.Log("Show.");
        foreach (Transform child in gridTransform)
        {
            Card_Memory card = child.GetComponent<Card_Memory>();
            card.Show();
        }

        Debug.Log("Wait...");

        StartCoroutine(WaitThenShow());

    }

    IEnumerator WaitThenShow()
    {
        Debug.Log("In Method. Waiting now...");
        yield return new WaitForSeconds(2f);

        Debug.Log("After Wait. Now Calling Hide.");

        Hide();
    }


    private void Hide()
    {
        Debug.Log("In Hide. Hiding...");

        foreach (Transform child in gridTransform)
        {
            Card_Memory card = child.GetComponent<Card_Memory>();
            card.Hide();
        }

        canSelect = true;

        //done
        Debug.Log("Done.");
    }

    IEnumerator ShowAllThenHide()
    {
        Debug.Log("In Show All Then Hide");
        foreach (Transform child in gridTransform)
        {
            Card_Memory card = child.GetComponent<Card_Memory>();
            card.Show();
        }

        yield return new WaitForSeconds(2f);

        foreach (Transform child in gridTransform)
        {
            Card_Memory card = child.GetComponent<Card_Memory>();
            card.Hide();
        }

        canSelect = true;
    }



    private void PrepareSprites()
    {
        spritePairs.Clear();

        int count = Mathf.Min(pairsToUse, sprites.Length);

        for (int i = 0; i < count; i++)
        {
            spritePairs.Add(sprites[i]);
            spritePairs.Add(sprites[i]);
        }

        ShuffleSprites(spritePairs);
    }

    void CreateCards()
    {
        foreach (Sprite sp in spritePairs)
        {
            Card_Memory card = Instantiate(cardPrefab, gridTransform);
            card.SetIconSprite(sp);
            card.controller = this;
            card.Hide();

            cards.Add(card);
        }

        Debug.Log($"Total de cartas generadas: {cards.Count}");
    }

    private void ClearOldCards()
    {
        foreach (Transform child in gridTransform)
        {
            Destroy(child.gameObject);
        }

        cards.Clear();
        firstSelected = null;
        secondSelected = null;
        canSelect = false;
    }

    public void SetSelected(Card_Memory card)
    {
        if (!canSelect || card.isSelect) return;

        card.Show();

        if (firstSelected == null)
        {
            firstSelected = card;
        }
        else
        {
            secondSelected = card;
            canSelect = false;
            StartCoroutine(CheckMatching());
        }
    }

    IEnumerator CheckMatching()
    {
        yield return new WaitForSeconds(0.5f);

        if (firstSelected.iconSprite == secondSelected.iconSprite)
        {
            gameManager.OnPairMatched();

            firstSelected.GetComponent<UnityEngine.UI.Button>().interactable = false;
            secondSelected.GetComponent<UnityEngine.UI.Button>().interactable = false;

            firstSelected.isSelect = true;
            secondSelected.isSelect = true;
        }
        else
        {
            gameManager.OnPairMistake();//throwing error

            firstSelected.Hide();
            secondSelected.Hide();

            firstSelected.isSelect = false;
            secondSelected.isSelect = false;
        }

        firstSelected = null;
        secondSelected = null;
        canSelect = true;
    }

    void ShuffleSprites(List<Sprite> spriteList)
    {
        for (int i = spriteList.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Sprite temp = spriteList[i];
            spriteList[i] = spriteList[randomIndex];
            spriteList[randomIndex] = temp;
        }
    }
}
