using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamarinApp
{
  public partial class MainPage : ContentPage
  {
    public MainPage()
    {
      InitializeComponent();
    }

    async void OnToggled(object sender, ToggledEventArgs e)
    {
      if(e.Value)
      {
        await Flashlight.TurnOnAsync();
      }
      else
      {
        await Flashlight.TurnOffAsync();
      }
    }
  }
}
