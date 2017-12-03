﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trade.Views;
using Xamarin.Forms;

namespace Trade.Style
{
    public class MenuCell : ViewCell
    {
        public string Text
        {
            get { return label.Text; }
            set { label.Text = value; }
        }
        Label label;
        public string Notify
        {
            get { return notify.Text; }
            set { notify.Text = value; }
        }
        Label notify;

        public ImageSource ImageSrc
        {
            get { return image.Source; }
            set { image.Source = value; }
        }
        Image image;
        public MenuPage Host { get; set; }

        public MenuCell()
        {
            image = new Image
            {
                HeightRequest = 20,
                WidthRequest = 20,
            };

            image.Opacity = 0.5;
            // image.SetBinding(Image.SourceProperty, ImageSrc);

            label = new Label
            {
                YAlign = TextAlignment.Center,
                TextColor = Color.Gray,
            };

            notify = new Label
            {
                YAlign = TextAlignment.Center,
                TextColor = Color.Red,
            };


            var layout = new StackLayout
            {
               
                BackgroundColor = Color.White,

                Padding = new Thickness(20, 0, 0, 0),
                Orientation = StackOrientation.Horizontal,
                Spacing = 20,
                
                Children = { image, label, notify }
            };
            View = layout;
        }

        protected override void OnTapped()
        {
            base.OnTapped();

            Host.Selected(label.Text);
        }
    }
}
