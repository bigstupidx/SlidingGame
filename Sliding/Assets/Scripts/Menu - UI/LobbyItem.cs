using UnityEngine;
public class LobbyItem : MonoBehaviour {

    public GameObject selectionOutline;
    public GameObject lockedSprite;


    public int price;
    [SerializeField]
    protected bool unlocked;
    [SerializeField]
    protected bool selected;

    protected void Awake()
    {
        Unlocked = Unlocked;
    }

    virtual public bool Unlocked
    {
        get
        {
            return unlocked;
        }

        set
        {
            unlocked = value;

            lockedSprite.SetActive(!unlocked);
        }
    }

    public bool Selected
    {
        get
        {
            return selected;
        }

        set
        {
            selected = value;
            selectionOutline.SetActive(selected);
        }
    }

}
