using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameManagement;

namespace Shred.Lib
{
    using MenuButtonExtensions;

    public class ListViewMenuControlItem : MenuButton
    {
        public virtual void SetSiblingIndex(int i)
        {
            if (i != base.transform.GetSiblingIndex())
            {
                base.transform.SetSiblingIndex(i);
            }
        }

        protected override void Start()
        {
            this.SetupStyles();
        }
    }
}
