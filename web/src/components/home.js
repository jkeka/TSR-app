import React, { Component } from 'react'

/**
 * Used for testing.
 */

export default class Home extends Component {
    constructor(props) {
        super(props)
        this.title = 'Home'
    }
    render() {
        return (
            
            <div>
                <h1>
                   {this.title} 
                </h1>
            </div>
        )
    }
}