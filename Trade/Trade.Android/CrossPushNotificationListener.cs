using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;
using PushNotification.Plugin;
using PushNotification.Plugin.Abstractions;

namespace Trade.Droid
{
    public class CrossPushNotificationListener : IPushNotificationListener
    {
        public CrossPushNotificationListener()
        {

        }
        public void OnError(string message, DeviceType deviceType)
        {
           
        }

        public void OnMessage(JObject values, DeviceType deviceType)
        {
            
        }

        public void OnRegistered(string token, DeviceType deviceType)
        {
          
        }

        public void OnUnregistered(DeviceType deviceType)
        {
           
        }

        public bool ShouldShowNotification()
        {
            return true;
        }
    }
}