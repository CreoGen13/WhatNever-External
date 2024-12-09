using System;
using Core.Infrastructure.Enums;
using Core.Infrastructure.Enums.GameVariables;
using UI.Base.BaseMenu;
using UniRx;

namespace UI.HUD.PencilMenu
{
    public class PencilMenuModel : BaseMenuModel<PencilMenuModel>
    {
        public bool ShouldShake;
        public bool IsShaking => ShouldShake && MenuAnimationState != MenuAnimationState.Opened;
        
        public readonly ReactiveDictionary<ItemVariable, ItemState> Pencils = new()
        {
            { ItemVariable.Stick , new ItemState(false, null)},
            { ItemVariable.PencilBlue , new ItemState(false, null)},
            { ItemVariable.PencilGreen , new ItemState(false, null)},
            { ItemVariable.PencilRed , new ItemState(false, null)},
            { ItemVariable.PencilYellow , new ItemState(false, null)},
        };

        public PencilMenuModel()
        {
            Subject = new BehaviorSubject<PencilMenuModel>(this);
        }

        public override void Update()
        {
            Subject.OnNext(this);
        }

        public void Clear()
        {
            foreach (var variable in Enum.GetValues(typeof(ItemVariable)))
            {
                var castedVariable = (ItemVariable)variable;
                
                if (Pencils.ContainsKey(castedVariable))
                {
                    Pencils[castedVariable] = new ItemState(false, null);
                }
            }

            ShouldShake = false;
        }
    }
}