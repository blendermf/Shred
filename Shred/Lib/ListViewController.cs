using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Shred.Lib
{
    [RequireComponent(typeof(ScrollRect))]
    public class ListViewController : MonoBehaviour
    {
        public bool AutoScrollToSelected = true;

        private ScrollRect _scrollRect;

        private List<ListViewMenuControlItem> _listViewItems;

        void OnItemSelected()
        {

        }

        public List<ListViewMenuControlItem> ControlViews {
            get {
                if (this._listViewItems == null)
                {
                    this._listViewItems = new List<ListViewMenuControlItem>();
                }
                return this._listViewItems;
            }

            private set { 
                this._listViewItems = value; 
            }
        }

        private ScrollRect ScrollRect {
            get {
                if (this._scrollRect == null)
                {
                    this._scrollRect = base.GetComponent<ScrollRect>();
                }
                return this._scrollRect;
            }
        }

        protected virtual void Update()
        {
            RectTransform targetRect = new RectTransform();
            if (this.AutoScrollToSelected && EventSystem.current != null 
                && EventSystem.current.currentSelectedGameObject != null
                && (targetRect = (EventSystem.current.currentSelectedGameObject.transform as RectTransform)) != null)
            {
                this.MoveRectTransformToVisible(targetRect);
            }
        }

        private void MoveRectTransformToVisible(RectTransform targetRect)
        {
            if (targetRect.parent != this.ScrollRect.content)
            {
                return;
            }
            Canvas.ForceUpdateCanvases();
            Vector2 rectMinPosLocal = targetRect.rect.min + (Vector2)targetRect.localPosition;
            Vector2 rectMinPosGlobal = rectMinPosLocal + (Vector2)this.ScrollRect.content.localPosition;
            Vector2 rectMaxPosLocal = targetRect.rect.max + (Vector2)targetRect.localPosition;
            Vector2 rectMaxPosGlobal = rectMaxPosLocal + (Vector2)this.ScrollRect.content.localPosition;
            Vector2 minOffsetFromViewport = rectMinPosGlobal - this.ScrollRect.viewport.rect.min;
            Vector2 maxOffsetFromViewport = rectMaxPosGlobal - this.ScrollRect.viewport.rect.max;

            if (this.ScrollRect.vertical)
            {
                if (minOffsetFromViewport.y < 0f)
                {
                    this.ScrollRect.verticalNormalizedPosition = (rectMinPosLocal.y - this.ScrollRect.content.rect.min.y) /
                                                                    (this.ScrollRect.content.rect.size.y - this.ScrollRect.viewport.rect.size.y);
                }
                else if (maxOffsetFromViewport.y > 0f)
                {
                    this.ScrollRect.verticalNormalizedPosition = 1f - (this.ScrollRect.content.rect.max.y - rectMaxPosLocal.y) /
                                                                    (this.ScrollRect.content.rect.size.y - this.ScrollRect.viewport.rect.size.y);
                }
            }

            if (this.ScrollRect.vertical)
            {
                if (minOffsetFromViewport.x < 0f)
                {
                    this.ScrollRect.horizontalNormalizedPosition = (rectMinPosLocal.x - this.ScrollRect.content.rect.min.x) /
                                                                    (this.ScrollRect.content.rect.size.x - this.ScrollRect.viewport.rect.size.x);
                }
                if (maxOffsetFromViewport.x > 0f)
                {
                    this.ScrollRect.verticalNormalizedPosition = 1f - (this.ScrollRect.content.rect.max.x - rectMaxPosLocal.x) /
                                                                    (this.ScrollRect.content.rect.size.x - this.ScrollRect.viewport.rect.size.x);
                }
            }
        }

        public virtual void UpdateList()
        {
            int i = 0;
            foreach (ListViewMenuControlItem item in this.ControlViews)
            {
                item.SetSiblingIndex(i++);
            }
        }

        public void AddControl(ListViewMenuControlItem item)
        {
            this.ControlViews.Add(item);
            this.UpdateList();
        }

        public void RemoveControl(ListViewMenuControlItem item)
        {
            this.ControlViews.Remove(item);
            this.UpdateList();
        }
    }
}
