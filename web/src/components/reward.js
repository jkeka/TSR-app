import React, { Component } from 'react'

export default class Reward extends Component {
    constructor(props) {
        super(props)
        this.title = 'Reward'
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