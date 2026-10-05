using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerCardIconLibrary",
    menuName = "Heroes Landing/Icon Libraries/Player Card Icons"
)]
public class PlayerCardIconLibrary : ScriptableObject
{
    [Header("Role Icons")]

    public Sprite vanguardIcon;
    public Sprite tankIcon;
    public Sprite rangerIcon;

    [Header("Class Icons")]

    public Sprite barbarianIcon;
    public Sprite knightIcon;
    public Sprite fighterClassIcon;
    public Sprite mageIcon;
    public Sprite priestIcon;
    public Sprite rangerClassIcon;
    public Sprite gunnerIcon;
    public Sprite mechIcon;

    [Header("Weapon Class Icons")]

    public Sprite swordsmanIcon;
    public Sprite axemanIcon;
    public Sprite spearmanIcon;
    public Sprite marksmanIcon;
    public Sprite archerIcon;
    public Sprite fighterWeaponIcon;
    public Sprite assassinIcon;

    [Header("Weapon Level Box")]

    public Sprite whiteLevelBox;

    public Sprite GetRoleIcon(PlayerCardRole selectedRole)
    {
        switch (selectedRole)
        {
            case PlayerCardRole.Vanguard:
                return vanguardIcon;

            case PlayerCardRole.Tanker:
                return tankIcon;

            case PlayerCardRole.Ranger:
                return rangerIcon;

            default:
                return null;
        }
    }

    public Sprite GetClassIcon(PlayerCardClass selectedClass)
    {
        switch (selectedClass)
        {
            case PlayerCardClass.Barbarian:
                return barbarianIcon;

            case PlayerCardClass.Knight:
                return knightIcon;

            case PlayerCardClass.Fighter:
                return fighterClassIcon;

            case PlayerCardClass.Mage:
                return mageIcon;

            case PlayerCardClass.Priest:
                return priestIcon;

            case PlayerCardClass.Ranger:
                return rangerClassIcon;

            case PlayerCardClass.Gunner:
                return gunnerIcon;

            case PlayerCardClass.Mech:
                return mechIcon;

            default:
                return null;
        }
    }

    public Sprite GetWeaponIcon(
        PlayerWeaponClass selectedWeaponClass
    )
    {
        switch (selectedWeaponClass)
        {
            case PlayerWeaponClass.Swordsman:
                return swordsmanIcon;

            case PlayerWeaponClass.Axeman:
                return axemanIcon;

            case PlayerWeaponClass.Spearman:
                return spearmanIcon;

            case PlayerWeaponClass.Marksman:
                return marksmanIcon;

            case PlayerWeaponClass.Archer:
                return archerIcon;

            case PlayerWeaponClass.Fighter:
                return fighterWeaponIcon;

            case PlayerWeaponClass.Assassin:
                return assassinIcon;

            default:
                return null;
        }
    }
}