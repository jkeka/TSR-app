import React, { Component } from 'react'

export default class Home extends Component {
    constructor(props) {
        super(props)
        this.title = 'Home'
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