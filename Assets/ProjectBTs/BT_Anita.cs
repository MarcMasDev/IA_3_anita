using UnityEngine;
using BTs;

[CreateAssetMenu(fileName = "BT_Anita", menuName = "Behaviour Trees/BT_Anita", order = 1)]
public class BT_Anita : BehaviourTree
{
    [Header("Costumers")]
    public string deskKey = "theFrontOfDesk";
    public string customerKey = "costumer";

    [Header("EFFECTS")]
    public string broomKey = "theBroom";
    public string singKey = "theNotes";

    [Header("WANDER")]
    public string keyAttractor = "theSweepingPoint";
    public string keySeekWeight = "0.5";

    [Header("AskCostumer")]
    public string keyFruit = "fruit";
    public string keyProductRequested = "productRequested";

    // construtor
    public BT_Anita() {}

    public override void OnConstruction()
    {
        root = new DynamicSelector();
        
        root.AddChild(new CONDITION_CustomerInStore(customerKey), Costumer());
        root.AddChild(new CONDITION_AlwaysTrue(), SweepAndSing());

        root = new RepeatForeverDecorator(root);
    }

    private Sequence SweepAndSing()
    {
        return new Sequence(
            new ACTION_ClearUtterance(),
            new ACTION_Activate(broomKey),
            new ACTION_Activate(singKey),
            new ACTION_WanderAround(keyAttractor, keySeekWeight));
    }
    private Sequence Costumer()
    {
        return new Sequence(
            new ACTION_Deactivate(broomKey),
            new ACTION_Deactivate(singKey),
            new ACTION_Utter("10", "3"),
            new ACTION_Arrive(deskKey),
            AskCustomer());
    }

    private Sequence AskCustomer()
    {
        Selector selector = new Selector(
           new Sequence(
               new ACTION_ParseAnswer(keyFruit, keyProductRequested), new ACTION_TellEngaged("13", "3"), SellProduct()),
           new ACTION_TellEngaged("12", "3"));

        Sequence seq_AskCustomer = new Sequence(new ACTION_EngageInDialog(customerKey),  new ACTION_AskEngaged("11", "3", keyFruit), selector, new ACTION_DisengageFromDialog());
        return seq_AskCustomer;
    }

    private Selector SellProduct()
    {
        Selector sel_SellProduct = new Selector(
            new Sequence(
                new CONDITION_CheckExistences(keyProductRequested), new ACTION_Sell(keyProductRequested), new ACTION_TellEngaged("14","3")),
            new ACTION_TellEngaged("15", "3"));

        return sel_SellProduct;
    }
}
