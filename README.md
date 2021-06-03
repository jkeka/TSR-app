# TSR-app
Tallships races -application for Lingsoft


Firebase requirements:
- a project with RealTime Database open for Android, iOS and Web App.
- two users (add in Firebase Console: Authentication):
  - admin user to add/edit/remove data on the Web UI
  - a common main user to use the app (app logs in with this on the loading screen)

Nodes:
  Descriptions:
    $locationId: $language: description: string

  Location:
    $locationName:
      {
        Latitude: string
        Longitude: string
        id: number
        name: string
        (shipId: number)
        type: string
      }

  Schedule:
    events:
      $eventID:
        {
          endTime: number (timestamp)
          id: number
          startTime: number (timestamp)
          translations: 
            {
              $language: { desc: string, event: string } 
            }
          venueId: number
        }

  Users:
    $deviceCode: {
      deviceCode: string
      language: string
      (visitedLocations: string array)
    }


Firebase rules (Firebase Console: Realtime Database -> Rules):
{
  "rules": {
    "Descriptions": {
      ".read": "auth.uid === 'AUTH-UID-OF-MAIN-USER' || auth.uid === 'AUTH-UID-OF-WEBUI-ADMIN-USER'",
    	".write": "auth.uid === 'AUTH-UID-OF-WEBUI-ADMIN-USER'"
    },
    "Location": {
      ".read": "auth.uid === 'AUTH-UID-OF-MAIN-USER' || auth.uid === 'AUTH-UID-OF-WEBUI-ADMIN-USER'",
    	".write": "auth.uid === 'AUTH-UID-OF-WEBUI-ADMIN-USER'"
    },
    "Users": {
      ".read": "auth.uid === 'AUTH-UID-OF-MAIN-USER' || auth.uid === 'AUTH-UID-OF-WEBUI-ADMIN-USER'",
      "$deviceCode": {
      	".write": "auth.uid === 'AUTH-UID-OF-MAIN-USER'",
        ".validate": "newData.hasChildren(['deviceCode', 'language'])",
        "deviceCode": {
          ".validate": "newData.val().length > 5 && newData.val().length < 100"
        },
        "language": {
          ".validate": "newData.val().length > 0 && newData.val().length < 10"
      	},
        "visitedLocations": {},
        "$other": {
          ".validate": false
        }
      }
    },
    "Schedule": {
      ".read": "auth.uid === 'AUTH-UID-OF-MAIN-USER' || auth.uid === 'AUTH-UID-OF-WEBUI-ADMIN-USER'",
      "events": {
        ".write": "auth.uid === 'AUTH-UID-OF-WEBUI-ADMIN-USER'",
        "$event": {
          ".validate": "newData.hasChildren(['endTime', 'id', 'startTime', 'translations', 'venueId'])"
        }
      }
    	
    }
  }
}