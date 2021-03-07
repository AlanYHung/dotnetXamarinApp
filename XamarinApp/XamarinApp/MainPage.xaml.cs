using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamarinApp
{
  public partial class MainPage : ContentPage
  {
    uint BatteryLifeInSeconds { get; set; }
    double LastCharge { get; set; }
    int ElapsedTime { get; set; }
    BatteryState isCharging { get; set; }
    bool isEyeball { get; set; }
    

    public MainPage()
    {
      InitializeComponent();
      BatteryLifeInSeconds = 18000;
      LastCharge = 1F;
      ElapsedTime = 0;
      isEyeball = false;

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

      Compass.Start(SensorSpeed.UI);
      Compass.ReadingChanged += Compass_ReadingChanged;
    }

    void OnImageTapped(Object sender, EventArgs e)
    {
      if (isEyeball)
      {
        Vibration.Vibrate(1000);
        EyeballImage.IsVisible = false;
        CompassFrame.IsVisible = true;
        isEyeball = false;
      }
      else
      {
        CompassFrame.IsVisible = false;
        EyeballImage.IsVisible = true;
        isEyeball = true;
      }
    }

    void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
    {
      var data = e.Reading;
      double northReading = data.HeadingMagneticNorth;

      if(isEyeball)
      {
        EyeballImage.RotateTo(-northReading, 250, Easing.SinInOut);
      }
      else
      {
        CompassImage.RotateTo(-northReading, 250, Easing.SinInOut);
      }

      HeadingLabel.Text = $"{(360 - northReading).ToString("0.00")} \u00B0N";
    }

    void Magnetometer_ReadingChanged(object sender, MagnetometerChangedEventArgs e)
    {
      var data = e.Reading;
      float xValue = data.MagneticField.X;
      float yValue = data.MagneticField.Y;

      if (isEyeball)
      {
        EyeballImage.RotateXTo(yValue / 3, 250, Easing.SinInOut);
        EyeballImage.RotateYTo(xValue / 2, 250, Easing.SinInOut);
      }
      else
      {
        CompassImage.RotateXTo(xValue / 3, 250, Easing.SinInOut);
        CompassImage.RotateYTo(yValue / 3, 250, Easing.SinInOut);
      }
    }

    void DisplayCountdown()
    {
      int hours = Convert.ToInt32(BatteryLifeInSeconds / 60 / 60 % 60);
      int minutes = Convert.ToInt32(BatteryLifeInSeconds / 60 % 60);
      int seconds = Convert.ToInt32(BatteryLifeInSeconds % 60);

      if(isCharging.Equals(BatteryState.Charging))
      {
        digitalclock.Text = "Charging...";
      }
      else
      {
        digitalclock.Text = hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");
      }

      BatteryLifeInSeconds--;
      ElapsedTime++;
    }

    void Battery_BatteryInfoChanged(object sender, BatteryInfoChangedEventArgs e)
    {
      double level = e.ChargeLevel;
      isCharging = e.State;

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
