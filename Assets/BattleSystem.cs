using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject[] enemyPrefabs = new GameObject[4];
    public int enemyindex = 0;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public BattleHud playerHUD;
    public BattleHud enemyHUD;

    public BattleState state;

    Unit playerUnit;
    Unit enemyUnit;

    public TextMeshProUGUI dialogueText;

    public int savedHp;
    public int savedMana;
    public bool IsFirstBattle = true;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine (SetupBattle());
    }
    IEnumerator SetupBattle()
    {
        if (enemyUnit != null)
            Destroy(enemyUnit.gameObject);

        GameObject playerGo = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGo.GetComponent<Unit>();
        GameObject enemyGo = Instantiate(enemyPrefabs[enemyindex], enemyBattleStation);
        enemyUnit = enemyGo.GetComponent<Unit>();

        dialogueText.text = "A " + enemyUnit.unitName + " approaches...";

        if (IsFirstBattle == false)
        {
            playerUnit.curHp = savedHp;
            playerUnit.curMana = savedMana;
        }
        playerHUD.setHUD(playerUnit);
        enemyHUD.setHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit.curHp);
        dialogueText.text = "The attack is successful!";
        
        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.curHp);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }
    IEnumerator EndBattle() 
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
            yield return new WaitForSeconds(2f);
            savedHp = playerUnit.curHp;
            savedMana = playerUnit.curMana;
            IsFirstBattle = false;

            enemyindex++;
            if (enemyindex < enemyPrefabs.Length)
            {
                StartCoroutine(SetupBattle());
            }
            else
                dialogueText.text = "You won the game!";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
            yield return new WaitForSeconds(2f);
        }
    }
    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
    }
    IEnumerator PlayerRest()
    {
        playerUnit.Rest(10,10);

        playerHUD.SetHP(playerUnit.curHp);
        playerHUD.setMana(playerUnit.curMana);
        dialogueText.text = "You feel refreshed!";

        yield return new WaitForSeconds(2f);
        state = BattleState.ENEMYTURN;

        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerMagic()
    {
        if (playerUnit.curMana >= 20)
        {
            bool isDead = enemyUnit.TakeDamage(30);
            enemyHUD.SetHP(enemyUnit.curHp);
            dialogueText.text = "Lightning bolt hit the enemy!";
            playerUnit.curMana -= 20;
            playerHUD.setMana(playerUnit.curMana);

            yield return new WaitForSeconds(2f);

            if (isDead)
            {
                state = BattleState.WON;
                StartCoroutine(EndBattle());
            }
            else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        }
        else
        {
            dialogueText.text = "Not enough mana";
            yield return new WaitForSeconds(2f);
        }
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerAttack());
    }
    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerRest());
    }

    public void OnMagicButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerMagic());
    }

}
