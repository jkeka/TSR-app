import React, { Component } from 'react'
import { Button, Form, Container, Table } from 'react-bootstrap'


export default class Venue extends Component {
  constructor(props) {
    super(props)
    this.title = 'Venues'
    this.ref = props.fireRef
    this.copyVenue = this.copyVenue.bind(this)
    this.copyAddress = this.copyAddress.bind(this)
    this.copyDescription = this.copyDescription.bind(this)
    this.handleChange = this.handleChange.bind(this)
    this.handleSubmit = this.handleSubmit.bind(this)
    this.removeVenue = this.removeVenue.bind(this)
    this.fillStuff = this.fillStuff.bind(this)
    this.state = { 
        venues: props.venues,
        venueFi: '',
        venueEn: '',
        venueSe: '',
        addressFi: '',
        addressEn: '',
        addressSe: '',
        descFi: '',
        descEn: '',
        descSe: '',
        latitude: '',
        longitude: ''
    }
  }
  removeVenue(key) {
    const ref = this.ref.child('venues').child(key)
    ref.remove()
    let tempVenues = JSON.parse(JSON.stringify(this.state.venues))
    delete tempVenues[key]
    this.setState({venues: tempVenues})
  }
  fillStuff() {
    this.setState({
        venueFi: 'Kioski',
        venueEn: 'Kiosk',
        venueSe: 'Kiosken',
        addressFi: 'Merikatu 1',
        addressEn: 'Sea Street 1',
        addressSe: 'Hav Gatan 1',
        descFi: 'Selostus',
        descEn: 'Description',
        descSe: 'Deskriptionen',
        latitude: -5,
        longitude: 6
    })
  }
  handleSubmit(event) {
    console.log('handleSubmit')
    event.preventDefault() 
    const newId = new Date().getTime()
    const newVenue = { 
        id: newId,
        latitude: this.state.latitude,
        longitude: this.state.longitude,
        translations: { 
            fi:  {
                venue: this.state.venueFi,
                address: this.state.addressFi,
                desc:  this.state.descFi
            },
            en: {
                venue: this.state.venueEn,
                address: this.state.addressEn,
                desc:  this.state.descEn
            },
            se: {
                venue: this.state.venueSe,
                address: this.state.addressSe,
                desc:  this.state.descSe
            }
        }
    }
    const ref = this.ref.child('venues').child(newId)
    ref.set(newVenue)
    let tempVenues = JSON.parse(JSON.stringify(this.state.venues))
    tempVenues[newId] = newVenue
    this.setState({
        venues: tempVenues,
        venueFi: '',
        venueEn: '',
        venueSe: '',
        addressFi: '',
        addressEn: '',
        addressSe: '',
        descFi: '',
        descEn: '',
        descSe: '',
        latitude: '',
        longitude: ''
    })
    
  }
  copyVenue() {
    const copy = JSON.parse(JSON.stringify(this.state.venueFi))
    this.setState({
        venueEn: copy, venueSe: copy
    })
  }
  copyAddress() {
    const copy = JSON.parse(JSON.stringify(this.state.addressFi))
    this.setState({
        addressEn: copy, addressSe: copy
    })
  }
  copyDescription() {
    const copy = JSON.parse(JSON.stringify(this.state.descFi))
    this.setState({
        descEn: copy, descSe: copy
    })
  }
  handleChange(event) {
    switch (event.target.name) {
      case 'venueFi':
        this.setState({venueFi: event.target.value})
        break;
      case 'venueEn':
        this.setState({venueEn: event.target.value})
        break;
      case 'venueSe':
        this.setState({venueSe: event.target.value})
        break;
      case 'addressFi':
        this.setState({addressFi: event.target.value})
        break;
      case 'addressEn':
        this.setState({addressEn: event.target.value})
        break;
      case 'addressSe':
        this.setState({addressSe: event.target.value})
        break;
      case 'descFi':
        this.setState({descFi: event.target.value})
        break;
      case 'descEn':
        this.setState({descEn: event.target.value})
        break;
      case 'descSe':
        this.setState({descSe: event.target.value})
        break;
      case 'latitude':
        this.setState({latitude: event.target.value})
        break;
      case 'longitude':
        this.setState({longitude: event.target.value})
        break;
      default:
        console.log('error with switch')
    }
  }
  render() {
    const venues = Object.entries(this.state.venues).map(([key, value], index) => {
      return (
        <tr key={index}>
          <td>{key}</td>
          <td>{value.id}</td>
          <td>{value.latitude}</td>
          <td>{value.longitude}</td>
          <td>
            <Button variant="danger" onClick={() => this.removeVenue(key)}>Remove</Button>
          </td>
        </tr>
      )
    })
    return (
        <Container>
            <h1>{this.title}</h1>
            <Table striped bordered hover>
              <thead>
                <tr>
                  <th>name</th>
                  <th>id</th>
                  <th>latitude</th>
                  <th>longitude</th>
                  <th>buttons</th>
                </tr>
              </thead>
              <tbody>
                {venues}
              </tbody>
            </Table>
            <h1>Add venue</h1>
        <Button variant="success" size="lg" onClick={() => this.fillStuff()}>Fill</Button>
        <Form onSubmit={this.handleSubmit}>

        <Form.Group>
            <Form.Label htmlFor="venueFi">Venue name in Finnish:</Form.Label>
            <Button size="sm" onClick={() => this.copyVenue()}>Copy to all</Button>
            <Form.Control type="text" name="venueFi" value={this.state.venueFi}
                onChange={this.handleChange} />
            
            <Form.Label htmlFor="venueEn">Venue name in English:</Form.Label>
            <Form.Control type="text" name="venueEn" value={this.state.venueEn}
                onChange={this.handleChange} />
            
            <Form.Label htmlFor="venueSe">Venue name in Swedish:</Form.Label>
            <Form.Control type="text" name="venueSe" value={this.state.venueSe}
                onChange={this.handleChange} />
        </Form.Group>

        <Form.Group>
            <Form.Label htmlFor="addressFi">Address in Finnish:</Form.Label>
            <Button size="sm" onClick={() => this.copyAddress()}>Copy to all</Button>
            <Form.Control type="text" name="addressFi" value={this.state.addressFi}
                onChange={this.handleChange} />
            
            <Form.Label htmlFor="addressEn">Address in English:</Form.Label>
            <Form.Control type="text" name="addressEn" value={this.state.addressEn}
                onChange={this.handleChange} />
            
            <Form.Label htmlFor="addressSe">Address in Swedish:</Form.Label>
            <Form.Control type="text" name="addressSe" value={this.state.addressSe}
                onChange={this.handleChange} />
        </Form.Group>

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

        <Form.Group>
            <Form.Label htmlFor="latitude">Latitude:</Form.Label>
            <Form.Control type="text" name="latitude" value={this.state.latitude}
                onChange={this.handleChange} />
            
            <Form.Label htmlFor="longitude">Longitude:</Form.Label>
            <Form.Control type="text" name="longitude" value={this.state.longitude}
                onChange={this.handleChange} />
        </Form.Group>

        <Form.Group>
            <Button variant="primary" type="submit">Add venue</Button>
        </Form.Group>
        </Form>
        </Container>
        
    )
  }
}