﻿using Android.App;
using Android.Content.PM;
using Avalonia.Android;

namespace ChatGPT.Android;

[Activity(Label = "ChatGPT.Android", Theme = "@style/MyTheme.NoActionBar", Icon = "@drawable/icon",
    LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
public class MainActivity : AvaloniaMainActivity
{
}