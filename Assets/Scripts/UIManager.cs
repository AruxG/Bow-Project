using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Items
{
    public int quantity;
    public string name;
    public float weight;
    public int value;
    public Items(string n, float w, int v)
    {
        name = n;
        weight = w;
        value = v;
    }
}
public class UIManager : MonoBehaviour
{
    #region Variables
    List<Items> potions = new List<Items>(4);
    List<Items> keys = new List<Items>();
    List<Items> others= new List<Items>(5);
    int gold;
    float GlobalWeight;

    [SerializeField] GameObject character;
    [SerializeField] GameObject staminaBoost;
    [SerializeField] GameObject healthBoost;

    [SerializeField] GameObject Main;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject Map;

    #region PauseMenuUI
    [SerializeField] GameObject ExitPanel;
    [SerializeField] GameObject ControlsPanel;
    [SerializeField] GameObject StatisticsPanel;
    [SerializeField] Button[] pauseButtons;
    #endregion
    #region InventoryUI
    [SerializeField] Text goldText;
    [SerializeField] Text weightText;
    [SerializeField] Canvas Inventory;
    
    //[SerializeField] CanvasGroup HUD;
    [SerializeField] GameObject[] HUD= new GameObject[4];
    [SerializeField] GameObject HealthBar;
    [SerializeField] GameObject StaminaBar;
    [SerializeField] GameObject AuxHBar;
    [SerializeField] GameObject AuxSBar;

    [SerializeField] GameObject potionPanel;
    [SerializeField] Button potionButton;
    [SerializeField] Button HealthPotionButton;
    [SerializeField] Button StaminaPotionButton;
    [SerializeField] Button HealingPotionButton;
    [SerializeField] Button BoostPotionButton;

    [SerializeField] GameObject keysPanel;
    [SerializeField] Button keysButton;
    [SerializeField] Text keysButtonText;

    [SerializeField] GameObject weaponPanel;
    [SerializeField] Button weaponButton;
    [SerializeField] Button arrowButton;
    [SerializeField] Button daggerButton;

    [SerializeField] GameObject otherPanel;
    [SerializeField] Button otherButton;
    [SerializeField] Button fireRuneButton;
    [SerializeField] Button venomRuneButton;
    [SerializeField] Button ghostRuneButton;
    [SerializeField] Button daggerRuneButton;
    [SerializeField] Button bowRuneButton;

    [SerializeField] GameObject[] items;

    private Vector3 hpos;
    private Vector3 spos;
    private Vector3 hUIpos= new Vector3(24,-18.7f,0);
    private Vector3 sUIpos= new Vector3(12.4f,-23.5f,0);
    #endregion
    #region MapUI
    [SerializeField] Button[] iconButtons;
    [SerializeField] GameObject Alert;
    [SerializeField] Text AlertText;
    [SerializeField] Image map;
    [SerializeField] RawImage map2;
    float minZoom = 0.8f;
    float maxZoom = 1.5f;
    float zoom = 0.8f;
    #endregion
    #endregion
    #region generalFunctions
    void Awake()
    {
        hpos=HealthBar.transform.Find("Health").transform.localPosition;
        spos=StaminaBar.transform.Find("Stamine").transform.localPosition;
        potions.Add(new Items("Health Potion", 0.5f,20));
        potions.Add(new Items("Healing Potion", 0.5f,10));
        potions.Add(new Items("Stamine Potion", 0.5f,10));
        potions.Add(new Items("Boost Potion", 0.5f,20));

        others.Add(new Items("Bow Rune", 0.5f,0));
        others.Add(new Items("Dagger Rune", 0.5f,0));
        others.Add(new Items("Fire Rune", 0.5f,0));
        others.Add(new Items("Ghost Rune", 0.5f,0));
        others.Add(new Items("Venom Rune", 0.5f,0));


        keysButtonText.text = "None";
        daggerButton.interactable = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        AddItem(new Items("Health Potion", 0.5f,20), potions);
        AddItem(new Items("Healing Potion", 0.5f,10), potions);
        AddItem(new Items("Healing Potion", 0.5f,10), potions);
        AddItem(new Items("Stamine Potion", 0.5f,10), potions);
        AddItem(new Items("Boost Potion", 0.5f,20), potions);

        AddItem(new Items("Cave key", 0,0), keys);
        AddItem(new Items("Cave key", 0,0), keys);
        AddItem(new Items("Dwarf key", 0,0), keys);

        AddItem(new Items("Bow Rune", 0.5f,0), others);
        AddItem(new Items("Dagger Rune", 0.5f,0), others);
        AddItem(new Items("Fire Rune", 0.5f,0), others);
        AddItem(new Items("Ghost Rune", 0.5f,0), others);
        AddItem(new Items("Venom Rune", 0.5f,0), others);
        IncreaseGold(2000);
    }

