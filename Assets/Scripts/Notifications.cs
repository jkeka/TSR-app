using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.Localization;

public class Notifications: MonoBehaviour

{
    static LocalizedString arrivalTitle = new LocalizedString() { TableReference = "Translations", TableEntryReference = "NOTIFICATION_ARRIVAL_TITLE" };
    static LocalizedString arrivalText = new LocalizedString() { TableReference = "Translations", TableEntryReference = "NOTIFICATION_ARRIVAL_TEXT" };

    public static AndroidNotificationChannel defaultChannel = new AndroidNotificationChannel()
    {
        Id = "channel_id",
        Name = "Default channel",
        Importance = Importance.High,
        Description = "Generic notification"
    };

    public static AndroidNotification arrivalNotification = new AndroidNotification()
    {
        Title = arrivalTitle.GetLocalizedString(),
        Text = arrivalText.GetLocalizedString(),
        FireTime = System.DateTime.Now
    };

    public static void SetNotificationLanguage()
        // Updates the language of notifications when necessary
    {
        arrivalNotification.Title = arrivalTitle.GetLocalizedString();
        arrivalNotification.Text = arrivalText.GetLocalizedString() + " " + CoordinateData.locationName;
    }
}




    

    


        


    

   
    
    




