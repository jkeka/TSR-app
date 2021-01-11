import React, { Component } from 'react'

export default class Location extends Component {
    constructor(props) {
        super(props)
        this.title = 'Location'
        this.state = { }
    }
    render() {
        return (
            <div>
                {this.title}
            </div>
        )
    }
}