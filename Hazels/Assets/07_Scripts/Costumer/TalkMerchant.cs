using UnityEngine;

public class TalkMerchant : InteractiveObj
{
    private Merchant merchant;
    
    public override void Interact()
    {
        merchant = GetComponent<Merchant>();
        Debug.Log("I DO NOT HAVE ANYTHING OF YOUR INTEREST, PEASANT!");
        merchant.finished = true;
    }
}
