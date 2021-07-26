using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace InstagramClone.Droid.PushNotifications
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        AndroidNotificationManager androidNotification = new AndroidNotificationManager();
        public override void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug(TAG, "From: " + message.From);
            Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);
            androidNotification.CrearNotificacionLocal(message.GetNotification().Title, message.GetNotification().Body);
        }
        public override void OnNewToken(string Token)
        {
            base.OnNewToken(Token);
            Preferences.Set("TokenFirebase", Token);
            sedRegisterToken(Token);
        }
        public void sedRegisterToken(string token)
        {

        }
    }
}