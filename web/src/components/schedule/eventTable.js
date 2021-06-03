import React, { useEffect, useState } from 'react'

/**
 * This component lists all current events in the Firebase db.
 * 
 * By clicking Edit, user can edit an event (the data is sent to NewEvent.js).
 */

export default function EventTable({ childevents, setEditedEvent, fb }) {
  const [events, setEvents] = useState(childevents)
  const [selectedEvent, setSelectedEvent] = useState('')
  const [dateDropdown, setDateDropdown] = useState(<option>No dates found</option>)
  const [eventList, setEventList] = useState(<tr><td>xError creating eventlist</td></tr>)

  useEffect(() => {
    setEvents(childevents)
  }, [childevents])

  useEffect(() => {
    let uniqueDates = new Set()
    Object.values(events).forEach(event => {
      let date = new Date(event.startTime)
      date.setHours(0)
      date.setMinutes(0)
      date.setSeconds(0)
      uniqueDates.add(date.getTime())
    })
    uniqueDates = Array.from(uniqueDates).sort((a, b) => a- b)
    if (uniqueDates.length > 1) {
      setDateDropdown([<option>all dates</option>, uniqueDates.map(d => <option key={d}>{new Date(d).toString()}</option>)])
    }
    const list = Object.entries(events).map(([key, value], index) => {
      return (
        <tr key={index} style={selectedEvent === key ? highLight : selectedEvent !== '' ? grey : null}>
          <td>{key}</td>
          <td>{value.translations.fi.event}</td>
          <td>{value.venueId}</td>
          <td>{new Date(value.startTime).toLocaleString('fi-FI')}</td>
          <td>{new Date(value.endTime).toLocaleString('fi-FI')}</td>
          <td>
            {selectedEvent === '' ?
            <button onClick={() => editEvent(key)}>Edit</button>
            : ''
            }
            
            {selectedEvent === key ? 
            <>
              <button onClick={() => removeEvent(key)}>Remove</button>
              <button onClick={() => resetForm()} style={{marginLeft: "1em"}}>Cancel</button>
            </>
            : ''
            }
          </td>
        </tr>
      )
    })
    setEventList(list)
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [events])

  const filterDate = (date) => {
    if (date.target.value === 'all dates') {
      setEvents(childevents)
      return
    }
    date = new Date(date.target.value)
    let filteredEvents = {}
    Object.entries(childevents).forEach(([key, value]) => {
      const d = new Date(value.startTime)
      if (date.getDate() === d.getDate() && date.getMonth() === d.getMonth()) {
        filteredEvents[key] = value
      }
    })
    setEvents(filteredEvents)
  }
  
  const editEvent = (key) => {
    console.log(`edit ${key}`)
    const x = Object.values(events).find(e => e.id === parseInt(key))
    setEditedEvent(x)
    setSelectedEvent(key)
  }

  const removeEvent = (key) => {
    if (window.confirm('really remove ' + key)) {
      console.log(`removing ${key}`)
      fb.child('Schedule').child('events').child(key).remove()
      setSelectedEvent('')
    }
    
  }

  const resetForm = () => {
    console.log(`cancel`)
    setSelectedEvent('')
    setEditedEvent(null)
  }

  const highLight = {background: 'yellow'}
  const grey = {color: 'darkgrey'}



  return (
    <div>
      Filter by date:&nbsp;
      <select onChange={filterDate}>
        {dateDropdown}
      </select>
      <table width="100%" border="1px">
        <thead>
          <tr>
            <th>id</th>
            <th>name</th>
            <th>venue</th>
            <th>startTime</th>
            <th>endTime</th>
            <th>buttons</th>
          </tr>
        </thead>
        <tbody>
          {eventList}
        </tbody>
      </table>
    </div>
    
  )
}