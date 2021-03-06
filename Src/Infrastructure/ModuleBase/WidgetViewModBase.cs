﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.ServiceLocation;
using Naver.Compass.Common.CommonBase;
using Naver.Compass.Service;
using Naver.Compass.Service.Document;

namespace Naver.Compass.InfoStructure
{
    public class WidgetViewModBase : WidgetViewModelDate, IWidgetPropertyData
    {
        public WidgetViewModBase()
        {
            IsSelected = false;
            //CanEdit = false;
            ParentID = Guid.Empty;
            _bSupportArrowStyle = false;
            ArrowWidth = 12;
        }

        #region Private member
        protected bool _bSupportRotate;
        protected bool _bSupportTextRotate;
        protected bool _bSupportBorder;
        protected bool _bSupportText;
        protected bool _bSupportTextVerAlign;
        protected bool _bSupportTextHorAlign;
        protected bool _bSupportBackground;
        protected bool _bSupportGradientBackground;
        protected bool _bSupportGradientBorderline;
        protected bool _bSupportArrowStyle;
        protected bool bIsSelected;
        private bool bCanEdit = false;
        private bool bIsGroup = false;
        private bool? bIsBrushModel = false;
        private Guid _parentGID;

        protected int ArrowWidth;
        #endregion

        #region Public Function and Property

        virtual public bool IsSupportBorber
        {
            get { return _bSupportBorder; }
        }
        virtual public bool IsSupportText
        {
            get { return _bSupportText; }
        }
        virtual public bool IsSupportTextVerAlign
        {
            get { return _bSupportTextVerAlign; }
        }
        virtual public bool IsSupportTextHorAlign
        {
            get { return _bSupportTextHorAlign; }
        }
        virtual public bool IsSupportBackground
        {
            get { return _bSupportBackground; }
        }
        virtual public bool IsSupportGradientBackground
        {
            get { return _bSupportGradientBackground; }
        }
        virtual public bool IsSupportGradientBorderline
        {
            get { return _bSupportGradientBorderline; }
        }
        virtual public bool IsSupportRotate
        {
            get { return _bSupportRotate; }
        }
        virtual public bool IsSupportTextRotate
        {
            get { return _bSupportTextRotate; }
        }
        virtual public bool IsSupportArrowStyle
        {
            get { return _bSupportArrowStyle; }
        }
        public Guid widgetGID
        {
            get;
            set;
        }
        public bool IsChildPageOpened
        {
            get;
            set;
        }
        virtual public Rect GetBoundingRectangle()
        {
            double x1 = Double.MaxValue;
            double y1 = Double.MaxValue;
            double x2 = Double.MinValue;
            double y2 = Double.MinValue;

            double left = this.Left;
            double top = this.Top;
            double itemWidth = this.ItemWidth;
            double itemHeight = this.ItemHeight;

            if (this.RotateAngle == 0)
            {
                x1 = left;
                y1 = top;
                x2 = left + itemWidth;
                y2 = top + itemHeight;
            }
            else
            {
                double x = left;
                double y = top;
                double xc = (left * 2 + itemWidth) / 2;
                double yc = (top * 2 + itemHeight) / 2;
                double angle = Math.Abs(this.RotateAngle) % 180;
                if (angle > 90)
                {
                    angle = 180 - angle;
                }
                double Kc = Math.Cos(angle * Math.PI / 180);
                double Ks = Math.Sin(angle * Math.PI / 180);

                double xr = xc - (Kc * (xc - x) + Ks * (yc - y));
                double yr = yc - (Ks * (xc - x) + Kc * (yc - y));
                double width = itemWidth + (x - xr) * 2;
                double height = itemHeight + (y - yr) * 2;

                x1 = xr;
                y1 = yr;
                x2 = xr + width;
                y2 = yr + height;
            }
            return new Rect(new System.Windows.Point(x1, y1), new System.Windows.Point(x2, y2));
        }
        public Rect GetBoundingRectangle(int childAngle, Rect childRec)
        {
            double x1 = Double.MaxValue;
            double y1 = Double.MaxValue;
            double x2 = Double.MinValue;
            double y2 = Double.MinValue;


            if (childAngle == 0)
            {
                x1 = childRec.Left;
                y1 = childRec.Top;
                x2 = childRec.Right;
                y2 = childRec.Bottom;
            }
            else
            {
                double x = childRec.Left;
                double y = childRec.Top;
                double xc = childRec.Left + childRec.Width / 2.0;
                double yc = childRec.Top + childRec.Height / 2.0;
                double angle = Math.Abs(childAngle) % 180;
                if (angle > 90)
                {
                    angle = 180 - angle;
                }
                double Kc = Math.Cos(angle * Math.PI / 180);
                double Ks = Math.Sin(angle * Math.PI / 180);

                double xr = xc - (Kc * (xc - x) + Ks * (yc - y));
                double yr = yc - (Ks * (xc - x) + Kc * (yc - y));
                double width = childRec.Width + (x - xr) * 2;
                double height = childRec.Height + (y - yr) * 2;

                x1 = xr;
                y1 = yr;
                x2 = xr + width;
                y2 = yr + height;
            }
            return new Rect(new System.Windows.Point(x1, y1), new System.Windows.Point(x2, y2));
        }
        virtual public Rect RevertBoundingRectangle(Rect BoundingRec)
        {
            double x1 = Double.MaxValue;
            double y1 = Double.MaxValue;
            double x2 = Double.MinValue;
            double y2 = Double.MinValue;


            if (this.RotateAngle == 0)
            {
                x1 = BoundingRec.Left;
                y1 = BoundingRec.Top;
                x2 = BoundingRec.Right;
                y2 = BoundingRec.Bottom;
            }
            else
            {
                double angle = Math.Abs(this.RotateAngle) % 180;
                if (angle > 90)
                {
                    angle = 180 - angle;
                }
                if (angle == 45)
                {
                    Rect OriginRec = GetBoundingRectangle();
                    double rate = BoundingRec.Height / OriginRec.Height;

                    double xc = BoundingRec.Left + (BoundingRec.Width / 2);
                    double yc = BoundingRec.Top + (BoundingRec.Height / 2);
                    double width = ItemWidth * rate;
                    double height = ItemHeight * rate;

                    x1 = xc - 0.5 * width;
                    x2 = xc + 0.5 * width;
                    y1 = yc - 0.5 * height;
                    y2 = yc + 0.5 * height;
                }
                else
                {
                    double Kc = Math.Cos(angle * Math.PI / 180);
                    double Ks = Math.Sin(angle * Math.PI / 180);
                    double w = BoundingRec.Width;
                    double h = BoundingRec.Height;
                    double xc = BoundingRec.Left + (BoundingRec.Width / 2);
                    double yc = BoundingRec.Top + (BoundingRec.Height / 2);

                    double width = (w * Kc - h * Ks) / (Math.Pow(Kc, 2) - Math.Pow(Ks, 2));
                    double height = (w * Ks - h * Kc) / (Math.Pow(Ks, 2) - Math.Pow(Kc, 2));

                    x1 = xc - 0.5 * width;
                    x2 = xc + 0.5 * width;
                    y1 = yc - 0.5 * height;
                    y2 = yc + 0.5 * height;
                }

            }
            return new Rect(new System.Windows.Point(x1, y1), new System.Windows.Point(x2, y2));
        }
        #endregion

