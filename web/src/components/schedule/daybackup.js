import React, { Component } from 'react'
import { Button, Form, Container, Table, Row, Col } from 'react-bootstrap'
import DatePicker from 'react-datepicker'
import "react-datepicker/dist/react-datepicker.css";

export default class Day extends Component {
  constructor(props) {
    super(props)
    this.title = 'Day'
    this.ref = props.fireRef
    this.setDate = this.setDate.bind(this)
    this.handleChange = this.handleChange.bind(this)
    this.handleSubmit = this.handleSubmit.bind(this)
    this.handleDaySubmit = this.handleDaySubmit.bind(this)
    this.removeDay = this.removeDay.bind(this)
    this.removeEvent = this.removeEvent.bind(this)
    this.state = { 
        days: props.days,
        events: props.events,
        venues: props.venues,
        eventDropDown: <option>No events found</option>,
        event: undefined,
        date: new Date()
    }
  }
  componentDidMount() {
    console.log(this.state.days)
    this.setState({
      eventDropDown: Object.entries(this.state.events).map(([key, value], index) => {
        if (!value.setToDay) {
          return (
            <option key={index} value={key}>{value.translations.en.event} ({key})</option>
          )
        } else {
          return null
        }
        
      }),
      event: Object.keys(this.state.events)[0]
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

  }
  handleSubmit(event) {
    event.preventDefault()
    console.log('submit')
    const newId = new Date().getTime()

    const newDate = `${this.state.date.getDate()}.${this.state.date.getMonth()}.${this.state.date.getFullYear()}`
    if(Object.entries(this.state.days).some(([day, value]) => value.date === newDate))  {
      console.log('date found!')
      const oldDate = Object.entries(this.state.days).find(([day, value]) => value.date === newDate)
      
      let tmpEventArr = []
      if(oldDate[1].events) {
        Object.entries(oldDate[1].events).forEach(([event, value]) => {
          tmpEventArr.push(value)
        })
      }
      tmpEventArr.push(this.state.event)
      let ref = this.ref.child('days').child(oldDate[0]).child('events')
      ref.set(tmpEventArr)
      ref = this.ref.child('events').child(this.state.event).child('setToDay')
      ref.set(true)

    } else {
      console.log('date not found, adding a new date')
      const newDay = { 
        id: newId,
        date: newDate,
        events: [this.state.event]
      }
      let ref = this.ref.child('days').child(newId)
      ref.set(newDay)
      ref = this.ref.child('events').child(this.state.event).child('setToDay')
      ref.set(true)

      let tempDays = JSON.parse(JSON.stringify(this.state.days))
      tempDays[newId] = newDay
      this.setState({
        days: tempDays,
      })
    }

  }
  removeDay(key) {
    const ref = this.ref.child('days').child(key)
    ref.remove()
    let tempDays = JSON.parse(JSON.stringify(this.state.days))
    delete tempDays[key]
    this.setState({days: tempDays})
  }
  removeEvent(day, event) {
    const eventRef = this.state.days[day].events.indexOf(event)
    let ref = this.ref.child('days').child(day).child('events').child(eventRef)
    ref.remove()
    ref = this.ref.child('events').child(event).child('setToDay')
    ref.set(false)

  }
  render() {
    const days = Object.entries(this.state.days).map(([key, value], index) => {
      let eventList = <ul><li>No events added</li></ul>
      if(value.events) {
        eventList = value.events.map(event => {
          return (
            <li key={event}>
              {event}
              <span style={{paddingLeft: '1em'}}>
                <Button size="sm" variant="danger" onClick={() => this.removeEvent(key, event)}>Remove event</Button>
              </span>
            </li>
          )})
      } 
      return (
        <tr key={index}>
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
        <h1>{this.title}</h1>
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
        <Form onSubmit={this.handleSubmit}>
          <Form.Group>
            <Form.Label>Select date</Form.Label>
            <DatePicker selected={this.state.date} onChange={date => this.setDate(date)} />
            {this.state.date.toString()}<br/>
            <Button onClick={() => this.handleDaySubmit()}>Add day</Button>
          </Form.Group>
          <Form.Group>
            <Form.Label>Select event</Form.Label>
            <Form.Control as="select" custom name="event" value={this.state.event} onChange={this.handleChange}>
              {this.state.eventDropDown}
            </Form.Control>
            {selectedEvent}
          </Form.Group>
          
          <hr/>
          <Form.Group>
            <Button variant="primary" type="submit">Add event to date</Button>
          </Form.Group>
        </Form>
          
      </Container>
            
      )
  }
}