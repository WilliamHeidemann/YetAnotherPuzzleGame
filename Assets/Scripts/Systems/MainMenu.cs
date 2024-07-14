using System.Collections.Generic;
using System.Linq;
using Components;
using Model;
using UnityEngine;
using UnityEngine.Events;
using UnityUtils;
using UtilityToolkit.Editor;
using UtilityToolkit.Runtime;
using Animator = Animation.Animator;

namespace Systems
{
    public class MainMenu : Singleton<MainMenu>
    {
        public UnityEvent onWorldSelected;
        public UnityEvent onLevelSelected;
        public UnityEvent onMenuSelected;

        public void MenuSelected() => onMenuSelected?.Invoke();
    }
}