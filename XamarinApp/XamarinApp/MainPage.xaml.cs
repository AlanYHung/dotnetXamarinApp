using System;
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
      Magnetometer.ReadingChanged += Magnetometer_ReadingChanged;
    }

    void Magnetometer_ReadingChanged(object sender, MagnetometerChangedEventArgs e)
    {
      var data = e.Reading.MagneticField;
      float xValue = data.MagneticField.X;
      float yValue = data.MagneticField.Y;
      float zValue = data.MagneticField.Z;

      xReading.Text = xValue.ToString();
      yReading.Text = yValue.ToString();
      zReading.Text = zValue.ToString();

      Compass.RotateXTo(xValue / 4, 250, Easing.SinInOut);
      Compass.RotateYTo(yValue / 4, 250, Easing.SinInOut);
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
