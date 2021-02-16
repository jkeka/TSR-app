import React, { Component } from 'react'
import { Button, Form, Container, Table, Row, Col } from 'react-bootstrap'
import DatePicker from 'react-datepicker'
import "react-datepicker/dist/react-datepicker.css"
import '../../css/custom.css'

export default class Day extends Component {
  constructor(props) {
    super(props)
    this.title = 'Day'
    this.ref = props.fireRef
    this.setDate = this.setDate.bind(this)
    this.handleChange = this.handleChange.bind(this)
    this.handleEventSubmit = this.handleEventSubmit.bind(this)
    this.handleDaySubmit = this.handleDaySubmit.bind(this)
    this.hideShowAddDay = this.showAddDay.bind(this)
    this.modifyDay = this.modifyDay.bind(this)
    this.removeDay = this.removeDay.bind(this)
    this.removeEvent = this.removeEvent.bind(this)
    this.makeBigDayObjects = this.makeBigDayObjects.bind(this)
    this.state = { 
        days: props.days,
        events: props.events,
        venues: props.venues,
        event: undefined,
        date: new Date(),
        info: "",
        selectedDay: null,
        hideAddDay: true
    }
  }
  componentDidMount() {
    console.log(this.state.days)
    console.log(this.state.events)
    const firstEvent = Object.entries(this.state.events).find(([key, value]) => !value.setToDay)[0]
    this.setState({event: firstEvent ? firstEvent : undefined})
  }
  makeBigDayObjects() {
    console.log('making big objects')
    //let daysArray = {}
    Object.entries(this.state.days).forEach(([id, day]) => {
      console.log(day)
      //let dayObj = {}
      for(let e of day.events) {
        console.log(e)
        let eventObj = Object.entries(this.state.events).find(([key, event]) => key === e)[1]
        console.log(eventObj)
        let testArray = [eventObj]
        console.log(testArray)
      }
    })
  }
  setDate(date) {
    console.log(date)
    this.setState({date: date})
  }
  handleChange(event) {
    console.log('handleChange')
    switch(event.target.name) {
      case 'event':
        this.setState({event: event.target.value})
        break
      default:
        console.log('error with switch')
    }
  }
  handleDaySubmit(event) {
    event.preventDefault()
    console.log('click')
    const newId = new Date().getTime()

    const newDate = `${this.state.date.getDate()}.${this.state.date.getMonth()}.${this.state.date.getFullYear()}`
    if(Object.entries(this.state.days).some(([day, value]) => value.date === newDate))  {
      console.log('date found!')
      this.setState({info: "Date already exists"})
    } else {
      console.log('date not found, adding a new date')
      const newDay = { 
        id: newId,
        date: newDate,
        events: {}
      }

      const ref = this.ref.child('days').child(newId)
      ref.set(newDay)

      let tempDays = {...this.state.days}
      tempDays[newId] = newDay
      this.setState({
        days: tempDays
      })
    }
  }
  modifyDay(key) {
    console.log('rowclick', key)
    if(this.state.selectedDay !== key) {
      this.setState({selectedDay: key, hideAddDay: true})
    }
  }
  showAddDay() {
    this.setState({hideAddDay: false, selectedDay: null})
  }
  handleEventSubmit(event) {
    event.preventDefault()
    console.log('submit event')

    if(this.state.event !== undefined && this.state.selectedDay !== null) {
      const date = Object.entries(this.state.days).find(([day, value]) => value.id.toString() === this.state.selectedDay)[1]
      
      let tmpEventArr = []
      if(date.events) {
        Object.entries(date.events).forEach(([event, value]) => {
          tmpEventArr.push(value)
        })
      } else {
        console.log('no events')
      }
      tmpEventArr.push(this.state.event)

      let tmpDays = {...this.state.days}
      tmpDays[this.state.selectedDay].events = tmpEventArr

      let tmpEvents = {...this.state.events}
      tmpEvents[this.state.event].setToDay = true


      let ref = this.ref.child('days').child(this.state.selectedDay).child('events')
      ref.set(tmpEventArr)
      ref = this.ref.child('events').child(this.state.event).child('setToDay')
      ref.set(true)
      
      let firstEvent = Object.entries(this.state.events).find(([key, value]) => !value.setToDay && value.id.toString() !== this.state.event)
      if (firstEvent) {
        firstEvent = firstEvent[0]
      }
      this.setState({days: tmpDays, info: "Event added", events: tmpEvents, event: firstEvent ? firstEvent : undefined})
    } else {
      this.setState({info: "Select an event or add a day"})
    }
  }
  removeDay(key) {
    if (this.state.days[key].events && this.state.days[key].events.length > 0) {
      this.setState({info: "Remove events from the day before removing day"})
    } else {
      console.log('no events detected, deleting day...')

      let tmpDays = {...this.state.days}
      tmpDays = Object.entries(tmpDays).filter(([day, value]) => day !== key)
      console.log(tmpDays)
      this.setState({days: tmpDays, event: undefined, selectedDay: null})

      const ref = this.ref.child('days').child(key)
      ref.remove()
    }
    
  }
  removeEvent(day, event) {
    console.log(day, event[1])


    
    console.log(this.state.days[day].events)
    const eventRef = this.state.days[day].events.indexOf(event[1])
    console.log(eventRef)
    let ref = this.ref.child('days').child(day).child('events').child(eventRef)
    ref.remove()
    ref = this.ref.child('events').child(event[1]).child('setToDay')
    ref.set(false)  

    let tmpEvents = {...this.state.events}
    tmpEvents[event[1]].setToDay = false

    let tmpDays = {...this.state.days}
    tmpDays[day].events = tmpDays[day].events.filter(e => e !== event[1])

    this.setState({days: tmpDays, events: tmpEvents})
  }
  render() {
    const eventDropDown = Object.entries(this.state.events).map(([key, value], index) => {
        if (!value.setToDay) {
          return (
            <option key={index} value={key}>{value.translations.en.event} ({key})</option>
          )
        } else {
          return null
        }
    })
    const days = Object.entries(this.state.days).map(([key, value], index) => {
      let eventList = <ul><li>No events added</li></ul>
      if(value.events) {
        eventList = Object.entries(value.events).map(event => {
          return (
            <li key={event}>
              {event}
              {this.state.selectedDay === key ?
                <span style={{paddingLeft: '1em'}}>
                  <Button size="sm" variant="danger" onClick={() => this.removeEvent(key, event)}>Remove event</Button>
                </span>
                : 
                ""
              }
            </li>
          )})
      }
      return (
        <tr key={index} onClick={() => this.modifyDay(key)} className={this.state.selectedDay === key ? "selected" : ""}>
          <td>{key}</td>
          <td>{value.date}</td>
          <td>{eventList}</td>
          <td>
            <Button variant="danger" onClick={() => this.removeDay(key)}>Remove day</Button>
          </td>
        </tr>
      )
    })
    let selectedEvent = <p>Not selected</p>
    if (this.state.event !== undefined) {
      const event = this.state.events[this.state.event]
      const venue = this.state.venues[event.venueId]
      selectedEvent = 
        <div>
          <Row>
            <Col>
              <h3>Selected event: {event.translations.en.event} ({event.id})</h3>
            </Col>
          </Row>
          <Row>
            <Col>
              <h4>Description</h4>
              <p>
                {event.translations.en.desc}
              </p>
              
            </Col>
            <Col>
              <h4>Venue info</h4>
              <p>
                {venue.translations.en.venue} ({venue.id})
                <br/>
                {venue.translations.en.address}
                <br/>
                {venue.translations.en.desc}
              </p>
            </Col>
            <Col>
              <h4>Start Time</h4>
              <p>{event.startHour}:{event.startMinute}</p>
              <h4>End Time</h4>
              <p>{event.endHour}:{event.endMinute}</p>
            </Col>
            
          </Row>
        </div>
    }
    
    return (
      <Container>
        <Button size="lg" variant="warning" onClick={() => this.makeBigDayObjects()}>Make big day objects</Button>
        <h1>{this.title}</h1>
        <h3 style={{color: "red"}}>{this.state.info}</h3>
        <Table>
          <thead>
            <tr>
              <th>id</th>
              <th>date</th>
              <th>events</th>
              <th>buttons</th>
            </tr>
          </thead>
          <tbody>
            {days}
          </tbody>
        </Table>
        <hr/>
        {this.state.hideAddDay ? <Button onClick={() => this.showAddDay()}>Add day</Button> :
        <Form onSubmit={this.handleDaySubmit}>
          <Form.Group>
            <Form.Label>Select date</Form.Label>
            <DatePicker selected={this.state.date} onChange={date => this.setDate(date)} />
            {`${this.state.date.getDate()}.${this.state.date.getMonth()}.${this.state.date.getFullYear()}`}<br/>
            <Button type="submit">Add day</Button>
          </Form.Group>
        </Form>
        }
        <hr/>
        {this.state.selectedDay === null ? <p>Select a date to add events</p> :
        <Form onSubmit={this.handleEventSubmit}>
          <Form.Group>
            <Form.Label>Select event</Form.Label>
            <Form.Control as="select" custom name="event" value={this.state.event} onChange={this.handleChange}>
              {eventDropDown}
            </Form.Control>
            {selectedEvent}
          </Form.Group>
          
          <hr/>
          <Form.Group>
            <Button variant="primary" type="submit">Add event to date</Button>
          </Form.Group>
        </Form>
        }
      </Container>
            
      )
  }
}