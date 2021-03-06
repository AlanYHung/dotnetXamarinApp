﻿using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamarinApp
{
  public partial class MainPage : ContentPage
  {
    uint BatteryLifeInSeconds { get; set; }
    double LastCharge { get; set; }
    int ElapsedTime { get; set; }

    public MainPage()
    {
      InitializeComponent();
      BatteryLifeInSeconds = 18000;
      LastCharge = 1F;
      ElapsedTime = 0;

      Battery.BatteryInfoChanged += Battery_BatteryInfoChanged;

      //https://www.youtube.com/watch?v=eDx8tbUrkP0
      Device.StartTimer(TimeSpan.FromSeconds(1), () => {
        Device.BeginInvokeOnMainThread(() =>
          DisplayCountdown()
        );

        return true;
      });

      Magnetometer.Start(SensorSpeed.UI);
    }

    void Magnetometer_ReadingChanged(object sender, MagnetometerChangedEventArgs e)
    {
      var data = e.Reading;
      float xValue = data.MagneticField.X;

      xReading.Text = xValue.ToString();
    }

    void DisplayCountdown()
    {
      int hours = Convert.ToInt32(BatteryLifeInSeconds / 60 / 60 % 60);
      int minutes = Convert.ToInt32(BatteryLifeInSeconds / 60 % 60);
      int seconds = Convert.ToInt32(BatteryLifeInSeconds % 60);

      digitalclock.Text = hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");
      BatteryLifeInSeconds--;
      ElapsedTime++;
    }

    void Battery_BatteryInfoChanged(object sender, BatteryInfoChangedEventArgs e)
    {
      double level = e.ChargeLevel;

      if (level < LastCharge)
      {
      }
      BatteryLifeInSeconds = Convert.ToUInt32(level * 100 * ElapsedTime);

      LastCharge = level;
      ElapsedTime = 0;
    }

    async void OnToggled(object sender, ToggledEventArgs e)
    {
      if (e.Value)
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
