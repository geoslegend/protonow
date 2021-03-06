﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Naver.Compass.Service.Document;
using System.Windows;

namespace Naver.Compass.Service.Html
{
    [Serializable]
    internal struct ListSerialItem
    {
        public bool IsSelected;
        public string Text;
    }


    [Serializable]
    internal class SerialListBox : SerialWidgetBase
    {
        [NonSerialized]
        private IListBox _element;

        #region public interface functions
        public SerialListBox(IListBox wdg) : base(wdg)
        {
            _element = wdg;
            InitializeProperty();
        }

        public void Update(IListBox newWdg)
        {
            _element = newWdg;
            base.Update(newWdg);
            InitializeProperty();
        }
        #endregion

        #region private functions
        private void InitializeProperty()
        {
            ShapeType = _element.WidgetType;
            Tooltip = _element.Tooltip;
            IsDisabled = _element.IsDisabled;
            Name = _element.Name;

            AllowMultiple=_element.AllowMultiple;
            Items = new List<ListSerialItem>();
            Items.Clear();

            //inner class object can't be md5 calculate through bin stream.
            for (int i=0;i< _element.Items.Count;i++)
            {
                ListSerialItem it = new ListSerialItem();
                it.IsSelected = _element.Items[i].IsSelected;
                it.Text = _element.Items[i].TextValue;
                Items.Add(it);
            }
  
        }
        #endregion

        #region Serialize Data
        public WidgetType ShapeType;
        public string Tooltip;
        public bool IsDisabled;
        public string Name;

        public bool AllowMultiple;
        public List<ListSerialItem> Items;
        #endregion
    }
}
