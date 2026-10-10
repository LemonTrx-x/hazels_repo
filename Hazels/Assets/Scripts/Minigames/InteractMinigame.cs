
public class InteractMinigame : InteractiveObj
{
    public override void Interact()
    {
        Minigames.Instance.StartMiniGame();
    }
}
