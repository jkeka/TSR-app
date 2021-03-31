import React, { Component } from 'react'
import { Button, Form, Table, Container } from 'react-bootstrap'
import firebase from '../services/firebase'

export default class User extends Component {
  constructor(props) {
    super(props)
    this.ref = firebase.database().ref("Users")
    
    this.handleChange = this.handleChange.bind(this)
    this.handleSubmit = this.handleSubmit.bind(this)
    this.removeClaimedReward = this.removeClaimedReward.bind(this)
    this.modifyClicked = this.modifyClicked.bind(this)
    this.addRewardKey = this.addRewardKey.bind(this)
    this.addClaimedRewardKey = this.addClaimedRewardKey.bind(this)
    this.state = {
      deviceCode: '',
      language: '',
      visitedLocations: [],
      tempDB: [],
      deviceInputDisabled: false,
      submitOrUpdate: 'Submit'
    }
  }
  componentDidMount() {
    // Database fetch
    this.ref.on('value', (snapshot) => {
      console.log(snapshot.val())
      if (snapshot.val() !== null) {
        this.setState({tempDB: snapshot.val()})
      } else {
        console.log('fetch failed')
      }
    })
  }
  addRewardKey() {
  
  }
  addClaimedRewardKey() {

  }
  modifyClicked(key, language) {
    console.log(language)
    this.setState({deviceCode: key, 
      language: language, 
      deviceInputDisabled: true,
      submitOrUpdate: 'Update'
    })
  }
  handleChange(event, index) {
    switch (event.target.name) {
      case 'deviceCode':
        this.setState({deviceCode: event.target.value})
        break;
      case 'language':
        this.setState({language: event.target.value})
        break;
      case 'rewardKey':
        let tmp = [...this.state.rewardKey]
        tmp[index] = event.target.value
        this.setState({rewardKey: tmp})
        break;
      case 'claimedRewardKey':
        let tmpClaimedReward = [...this.state.claimedRewardKey]
        tmpClaimedReward[index] = event.target.value
        this.setState({claimedRewardKey: tmpClaimedReward})
        break;
      default:
        console.log('error with switch')
    }
  }
  handleSubmit(event) {
    const testObj = { deviceCode: this.state.deviceCode, language: this.state.language }
    const ref = this.ref.child(this.state.deviceCode)
    ref.set(testObj)
    this.setState({deviceInputDisabled: false, submitOrUpdate: 'Submit'})
    
    event.preventDefault() 
  }
  removeDevice(device) {
    console.log(device)
    const ref = this.ref.child(device)
    ref.remove()
  }
  removeReward(index) {

  }
  removeClaimedReward(key) {

  }
  render() {
    const resultTable = Object.entries(this.state.tempDB).map(([key, value], index) => {
      const locList = value.visitedLocations.map(loc => <li key={loc}>{loc}</li>)
      return (
        <tr key={index}>
          <td>{key}</td>
          <td>{value.language}</td>
          <td>
            <ul>
              {locList}
            </ul>
          </td>
          <td>
            {/*<Button onClick={() => this.modifyClicked(key, value.language)}>Modify</Button>*/}
            <Button variant="danger" onClick={() => this.removeDevice(key)}>Remove</Button>
          </td>
        </tr>
      )
    })
    return (
      <Container>
      {/* NOT NEEDED FORM FOR ADDING USERS
      <Form onSubmit={this.handleSubmit}>

        <Form.Group>
          <Form.Label htmlFor="device">deviceCode:</Form.Label>
          <Form.Control type="text" name="deviceCode" value={this.state.deviceCode}
            onChange={this.handleChange} disabled={this.state.deviceInputDisabled ? "disabled" : ""} />
        </Form.Group>

        <Form.Group>
          <Form.Label htmlFor="language">language:</Form.Label>
          <Form.Control type="text" name="language" value={this.state.language}
            onChange={this.handleChange} />
        </Form.Group>
        
        <Form.Group>
          <Form.Label htmlFor="rewardKey">rewardKey:</Form.Label>
          
          <Button onClick={() => this.addRewardKey()}>+</Button><br/>
        </Form.Group>
        
        <Form.Group>
          <Button variant="primary" type="submit">{this.state.submitOrUpdate}</Button>
        </Form.Group>
      </Form>
      */}
    
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>device</th>
            <th>lang</th>
            <th>visitedLocations</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {resultTable}
        </tbody>
      </Table>
    </Container>
    )
  }
}