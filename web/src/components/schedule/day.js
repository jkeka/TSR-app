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
    this.removeDay = this.removeDay.bind(this)
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
    this.setState({
      eventDropDown: Object.entries(this.state.events).map(([key, value], index) => {
        return (
          <option key={index} value={key}>{value.translations.en.event} ({key})</option>
        )
      }),
      event: Object.keys(this.state.events)[0]
    })
  }
  setDate(date) {
    console.log(date)
  }
  handleChange(event) {
    console.log('handleChange')
  } 
  handleSubmit(event) {
    event.preventDefault()
    console.log('submit')
  }
  removeDay(key) {
    
  }
  render() {
    const days = Object.entries(this.state.days).map(([key, value], index) => {
      return (
        <tr key={index}>
          <td>{key}</td>
          <td>{value.date}</td>
          
          <td>
            <Button variant="danger" onClick={() => this.removeDay(key)}>Remove</Button>
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