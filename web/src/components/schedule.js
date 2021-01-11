import React, { Component } from 'react'

export default class Schedule extends Component {
    constructor(props) {
        super(props)
        this.title = 'Schedule'
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