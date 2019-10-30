using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shred.Lib
{
    public class ListViewMenuControlItem : MenuButton
    {
        public virtual void SetSiblingIndex(int i)
        {
            if (i != base.transform.GetSiblingIndex())
            {
                base.transform.SetSiblingIndex(i);
            }
        }
    }
}
