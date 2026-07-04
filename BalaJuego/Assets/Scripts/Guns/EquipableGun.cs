using TMPro;
using UnityEngine;

public class EquipableGun : BaseGun, IInteractable
{
    public void setHover(bool set)
    {
        print("setHover" + set);
        int setI = set ? 1 : 0;
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        GetComponentInChildren<SpriteRenderer>().GetPropertyBlock(block, 0);
        block.SetInt("_isOutlined", setI);
        print(GetComponentInChildren<SpriteRenderer>().name);
        GetComponentInChildren<SpriteRenderer>().SetPropertyBlock(block, 0);
        musicManager.Instance.PlaySoundPitch("bip", 0.2f);
        if (!set)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void tryGrab(PlayerShoot player)
    {
        player.getInteractableObject(gameObject);
        bulletCount = player.GetComponentInChildren<TMP_Text>();
        setBullets(startBullets);
    }
}