using ImageCircle.Forms.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trade.Models.TableViewModel;
using Trade.Style;
using Xamarin.Forms;

namespace Trade.Views
{

        public class Profile : ContentPage
        {
            public Profile(MyProfile myProfile = null)
            {

                Title = "Profile";
                NavigationPage.SetHasNavigationBar(this, true);
                BackgroundColor = Color.White;

                var backgroundImage = new Image()
                {
                    BackgroundColor = Color.FromHex("#F57C00"),
                    Aspect = Aspect.AspectFill,
                    //IsOpaque = false,
                    //Opacity = 0.8,
                };

                var shader = new BoxView()
                {
                    Color = Color.FromHex("#F57C00").MultiplyAlpha(.5)
                };

                var face = new CircleImage
                {
                    BorderColor = Color.White,
                    BorderThickness = 2,
                    HeightRequest = 100,
                    WidthRequest = 100,
                    Aspect = Aspect.AspectFill,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Source =
                        new UriImageSource { Uri = new Uri(myProfile.avatar), CacheValidity = TimeSpan.FromDays(15) },
                };





                var details = new DetailsView(myProfile);
                var slideshow = new ShowProfileDetailsView();



                RelativeLayout relativeLayout = new RelativeLayout()
                {
                    HeightRequest = 100,
                };

                relativeLayout.Children.Add(
                    backgroundImage,
                    Constraint.Constant(0),
                    Constraint.Constant(0),
                    Constraint.RelativeToParent((parent) =>
                    {
                        return parent.Width;
                    }),
                    Constraint.RelativeToParent((parent) =>
                    {
                        return parent.Height * .35;
                    })
                );

                relativeLayout.Children.Add(
                    shader,
                    Constraint.Constant(0),
                    Constraint.Constant(0),
                    Constraint.RelativeToParent((parent) =>
                    {
                        return parent.Width;
                    }),
                    Constraint.RelativeToParent((parent) =>
                    {
                        return parent.Height * .35;
                    })
                );

                relativeLayout.Children.Add(
                    face,
                    Constraint.RelativeToParent((parent) =>
                    {
                        return ((parent.Width / 2) - (face.Width / 2));
                    }),
                    Constraint.RelativeToParent((parent) =>
                    {
                        return parent.Height * .1;
                    }),
                    Constraint.RelativeToParent((parent) =>
                    {
                        return parent.Width * .5;
                    }),
                    Constraint.RelativeToParent((parent) =>
                    {
                        return parent.Width * .5;
                    })
                );



                relativeLayout.Children.Add(
                        slideshow,
                        Constraint.Constant(0),
                        Constraint.RelativeToView(details, (parent, view) =>
                        {
                            return view.Y + view.Height;
                        }),
                        Constraint.RelativeToParent((parent) =>
                        {
                            return parent.Width;
                        }),
                        Constraint.RelativeToView(details, (parent, view) =>
                        {
                            var detailsbottomY = view.Y + view.Height;
                            return parent.Height - detailsbottomY;
                        })
                    );

                face.SizeChanged += (sender, e) =>
                {
                    relativeLayout.ForceLayout();
                };

                this.Content = relativeLayout;

            }
        }
}