        #region binding operation properry
        virtual public bool IsSelected
        {
            get { return bIsSelected; }
            set
            {
                if (bIsSelected != value)
                {
                    bIsSelected = value;
                    if (bIsSelected == false)
                    {
                        CanEdit = false;
                        SelectionService.RemoveWidget(this);
                    }
                    else
                    {
                        SelectionService.RegisterWidget(this);
                    }
                    FirePropertyChanged("IsSelected");
                }
            }
        }
        virtual public bool Raw_IsSelected
        {
            set
            {
                IsSelected = value;
            }
        }
        virtual public ICommand DoubleClickCommand
        {
            get
            {
                return null;
            }
        }

        public bool CanEdit
        {
            get { return bCanEdit; }
            set
            {
                if (bCanEdit != value)
                {
                    bCanEdit = value;
                    FirePropertyChanged("CanEdit");
                }
            }
        }
        public bool? IsBrushModel
        {
            get { return bIsBrushModel; }
            set
            {
                if (bIsBrushModel != value)
                {
                    bIsBrushModel = value;
                    FirePropertyChanged("IsBrushModel");
                }
            }
        }

        public Guid ParentID
        {
            get { return _parentGID; }
            set
            {
                if (_parentGID != value)
                {
                    if (value == Guid.Empty)
                    {
                        IsHiddenModeInGroup = false;
                    }
                    _parentGID = value;
                    FirePropertyChanged("ParentID");
                }
            }
        }
        public bool IsGroup
        {
            get { return bIsGroup; }
            set
            {
                if (bIsGroup != value)
                {

                    bIsGroup = value;
                    FirePropertyChanged("IsGroup");
                }
            }
        }

        private string _widgetTypeName;
        public string WidgetTypeName
        {
            get { return _widgetTypeName; }
            set
            {
                _widgetTypeName = value;
            }
        }

        private bool _isHiddenModeInGroup;
        virtual public bool IsHiddenModeInGroup
        {

            get
            {

                return _isHiddenModeInGroup;
            }
            set
            {
                if (_isHiddenModeInGroup != value)
                {
                    _isHiddenModeInGroup = value;
                }
                FirePropertyChanged("IsHiddenModeInGroup");
            }
        }

        /// <summary>
        /// Whether show border if the widget is hidden in a group
        /// </summary>
        /// <param name="bShow"></param>
        public void ShowBorderInGroup(bool bShow)
        {
            if (bShow && (ParentID != Guid.Empty) && !IsShowInPageView2Adaptive)
            {
                IsHiddenModeInGroup = true;
            }
            else
            {
                IsHiddenModeInGroup = false;
            }

            FirePropertyChanged("IsHiddenModeInGroup");
        }
        #endregion binding operation properry
    }

    public class WidgetRotateViewModBase : WidgetViewModBase
    {
        public WidgetRotateViewModBase()
        {

        }

        //Adorner Style
        public bool IsRotatedStyle
        {
            get { return true; }
        }
    }

    public class WidgetLineViewModeBase : WidgetRotateViewModBase
    {
        public WidgetLineViewModeBase(IWidget widget)
        {
            _model = new LineModel(widget);
        }

        #region Override Functions
        override protected void UpdateWidgetStyle2UI()
        {
            base.UpdateWidgetStyle2UI();
            UpdateBackgroundStyle();
            FirePropertyChanged("LineArrowStyle");
            FirePropertyChanged("PathDataMain");
            FirePropertyChanged("PathDataExtern");
        }

        public override Rect GetBoundingRectangle()
        {
            this.IsActual = true;
            return base.GetBoundingRectangle();
        }

        #endregion

        #region Binding line style property
        virtual public ArrowStyle LineArrowStyle
        {
            get
            {
                return (_model as LineModel).LineArrowStyle;
            }
            set
            {
                if ((_model as LineModel).LineArrowStyle != value)
                {
                    (_model as LineModel).LineArrowStyle = value;
                    FirePropertyChanged("LineArrowStyle");
                }
            }
        }
        #endregion
    }
}
