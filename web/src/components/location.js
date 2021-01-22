import React, { Component } from 'react'
import firebase from '../services/firebase'
import { Button, Form, Table } from 'react-bootstrap'

export default class Location extends Component {
    constructor(props) {
        super(props)
        this.title = 'Location'
        this.state = { ships: {},
                        temp: { name: 'Titanic', lat: '0', lon: '0' } }
        this.handleChange = this.handleChange.bind(this)
        this.handleSubmit = this.handleSubmit.bind(this)
        this.handleRemove = this.handleRemove.bind(this)
        this.ref = firebase.database().ref("Ships")
    }
    componentDidMount() {
        this.ref.on('value', (snapshot) => {
            console.log(snapshot.val())
            if (snapshot.val() !== null) {
                this.setState({ships: snapshot.val()})
            }
            
        })
    }
    handleSubmit(event) {
        console.log(this.state.ships)

        const testObj = { Latitude: this.state.temp.lat, Longitude: this.state.temp.lon }
        const ref = this.ref.child(this.state.temp.name)
        ref.set(testObj)
        this.setState({temp: { name: 'Success', lat: '1', lon: '2' }})

        event.preventDefault()
    }
    handleRemove(ship) {
        console.log(ship)
        const ref = this.ref.child(ship)
        ref.remove()
    }
    handleChange(event, index) {
        let changedObject = JSON.parse(JSON.stringify(this.state.temp))
        switch (event.target.name) {
          case 'name':
            changedObject.name = event.target.value
            this.setState({temp: changedObject})
            break;
          case 'latitude':
            changedObject.lat = event.target.value
            this.setState({temp: changedObject})
            break;
          case 'longitude':
            changedObject.lon = event.target.value
            this.setState({temp: changedObject})
            break;
          default:
            console.log('error with switch')
        }
    }
    render() {
        console.log(this.state.ships)
        const tmp = this.state.temp

        const fetchedShips = Object.entries(this.state.ships).map(([key, value], index) => {
            return (
                <tr key={index}>
                    <td>
                        {key}
                    </td>
                    <td>
                        <li>{value.Latitude}</li>
                    </td>
                    <td>
                        <li>{value.Longitude}</li>
                    </td>
                    <td>
                        <Button variant="danger" onClick={() => this.handleRemove(key)}>Remove</Button>
                    </td>
                </tr>
            )
        })
        
        return (
            <div>
                {this.title}

                <Table striped bordered hover>
                    <thead>
                        <tr>
                        <th>Name</th>
                        <th>Latitude</th>
                        <th>Longitude</th>
                        <th>Button</th>
                        </tr>
                    </thead>
                    <tbody>
                        {fetchedShips}
                    </tbody>
                </Table>
                <p>{tmp.name} - lat: {tmp.lat}, lon: {tmp.lon}</p>
                <Form onSubmit={this.handleSubmit}>

                    <Form.Group>
                        <Form.Label htmlFor="name">name:</Form.Label>
                        <Form.Control type="text" name="name" value={this.state.temp.name}
                        onChange={this.handleChange} />
                    </Form.Group>

                    <Form.Group>
                        <Form.Label htmlFor="latitude">latitude:</Form.Label>
                        <Form.Control type="text" name="latitude" value={this.state.temp.lat}
                        onChange={this.handleChange} />
                    </Form.Group>

                    <Form.Group>
                        <Form.Label htmlFor="longitude">longitude:</Form.Label>
                        <Form.Control type="text" name="longitude" value={this.state.temp.lon}
                        onChange={this.handleChange} />
                    </Form.Group>

                    <Form.Group>
                        <Button variant="primary" type="submit">Submit</Button>
                    </Form.Group>
                    </Form>
            </div>
        )
    }
}