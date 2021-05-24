import React, { useState } from 'react'

export default function EventTable({ events, setEditedEvent, fb }) {
  const [selectedEvent, setSelectedEvent] = useState('')

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
    setEditedEvent({
      endTime: new Date(), 
      id: new Date().getTime(), 
      startTime: new Date(),
      translations: { 
        en: {
          desc: '',
          event: ''
        },
        fi: {
          desc: '',
          event: ''
        },
        se: {
          desc: '',
          event: ''
        }
      },
      venueId: 'null'})
  }

  const highLight = {background: 'yellow'}
  const grey = {color: 'darkgrey'}

  const eventList = Object.entries(events).map(([key, value], index) => {
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

  return (
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
  )
}