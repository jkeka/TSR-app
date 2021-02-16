import React, { Component } from 'react'
import { Button, Form, Container, Table, Row, Col } from 'react-bootstrap'

export default class Event extends Component {
  constructor(props) {
    super(props)
    this.title = 'Events'
    this.ref = props.fireRef
    this.handleChange = this.handleChange.bind(this)
    this.handleSubmit = this.handleSubmit.bind(this)
    this.copyEvent = this.copyEvent.bind(this)
    this.copyDescription = this.copyDescription.bind(this)
    this.fillStuff = this.fillStuff.bind(this)
    this.state = { 
      events: props.events,
      venues: props.venues,
      venueDropDown: <option>no venues found</option>,
      venue: '',
      startHour: 0,
      startMinute: 0,
      endHour: 0,
      endMinute: 0,
      eventFi: '',
      eventEn: '',
      eventSe: '',
      descFi: '',
      descEn: '',
      descSe: ''
    }
  }
  componentDidMount() {
    this.setState({
      venueDropDown: Object.entries(this.state.venues).map(([key, value], index) => {
        return (
          <option key={index} value={key}>{value.translations.en.venue} ({key})</option>
        )
      }),
      venue: Object.keys(this.state.venues)[0]
    })
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
        startHour: 12,
        startMinute: 15,
        endHour: 13,
        endMinute: 30
    })
  }
  handleChange(event) {
    console.log('handlechange')
    switch (event.target.name) {
      case 'venue':
        this.setState({venue: event.target.value})
        break
      case 'endHour':
        this.setState({endHour: event.target.value})
        break
      case 'endMinute':
        this.setState({endMinute: event.target.value})
        break
      case 'startHour':
        this.setState({startHour: event.target.value})
        break
      case 'startMinute':
        this.setState({startMinute: event.target.value})
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
    const newId = new Date().getTime()
    const venueId = this.state.venues[this.state.venue].id
    const newEvent = { 
      id: newId,
      startHour: this.state.startHour,
      startMinute: this.state.startMinute,
      endHour: this.state.endHour,
      endMinute: this.state.endMinute,
      venueId: venueId,
      setToDay: false,
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
      startHour: 0,
      startMinute: 0,
      endHour: 0,
      endMinute: 0,
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
  render() {
    const events = Object.entries(this.state.events).map(([key, value], index) => {
      return (
        <tr key={index}>
          <td>{key}</td>
          <td>{value.venueId}</td>
          <td>{value.startHour}:{value.startMinute}</td>
          <td>{value.endHour}:{value.endMinute}</td>
          <td>{value.setToDay ? 'Yes' : 'No'}</td>
          <td>
            <Button variant="danger" onClick={() => this.removeEvent(key)}>Remove</Button>
          </td>
        </tr>
      )
    })
    let hours = []
    let minutes = []
    for (let x = 0; x < 24; x++) {
      hours[x] = <option key={x} value={x}>{x}</option>
    }
    for (let x = 0; x < 60; x += 15) {
      minutes[x] = <option key={x} value={x}>{x}</option>
    }
    return (
      <Container>
        
        <h1>{this.title}</h1>
        <Table>
          <thead>
            <tr>
              <th>id</th>
              <th>venue</th>
              <th>startTime</th>
              <th>endTime</th>
              <th>setToDay</th>
              <th>buttons</th>
            </tr>
          </thead>
          <tbody>
            {events}
          </tbody>
        </Table>
        <hr/>
        <h1>Add Event</h1>
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
            {this.state.venueDropDown}
          </Form.Control>
          <hr/>
          <Form.Label>Select start time</Form.Label>
          <Row>
            <Col>
              <Form.Label>Select hours</Form.Label>
              <Form.Control as="select" custom name="startHour" value={this.state.startHour} onChange={this.handleChange}>
                {hours}
              </Form.Control>
            </Col>
            <Col>
              <Form.Label>Select minutes</Form.Label>
              <Form.Control as="select" custom name="startMinute" value={this.state.startMinute} onChange={this.handleChange}>
                {minutes}
              </Form.Control>
            </Col>
            <Col xs={8}></Col> 
          </Row>
          <hr/>
          <Form.Label>Select end time</Form.Label>
          <Row>
            <Col>
              <Form.Label>Select hours</Form.Label>
              <Form.Control as="select" custom name="endHour" value={this.state.endHour} onChange={this.handleChange}>
                {hours}
              </Form.Control>
            </Col>
            <Col>
              <Form.Label>Select minutes</Form.Label>
              <Form.Control as="select" custom name="endMinute" value={this.state.endMinute} onChange={this.handleChange}>
                {minutes}
              </Form.Control>
            </Col>
            <Col xs={8}></Col> 
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
            <Button variant="primary" type="submit">Add venue</Button>
          </Form.Group>
        </Form>
        
      </Container>
        
    )
  }
}