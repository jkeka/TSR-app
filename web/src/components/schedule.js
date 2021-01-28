import React, { Component } from 'react'
import { Button, Container } from 'react-bootstrap'
import firebase from '../services/firebase'
import Venue from './schedule/venue'
import Day from './schedule/day'
import Event from './schedule/event'

export default class Schedule extends Component {
    constructor(props) {
        super(props)
        this.title = 'Schedule'
        this.ref = firebase.database().ref("Schedule")
        this.changeRoute = this.changeRoute.bind(this)
        this.state = { 
            venues: [],
            days: [],
            events: [],
            router: <p>Choose a tab</p>,
            dayButton: "primary",
            eventButton: "primary",
            venueButton: "primary"
        }
    }
    componentDidMount() {
        // Database fetch
        this.ref.child('venues').on('value', (snapshot) => {
            if (snapshot.val() !== null) {
                this.setState({venues: snapshot.val()})
            } else {
                console.log('venues fetch failed or nothing to fetch')
            }
        })
        this.ref.child('days').on('value', (snapshot) => {
            if (snapshot.val() !== null) {
                this.setState({days: snapshot.val()})
            } else {
                console.log('days fetch failed or nothing to fetch')
            }
        })
        this.ref.child('events').on('value', (snapshot) => {
            if (snapshot.val() !== null) {
                this.setState({events: snapshot.val()})
            } else {
                console.log('events fetch failed or nothing to fetch')
            }
        })
        
    }
    changeRoute(route) {
        switch(route) {
            case 'venue':
                this.setState({router: <Venue venues={this.state.venues} fireRef={this.ref} />,
                    venueButton: "success", dayButton: "primary", eventButton: "primary"})
                break
            case 'event':
                this.setState({router: <Event events={this.state.events} venues={this.state.venues} 
                    fireRef={this.ref} />,
                    venueButton: "primary", dayButton: "primary", eventButton: "success"})
                break
            case 'day':
                this.setState({router: <Day days={this.state.days} fireRef={this.ref}
                    events={this.state.events} venues={this.state.venues} />,
                    venueButton: "primary", dayButton: "success", eventButton: "primary"})
                break
            default:
                this.setState({router: <p>Choose a tab</p>})
                break
        }
    }
    render() {
        return (
            <Container>
                    <h1>{this.title}</h1>
                    <Button variant={this.state.dayButton} onClick={() => this.changeRoute('day')}>Day</Button>
                    <Button variant={this.state.eventButton} onClick={() => this.changeRoute('event')}>Event</Button>
                    <Button variant={this.state.venueButton} onClick={() => this.changeRoute('venue')}>Venue</Button>
                    {this.state.router}
            </Container>
            
        )
  }
}