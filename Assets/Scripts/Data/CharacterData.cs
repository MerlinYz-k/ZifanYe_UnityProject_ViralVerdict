using UnityEngine;

[CreateAssetMenu(fileName = "DefaultCharacterData", menuName = "BeiFenVV/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
	public string displayName = "name";

	public string jobTitle;

	public Sprite avatar;

    public EUserType userType = EUserType.NormalUsers;
}

public enum EUserType
{
    NormalUsers,
    SeniorModerator
}
