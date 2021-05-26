import React, { useEffect, useState } from 'react'
import EventTable from './schedule/eventTable'
import NewEvent from './schedule/newEvent'

export default function Schedule({firebase}) {
    const [events, setEvents] = useState({})
    const [venues, setVenues] = useState({})
    const [editedEvent, setEditedEvent] = useState({})

    useEffect(() =>  {
        firebase.child('Schedule').child('events').on('value', (snapshot) => {
            if (snapshot.val() !== null) {
                setEvents(snapshot.val())
            } else {
                console.log('events fetch failed or nothing to fetch')
            }
          })
    }, [firebase])

    useEffect(() => {
        firebase.child('Location').on('value', (snapshot) => {
            if (snapshot.val() !== null) {
                let tmpVenues = {}
                for (const [key, value] of Object.entries(snapshot.val())) {
                    // Adding only locations with type venue to the venue list
                    if (value.type === 'venue') {
                        tmpVenues[key] = (snapshot.val()[key])
                    }
                }
                setVenues(tmpVenues)
            } else {
                console.log('locations fetch failed or nothing to fetch')
            }
          })
    }, [firebase])

    return (
        <div>
            <h1>Schedule</h1>
            <button onClick={() => console.log(events)}>events</button>
            <button onClick={() => console.log(venues)}>venues</button>
            <EventTable childevents={events} setEditedEvent={setEditedEvent} fb={firebase} />

            <NewEvent venues={venues} editEvent={editedEvent} fb={firebase} />
            
        </div>
    )
}