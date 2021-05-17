import React, { Component } from 'react'
import firebase from '../services/firebase'
import { Button, Form, Table } from 'react-bootstrap'
import MapPicker from 'react-google-map-picker'

const DefaultLocation = { lat: 60.43, lng: 22.24 }
const DefaultZoom = 15
const DefaultTemp = { name: '', Latitude: '0', Longitude: '0', type: 'ship' }
const DefaultType = 'ship'

export default class Location extends Component {
    constructor(props) {
        super(props)
        this.state = { 
            locations: {},
            types: ['ship', 'venue', 'booth', 'restaurant'],
            temp: DefaultTemp,
            typeDropDown: <option>no types found</option>,
            type: DefaultType,
            mapLocation: DefaultLocation,
            zoom: DefaultZoom,
            defaultLocation: DefaultLocation,
            selectedLocation: null,
            mapLoaded: false,
            authed: false,
            showMap: false,
            filter: 'ship'
        }
        this.handleChange = this.handleChange.bind(this)
        this.handleSubmit = this.handleSubmit.bind(this)
        this.handleRemove = this.handleRemove.bind(this)
        this.ref = firebase.database().ref("Location")
        this.handleChangeLocation = this.handleChangeLocation.bind(this)
        this.handleChangeZoom = this.handleChangeZoom.bind(this)
        this.modifyLocation = this.modifyLocation.bind(this)
        this.cancel = this.cancel.bind(this)
    }
    handleChangeLocation(lat, lng) {
        let changedObject = JSON.parse(JSON.stringify(this.state.temp))
        changedObject.Latitude = lat
        changedObject.Longitude = lng
        this.setState({mapLocation: {lat:lat, lng:lng}, temp: changedObject })
    }
      
    handleChangeZoom(newZoom) {
        this.setState({zoom: newZoom})
    }
    
