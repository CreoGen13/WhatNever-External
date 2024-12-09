namespace UI.HUD.CharactersMenu
{
    public struct CharacterState
    {
        public readonly bool IsActive;
        public readonly bool[] Features;

        public CharacterState(bool isActive, bool[] features)
        {
            IsActive = isActive;
            Features = features;
        }
    }
}