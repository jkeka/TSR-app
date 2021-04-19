import React, { Component } from 'react'
import firebase from '../../services/firebase'
import { Button, Form, Container, Table, Row, Col } from 'react-bootstrap'
import DatePicker from 'react-datepicker'

export default class Event extends Component {
  constructor(props) {
    super(props)
    this.title = 'Events'
    this.ref = firebase.database().ref()
    this.handleChange = this.handleChange.bind(this)
    this.handleSubmit = this.handleSubmit.bind(this)
    this.copyEvent = this.copyEvent.bind(this)
    this.copyDescription = this.copyDescription.bind(this)
    this.setStartTime = this.setStartTime.bind(this)
    this.setEndTime = this.setEndTime.bind(this)
    this.fillStuff = this.fillStuff.bind(this)
    this.editEvent = this.editEvent.bind(this)
    this.resetForm = this.resetForm.bind(this)
    this.state = { 
      events: {},
      venues: {},
      venue: '',
      startTime: new Date(),
      endTime: new Date(),
      eventFi: '',
      eventEn: '',
      eventSe: '',
      descFi: '',
      descEn: '',
      descSe: '',
      selectedEvent: '',
      id: ''
    }
  }
  componentDidMount() {
    let tmpStartDate = new Date(this.state.startTime)
    let tmpEndDate = new Date(this.state.endTime)
    tmpStartDate.setHours(12)
    tmpStartDate.setMinutes(0)
    tmpEndDate.setHours(tmpStartDate.getHours() + 1)
    tmpEndDate.setMinutes(0)
    // let tmpEvents = {}
    let tmpVenues = {}
    this.ref.child('Schedule').child('events').on('value', (snapshot) => {
      if (snapshot.val() !== null) {
        this.setState({events: snapshot.val()})
      } else {
        console.log('events fetch failed or nothing to fetch')
      }
    })
    this.ref.child('Location').on('value', (snapshot) => {
      if (snapshot.val() !== null) {
        for (const [key, value] of Object.entries(snapshot.val())) {
          // Adding only locations with type venue to the venue list
          if (value.type === 'venue') {
            tmpVenues[key] = (snapshot.val()[key])
          }
          this.setState({venues: tmpVenues, venue: Object.keys(this.state.venues)[0]})
        }
        
      } else {
        console.log('locations fetch failed or nothing to fetch')
      }
    })
    this.setState({
      startTime: tmpStartDate,
      endTime: tmpEndDate,
    })
  }
  setStartTime(date) {
    this.setState({startTime: date})
  }
  setEndTime(date) {
    this.setState({endTime: date})
  }
  fillStuff() {
    this.setState({
        eventFi: 'Tapahtuma',
        eventEn: 'Event',
        eventSe: 'Händelse',
        descFi: 'eventSelostus',
        descEn: 'eventDescription',
        descSe: 'eventDeskriptionen',
        venue: Object.keys(this.state.venues)[1],

    })
  }
  handleChange(event) {
    console.log('handlechange')
    switch (event.target.name) {
      case 'venue':
        this.setState({venue: event.target.value})
        break
      case 'eventFi':
        this.setState({eventFi: event.target.value})
        break
      case 'eventEn':
        this.setState({eventEn: event.target.value})
        break
      case 'eventSe':
        this.setState({eventSe: event.target.value})
        break
      case 'descFi':
        this.setState({descFi: event.target.value})
        break
      case 'descEn':
        this.setState({descEn: event.target.value})
        break
      case 'descSe':
        this.setState({descSe: event.target.value})
        break
      default:
        console.log('error with switch')
    }
  }
  handleSubmit(event) {
    console.log('handleSubmit')
    event.preventDefault()
    let newId = new Date().getTime()
    if (this.state.id !== '') {
      newId = this.state.id
    }
    const venueId = this.state.venues[this.state.venue].id
    const newEvent = { 
      id: newId,
      startTime: this.state.startTime.getTime(),
      endTime: this.state.endTime.getTime(),
      venueId: venueId,
      translations: { 
        fi:  {
          event: this.state.eventFi,
          desc:  this.state.descFi
        },
        en: {
          event: this.state.eventEn,
          desc:  this.state.descEn
        },
        se: {
          event: this.state.eventSe,
          desc:  this.state.descSe
        }
      }
    }
    console.log(newEvent)
    
    const ref = this.ref.child('events').child(newId)
    ref.set(newEvent)
    let tempEvents = JSON.parse(JSON.stringify(this.state.events))
    tempEvents[newId] = newEvent
    this.setState({
      events: tempEvents,
      eventFi: '',
      eventEn: '',
      eventSe: '',
      descFi: '',
      descEn: '',
      descSe: '',
      startTime: new Date(),
      endTime: new Date(),
      venue: Object.keys(this.state.venues)[0]
    })
    
  }
  removeEvent(key) {
    console.log(key)
    const ref = this.ref.child('events').child(key)
    ref.remove()
    let tempEvents = JSON.parse(JSON.stringify(this.state.events))
    delete tempEvents[key]
    this.setState({events: tempEvents})
  }
  copyEvent() {
    const copy = JSON.parse(JSON.stringify(this.state.eventFi))
    this.setState({
        eventEn: copy, eventSe: copy
    })
  }
  copyDescription() {
    const copy = JSON.parse(JSON.stringify(this.state.descFi))
    this.setState({
        descEn: copy, descSe: copy
    })
  }
  editEvent(key) {
    console.log(key)
    const eventToEdit = Object.values(this.state.events).find(event => event.id.toString() === key)
    console.log(eventToEdit)
    console.log(this.state.venues)
    const tmpVenue = Object.values(this.state.venues).find(venue => venue.id.toString() === key)
    const tmpStartDate = new Date(eventToEdit.startTime)
    const tmpEndDate = new Date(eventToEdit.endTime)
    this.setState({
      selectedEvent: key,
      startTime: tmpStartDate,
      endTime: tmpEndDate,
      id: eventToEdit.id,
      eventFi: eventToEdit.translations.fi.event,
      descFi: eventToEdit.translations.fi.desc,
      eventEn: eventToEdit.translations.en.event,
      descEn: eventToEdit.translations.en.desc,
      eventSe: eventToEdit.translations.se.event,
      descSe: eventToEdit.translations.se.desc,
      venue: tmpVenue
    })
  }
  resetForm() {
    this.setState({
      selectedEvent: '',
      startTime: '',
      endTime: '',
      id: '',
      eventFi: '',
      descFi: '',
      eventEn: '',
      descEn: '',
      eventSe: '',
      descSe: ''
    })
  }
  render() {
    const events = Object.entries(this.state.events).map(([key, value], index) => {
      return (
        <tr key={index}>
          <td>{key}</td>
          <td>{value.translations.fi.event}</td>
          <td>{value.venueId}</td>
          <td>{new Date(value.startTime).toLocaleString()}</td>
          <td>{new Date(value.endTime).toLocaleString()}</td>
          <td>
            {this.state.selectedEvent === '' ?
            <Button variant="info" onClick={() => this.editEvent(key)}>Edit</Button>
            : ''
            }
            
            {this.state.selectedEvent === key ? 
            <>
            <Button variant="danger" onClick={() => this.removeEvent(key)}>Remove</Button>
            <Button variant="info" onClick={() => this.resetForm()} 
              style={{marginLeft: "1em"}}>Cancel</Button>
            </>
            : ''
            }
          </td>
        </tr>
      )
    })
    const venueDropDown = Object.entries(this.state.venues).map(([key, value], index) => {
        return (
          <option key={index} value={key}>{key} ({value.id})</option>
        )
      })
    return (
      <Container>
        
        <a href="#addEvent">Add event</a>
        <h1>{this.title}</h1>
        
        <Table>
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
            {events}
          </tbody>
        </Table>
        <hr/>
        <h1 id="addEvent">Add Event</h1>
        <Button variant="success" size="lg" onClick={() => this.fillStuff()}>Fill</Button>
        <Form onSubmit={this.handleSubmit}>
          <Form.Group>
            <Form.Label htmlFor="eventFi">Event name in Finnish:</Form.Label>
            <Button size="sm" onClick={() => this.copyEvent()}>Copy to all</Button>
            <Form.Control type="text" name="eventFi" value={this.state.eventFi}
                onChange={this.handleChange} />
            
            <Form.Label htmlFor="eventEn">Event name in English:</Form.Label>
            <Form.Control type="text" name="eventEn" value={this.state.eventEn}
                onChange={this.handleChange} />
            
            <Form.Label htmlFor="eventSe">Event name in Swedish:</Form.Label>
            <Form.Control type="text" name="eventSe" value={this.state.eventSe}
                onChange={this.handleChange} />
          </Form.Group>
          <hr/>
          <Form.Label>Select venue</Form.Label>
          <Form.Control as="select" custom name="venue" value={this.state.venue} onChange={this.handleChange}>
            {venueDropDown}
          </Form.Control>
          <hr/>
          <Row>
            <Col>
            <Form.Label>Select start time</Form.Label>
            <br/>
            <DatePicker showTimeSelect dateFormat="Pp" selected={this.state.startTime} onChange={date => this.setStartTime(date)} />
            </Col>
            <Col>
            <Form.Label>Select end time</Form.Label>
            <br/>
            <DatePicker showTimeSelect dateFormat="Pp" selected={this.state.endTime} onChange={date => this.setEndTime(date)} />
            </Col>
          </Row>
          <hr/>
          <Form.Group>
            <Form.Label htmlFor="descFi">Description in Finnish:</Form.Label>
            <Button size="sm" onClick={() => this.copyDescription()}>Copy to all</Button>
            <Form.Control type="text" name="descFi" value={this.state.descFi}
                onChange={this.handleChange} />
            
            <Form.Label htmlFor="descEn">Description in English:</Form.Label>
            <Form.Control type="text" name="descEn" value={this.state.descEn}
                onChange={this.handleChange} />
            
            <Form.Label htmlFor="descSe">Description in Swedish:</Form.Label>
            <Form.Control type="text" name="descSe" value={this.state.descSe}
                onChange={this.handleChange} />
          </Form.Group>
          <hr/>
          <Form.Group>
            <Button variant="primary" type="submit">Add event</Button>
          </Form.Group>
        </Form>
        
      </Container>
        
    )
  }
}