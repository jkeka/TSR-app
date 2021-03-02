import React, { Component } from 'react'
import firebase from '../services/firebase'
import { Button, Form, Table } from 'react-bootstrap'

export default class Location extends Component {
    constructor(props) {
        super(props)
        this.title = 'Location'
        this.state = { 
            locations: {},
            types: ['ship', 'venue', 'booth'],
            temp: { name: 'Titanic', Latitude: '0', Longitude: '0', type: 'ship' },
            typeDropDown: <option>no types found</option>,
            type: 'ship'
        }
        this.handleChange = this.handleChange.bind(this)
        this.handleSubmit = this.handleSubmit.bind(this)
        this.handleRemove = this.handleRemove.bind(this)
        this.ref = firebase.database().ref("Location")
    }
    componentDidMount() {
        let tmpDropDown = this.state.types.map(t => <option key={t}>{t}</option>) 
        this.ref.on('value', (snapshot) => {
            if (snapshot.val() !== null) {
                this.setState(
                    {
                        locations: snapshot.val(),
                        typeDropDown: tmpDropDown    
                    }
        )
            } else {
                console.log('no locations found')
                this.setState({typeDropDown: tmpDropDown})
            }
        })
        
    }
    handleSubmit(event) {
        event.preventDefault()
        console.log(this.state.locations)
        const ref = this.ref.child(this.state.temp.name)
        let tmpLocations = {...this.state.locations}
        let newObj = {...this.state.temp}
        newObj.id = new Date().getTime()
        ref.set(newObj)
        tmpLocations[this.state.temp.name] = newObj
        this.setState({locations: tmpLocations})
    }
    handleRemove(loc, locId) {
        console.log(loc, locId)
        const ref = this.ref.child(loc)
        ref.remove()
        let tmpLocations = {...this.state.locations}
        delete tmpLocations[loc]
        this.setState({locations: tmpLocations})
    }
    handleChange(event) {
        let changedObject = JSON.parse(JSON.stringify(this.state.temp))
        switch (event.target.name) {
            case 'name':
                changedObject.name = event.target.value
                this.setState({temp: changedObject})
                break
            case 'latitude':
                changedObject.Latitude = event.target.value
                this.setState({temp: changedObject})
                break
            case 'longitude':
                changedObject.Longitude = event.target.value
                this.setState({temp: changedObject})
                break
            case 'type':
                changedObject.type = event.target.value
                this.setState({temp: changedObject, type: event.target.value})
                break
            default:
                console.log('error with switch')
        }
    }
    render() {
        console.log(this.state.locations)
        const tmp = this.state.temp

        const fetchedLocs = Object.entries(this.state.locations).map(([key, value], index) => {
            return (
                <tr key={index}>
                    <td>
                        {value.id}
                    </td>
                    <td>
                        {key}
                    </td>
                    <td>
                        <li>{value.type}</li>
                    </td>
                    <td>
                        <li>{value.Latitude}</li>
                    </td>
                    <td>
                        <li>{value.Longitude}</li>
                    </td>
                    <td>
                        <Button variant="danger" onClick={() => this.handleRemove(key, value.id)}>Remove</Button>
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
                        <th>id</th>
                        <th>Name</th>
                        <th>Type</th>
                        <th>Latitude</th>
                        <th>Longitude</th>
                        <th>Button</th>
                        </tr>
                    </thead>
                    <tbody>
                        {fetchedLocs}
                    </tbody>
                </Table>
                <p>{tmp.name} - lat: {tmp.lat}, lon: {tmp.lon}, {tmp.type}</p>
                <Form onSubmit={this.handleSubmit}>

                    <Form.Control as="select" custom name="type" value={this.state.type} onChange={this.handleChange}>
                        {this.state.typeDropDown}
                    </Form.Control>

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