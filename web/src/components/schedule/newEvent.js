import React, { useEffect, useState } from 'react'
import DatePicker from "react-datepicker"
import "react-datepicker/dist/react-datepicker.css"
import EventDescription from './eventDescription'
import './newEvent.css'

const eventTemplate = { 
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
  venueId: 'null'
}

export default function NewEvent({ venues, editEvent, fb }) {
  const [startDate, setStartDate] = useState(new Date())
  const [endDate, setEndDate] = useState(new Date())
  const [newEvent, setNewEvent] = useState(eventTemplate)
  const [venueDropDown, setVenueDropDown] = useState(<option>No venues found</option>)
  const [message, setMessage] = useState(null)

  useEffect(() => {
    if (editEvent && editEvent.id) {
      console.log(editEvent)
      setEndDate(new Date(editEvent.endTime))
      setStartDate(new Date(editEvent.startTime))
      setNewEvent(editEvent)
    } else if (editEvent === null) {
      setNewEvent(eventTemplate)
    }
  }, [editEvent])

  useEffect(() => {
    if (endDate.getTime() <= startDate.getTime()) {
      createMessage('Ending time must be later than starting time. Setting ending time to +15min of starting time.', true, 5000)
      setEndDate(new Date(startDate.getTime() + 900000))
      setNewEvent({ ...newEvent, startTime: startDate.getTime(), endTime: endDate.getTime() })
    } else {
      setNewEvent({ ...newEvent, startTime: startDate.getTime(), endTime: endDate.getTime() })
    }
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [startDate, endDate])

  // VENUE DROPDOWN MENU
  useEffect(() => {
    setVenueDropDown([<option>Select a venue...</option>, 
      Object.values(venues).map(venue => <option key={venue.id}>{venue.name} ({venue.id})</option>)])
    if (Object.values(venues)[0]) {
      setNewEvent({ ...newEvent, venueId: Object.values(venues)[0].id })
    }
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [venues])
  
  const createMessage = (message, error = false, timeout = 3000) => {
    let style = { color: 'green' }
    if (error) { style.color = 'red' }
    setMessage(<p style={style}>{message}</p>)
    setTimeout(() => { setMessage(null) }, timeout)
  }

  const onChange = (e, o = null) => {
    const value = e.target.value
    switch (e.target.name) {
      case 'venue':
        if (value !== 'Select a venue...') {
          setNewEvent({ ...newEvent, venueId: parseInt(value.substring(value.length - 14, value.length - 1)) })
        }
        break
      case 'fi':
        setNewEvent({ ...newEvent, translations: { ...newEvent.translations, fi: o }})
        break
      case 'en':
        setNewEvent({ ...newEvent, translations: { ...newEvent.translations, en: o }})
        break
      case 'se':
        setNewEvent({ ...newEvent, translations: { ...newEvent.translations, se: o }})
        break
      default:
        console.log('error with switch')
    }
  }

  const submitData = () => {
    if (window.confirm('really send new data?')) {
      console.log('submitting', newEvent)
      try {
        fb.child('Schedule').child('events').child(newEvent.id).set(newEvent)
      } catch(e) {
        console.log('error', e)
        console.log({ message: 'Error adding event', error: true })
      }
      
    }
  }

  return (
    <div>
      <h2>{editEvent ? 'Edit event' : 'New Event'}</h2>

      <select onChange={onChange} name="venue">
        {venueDropDown}
      </select>

      {venues && Number.isInteger(newEvent.venueId) ?
      <div>
        <div style={{float: 'right', width: '33%', fontSize: 'x-small', border: '1px solid black'}}>
            { Object.entries(newEvent).map(([key, value]) => {
              if (key === 'translations') {
                return (
                  <div key={value.id}>{key}: 
                    <ul>
                      {Object.entries(value).map(([k, v]) => <li key={k}>{k}: {JSON.stringify(v)}</li>)}
                    </ul>
                  </div>
                )
              } 
              return <div key={value.id}>{key}: {JSON.stringify(value)}<br/></div>
              
            })}
        </div>

        <br/>

        Start time: 
        <DatePicker 
          selected={startDate} 
          onChange={date => setStartDate(date)} 
          showTimeSelect
          dateFormat="d. MMMM yyyy HH:mm" 
          timeFormat="HH:mm"
          timeIntervals={15}
        /> 
        <br/>
        End time: 
        <DatePicker 
          selected={endDate} 
          onChange={date => setEndDate(date)} 
          showTimeSelect 
          dateFormat="d. MMMM yyyy HH:mm" 
          timeFormat="HH:mm"
          timeIntervals={15}
        />

        <div className="cont">
          <div className="co">
            <EventDescription 
              desc={newEvent.translations.fi.desc} 
              event={newEvent.translations.fi.event} 
              onChange={(e, o) => onChange(e, o)}
              lang="fi"
            />
          </div>
          <div className="co">
            <EventDescription 
              desc={newEvent.translations.en.desc} 
              event={newEvent.translations.en.event} 
              onChange={(e, o) => onChange(e, o)}
              lang="en"
            />
          </div>
          <div className="co">
            <EventDescription 
              desc={newEvent.translations.se.desc} 
              event={newEvent.translations.se.event} 
              onChange={(e, o) => onChange(e, o)}
              lang="se"
            />
          </div>
        </div>
        <button onClick={submitData}>submit data</button>
        {message}
      </div>
      :
      ''
      }
      
    </div>
  )
}