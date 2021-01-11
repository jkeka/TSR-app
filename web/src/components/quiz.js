import React, { Component } from 'react'

export default class Quiz extends Component {
    constructor(props) {
        super(props)
        this.title = 'Quiz'
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