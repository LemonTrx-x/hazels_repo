
public class StartMinigame : InteractiveObj
{
    public Minigame1 script;
    
    public override void Interact()
    {
        script.GetComponent<Minigame1>().enabled = true;
    }
}