    componentDidMount() {
        let tmpDropDown = this.state.types.map(t => <option key={t}>{t}</option>) 
        this.ref.on('value', (snapshot) => {
            if (snapshot.val() !== null) {
                this.setState(
                    {
                        locations: snapshot.val(),
                        typeDropDown: tmpDropDown,
                        mapLoaded: true    
                    })       
            } else {
                console.log('no locations found')
                this.setState({typeDropDown: tmpDropDown, mapLoaded: true })
            }
        })
        const user = firebase.auth().currentUser;

        if (user) {
        this.setState({authed: true})
        } else {
        // No user is signed in.
        }
    }
    handleSubmit(event) {
        event.preventDefault()
        console.log(this.state.locations)
        const ref = this.ref.child(this.state.temp.name)
        let tmpLocations = {...this.state.locations}
        let newObj = {...this.state.temp}
        newObj.Latitude = newObj.Latitude.toString().replace(/,/g, '.')
        newObj.Longitude = newObj.Longitude.toString().replace(/,/g, '.')
        newObj.id = new Date().getTime()
        ref.set(newObj)
        tmpLocations[this.state.temp.name] = newObj
        this.setState({locations: tmpLocations})
    }
    handleRemove(loc, locId) {
        console.log(loc, locId)
        if (window.confirm(`really remove ${loc}, ${locId}?`)) {
            const ref = this.ref.child(loc)
            ref.remove()
            let tmpLocations = {...this.state.locations}
            delete tmpLocations[loc]
            this.setState({locations: tmpLocations})
        }
        
    }
    handleChange(event) {
        let changedObject = JSON.parse(JSON.stringify(this.state.temp))
        let newMapLocation = JSON.parse(JSON.stringify(this.state.mapLocation))
        switch (event.target.name) {
            case 'name':
                changedObject.name = event.target.value
                this.setState({temp: changedObject})
                break
            case 'latitude':
                changedObject.Latitude = event.target.value.replace(".", ',')
                newMapLocation.lat = parseFloat(event.target.value.replace(/,/g, '.'))
                this.setState({temp: changedObject, mapLocation: newMapLocation})
                break
            case 'longitude':
                changedObject.Longitude = event.target.value.replace(".", ',')
                newMapLocation.lng = parseFloat(event.target.value.replace(/,/g, '.'))
                this.setState({temp: changedObject, mapLocation: newMapLocation})
                break
            case 'type':
                changedObject.type = event.target.value
                if (event.target.value !== 'ship') {
                    delete changedObject.shipId
                }
                this.setState({temp: changedObject, type: event.target.value})
                break
            case 'filter':
                this.setState({filter: event.target.value})
                break
            case 'shipId':
                changedObject.shipId = parseInt(event.target.value)
                this.setState({temp: changedObject})
                break
            default:
                console.log('error with switch')
        }
        console.log(changedObject)
    }
    modifyLocation(value) {
        const formattedLat = parseFloat(value.Latitude.replace(/,/g, '.'))
        const formattedLon = parseFloat(value.Longitude.replace(/,/g, '.'))
        this.handleChangeLocation(formattedLat, formattedLon)
        let loc = { name: value.name, Latitude: value.Latitude, Longitude: value.Longitude, type: value.type }
        console.log(value)
        if (value.type === 'ship') {
            loc = { ...loc, shipId: value.shipId }
        } 
        this.setState({
            defaultLocation: { lat: formattedLat, lng: formattedLon }, 
            selectedLocation: value.id, 
            temp: loc,
            type: value.type
        }) 
        
        
    }
    handleUpdate(key, value) {
        console.log(key, value)
        const ref = this.ref.child(key)
        let tmpLocations = {...this.state.locations}
        let newObj = {...this.state.temp}
        newObj.id = value
        ref.set(newObj)
        tmpLocations[this.state.temp.name] = newObj
        this.setState({locations: tmpLocations})
    }
    cancel() {
        this.setState({
            selectedLocation: null,
            type: DefaultType,
            temp: DefaultTemp
        })
    }
    render() {
        const tmp = this.state.temp

        let filteredLocs = this.state.locations
        if (this.state.filter !== 'all') {
            filteredLocs = Object.values(this.state.locations).filter(value => value.type === this.state.filter)
        }

        const locArray = []
        Object.values(filteredLocs).forEach(loc => locArray.push(loc))

        if (this.state.filter === 'ship') {
            locArray.sort((a,b) => a.shipId - b.shipId)
        }

        
        const fetchedLocs = locArray.map(value => {
            let style = value.shipId === 0 || value.shipId === 999 ? {background: 'DarkSalmon'} : {background: 'DarkSeaGreen'}
            return (
                <tr key={value.id} style={style}>
                    {this.state.filter === 'ship' ? <td>{value.shipId}</td> : null}
                    <td>
                        {value.id}
                    </td>
                    <td>
                        {value.name}
                    </td>
                    <td>
                        {value.type}
                    </td>
                    <td>
                        {value.Latitude}
                    </td>
                    <td>
                        {value.Longitude}
                    </td>
                    {this.state.selectedLocation === value.id ? 
                    <td>
                        <Button variant="success" onClick={() => this.handleUpdate(value.name, value.id)}>Update</Button>
                        <br/>
                        <Button variant="danger" onClick={() => this.handleRemove(value.name, value.id)}>Remove</Button>
                        <br/>
                        <Button variant="warning" onClick={() => this.cancel()}>Cancel</Button>
                    </td>
                    :
                    <td>
                        <Button size="sm" onClick={() => this.modifyLocation(value)}>Edit</Button>
                    </td>
                    }
                </tr>
            )
        })
        
        return (
            <div>
            {this.state.authed ?
            <div>
                <table style={{border: "1px solid black", float: "right"}}>
                    <tr>
                        <th colSpan='2'>Object to be sent to DB</th>
                    </tr>
                    <tr>
                        <td>name</td>
                        <td>{tmp.name}</td>
                    </tr>
                    <tr>
                        <td>latitude</td>
                        <td>{tmp.Latitude}</td>
                    </tr>
                    <tr>
                        <td>longitude</td>
                        <td>{tmp.Longitude}</td>
                    </tr>
                    <tr>
                        <td>type</td>
                        <td>{tmp.type}</td>
                    </tr>
                </table>

                <Form onSubmit={this.handleSubmit}>
                    <label>Latitude:</label>
                    <input type="text" name="latitude" value={this.state.mapLocation.lat} 
                        onChange={this.handleChange}/>
                    <label>Longitude:</label>
                    <input type='text' name="longitude" value={this.state.mapLocation.lng} 
                        onChange={this.handleChange} />
                    
                    <label>Zoom:</label>
                    <input type='text' value={this.state.zoom} disabled/>
                    <br/>
                    <label>Type:</label>
                    <select name="type" value={this.state.type} onChange={this.handleChange}>
                        {this.state.typeDropDown}
                    </select>
                    <label>Name:</label><input type="text" name="name" value={this.state.temp.name}
                        onChange={this.handleChange} disabled={this.state.selectedLocation}></input>
                    <br/>
                    {this.state.type === 'ship' ?
                    <div>
                        <label>Ship Id:</label>
                        <input type='number' name="shipId" value={this.state.temp.shipId} 
                        onChange={this.handleChange} />
                    </div>
                    
                    :
                    ''}
                    {this.state.selectedLocation === null ?
                        <Button variant="primary" type="submit">Submit a new Location</Button>
                        :
                        ''
                    }
                    
                </Form>

                <br/>
                
                {this.state.mapLoaded && this.state.showMap ? 
                <div>
                    <button onClick={() => this.setState({showMap: false})}>Hide map</button>
                    <MapPicker 
                        defaultLocation={this.state.defaultLocation}
                        zoom={this.state.zoom}
                        style={{height:'500px'}}
                        onChangeLocation={this.handleChangeLocation} 
                        onChangeZoom={this.handleChangeZoom}
                        apiKey='AIzaSyBrVmDD28eMPJ3QGoJfBiml1QQdvB0EUuU'
                    />
                </div>
                : <button onClick={() => this.setState({showMap: true})}>Show map</button>}

                <br/><br/>
                filter: 
                <select name="filter" value={this.state.filter} onChange={this.handleChange}>
                    <option>all</option>
                    {this.state.types.map(type => <option key={type}>{type}</option>)}
                </select>
                location count: {filteredLocs.length}
                
                
                <Table striped bordered hover style={{fontSize: 'small'}}>
                    <thead>
                        <tr>
                        {this.state.filter === 'ship' ? <th>shipId</th> : null}
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
            </div>
            :
            <p>Please log in</p>}
                    
            </div>
        )
    }
}