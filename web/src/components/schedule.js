import React, { Component } from 'react'
import langSrv from '../services/language'

export default class Schedule extends Component {
    constructor(props) {
        super(props)
        this.title = 'Schedule'
        this.state = { languages: ['testi'] }
    }
    componentDidMount() {
      this.setState({languages: langSrv.getLanguages()})
    }
    render() {
        return (
            <div>
                {this.title}<br/>
                {this.state.languages}
            </div>
        )
    }
}