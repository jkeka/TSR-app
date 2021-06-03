using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.Localization;

public class Notifications: MonoBehaviour
{
    //This class stores and operates mobile notifications of the application

    // References to tables used in string localization
    static LocalizedString arrivalTitle = new LocalizedString() { TableReference = "Translations", TableEntryReference = "NOTIFICATION_ARRIVAL_TITLE" };
    static LocalizedString arrivalText = new LocalizedString() { TableReference = "Translations", TableEntryReference = "NOTIFICATION_ARRIVAL_TEXT" };

    // Default channel for notifications
    public static AndroidNotificationChannel defaultChannel = new AndroidNotificationChannel()
    {
        Id = "channel_id",
        Name = "Default channel",
        Importance = Importance.High,
        Description = "Generic notification"
    };

    // Notification for arrival to the selected non-ship destination
    public static AndroidNotification arrivalNotification = new AndroidNotification()
    {
        Title = arrivalTitle.GetLocalizedString(),
        Text = arrivalText.GetLocalizedString(),
        FireTime = System.DateTime.Now
    };

    // Updates the language of notifications
    public static void SetNotificationLanguage()
    {
        arrivalNotification.Title = arrivalTitle.GetLocalizedString();
        arrivalNotification.Text = arrivalText.GetLocalizedString() + " " + CoordinateData.locationName;
    }
}




    

    


        


    

   
    
    