    // Update is called once per frame
    public void ShowInventory()
    {
        Time.timeScale = 0;
       
        foreach (GameObject g in HUD)
        {
            g.SetActive(false);
        }
        HealthBar.GetComponent<Image>().enabled = false;
        foreach (Image i in HealthBar.GetComponentsInChildren<Image>())
        {
            i.enabled = false;
        }
        StaminaBar.GetComponent<Image>().enabled = false;
        foreach (Image i in StaminaBar.GetComponentsInChildren<Image>())
        {
            i.enabled = false;
        }
        HealthBar.transform.Find("Health").transform.localPosition = hUIpos;
        StaminaBar.transform.Find("Stamine").transform.localPosition = sUIpos;
        Inventory.gameObject.SetActive(true);
        if (Map.activeSelf) { HideMap(); }
        goldText.text = "Gold: " + gold;
        weightText.text = "Weight:" + " " + GlobalWeight + "/" + character.GetComponent<Controller>().stamineMax * 2f;
    }
    public void HideInventory()
    {
        if (!Main.activeSelf && !PauseMenu.activeSelf && !Map.activeSelf) { Time.timeScale = 1; }
        
        potionPanel.SetActive(false);
        keysPanel.SetActive(false);
        weaponPanel.SetActive(false);
        otherPanel.SetActive(false);
        potionButton.interactable = true;
        keysButton.interactable = true;
        weaponButton.interactable = true;
        otherButton.interactable = true;
        HealingPotionButton.interactable = true;
        HealthPotionButton.interactable = true;
        BoostPotionButton.interactable = true;
        StaminaPotionButton.interactable = true;

        for (int i = 0; i < items.Length; i++)
        {
            items[i].SetActive(false);
        }
        foreach (GameObject g in HUD)
        {
            g.SetActive(true);
        }
        HealthBar.GetComponent<Image>().enabled = true;
        foreach (Image i in HealthBar.GetComponentsInChildren<Image>())
        {
            i.enabled = true;
        }
        StaminaBar.GetComponent<Image>().enabled = true;
        foreach (Image i in StaminaBar.GetComponentsInChildren<Image>())
        {
            i.enabled = true;
        }
        Inventory.gameObject.SetActive(false);
        HealthBar.transform.Find("Health").transform.localPosition = hpos;
        StaminaBar.transform.Find("Stamine").transform.localPosition = spos;
    }
    public void ShowMain()
    {
        Time.timeScale = 0;
        Main.SetActive(true);
        if (Inventory.gameObject.activeSelf) { HideInventory(); }
        if (Map.gameObject.activeSelf) { HideMap(); }
        foreach (GameObject g in HUD)
        {
            g.SetActive(false);
        }
        HealthBar.GetComponent<Image>().enabled = false;
        foreach (Image i in HealthBar.GetComponentsInChildren<Image>())
        {
            i.enabled = false;
        }
        StaminaBar.GetComponent<Image>().enabled = false;
        foreach (Image i in StaminaBar.GetComponentsInChildren<Image>())
        {
            i.enabled = false;
        }
        
    }
    public void HideMain()
    {
        if (!PauseMenu.activeSelf) { Time.timeScale = 1; }
        foreach (GameObject g in HUD)
        {
            g.SetActive(true);
        }
        HealthBar.GetComponent<Image>().enabled = true;
        foreach (Image i in HealthBar.GetComponentsInChildren<Image>())
        {
            i.enabled = true;
        }
        StaminaBar.GetComponent<Image>().enabled = true;
        foreach (Image i in StaminaBar.GetComponentsInChildren<Image>())
        {
            i.enabled = true;
        }
        Main.SetActive(false);
    }
    public void ShowMap()
    {
        Time.timeScale = 0;
        Map.SetActive(true);
        CancelTP();
        /*iconButtons[0].gameObject.SetActive(GameObject.FindObjectOfType<ReloadControlController>().dungeonIcon);
        iconButtons[1].gameObject.SetActive(GameObject.FindObjectOfType<ReloadControlController>().fireIcon);
        iconButtons[2].gameObject.SetActive(GameObject.FindObjectOfType<ReloadControlController>().signIcon);
        iconButtons[3].gameObject.SetActive(GameObject.FindObjectOfType<ReloadControlController>().lumberIcon);*/
        if (Inventory.gameObject.activeSelf) { HideInventory(); }
        foreach (GameObject g in HUD)
        {
            g.SetActive(false);
        }
        HealthBar.GetComponent<Image>().enabled = false;
        foreach (Image i in HealthBar.GetComponentsInChildren<Image>())
        {
            i.enabled = false;
        }
        StaminaBar.GetComponent<Image>().enabled = false;
        foreach (Image i in StaminaBar.GetComponentsInChildren<Image>())
        {
            i.enabled = false;
        }

    }
    public void HideMap()
    {
        if (!Main.activeSelf && !PauseMenu.activeSelf && !Inventory.gameObject.activeSelf) { Time.timeScale = 1; }
        if (!Inventory.gameObject.activeSelf)
        {
            foreach (GameObject g in HUD)
            {
                g.SetActive(true);
            }
            HealthBar.GetComponent<Image>().enabled = true;
            foreach (Image i in HealthBar.GetComponentsInChildren<Image>())
            {
                i.enabled = true;
            }
            StaminaBar.GetComponent<Image>().enabled = true;
            foreach (Image i in StaminaBar.GetComponentsInChildren<Image>())
            {
                i.enabled = true;
            }
        }
        Map.SetActive(false);
    }
    public void ShowPauseMenu()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        if (Inventory.gameObject.activeSelf) { HideInventory(); }
        if (Map.gameObject.activeSelf) { HideMap(); }
        if (Main.activeSelf) { HideMain(); }
        foreach (GameObject g in HUD)
        {
            g.SetActive(false);
        }
        HealthBar.GetComponent<Image>().enabled = false;
        foreach (Image i in HealthBar.GetComponentsInChildren<Image>())
        {
            i.enabled = false;
        }
        StaminaBar.GetComponent<Image>().enabled = false;
        foreach (Image i in StaminaBar.GetComponentsInChildren<Image>())
        {
            i.enabled = false;
        }
    }
    public void HidePauseMenu()
    {
        Time.timeScale = 1;
        ExitPanel.SetActive(false);
        ControlsPanel.SetActive(false);
        StatisticsPanel.SetActive(false);
        for (int i = 0; i < pauseButtons.Length; i++) { pauseButtons[i].interactable = true; }
        foreach (GameObject g in HUD)
        {
            g.SetActive(true);
        }
        HealthBar.GetComponent<Image>().enabled = true;
        foreach (Image i in HealthBar.GetComponentsInChildren<Image>())
        {
            i.enabled = true;
        }
        StaminaBar.GetComponent<Image>().enabled = true;
        foreach (Image i in StaminaBar.GetComponentsInChildren<Image>())
        {
            i.enabled = true;
        }
        PauseMenu.SetActive(false);
    }
    void Update()
    {
        if (!character.GetComponent<Animator>().GetBool("Shooting"))
        {
            if (Input.GetKeyDown(KeyCode.I) && !Main.activeSelf && !PauseMenu.activeSelf)
            {
                if (!Inventory.gameObject.activeSelf)
                {
                    ShowInventory();
                }
                else
                {
                    HideInventory();
                }
            }
            if (Input.GetKeyDown(KeyCode.M) && !Main.activeSelf && !PauseMenu.activeSelf)// && SceneManager.GetActiveScene().name == "Forest")
            {
                if (!Map.gameObject.activeSelf)
                {
                    ShowMap();
                }
                else
                {
                    HideMap();
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab) && !PauseMenu.activeSelf)
            {
                if (!Main.activeSelf)
                {
                    ShowMain();
                }
                else
                {
                    HideMain();
                }
            }
            if (Input.GetKeyUp(KeyCode.Tab) && !PauseMenu.activeSelf && Main.activeSelf)
            {
                HideMain();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!PauseMenu.activeSelf)
                {
                    ShowPauseMenu();
                }
                else
                {
                    HidePauseMenu();
                }
            }
        }
        if (!potionPanel.activeSelf)
        {
            for (int i = 0; i < 4; i++)
            {
                items[i].SetActive(false);
            }
        }
        if (staminaBoost.activeSelf)
        {
            var s = staminaBoost.GetComponent<Image>().color;
            s.a -= Time.deltaTime * 1 / 60;
            staminaBoost.GetComponent<Image>().color = s;
        }
        else
        {
            staminaBoost.GetComponent<Image>().color = new Color(staminaBoost.GetComponent<Image>().color.r, staminaBoost.GetComponent<Image>().color.g, staminaBoost.GetComponent<Image>().color.b, 1);
        }
        if (healthBoost.activeSelf)
        {
            var s = healthBoost.GetComponent<Image>().color;
            s.a -= Time.deltaTime * 1 / 60;
            healthBoost.GetComponent<Image>().color = s;
        }
        else
        {
            healthBoost.GetComponent<Image>().color = new Color(healthBoost.GetComponent<Image>().color.r, healthBoost.GetComponent<Image>().color.g, healthBoost.GetComponent<Image>().color.b, 1);
        }
        if (Map.activeSelf)
        {
            Zoom(Input.GetAxis("Mouse ScrollWheel")*Time.unscaledDeltaTime*20f);
        }
    }
    void Zoom(float increment)
    {
        zoom += increment;
        if (zoom >= maxZoom)
        {
            zoom = maxZoom;
        }
        else if (zoom <= minZoom)
        {
            zoom = minZoom;
        }
        ///if (SceneManager.GetActiveScene().name=="Forest") {
            map.rectTransform.localScale = new Vector2(zoom, zoom);
        /*}
        else
        {
            map2.rectTransform.localScale = new Vector2(zoom, zoom);
        }*/
    }
    public void showItem(int item)
    {
        if (potions[1].quantity > 0) { HealingPotionButton.interactable = true; }
        if (potions[2].quantity > 0) { HealthPotionButton.interactable = true; }
        if (potions[0].quantity > 0) { BoostPotionButton.interactable = true; }
        if (potions[3].quantity > 0) { StaminaPotionButton.interactable = true; }
        EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
        for (int i = 0; i < items.Length; i++)
        {
            items[i].SetActive(false);
        }
        items[item].SetActive(true);
    }
    public void showPotionPanel()
    {
        if (potions[1].quantity > 0) { HealingPotionButton.interactable = true; }
        if (potions[2].quantity > 0) { HealthPotionButton.interactable = true; }
        if (potions[0].quantity > 0) { BoostPotionButton.interactable = true; }
        if (potions[3].quantity > 0) { StaminaPotionButton.interactable = true; }

        potionButton.interactable = false;
        keysButton.interactable = true;
        otherButton.interactable = true;
        weaponButton.interactable = true;

        potionPanel.SetActive(true);
        keysPanel.SetActive(false);
        otherPanel.SetActive(false);
        weaponPanel.SetActive(false);

        if (potions[0] == null|| potions[0].quantity == 0)
        {
            BoostPotionButton.interactable = false;
        }
        else if(potions[0] != null)
        {BoostPotionButton.GetComponent<Text>().text = "Boost Potion(" + potions[0].quantity + ")";}

        if (potions[1] == null || potions[1].quantity == 0)
        {
            HealingPotionButton.interactable = false;
        }
        else if (potions[1] != null)
        { HealingPotionButton.GetComponent<Text>().text = "Healing Potion(" + potions[1].quantity + ")"; }

        if (potions[2] == null || potions[2].quantity == 0)
        {
            HealthPotionButton.interactable = false;
        }
        else if (potions[2] != null)
        { HealthPotionButton.GetComponent<Text>().text = "Health Potion(" + potions[2].quantity + ")"; }

        if (potions[3] == null || potions[3].quantity == 0)
        {
            StaminaPotionButton.interactable = false;
        }
        else if (potions[3] != null)
        { StaminaPotionButton.GetComponent<Text>().text = "Stamina Potion(" + potions[3].quantity + ")"; }
    }
    public void showOtherPanel()
    {
        otherButton.interactable = false;
        potionButton.interactable = true;
        keysButton.interactable = true;
        weaponButton.interactable = true;

        otherPanel.SetActive(true);
        potionPanel.SetActive(false);
        keysPanel.SetActive(false);
        weaponPanel.SetActive(false);

        HealthBar.transform.Find("Health").GetComponent<Image>().enabled = false;
        StaminaBar.transform.Find("Stamine").GetComponent<Image>().enabled = false;

        if (others[0] == null|| others[0].quantity == 0)
        {
            bowRuneButton.interactable = false;
        }else if(others[0]!=null)
        {bowRuneButton.GetComponent<Text>().text = "Bow Rune(" + others[0].quantity + ")";}

        if (others[1] == null || others[1].quantity == 0)
        {
            daggerRuneButton.interactable = false;
        }
        else if (others[1] != null)
        { daggerRuneButton.GetComponent<Text>().text = "Dagger Rune(" + others[1].quantity + ")"; }

        if (others[2] == null || others[2].quantity == 0)
        {
            fireRuneButton.interactable = false;
        }
        else if (others[2] != null)
        { fireRuneButton.GetComponent<Text>().text = "Fire Rune(" + others[2].quantity + ")"; }

        if (others[3] == null || others[3].quantity == 0)
        {
            ghostRuneButton.interactable = false;
        }
        else if (others[3] != null)
        { ghostRuneButton.GetComponent<Text>().text = "Ghost Rune(" + others[3].quantity + ")"; }

        if (others[4] == null || others[4].quantity == 0)
        {
            venomRuneButton.interactable = false;
        }
        else if (others[4] != null)
        { venomRuneButton.GetComponent<Text>().text = "Venom Rune(" + others[4].quantity + ")"; }
    }
    public void showWeaponsPanel()
    {
        weaponButton.interactable = false;
        keysButton.interactable = true;
        potionButton.interactable = true;
        otherButton.interactable = true;

        weaponPanel.SetActive(true);
        keysPanel.SetActive(false);
        otherPanel.SetActive(false);
        potionPanel.SetActive(false);

        HealthBar.transform.Find("Health").GetComponent<Image>().enabled = false;
        StaminaBar.transform.Find("Stamine").GetComponent<Image>().enabled = false;

        arrowButton.GetComponent<Text>().text= "Arrows("+character.GetComponent<Controller>().numArrows+")";
    }
    public void showKeysPanel()
    {
        keysButton.interactable = false;
        potionButton.interactable = true;
        weaponButton.interactable = true;
        otherButton.interactable = true;

        keysPanel.SetActive(true);
        potionPanel.SetActive(false);
        otherPanel.SetActive(false);
        weaponPanel.SetActive(false);

        HealthBar.transform.Find("Health").GetComponent<Image>().enabled = false;
        StaminaBar.transform.Find("Stamine").GetComponent<Image>().enabled = false;

        if (keys.Count != 0)
        {
            keysButtonText.text = "";
            foreach (Items i in keys)
            {
                keysButtonText.text += i.name + "(" + i.quantity + ")\n";
            }
        }
    }
    public void AddItem(Items item, List<Items> list)
    {
        if (!list.Exists(p => p.name == item.name))
        {
            list.Add(item);
        }
        list.Find(p => p.name == item.name).quantity++;
        IncreaseWeight(item);
        list.Sort(delegate (Items i1, Items i2)
        {
            return i1.name.CompareTo(i2.name);
        });
    }
    public void RemoveItem(Items item, List<Items> list)
    {
        list.Find(p => p.name == item.name).quantity--;
        DecreaseWeight(item);
    }
    public void IncreaseGold(int g)
    {
        gold += g;
    }
    public void DecreaseGold(int g)
    {
        if (gold - g >= 0)
        {
            gold -= g;
        }
        else
        {
            //indicar que no tiene suficiente oro
        }
    }
    void IncreaseWeight(Items item)
    {
        GlobalWeight += item.weight;
        weightText.text = "Weight:" + " " + GlobalWeight + "/" + character.GetComponent<Controller>().stamineMax * 2f;
        if (GlobalWeight>= character.GetComponent<Controller>().stamineMax * 2f)
        {
            character.GetComponent<Controller>().overweight = true;
        }
    }
    void DecreaseWeight(Items item)
    {
        GlobalWeight -= item.weight;
        //item.quantity--;
        weightText.text = "Weight:" + " " + GlobalWeight + "/" + character.GetComponent<Controller>().stamineMax * 2f;
        if (GlobalWeight < character.GetComponent<Controller>().stamineMax * 2f)
        {
            character.GetComponent<Controller>().overweight = false;
        }
    }
    #endregion
    #region Potion operations
    public void Health(bool use)
    {
        RemoveItem(potions[2],potions);
        if (potions[2].quantity == 0) { HealthPotionButton.interactable = false;
            items[2].transform.Find("Use").GetComponent<Button>().interactable = false;
            items[2].transform.Find("Drop").GetComponent<Button>().interactable = false;
        }
        HealthPotionButton.GetComponent<Text>().text = "Health Potion(" + potions[2].quantity + ")";
        if (use)
        {
            /*AuxHBar.SetActive(true);
            AuxSBar.SetActive(false);*/
            HealthBar.transform.Find("Health").GetComponent<Image>().enabled = true;
            StaminaBar.transform.Find("Stamine").GetComponent<Image>().enabled = false;
            StartCoroutine(health());
        }
    }
    IEnumerator health()
    {
        healthBoost.SetActive(true);
        var aux = character.GetComponent<Controller>().maxHealth;
        character.GetComponent<Controller>().maxHealth += 0.1f * character.GetComponent<Controller>().maxHealth;
        yield return new WaitForSeconds(60);
        healthBoost.SetActive(false);
        character.GetComponent<Controller>().maxHealth = aux;
    }
    public void Healing(bool use)
    {
        RemoveItem(potions[1], potions);
        if (potions[1].quantity == 0)
        {
            HealingPotionButton.interactable = false;
            items[1].transform.Find("Use").GetComponent<Button>().interactable = false;
            items[1].transform.Find("Drop").GetComponent<Button>().interactable = false;
        }
        HealingPotionButton.GetComponent<Text>().text = "Healing Potion(" + potions[1].quantity + ")";
        if (use)
        {
            /*AuxHBar.SetActive(true);
            AuxSBar.SetActive(false);*/
            HealthBar.transform.Find("Health").GetComponent<Image>().enabled = true;
            StaminaBar.transform.Find("Stamine").GetComponent<Image>().enabled = false;
            character.GetComponent<Controller>().health = Mathf.Min(character.GetComponent<Controller>().health + character.GetComponent<Controller>().maxHealth * 10 / 100, character.GetComponent<Controller>().maxHealth);
        }
    }
    public void Boost(bool use)
    {
        RemoveItem(potions[0], potions);
        if (potions[0].quantity == 0) { BoostPotionButton.interactable = false;
            items[0].transform.Find("Use").GetComponent<Button>().interactable = false;
            items[0].transform.Find("Drop").GetComponent<Button>().interactable = false;
        }
        BoostPotionButton.GetComponent<Text>().text = "Boost Potion(" + potions[0].quantity + ")";
        if (use)
        {
            /*AuxHBar.SetActive(false);
            AuxSBar.SetActive(true);*/
            HealthBar.transform.Find("Health").GetComponent<Image>().enabled = false;
            StaminaBar.transform.Find("Stamine").GetComponent<Image>().enabled = true;
            StartCoroutine(boost());
        }
    }
    IEnumerator boost()
    {
        staminaBoost.SetActive(true);
        
        var aux = character.GetComponent<Controller>().stamineMax;
        character.GetComponent<Controller>().stamineMax += 0.1f * character.GetComponent<Controller>().stamineMax;
        weightText.text = "Weight:" + " " + GlobalWeight + "/" + character.GetComponent<Controller>().stamineMax * 2f;
        yield return new WaitForSeconds(60);

        staminaBoost.SetActive(false);
        character.GetComponent<Controller>().stamineMax = aux;
        weightText.text = "Weight:" + " " + GlobalWeight + "/" + character.GetComponent<Controller>().stamineMax * 2f;
    }
    public void Stamina(bool use)
    {
        RemoveItem(potions[3], potions);
        if (potions[3].quantity == 0) { StaminaPotionButton.interactable = false;
            items[3].transform.Find("Use").GetComponent<Button>().interactable = false;
            items[3].transform.Find("Drop").GetComponent<Button>().interactable = false;
        }
        StaminaPotionButton.GetComponent<Text>().text = "Stamina Potion(" + potions[3].quantity + ")";
        if (use)
        {
            /*AuxHBar.SetActive(false);
            AuxSBar.SetActive(true);*/
            HealthBar.transform.Find("Health").GetComponent<Image>().enabled = false;
            StaminaBar.transform.Find("Stamine").GetComponent<Image>().enabled = true;
            character.GetComponent<Controller>().stamine = Mathf.Min(character.GetComponent<Controller>().stamine + character.GetComponent<Controller>().stamineMax * 10 / 100, character.GetComponent<Controller>().stamineMax);
        }
    }
    #endregion
    #region Rune operations
    //fire Rune
    //ghost rune
    //venom rune
    //bow rune
    //dagger rune
    #endregion
    #region PauseMenu Funcions
    public void Resume()
    {
        HidePauseMenu();
    }
    public void Exit()
    {
        for (int i = 0; i < pauseButtons.Length; i++) { pauseButtons[i].interactable = true; }
        ControlsPanel.SetActive(false);
        StatisticsPanel.SetActive(false);
        EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
        ExitPanel.SetActive(true);
    }
    public void MainMenu()
    {
        HidePauseMenu();
        StartCoroutine(GameObject.FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, "MainMenu"));
    }
    public void Desktop()
    {
        Application.Quit();
    }
    public void Cancel(GameObject panel)
    {
        panel.SetActive(false);
        for (int i = 0; i < pauseButtons.Length; i++) { pauseButtons[i].interactable = true; }
    }
    public void Controls()
    {
        for (int i = 0; i < pauseButtons.Length; i++) { pauseButtons[i].interactable = true; }
        ExitPanel.SetActive(false);
        StatisticsPanel.SetActive(false);
        EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
        ControlsPanel.SetActive(true);
    }
    public void Save()
    {

    }
    public void Load()
    {

    }
    public void Stats()
    {
        for (int i = 0; i < pauseButtons.Length; i++) { pauseButtons[i].interactable = true; }
        ExitPanel.SetActive(false);
        ControlsPanel.SetActive(false);
        EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
        StatisticsPanel.SetActive(true);
        StatisticsPanel.GetComponentInChildren<Text>().text= 
            "Level: 0 \nHealth: "+ character.GetComponent<Controller>().maxHealth + "\nStamina :"+ character.GetComponent<Controller>().stamineMax + "\nBow: \n\tBase Damage: \n\tFire Damage: \n\tVenom Damage: \n\tGhost Damage: \n\tKills: \nDagger: \n\tDamage: \n\tKills:";
    }


    #endregion
    #region Map Functions
    public void Close()
    {
        HideMap();
    }
    public void AskTP(int id)
    {
        string Place="";
        for(int i=0; i< iconButtons.Length; i++)
        {
            iconButtons[i].interactable = false;
        }
        Alert.SetActive(true);
        switch (id)
        {
            case 5:
                Place = "FireRange";
                break;
            case 6:
                Place = "SignPost";
                break;
            case 7:
                Place = "Lumber Camp";
                break;
            case 8:
                Place = "Dwarf Dungeon";
                break;
        }
        AlertText.text = "Do you want to travel to "+Place+"?";
        GameObject.FindObjectOfType<ReloadControlController>().doorID = id;
        /*.fireIcon = iconButtons[1].gameObject.activeSelf;
        GameObject.FindObjectOfType<ReloadControlController>().lumberIcon = iconButtons[3].gameObject.activeSelf;
        GameObject.FindObjectOfType<ReloadControlController>().signIcon = iconButtons[2].gameObject.activeSelf;
        GameObject.FindObjectOfType<ReloadControlController>().dungeonIcon = iconButtons[0].gameObject.activeSelf;*/
    }
    public void ConfirmTP()
    {
        HideMap();
        StartCoroutine(GameObject.FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, "Forest"));
    }
    public void CancelTP()
    {
        Alert.SetActive(false);
        for (int i = 0; i < iconButtons.Length; i++)
        {
            iconButtons[i].interactable = true;
        }
    }

    #endregion
}
