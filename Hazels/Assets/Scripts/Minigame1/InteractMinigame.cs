
public class InteractMinigame : InteractiveObj
{
    public Minigame1 script;
    
    public override void Interact()
    {
        script.GetComponent<Minigame1>().StartMiniGame();
    }
}
