import React, { Component } from 'react'
import firebase from '../services/firebase'

export default class Home extends Component {
    constructor(props) {
        super(props)
        this.title = 'Home'
        this.state = { testi: '' }
    }
    componentDidMount() {
        const ref = firebase.database().ref()
        
        ref.on('value', (snapshot) => {
            console.log(snapshot.val())
            this.setState({testi: snapshot.val().Testi})
        })
        
    }
    render() {
        return (
            
            <div>
                {this.title}<br/>
                <h1>
                {this.state.testi}
                </h1>
            </div>
        )
    }
}