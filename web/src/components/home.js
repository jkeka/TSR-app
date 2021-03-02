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
            this.setState({testi: JSON.stringify(snapshot.val(), null, 2)})
        })
        
    }
    render() {
        return (
            
            <div>
                <h1>
                   {this.title} 
                </h1>
                <p>
                    {this.state.testi}
                </p>
            </div>
        )
    }
}