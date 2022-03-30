using UnityEngine;
using BTs;

[CreateAssetMenu(fileName = "BT_Anita", menuName = "Behaviour Trees/BT_Anita", order = 1)]
public class BT_Anita : BehaviourTree
{
    /* If necessary declare BT parameters here. 
       All public parameters must be of type string. All public parameters must be
       regarded as keys in/for the blackboard context.
       Use prefix "key" for input parameters (information stored in the blackboard that must be retrieved)
       use prefix "keyout" for output parameters (information that must be stored in the blackboard)

       e.g.
       public string keyDistance;
       public string keyoutObject 
     */
    [Header("Costumers")]
    public string deskKey = "theFrontOfDesk";
    public string costumerKey = "costumer";

    [Header("EFFECTS")]
    public string broomKey = "theBroom";
    public string singKey = "theNotes";

    [Header("WANDER")]
    public string keyAttractor = "theSweepingPoint";
    public string keySeekWeight = "0.5";



     // construtor
    public BT_Anita()  { 
        /* Receive BT parameters and set them. Remember all are of type string */
    }
    
    public override void OnConstruction()
    {
        /* Write here (method OnConstruction) the code that constructs the Behaviour Tree 
           Remember to set the root attribute to a proper node
           e.g.
            ...
            root = new SEQUENCE();
            ...

          A behaviour tree can use other behaviour trees.  
      */
        
        root = new DynamicSelector();
        root.AddChild(new CONDITION_CustomerInStore(costumerKey), Costumer());
        root.AddChild(new CONDITION_AlwaysTrue(), SweepAndSing());
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
            new ACTION_Arrive(deskKey));
        //see costumer
    }
}
