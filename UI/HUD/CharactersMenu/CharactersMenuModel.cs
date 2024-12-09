using System;
using System.Collections.Generic;
using Core.Infrastructure.Enums;
using UI.Base.BaseMenu;
using UniRx;

namespace UI.HUD.CharactersMenu
{
    public class CharactersMenuModel : BaseMenuModel<CharactersMenuModel>
    {
        public readonly ReactiveDictionary<CharacterName, CharacterState> Characters = new()
        {
            { CharacterName.Deer , new CharacterState(false, new bool[6])},
            { CharacterName.Snail , new CharacterState(false, new bool[6])},
        };

        public readonly List<Action> TemporaryActions = new();

        public CharactersMenuModel()
        {
            Subject = new BehaviorSubject<CharactersMenuModel>(this);
        }
        public override void Update()
        {
            Subject.OnNext(this);
        }

        public void ClearCharacters()
        {
            foreach (var character in Characters)
            {
                Characters[character.Key] = new CharacterState(false, new bool[6]);
            }
        }
    }
}